using System;
using Jarvis.Core.Extensibility;

namespace Jarvis.Core.Options
{
    public class ModuleOption:IOption
    {
        readonly IJarvisModule _module;

        public ModuleOption(IJarvisModule module) {
            _module = module;
        }

        public string Name { get { return _module.Name; } }
    }
}