using System;

namespace Jarvis.Core.Events
{
    public interface IJarvisEventAggregator
    {
        void Subscribe(object instance);
        void Unsubscribe(object instance);
        void Publish(object message);
    }
}