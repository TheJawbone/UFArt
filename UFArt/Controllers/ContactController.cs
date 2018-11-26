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

        public ContactController(IEmailService emailService, IEmailConfiguration emailConfiguration, ITextAssetsRepository textRepository)
        {
            _emailService = emailService;
            _emailConfiguration = emailConfiguration;
            _textRepository = textRepository;
        }

        public IActionResult Index(bool messageSent = false)
        {
            return View(new ContactViewModel(_textRepository) { MessageSent = messageSent });
        }

        [HttpPost]
        public IActionResult SendMessage(string content)
        {
            var message = new EmailMessageFactory(_emailConfiguration).CreateContactMessage(content);
            var result = _emailService.Send(message);
            if(result.Succeeded)
                return RedirectToAction("Index", new { messageSent = true });
            else return View("Error");
        }
    }
}