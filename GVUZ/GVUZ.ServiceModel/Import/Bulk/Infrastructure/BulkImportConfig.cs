using System;
using System.Configuration;

namespace GVUZ.ServiceModel.Import.Bulk.Infrastructure
{
    public static class BulkImportConfig
    {
        public static int? ImportApplicationsPackageSize
        {
            get
            {
                return ConfigurationManager.AppSettings["ImportApplicationsPackageSize"] != null ?
                    Int32.Parse(ConfigurationManager.AppSettings["ImportApplicationsPackageSize"]) : (int?)null;
            }
        }
    }

    public enum ImportType
    {
        Bulk = 0,
        Ef = 1,
        Xml = 2
    }
}
