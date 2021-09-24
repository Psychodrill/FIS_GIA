using System;
using System.Reflection;
using System.Windows.Forms;
using EgePlatform.PackageManager.Core;

namespace Fbs.Updater
{
    public class FbsPackage : ApplicationPackage
    {
        public string SystemId { get { return "FBS"; } }

        public string Title
        {
            get
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                return (attributes[0] as AssemblyTitleAttribute).Title;
            }
        }
        public string Description
        {
            get
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                return (attributes[0] as AssemblyDescriptionAttribute).Description;
            }
        }
        public Version Version
        {
            get
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                return assembly.GetName().Version;
            }
        }

        public Task[] TestPlan
        {
            get { return new Task[0]; }
        }

        public Task[] ExecutionPlan
        {
            get { return new Task[0]; }
        }

        public bool IsExecutable(PackageConfig config)
        {
            throw new NotImplementedException();
        }

        public void Configure(PackageConfig config, Form owner)
        {
            throw new NotImplementedException();
        }
    }
}
