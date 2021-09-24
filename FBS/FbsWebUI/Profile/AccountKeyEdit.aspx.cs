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

namespace Fbs.Web.Profile
{
    public partial class AccountKeyEdit : BasePage
    {
        private const string SuccessUri = "/Profile/AccountKeyEditSuccess.aspx?key={0}";
        private const string ErrorAccountKeyNotFound = "Ключ \"{0}\" не найден";
        private const string AccountKeyCodeQueryKey = "key";
        private AccountKey mCurrentAccountKey;

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
                    if ((mCurrentAccountKey = AccountKey.GetAccountKey(AccountKeyCode)) == null)
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
            this.PageTitle = string.Format("Редактирование ключа “{0}”", CurrentAccountKey.Key);

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
                Response.Redirect(String.Format(SuccessUri, AccountKeyCode), true);
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
