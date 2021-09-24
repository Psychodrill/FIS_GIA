using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FbsHashedCNEsReplicator
{
   public static  class CSVFormatResolver
    {
       public enum CSVFormat
       {
           CNEsImport,DeniedCNEsImport,Unrecognized
       }

       public static CSVFormat ResolveFormat(string fileName)
       {
           CSVFormat Result = CSVFormat.Unrecognized;

           FileStream SourceFileStream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
           using (StreamReader FileReader = new StreamReader(SourceFileStream, Encoding.GetEncoding(1251)))
           {
               string CSVLine;
               if ((CSVLine = FileReader.ReadLine()) != null)
               {
                   int LinePartsLength = CSVLine.Split('#').Length;
                   if (LinePartsLength == Consts.DeniedCNEsCSVFieldsCount)
                   {
                      Result= CSVFormat.DeniedCNEsImport;
                   }
                   else if (LinePartsLength == Consts. CNEsCSVFieldsCount)
                   {
                       Result = CSVFormat.CNEsImport;
                   }
               }
           }

           SourceFileStream.Close();

           return Result;
       }
    }
}
