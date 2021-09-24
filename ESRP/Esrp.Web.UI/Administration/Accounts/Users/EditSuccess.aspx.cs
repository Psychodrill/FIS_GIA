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
using Esrp.Core.Users;

namespace Esrp.Web.Administration.Accounts.Users
{
    public partial class EditSuccess : System.Web.UI.Page
    {
        #region Constants & Fields

        private const string ErrorUserNotFound = "Пользователь \"{0}\" не найден";
        private const string LoginQueryKey = "login";
				private const string RequestIDKey = "req_id";
        UserAccount mCurrentUser;
        private OrgUser mCurrentOrgUser;

        #endregion

        #region Properties

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

        /// <summary>
        /// Gets CurrentUser.
        /// </summary>
        /// <exception cref="NullReferenceException">
        /// </exception>
        public OrgUser CurrentOrgUser
        {
            get
            {
                if (this.mCurrentOrgUser == null)
                {
                    this.mCurrentOrgUser = OrgUserDataAccessor.Get(this.Login);
                }

                if (this.mCurrentOrgUser == null)
                {
                    throw new NullReferenceException(string.Format("Пользователь \"{0}\" не найден", this.Login));
                }

                return this.mCurrentOrgUser;
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

        #endregion

        #region Methods

        protected String GetUserKeyCode()
        {
            if (String.IsNullOrEmpty(Request["UserKey"]))
                return "";
            return Request["UserKey"];
        }


        protected void Page_Load(object sender, EventArgs e)
        {
        	AddRequestIDToContinueEditUrl();
        }

    	private void AddRequestIDToContinueEditUrl()
    	{
    		string url = String.Format("/Administration/Accounts/Users/Edit{0}.aspx?login={1}",
    																GetUserKeyCode(), CurrentUser.Login);
    		if (!String.IsNullOrEmpty(RequestID))
    			url += "&req_id=" + RequestID;
					
    		lnkContinueEdit.HRef = url;
        }

        #endregion

    }
}
