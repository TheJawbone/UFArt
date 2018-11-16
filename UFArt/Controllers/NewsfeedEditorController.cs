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
        private readonly ITextAssetsRepository _textRepository;
        private StorageFacade _storageFacade;

        public NewsfeedEditorController(IOptions<StorageSettings> options, ITextAssetsRepository textRepository, INewsfeedRepository repo)
        {
            _repo = repo;
            _textRepository = textRepository;
            _storageFacade = new StorageFacade(options);
        }

        public IActionResult AddNews() => View(new NewsAddViewModel(_textRepository));

        public IActionResult ManageNews() => View(new NewsManageViewModel(_repo, _textRepository));

        public IActionResult UpdateNews(int id)
        {
            var news = _repo.News.Where(n => n.ID == id).FirstOrDefault();
            if (news != null)
            {
                var viewModel = new NewsAddViewModel(news, Request.HttpContext, _textRepository);
                return View("AddNews", viewModel);
            }
            else return View("Error");
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
                        switch (Request.HttpContext.Session.GetString("language"))
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
                        _textRepository.SaveAsset(headerAsset);
                        _textRepository.SaveAsset(textAsset);
                        _repo.Save(news);
                        var queryParams = new Dictionary<string, string>()
                        {
                            { "messageKey", "success_news_added" },
                            { "returnUri", "/NewsfeedEditor/AddNews" }
                        };
                        return Redirect(QueryHelpers.AddQueryString("/InformationScreens/Success", queryParams));
                    }
                    else
                    {
                        switch (Request.HttpContext.Session.GetString("language"))
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
                        var queryParams = new Dictionary<string, string>()
                        {
                            { "messageKey", "success_news_modified" },
                            { "returnUri", "/NewsfeedEditor/ManageNews" }
                        };
                        return Redirect(QueryHelpers.AddQueryString("/InformationScreens/Success", queryParams));
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

        public async Task<IActionResult> DeleteNews(int id)
        {
            await _repo.Delete(id);
            return RedirectToAction("ManageNews");
        }
    }
}