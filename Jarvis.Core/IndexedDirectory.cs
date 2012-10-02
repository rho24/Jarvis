using System;
using System.Collections.Generic;

namespace Jarvis.Core
{
    public class IndexedDirectory
    {
        public string Path { get; set; }

        public DateTime Created { get; set; }

        public IEnumerable<FileItem> Files { get; set; }
    }
}