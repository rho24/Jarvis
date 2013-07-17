using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client;

namespace Jarvis.Core
{
    public class IndexDirectoriesJob : IScheduledJob
    {
        readonly IDocumentStore _documentStore;

        public IndexDirectoriesJob(IDocumentStore documentStore) {
            _documentStore = documentStore;
        }

        public void Execute() {
            Task[] tasks;

            using(var session = _documentStore.OpenSession()) {
                var directories = session.Query<IndexedDirectory>().ToList();
                tasks = directories.Select(d => Task.Factory.StartNew(() => Index(d.Id))).ToArray();
            }

            Task.WaitAll(tasks);
        }

        void Index(int id) {
            using(var session = _documentStore.OpenSession()) {

                var directory = session.Load<IndexedDirectory>(id);

                var dir = new DirectoryInfo(directory.Path.ResolvePathAliases());

                directory.Files =
                    dir.EnumerateFiles("*.lnk", SearchOption.AllDirectories).Select(f => new FileOption { Name = Path.GetFileNameWithoutExtension(f.Name), FullPath = f.FullName }).ToList();

                session.Store(directory);
                session.SaveChanges();
            }
        }
    }
}