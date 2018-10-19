using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UFArt.Models;
using UFArt.Models.Gallery;
using UFArt.Models.ViewModels;

namespace UFArt.Controllers
{
    public class GalleryController : Controller
    {
        private IGalleryRepository _galleryRepo;
        private ITechniqueRepository _techniqueRepo;
        public int PageSize = 9;

        public GalleryController(IGalleryRepository galleryRepo, ITechniqueRepository techniqueRepo)
        {
            _galleryRepo = galleryRepo;
            _techniqueRepo = techniqueRepo;
        }

        public IActionResult List(string techniqueCodeName, int pageNumber = 1)
        {
            var techniqueName = _techniqueRepo.Techniques.Where(t => t.CodeName == techniqueCodeName).FirstOrDefault().Name;
            return View(new GalleryElementsViewModel
            {
                Elements = _galleryRepo.ArtPieces
                    .Where(ap => ap.Technique == techniqueName)
                    .OrderBy(ap => ap.Name)
                    .Skip((pageNumber - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = pageNumber,
                    ItemsPerPage = PageSize,
                    TotalItems = _galleryRepo.ArtPieces.Count()
                }
            });
        }
    }
}