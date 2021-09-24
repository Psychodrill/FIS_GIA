using System;
using System.Collections;
using System.Web;
using System.Web.UI;

namespace Esrp.Web.Administration.Accounts.Auditors
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
