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

namespace UFArt.Models.Newsfeed
{
    public class NewsfeedRepository : INewsfeedRepository
    {
        static CloudBlobClient blobClient;
        static CloudBlobContainer blobContainer;
        private ApplicationDbContext _context;
        private IOptions<StorageSettings> _storageSettings;
        const string blobContainerName = "webappstoragedotnet-imagecontainer";

        public IQueryable<News> News => _context.News.OrderByDescending(n => n.Timestamp);

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
            catch(Exception ex)
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
            News news = await _context.News.Where(n => n.ID == id).FirstOrDefaultAsync();
            if (news != null)
            {
                if(news.ImageUrl != null)
                {
                    Uri uri = new Uri(news.ImageUrl);
                    string filename = Path.GetFileName(uri.LocalPath);
                    var blob = blobContainer.GetBlockBlobReference(filename);
                    await blob.DeleteIfExistsAsync();
                }

                try
                {
                    _context.News.Remove(news);
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
