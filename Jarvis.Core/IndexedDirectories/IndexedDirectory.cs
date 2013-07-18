using System;
using System.Collections.Generic;
using Jarvis.Core.Options;

namespace Jarvis.Core.IndexedDirectories
{
    public class IndexedDirectory
    {
        public int Id { get; set; }
        public string Path { get; set; }

        public DateTime Created { get; set; }

        public IEnumerable<FileOption> Files { get; set; }
    }
}