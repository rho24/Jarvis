using System;

namespace Jarvis.Core
{
    public interface IScheduler
    {
        void Immediate(Action action);
    }
}