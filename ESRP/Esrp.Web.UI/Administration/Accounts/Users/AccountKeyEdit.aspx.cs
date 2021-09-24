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
using System.Xml.Linq;
using Esrp.Core;

namespace Esrp.Web.Administration.Accounts.Users
{
    public partial class AccountKeyEdit : BasePage
    {

        #region Constants & Fields

        private const string SuccessUri = "/Administration/Accounts/Users/AccountKeyEditSuccess.aspx?login={0}&key={1}&UserKey={2}";
        private const string LoginQueryKey = "login";
        private const string AccountKeyCodeQueryKey = "key";
        private const string ErrorAccountKeyNotFound = "Ключ \"{0}\" не найден";
        private AccountKey mCurrentAccountKey;
        private const string ErrorUserNotFound = "Пользователь \"{0}\" не найден";
        private string mCurrentUserLogin;

        #endregion

        #region Properties

        public string Login
        {
            get
            {
                if (string.IsNullOrEmpty(Request.QueryString[LoginQueryKey]))
                    return string.Empty;
                return Request.QueryString[LoginQueryKey];
            }
        }

        private string AccountKeyCode
        {
            get
            {
                if (string.IsNullOrEmpty(Request.QueryString[AccountKeyCodeQueryKey]))
                    return string.Empty;
                return Request.QueryString[AccountKeyCodeQueryKey];
            }
        }

        public AccountKey CurrentAccountKey
        {
            get
            {
                if (mCurrentAccountKey == null)
                    if ((mCurrentAccountKey = AccountKey.GetAccountKey(Login ,AccountKeyCode)) == null)
                        throw new NullReferenceException(String.Format(ErrorAccountKeyNotFound,
                            AccountKeyCode));

                if (Page.IsPostBack)
                    DataBindCurrentAccountKey();

                return mCurrentAccountKey;
            }
        }

        /// <summary>
        /// Gets CurrentUser.
        /// </summary>
        /// <exception cref="NullReferenceException">
        /// </exception>
        public string CurrentUserLogin
        {
            get
            {
                if (this.mCurrentUserLogin == null)
                {
                    this.mCurrentUserLogin = AdministratorAccount.GetAdministratorAccountForce(this.Login).Login;
                }

                if (this.mCurrentUserLogin == null)
                {
                    this.mCurrentUserLogin = UserAccount.GetUserAccount(this.Login).Login;
                }

                if (this.mCurrentUserLogin == null)
                {
                    throw new NullReferenceException(string.Format(ErrorUserNotFound, this.Login));
                }

                return this.mCurrentUserLogin;
            }
        }

        #endregion

        #region Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
                return;
            if (Request.UrlReferrer != null && Request.UrlReferrer.LocalPath!=null)
            {
                if (Request.UrlReferrer.LocalPath.Contains("ListIS.aspx"))
                {
                    Session["BackLink.HRef"] = Request.UrlReferrer.ToString();
                }
                BackLink.HRef = (string)Session["BackLink.HRef"];
            }

            // Установлю заголовок страницы
            this.PageTitle = string.Format("Редактирование ключа “{0}”", Login, CurrentAccountKey.Key);

            // Заполню контролы
            txtDateFrom.Value = CurrentAccountKey.DateFrom;
            txtDateTo.Value = CurrentAccountKey.DateTo ;
            cbIsActive.Checked = CurrentAccountKey.IsActive;
        }

        protected String GetUserKeyCode()
        {
            if (String.IsNullOrEmpty(Request["UserKey"]))
                return "";
            return Request["UserKey"];
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (Page.IsValid)
            {
                CurrentAccountKey.Update();
                Response.Redirect(String.Format(SuccessUri, Login, AccountKeyCode, GetUserKeyCode()), true);
            }
        }

        private void DataBindCurrentAccountKey()
        {
            mCurrentAccountKey.DateFrom = txtDateFrom.Value;
            mCurrentAccountKey.DateTo = txtDateTo.Value;
            mCurrentAccountKey.IsActive = cbIsActive.Checked;
        }

        private DateTime? ConvertToDate(string value)
        {
            DateTime date;
            return DateTime.TryParse(value, out date) ? (DateTime?)date : null;
        }

        #endregion
    }
}
