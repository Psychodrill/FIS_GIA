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

namespace Esrp.Web.Administration.Accounts.SupportUsers
{
    public partial class History : BasePage
    {
        public string Login
        {
            get
            {
                if (string.IsNullOrEmpty(Request.QueryString["login"]))
                    return string.Empty;

                return Request.QueryString["login"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Установлю заголовок страницы
            this.PageTitle = string.Format("История изменений “{0}”", Login);
        }
    }
}
