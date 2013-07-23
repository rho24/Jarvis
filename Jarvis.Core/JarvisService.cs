using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Jarvis.Core.Events;
using Jarvis.Core.Extensibility;
using Jarvis.Core.IndexedDirectories;
using Jarvis.Core.Infrastructure;
using Jarvis.Core.Options;
using Raven.Abstractions.Extensions;
using Raven.Client;
using Raven.Client.Embedded;

namespace Jarvis.Core
{
    public class JarvisService : IJarvisService, IJarvisHandle<ExecuteOptionMessage>
    {
        readonly IDocumentStore _documentStore;

        readonly IEnumerable<IJarvisModule> _modules;
        readonly IEnumerable<IScheduledJob> _scheduledJobs;
        readonly IScheduler _scheduler;

        public IJarvisServiceSettings Settings { get; set; }

        public JarvisService(IComponentContext container) {
            _modules = container.Resolve<IEnumerable<IJarvisModule>>();
            _documentStore = container.Resolve<IDocumentStore>();
            _scheduler = container.Resolve<IScheduler>();
            _scheduledJobs = container.Resolve<IEnumerable<IScheduledJob>>();

            Initialize();
        }

        public void Handle(ExecuteOptionMessage message) {
            ExecuteOption(message.Option);
        }
        
        public IEnumerable<IOption> GetOptions(string term) {
            return _modules.Where(m => m.ShowOptionsInRoot).SelectMany(m => m.GetOptions(term));
        }

        public string StudioUrl {
            get {
                var embeddableDocumentStore = _documentStore as EmbeddableDocumentStore;
                return embeddableDocumentStore != null ? embeddableDocumentStore.HttpServer.Configuration.ServerUrl : null;
            }
        }

        public IEnumerable<IOption> GetSubOptions(IOption option) {
            return _modules.SelectMany(m => m.GetSubOptions(option, "")).Fetch();
        }

        public bool ExecuteOption(IOption option) {
            var executableOption = option as IHasDefaultAction;

            if(executableOption != null) {
                executableOption.Execute();
                return true;
            }

            return false;
        }

        void Initialize() {
            FirstTimeSetup();
            using(var session = _documentStore.OpenSession()) Settings = session.Query<JarvisServiceSettings>().Customize(c => c.WaitForNonStaleResultsAsOfLastWrite()).Single();

            _modules.ForEach(m => m.Initialize());

            _scheduler.Initialize(_scheduledJobs);
        }

        void FirstTimeSetup() {
            using(var session = _documentStore.OpenSession()) {
                if(session.Query<JarvisServiceSettings>().Any()) return;

                session.Store(new JarvisServiceSettings { Created = DateTime.UtcNow });

                session.Store(new IndexedDirectory { Path = @"%AppData%\Microsoft\Internet Explorer\Quick Launch\User Pinned\TaskBar" });
                session.Store(new IndexedDirectory { Path = @"~\Desktop" });
                session.Store(new IndexedDirectory { Path = @"%programdata%\Microsoft\Windows\Start Menu\" });

                session.SaveChanges();
            }
        }
    }
}