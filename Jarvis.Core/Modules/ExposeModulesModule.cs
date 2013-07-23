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

        public void Initialize() {}

        public IEnumerable<IOption> GetOptions(string term) {
            return _modules.Value.Where(m => m.ShowModuleInRoot).Select(m => new ModuleOption(m)).FuzzySearch(term).Fetch();
        }

        public IEnumerable<IOption> GetSubOptions(IOption selectedOption, string term) {
            var option = selectedOption as ModuleOption;
            if(option == null) return Enumerable.Empty<IOption>();

            if(option.Module == this)
                return _modules.Value.Select(m => new ModuleOption(m)).FuzzySearch(term).Fetch();

            return option.Module.GetOptions(term);
        }

        #region Nested type: ModuleOption

        class ModuleOption : IOption
        {
            readonly IJarvisModule _module;

            public IJarvisModule Module {
                get { return _module; }
            }

            public ModuleOption(IJarvisModule module) {
                _module = module;
            }

            public string Name {
                get { return _module.Name; }
            }
        }

        #endregion
    }
}