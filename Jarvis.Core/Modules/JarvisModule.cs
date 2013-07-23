using System;
using System.Collections.Generic;
using System.Linq;
using Jarvis.Core.Extensibility;
using Jarvis.Core.Infrastructure;
using Jarvis.Core.Options;

namespace Jarvis.Core.Modules
{
    public class JarvisModule:IJarvisModule
    {
        readonly IEnumerable<IJarvisOptionsProvider> _optionsProviders;
        public string Name { get { return "Jarvis"; } }
        public bool ShowModuleInRoot { get { return true; } }
        public bool ShowOptionsInRoot { get { return false; } }

        public JarvisModule(IEnumerable<IJarvisOptionsProvider> optionsProviders ) {
            _optionsProviders = optionsProviders;
        }

        public void Initialize() {
        }

        public IEnumerable<IOption> GetOptions(string term)
        {
            return _optionsProviders.SelectMany(p => p.CreateSubOptions()).FuzzySearch(term);
        }

        public IEnumerable<IOption> GetSubOptions(IOption selectedOption, string term) {
            return Enumerable.Empty<IOption>();
        }
    }
}