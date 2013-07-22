using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace Jarvis.Core.Events
{
    public class CurrentUnreadEmails
    {
        public IEnumerable<MailMessage> Emails { get; set; }
    }
}