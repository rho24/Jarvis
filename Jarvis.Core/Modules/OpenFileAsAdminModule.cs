using System;
using System.Collections.Generic;
using System.Linq;
using Jarvis.Core.Extensibility;
using Jarvis.Core.Infrastructure;
using Jarvis.Core.Options;

namespace Jarvis.Core.Modules
{
    public class OpenFileAsAdminModule : IJarvisModule
    {
        public string Name {
            get { return "Open file as admin module"; }
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

            return new IOption[] { new OpenFileAsAdminOption(fileOption) };
        }

        #region Nested type: OpenFileAsAdminOption

        class OpenFileAsAdminOption : IOption, IHasDefaultAction
        {
            readonly FileOption _option;

            public OpenFileAsAdminOption(FileOption option) {
                _option = option;
            }

            public void Execute() {
                var executer = new ProcessStarter(_option.FullPath, true);
                executer.Execute();
            }

            public string Name {
                get { return "Open file as admin"; }
            }
        }

        #endregion
    }
}