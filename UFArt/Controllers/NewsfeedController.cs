using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UFArt.Models;
using UFArt.Models.Newsfeed;

namespace UFArt.Controllers
{
    public class NewsfeedController : Controller
    {
        private INewsfeedRepository _repo;

        public NewsfeedController(INewsfeedRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            return View(_repo.News);
        }
    }
}