using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace FbsReportSender.Excel
{
    class ExcelCreator
    {
        public static string GetXmlExcelString(string reportName, List<DataTable> tables)
        {
            XmlDataViewSerialiser serialyser = new XmlDataViewSerialiser(reportName);

            foreach (DataTable table in tables)
            {
                serialyser.AddDataView(table.DefaultView);
            }
            string xmlReportData = serialyser.GetXmlAndClose();

            //string xsltPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources/CreateExcel.xsl");
            //return XSLT.GetXsltTransformation(xmlReportData, new XSLTParams(xsltPath, true));
           // return XSLT.GetString(xmlReportData, new XSLTParams(@".\Resources\CreateExcel.xsl", true));
            return XSLT.GetString(xmlReportData, new XSLTParams(DefinePath(), true));
        }

        private static string DefinePath()
        {
            string filePath = "Resources/CreateExcel.xsl";
            return Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, filePath);
        }

        public static Stream GetXmlExcelStream(string reportName, List<DataTable> tables)
        {
            XmlDataViewSerialiser serialyser = new XmlDataViewSerialiser(reportName);

            foreach (DataTable table in tables)
            {
                serialyser.AddDataView(table.DefaultView);

            }
            string xmlReportData = serialyser.GetXmlAndClose();
          //  return XSLT.GetStream(xmlReportData, new XSLTParams(@".\Resources\CreateExcel.xsl", true));
            return XSLT.GetStream(xmlReportData, new XSLTParams(DefinePath(), true));
        }
    }
}