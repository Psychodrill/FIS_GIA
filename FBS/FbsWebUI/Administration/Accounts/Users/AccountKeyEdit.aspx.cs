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
using Fbs.Core;

namespace Fbs.Web.Administration.Accounts.Users
{
    public partial class AccountKeyEdit : BasePage
    {
        private const string SuccessUri = "/Administration/Accounts/Users/AccountKeyEditSuccess.aspx?login={0}&key={1}";
        private const string LoginQueryKey = "login";
        private const string AccountKeyCodeQueryKey = "key";
        private const string ErrorAccountKeyNotFound = "Ключ \"{0}\" не найден";

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

        private AccountKey mCurrentAccountKey;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
                return;

            // Установлю заголовок страницы
            this.PageTitle = string.Format("Редактирование ключа “{0}”", Login, CurrentAccountKey.Key);

            // Заполню контролы
            txtDateFrom.Text = CurrentAccountKey.DateFrom == null ?
                String.Empty : ((DateTime)CurrentAccountKey.DateFrom).ToShortDateString();
            txtDateTo.Text = CurrentAccountKey.DateTo == null ?
                String.Empty : ((DateTime)CurrentAccountKey.DateTo).ToShortDateString();
            cbIsActive.Checked = CurrentAccountKey.IsActive;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (Page.IsValid)
            {
                CurrentAccountKey.Update();
                Response.Redirect(String.Format(SuccessUri, Login, AccountKeyCode), true);
            }
        }

        private void DataBindCurrentAccountKey()
        {
            mCurrentAccountKey.DateFrom = ConvertToDate(txtDateFrom.Text);
            mCurrentAccountKey.DateTo = ConvertToDate(txtDateTo.Text);
            mCurrentAccountKey.IsActive = cbIsActive.Checked;
        }

        private DateTime? ConvertToDate(string value)
        {
            DateTime date;
            return DateTime.TryParse(value, out date) ? (DateTime?)date : null;
        }
    }
}
