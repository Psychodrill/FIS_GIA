using System;
using System.IO;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Esrp.Core;

namespace Esrp.Web.Administration.Accounts.Support
{
    public partial class Edit : BasePage
    {
        private const string ErrorUserNotFound = "Пользователь \"{0}\" не найден";
        private const string LoginQueryKey = "login";
        SupportAccount mCurrentUser;

        public SupportAccount CurrentUser
        {
            get
            {
                if (mCurrentUser == null)
                    mCurrentUser = SupportAccount.GetSupportAccount(Login);

                if (mCurrentUser == null)
                    throw new NullReferenceException(String.Format(ErrorUserNotFound, Login));

                // Если форма отправлена, заполню поля пользователя из контролов
                if (Page.IsPostBack)
                    DataBindCurrenUser();

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
            if (Page.IsPostBack)
                return;

            // Заполню соответствующие контролы при первом обращении к странице
            txtEmail.Text = CurrentUser.Email;
            txtPhone.Text = CurrentUser.Phone;
            txtFirstName.Text = CurrentUser.FirstName;
            txtLastName.Text = CurrentUser.LastName;
            txtPatronymicName.Text = CurrentUser.PatronymicName;
            //txtIpAddresses.Text = CurrentUser.GetIpAddressesAsEdit();
            //cbNoFixedIp.Checked = !CurrentUser.HasFixedIp;

            // Нажатие на кнопку не должно инициировать проверку изменений на форме
            btnUpdate.Attributes.Add("handleClick", "false");

            // Установлю заголовок страницы
            this.PageTitle = string.Format("Редактирование “{0}”", CurrentUser.Login);
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            // Проверю валидность контролов страницы
            if (!Page.IsValid)
                return;

            // Обновлю информацию нового пользователя
            CurrentUser.Update();

            // Выполню действия после успешной регистрации
            ProcessSuccess();
        }

        private void DataBindCurrenUser()
        {
            // Заполню поля пользователя из соответствующих контролов
            mCurrentUser.Email = txtEmail.Text.Trim();
            //mCurrentUser.SetIpAddressesAsEdit(txtIpAddresses.Text);
            mCurrentUser.Phone = txtPhone.Text.Trim();
            mCurrentUser.FirstName = txtFirstName.Text.Trim();
            mCurrentUser.LastName = txtLastName.Text.Trim();
            mCurrentUser.PatronymicName = txtPatronymicName.Text.Trim();
            //mCurrentUser.HasFixedIp = !cbNoFixedIp.Checked;
        }


        private void ProcessSuccess()
        {
            // Остаемся на текущей странице
            Response.Redirect(CurrentUrl, true);
        }
    }
}
