using System.Collections.Generic;
using System.IO;
using System.Linq;
using Jarvis.Core.Extensibility;
using Jarvis.Core.Infrastructure;
using Jarvis.Core.Options;

namespace Jarvis.Core
{
    internal class DirectorySource : ISource
    {
        private readonly DirectoryInfo _dir;
        private readonly string _path;

        public DirectorySource(string path) {
            _path = path;

            _dir = new DirectoryInfo(path.ResolvePathAliases());
        }

        public string Description {
            get { return "Directory: {0}".Fmt(_path); }
        }

        public IEnumerable<IOption> GetOptions(string term) {
            if (!_dir.Exists)
                return Enumerable.Empty<IOption>();

            return
                _dir.EnumerateFiles("*.lnk", SearchOption.AllDirectories)
                    .Select(f => new FileOption {Name = Path.GetFileNameWithoutExtension(f.Name), FullPath = f.FullName})
                    .Where(f => f.Name.ToLowerInvariant().Contains(term.ToLowerInvariant()));
        }
    }
}