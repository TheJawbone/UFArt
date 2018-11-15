using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UFArt.Models.Contact;
using UFArt.Models.TextAssets;

namespace UFArt.Controllers
{
    public class ContactEditorController : Controller
    {
        private ITextAssetsRepository _textRepository;

        public ContactEditorController(ITextAssetsRepository textRepository)
        {
            _textRepository = textRepository;
        }

        public IActionResult Index()
        {
            var viewModel = new ContactViewModel(_textRepository);
            viewModel.Email = _textRepository.GetTranslatedValue("contact_email_address", Request.HttpContext);
            viewModel.Telephone = _textRepository.GetTranslatedValue("contact_telephone_number", Request.HttpContext);
            return View();
        }

        public IActionResult UploadAsync(ContactViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var emailAsset = _textRepository.GetAsset("contact_email_address");
                var telephoneAsset = _textRepository.GetAsset("contact_telephone_number");

                emailAsset.Value_pl = viewModel.Email;
                emailAsset.Value_en = viewModel.Email;
                telephoneAsset.Value_pl = viewModel.Telephone;
                telephoneAsset.Value_en = viewModel.Telephone;

                _textRepository.SaveAsset(emailAsset);
                _textRepository.SaveAsset(telephoneAsset);

                return View("Success", new string[] { "Pomyślnie zaktualizowano dane kontaktowe", "/Contact" });
            }
            else
            {
                return View("Index", viewModel);
            }
        }
    }
}