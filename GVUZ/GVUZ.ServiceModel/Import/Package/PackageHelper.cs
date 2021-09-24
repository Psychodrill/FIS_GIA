using System;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;

namespace GVUZ.ServiceModel.Import.Package
{
    public static class PackageHelper
    {
        public static void GenerateXmlPackage<T>(string fileName, T packageData)
        {
            var xmlSerlzr = new XmlSerializer(typeof (T));
            try
            {
                using (var fs = new FileStream(fileName, FileMode.CreateNew))
                {
                    xmlSerlzr.Serialize(fs, packageData);
                    fs.Close();
                }
            }
            catch (Exception)
            {
                if (File.Exists(fileName))
                    File.Delete(fileName);
                throw;
            }
        }

        public static string GenerateXmlPackageIntoString<T>(T packageData, string rootName = null)
        {
            XmlSerializer xmlSerlzr;
            if (rootName == null)
                xmlSerlzr = new XmlSerializer(typeof (T));
            else
                xmlSerlzr = new XmlSerializer(typeof (T), new XmlRootAttribute(rootName));

            var dtfi = new DateTimeFormatInfo {FullDateTimePattern = "yyyy-MM-ddTHH:mm:ss"};

            using (var fs = new StringWriter(dtfi))
            {
                xmlSerlzr.Serialize(fs, packageData);
                fs.Close();
                return fs.ToString();
            }
        }
    }
}