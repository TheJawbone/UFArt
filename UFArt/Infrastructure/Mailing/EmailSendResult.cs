using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UFArt.Infrastructure.Mailing
{
    public class EmailSendResult
    {
        public List<EmailMessage> FailedMessages { get; set; }
        public List<EmailMessage> SucceededMessages { get; set; }
        public bool Succeeded => FailedMessages.Count > 0 ? false : true;

        public EmailSendResult()
        {
            FailedMessages = new List<EmailMessage>();
            SucceededMessages = new List<EmailMessage>();
        }
    }
}
