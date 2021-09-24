using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;

namespace Core
{
    public static class Config
    {
        public static string TempDirPath()
        {
            string tmp = (GetProperty("TempDirPath") == null ? string.Empty : GetProperty("TempDirPath")).Replace(@"/", @"\").Trim();
            if (tmp.Length == 0)
            {
                return tmp;
            }

            return tmp[tmp.Length - 1] != '\\' ? tmp + "\\" : tmp;
        }

        public static string LogFileName()
        {
            return GetProperty("LogFileName").Trim();
        }

        public static string Source2010ConnectionString()
        {
            return GetProperty("Source2010ConnectionString");
        }

        public static string Source2011ConnectionString()
        {
            return GetProperty("Source2011ConnectionString");
        }

        public static string EsrpConnectionString()
        {
            return GetProperty("EsrpConnectionString");
        }

        public static string DestConnectionString()
        {
            return GetProperty("DestConnectionString");
        }

        public static string ValueDelimeter()
        {
            return GetProperty("ValueDelimeter");
        }

        public static int CommandTimeout()
        {
            return Convert.ToInt32(GetProperty("CommandTimeout").ToString());
        }

        public static int UpdateFlushRowsCount()
        {
            return Convert.ToInt32(GetProperty("UpdateFlushRowsCount"));
        }

        public static int InsertFlushRowsCount()
        {
            return Convert.ToInt32(GetProperty("InsertFlushRowsCount"));
        }

        public static int DeleteFlushRowsCount()
        {
            return Convert.ToInt32(GetProperty("DeleteFlushRowsCount"));
        }

        private static string GetProperty(string propertyName)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("App.config");

            XmlNodeList lst = xDoc.SelectNodes(string.Format("./configuration/appSettings/add[@key='{0}']", propertyName));
            if (lst.Count > 0)
            {
                return lst[0].Attributes["value"].Value;
            }
            else
            {
                return null;
            }

        }
    }
}
