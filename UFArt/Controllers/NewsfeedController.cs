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
        private readonly INewsfeedRepository _repo;

        public NewsfeedController(INewsfeedRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            return View(new NewsfeedViewModel(_repo));
        }

        [HttpPost]
        public IActionResult ShowMore(NewsfeedViewModel viewModel)
        {
            viewModel.Repo = _repo;
            if (viewModel.NewsDisplayed + viewModel.NewsIncrement > viewModel.Repo.News.Count())
                viewModel.NewsDisplayed = viewModel.Repo.News.Count();
            else viewModel.NewsDisplayed += viewModel.NewsIncrement;
            return View("Index", viewModel);
        }

        [HttpPost]
        public IActionResult ShowLess(NewsfeedViewModel viewModel)
        {
            viewModel.Repo = _repo;
            if (viewModel.NewsDisplayed < 2 * viewModel.NewsIncrement)
                viewModel.NewsDisplayed = viewModel.NewsIncrement;
            else viewModel.NewsDisplayed -= viewModel.NewsIncrement;
            return View("Index", viewModel);
        }
    }
}