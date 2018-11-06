using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UFArt.Infrastructure.Mailing
{
    public interface IEmailService
    {
        EmailSendResult Send(EmailMessage emailMessage);
    }
}
