using Caliburn.Micro.Autofac;

namespace Jarvis.Client
{
    public class AppBootstrapper : AutofacBootstrapper<ShellViewModel>
    {
        protected override void ConfigureBootstrapper()
        {
            //  you must call the base version first!
            base.ConfigureBootstrapper();

            //  override namespace naming convention
            EnforceNamespaceConvention = false;
            //  change our view model base type
            ViewModelBaseType = typeof(IShell);
        }
    }
}