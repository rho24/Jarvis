using System;
using System.Collections.Generic;
using System.IO;
using Jarvis.Core.Extensibility;
using Jarvis.Core.Infrastructure;
using Jarvis.Core.Options;

namespace Jarvis.Core
{
    internal class OpenContainingDirectoryProvider : ISubOptionsProvider
    {
        public bool CanSupport(IOption option) {
            return option is FileOption;
        }

        public IEnumerable<IOption> CreateSubOptions(IOption option) {
            yield return new OpenContainingDirectoryOption((FileOption) option);
        }

        #region Nested type: OpenContainingDirectoryOption

        private class OpenContainingDirectoryOption : IOption, IHasDefaultAction
        {
            private readonly FileOption _option;

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