using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using UFArt.Models;
using UFArt.Models.About;
using UFArt.Models.Configuration;
using UFArt.Models.TextAssets;
using UFArt.Models.ViewModels;

namespace UFArt.Controllers
{
    public class AboutEditorController : Controller
    {
        private StorageFacade _storageFacade;
        private ITextAssetsRepository _textRepository;

        public AboutEditorController(IOptions<StorageSettings> storageSettings, ITextAssetsRepository textRepository)
        {
            _storageFacade = new StorageFacade(storageSettings);
            _textRepository = textRepository;
        }

        public IActionResult Index(string language = "pl", bool success = false)
        {
            var viewModel = new AboutViewModel(_textRepository) { Language = language, SuccessFlag = success };
            switch(language)
            {
                case "pl":
                    viewModel.Text = _textRepository.GetAsset("about_text").Value_pl;
                    break;
                case "en":
                    viewModel.Text = _textRepository.GetAsset("about_text").Value_en;
                    break;
            }
            viewModel.ImageUri = _textRepository.GetTranslatedValue("about_image_uri", Request.HttpContext);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UploadAsync(AboutViewModel viewModel)
        {
            try
            {
                viewModel.ImageUri = _textRepository.GetTranslatedValue("about_image_uri", Request.HttpContext);
                var file = Request.Form.Files.FirstOrDefault();
                if (file == null && viewModel.ImageUri == null) ModelState.AddModelError("FileNotSelected", "Plik ze zdjęciem nie został wybrany");

                if (ModelState.IsValid)
                {
                    if (file != null)
                    {
                        if (viewModel.ImageUri != null && file != null)
                            await _storageFacade.DeleteImageBlob(viewModel.ImageUri);
                        viewModel.ImageUri = await _storageFacade.UploadImageBlob(file);
                    }

                    var asset = _textRepository.GetAsset("about_image_uri");
                    asset.Value_pl = viewModel.ImageUri;
                    _textRepository.SaveAsset(asset);

                    asset = _textRepository.GetAsset("about_text");
                    switch (viewModel.Language)
                    {
                        case "pl":
                            asset.Value_pl = viewModel.Text;
                            break;
                        case "en":
                            asset.Value_en = viewModel.Text;
                            break;
                    }
                    _textRepository.SaveAsset(asset);

                    return RedirectToAction("Index", new { language = viewModel.Language, success = true });
                }
                else
                {
                    return View("Index", viewModel);
                }
            }
            catch (Exception ex)
            {
                ViewData["message"] = ex.Message;
                ViewData["trace"] = ex.StackTrace;
                return View("Error");
            }
        }

        public IActionResult ChangeLanguageToPl()
        {
            return RedirectToAction("Index", new { language = "pl" });
        }

        public IActionResult ChangeLanguageToEn()
        {
            return RedirectToAction("Index", new { language = "en" });
        }
    }
}