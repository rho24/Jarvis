using System;
using System.Collections.Generic;
using System.Linq;
using Jarvis.Core.Extensibility;
using Jarvis.Core.Infrastructure;
using Jarvis.Core.Options;

namespace Jarvis.Core
{
    public class JarvisModule:IJarvisModule
    {
        readonly IEnumerable<JarvisOptionsProvider> _optionsProviders;
        public string Name { get { return "Jarvis"; } }
        public bool ShowModuleInRoot { get { return true; } }
        public bool ShowOptionsInRoot { get { return false; } }

        public JarvisModule(IEnumerable<JarvisOptionsProvider> optionsProviders ) {
            _optionsProviders = optionsProviders;
        }

        public void Initialize() {
        }

        public IEnumerable<IOption> GetOptions(string term) {
            return _optionsProviders.SelectMany(p => p.CreateSubOptions(null)).FuzzySearch(term);
        }

        public class JarvisOption : IOption
        {
            public string Name
            {
                get { return "Jarvis"; }
            }
        }
    }
}