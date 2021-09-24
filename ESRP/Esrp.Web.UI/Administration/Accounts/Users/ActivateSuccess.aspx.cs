using System;
using Esrp.Core;
using Esrp.Core.Users;

namespace Esrp.Web.Administration.Accounts.Users
{
    public partial class ActivateSuccess : System.Web.UI.Page
    {
		protected String GetUserKeyCode()
		{
			if (String.IsNullOrEmpty(Request["UserKey"]))
				return "";
			return Request["UserKey"];
		}


        private const string ErrorUserNotFound = "Пользователь \"{0}\" не найден";
        private const string LoginQueryKey = "login";
		private const string RequestIDKey = "req_id";
        OrgUser mCurrentUser;

        public OrgUser CurrentUser
        {
            get
            {
                if (mCurrentUser == null)
                    mCurrentUser = OrgUserDataAccessor.Get(this.Login);

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

        public string RequestID
        {
            get
            {
                if (string.IsNullOrEmpty(Request.QueryString[RequestIDKey]))
                    return string.Empty;

                return Request.QueryString[RequestIDKey];
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
        	AddRequestIDToContinueEditUrl();
            if (!Page.IsPostBack)
            {
                if (Request.UrlReferrer != null && Request.UrlReferrer.LocalPath!=null)
                {
                    if (Request.UrlReferrer.LocalPath.Contains("List.aspx"))
                    {
                        Session["BackLink.HRef"] = Request.UrlReferrer.ToString();
                    }
                    BackLink.HRef = (string)Session["BackLink.HRef"];
                }
            }
        }

    		private void AddRequestIDToContinueEditUrl()
    		{
    			string url = String.Format("/Administration/Accounts/Users/Edit{0}.aspx?login={1}",
    																 GetUserKeyCode(), CurrentUser.login);
    			if (!String.IsNullOrEmpty(RequestID))
    				url += "&req_id=" + RequestID;
					
    			lnkContinueEdit.HRef = url;
    		}
    }
}
