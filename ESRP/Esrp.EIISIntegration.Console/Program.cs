using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Esrp.EIISIntegration;
using Esrp.EIISIntegration.Import;
using Esrp.EIISIntegration.Import.Importers;
using System.Configuration;
using Esrp.Integration.Common;

namespace Esrp.EIISIntegration.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            EIISEntryPoint.Run(false);
            System.Threading.Thread.Sleep(60 * 1000);
        } 
    }
}
