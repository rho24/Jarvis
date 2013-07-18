using System;

namespace Jarvis.Core
{
    public class GmailModuleConfig
    {
        public string Id {
            get { return "GmailModuleConfig"; }
        }

        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        public string TargetUser { get; set; }
        public string Domain { get; set; }
        public string SignatureMethod { get; set; }
    }
}