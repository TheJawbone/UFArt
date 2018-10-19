using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Extensions.Options;
using UFArt.Models.Configuration;
using UFArt.Models;
using UFArt.Models.Gallery;

namespace UFArt.Controllers
{
    public class GalleryEditorOldController : Controller
    {
        private static CloudBlobClient blobClient;
        private const string blobContainerName = "webappstoragedotnet-imagecontainer";
        private static CloudBlobContainer blobContainer;
        private readonly IOptions<StorageSettings> _storageSettings;
        private IGalleryRepositories _repos;

        public GalleryEditorOldController(IOptions<StorageSettings> storageSettings, IGalleryRepositories repos)
        {
            _storageSettings = storageSettings;
            _repos = repos;
        }

        public IActionResult Index()
        {
            return View(_repos.OilPaintingsRepository.OilPaintings.FirstOrDefault());
        }

        [HttpPost]
        public async Task<ActionResult> UploadAsync(Painting painting)
        {
            try
            {
                var file = Request.Form.Files.FirstOrDefault();
                if (file != null)
                {
                    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_storageSettings.Value.DevConnectionString);
                    blobClient = storageAccount.CreateCloudBlobClient();
                    blobContainer = blobClient.GetContainerReference(blobContainerName);
                    CloudBlockBlob blob = blobContainer.GetBlockBlobReference(GetRandomBlobName(file.FileName));
                    await blob.UploadFromStreamAsync(file.OpenReadStream());

                    painting.ImageUrl = blob.Uri.ToString();
                    painting.Technique = new TechniqueDict { Name = "Olej na płótnie" };
                    _repos.OilPaintingsRepository.Save(painting);

                    return RedirectToAction("Index");
                }
                else return View("Error");
            }
            catch (Exception ex)
            {
                ViewData["message"] = ex.Message;
                ViewData["trace"] = ex.StackTrace;
                return View("Error");
            }
        }

        private string GetRandomBlobName(string filename)
        {
            string ext = Path.GetExtension(filename);
            return string.Format("{0:10}_{1}{2}", DateTime.Now.Ticks, Guid.NewGuid(), ext);
        }
    }
}