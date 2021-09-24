using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;

namespace GVUZ.CompositionExportHost
{
    public partial class CompositionServiceHost : ServiceBase
    {
        public ServiceHost serviceHost = null;
        public CompositionServiceHost()
        {
            // Name the Windows Service
            //ServiceName = "CompositionServiceHost";
        }

        public static void Main()
        {
            ServiceBase.Run(new CompositionServiceHost());
            LogHelper.InitLoging();
            LogHelper.Log.Info("Main");
        }

        // Start the Windows service.
        protected override void OnStart(string[] args)
        {
            LogHelper.InitLoging();
            LogHelper.Log.Info("Сервис запускается");
            try
            {

                //scheduler.Start();

                if (serviceHost != null)
                {
                    serviceHost.Close();
                }

                serviceHost = new ServiceHost(typeof(CompositionExportService));
                serviceHost.Open();

                LogHelper.Log.Info("Сервис запущен");
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(ex.Message, ex);
                throw;
            }
            finally
            {

            }

        }

        protected override void OnStop()
        {
            try
            {
                //scheduler.Stop();
                if (serviceHost != null)
                {
                    serviceHost.Close();
                    serviceHost = null;
                }
                LogHelper.Log.Info("Сервис остановлен");
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(ex.Message, ex);
                throw;
            }
        }
    }
}
