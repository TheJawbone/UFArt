using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UFArt.Controllers
{
    public class NavbarController : Controller
    {
        public IActionResult Navbar()
        {
            return View("Navbar", "Hello");
        }
    }
}