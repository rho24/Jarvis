using System.Collections.Generic;
using System.IO;
using System.Linq;
using Raven.Client;

namespace Jarvis.Core
{
    internal class IndexManager
    {
        private readonly IDocumentStore _documentStore;

        public IndexManager(IDocumentStore documentStore) {
            _documentStore = documentStore;
        }

        public void Index() {
            var filesToIndex = _documentStore.OpenSession().Query<IndexedDirectory>().ToList().SelectMany(GetFilesInDir);

            foreach (var file in filesToIndex) {
                using (var session = _documentStore.OpenSession()) {
                    if (!session.Query<IndexedFile>().Any(f => f.Path == file.Path))
                        session.Store(file);
                    session.SaveChanges();
                }
            }
        }

        private IEnumerable<IndexedFile> GetFilesInDir(IndexedDirectory indexedDirectory) {
            var dir = new DirectoryInfo(indexedDirectory.Path.ResolvePathAliases());
            if (!dir.Exists)
                return Enumerable.Empty<IndexedFile>();

            return dir.EnumerateFiles("*.lnk", SearchOption.AllDirectories).Select(f => new IndexedFile {Path = f.FullName});
        }
    }
}