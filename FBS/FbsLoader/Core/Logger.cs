using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Core
{
    public sealed class Logger
    {
        private static volatile Logger instance;
        private static object syncRoot = new Object();

        private Logger()
        {
            string year = DateTime.Now.Year.ToString();
            string month = DateTime.Now.Month.ToString().Length == 1 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString();
            string day = DateTime.Now.Day.ToString().Length == 1 ? "0" + DateTime.Now.Day.ToString() : DateTime.Now.Day.ToString();

            logFilePath = Config.TempDirPath() + year + "_" + month + "_" + day + "_" + Config.LogFileName();
            WriteLog(string.Empty);
        }

        public static Logger Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Logger();
                    }
                }

                return instance;
            }
        }

        private string logFilePath { get; set; }

        public void WriteLog(string message)
        {
            TextWriter tw = new StreamWriter(logFilePath, true, Encoding.Unicode);
            if (message.Length != 0)
            {
                tw.WriteLine(DateTime.Now.ToString() + ": " + message);
            }
            else
            {
                tw.WriteLine();
            }
            tw.Close();
        }

        public void WriteLog(string message, int level)
        {
            WriteLog(message.PadLeft(message.Length + level * 5, ' '));
        }
    }
}
