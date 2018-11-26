using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
            var technique = _techniqueRepo.Techniques.Where(t => t.ID == techniqueId).FirstOrDefault();
            var elements = _galleryRepo.ArtPieces
                    .Where(ap => ap.Technique.ID == techniqueId)
                    .OrderBy(ap => ap.Name)
                    .Skip((pageNumber - 1) * PageSize)
                    .Take(PageSize);
            var pagingInfo = new PagingInfo()
            {
                CurrentPage = pageNumber,
                ItemsPerPage = PageSize,
                TechniqueNameId = technique.Name.Id,
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

        [HttpPost]
        public IActionResult SendOffer(GalleryElementDetailsViewModel viewModel)
        {
                var message = new EmailMessageFactory(_emailConfiguration).CreateOfferMessage(viewModel);
                var result = _emailService.Send(message);
                if (result.Succeeded)
                {
                    message = new EmailMessageFactory(_emailConfiguration).CreateOfferConfirmationMessage(viewModel);
                    result = _emailService.Send(message);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Details", new
                        {
                            id = viewModel.ArtPieceId,
                            returnUrl = viewModel.ReturnUrl,
                            offerSent = true
                        });
                    }
                    else return View("Error");
                }
                else return View("Error");
        }

        public async Task<IActionResult> Details(int id, string returnUrl, bool offerSent = false)
        {
            var artPiece = _galleryRepo.ArtPieces.Where(ap => ap.ID == id).FirstOrDefault();
            var userId = Request.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);
            if (artPiece != null)
                return View(new GalleryElementDetailsViewModel(_textRepository, artPiece, returnUrl, Request.HttpContext)
                {
                    User = user,
                    OfferSuccesfullySent = offerSent,
                });
            else return View("Error");
        }
    }
}