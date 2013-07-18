using System;
using System.Collections.Generic;
using Jarvis.Core.Extensibility;

namespace Jarvis.Core.Infrastructure
{
    public interface IScheduler
    {
        void Initialize(IEnumerable<IScheduledJob> jobs);
    }
}