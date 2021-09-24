namespace Esrp.Core
{
    using System.Configuration;

    public static class DBSettings
    {
        public static string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["Esrp.Core.Properties.Settings.EsrpConnectionString"].ToString(); }
        }

        public static string ConnectionString_ForHashedBase
        {
            get { return ConfigurationManager.ConnectionStrings["Esrp.Core.Properties.Settings.EsrpConnectionStringHashed"].ToString(); }
        }
    }
}
