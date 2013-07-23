using System;
using System.Collections.Generic;
using System.Linq;
using Jarvis.Core.Extensibility;
using Jarvis.Core.Options;

namespace Jarvis.Core
{
    public class JarvisModule:IJarvisModule
    {
        public string Name { get { return "Jarvis"; } }
        public bool ShowModuleInRoot { get { return true; } }
        public bool ShowOptionsInRoot { get { return false; } }

        public void Initialize() {
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