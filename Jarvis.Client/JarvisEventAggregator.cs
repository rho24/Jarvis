using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Jarvis.Core.Events;
using Jarvis.Core.Infrastructure;

namespace Jarvis.Client
{
    public class JarvisEventAggregator : IJarvisEventAggregator, IHandle<object>
    {
        readonly IEventAggregator _caliburnEventAggregator;
        readonly List<SubscriberProxy> _subscribers = new List<SubscriberProxy>();

        public JarvisEventAggregator(IEventAggregator caliburnEventAggregator) {
            _caliburnEventAggregator = caliburnEventAggregator;
            _caliburnEventAggregator.Subscribe(this);
        }

        public void Handle(object message) {
            _subscribers.Apply(s => s.Handle(message));

            _subscribers.RemoveAll(s => s.ReferenceLost());
        }

        public void Subscribe(object instance) {
            if(_subscribers.Any(s => s.ProxyFor(instance)))
                return;

            _subscribers.Add(new SubscriberProxy(instance));
        }

        public void Unsubscribe(object instance) {
            _subscribers.RemoveAll(s => s.ProxyFor(instance));
        }

        public void Publish(object message) {
            _caliburnEventAggregator.Publish(message);
        }

        #region Nested type: SubscriberProxy

        class SubscriberProxy
        {
            readonly WeakReference _reference;

            public SubscriberProxy(object instance) {
                _reference = new WeakReference(instance);
            }

            public bool ProxyFor(object instance) {
                return _reference.Target == instance;
            }

            public void Handle(object message) {
                var target = _reference.Target;
                if(target == null)
                    return;

                var t = target.GetType();

                var handlerInterfaces =
                    t.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IJarvisHandle<>) && i.GetGenericArguments()[0].IsInstanceOfType(message)).Fetch();

                foreach(var handlerInterface in handlerInterfaces) {
                    var method = handlerInterface.GetMethod("Handle");
                    method.Invoke(target, new[] { message });
                }
            }

            public bool ReferenceLost() {
                return _reference.Target == null;
            }
        }

        #endregion
    }
}