using System;
using System.Collections.Generic;
using System.Linq;
using Jarvis.Core.Extensibility;
using Jarvis.Core.Infrastructure;
using Jarvis.Core.Options;
using Raven.Client;
using Raven.Client.Linq;

namespace Jarvis.Core.IndexedDirectories
{
    public class IndexedDirectoriesSource : ISource
    {
        readonly IDocumentStore _documentStore;

        public IndexedDirectoriesSource(IDocumentStore documentStore) {
            _documentStore = documentStore;
        }

        public string Description {
            get { return "Indexed Directories"; }
        }

        public IEnumerable<IOption> GetOptions(string term) {
            using(var session = _documentStore.OpenSession()) {
                var items = session.Query<FileOption>("items").Where(f => f.Name.StartsWith(term)).AsProjection<FileOption>().Distinct().Fetch();
                return items;
            }
        }
    }
}