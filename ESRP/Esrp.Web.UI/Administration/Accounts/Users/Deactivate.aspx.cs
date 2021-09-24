using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Esrp.Core;
using Esrp.Core.Common;
using Esrp.Core.Users;


namespace Esrp.Web.Administration.Accounts.Users
{
    public partial class Deactivate : System.Web.UI.Page
    {

        #region Constants & Fields

        private const string SuccessUri = "/Administration/Accounts/Users/DeactivateSuccess.aspx?login={0}&UserKey={1}";
        UserAccount mCurrentUser;
        OrgUser mCurrentOrgUser;

        #endregion

        #region Properties

        private string _errorMessage;
        protected String GetUserKeyCode()
        {
            if (String.IsNullOrEmpty(Request["UserKey"]))
                return "";
            return Request["UserKey"];
        }


        public UserAccount CurrentUser
        {
            get
            {
                if (mCurrentUser == null)
                    mCurrentUser = UserAccount.GetUserAccount(Login);

                if (mCurrentUser == null)
                    throw new NullReferenceException(String.Format("Пользователь \"{0}\" не найден",
                        Login));

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
                if (string.IsNullOrEmpty(Request.QueryString["login"]))
                    return string.Empty;

                return Request.QueryString["login"];
            }
        }

        #endregion

        #region Methods


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
                }

                BackLink.HRef = (string)Session["BackLink.HRef"];
            }
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            ManageUI();
        }

        private void ManageUI()
        {
            pErrorMessage.Visible = false;
            if (!String.IsNullOrEmpty(_errorMessage))
            {
                pErrorMessage.Visible = true;
                pErrorMessage.InnerText = _errorMessage;
            }
        }

        protected void btnDeactivate_Click(object sender, EventArgs e)
        {
            // проверю валидность контролов страницы
            if (!Page.IsValid)
                return;

            try
            {
                // отключу пользователя
                CurrentUser.Deactivate(txtCause.Text.Trim());

            }
            catch (DbException exc)
            {
                _errorMessage = exc.Message;
            }

            if (String.IsNullOrEmpty(_errorMessage))
            {
                // переход на страницу указания причины
                Response.Redirect(String.Format(SuccessUri, Login, GetUserKeyCode()), true);
            }
        }

        #endregion
    }
}
