using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UFArt.Models.Configuration;

namespace UFArt.Models
{
    public class StorageFacade
    {
        private static CloudBlobClient _imageBlobClient;
        private static CloudBlobContainer _imageBlobContainer;
        private readonly IOptions<StorageSettings> _storageSettings;
        private const string _imageBlobContainerName = "webappstoragedotnet-imagecontainer";

        public StorageFacade(IOptions<StorageSettings> storageSettings)
        {
            _storageSettings = storageSettings;
            ConfigureStorage();
        }

        private async void ConfigureStorage()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_storageSettings.Value.DevConnectionString);
            _imageBlobClient = storageAccount.CreateCloudBlobClient();
            _imageBlobContainer = _imageBlobClient.GetContainerReference(_imageBlobContainerName);
            await _imageBlobContainer.CreateIfNotExistsAsync();
        }

        private string GetRandomBlobName(string filename)
        {
            string ext = Path.GetExtension(filename);
            return string.Format("{0:10}_{1}{2}", DateTime.Now.Ticks, Guid.NewGuid(), ext);
        }

        public async Task<string> UploadImageBlob(IFormFile file)
        {
            CloudBlockBlob blob = _imageBlobContainer.GetBlockBlobReference(GetRandomBlobName(file.Name));
            await blob.UploadFromStreamAsync(file.OpenReadStream());
            return blob.Uri.ToString();
        }
    }
}
