using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UFArt.Infrastructure;

namespace UFArt.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Home()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SetLanguageToPl(string returnUri)
        {
            Request.HttpContext.Session.SetString("language", "pl");
            return Redirect(returnUri);
        }

        public IActionResult SetLanguageToEn(string returnUri)
        {
            Request.HttpContext.Session.SetString("language", "en");
            return Redirect(returnUri);
        }
    }
}