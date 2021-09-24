using System;
using System.Configuration;
using Esrp.Integration.Common;

namespace Esrp.SelfIntegration.ReplicationClient
{
    public static class ESRPClientEntryPoint
    {
        private static bool showExceptionDetails_;
        private static Logger logger_;
        public static void Run(bool showExceptionDetails)
        {
            logger_ = new Logger("Log_ESRPToESRPReplication");

            showExceptionDetails_ = showExceptionDetails;

            string serviceUrl = ConfigurationManager.AppSettings["ESRPPrivateUrl"];
            string rowCountStr = ConfigurationManager.AppSettings["ESRPReplicationRowCount"];
            bool useEnsureCommands = StringsHelper.IsTrueString(ConfigurationManager.AppSettings["ESRPUseEnsureCommands"]);

            string connectionString = null;
            if (ConfigurationManager.ConnectionStrings["ESRP_Public"] != null)
            {
                connectionString = ConfigurationManager.ConnectionStrings["ESRP_Public"].ConnectionString;
            }

            if ((String.IsNullOrEmpty(serviceUrl))
                || (String.IsNullOrEmpty(rowCountStr))
                || (String.IsNullOrEmpty(connectionString)))
            {
                logger_.WriteLine("Не указаны необходимые настройки");
                return;
            }
            int rowCount;
            if ((!Int32.TryParse(rowCountStr, out rowCount)) || (rowCount <= 0))
            {
                logger_.WriteLine("Не указаны необходимые настройки");
                return;
            }

            string connectionError;
            if (!SqlConnectionChecker.CheckConnection(connectionString, out connectionError))
            {
                logger_.WriteLine(String.Format("Невозможно соединиться с БД ЕСРП: {0}", connectionError));
                return;
            }

            ESRPClient esrp = new ESRPClient(serviceUrl, connectionString, rowCount, useEnsureCommands);
            esrp.CriticalError += new EventHandler<ExceptionEventArgs>(ESRP_Error);
            esrp.Message += new EventHandler<MessageEventArgs>(ESRP_Message);

            logger_.WriteLine("Репликация запущена (данные будут отправлены из открытого контура ЕСРП в закрытый контур ЕСРП)");
            esrp.RunReplication();
            logger_.WriteLine("Репликация завершена");
        }

        private const bool ShowStackTraces = false;

        private static void ESRP_Error(object sender, ExceptionEventArgs e)
        {
            if (showExceptionDetails_)
            {
                logger_.WriteLine(String.Format("При репликации произошла критическая ошибка: {0} ({1})", e.Exception.Message, e.Exception.StackTrace));
            }
            else
            {
                logger_.WriteLine(String.Format("При репликации произошла критическая ошибка: {0}", e.Exception.Message));
            }
        }

        private static void ESRP_Message(object sender, MessageEventArgs e)
        {
            logger_.WriteLine(e.Message);
        }
    }
}
