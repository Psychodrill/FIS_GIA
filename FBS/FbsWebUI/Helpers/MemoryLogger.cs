using System;
using System.Text;
using Fbs.Core.Loggers;

namespace Fbs.Web.Helpers
{
    public class MemoryLogger : ILogger
    {
        StringBuilder log = new StringBuilder();
        public void WriteMessage(string message)
        {
            log.Append(message + "<br />");
        }

        public void WriteError(Exception ex)
        {
            log.AppendFormat("Произошла ошибка: {0}<br />", ex.Message);
            log.AppendFormat("Тип: {0}<br />", ex.GetType().ToString());
            log.AppendFormat("Источник: {0}<br />", ex.Source);
            log.AppendFormat("Стек: {0}<br />", ex.StackTrace);

            if (ex.InnerException != null)
            {
                log.AppendFormat("Произошла ошибка (внутренняя ошибка): {0}<br />", ex.InnerException.Message);
                log.AppendFormat("Тип (внутренняя ошибка): {0}<br />", ex.InnerException.GetType().ToString());
                log.AppendFormat("Источник (внутренняя ошибка): {0}<br />", ex.InnerException.Source);
            }
        }

        public string GetLog()
        {
            return log.ToString();
        }
    }
}
