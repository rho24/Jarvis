using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Raven.Client;
using Raven.Client.Embedded;

namespace Jarvis.Core
{
    public class JarvisService : IJarvisService
    {
        readonly IContainer _container;

        readonly IDocumentStore _documentStore;

        readonly IEnumerable<ISubOptionProvider> _subOptionProvides;

        IEnumerable<ISource> _sources;

        public IJarvisServiceSettings Settings { get; set; }

        public JarvisService() {
            _container = BuildContainer();
            _subOptionProvides = _container.Resolve<IEnumerable<ISubOptionProvider>>();
            _documentStore = _container.Resolve<IDocumentStore>();

            Initialize();
        }

        public IEnumerable<IOption> GetOptions(string term) {
            return _sources.SelectMany(s => s.GetOptions(term)).Fetch();
        }

        public string StudioUrl {
            get {
                var embeddableDocumentStore = _documentStore as EmbeddableDocumentStore;
                return embeddableDocumentStore != null ? embeddableDocumentStore.HttpServer.Configuration.ServerUrl : null;
            }
        }

        public IEnumerable<IOption> GetSubOptions(IOption option) {
            return _subOptionProvides.Where(p => p.CanSupport(option)).Select(p => p.CreateSubOption(option)).Fetch();
        }

        public bool ExecuteOption(IOption option) {
            var executableOption = option as IHasDefaultAction;

            if (executableOption != null) {
                executableOption.Execute();
                return true;
            }

            return false;
        }

        void Initialize() {
            FirstTimeSetup();

            using (var session = _documentStore.OpenSession()) {
                Settings = session.Query<JarvisServiceSettings>().Customize(c => c.WaitForNonStaleResultsAsOfLastWrite()).Single();

                _sources =
                    session.Query<IndexedDirectory>()
                        .Customize(c => c.WaitForNonStaleResultsAsOfLastWrite())
                        .Fetch()
                        .Select(d => _container.Resolve<IndexedDirectorySource>(new NamedParameter("path", d.Path)))
                        .Fetch();
            }
        }

        void FirstTimeSetup() {
            using (var session = _documentStore.OpenSession()) {
                if (session.Query<JarvisServiceSettings>().Any()) return;

                session.Store(new JarvisServiceSettings { Created = DateTime.UtcNow });

                session.Store(new IndexedDirectory { Path = @"%AppData%\Microsoft\Internet Explorer\Quick Launch\User Pinned\TaskBar" });
                session.Store(new IndexedDirectory { Path = @"~\Desktop" });

                session.SaveChanges();
            }
        }

        IContainer BuildContainer() {
            var builder = new ContainerBuilder();

            var databaseDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Jarvis");

            var store = new EmbeddableDocumentStore { DataDirectory = databaseDir, UseEmbeddedHttpServer = true }.Initialize();

            builder.RegisterInstance(store).As<IDocumentStore>();

            builder.Register(c => c.Resolve<IDocumentStore>().OpenSession()).As<IDocumentSession>().OnRelease(d => d.Dispose());

            builder.RegisterType<SynchronousScheduler>().As<IScheduler>().SingleInstance();

            builder.RegisterType<IndexedDirectorySource>().AsSelf();

            builder.RegisterAssemblyTypes(Assembly.GetCallingAssembly()).AssignableTo<ISubOptionProvider>().AsImplementedInterfaces();

            return builder.Build();
        }
    }
}