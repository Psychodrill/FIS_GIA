using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Fbs.Core
{
    public static class DBSettings
    {
        public static string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["Fbs.Core.Properties.Settings.FbsConnectionString"].ToString(); }
        }

        public static string ConnectionString_ForHashedBase
        {
            get { return ConfigurationManager.ConnectionStrings["Fbs.Core.Properties.Settings.FbsConnectionStringHashed"].ToString(); }
        }
    }
}
