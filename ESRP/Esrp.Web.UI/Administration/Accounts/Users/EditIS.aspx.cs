namespace Esrp.Web.Administration.Accounts.Users
{
    using System;
    using System.Linq;
    using System.Web.UI.WebControls;

    using Esrp.Core;
    using Esrp.Web.Administration.SqlConstructor.UserAccounts;

    /// <summary>
    /// The edit is.
    /// </summary>
    public partial class EditIS : BasePage
    {
        #region Constants and Fields

        private const string ErrorUserNotFound = "Пользователь \"{0}\" не найден";

        private const string LoginQueryKey = "login";

        private AdministratorAccount mCurrentUser;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets CurrentUser.
        /// </summary>
        /// <exception cref="NullReferenceException">
        /// </exception>
        public AdministratorAccount CurrentUser
        {
            get
            {
                if (this.mCurrentUser == null)
                {
                    this.mCurrentUser = AdministratorAccount.GetAdministratorAccountForce(this.Login);
                }

                if (this.mCurrentUser == null)
                {
                    throw new NullReferenceException(string.Format(ErrorUserNotFound, this.Login));
                }

                // Если форма отправлена, заполню поля пользователя из контролов
                if (this.Page.IsPostBack)
                {
                    this.DataBindCurrenUser();
                }

                return this.mCurrentUser;
            }
        }

        /// <summary>
        /// Gets Login.
        /// </summary>
        public string Login
        {
            get
            {
                if (string.IsNullOrEmpty(this.Request.QueryString[LoginQueryKey]))
                {
                    return string.Empty;
                }

                return this.Request.QueryString[LoginQueryKey];
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
            if (Request.UrlReferrer != null && Request.UrlReferrer.LocalPath!=null)
            {
                if (Request.UrlReferrer.LocalPath.Contains("ListIS.aspx"))
                {
                    Session["BackLink.HRef"] = Request.UrlReferrer.ToString();
                }
                BackLink.HRef = (string)Session["BackLink.HRef"];
            }

            // Заполню соответствующие контролы при первом обращении к странице
            this.txtPhone.Text = this.CurrentUser.Phone;
            this.txtFirstName.Text = this.CurrentUser.FirstName;
            this.txtLastName.Text = this.CurrentUser.LastName;
            this.txtPatronymicName.Text = this.CurrentUser.PatronymicName;

            this.phAccessGroup.Visible = this.User.Identity.Name != this.CurrentUser.Login;

            this.cblGroup.DataSource = SqlConstructor_GetUsersIS.GetAvailableGroups(this.User.Identity.Name);
            this.cblGroup.DataBind();
            int[] userGroups = SqlConstructor_GetUsersIS.GetUserGroups(this.CurrentUser.Login);
            foreach (int userGroup in userGroups)
            {
                var item = this.cblGroup.Items.FindByValue(userGroup.ToString());
                if (item != null)
                {
                    item.Selected = true;
                }
            }

            // Нажатие на кнопку не должно инициировать проверку изменений на форме
            this.btnUpdate.Attributes.Add("handleClick", "false");

            // Установлю заголовок страницы
            this.PageTitle = string.Format("Редактирование “{0}”", this.CurrentUser.Login);
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
            // Проверю валидность контролов страницы
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
                            ErrorMessage = @"Необходимо выбрать хотя бы одну группу", 
                            ControlToValidate = "cblGroup", 
                            IsValid = false
                        });
                return;
            }

            // Обновлю информацию нового пользователя
            this.CurrentUser.Email = this.CurrentUser.Login;
            this.CurrentUser.FirstName = this.txtFirstName.Text.Trim();
            this.CurrentUser.LastName = this.txtLastName.Text.Trim();
            this.CurrentUser.PatronymicName = this.txtPatronymicName.Text.Trim();
            this.CurrentUser.Phone = this.txtPhone.Text.Trim();
            this.CurrentUser.Update();
            if (this.User.Identity.Name != this.CurrentUser.Login)
            {
                // 1, 2, 4, 5, 13, 14
                SqlConstructor_GetUsersIS.DeleteUserGroupsIS(this.CurrentUser.Login);
                foreach (ListItem item in this.cblGroup.Items)
                {
                    if (item.Selected)
                    {
                        SqlConstructor_GetUsersIS.SetUserGroup(
                            this.CurrentUser.Login, Convert.ToInt32(item.Value), false);
                    }
                }
            }

            // Выполню действия после успешной регистрации
            this.ProcessSuccess();
        }

        private void DataBindCurrenUser()
        {
            // Заполню поля пользователя из соответствующих контролов
            this.mCurrentUser.Phone = this.txtPhone.Text.Trim();
            this.mCurrentUser.FirstName = this.txtFirstName.Text.Trim();
            this.mCurrentUser.LastName = this.txtLastName.Text.Trim();
            this.mCurrentUser.PatronymicName = this.txtPatronymicName.Text.Trim();
        }

        private void ProcessSuccess()
        {
            // Остаемся на текущей странице
            this.Response.Redirect(this.CurrentUrl, true);
        }

        #endregion
    }
}