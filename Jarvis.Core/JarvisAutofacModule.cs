using System;
using System.IO;
using System.Reflection;
using Autofac;
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

            builder.RegisterType<SynchronousScheduler>().As<IScheduler>().SingleInstance();

            builder.RegisterType<JarvisOptionsSource>().AsSelf();
            builder.RegisterType<IndexedDirectoriesSource>().AsSelf();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly(), Assembly.GetEntryAssembly())
                .AssignableTo<IJarvisModule>().AsImplementedInterfaces()
                .AssignableTo<ISubOptionsProvider>().AsImplementedInterfaces();
        }
    }
}