using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Esrp.Core.Loggers
{
    public interface ILogger
    {
         void WriteMessage(string message);
         void WriteError(Exception ex);
    }
}
