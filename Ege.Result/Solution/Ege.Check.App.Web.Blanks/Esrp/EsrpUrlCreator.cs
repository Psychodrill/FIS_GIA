namespace Ege.Check.App.Web.Blanks.Esrp
{
    using System.Text;
    using global::Esrp;
    using JetBrains.Annotations;

    internal class EsrpUrlCreator : IEsrpUrlCreator
    {
        private const string SystemIdKey = "sid";
        private const string TypeAuthKey = "ra";
        private const string UrlKey = "rp";

        [NotNull]private readonly IEsrpSettings _esrpSettings;

        public EsrpUrlCreator([NotNull]IEsrpSettings esrpSettings)
        {
            _esrpSettings = esrpSettings;
        }

        public string Login(string returnUrl)
        {
            return CreateUrl(returnUrl, AuthorizationType.Logon, BlanksSystemId.SystemId);
        }

        public string Logout(string returnUrl)
        {
            return CreateUrl(returnUrl, AuthorizationType.Logout, BlanksSystemId.SystemId);
        }

        private string CreateUrl(string returnUrl, AuthorizationType authtype, SystemId systemId)
        {
            var sb = new StringBuilder(_esrpSettings.EsrpUrl);
            sb.AppendFormat("?{0}={1}", TypeAuthKey, (int) authtype);
            sb.AppendFormat("&{0}={1}", SystemIdKey, (int) systemId);
            sb.AppendFormat("&{0}={1}", UrlKey, returnUrl);
            return sb.ToString();
        }
    }
}
