using System;
using System.Configuration;
using Esrp.Integration.Common;

namespace Esrp.SelfIntegration.ReplicationClient
{
    public static class FISClientEntryPoint
    {
        private static bool showExceptionDetails_; 
        private static Logger logger_;
        public static void Run(bool showExceptionDetails )
        {
            logger_ = new Logger("Log_ESRPToFISReplication");

            showExceptionDetails_ = showExceptionDetails;

            string serviceUrl = ConfigurationManager.AppSettings["ESRPPrivateUrl"];
             string rowCountStr = ConfigurationManager.AppSettings["FISReplicationRowCount"];

            if ((String.IsNullOrEmpty(serviceUrl))
                || (String.IsNullOrEmpty(rowCountStr)))
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

            FISClient fis = new FISClient(serviceUrl, rowCount);
            fis.CriticalError += new EventHandler<ExceptionEventArgs>(FIS_Error);
            fis.Message += new EventHandler<MessageEventArgs>(FIS_Message);

            logger_.WriteLine("Репликация запущена (данные будут отправлены из закрытого контура ЕСРП в ФИС)");
            fis.RunReplication();
            logger_.WriteLine("Репликация завершена");
        }

        private const bool ShowStackTraces = false;

        private static void FIS_Error(object sender, ExceptionEventArgs e)
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

        private static void FIS_Message(object sender, MessageEventArgs e)
        {
            logger_.WriteLine(e.Message);
        }
    }
}
