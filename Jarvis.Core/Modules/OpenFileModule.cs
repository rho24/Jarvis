using System;
using System.Collections.Generic;
using System.Linq;
using Jarvis.Core.Extensibility;
using Jarvis.Core.Infrastructure;
using Jarvis.Core.Options;

namespace Jarvis.Core.Modules
{
    public class OpenFileModule : IJarvisModule
    {
        public string Name {
            get { return "Open file module"; }
        }

        public bool ShowModuleInRoot {
            get { return false; }
        }

        public bool ShowOptionsInRoot {
            get { return false; }
        }

        public void Initialize() {}

        public IEnumerable<IOption> GetOptions(string term) {
            return Enumerable.Empty<IOption>();
        }

        public IEnumerable<IOption> GetSubOptions(IOption selectedOption, string term) {
            var fileOption = selectedOption as FileOption;
            if(fileOption == null)
                return Enumerable.Empty<IOption>();

            return new IOption[] { new OpenFileOption(fileOption) };
        }

        #region Nested type: OpenFileOption

        class OpenFileOption : IOption, IHasDefaultAction
        {
            readonly FileOption _option;

            public OpenFileOption(FileOption option) {
                _option = option;
            }

            public void Execute() {
                var executer = new ProcessStarter(_option.FullPath);
                executer.Execute();
            }

            public string Name {
                get { return "Open file"; }
            }
        }

        #endregion
    }
}