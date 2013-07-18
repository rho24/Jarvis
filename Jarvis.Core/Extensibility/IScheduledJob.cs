using System;

namespace Jarvis.Core.Extensibility
{
    public interface IScheduledJob
    {
        void Execute();
    }
}