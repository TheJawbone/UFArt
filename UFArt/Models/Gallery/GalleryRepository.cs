using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.Configuration;

namespace UFArt.Models.Gallery
{
    public class GalleryRepository : IGalleryRepository
    {
        static CloudBlobClient blobClient;
        static CloudBlobContainer blobContainer;
        private ApplicationDbContext _context;
        private IOptions<StorageSettings> _storageSettings;
        const string blobContainerName = "webappstoragedotnet-imagecontainer";

        public IQueryable<ArtPiece> ArtPieces => _context.ArtPieces
            .Include(ap => ap.AdditionalInfo)
            .Include(ap => ap.Description)
            .Include(ap => ap.Name)
            .Include(ap => ap.Technique)
            .ThenInclude(t => t.Name);

        public GalleryRepository(ApplicationDbContext context, IOptions<StorageSettings> storageSettings)
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

        public bool Save(ArtPiece artPiece)
        {
            try
            {
                _context.ArtPieces.Add(artPiece);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                if (ex is DbUpdateException || ex is DbUpdateConcurrencyException)
                {
                    return false;
                }
                throw;
            }
        }

        public bool Update(ArtPiece artPiece)
        {
            try
            {
                _context.ArtPieces.Update(artPiece);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                if (ex is DbUpdateException || ex is DbUpdateConcurrencyException)
                {
                    return false;
                }
                throw;
            }
        }

        public async Task<bool> Delete(int id)
        {
            ArtPiece artPiece = _context.ArtPieces.Where(p => p.ID == id).FirstOrDefault();
            if (artPiece != null)
            {
                Uri uri = new Uri(artPiece.ImageUri);
                string filename = Path.GetFileName(uri.LocalPath);

                try
                {
                    _context.ArtPieces.Remove(artPiece);
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

                var blob = blobContainer.GetBlockBlobReference(filename);
                await blob.DeleteIfExistsAsync();
                return true;
            }
            return false;
        }
    }
}
