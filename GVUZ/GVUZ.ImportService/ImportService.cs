using System;
using System.Configuration;
using System.Reflection;
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
	public partial class ImportService : ServiceBase
	{
		public ImportService()
		{
			InitializeComponent();
            this.ServiceName = ConfigurationManager.AppSettings.Get("ServiceName");
		}

		protected override void OnStart(string[] args)
		{
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
		}

		protected override void OnStop()
		{
			ProcessingManager.Stop();
            LogHelper.Log.Info("Сервис импорта остановлен: " + DateTime.Now);
		}
	}
}
