using System;
using System.ServiceProcess;

namespace GVUZ.AppExport.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (args != null && args.Length > 0 && args[0].ToLowerInvariant() == "console")
            {
                var launcher = new ServiceLauncher();
                Console.WriteLine("Starting...");
                launcher.Start();
                Console.WriteLine("Started");
                Console.ReadKey();
                Console.WriteLine("Stopping...");
                launcher.Stop();
                Console.WriteLine("Stopped");
            }
            else
            {
                ServiceBase.Run(new ServiceApplication());    
            }
        }
    }
}
