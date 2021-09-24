using System;
using System.IO;
using System.Collections.Generic;

namespace Esrp.Integration.Common
{
    public class Logger
    {
        public Logger(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
                throw new ArgumentNullException("fileName");

            ToConsole = true;
            ToFile = true;

            logFilePath_ = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Logs\\" + fileName + "_" + DateTime.Now.ToShortDateString() + "_" + DateTime.Now.ToShortTimeString().Replace(":", "-") + ".txt";
        }
        public bool ToConsole { get; set; }
        public bool ToFile { get; set; }

        private string logFilePath_;

        public void WriteLine(string message)
        {
            string nowStr = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
            string line = nowStr + ": " + message;

            if (ToConsole)
            {
                System.Console.WriteLine(line);
            }
            if (ToFile)
            {
                AppendAllLines(logFilePath_, new string[] { line });
            }
        }

        private void AppendAllLines(string path, IEnumerable<string> lines)
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }
                using (var writer = new StreamWriter(path, true))
                {
                    foreach (var line in lines)
                    {
                        writer.WriteLine(line);
                    }
                }
            }
            catch
            {
            }
        }
    }
}
