using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace GVUZ.CompositionExportHost
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            ServiceBase.Run(new ServiceBase[] { new CompositionExportService() });
        }
    }
}
