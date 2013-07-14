using System;
using System.Collections.Generic;

namespace Jarvis.Core
{
    public abstract class JarvisOptionsProvider : ISubOptionsProvider
    {
        public bool CanSupport(IOption option) {
            return option is JarvisOptionsSource.JarvisOption;
        }

        public abstract IEnumerable<IOption> CreateSubOptions(IOption option);
    }
}