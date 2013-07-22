using System;

namespace Jarvis.Core.Gmail
{
    public class GmailModuleConfig
    {
        public string Id {
            get { return "GmailModuleConfig"; }
        }

        public string UserName { get; set; }
        public string Password { get; set; }
    }
}