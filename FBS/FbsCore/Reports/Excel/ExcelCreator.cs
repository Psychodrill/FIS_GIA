using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Fbs.Core.Reports.Excel
{
    class ExcelCreator
    {
        public static Stream GetXmlExcelStream(string reportName, List<DataTableWithTag> tables,string xsltPath)
        {
            XmlDataViewSerialiser serialyser = new XmlDataViewSerialiser(reportName);

            if (tables.Count == 0)
                throw new Exception("Не обнаружено ни одного отчета для отправки");

            foreach (DataTableWithTag DT in tables)
            {
                serialyser.AddTable(DT);
            }

            string xmlReportData = serialyser.GetXmlAndClose();
            return XSLT.GetStream(xmlReportData, xsltPath);
        }
    }
}