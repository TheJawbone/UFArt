using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UFArt.Models;
using UFArt.Models.ViewModels;

namespace UFArt.Controllers
{
    public class GalleryController : Controller
    {
        private IDataRepository _repository;
        public int PageSize = 2;

        public GalleryController(IDataRepository repository)
        {
            _repository = repository;
        }

        public IActionResult ListOilPaintings(int pageNumber = 1)
        {
            return View(new PaintingsViewModel
            {
                Elements = _repository.OilPaintings
                    .Where(painting => painting.Technique.Name == "Olej na płótnie")
                    .OrderBy(painting => painting.Name)
                    .Skip((pageNumber - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = pageNumber,
                    ItemsPerPage = PageSize,
                    TotalItems = _repository.OilPaintings.Count()
                }
            });
        }

        public IActionResult ListPottery(int pageNumber = 1)
        {
            return View(new PotteryViewModel
            {
                Elements = _repository.Potteries
                    .OrderBy(pottery => pottery.Name)
                    .Skip((pageNumber - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = pageNumber,
                    ItemsPerPage = PageSize,
                    TotalItems = _repository.OilPaintings.Count()
                }
            });
        }
    }
}