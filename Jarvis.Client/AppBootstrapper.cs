using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using Autofac;
using Caliburn.Micro.Autofac;
using Jarvis.Core;
using Application = System.Windows.Application;

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
            builder.RegisterInstance(this.Application).As<Application>();
        }

        protected override void OnStartup(object sender, StartupEventArgs e) {
            var icon = new NotifyIcon {Text = "Jarvis", Visible = true, Icon = new Icon("SysTray.ico"),ContextMenu = new ContextMenu(new[]{new MenuItem("Close", CloseApp)})};
            base.OnStartup(sender, e);
        }

        public void CloseApp(object sender, EventArgs eventArgs) {
            this.Application.MainWindow.Close();
        }
    }
}