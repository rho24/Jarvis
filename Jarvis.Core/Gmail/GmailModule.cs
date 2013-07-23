using System;
using System.Collections.Generic;
using System.Linq;
using Jarvis.Core.Extensibility;
using Jarvis.Core.Options;

namespace Jarvis.Core.Gmail
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

        public string Name {
            get { return "Gmail"; }
        }

        public bool ShowModuleInRoot {
            get { return true; }
        }

        public bool ShowOptionsInRoot {
            get { return false; }
        }

        public void Initialize() {}
        public IEnumerable<IOption> GetOptions(string term)
        {
            return Enumerable.Empty<IOption>();
        }

        public IEnumerable<IOption> GetSubOptions(IOption selectedOption, string term)
        {
            return Enumerable.Empty<IOption>();
        }
    }
}