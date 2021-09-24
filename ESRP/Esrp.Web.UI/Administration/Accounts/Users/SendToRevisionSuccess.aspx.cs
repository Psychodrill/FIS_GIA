using System;
using Esrp.Core;

namespace Esrp.Web.Administration.Accounts.Users
{
    public partial class SendToRevisionSuccess : System.Web.UI.Page
    {
		protected String GetUserKeyCode()
		{
			if (String.IsNullOrEmpty(Request["UserKey"]))
				return "";
			return Request["UserKey"];
		}

        private const string ErrorUserNotFound = "Пользователь \"{0}\" не найден";
        private const string LoginQueryKey = "login";
        UserAccount mCurrentUser;

        public UserAccount CurrentUser
        {
            get
            {
                if (mCurrentUser == null)
                    mCurrentUser = UserAccount.GetUserAccount(Login);

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

        }
    }
}
