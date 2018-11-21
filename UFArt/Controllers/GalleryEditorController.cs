using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using UFArt.Infrastructure;
using UFArt.Models;
using UFArt.Models.Configuration;
using UFArt.Models.Gallery;
using UFArt.Models.Newsfeed;
using UFArt.Models.TextAssets;
using UFArt.Models.ViewModels;

namespace UFArt.Controllers
{
    [Authorize(Roles = "editor")]
    public class GalleryEditorController : Controller
    {
        private StorageFacade _storageFacade;
        private IGalleryRepository _galleryRepo;
        private readonly ITechniqueRepository _techniqueRepo;
        private readonly ITextAssetsRepository _textRepo;
        private readonly IOptions<StorageSettings> _storageSettings;
        private ArtPieceFactory _factory;

        public GalleryEditorController(IOptions<StorageSettings> storageSettings, IGalleryRepository repo,
            ITextAssetsRepository textRepo, ITechniqueRepository techniqueRepo)
        {
            _galleryRepo = repo;
            _techniqueRepo = techniqueRepo;
            _storageSettings = storageSettings;
            _textRepo = textRepo;
            _storageFacade = new StorageFacade(storageSettings);
            _factory = new ArtPieceFactory(_galleryRepo, _textRepo, _techniqueRepo);
        }

        public IActionResult AddGalleryElement() => View(new ArtPieceCreationViewModel(_techniqueRepo, _textRepo));

        public IActionResult AddTechnique() => View(new TechniqueAddViewModel(_textRepo));

        public IActionResult ManageGallery() => View(new GalleryViewModel(_textRepo, _galleryRepo));

        public IActionResult ManageTechniques() => View(new TechniquesViewModel(_textRepo, _techniqueRepo.Techniques));

        public IActionResult UpdateGalleryElement(int id)
        {
            var viewModel = new ArtPieceCreationViewModel(_techniqueRepo, _textRepo);
            var artPiece = _galleryRepo.ArtPieces.Where(ap => ap.ID == id).FirstOrDefault();
            if (artPiece != null) viewModel = _factory.CreateViewModel(artPiece, Request.HttpContext);
            return View("AddGalleryElement", viewModel);
        }

        public IActionResult UpdateTechnique(int id)
        {
            var technique = _techniqueRepo.Techniques.Where(t => t.ID == id).FirstOrDefault();
            var viewModel = new TechniqueAddViewModel(_textRepo)
            {
                NamePl = technique.Name.Value_pl,
                NameEn = technique.Name.Value_en
            };
            return RedirectToAction("AddTechnique", technique);
        }

        [HttpPost]
        public IActionResult AddTechnique(TechniqueAddViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var asset = new TextAsset() { Key = "technique_value" };
                asset.Value_pl = viewModel.NamePl;
                asset.Value_en = viewModel.NameEn;
                _textRepo.SaveAsset(asset);
                var technique = new Technique() { Name = asset };
                _techniqueRepo.Save(technique);
                var queryParams = new Dictionary<string, string>()
                {
                    { "messageKey", "success_gallery_element_added" },
                    { "returnUri", "/GalleryEditor/ManageTechniques" }
                };
                return Redirect(QueryHelpers.AddQueryString("/InformationScreens/Success", queryParams));
            }
            else
            {
                viewModel.TextRepository = _textRepo;
                return View("AddTechnique", viewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadAsync(ArtPieceCreationViewModel viewModel)
        {
            try
            {
                var file = Request.Form.Files.FirstOrDefault();
                if (file == null && viewModel.ImageUri == null) ModelState.AddModelError("FileNotSelected", "Plik ze zdjęciem nie został wybrany");

                if (ModelState.IsValid)
                {
                    if (file != null)
                    {
                        if (viewModel.ImageUri != null)
                            await _storageFacade.DeleteImageBlob(viewModel.ImageUri);
                        viewModel.ImageUri = await _storageFacade.UploadImageBlob(file);
                    }
                    if (viewModel.Id == 0)
                    {
                        var artPiece = _factory.CreateArtPiece(viewModel, Request.HttpContext);
                        _galleryRepo.Save(artPiece);
                        var queryParams = new Dictionary<string, string>()
                        {
                            { "messageKey", "success_gallery_element_added" },
                            { "returnUri", "/GalleryEditor/AddGalleryElement" }
                        };
                        return Redirect(QueryHelpers.AddQueryString("/InformationScreens/Success", queryParams));
                    }
                    else
                    {
                        var artPiece = _factory.UpdateArtPiece(viewModel, Request.HttpContext);
                        _galleryRepo.Update(artPiece);
                        var queryParams = new Dictionary<string, string>()
                        {
                            { "messageKey", "success_gallery_element_modified" },
                            { "returnUri", "/GalleryEditor/AddGalleryElement" }
                        };
                        return Redirect(QueryHelpers.AddQueryString("/InformationScreens/Success", queryParams));
                    }
                }
                else
                {
                    viewModel.TechniqueRepository = _techniqueRepo;
                    return View("AddGalleryElement", viewModel);
                }
            }
            catch (Exception ex)
            {
                ViewData["message"] = ex.Message;
                ViewData["trace"] = ex.StackTrace;
                return View("Error");
            }
        }

        public IActionResult DeleteGalleryElement(int id)
        {
            _galleryRepo.Delete(id);
            return RedirectToAction("ManageGallery");
        }

        public IActionResult DeleteTechnique(int id)
        {
            _techniqueRepo.Delete(id);
            return RedirectToAction("ManageTechniques");
        }
    }
}