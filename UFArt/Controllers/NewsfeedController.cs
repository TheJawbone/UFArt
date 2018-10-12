using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UFArt.Models;

namespace UFArt.Controllers
{
    public class NewsfeedController : Controller
    {
        private IDataRepository _repository;

        public NewsfeedController(IDataRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View(_repository.News);
        }
    }
}