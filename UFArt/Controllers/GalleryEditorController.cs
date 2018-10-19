using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using UFArt.Models;
using UFArt.Models.Configuration;
using UFArt.Models.Gallery;
using UFArt.Models.ViewModels;

namespace UFArt.Controllers
{
    public class GalleryEditorController : Controller
    {
        private static CloudBlobClient _blobClient;
        private const string _blobContainerName = "webappstoragedotnet-imagecontainer";
        private static CloudBlobContainer _blobContainer;
        private readonly IOptions<StorageSettings> _storageSettings;
        private IGalleryRepository _galleryRepo;
        private ITechniqueRepository _techniqueRepo;

        public GalleryEditorController(IOptions<StorageSettings> storageSettings, IGalleryRepository repo, ITechniqueRepository techniqueRepo)
        {
            _storageSettings = storageSettings;
            _galleryRepo = repo;
            _techniqueRepo = techniqueRepo;
            ConfigureStorage();
        }

        private async void ConfigureStorage()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_storageSettings.Value.DevConnectionString);
            _blobClient = storageAccount.CreateCloudBlobClient();
            _blobContainer = _blobClient.GetContainerReference(_blobContainerName);
            await _blobContainer.CreateIfNotExistsAsync();
        }

        public IActionResult Index()
        {
            return View(new ArtPieceCreationViewModel(_techniqueRepo));
        }

        [HttpPost]
        public async Task<ActionResult> UploadAsync(ArtPiece artPiece)
        {
            try
            {
                var file = Request.Form.Files.FirstOrDefault();
                if (file != null)
                {
                    CloudBlockBlob blob = _blobContainer.GetBlockBlobReference(GetRandomBlobName(file.FileName));
                    await blob.UploadFromStreamAsync(file.OpenReadStream());

                    artPiece.ImageUri = blob.Uri.ToString();
                    _galleryRepo.Save(artPiece);

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