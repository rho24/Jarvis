using System;
using System.Collections.Generic;
using Jarvis.Core.Extensibility;
using Jarvis.Core.Options;

namespace Jarvis.Core
{
    public interface IJarvisService
    {
        string StudioUrl { get; }

        IEnumerable<IOption> GetOptions(string term);
        IEnumerable<IOption> GetSubOptions(IOption option);
        bool ExecuteOption(IOption option);
    }
}