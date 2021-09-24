using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using GVUZ.ImportService2016.Core;
using GVUZ.ImportService2016.Core.Main;
using GVUZ.ImportService2016.Core.Main.Log;

namespace GVUZ.ImportService2016
{
    public partial class ImportService2016 : ServiceBase
    {
        public ImportService2016()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                LogHelper.InitLoging();

                ProcessingManager.Start();
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
