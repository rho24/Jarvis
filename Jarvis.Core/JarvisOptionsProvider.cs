using System;
using System.Collections.Generic;
using Jarvis.Core.Extensibility;
using Jarvis.Core.Options;

namespace Jarvis.Core
{
    public abstract class JarvisOptionsProvider : ISubOptionsProvider
    {
        public bool CanSupport(IOption option) {
            return option is JarvisModule.JarvisOption;
        }

        public abstract IEnumerable<IOption> CreateSubOptions(IOption option);
    }
}