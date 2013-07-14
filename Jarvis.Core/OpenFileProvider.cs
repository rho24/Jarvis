using System.Collections.Generic;

namespace Jarvis.Core
{
    internal class OpenFileProvider : ISubOptionsProvider
    {
        public bool CanSupport(IOption option) {
            return option is FileOption;
        }

        public IEnumerable<IOption> CreateSubOptions(IOption option) {
            yield return new OpenFileOption((FileOption) option);
        }
        
        #region Nested type: OpenFileOption

        private class OpenFileOption : IOption, IHasDefaultAction
        {
            private readonly FileOption _option;

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