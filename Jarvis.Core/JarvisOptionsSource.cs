using System;
using System.Collections.Generic;
using System.Linq;

namespace Jarvis.Core
{
    public class JarvisOptionsSource : ISource
    {
        public string Description {
            get { return "Jarvis options"; }
        }

        public IEnumerable<IOption> GetOptions(string term) {
            if("jarvis".Contains(term.ToLowerInvariant()))
                return new[]{new JarvisOption()};

            return Enumerable.Empty<IOption>();
        }

        public class JarvisOption : IOption
        {
            public string Name
            {
                get { return "Jarvis"; }
            }
        }
    }
}