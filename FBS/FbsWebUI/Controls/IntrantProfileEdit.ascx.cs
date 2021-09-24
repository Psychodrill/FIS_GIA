using System;
using Fbs.Core;

namespace Fbs.Web.Controls
{
    public partial class IntrantProfileEdit : System.Web.UI.UserControl
    {
        private const string SuccessUri = "/Profile/View.aspx";

        private IntrantAccount mCurrentUser;

        public IntrantAccount CurrentUser
        {
            get
            {
                if (mCurrentUser == null)
                    mCurrentUser = IntrantAccount.GetIntrantAccount(Account.ClientLogin);

                if (mCurrentUser == null)
                    throw new NullReferenceException(String.Format("Пользователь \"{0}\" не найден",
                        Account.ClientLogin));

                return mCurrentUser;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Выйду если постбэк
            if (Page.IsPostBack)
            {
                //vldIpAddresses.Enabled = !cbNoFixedIp.Checked;
                return;
            }

            // Заполню контролы
            litUserName.Text = CurrentUser.Login;
            txtFirstName.Text = CurrentUser.FirstName;
            txtLastName.Text = CurrentUser.LastName;
            txtPatronymicName.Text = CurrentUser.PatronymicName;
            txtEmail.Text = CurrentUser.Email;
            txtPhone.Text = CurrentUser.Phone;
            //txtIpAddresses.Text = CurrentUser.GetIpAddressesAsEdit();
            //cbNoFixedIp.Checked = !CurrentUser.HasFixedIp;

            btnUpdate.Attributes.Add("handleClick", "false");
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            // Проверю валидность контролов страницы
            if (!Page.IsValid)
                return;

            IntrantAccount user = IntrantAccount.GetIntrantAccount(Account.ClientLogin);
            if (user == null)
                throw new NullReferenceException(String.Format("Пользователь \"{0}\" не найден",
                    Account.ClientLogin));

            // Регистрирую нового пользователя
            UpdateUser(ref user);

            // Выполню действия после успешной регистрации
            ProcessSuccess();
        }

        private void UpdateUser(ref IntrantAccount user)
        {
            user.FirstName = txtFirstName.Text.Trim();
            user.LastName = txtLastName.Text.Trim();
            user.PatronymicName = txtPatronymicName.Text.Trim();
            user.Phone = txtPhone.Text.Trim();
            user.Email = txtEmail.Text.Trim();
            //user.SetIpAddressesAsEdit(txtIpAddresses.Text);
            //user.HasFixedIp = !cbNoFixedIp.Checked;
            user.Update();
        }

        private void ProcessSuccess()
        {
            // Перейду на страницу успеха
            Response.Redirect(SuccessUri, true);
        }
    }
}