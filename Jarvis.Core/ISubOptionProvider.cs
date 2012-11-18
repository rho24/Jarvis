using System.IO;

namespace Jarvis.Core
{
    public interface ISubOptionProvider
    {
        bool CanSupport(IOption option);
        IOption CreateSubOption(IOption option);
    }

    internal class OpenContainingDirectoryProvider : ISubOptionProvider
    {
        public bool CanSupport(IOption option) {
            return option is FileOption;
        }

        public IOption CreateSubOption(IOption option) {
            return new OpenContainingDirectoryOption((FileOption) option);
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