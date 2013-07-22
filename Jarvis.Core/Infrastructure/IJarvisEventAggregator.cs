using System;

namespace Jarvis.Core.Infrastructure
{
    public interface IJarvisEventAggregator
    {
        void Subscribe(object instance);
        void Unsubscribe(object instance);
        void Publish(object message);
    }
}