using System;
using System.IO;

namespace FBS.Common
{
    public static class Logger
    {
        private static readonly string LogFileDateTimePrefix = DateTime.Now.ToShortDateString() + "_" + DateTime.Now.ToShortTimeString().Replace(":", "-");
        private static readonly string LogFilePathPrefix = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\CompFixLog";

        private static string LogFilePath
        {
            get
            {
                return String.Format("{0}_{1}_{2}.txt", LogFilePathPrefix, Prefix, LogFileDateTimePrefix);
            }
        }

        private static string Prefix = "Common";
        public static void SetPrefix(string prefix)
        {
            if (String.IsNullOrEmpty(prefix))
            {
                Prefix = "Common";
            }
            else
            {
                Prefix = prefix;
            }
        }

        public static void WriteLine(string message, bool withDateTime = true)
        {
            string line;
            if (withDateTime)
            {
                string nowStr = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
                line = nowStr + ": " + message;
            }
            else
            {
                line = message;
            }
            Console.WriteLine(line);
            try
            {
                File.AppendAllLines(LogFilePath, new string[] { line });
            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("ОШИБКА ЗАПИСИ ЛОГА: {0}", ex.Message));
            }
        }

        public static class DetailedLogger
        {
            private static readonly string DetailedLogFileDateTimePrefix = DateTime.Now.ToShortDateString() + "_" + DateTime.Now.ToShortTimeString().Replace(":", "-");
            private static readonly string DetailedLogFilePathPrefix = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\ReplicationLogDetailed";

            private static string DetailedLogFilePath
            {
                get
                {
                    return String.Format("{0}_{1}_{2}.txt", DetailedLogFilePathPrefix, Logger.Prefix, DetailedLogFileDateTimePrefix);
                }
            }

            public static bool Enabled { get; set; }

            public static void WriteLine(string message)
            {
                if (!Enabled)
                    return;

                string nowStr = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
                string line = nowStr + ": " + message;
                Console.WriteLine(line);

                try
                {
                    File.AppendAllLines(DetailedLogFilePath, new string[] { line });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(String.Format("ОШИБКА ЗАПИСИ ЛОГА: {0}", ex.Message));
                }
            }
        }
    }
}
