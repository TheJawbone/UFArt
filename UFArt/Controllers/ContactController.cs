using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UFArt.Infrastructure.Mailing;

namespace UFArt.Controllers
{
    public class ContactController : Controller
    {
        private IEmailService _emailService;
        private IEmailConfiguration _emailConfiguration;

        public ContactController(IEmailService emailService, IEmailConfiguration emailConfiguration)
        {
            _emailService = emailService;
            _emailConfiguration = emailConfiguration;
        }

        public IActionResult Index()
        {
            return View();
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