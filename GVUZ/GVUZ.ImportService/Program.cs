using System;
using System.ServiceProcess;
using FogSoft.Helpers;
using GVUZ.Helper.ExternalValidation;
using GVUZ.ServiceModel.Import.AppCheckProcessor;
using GVUZ.ServiceModel.Import.Bulk.Infrastructure;
using GVUZ.ServiceModel.Import.Core.Packages;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace GVUZ.ImportService
{
	static class Program
	{
	    private static void Main(string[] args)
	    {
	        if (args.Length > 0 && args[0].ToLowerInvariant().Trim() == "console")
	        {
	            Console.WriteLine("Starting...");

                try
                {
                    LogHelper.InitLoging();

                    var unity = new UnityContainer();
                    unity.RegisterInstance<IEgeInformationProvider>(new EgeInformationProvider());
                    ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(unity));

                    PackageManager.Initialize();
                    BulkEntitesMapper.Initialize();
                    //ProcessingManager.Start();

                    LogHelper.Log.Info("Сервис импорта запущен: " + DateTime.Now);
                }
                catch (Exception ex)
                {
                    LogHelper.Log.Error(ex.Message, ex);
                    throw;
                }

	            Console.WriteLine("Started, press any key to terminate...");
	            
                Console.ReadKey();

	            Console.WriteLine("Stopping...");
                //ProcessingManager.Stop();
                LogHelper.Log.Info("Сервис импорта остановлен: " + DateTime.Now);
	            Console.WriteLine("Stopped");

	        }

	        //else
	        //{
	        //    ServiceBase[] ServicesToRun;
	        //    ServicesToRun = new ServiceBase[]
	        //        {
	        //            new ImportService()
	        //        };
	        //    ServiceBase.Run(ServicesToRun);
	        //}
	    }
	}
}
