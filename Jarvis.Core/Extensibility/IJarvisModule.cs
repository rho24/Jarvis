using System;
using System.Collections.Generic;
using Jarvis.Core.Options;

namespace Jarvis.Core.Extensibility
{
    public interface IJarvisModule
    {
        string Name { get; }
        bool ShowModuleInRoot { get; }
        bool ShowOptionsInRoot { get; }

        void Initialize();
        IEnumerable<IOption> GetOptions(string term);
    }
}