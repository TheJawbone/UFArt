using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UFArt.Infrastructure.Mailing;
using UFArt.Models;
using UFArt.Models.Gallery;
using UFArt.Models.Identity;
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
        private UserManager<User> _userManager;

        public int PageSize = 9;

        public GalleryController(IGalleryRepository galleryRepo, ITechniqueRepository techniqueRepo, UserManager<User> userManager,
            IEmailConfiguration emailConfiguration, IEmailService emailService, ITextAssetsRepository textRepository)
        {
            _galleryRepo = galleryRepo;
            _techniqueRepo = techniqueRepo;
            _emailConfiguration = emailConfiguration;
            _emailService = emailService;
            _textRepository = textRepository;
            _userManager = userManager;
        }

        public IActionResult DispatchTechniqueListing(int techniqueNameId, int pageNumber = 1)
        {
            int techniqueId = _techniqueRepo.Techniques.Where(t => t.Name.Id == techniqueNameId).FirstOrDefault().ID;
            return GenerateResultView(techniqueId, pageNumber);
        }

        private IActionResult GenerateResultView(int techniqueId, int pageNumber = 1)
        {
            var elements = _galleryRepo.ArtPieces
                    .Where(ap => ap.Technique.ID == techniqueId)
                    .OrderBy(ap => ap.Name)
                    .Skip((pageNumber - 1) * PageSize)
                    .Take(PageSize);
            var pagingInfo = new PagingInfo()
            {
                CurrentPage = pageNumber,
                ItemsPerPage = PageSize,
                TotalItems = _galleryRepo.ArtPieces
                    .Where(p => p.Technique == _techniqueRepo.Techniques.Where(t => t.ID == techniqueId).FirstOrDefault())
                    .Count()
            };
            var viewModel = new GalleryElementsViewModel(_textRepository)
            {
                Elements = elements,
                PagingInfo = pagingInfo
            };
            return View("List", viewModel);
        }

        public async Task<IActionResult> MakeOffer(int id)
        {
            var userId = Request.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);
            return View(new OfferViewModel(_textRepository) { ArtPieceId = id, User = user });
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

        public IActionResult Details(int id, string returnUrl)
        {
            var artPiece = _galleryRepo.ArtPieces.Where(ap => ap.ID == id).FirstOrDefault();
            if (artPiece != null) return View(new GalleryElementDetailsViewModel(_textRepository, artPiece, returnUrl, Request.HttpContext));
            else return View("Error");
        }
    }
}