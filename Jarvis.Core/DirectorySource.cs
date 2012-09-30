using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        public IEnumerable<IItem> GetItems() {
            if (!_dir.Exists)
                return Enumerable.Empty<IItem>();

            return _dir.EnumerateFiles("*.lnk", SearchOption.AllDirectories).Select(f => new FileItem {Name = Path.GetFileNameWithoutExtension(f.Name), FullPath = f.FullName});
        }
    }
}