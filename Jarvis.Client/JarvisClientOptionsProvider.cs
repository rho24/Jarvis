using System;
using System.Collections.Generic;
using Caliburn.Micro;
using Jarvis.Core;
using Jarvis.Core.Extensibility;
using Jarvis.Core.Options;

namespace Jarvis.Client
{
    public class JarvisClientOptionsProvider : IJarvisOptionsProvider
    {
        readonly IEventAggregator _eventAggregator;

        public JarvisClientOptionsProvider(IEventAggregator eventAggregator) {
            _eventAggregator = eventAggregator;
        }

        public IEnumerable<IOption> CreateSubOptions() {
            yield return new LambdaOption("Close Jarvis", () => _eventAggregator.Publish(new CloseJarvisEvent()));
        }
    }
}