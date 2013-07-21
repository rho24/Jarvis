using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace Jarvis.Core
{
    public class CurrentUnreadEmails
    {
        public IEnumerable<MailMessage> Emails { get; set; }
    }
}