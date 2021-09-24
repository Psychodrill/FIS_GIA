namespace Ege.Check.App.Web.Blanks.Esrp
{
    using System.Configuration;

    internal class EsrpSettings : IEsrpSettings
    {
        public string EsrpUrl
        {
            get { return ConfigurationManager.AppSettings["Esrp.Url"]; }
        }
    }
}