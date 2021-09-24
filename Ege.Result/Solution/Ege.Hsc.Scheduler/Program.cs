namespace Ege.Hsc.Scheduler
{
    using System;
    using System.ServiceProcess;

    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            var scheduler = new Scheduler();
            if (Environment.UserInteractive)
            {
                Console.WriteLine("Start");
                scheduler.RunConsole();
            }
            else
            {
                var servicesToRun = new ServiceBase[]
                    {
                        scheduler
                    };
                ServiceBase.Run(servicesToRun);
            }
        }
    }
}
