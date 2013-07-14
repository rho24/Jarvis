using System;
using System.IO;
using System.Reflection;
using Autofac;
using Raven.Client;
using Raven.Client.Embedded;
using Module = Autofac.Module;

namespace Jarvis.Core
{
    public class JarvisModule : Module
    {
        protected override void Load(ContainerBuilder builder) {
            var databaseDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Jarvis");

            var store = new EmbeddableDocumentStore { DataDirectory = databaseDir, UseEmbeddedHttpServer = true }.Initialize();

            builder.RegisterInstance(store).As<IDocumentStore>();

            builder.Register(c => c.Resolve<IDocumentStore>().OpenSession()).As<IDocumentSession>().OnRelease(d => d.Dispose());

            builder.RegisterType<SynchronousScheduler>().As<IScheduler>().SingleInstance();

            builder.RegisterType<JarvisOptionsSource>().AsSelf();
            builder.RegisterType<IndexedDirectorySource>().AsSelf();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AssignableTo<ISubOptionsProvider>().AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(Assembly.GetEntryAssembly()).AssignableTo<ISubOptionsProvider>().AsImplementedInterfaces();
        }
    }
}