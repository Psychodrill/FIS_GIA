using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fbs.Utility; 

namespace Fbs.Core
{
    /// <summary>
    /// Менеджер действий
    /// </summary>
    public class TaskManager
    {
         /// <summary>
         /// Отправка email сообшения
         /// </summary>
         /// <param name="message">Сообщение</param>
        public static void SendEmail(EmailMessage message)
        {
            EmailSender sender = new EmailSender(message);

            try
            {
                sender.Send();
            }
            catch (Exception ex)
            {
                // TODO: Временное решение! 
                // Не показываем пользователю ошибки, связанные с отправкой сообщений
                LogManager.Error(ex);
            }
        }

        public static string  SendEmailWithLog(EmailMessage message)
        {
            EmailSender sender = new EmailSender(message);

            try
            {
                sender.Send();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "";
        }
    }
}
