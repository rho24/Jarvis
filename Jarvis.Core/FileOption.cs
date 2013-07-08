namespace Jarvis.Core
{
    public class FileOption : IOption, IHasDefaultAction
    {
        public string DirectoryPath { get; set; }
        public string Name { get; set; }
        public string FullPath { get; set; }

        public void Execute() {
            var executer = new ProcessStarter(FullPath);
            executer.Execute();
        }
    }
}