using System;
using System.Collections.Generic;

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