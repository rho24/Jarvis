using System;
using System.Reactive.Linq;
using System.Windows.Forms;
using Autofac;
using Caliburn.Micro.Autofac;
using Jarvis.Core;
using ManagedWinapi;

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
        }

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e) {
            base.OnStartup(sender, e);
        }
    }
}