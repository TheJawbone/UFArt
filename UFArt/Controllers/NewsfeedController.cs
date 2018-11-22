using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UFArt.Models;
using UFArt.Models.Newsfeed;
using UFArt.Models.TextAssets;

namespace UFArt.Controllers
{
    public class NewsfeedController : Controller
    {
        private readonly INewsfeedRepository _repo;
        private readonly ITextAssetsRepository _textRepository;

        public NewsfeedController(INewsfeedRepository repo, ITextAssetsRepository textRepository)
        {
            _repo = repo;
            _textRepository = textRepository;
        }

        public IActionResult Index(NewsfeedViewModel viewModel)
        {
            if (viewModel.NewsDisplayed == 0)
                viewModel = new NewsfeedViewModel(_textRepository, _repo);
            else
            {
                viewModel.Repo = _repo;
                viewModel.TextRepository = _textRepository;
            }
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult ShowMore(NewsfeedViewModel viewModel)
        {
            viewModel.Repo = _repo;
            viewModel.TextRepository = _textRepository;
            if (viewModel.NewsDisplayed + viewModel.NewsIncrement > viewModel.Repo.News.Count())
                viewModel.NewsDisplayed = viewModel.Repo.News.Count();
            else viewModel.NewsDisplayed += viewModel.NewsIncrement;
            return RedirectToAction("Index", viewModel);
        }

        [HttpPost]
        public IActionResult ShowLess(NewsfeedViewModel viewModel)
        {
            viewModel.Repo = _repo;
            viewModel.TextRepository = _textRepository;
            if (viewModel.NewsDisplayed < 2 * viewModel.NewsIncrement)
                viewModel.NewsDisplayed = viewModel.NewsIncrement;
            else viewModel.NewsDisplayed -= viewModel.NewsIncrement;
            return RedirectToAction("Index", viewModel);
        }
    }
}