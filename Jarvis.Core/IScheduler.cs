using System;
using System.Collections.Generic;

namespace Jarvis.Core
{
    public interface IScheduler
    {
        void Initialize(IEnumerable<IScheduledJob> jobs);
    }
}