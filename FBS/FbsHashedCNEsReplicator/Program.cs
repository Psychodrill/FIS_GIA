using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FbsHashedCNEsReplicator.Properties;

namespace FbsHashedCNEsReplicator
{
    class Program
    {
        static void Main(string[] args)
        {
            Fbs.Core.Loggers.ConsoleLogger Logger = new Fbs.Core.Loggers.ConsoleLogger();
            try
            {
                Logger.WriteMessage("---Operation started");
                DirectoryInfo DI = new DirectoryInfo(Settings.Default.LoadFolder);
                foreach (FileInfo FI in DI.GetFiles("*.csv"))
                {
                    if (CSVFormatResolver.ResolveFormat(FI.FullName) == CSVFormatResolver.CSVFormat.CNEsImport)
                    {
                        CNECSVParser.ParseFile(FI.FullName, Settings.Default.DBConnection, Logger);
                    }
                    else if (CSVFormatResolver.ResolveFormat(FI.FullName) == CSVFormatResolver.CSVFormat.DeniedCNEsImport)
                    {
                        DeniedCNECSVParser.ParseFile(FI.FullName, Settings.Default.DBConnection, Logger);
                    }
                }

                Logger.WriteMessage("---Operation completed");
                
                //throw new Exception("---Operation completed");
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
            System.Threading.Thread.Sleep(60000);
        }
    }
}
