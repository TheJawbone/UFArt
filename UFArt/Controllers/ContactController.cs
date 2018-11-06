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

        public ContactController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendTestMail()
        {
            EmailMessage message = new EmailMessage();
            message.Content = "Hello";
            message.Subject = "Test";
            _emailService.Send(message);
            return View("Index");
        }
    }
}