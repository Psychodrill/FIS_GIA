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
    public partial class ChangePasswordSuccess : System.Web.UI.Page
    {
        #region Constants & Fields

        private const string ErrorUserNotFound = "Пользователь \"{0}\" не найден";
        private const string LoginQueryKey = "login";
        private UserAccount mCurrentUser;
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

        public OrgUser CurrentOrgUser
        {
            get
            {
                if (mCurrentOrgUser == null)
                    mCurrentOrgUser = OrgUserDataAccessor.Get(this.Login);

                if (mCurrentOrgUser == null)
                    throw new NullReferenceException(String.Format("Пользователь \"{0}\" не найден",
                        Login));

                return mCurrentOrgUser;
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

        #endregion
    }
}
