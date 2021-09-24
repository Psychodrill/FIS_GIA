using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Esrp.Core;

namespace Esrp.Web.Administration.Accounts.Support
{
    public partial class View : BasePage
    {
        private const string ErrorUserNotFound = "Пользователь \"{0}\" не найден";
        private const string LoginQueryKey = "login";
        SupportAccount mCurrentUser;

        public SupportAccount CurrentUser
        {
            get
            {
                if (mCurrentUser == null)
                    mCurrentUser = SupportAccount.GetSupportAccount(Login);

                if (mCurrentUser == null)
                    throw new NullReferenceException(String.Format(ErrorUserNotFound, Login));

                return mCurrentUser;
            }
        }

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
            // Установлю заголовок страницы
            this.PageTitle = string.Format("Регистрационные данные “{0}”", CurrentUser.Login);
        }
    }
}
