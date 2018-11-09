using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UFArt.Infrastructure.Mailing;
using UFArt.Models.Contact;
using UFArt.Models.TextAssets;

namespace UFArt.Controllers
{
    public class ContactController : Controller
    {
        private IEmailService _emailService;
        private readonly ITextAssetsRepository _textRepository;
        private readonly IEmailConfiguration _emailConfiguration;
        private readonly ContactViewModel _model;

        public ContactController(IEmailService emailService, IEmailConfiguration emailConfiguration, ITextAssetsRepository textRepository)
        {
            _emailService = emailService;
            _emailConfiguration = emailConfiguration;
            _textRepository = textRepository;
        }

        public IActionResult Index()
        {
            return View(new ContactViewModel(_textRepository, Request.HttpContext));
        }

        [HttpPost]
        public IActionResult SendMessage(string content)
        {
            var message = new EmailMessageFactory(_emailConfiguration).CreateContactMessage(content);
            var result = _emailService.Send(message);
            if(result.Succeeded)
                return View("Success", new string[] { "Pomyślnie wysłano wiadomość", "/Contact" });
            else return View("Error");
        }
    }
}