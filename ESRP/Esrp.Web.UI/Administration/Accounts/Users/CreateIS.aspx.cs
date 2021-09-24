namespace Esrp.Web.Administration.Accounts.Users
{
    using System;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Esrp.Core;
    using Esrp.Utility;
    using Esrp.Web.Administration.SqlConstructor.UserAccounts;
    using System.Security.Cryptography;
    using Esrp.Web.Extentions;
    using System.Text;

    /// <summary>
    /// The create is.
    /// </summary>
    public partial class CreateIS : Page
    {
        #region Constants and Fields

        private const string SuccessUri = "/Administration/Accounts/Users/EditIS.aspx?login={0}";

        private string mCurrentPassword;

        private AdministratorAccount mCurrentUser;

        #endregion

        #region Properties

        private string CurrentPassword
        {
            get
            {
                if (string.IsNullOrEmpty(this.mCurrentPassword))
                {
                    this.mCurrentPassword = Utility.GeneratePassword();
                }

                return this.mCurrentPassword;
            }
        }

        private AdministratorAccount CurrentUser
        {
            get
            {
                if (this.mCurrentUser == null)
                {
                    this.mCurrentUser = new AdministratorAccount();
                    this.DataBindCurrenUser();
                }

                return this.mCurrentUser;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The page_ load.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Page.IsPostBack)
            {
                return;
            }

            this.cblGroup.DataSource = SqlConstructor_GetUsersIS.GetAvailableGroups(this.User.Identity.Name);
            this.cblGroup.DataBind();

            // Нажатие на кнопку не должно инициировать проверку изменений на форме
            this.btnUpdate.Attributes.Add("handleClick", "false");
        }

        /// <summary>
        /// The btn update_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!this.Page.IsValid)
            {
                return;
            }

            bool isAnySelected = this.cblGroup.Items.Cast<ListItem>().Any(x => x.Selected);
            if (!isAnySelected)
            {
                this.Page.Validators.Add(
                    new CustomValidator
                        {
                            ErrorMessage = "Необходимо выбрать хотя бы одну группу", 
                            ControlToValidate = "cblGroup", 
                            IsValid = false
                        });
                return;
            }

            // Создам нового пользователя
            this.CurrentUser.Update();
            SqlConstructor_GetUsersIS.DeleteUserGroupsIS(this.CurrentUser.Login);
            foreach (ListItem item in this.cblGroup.Items)
            {
                if (item.Selected)
                {
                    SqlConstructor_GetUsersIS.SetUserGroup(this.CurrentUser.Login, Convert.ToInt32(item.Value), false);
                }
            }

            // Выполню действия после успешной регистрации
            this.ProcessSuccess();
        }

        /// <summary>
        /// The vld login_ server validate.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        protected void vldLogin_ServerValidate(object source, ServerValidateEventArgs args)
        {
            // Получу введенный логин
            string login = args.Value.Trim();

            // Проверю существоване логина в БД
            args.IsValid = !IntrantAccount.CheckNewLogin(login);
            if (!args.IsValid)
            {
                // Если логин занят покажу ошибку
                this.vldLogin.ErrorMessage = string.Format(this.vldLogin.ErrorMessage, login);
                this.txtLogin.Text = string.Empty;
                this.txtLogin.Focus();
            }
        }

        private void DataBindCurrenUser()
        {
            // Заполню поля значениями из соответствующих контролов
            this.mCurrentUser.Login = this.txtLogin.Text.Trim();
            this.mCurrentUser.Password = this.CurrentPassword;
            this.mCurrentUser.Email = this.txtLogin.Text.Trim();
            this.mCurrentUser.Phone = this.txtPhone.Text.Trim();
            this.mCurrentUser.FirstName = this.txtFirstName.Text.Trim();
            this.mCurrentUser.LastName = this.txtLastName.Text.Trim();
            this.mCurrentUser.PatronymicName = this.txtPatronymicName.Text.Trim();
        }

        private void ProcessSuccess()
        {
            // Подготовлю email сообщение 
            var template = new EmailTemplate(EmailTemplateTypeEnum.Registration);
            EmailMessage message = template.ToEmailMessage();
            message.To = this.CurrentUser.Email;
            //message.Params = Utility.CollectEmailMetaVariables(user, password, Utility.GetSeverPath(this.Request));
            string toEncrypt = String.Format("{0};{1};{2}", this.CurrentUser.Login, DateTime.Now.ToString(), this.CurrentUser.PasswordHash);
            RSACryptoServiceProvider Rsa = Helper.GetCurrentRsaProvider(true);

            toEncrypt = Uri.EscapeDataString(Convert.ToBase64String(Rsa.Encrypt(Encoding.Default.GetBytes(toEncrypt), false)));
            message.Params = Utility.CollectEmailMetaVariables(this.CurrentUser, this.CurrentUser.PasswordHash, Utility.GetSeverPath(this.Request), String.Format("{0}/WebUserAccount/ResetPassword.aspx?userId={1}&isNew=1", Utility.GetSeverPath(this.Request), toEncrypt));
                 

            // Отправлю email сообщение
            TaskManager.SendEmail(message);

            // Остаемся на текущей странице
            this.Response.Redirect(string.Format(SuccessUri, this.CurrentUser.Login), true);
        }

        #endregion
    }
}