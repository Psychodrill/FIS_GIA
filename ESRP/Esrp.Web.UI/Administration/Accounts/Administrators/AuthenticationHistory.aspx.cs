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
using Esrp.Core;

namespace Esrp.Web.Administration.Accounts.Administrators
{
    public partial class AuthenticationHistory : BasePage
    {
        private const string LoginQueryKey = "login";
        private const string ErrorUserNotFound = "Пользователь \"{0}\" не найден";
        private const string TitleFormat = "История аутентификаций “{0}”";

        public string Login
        {
            get
            {
                if (string.IsNullOrEmpty(Request.QueryString[LoginQueryKey]))
                    return string.Empty;

                return Request.QueryString[LoginQueryKey];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Проверю соответствие типа запрашиваемого аккаунта
            if (Account.GetType(Login) != typeof(AdministratorAccount))
                throw new NullReferenceException(String.Format(ErrorUserNotFound, Login));
               
            // Установлю заголовок страницы
            this.PageTitle = string.Format(TitleFormat, Login);
        }
    }
}
