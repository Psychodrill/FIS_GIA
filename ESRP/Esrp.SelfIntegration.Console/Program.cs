using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Esrp.SelfIntegration.ReplicationClient;

namespace Esrp.SelfIntegration.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            ESRPClientEntryPoint.Run(false);
            //FISClientEntryPoint.Run(false);

            System.Threading.Thread.Sleep(60 * 1000);            
        }
    }
}
