using System;
using System.Collections.Generic;
using System.Linq;
using Jarvis.Core.Extensibility;
using Jarvis.Core.Infrastructure;
using Jarvis.Core.Options;
using Raven.Abstractions.Indexing;
using Raven.Client;
using Raven.Client.Indexes;
using Raven.Client.Linq;

namespace Jarvis.Core.IndexedDirectories
{
    public class IndexedDirectoriesModule : IJarvisModule
    {
        readonly IDocumentStore _documentStore;

        public IndexedDirectoriesModule(IDocumentStore documentStore) {
            _documentStore = documentStore;
        }

        public string Name {
            get { return "Indexed Directories"; }
        }

        public bool ShowModuleInRoot { get { return true; } }

        public bool ShowOptionsInRoot {
            get { return true; }
        }

        public void Initialize() {
            _documentStore.DatabaseCommands.PutIndex(
                "items",
                new IndexDefinitionBuilder<IndexedDirectory, FileOption> {
                    Map = dirs => from d in dirs from f in d.Files select new { DirectoryPath = d.Path, f.Name, f.FullPath },
                    Indexes = { { x => x.DirectoryPath, FieldIndexing.Default }, { x => x.Name, FieldIndexing.Analyzed } },
                    Stores = { { x => x.Name, FieldStorage.Yes }, { x => x.FullPath, FieldStorage.Yes }, }
                },
                true);
        }

        public IEnumerable<IOption> GetOptions(string term) {
            using(var session = _documentStore.OpenSession()) {
                var items = session.Query<FileOption>("items").Where(f => f.Name.StartsWith(term)).AsProjection<FileOption>().Distinct().Fetch();
                return items;
            }
        }
    }
}