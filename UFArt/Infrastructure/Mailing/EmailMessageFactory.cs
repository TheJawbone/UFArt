using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UFArt.Models.Gallery;

namespace UFArt.Infrastructure.Mailing
{
    public class EmailMessageFactory
    {
        private IEmailConfiguration _emailConfiguration;

        public EmailMessageFactory(IEmailConfiguration emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;
        }

        public EmailMessage CreateOfferMessage(OfferViewModel offer)
        {
            var message = new EmailMessage();
            message.FromAddress = new EmailAddress() { Address = offer.Email };
            message.ToAddresses.Add(new EmailAddress() { Address = _emailConfiguration.SmtpUsername });
            message.Subject = "UFArt - Oferta kupna obrazu";
            StringBuilder contentBuilder = new StringBuilder();
            contentBuilder.AppendLine(string.Format("Numer ID obrazu: {0}", offer.ArtPieceId));
            contentBuilder.AppendLine(string.Format("Imię klienta: {0}", offer.ClientName));
            contentBuilder.AppendLine(string.Format("Adres email klienta: {0}", offer.Email));
            if (offer.Phone != null)
                contentBuilder.AppendLine(string.Format("Numer telefonu klienta: {0}", offer.Phone));
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

        internal EmailMessage CreateOfferConfirmationMessage(OfferViewModel offer)
        {
            var message = new EmailMessage();
            message.FromAddress = new EmailAddress() { Address = _emailConfiguration.SmtpUsername };
            message.ToAddresses.Add(new EmailAddress() { Address = offer.Email });
            message.Subject = "UFArt - Potwierdzenie złożenia oferty";
            message.Content = string.Format("Witaj {0},\r\nDziękujemy za wyrażenie zainteresowania kupnem przedmiotu. " +
                "Skontaktujemy się z Tobą jak najszybciej w celu omówienia szczegółów.\r\n\r\n" +
                "Pozdrawiamy,\r\nZespół Urszula Figiel Art\r\n\r\n" +
                "Wiadomość została wygenerowana automatycznie.", offer.ClientName);
            return message;
        }
    }
}
