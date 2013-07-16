using System;
using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client;
using Raven.Client.Indexes;

namespace Jarvis.Core
{
    public class IndexedDirectoriesModule : IJarvisModule
    {
        readonly IDocumentStore _documentStore;

        public IndexedDirectoriesModule(IDocumentStore documentStore) {
            _documentStore = documentStore;
        }

        public void Initialize()
        {
            _documentStore.DatabaseCommands.PutIndex("items",
                new IndexDefinitionBuilder<IndexedDirectory, FileOption>
                {
                    Map = dirs => from d in dirs
                        from f in d.Files
                        select new { DirectoryPath = d.Path, f.Name, f.FullPath },
                    Indexes = {
                        {x => x.DirectoryPath, FieldIndexing.Default},
                        {x => x.Name, FieldIndexing.Analyzed}
                    },
                    Stores = {
                        {x => x.Name, FieldStorage.Yes},
                        {x => x.FullPath, FieldStorage.Yes},
                    }
                },
                true);
        }
    }
}