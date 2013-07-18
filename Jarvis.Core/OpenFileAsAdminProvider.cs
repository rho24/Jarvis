using System.Collections.Generic;
using Jarvis.Core.Extensibility;
using Jarvis.Core.Infrastructure;
using Jarvis.Core.Options;

namespace Jarvis.Core
{
    internal class OpenFileAsAdminProvider : ISubOptionsProvider
    {
        public bool CanSupport(IOption option) {
            return option is FileOption;
        }

        public IEnumerable<IOption> CreateSubOptions(IOption option) {
            yield return new OpenFileAsAdminOption((FileOption) option);
        }
        
        #region Nested type: OpenFileOption

        private class OpenFileAsAdminOption : IOption, IHasDefaultAction
        {
            private readonly FileOption _option;

            public OpenFileAsAdminOption(FileOption option)
            {
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