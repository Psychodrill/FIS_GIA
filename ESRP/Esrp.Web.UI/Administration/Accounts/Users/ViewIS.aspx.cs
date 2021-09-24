using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using Esrp.Core;
using Esrp.Web.Administration.Organizations;
using Region=Esrp.Web.Administration.Organizations.Region;

using Esrp.Core.Users;
using Esrp.Core.Organizations;
using Esrp.Core.Loggers;

namespace Esrp.Web.Administration.Accounts.Users
{
    public partial class ViewIS : BasePage
    {
        private const string ErrorUserNotFound = "Пользователь \"{0}\" не найден";
        private const string LoginQueryKey = "login";
        AdministratorAccount mCurrentUser;

        public AdministratorAccount CurrentUser
        {
            get
            {
                if (mCurrentUser == null)
                    mCurrentUser = AdministratorAccount.GetAdministratorAccountForce(Login);

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
            
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["LogUserView"]))
                {
                    AccountEventLogger.LogAccountViewEvent(Login);
                }
            // Установлю заголовок страницы)
            this.PageTitle = string.Format("Регистрационные данные “{0}”", CurrentUser.Login);
        }
    }
}
