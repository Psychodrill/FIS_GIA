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
using Esrp.Utility;

namespace Esrp.Web.Administration.Accounts.Support
{
    public partial class Create : System.Web.UI.Page
    {
        private const string SuccessUri = "/Administration/Accounts/SupportUsers/Edit.aspx?login={0}";

        private SupportAccount mCurrentUser;
        private SupportAccount CurrentUser
        {
            get
            {
                if (mCurrentUser == null)
                {
                    mCurrentUser = new SupportAccount();
                    DataBindCurrenUser();
                }

                return mCurrentUser;
            }
        }

        private string mCurrentPassword;
        private string CurrentPassword
        {
            get
            {
                if (String.IsNullOrEmpty(mCurrentPassword))
                    mCurrentPassword = Utility.GeneratePassword();
                return mCurrentPassword;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
                return;

            // Нажатие на кнопку не должно инициировать проверку изменений на форме
            btnUpdate.Attributes.Add("handleClick", "false");
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;
            
            // Создам нового пользователя
            CurrentUser.Update();

            // Выполню действия после успешной регистрации
            ProcessSuccess();
        }


        protected void validLogin_ServerValidate(object source, ServerValidateEventArgs args)
        {
            // Получу введенный логин
            string login = args.Value.Trim();

            // Проверю существоване логина в БД
            args.IsValid = !SupportAccount.CheckNewLogin(login);
            if (!args.IsValid)
            {
                // Если логин занят покажу ошибку
                validLogin.ErrorMessage = String.Format(validLogin.ErrorMessage, login);
                txtLogin.Text = string.Empty;
            }
        }

        private void DataBindCurrenUser()
        {
            // Заполню поля значениями из соответствующих контролов
            mCurrentUser.Login = txtLogin.Text.Trim();
            mCurrentUser.Password = CurrentPassword;
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
            // Подготовлю email сообщение 
            EmailTemplate template = new EmailTemplate(EmailTemplateTypeEnum.RegistrationWithoutOrg);
            EmailMessage message = template.ToEmailMessage();
            message.To = CurrentUser.Email;
            message.Params = Utility.CollectEmailMetaVariables(CurrentUser, CurrentPassword, Utility.GetSeverPath(Request));

            // Отправлю email сообщение
            TaskManager.SendEmail(message);

            // Остаемся на текущей странице
            Response.Redirect(String.Format(SuccessUri, CurrentUser.Login), true);
        }
    }
}
