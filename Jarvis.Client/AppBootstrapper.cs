using System;
using Autofac;
using Caliburn.Micro.Autofac;
using Jarvis.Core;
using Jarvis.Core.Infrastructure;

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
            ViewModelBaseType = typeof(IShell);

            AutoSubscribeEventAggegatorHandlers = true;
        }

        protected override void ConfigureContainer(ContainerBuilder builder) {
            builder.RegisterType<JarvisEventAggregator>().As<IJarvisEventAggregator>().SingleInstance();
            builder.RegisterType<SystemTrayNotificationManager>().As<INotificationManager>().SingleInstance();
            builder.RegisterType<JarvisService>().As<IJarvisService>().SingleInstance();
            builder.RegisterType<LaunchViewModel>().AsSelf();
            builder.RegisterModule<JarvisAutofacModule>();
        }
    }
}