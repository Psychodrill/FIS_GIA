namespace Esrp.Web.Administration.Accounts.Users
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Esrp.Core;
    using Esrp.Core.Organizations;
    using Esrp.Core.Systems;
    using Esrp.Utility;
    using Esrp.Web.Administration.SqlConstructor.UserAccounts;
    using Esrp.Services;

    /// <summary>
    /// Страница создания пользователя ОУ
    /// </summary>
    public partial class CreateOU : Page
    {
        #region Constants and Fields

        private const string SuccessUri = "/Administration/Accounts/Users/EditOU.aspx?login={0}";

        private readonly InformationSystemsService informationSystemsService = new InformationSystemsService();
        private readonly UsersService usersService = new UsersService();
        private readonly GroupService groupService = new GroupService();

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
        /// Обработчик события "загрузка страницы"
        /// </summary>
        /// <param name="sender">
        /// Источник события
        /// </param>
        /// <param name="e">
        /// Параметры события
        /// </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Page.IsPostBack)
            {
                return;
            }
            if (Request.UrlReferrer != null && Request.UrlReferrer.LocalPath!=null)
            {
                if (Request.UrlReferrer.LocalPath.Contains("ListOU.aspx"))
                {
                    Session["BackLink.HRef"] = Request.UrlReferrer.ToString();
                }
                BackLink.HRef = (string)Session["BackLink.HRef"];
            }
            // Список checkbox: Информационные системы
            this.cblSystems.DataSource = this.informationSystemsService.GetAvailableSystemsByLogin(this.User.Identity.Name);
            this.cblSystems.DataBind();

            // Нажатие на кнопку не должно инициировать проверку изменений на форме
            this.btnUpdate.Attributes.Add("handleClick", "false");
        }

        /// <summary>
        /// Instructs any validation controls included on the page to validate their assigned information.
        /// </summary>
        public override void Validate()
        {
            base.Validate();
            var noSystemSelected = true;
            var items = this.cblSystems.Items;

            foreach (ListItem item in items)
            {
                if (item.Selected)
                {
                    noSystemSelected = false;
                }
            }

            if (noSystemSelected)
            {
                this.cvSystems.IsValid = false;
            }
        }

        /// <summary>
        /// The btn update_ click.
        /// </summary>
        /// <param name="sender">
        /// Источник события
        /// </param>
        /// <param name="e">
        /// Параметры события
        /// </param>
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!this.Page.IsValid)
            {
                return;
            }

            var orgRID = GeneralSystemManager.GetUserOrganizationRequest(this.User.Identity.Name);

            // Создам нового пользователя
            this.CurrentUser.Update();

            var items = this.cblSystems.Items;
            var checkboxAccessFbd = items.FindByValue("3");
            var checkboxAccessFbs = items.FindByValue("2");

            GeneralSystemManager.SetUserOrganizationRequest(this.CurrentUser.Login, orgRID);

            var org = OrganizationDataAccessor.GetByLogin(this.User.Identity.Name);

            this.usersService.DeleteGroupByLogin(this.CurrentUser.Login);

            foreach (ListItem item in items)
            {
                if (item.Value == @"2" || item.Value == @"3")
                {
                    continue;
                }

                if (item.Selected)
                {
                    var groupId = this.groupService.GetGroupIdDefaultForSystem(Convert.ToInt32(item.Value));

                    if (groupId <= 0)
                    {
                        this.Page.AddError(string.Format("У {0} нет группы по умолчанию", item.Text));
                        return;
                    }

                    this.usersService.SetGroupByLogin(this.CurrentUser.Login, groupId);
                }
            }

            SqlConstructor_GetUsersIS.DeleteUserGroupsInclude(this.CurrentUser.Login, 16, 6, 7, 8, 9, 10, 11);

            if (checkboxAccessFbd != null && checkboxAccessFbd.Selected)
            {
                SqlConstructor_GetUsersIS.SetUserGroup(this.CurrentUser.Login, FbdManager.UserGroupCode, false);
            }

            if ((org!=null)&& (checkboxAccessFbs != null && checkboxAccessFbs.Selected))
            {
                if (org.OrgType.Id == 1)
                {
                    SqlConstructor_GetUsersIS.SetUserGroup(this.CurrentUser.Login, FbsManager.VuzGroupCode, false);
                }

                if (org.OrgType.Id == 2)
                {
                    SqlConstructor_GetUsersIS.SetUserGroup(this.CurrentUser.Login, FbsManager.SsuzGroupCode, false);
                }

                // РЦОИ
                if (org.OrgType.Id == 3)
                {
                    SqlConstructor_GetUsersIS.SetUserGroup(
                        this.CurrentUser.Login, FbsManager.InfoProcessingGroupCode, false);
                }

                // ОУО
                if (org.OrgType.Id == 4)
                {
                    SqlConstructor_GetUsersIS.SetUserGroup(this.CurrentUser.Login, FbsManager.DirectionGroupCode, false);
                }

                // Учредитель
                if (org.OrgType.Id == 5)
                {
                    SqlConstructor_GetUsersIS.SetUserGroup(this.CurrentUser.Login, FbsManager.FounderGroupCode, false);
                }

                // Другое
                if (org.OrgType.Id == 6)
                {
                    SqlConstructor_GetUsersIS.SetUserGroup(this.CurrentUser.Login, FbsManager.OtherGroupCode, false);
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
            var template = new EmailTemplate(EmailTemplateTypeEnum.RegistrationWithoutOrg);
            var message = template.ToEmailMessage();
            message.To = this.CurrentUser.Email;
            message.Params = Utility.CollectEmailMetaVariables(
                this.CurrentUser, this.CurrentPassword, Utility.GetSeverPath(this.Request));

            // Отправлю email сообщение
            TaskManager.SendEmail(message);

            // Остаемся на текущей странице
            this.Response.Redirect(string.Format(SuccessUri, this.CurrentUser.Login), true);
        }

        #endregion
    }
}