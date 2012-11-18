using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client;
using Raven.Client.Indexes;
using Raven.Client.Linq;

namespace Jarvis.Core
{
    internal class IndexedDirectorySource : ISource
    {
        private readonly DirectoryInfo _dir;
        private readonly IDocumentStore _documentStore;
        private readonly string _path;

        public IndexedDirectorySource(string path, IDocumentStore documentStore, IScheduler scheduler) {
            _path = path;
            _documentStore = documentStore;

            _dir = new DirectoryInfo(path.ResolvePathAliases());

            scheduler.Immediate(CreateItemsIndex);
            scheduler.Immediate(Index);
        }

        public string Description {
            get { return "Indexed Directory: {0}".Fmt(_path); }
        }

        public IEnumerable<IOption> GetOptions(string term) {
            using (var session = _documentStore.OpenSession()) {
                var items = session.Query<FileOption>("items").Where(f => f.Name.StartsWith(term)).AsProjection<FileOption>().Fetch();
                return items;
            }
        }

        private void CreateItemsIndex() {
            _documentStore.DatabaseCommands.PutIndex("items",
                                                     new IndexDefinitionBuilder<IndexedDirectory, FileOption> {
                                                         Map = dirs => from d in dirs
                                                                       from f in d.Files
                                                                       select new {f.Name, f.FullPath},
                                                         Indexes = {
                                                             {x => x.Name, FieldIndexing.Analyzed}
                                                         },
                                                         Stores = {
                                                             {x => x.Name, FieldStorage.Yes},
                                                             {x => x.FullPath, FieldStorage.Yes},
                                                         }
                                                     },
                                                     true);
        }

        private void Index() {
            using (var session = _documentStore.OpenSession()) {
                var directory = session.Query<IndexedDirectory>().SingleOrDefault(d => d.Path == _path) ?? new IndexedDirectory {Path = _path, Created = DateTime.UtcNow};

                directory.Files = _dir.EnumerateFiles("*.lnk", SearchOption.AllDirectories)
                    .Select(f => new FileOption {Name = Path.GetFileNameWithoutExtension(f.Name), FullPath = f.FullName}).ToList();

                session.Store(directory);
                session.SaveChanges();
            }
        }
    }

    public class ItemsIndex : AbstractMultiMapIndexCreationTask<IOption>
    {
        public ItemsIndex() {
            AddMap<IndexedDirectory>(dirs => from d in dirs
                                             from f in d.Files
                                             select new {f.Name, f.FullPath});

        }
    }
}