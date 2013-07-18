using System;

namespace Jarvis.Core.Options
{
    public class LambdaOption : IOption, IHasDefaultAction
    {
        readonly Action _executeAction;

        public LambdaOption(string name, Action executeAction) {
            Name = name;
            _executeAction = executeAction;
        }

        public void Execute() {
            _executeAction();
        }

        public string Name { get; private set; }
    }
}