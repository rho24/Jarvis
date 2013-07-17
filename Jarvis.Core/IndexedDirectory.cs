using System;
using System.Collections.Generic;

namespace Jarvis.Core
{
    public class IndexedDirectory
    {
        public int Id { get; set; }
        public string Path { get; set; }

        public DateTime Created { get; set; }

        public IEnumerable<FileOption> Files { get; set; }
    }
}