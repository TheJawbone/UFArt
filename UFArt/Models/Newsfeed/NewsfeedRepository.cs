using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.Configuration;
using UFArt.Models.TextAssets;

namespace UFArt.Models.Newsfeed
{
    public class NewsfeedRepository : INewsfeedRepository
    {
        private static CloudBlobClient blobClient;
        private static CloudBlobContainer blobContainer;
        private ApplicationDbContext _context;
        private IOptions<StorageSettings> _storageSettings;
        private const string blobContainerName = "webappstoragedotnet-imagecontainer";

        public IQueryable<News> News => _context.News.Include(n => n.Header).Include(n => n.Text)
            .OrderByDescending(n => n.Timestamp);

        public NewsfeedRepository(ApplicationDbContext context, IOptions<StorageSettings> storageSettings)
        {
            _context = context;
            _storageSettings = storageSettings;
            ConfigureStorage();
        }

        private async void ConfigureStorage()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_storageSettings.Value.StorageConnectionString);
            blobClient = storageAccount.CreateCloudBlobClient();
            blobContainer = blobClient.GetContainerReference(blobContainerName);
            await blobContainer.CreateIfNotExistsAsync();
        }

        public bool Save(News news)
        {
            try
            {
                _context.News.Add(news);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                if (ex is DbUpdateException || ex is DbUpdateConcurrencyException) return false;
                throw;
            }
        }

        public bool Update(News news)
        {
            try
            {
                _context.News.Update(news);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                if (ex is DbUpdateException || ex is DbUpdateConcurrencyException) return false;
                throw;
            }
        }

        public async Task<bool> Delete(int id)
        {
            News news = await _context.News.Where(n => n.ID == id).Include(n => n.Header).Include(n => n.Text).FirstOrDefaultAsync();
            if (news != null)
            {
                Uri uri = new Uri(news.ImageUrl);
                string filename = Path.GetFileName(uri.LocalPath);
                var blob = blobContainer.GetBlockBlobReference(filename);
                await blob.DeleteIfExistsAsync();

                try
                {
                    var headerId = news.Header.Id;
                    var textId = news.Text.Id;
                    _context.News.Remove(news);
                    _context.TextAssets.Remove(_context.TextAssets.Where(ta => ta.Id == headerId).FirstOrDefault());
                    _context.TextAssets.Remove(_context.TextAssets.Where(ta => ta.Id == textId).FirstOrDefault());
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    if (ex is DbUpdateException || ex is DbUpdateConcurrencyException)
                    {
                        return false;
                    }
                    throw;
                }

                return true;
            }
            return false;
        }
    }
}
