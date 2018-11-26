using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UFArt.Models.Gallery;
using UFArt.Models.Identity;
using UFArt.Models.ViewModels;

namespace UFArt.Infrastructure.Mailing
{
    public class EmailMessageFactory
    {
        private IEmailConfiguration _emailConfiguration;

        public EmailMessageFactory(IEmailConfiguration emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;
        }

        public EmailMessage CreateOfferMessage(GalleryElementDetailsViewModel viewModel)
        {
            var message = new EmailMessage();
            message.FromAddress = new EmailAddress() { Address = viewModel.Email };
            message.ToAddresses.Add(new EmailAddress() { Address = _emailConfiguration.SmtpUsername });
            message.Subject = "UFArt - Oferta kupna obrazu";

            StringBuilder contentBuilder = new StringBuilder();
            contentBuilder.AppendLine(string.Format("<img src=\"{0}\" height=\"150\"><br>", viewModel.ImageUri));
            contentBuilder.AppendLine(string.Format("Imię klienta: {0}<br>", viewModel.ClientName));
            contentBuilder.AppendLine(string.Format("Adres email klienta: {0}<br>", viewModel.Email));
            if (viewModel.Phone != null)
                contentBuilder.AppendLine(string.Format("Numer telefonu klienta: {0}<br>", viewModel.Phone));
            message.Content = contentBuilder.ToString();
            return message;
        }

        public EmailMessage CreateContactMessage(string content)
        {
            var message = new EmailMessage();
            message.FromAddress = new EmailAddress() { Address = _emailConfiguration.SmtpUsername };
            message.ToAddresses.Add(new EmailAddress() { Address = _emailConfiguration.SmtpUsername });
            message.Subject = "UFArt - Wiadomość kontaktowa";
            message.Content = content;
            return message;
        }

        internal EmailMessage CreateOfferConfirmationMessage(GalleryElementDetailsViewModel viewModel)
        {
            var message = new EmailMessage();
            message.FromAddress = new EmailAddress() { Address = _emailConfiguration.SmtpUsername };
            message.ToAddresses.Add(new EmailAddress() { Address = viewModel.Email });
            message.Subject = "UFArt - Potwierdzenie złożenia oferty";
            message.Content = string.Format("Witaj {0},<br><br>Dziękujemy za wyrażenie zainteresowania kupnem przedmiotu. " +
                "Skontaktujemy się z Tobą jak najszybciej w celu omówienia szczegółów.<br><br>" +
                "Pozdrawiamy,<br>Zespół Urszula Figiel Art<br><br>" +
                "Wiadomość została wygenerowana automatycznie.", viewModel.ClientName);
            return message;
        }

        public EmailMessage CreateActivationMessage(User user, HttpRequest request)
        {
            var link = string.Format("{0}://{1}/AccountActivation?code={2}", request.Scheme, request.Host, user.Id.ToString());
            var message = new EmailMessage();
            message.FromAddress = new EmailAddress() { Address = _emailConfiguration.SmtpUsername };
            message.ToAddresses.Add(new EmailAddress() { Address = user.Email });
            message.Subject = "UFArt - aktywacja konta";
            message.Content = string.Format("Witaj {0},<br><br>Dziękujemy za założenie konta w serwisie Urszula Figiel Art. " +
                "Kliknij poniższy link, aby aktywować konto. <br><br>{1}<br><br>Pozdrawiamy,<br>Zespół Urszula Figiel Art.",
                user.UserName, link);
            return message;
        }
    }
}
