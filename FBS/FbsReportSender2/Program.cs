using System;
using FbsReportSender.Email;

namespace FbsReportSender
{
    class Program
    {
        /// <summary>
        /// /w - weekly
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            try
            {
                EmailScheduleTypeEnum schedule = EmailScheduleTypeEnum.Daily;
                if (args.Length > 0 && args[0].Contains("w"))
                {
                    schedule = EmailScheduleTypeEnum.Weekly;    
                }

                LogMessage("Начало формирования.");
                var message = new EMailMessageViewReports(schedule);
                LogMessage("Начало отправки.");

                EMailSender.Send(message);
                LogMessage("Сообщение успешно отправилось.");
            }
            catch(Exception exp)
            {
                LogError(exp);
                
            }

            //Чтобы успеть глянуть сообщение об ошибке
            System.Threading.Thread.Sleep(25000);
        }

        internal static void LogMessage(string message)
        {
            Console.WriteLine(message);
        }

        internal static void LogError(Exception exp)
        {
            Console.WriteLine("Произошла ошибка:" + exp.Message);
            Console.WriteLine("Источник:" + exp.Source);
            Console.WriteLine("Стек:" + exp.StackTrace);
        }
    }
}
