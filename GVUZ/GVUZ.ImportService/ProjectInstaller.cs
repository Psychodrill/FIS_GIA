using System;
using System.ComponentModel;
using System.Configuration;
using System.Reflection;


namespace GVUZ.ImportService
{
	[RunInstaller(true)]
	public partial class ProjectInstaller : System.Configuration.Install.Installer
	{
		public ProjectInstaller()
		{
			InitializeComponent();
            // 
            // serviceInstaller1
            // 
            this.serviceInstaller1.DisplayName = GetConfigurationValue("ServiceName");
            this.serviceInstaller1.ServiceName = GetConfigurationValue("ServiceName"); 
		}

        private string GetConfigurationValue(string key)
        {
            Assembly service = Assembly.GetAssembly(typeof(ProjectInstaller));
            Configuration config = ConfigurationManager.OpenExeConfiguration(service.Location);
            if (config.AppSettings.Settings[key] != null)
            {
                return config.AppSettings.Settings[key].Value;
            }
            else
            {
                throw new IndexOutOfRangeException
                    ("Settings collection does not contain the requested key: " + key);
            }
        }
	}
}
