using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace UFArt.Infrastructure.Mailing
{
    public class EmailService : IEmailService
    {
        private readonly IEmailConfiguration _emailConfiguration;

        public EmailService(IEmailConfiguration emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;
        }

        public EmailSendResult Send(EmailMessage message)
        {
            var result = new EmailSendResult();
            foreach (EmailAddress to in message.ToAddresses)
            {
                var mailMessage = new MailMessage(message.FromAddress.Address, to.Address, message.Subject, message.Content);
                mailMessage.IsBodyHtml = true;
                using (var emailClient = new SmtpClient())
                {
                    emailClient.Host = _emailConfiguration.SmtpServer;
                    emailClient.Port = _emailConfiguration.SmtpPort;
                    emailClient.EnableSsl = true;
                    emailClient.Credentials = new NetworkCredential(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);
                    try
                    {
                        emailClient.Send(mailMessage);
                        result.SucceededMessages.Add(message);
                    }
                    catch(Exception)
                    {
                        result.FailedMessages.Add(message);
                    }
                }
            }
            return result;
        }
    }
}
