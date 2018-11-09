using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UFArt.Models.TextAssets;

namespace UFArt.Controllers
{
    public class AboutController : Controller
    {
        private readonly ITextAssetsRepository _repo;

        public AboutController(ITextAssetsRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            return View("Index", _repo.GetTranslatedValue("about_header", Request.HttpContext));
        }
    }
}