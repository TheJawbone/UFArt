using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using UFArt.Infrastructure;
using UFArt.Models;
using UFArt.Models.Configuration;
using UFArt.Models.Gallery;
using UFArt.Models.Newsfeed;
using UFArt.Models.ViewModels;

namespace UFArt.Controllers
{
    public class GalleryEditorController : Controller
    {
        private StorageFacade _storageFacade;
        private IGalleryRepository _galleryRepo;
        private readonly ITechniqueRepository _techniqueRepo;
        private readonly IOptions<StorageSettings> _storageSettings;

        public GalleryEditorController(IOptions<StorageSettings> storageSettings, IGalleryRepository repo, ITechniqueRepository techniqueRepo)
        {
            _galleryRepo = repo;
            _techniqueRepo = techniqueRepo;
            _storageSettings = storageSettings;
            _storageFacade = new StorageFacade(storageSettings);
        }

        public IActionResult AddGalleryElement()
        {
            return View(new ArtPieceCreationViewModel(_techniqueRepo));
        }

        public IActionResult ManageGallery()
        {
            return View(_galleryRepo);
        }

        [HttpPost]
        public async Task<IActionResult> UploadAsync(ArtPieceCreationViewModel viewModel)
        {
            try
            {
                var file = Request.Form.Files.FirstOrDefault();
                if(file == null) ModelState.AddModelError("FileNotSelected", "Plik ze zdjęciem nie został wybrany");

                if (ModelState.IsValid)
                {
                    viewModel.ArtPiece.ImageUri = await _storageFacade.UploadImageBlob(file);
                    _galleryRepo.Save(viewModel.ArtPiece);
                    return View("Success", new string[] { "Element galerii został dodany", "/GalleryEditor/AddGalleryElement" });
                }
                //else return View("AddGalleryElement", new ArtPieceCreationViewModel(_techniqueRepo));
                //else return RedirectToAction("AddGalleryElement");
                else return View("AddGalleryElement");
            }
            catch (Exception ex)
            {
                ViewData["message"] = ex.Message;
                ViewData["trace"] = ex.StackTrace;
                return View("Error");
            }
        }
    }
}