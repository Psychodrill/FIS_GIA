using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Esrp.Web.Administration.Accounts.Administrators
{
    public partial class HistoryVersion : BasePage
    {
        public string Version
        {
            get
            {
                if (string.IsNullOrEmpty(Request.QueryString["version"]))
                    return string.Empty;

                return Request.QueryString["version"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Установлю заголовок страницы
            this.PageTitle = string.Format("Просмотр версии №{0}", Version);
        }
    }
}
