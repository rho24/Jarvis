using Autofac;
using Caliburn.Micro.Autofac;
using Jarvis.Core;
using NLog;

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
            builder.RegisterType<JarvisService>().As<IJarvisService>().SingleInstance();
            builder.RegisterType<LaunchViewModel>().AsSelf();
            builder.RegisterModule<JarvisModule>();
        }
    }
}