using System;
using System.Collections.Generic;
using Jarvis.Core.Options;

namespace Jarvis.Core.Extensibility
{
    public interface ISubOptionsProvider
    {
        bool CanSupport(IOption option);
        IEnumerable<IOption> CreateSubOptions(IOption option);
    }
}