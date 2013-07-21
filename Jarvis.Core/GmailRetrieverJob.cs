using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Xml.Linq;
using Jarvis.Core.Events;
using Jarvis.Core.Extensibility;
using Jarvis.Core.Infrastructure;

namespace Jarvis.Core
{
    public class GmailRetrieverJob : IScheduledJob
    {
        readonly IJarvisEventAggregator _eventAggregator;
        readonly GmailModule _module;

        public GmailRetrieverJob(GmailModule module, IJarvisEventAggregator eventAggregator) {
            _module = module;
            _eventAggregator = eventAggregator;
        }

        public void Execute() {
            var http = new HttpClient(new HttpClientHandler { Credentials = new NetworkCredential(_module.Config.UserName, _module.Config.Password) });

            http.GetStreamAsync("https://mail.google.com/mail/feed/atom").ContinueWith(
                t => {
                    var result = t.Result;
                    var xml = XDocument.Load(result);
                    XNamespace xmlns = "http://purl.org/atom/ns#";
                    var emails = (from entry in xml.Descendants(xmlns + "entry")
                        from author in entry.Descendants(xmlns + "author")
                        select
                            new MailMessage {
                                Subject = entry.Element(xmlns + "title").Value,
                                Body = entry.Element(xmlns + "summary").Value,
                                From = new MailAddress(author.Element(xmlns + "email").Value, author.Element(xmlns + "name").Value)
                            }).Fetch();
                    _eventAggregator.Publish(new CurrentUnreadEmails { Emails = emails });
                });
        }
    }
}