using System;
using System.Collections.Generic;
using Jarvis.Core.Options;

namespace Jarvis.Core.Extensibility
{
    public interface ISource
    {
        string Description { get; }
        IEnumerable<IOption> GetOptions(string term);
    }
}