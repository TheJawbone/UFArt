﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UFArt.Infrastructure.Mailing;
using UFArt.Models;
using UFArt.Models.Gallery;
using UFArt.Models.TextAssets;
using UFArt.Models.ViewModels;

namespace UFArt.Controllers
{
    public class GalleryController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly IGalleryRepository _galleryRepo;
        private readonly ITechniqueRepository _techniqueRepo;
        private readonly ITextAssetsRepository _textRepository;
        private readonly IEmailConfiguration _emailConfiguration;

        public int PageSize = 9;

        public GalleryController(IGalleryRepository galleryRepo, ITechniqueRepository techniqueRepo,
            IEmailConfiguration emailConfiguration, IEmailService emailService, ITextAssetsRepository textRepository)
        {
            _galleryRepo = galleryRepo;
            _techniqueRepo = techniqueRepo;
            _emailConfiguration = emailConfiguration;
            _emailService = emailService;
            _textRepository = textRepository;
        }

        public IActionResult ListOilPaintings(int pageNumber = 1) =>
            GenerateResultView("OP", pageNumber);

        public IActionResult ListWatercolorPaintings(int pageNumber = 1) =>
            GenerateResultView("WP", pageNumber);

        public IActionResult ListSketches(int pageNumber = 1) =>
            GenerateResultView("SK", pageNumber);

        public IActionResult ListPottery(int pageNumber = 1) =>
            GenerateResultView("PO", pageNumber);

        public IActionResult MakeOffer(int id)
        {
            var test = id;
            return View(new OfferViewModel(_textRepository) { ArtPieceId = id });
        }

        [HttpPost]
        public IActionResult SendOffer(OfferViewModel offer)
        {
            if (ModelState.IsValid)
            {
                var message = new EmailMessageFactory(_emailConfiguration).CreateOfferMessage(offer);
                var result = _emailService.Send(message);
                if (result.Succeeded)
                {
                    message = new EmailMessageFactory(_emailConfiguration).CreateOfferConfirmationMessage(offer);
                    result = _emailService.Send(message);
                    if(result.Succeeded)
                        return View("Success", new string[] { "Pomyślnie wysłano ofertę", "/" });
                    else return View("Error");
                }
                else return View("Error");


            }
            else return View("MakeOffer", offer);
        }

        private IActionResult GenerateResultView(string techniqueCodeName, int pageNumber = 1)
        {
            var techniqueName = _techniqueRepo.Techniques.Where(t => t.CodeName == techniqueCodeName).FirstOrDefault().Name;
            return View("List", new GalleryElementsViewModel(_textRepository)
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
                    TotalItems = _galleryRepo.ArtPieces
                    .Where(p => p.Technique == _techniqueRepo.Techniques
                        .Where(t => t.CodeName == techniqueCodeName).FirstOrDefault().Name)
                    .Count()
                }
            });
        }

        public IActionResult Details(int id, string returnUrl)
        {
            var artPiece = _galleryRepo.ArtPieces.Where(ap => ap.ID == id).FirstOrDefault();
            if (artPiece != null) return View(new GalleryElementDetailsViewModel(_textRepository, artPiece, returnUrl, Request.HttpContext));
            else return View("Error");
        }
    }
}