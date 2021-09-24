using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;

namespace GVUZ.CompositionExportHost
{
    public static class LogHelper
    {
        public static readonly ILog Log = LogManager.GetLogger("GlobalLogger");

        public static void InitLoging()
        {
            InitLoging(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config"));
        }

        public static void InitLoging(string configFilePath)
        {
            XmlConfigurator.ConfigureAndWatch(new FileInfo(configFilePath));

            //GlobalContext.Properties["RawUrl"] = new RawUrl();
            //GlobalContext.Properties["UrlReferrer"] = new UrlReferrer();
            //GlobalContext.Properties["UserHostAddress"] = new UserHostAddress();
            //GlobalContext.Properties["UserHostName"] = new UserHostName();
            //GlobalContext.Properties["UserAgent"] = new UserAgent();
            GlobalContext.Properties["MachineName"] = new MachineName();
            GlobalContext.Properties["BaseAppPath"] = new BaseApplicationPath();
        }

        public static void LogCurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.IsTerminating)
            {
                if (Log.IsFatalEnabled)
                {
                    Log.Fatal("Fatal Exception (background thread)",
                                        (e.ExceptionObject as Exception) ?? new Exception(e.ExceptionObject.ToString()));
                }
            }
            else
            {
                if (Log.IsErrorEnabled)
                {
                    Log.Error("Unhandled Exception in Domain",
                                        (e.ExceptionObject as Exception) ?? new Exception(e.ExceptionObject.ToString()));
                }
            }
        }


    }

    public class GCAllocatedBytesHelper
    {
        public override string ToString()
        {
            return GC.GetTotalMemory(true).ToString();
        }
    }

    public class MachineName
    {
        public override string ToString()
        {
            return Environment.MachineName;
        }
    }

    public class ExecutingAssembly
    {
        public override string ToString()
        {
            return Assembly.GetExecutingAssembly().FullName;
        }
    }

    public class AppDomainName
    {
        public override string ToString()
        {
            return AppDomain.CurrentDomain.FriendlyName;
        }
    }

    public class BaseApplicationPath
    {
        public override string ToString()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }
    }

    public class WindowsIdentityName
    {
        public override string ToString()
        {
            return WindowsIdentity.GetCurrent().Name;
        }
    }
}
