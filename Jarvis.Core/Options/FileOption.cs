using System;
using Jarvis.Core.Infrastructure;

namespace Jarvis.Core.Options
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