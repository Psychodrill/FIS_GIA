using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GVUZ.ServiceModel.Import.Schemas
{
   public static class XmlPreparations
    {
       public static string Prepare(string originalXml)
       {
           StringBuilder packageDataReplacer = new StringBuilder(originalXml);
           packageDataReplacer = packageDataReplacer
                     .Replace("<FinSourceAndEduForm>", "<FinSourceEduForm>")
                     .Replace("</FinSourceAndEduForm>", "</FinSourceEduForm>")
                     .Replace("<EducationalFormID>", "<EducationFormID>")
                     .Replace("</EducationalFormID>", "</EducationFormID>");
           return packageDataReplacer.ToString();
       }
    }
}
