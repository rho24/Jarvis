using Jarvis.Core.Infrastructure;
using Jarvis.Core.Options;

namespace Jarvis.Core.Gmail
{
    public class GmailOption : IOption, IHasDefaultAction
    {
        public string Name { get { return "Gmail"; } }
        public void Execute()
        {
            var executer = new ProcessStarter("http://gmail.com");
            executer.Execute();
        }
    }
}