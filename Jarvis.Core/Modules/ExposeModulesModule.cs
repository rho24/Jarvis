using System;
using System.Collections.Generic;
using System.Linq;
using Jarvis.Core.Extensibility;
using Jarvis.Core.Infrastructure;
using Jarvis.Core.Options;

namespace Jarvis.Core.Modules
{
    public class ExposeModulesModule : IJarvisModule
    {
        readonly Lazy<IEnumerable<IJarvisModule>> _modules;
        public ExposeModulesModule(Lazy<IEnumerable<IJarvisModule>> modules) {
            _modules = modules;
        }

        public string Name {
            get { return "Modules"; }
        }

        public bool ShowModuleInRoot {
            get { return true; }
        }

        public bool ShowOptionsInRoot {
            get { return true; }
        }

        public void Initialize() {
        }

        public IEnumerable<IOption> GetOptions(string term) {
            return _modules.Value.Where(m => m.ShowModuleInRoot).Select(m => new ModuleOption(m)).FuzzySearch(term).Fetch();
        }
    }
}