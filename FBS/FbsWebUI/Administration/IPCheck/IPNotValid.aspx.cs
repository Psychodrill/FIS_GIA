using System;

namespace Fbs.Web.Administration.IPCheck
{
    public partial class IPNotValid : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string ipAddress = Request.UserHostAddress;
            string error;
            if (IPChecker.IsAdminIP_InAllowedRage(ipAddress, out error))
                lblError.Text = string.Format("IP Адрес '{0}' разрешён для захода в раздел Администрирования.", ipAddress);
            else
            {
                lblError.Text = string.Format("IP Адрес '{0}' запрещён для захода в раздел Администрирования. {1}", ipAddress, error);
            }
        }
    }
}
