using System;
using Jarvis.Core.Extensibility;

namespace Jarvis.Core
{
    public class GmailModule : IJarvisModule
    {
        readonly GmailModuleConfig _config;

        public GmailModuleConfig Config {
            get { return _config; }
        }

        public GmailModule(GmailModuleConfig config) {
            _config = config;
        }

        public void Initialize() {}
    }
}