using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace ReplicationService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (args != null && args.Length > 0 && args[0].Length > 1
               && (args[0][0] == '-' || args[0][0] == '/'))
            {
                switch (args[0].Substring(1).ToLower())
                {
                    case "install":
                    case "i":
                        if (!ServiceInstallerUtility.Install())
                            Console.WriteLine("Не удалось инсталлировать сервис");
                        break;
                    case "uninstall":
                    case "u":
                        if (!ServiceInstallerUtility.Uninstall())
                            Console.WriteLine("Не удалось деинсталлировать сервис");
                        break;
                    default:
                        Console.WriteLine("Unrecognized parameters.");
                        break;
                }
            }
            else
            {
                try
                {
                    ServiceBase[] ServicesToRun;
                    ServicesToRun = new ServiceBase[]
                {
                    new EsrpReplicationService()
                };
                    ServiceBase.Run(ServicesToRun);
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry("Application", ex.ToString(), EventLogEntryType.Error);
                }
            }
        }
    }
}
