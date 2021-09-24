using System;

namespace Esrp.Web.Administration.Accounts.Administrators
{
    public partial class AccountKeyList : BasePage
    {
		protected String GetUserKeyCode()
		{
			if (String.IsNullOrEmpty(Request["UserKey"]))
				return "";
			return Request["UserKey"];
		}

        private const string LoginQueryKey = "login";

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
            this.PageTitle = String.Format("Ключи доступа “{0}”", Login); 
        }
    }
}
