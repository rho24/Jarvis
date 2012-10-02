using System;

namespace Jarvis.Core
{
    public class SynchronousScheduler:IScheduler
    {
        public void Immediate(Action action) {
            action();
        }
    }
}