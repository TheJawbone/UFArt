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
            message.Subject = "Oferta kupna obrazu";
            StringBuilder contentBuilder = new StringBuilder();
            contentBuilder.AppendLine(string.Format("Numer ID obrazu: {0}", offer.ArtPieceId));
            contentBuilder.AppendLine(string.Format("Imię klienta: {0}", offer.ClientName));
            contentBuilder.AppendLine(string.Format("Adres email klienta: {0}", offer.Email));
            if (offer.Phone != null)
                contentBuilder.AppendLine(string.Format("Numer telefonu klienta: {0}", offer.Phone));
            message.Content = contentBuilder.ToString();
            return message;
        }

        internal EmailMessage CreateOfferConfirmationMessage(OfferViewModel offer)
        {
            var message = new EmailMessage();
            message.FromAddress = new EmailAddress() { Address = _emailConfiguration.SmtpUsername };
            message.ToAddresses.Add(new EmailAddress() { Address = offer.Email });
            message.Subject = "Potwierdzenie złożenia oferty";
            message.Content = "Dziękujemy za wyrażenie zainteresowania kupnem przedmiotu." +
                "Skontaktujemy się z Tobą jak najszybciej w celu omówienia szczegółów.\r\n\r\n" +
                "Pozdrawiamy,\r\nZespół Urszula Figiel Art\r\n\r\n" +
                "Wiadomość została wygenerowana automatycznie.";
            return message;
        }
    }
}
