using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using UFArt.Models;
using UFArt.Models.Configuration;
using UFArt.Models.Newsfeed;
using UFArt.Models.TextAssets;
using UFArt.Models.ViewModels;

namespace UFArt.Controllers
{
    [Authorize(Roles = "editor")]
    public class NewsfeedEditorController : Controller
    {
        private readonly INewsfeedRepository _repo;
        private readonly ITextAssetsRepository _textRepo;
        private StorageFacade _storageFacade;

        public NewsfeedEditorController(IOptions<StorageSettings> options, ITextAssetsRepository textRepository,
            INewsfeedRepository repo)
        {
            _repo = repo;
            _textRepo = textRepository;
            _storageFacade = new StorageFacade(options);
        }

        public IActionResult AddNews() => View(new NewsAddViewModel(_textRepo));

        public IActionResult ManageNews() => View(new NewsManageViewModel(_repo, _textRepo));

        public IActionResult UpdateNews(int id, string language = "pl", bool success = false)
        {
            var news = _repo.News.Where(n => n.ID == id).FirstOrDefault();
            if (news != null)
            {
                var viewModel = new NewsAddViewModel(news, language, _textRepo) { Language = language, SuccessFlag = success };
                return View("AddNews", viewModel);
            }
            else return View("AddNews", new NewsAddViewModel(_textRepo) { Language = language, SuccessFlag = success });
        }

        [HttpPost]
        public async Task<ActionResult> UploadAsync(NewsAddViewModel viewModel)
        {
            try
            {
                var file = Request.Form.Files.FirstOrDefault();
                if (file == null && viewModel.ImageUri == null) ModelState.AddModelError("FileNotSelected", "Plik ze zdjęciem nie został wybrany");

                if (ModelState.IsValid)
                {

                    if (file != null) viewModel.ImageUri = await _storageFacade.UploadImageBlob(file);
                    var news = _repo.News.Where(n => n.ID == viewModel.ID).FirstOrDefault();
                    if (news == null)
                    {
                        news = new News()
                        {
                            ID = viewModel.ID,
                            ImageUrl = viewModel.ImageUri,
                            Timestamp = viewModel.Timestamp
                        };

                        var headerAsset = new TextAsset() { Key = "news_piece_header" };
                        var textAsset = new TextAsset() { Key = "news_piece_text" };
                        news.Header = headerAsset;
                        news.Text = textAsset;
                        switch (viewModel.Language)
                        {
                            case "pl":
                                news.Header.Value_pl = viewModel.Header;
                                news.Text.Value_pl = viewModel.Text;
                                break;
                            case "en":
                                news.Header.Value_en = viewModel.Header;
                                news.Text.Value_en = viewModel.Text;
                                break;
                        }
                        _textRepo.SaveAsset(headerAsset);
                        _textRepo.SaveAsset(textAsset);
                        _repo.Save(news);
                        return RedirectToAction("UpdateNews", new
                        {
                            id = news.ID,
                            language = viewModel.Language,
                            success = true
                        });
                    }
                    else
                    {
                        news.ImageUrl = viewModel.ImageUri;
                        switch (viewModel.Language)
                        {
                            case "pl":
                                news.Header.Value_pl = viewModel.Header;
                                news.Text.Value_pl = viewModel.Text;
                                break;
                            case "en":
                                news.Header.Value_en = viewModel.Header;
                                news.Text.Value_en = viewModel.Text;
                                break;
                        }
                        _repo.Update(news);
                        return RedirectToAction("UpdateNews", new
                        {
                            id = news.ID,
                            language = viewModel.Language,
                            success = true
                        });
                    }
                }
                else return View("AddNews", viewModel);
            }
            catch (Exception ex)
            {
                ViewData["message"] = ex.Message;
                ViewData["trace"] = ex.StackTrace;
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOlderThan(int numberOfDays)
        {
            IQueryable<News> newsToDelete;
            if(numberOfDays < 0) newsToDelete = _repo.News;
            else
            {
                DateTime deleteTreshold = DateTime.Now.AddDays(-numberOfDays);
                newsToDelete = _repo.News.Where(n => n.Timestamp < deleteTreshold);
            }

            foreach (var news in newsToDelete)
            {
                await _repo.Delete(news.ID);
            }

            return RedirectToAction("ManageNews");
        }

        public async Task<IActionResult> DeleteNews(int id)
        {
            await _repo.Delete(id);
            return RedirectToAction("ManageNews");
        }

        public IActionResult ChangeLanguageToPl(int id)
        {
            return RedirectToAction("UpdateNews", new { id, language = "pl" });
        }

        public IActionResult ChangeLanguageToEn(int id)
        {
            return RedirectToAction("UpdateNews", new { id, language = "en" });
        }
    }
}