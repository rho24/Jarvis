using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace Jarvis.Core
{
    public class SimpleScheduler : IScheduler
    {
        IEnumerable<IDisposable> _triggers;

        public void Initialize(IEnumerable<IScheduledJob> jobs) {

            _triggers = jobs.Select(j => Observable.Timer(DateTimeOffset.UtcNow, TimeSpan.FromMinutes(1)).Subscribe(
                l => {
                    j.Execute();
                })).ToList();
        }
    }
}