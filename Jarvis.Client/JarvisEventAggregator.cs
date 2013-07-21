using Caliburn.Micro;
using Jarvis.Core.Events;

namespace Jarvis.Client
{
    public class JarvisEventAggregator : IJarvisEventAggregator
    {
        readonly IEventAggregator _caliburnEventAggregator;

        public JarvisEventAggregator(IEventAggregator caliburnEventAggregator) {
            _caliburnEventAggregator = caliburnEventAggregator;
        }

        public void Subscribe(object instance) {
            _caliburnEventAggregator.Subscribe(instance);
        }

        public void Unsubscribe(object instance)
        {
            _caliburnEventAggregator.Unsubscribe(instance);
        }

        public void Publish(object message)
        {
            _caliburnEventAggregator.Publish(message);
        }
    }
}