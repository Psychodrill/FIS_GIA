using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FogSoft.Helpers;
using GVUZ.Helper.ExternalValidation;
using GVUZ.ServiceModel.Import.AppCheckProcessor;
using GVUZ.ServiceModel.Import.Bulk.Infrastructure;
using GVUZ.ServiceModel.Import.Core.Packages;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace GVUZ.ImportServiceTest.Forms
{
    public partial class ImportServiceTest : Form
    {
        public ImportServiceTest()
        {
            InitializeComponent();
        }

        private void cmdStart_Click(object sender, EventArgs e)
        {
            // Один раз стартуем сервис, чтобы он выполнил импорт первой из записей в очереди

            try
            {
                LogHelper.InitLoging();
#warning Используется IoC из устаревшей Microsoft.Practices.
                var unity = new UnityContainer();
                unity.RegisterInstance<IEgeInformationProvider>(new EgeInformationProvider());
                ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(unity));

                PackageManager.Initialize();
                BulkEntitesMapper.Initialize();
                //ProcessingManager.StartWinForms();

                LogHelper.Log.Info("Сервис импорта запущен: " + DateTime.Now);
                MessageBox.Show("Сервис импорта запущен!");
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(ex.Message, ex);
                throw;
            }
        }
    }
}
