using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Jarvis.Core.Events;
using Jarvis.Core.Extensibility;
using Jarvis.Core.Gmail;
using Jarvis.Core.Infrastructure;
using Raven.Client;
using Raven.Client.Embedded;
using Module = Autofac.Module;

namespace Jarvis.Core
{
    public class JarvisAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder) {
            var databaseDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Jarvis");

            var store = new EmbeddableDocumentStore { DataDirectory = databaseDir, UseEmbeddedHttpServer = true }.Initialize();

            builder.RegisterInstance(store).As<IDocumentStore>();

            builder.Register(c => c.Resolve<IDocumentStore>().OpenSession()).As<IDocumentSession>().OnRelease(d => d.Dispose());

            builder.RegisterType<SimpleScheduler>().AsImplementedInterfaces().SingleInstance();
            
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly(), Assembly.GetEntryAssembly()).AssignableTo<IJarvisModule>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly(), Assembly.GetEntryAssembly()).AssignableTo<ISubOptionsProvider>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly(), Assembly.GetEntryAssembly()).AssignableTo<IScheduledJob>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly(), Assembly.GetEntryAssembly()).AssignableTo<IJarvisOptionsProvider>().As<IJarvisOptionsProvider>().SingleInstance();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly(), Assembly.GetEntryAssembly()).AsSelf();

            builder.Register(
                c => {
                    var session = c.Resolve<IDocumentSession>();
                    var config = session.Load<GmailModuleConfig>("GmailModuleConfig");
                    if(config == null) {
                        config = new GmailModuleConfig();
                        session.Store(config);
                        session.SaveChanges();
                    }
                    return config;
                });
        }

        protected override void AttachToComponentRegistration(IComponentRegistry registry, IComponentRegistration registration) {
            registration.Activated += registration_Activated;
        }

        void registration_Activated(object sender, ActivatedEventArgs<object> e) {
            if(e == null)
                return;

            if(e.Instance.GetType().GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IJarvisHandle<>)))
                e.Context.Resolve<IJarvisEventAggregator>().Subscribe(e.Instance);
        }
    }
}