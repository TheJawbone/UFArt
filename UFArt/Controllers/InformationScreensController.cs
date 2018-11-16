using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UFArt.Models.TextAssets;
using UFArt.Models.ViewModels;

namespace UFArt.Controllers
{
    public class InformationScreensController : Controller
    {
        private ITextAssetsRepository _textRepository;

        public InformationScreensController(ITextAssetsRepository textRepo)
        {
            _textRepository = textRepo;
        }

        public IActionResult Success()
        {
            return View(new ActionSuccessViewModel(_textRepository)
            {
                Message = _textRepository.GetAsset(Request.Query["messageKey"]),
                ReturnUri = Request.Query["returnUri"]
            });
        }
    }
}