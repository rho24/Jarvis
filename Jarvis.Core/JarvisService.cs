using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
using Raven.Client;
using Raven.Client.Embedded;

namespace Jarvis.Core
{
    public class JarvisService : IJarvisService
    {
        private readonly IContainer _container;
        private readonly IDocumentStore _documentStore;
        public IJarvisServiceSettings Settings { get; set; }

        public JarvisService() {
            _container = BuildContainer();
            _documentStore = _container.Resolve<IDocumentStore>();

            Initialize();
        }

        public IEnumerable<IOption> GetOptions(string term) {
            return _sources.SelectMany(s => s.GetOptions(term)).Fetch();
        }

        private IEnumerable<ISource> _sources;
        public string StudioUrl { get { return (_documentStore as EmbeddableDocumentStore).HttpServer.Configuration.ServerUrl; } }

        private void Initialize() {
            FirstTimeSetup();

            using (var session = _documentStore.OpenSession()) {
                Settings = session.Query<JarvisServiceSettings>()
                    .Customize(c => c.WaitForNonStaleResultsAsOfLastWrite()).Single();

                _sources = session.Query<IndexedDirectory>()
                    .Customize(c => c.WaitForNonStaleResultsAsOfLastWrite())
                    .Fetch()
                    .Select(d => _container.Resolve<IndexedDirectorySource>(new NamedParameter("path", d.Path)))
                    .Fetch();
            }
        }

        private void FirstTimeSetup() {
            using (var session = _documentStore.OpenSession()) {
                if (session.Query<JarvisServiceSettings>().Any())
                    return;

                session.Store(new JarvisServiceSettings {Created = DateTime.UtcNow});

                session.Store(new IndexedDirectory {Path = @"%AppData%\Microsoft\Internet Explorer\Quick Launch\User Pinned\TaskBar"});
                session.Store(new IndexedDirectory {Path = @"~\Desktop"});

                session.SaveChanges();
            }
        }

        private IContainer BuildContainer() {
            var builder = new ContainerBuilder();

            var databaseDir = Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData),
                                           "Jarvis");

            var store = new EmbeddableDocumentStore {
                DataDirectory = databaseDir,
                UseEmbeddedHttpServer = true
            }.Initialize();

            builder.RegisterInstance(store).As<IDocumentStore>();

            builder.Register(c => c.Resolve<IDocumentStore>().OpenSession())
                .As<IDocumentSession>()
                .OnRelease(d => d.Dispose());

            builder.RegisterType<SynchronousScheduler>().As<IScheduler>().SingleInstance();

            builder.RegisterType<IndexedDirectorySource>().AsSelf();

            return builder.Build();
        }
    }
}