using Autofac;
using Caliburn.Micro.Autofac;
using Raven.Client;
using Raven.Client.Embedded;

namespace Jarvis.Client
{
    public class AppBootstrapper : AutofacBootstrapper<ShellViewModel>
    {
        protected override void ConfigureBootstrapper() {
            //  you must call the base version first!
            base.ConfigureBootstrapper();

            //  override namespace naming convention
            EnforceNamespaceConvention = false;
            //  change our view model base type
            ViewModelBaseType = typeof (IShell);
        }

        protected override void ConfigureContainer(ContainerBuilder builder) {
            builder.Register(c => new EmbeddableDocumentStore {
                DataDirectory = "Data",
                UseEmbeddedHttpServer = true
            }.Initialize()).As<IDocumentStore>().SingleInstance();

            builder.Register(c => c.Resolve<IDocumentStore>().OpenSession())
                .As<IDocumentSession>()
                .OnRelease(d => d.Dispose());
        }
    }
}