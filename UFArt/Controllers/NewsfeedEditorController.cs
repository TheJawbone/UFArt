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

        public IActionResult Index()
        {
            return View();
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
                    _repo.Save(news);
                    return View("Success", new string[] { "Wpis do aktualności został dodany", "/NewsfeedEditor" });
                }
                catch (Exception ex)
                {
                    ViewData["message"] = ex.Message;
                    ViewData["trace"] = ex.StackTrace;
                    return View("Error");
                }
            }
            else return View("Index");
        }
    }
}