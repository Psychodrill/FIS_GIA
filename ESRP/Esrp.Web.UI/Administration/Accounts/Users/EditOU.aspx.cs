namespace Esrp.Web.Administration.Accounts.Users
{
    using System;
    using System.Web.UI.WebControls;

    using Esrp.Core;
    using Esrp.Core.Organizations;
    using Esrp.Core.Systems;
    using Esrp.Utility;
    using Esrp.Web.Administration.SqlConstructor.UserAccounts;

    using Esrp.Services;

    /// <summary>
    /// Карточка редактирования ОУ
    /// </summary>
    public partial class EditOU : BasePage
    {
        #region Constants and Fields

        private const string ErrorUserNotFound = "Пользователь \"{0}\" не найден";

        private const string LoginQueryKey = "login";

        private readonly InformationSystemsService informationSystemsService = new InformationSystemsService();
        private readonly UsersService usersService = new UsersService();
        private readonly GroupService groupService = new GroupService();

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
            if (Request.UrlReferrer != null && Request.UrlReferrer.LocalPath != null)
            {
                if (Request.UrlReferrer.LocalPath.Contains("ListOU.aspx"))
                {
                    Session["BackLink.HRef"] = Request.UrlReferrer.ToString();
                }

            }
            BackLink.HRef = (string)Session["BackLink.HRef"];
            if (this.Page.IsPostBack)
            {
                return;
            }

            this.chblGroup.DataSource = SqlConstructor_GetUsersOU.GetAvailableOUGroups(this.User.Identity.Name);
            this.chblGroup.DataBind();

            var groupCodes = SqlConstructor_GetUsersIS.GetUserGroupCodes(this.CurrentUser.Login);
            foreach (string groupCode in groupCodes)
            {
                var item = this.chblGroup.Items.FindByValue(groupCode);
                if (item != null)
                {
                    item.Selected = true;
                }
            }

            // Список checkbox: Информационные системы
            this.cblSystems.DataSource = this.informationSystemsService.GetAvailableSystemsByLogin(this.User.Identity.Name);
            this.cblSystems.DataBind();

            var userSystems = this.informationSystemsService.GetUserSystems(this.CurrentUser.Login);
            foreach (var userSystem in userSystems)
            {
                var item = this.cblSystems.Items.FindByValue(userSystem.ToString());
                if (item != null)
                {
                    item.Selected = true;
                }
            }

            // Заполню соответствующие контролы при первом обращении к странице
            this.txtPhone.Text = this.CurrentUser.Phone;
            this.txtFirstName.Text = this.CurrentUser.FirstName;
            this.txtLastName.Text = this.CurrentUser.LastName;
            this.txtPatronymicName.Text = this.CurrentUser.PatronymicName;
            this.trAccessBlock.Visible = this.User.Identity.Name != this.CurrentUser.Login;
         

            var org = OrganizationDataAccessor.GetByLogin(this.CurrentUser.Login);

            var items = this.cblSystems.Items;
            var checkboxAccessFbd = items.FindByValue("3");
            var checkboxAccessFbs = items.FindByValue("2");

            if (checkboxAccessFbs != null)
            {
                var hasAbilityToFBD = false;
                if (org != null)
                {
                    if (org.OrgType.Id == 1 || org.OrgType.Id == 2)
                    {
                        hasAbilityToFBD = true;
                    }

                    if (org.OrgType.Id == 1)
                    {
                        checkboxAccessFbs.Selected = GeneralSystemManager.HasAccessToGroup(
                            this.CurrentUser.Login, FbsManager.VuzGroupCode);
                    }

                    if (org.OrgType.Id == 2)
                    {
                        checkboxAccessFbs.Selected = GeneralSystemManager.HasAccessToGroup(
                            this.CurrentUser.Login, FbsManager.SsuzGroupCode);
                    }

                    if (org.OrgType.Id == 3)
                    {
                        // РЦОИ
                        checkboxAccessFbs.Selected = GeneralSystemManager.HasAccessToGroup(
                            this.CurrentUser.Login, FbsManager.InfoProcessingGroupCode);
                    }

                    if (org.OrgType.Id == 4)
                    {
                        // ОУО
                        checkboxAccessFbs.Selected = GeneralSystemManager.HasAccessToGroup(
                            this.CurrentUser.Login, FbsManager.DirectionGroupCode);
                    }

                    if (org.OrgType.Id == 6)
                    {
                        // Учредитель
                        checkboxAccessFbs.Selected = GeneralSystemManager.HasAccessToGroup(
                            this.CurrentUser.Login, FbsManager.FounderGroupCode);
                    }

                    if (org.OrgType.Id == 5)
                    {
                        // Другое
                        checkboxAccessFbs.Selected = GeneralSystemManager.HasAccessToGroup(
                            this.CurrentUser.Login, FbsManager.OtherGroupCode);
                    }
                }

                if (checkboxAccessFbd != null)
                {
                    //checkboxAccessFbd.Selected = GeneralSystemManager.HasAccessToGroup(this.CurrentUser.Login, FbdManager.UserGroupCode);

                    if (!hasAbilityToFBD)
                    {
                        checkboxAccessFbd.Enabled = false;
                        checkboxAccessFbd.Selected = false;
                    }
                }
            }

            // Нажатие на кнопку не должно инициировать проверку изменений на форме
            this.btnUpdate.Attributes.Add("handleClick", "false");

            // Установлю заголовок страницы
            this.PageTitle = string.Format("Редактирование “{0}”", this.CurrentUser.Login);
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

            // Обновлю информацию нового пользователя
            this.CurrentUser.Email = this.CurrentUser.Login;
            this.CurrentUser.Update();

            Organization org = OrganizationDataAccessor.GetByLogin(this.User.Identity.Name);

            if (this.User.Identity.Name != this.CurrentUser.Login)
            {
                var items = this.cblSystems.Items;
                var checkboxAccessFbd = items.FindByValue("3");
                var checkboxAccessFbs = items.FindByValue("2");

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

                if (checkboxAccessFbs != null && checkboxAccessFbs.Selected)
                {
                    if (org.OrgType.Id == 1)
                    {
                        SqlConstructor_GetUsersIS.SetUserGroup(this.CurrentUser.Login, FbsManager.VuzGroupCode, false);
                    }

                    if (org.OrgType.Id == 2)
                    {
                        SqlConstructor_GetUsersIS.SetUserGroup(this.CurrentUser.Login, FbsManager.SsuzGroupCode, false);
                    }

                    if (org.OrgType.Id == 3)
                    {
                        // РЦОИ
                        SqlConstructor_GetUsersIS.SetUserGroup(
                            this.CurrentUser.Login, FbsManager.InfoProcessingGroupCode, false);
                    }

                    if (org.OrgType.Id == 4)
                    {
                        // ОУО
                        SqlConstructor_GetUsersIS.SetUserGroup(
                            this.CurrentUser.Login, FbsManager.DirectionGroupCode, false);
                    }

                    if (org.OrgType.Id == 5)
                    {
                        // Учредитель
                        SqlConstructor_GetUsersIS.SetUserGroup(
                            this.CurrentUser.Login, FbsManager.FounderGroupCode, false);
                    }

                    if (org.OrgType.Id == 6)
                    {
                        // Другое
                        SqlConstructor_GetUsersIS.SetUserGroup(this.CurrentUser.Login, FbsManager.OtherGroupCode, false);
                    }
                }

                if (checkboxAccessFbd != null)
                {
                    if (!(org.OrgType != null && (org.OrgType.Id == 1 || org.OrgType.Id == 2)))
                    {
                        checkboxAccessFbd.Selected = false;
                    }

                    if (checkboxAccessFbd.Selected)
                    {
                        foreach (ListItem groupItem in chblGroup.Items)
                        {
                            if (groupItem.Selected)
                            {
                                SqlConstructor_GetUsersIS.SetUserGroup(this.CurrentUser.Login, groupItem.Value, false);
                            }
                            else
                            {
                                SqlConstructor_GetUsersIS.DeleteUserGroup(this.CurrentUser.Login, groupItem.Value);
                            }
                        }
                    } 
                }
            }

            this.Page.Validate();
            if (!this.Page.IsValid)
            {
                return;
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
            foreach (ListItem item in chblGroup.Items)
            {
                if (item.Selected)
                {
                    this.mCurrentUser.SetGroupCode(item.Value);
                    break;
                }
            }
        }

        private void ProcessSuccess()
        {
            // Остаемся на текущей странице
            this.Response.Redirect(this.CurrentUrl, true);
        }

        #endregion
    }
}