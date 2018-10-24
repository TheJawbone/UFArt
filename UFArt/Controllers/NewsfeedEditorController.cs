using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UFArt.Models;
using UFArt.Models.Configuration;
using UFArt.Models.Newsfeed;

namespace UFArt.Controllers
{
    public class NewsfeedEditorController : Controller
    {
        private INewsfeedRepository _repo;
        private StorageFacade _storageFacade;

        public NewsfeedEditorController(IOptions<StorageSettings> options, INewsfeedRepository repo)
        {
            _repo = repo;
            _storageFacade = new StorageFacade(options);
        }

        public IActionResult AddNews() => View();

        public IActionResult ManageNews() => View(_repo);

        public IActionResult UpdateNews(int id)
        {
            var news = _repo.News.Where(n => n.ID == id).FirstOrDefault();
            if (news != null) return View("AddNews", news);
            else return View("Error");
        }

        [HttpPost]
        public async Task<ActionResult> UploadAsync(News news)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var file = Request.Form.Files.FirstOrDefault();
                    if (file != null) news.ImageUrl = await _storageFacade.UploadImageBlob(file);
                    if (news.ID == 0)
                    {
                        _repo.Save(news);
                        return View("Success", new string[] { "Wpis aktualności został dodany", "/NewsfeedEditor/AddNews" });
                    }
                    else
                    {
                        _repo.Update(news);
                        return View("Success", new string[] { "Wpis aktualności został zaktualizowany", "/NewsfeedEditor/ManageNews" });
                    }
                }
                catch (Exception ex)
                {
                    ViewData["message"] = ex.Message;
                    ViewData["trace"] = ex.StackTrace;
                    return View("Error");
                }
            }
            else return View("AddNews");
        }

        public async Task<IActionResult> DeleteNews(int id)
        {
            await _repo.Delete(id);
            return RedirectToAction("ManageNews");
        }
    }
}