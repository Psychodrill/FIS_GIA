namespace Esrp.Core
{
    using System;

    using Esrp.Utility; 

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
            var sender = new EmailSender(message);

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
            var sender = new EmailSender(message);

            try
            {
                sender.Send();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return string.Empty;
        }
    }
}
