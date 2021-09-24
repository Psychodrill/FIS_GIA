using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;
using System.Reflection;

namespace ReplicationService
{
    [RunInstaller(true)]
    public class ServiceInstallerUtility : Installer
    {
        private static readonly string exePath = Assembly.GetExecutingAssembly().Location;

        private readonly ServiceInstaller serviceInstaller;
        private readonly ServiceProcessInstaller processInstaller;

        public ServiceInstallerUtility()
        {
            this.processInstaller = new ServiceProcessInstaller();
            this.serviceInstaller = new ServiceInstaller();

            this.processInstaller.Account = ServiceAccount.LocalSystem;
            this.serviceInstaller.StartType = ServiceStartMode.Manual;
            this.serviceInstaller.ServiceName = "ESRP Replication Service";

            Installers.Add(this.serviceInstaller);
            Installers.Add(this.processInstaller);
        }

        public static bool Install()
        {
            try { ManagedInstallerClass.InstallHelper(new[] { exePath }); }
            catch { return false; }
            return true;
        }

        public static bool Uninstall()
        {
            try { ManagedInstallerClass.InstallHelper(new[] { "/u", exePath }); }
            catch { return false; }
            return true;
        }
    }
}
