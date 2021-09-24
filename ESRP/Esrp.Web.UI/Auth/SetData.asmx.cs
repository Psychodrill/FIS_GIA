using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Configuration;
using Esrp.Core;
using Esrp.Web.Administration.IPCheck;

using Esrp.SelfIntegration.ReplicationServer;

namespace Esrp.Web.Auth
{
    using Esrp.Utility;

    [WebService(Namespace = "urn:ersp:v1")]
    [WebServiceBinding(ConformsTo = WsiProfiles.None)]
    [System.ComponentModel.ToolboxItem(false)]
    public class SetData : System.Web.Services.WebService
    {
        [WebMethod(MessageName = "ApplyBatch")]
        public long[] ApplyBatch(XmlElement batch,  out string errorInfo)
        {
            var address = HttpContext.Current.Request.UserHostAddress;
            LogManager.Info(String.Format(@"Поступил запрос ApplyBatch с адреса {0}.", address));

            string esrpConnectionString =   GetConnectionString("Esrp.Core.Properties.Settings.EsrpConnectionString");

            CheckAddress(address);

            ESRPServer esrp = new ESRPServer(esrpConnectionString);
            LogManager.Info(string.Format("{0} - репликация начата (размер пакета данных {1})", DateTime.Now, batch.ToString().Length));
            esrp.RunReplication(batch);
            LogManager.Info(string.Format("{0} - репликация завершена", DateTime.Now));

            if (esrp.Success)
            {
                errorInfo = null;
                return null;
            }
            else
            {
                errorInfo = esrp.Exception.Message;
                return esrp.FailedIds;
            }
        }

        private static void CheckAddress(string address)
        {
            if (!IPChecker.CheckOuterSite(address))
            {
                LogManager.Info(String.Format(@"Запрос на синхронизацию данных с адреса {0} не может быть обработан, т.к. адрес не разрешен в веб конфиге", address));
                throw new Exception(String.Format(@"Запрос на синхронизацию данных с адреса {0} не может быть обработан, т.к. адрес не разрешен в веб конфиге", address));
            }
        }

        [WebMethod(MessageName = "SyncWithFis")]
        public int SyncWithFis(int maxRowCount,  out string errorInfo)
        {
            var address = HttpContext.Current.Request.UserHostAddress;
            LogManager.Info(String.Format(@"Поступил запрос SyncWithFis с адреса {0}.", address));

            string esrpConnectionString = GetConnectionString("Esrp.Core.Properties.Settings.EsrpConnectionString");
            string fisConnectionString = GetConnectionString("Fis");

            CheckAddress(address);

            FISServer fis = new FISServer(esrpConnectionString, fisConnectionString, maxRowCount);
            LogManager.Info(string.Format("{0} - синхронизация с ФИС начата", DateTime.Now));
            fis.RunSynchronization( );
            LogManager.Info(string.Format("{0} - синхронизация с ФИС завершена", DateTime.Now));

            if (fis.Success)
            {
                errorInfo = null;
            }
            else
            {
                errorInfo = fis.Exception.Message;
            }
            return fis.RemainingCount;
        }

        private string GetConnectionString(string connectionStringName)
        {
            string result = null;
            if (ConfigurationManager.ConnectionStrings[connectionStringName] != null)
            {
                result = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            }
            if (String.IsNullOrEmpty(result))
            {
                LogManager.Info("Критическая ошибка в настройках системы");
                throw new Exception("Критическая ошибка в настройках системы");
            }
            return result;
        }
    }
}
