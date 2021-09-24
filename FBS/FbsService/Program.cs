using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace FbsService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                ///Запускаем как обычное приложение, если указан ключ /p
                if (args[0].Contains("p"))
                {
                    RunAsProgram();
                }
            }
            RunAsService();
        }

        private static void RunAsProgram()
        {
            using(TaskService taskService = new TaskService())
            {
                TaskManager.Instance().BeginExecuteTasks();
                while (true)
                {
                }
            }
        }

        private static void RunAsService()
        {
            ServiceBase[] servicesToRun = new ServiceBase[]
                                              {
                                                  new TaskService()
                                              };
            ServiceBase.Run(servicesToRun);
        }
    }

    /// <summary>
    /// Модуль функций, упрощающих выдачу сообщений.
    /// </summary>
    internal sealed class LogManager
    {
        private LogManager() { }  // Неинстанциируемый класс

        /// <summary> Получение логгера для заданного типа лога. </summary>
        /// <param name="type"> Тип лога, см. перечисление LogType. </param>
        /// <returns> Логгер. </returns>
        public static log4net.ILog GetLogger(string name)
        {
            return log4net.LogManager.GetLogger(name);
        }

        /// <summary> Печать информационного сообщения. </summary>
        public static void Info(string name, string message)
        {
            LogManager.GetLogger(name).Info(message);
        }

        /// <summary> Печать информационного сообщения с форматированием. </summary>
        public static void InfoFormat(string name, string format, object arg0)
        {
            LogManager.GetLogger(name).InfoFormat(format, arg0);
        }

        /// <summary> Печать информационного сообщения с форматированием. </summary>
        public static void InfoFormat(string name, string format, params object[] args)
        {
            LogManager.GetLogger(name).InfoFormat(format, args);
        }

        /// <summary> Печать предупреждения. </summary>
        /// <param name="message">Строка для печати</param>
        public static void Warn(string name, string message)
        {
            LogManager.GetLogger(name).Warn(message);
        }

        /// <summary> Печать предупреждения с форматированием. </summary>
        public static void WarnFormat(string name, string format, object arg0)
        {
            LogManager.GetLogger(name).WarnFormat(format, arg0);
        }

        /// <summary> Печать предупреждения с форматированием. </summary>
        public static void WarnFormat(string name, string format, params object[] args)
        {
            LogManager.GetLogger(name).WarnFormat(format, args);
        }

        /// <summary> Печать сообщения об ошибке. </summary>
        /// <param name="ex">Объект-исключение</param>
        public static void Error(string name, Exception ex)
        {
            LogManager.GetLogger(name).Error(ex);
        }
    }
 
}
