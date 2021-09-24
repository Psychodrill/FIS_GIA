using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace  Fbs.Core.Loggers
{
   public  class ConsoleLogger : Fbs.Core.Loggers.ILogger
    {
        public void WriteMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void WriteError(Exception ex)
        {
            Console.WriteLine("Произошла ошибка:" + ex.Message);
            Console.WriteLine("Источник:" + ex.Source);
            Console.WriteLine("Стек:" + ex.StackTrace);
        }
    }
}
