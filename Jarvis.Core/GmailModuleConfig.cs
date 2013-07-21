using System;

namespace Jarvis.Core
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