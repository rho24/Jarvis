using System;
using System.Collections.Generic;
using Caliburn.Micro;
using Jarvis.Core;
using Jarvis.Core.Options;

namespace Jarvis.Client
{
    public class JarvisClientOptionsProvider : JarvisOptionsProvider
    {
        readonly IEventAggregator _eventAggregator;

        public JarvisClientOptionsProvider(IEventAggregator eventAggregator) {
            _eventAggregator = eventAggregator;
        }

        public override IEnumerable<IOption> CreateSubOptions(IOption option)
        {
            yield return new LambdaOption("Close Jarvis", () => _eventAggregator.Publish(new CloseJarvisEvent()));
        }
    }
}