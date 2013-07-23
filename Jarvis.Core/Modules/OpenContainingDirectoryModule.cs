using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Jarvis.Core.Extensibility;
using Jarvis.Core.Infrastructure;
using Jarvis.Core.Options;

namespace Jarvis.Core.Modules
{
    public class OpenContainingDirectoryModule : IJarvisModule
    {
        public string Name {
            get { return "Open containing directory module"; }
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

            return new IOption[] { new OpenContainingDirectoryOption(fileOption) };
        }

        #region Nested type: OpenContainingDirectoryOption

        class OpenContainingDirectoryOption : IOption, IHasDefaultAction
        {
            readonly FileOption _option;

            public OpenContainingDirectoryOption(FileOption option) {
                _option = option;
            }

            public void Execute() {
                var dir = Path.GetDirectoryName(_option.FullPath);

                var executer = new ProcessStarter(dir);
                executer.Execute();
            }

            public string Name {
                get { return "Open Containing Directory"; }
            }
        }

        #endregion
    }
}