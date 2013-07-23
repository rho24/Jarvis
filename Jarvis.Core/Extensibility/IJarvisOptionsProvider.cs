using System;
using System.Collections.Generic;
using Jarvis.Core.Options;

namespace Jarvis.Core.Extensibility
{
    public interface IJarvisOptionsProvider
    {
        IEnumerable<IOption> CreateSubOptions();
    }
}