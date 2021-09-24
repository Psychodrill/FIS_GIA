namespace Esrp.Web.Administration.Accounts.Users
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI.WebControls;

    using Esrp.Core;
    using Esrp.Core.CatalogElements;
    using Esrp.Core.Common;
    using Esrp.Core.Organizations;
    using Esrp.Core.Systems;
    using Esrp.Core.Users;
    using Esrp.Utility;

    using Esrp.Services;

    using Esrp.Web.Administration.SqlConstructor.UserAccounts;

    /// <summary>
    /// The edit.
    /// </summary>
    public partial class Edit : BasePage
    {
        #region Constants and Fields

        private const string RequestIDParamName = "req_id";

        private const string SuccessUri = "/Administration/Accounts/Users/EditSuccess.aspx?login={0}";

        private readonly InformationSystemsService informationSystemsService = new InformationSystemsService();

        private OrgUser mCurrentUser;

        #endregion

        #region Properties

        private string rcDescriptionText
        {
            get
            {
                int model = int.Parse(this.BehaviorModelList.SelectedValue);
                return model == 999 ? this.AnotherRCModelName.Text : string.Empty;
            }

            set
            {
                this.AnotherRCModelName.Text = value;
            }
        }

        private int? rcModel
        {
            get
            {
                return int.Parse(this.BehaviorModelList.SelectedValue);
            }

            set
            {
                if (value != -1)
                {
                    this.BehaviorModelList.SelectedValue = value.ToString();
                }

                if (value != 999)
                {
                    this.AnotherRCModelName.Text = string.Empty;
                    this.AnotherRCModelName.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Gets CurrentUser.
        /// </summary>
        /// <exception cref="NullReferenceException">
        /// </exception>
        public OrgUser CurrentUser
        {
            get
            {
                if (this.mCurrentUser == null)
                {
                    this.mCurrentUser = OrgUserDataAccessor.Get(this.Login);
                }

                if (this.mCurrentUser == null)
                {
                    throw new NullReferenceException(string.Format("Пользователь \"{0}\" не найден", this.Login));
                }

                return this.mCurrentUser;
            }
        }

        /// <summary>
        /// Gets a value indicating whether IsQueryStringContainsRequestID.
        /// </summary>
        public bool IsQueryStringContainsRequestID
        {
            get
            {
                return this.Request.QueryString.AllKeys.Contains(RequestIDParamName);
            }
        }

        /// <summary>
        /// Gets a value indicating whether IsRegistrationDocumentExists.
        /// </summary>
        public bool IsRegistrationDocumentExists
        {
            get
            {
                return this.CurrentUser.registrationDocument != null;
            }
        }

        /// <summary>
        /// Gets Login.
        /// </summary>
        public string Login
        {
            get
            {
                if (string.IsNullOrEmpty(this.Request.QueryString["login"]))
                {
                    return string.Empty;
                }

                return this.Request.QueryString["login"];
            }
        }

        /// <summary>
        /// Gets RequestID.
        /// </summary>
        public int RequestID
        {
            get
            {
                int requestID = 0;

                requestID = this.CurrentUser.RequestedOrganization.Id;

                return requestID;
            }
        }

        /// <summary>
        /// Gets a value indicating whether UserIsLinkedToOrg.
        /// </summary>
        public bool UserIsLinkedToOrg
        {
            get
            {
                return this.CurrentUser.RequestedOrganization.OrganizationId != null;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Кнопка видна, если пользователь не привязан к организации и у редактора - право админа.
        /// </summary>
        private bool AddToOrgsVisible
        {
            get
            {
                return (!this.UserIsLinkedToOrg) && this.User.IsInRole("CreateOrganization");
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The bt add to orgs_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        /// <exception cref="Exception">
        /// </exception>
        protected void BtAddToOrgs_Click(object sender, EventArgs e)
        {
            if (!this.Page.IsValid)
            {
                return;
            }

            // Пользователь уже был привязан к организации из справочника, ее добавление заново - дубль
            if (this.CurrentUser.RequestedOrganization.OrganizationId > 0)
            {
                throw new Exception("Попытка создания дублирующей организации в справочнике заблокирована");
            }

            this.UpdateUser(this.CurrentUser); // Обновим пользователя

            int RequestId = this.CurrentUser.RequestedOrganization.Id; // Запомним 
            this.CurrentUser.RequestedOrganization.Id = 0; // Нужно чтобы орг. добавилась, а не обновилась

            // Создаем
            int NewOrgId = OrganizationDataAccessor.UpdateOrCreate(this.CurrentUser.RequestedOrganization, "SYSTEM");

            // Связываем заявку с организацией
            OrganizationDataAccessor.BindRequestToOrganization(RequestId, NewOrgId);

            this.Response.Redirect(string.Format(SuccessUri, this.CurrentUser.login));
        }

        /// <summary>
        /// The on pre render.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            this.ManageUI();
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

            this.chblGroup.DataSource = SqlConstructor_GetUsersOU.GetAvailableOUGroups(this.User.Identity.Name);
            this.chblGroup.DataBind();

            var groupCodes = SqlConstructor_GetUsersIS.GetUserGroupCodes(this.CurrentUser.login);
            foreach (string groupCode in groupCodes)
            {
                var item = this.chblGroup.Items.FindByValue(groupCode);
                if (item != null)
                {
                    item.Selected = true;
                }
            }

            // Выставлю сначала значение по умолчанию, а затем перепишу его, если оно есть
            this.rblReceptionOnResultsCNE.SelectedValue = "0";


            if (this.CurrentUser.RequestedOrganization.ReceptionOnResultsCNE != null)
            {
                this.rblReceptionOnResultsCNE.SelectedValue = this.CurrentUser.RequestedOrganization.ReceptionOnResultsCNE.ToString();
            }

            // Список checkbox: Информационные системы
            this.cblSystems.DataSource = this.informationSystemsService.GetAvailableSystems();
            this.cblSystems.DataBind();

            var userSystems = this.informationSystemsService.GetUserSystems(this.CurrentUser.login);
            foreach (var userSystem in userSystems)
            {
                var item = this.cblSystems.Items.FindByValue(userSystem.ToString());
                if (item != null)
                {
                    item.Selected = true;
                }
            }

            Organization currentOrg = this.GetOrg();

            // Если организация не выбрана или тип организации не ВУЗ или ССУЗ, то...
            if (!CanAccessFbd(currentOrg))
            {
                // Запрещаем выбирать "ФИС ЕГЭ и приема"
                var checkboxAccessFbd = this.cblSystems.Items.FindByValue(Constants.Systems.FBD.ToString());
                if (checkboxAccessFbd != null)
                {
                    checkboxAccessFbd.Enabled = false;
                    checkboxAccessFbd.Selected = false;
                }
            }

            this.BtAddToOrgs.Visible = this.AddToOrgsVisible;

            // заполню соответствующие контролы
            this.litStatus.Text = UserAccountExtentions.GetUserAccountNewStatusName(this.CurrentUser.status);
            this.litStatusDescription.Text = UserAccountExtentions.GetEditStatusDescription(this.CurrentUser);

            this.txtOrganizationChiefName.Text = this.CurrentUser.RequestedOrganization.DirectorFullName;

            this.txtPhone.Text = this.CurrentUser.phone;
            this.txtPosition.Text = this.CurrentUser.position;
            this.txtFullName.Text = this.CurrentUser.lastName;

            // txtIpAddresses.Text = CurrentUser.GetIpAddressesAsEdit();
            this.txtOrganizationAddress.Text = this.CurrentUser.RequestedOrganization.LawAddress;
            this.txtOrganizationFax.Text = this.CurrentUser.RequestedOrganization.Fax;
            this.txtOrganizationFounderName.Text = this.CurrentUser.RequestedOrganization.OwnerDepartment;

            this.ddlOrganizationRegion.DataSource = RegionDataAcessor.GetAllInEtalon(null);
            this.ddlOrganizationRegion.DataBind();
            this.ddlOrganizationRegion.SelectedValue = this.CurrentUser.RequestedOrganization.Region.Id.ToString();

            this.ddlEducationInstitutionType.DataSource = CatalogDataAcessor.GetAll(CatalogDataAcessor.Catalogs.OrganizationTypes);

            // GetEducationInstitutionTypes();
            this.ddlEducationInstitutionType.DataBind();
            this.ddlEducationInstitutionType.SelectedValue = Convert.ToString(this.CurrentUser.RequestedOrganization.OrgType.Id);

            this.DLLOrgKind.DataSource = CatalogDataAcessor.GetAll(CatalogDataAcessor.Catalogs.OrganizationKinds);
            this.DLLOrgKind.DataBind();
            if (this.CurrentUser.RequestedOrganization.Kind.Id != null)
            {
                this.DLLOrgKind.SelectedValue = Convert.ToString(this.CurrentUser.RequestedOrganization.Kind.Id);
            }
            else
            {
                this.DLLOrgKind.SelectedValue = string.Empty;
            }

            this.txtOrganizationPhone.Text = this.CurrentUser.RequestedOrganization.Phone;

            this.TBAccred.Text = this.CurrentUser.RequestedOrganization.AccreditationSertificate;
            this.TbDirectorPosition.Text = this.CurrentUser.RequestedOrganization.DirectorPosition;
            this.TBEMail.Text = this.CurrentUser.RequestedOrganization.EMail;
            this.TBSite.Text = this.CurrentUser.RequestedOrganization.Site;
            this.TbFactAddress.Text = this.CurrentUser.RequestedOrganization.FactAddress;

            this.tbINN.Text = this.CurrentUser.RequestedOrganization.INN;
            this.tbOGRN.Text = this.CurrentUser.RequestedOrganization.OGRN;
            this.tbKPP.Text = this.CurrentUser.RequestedOrganization.KPP;

            this.TbPhoneCode.Text = this.CurrentUser.RequestedOrganization.PhoneCityCode;
            this.TBShortName.Text = this.CurrentUser.RequestedOrganization.ShortName;

            this.ChIsFilial.Checked = this.CurrentUser.RequestedOrganization.IsFilial;
            if (this.CurrentUser.RequestedOrganization.IsPrivate)
            {
                this.DDLOPF.SelectedValue = "1";
            }
            else
            {
                this.DDLOPF.SelectedValue = "0";
            }

            if (this.CurrentUser.RequestedOrganization.Kind.Id != null)
            {
                this.DLLOrgKind.SelectedValue = this.CurrentUser.RequestedOrganization.Kind.Id.ToString();
            }
            else
            {
                this.DLLOrgKind.SelectedValue = "0";
            }

            // Модель приемной кампании
            this.rcModel = this.CurrentUser.RequestedOrganization.RCModelId;
            this.rcDescriptionText = this.CurrentUser.RequestedOrganization.RCDescription;

            // ------------------------------------------------------------------------------
            Organization currentEtalonOrg = this.GetOrg();

            // если установлено соответствие с эталонной организацией
            if (currentEtalonOrg != null)
            {
                this.OrganizationName_hiddenField.Value = currentEtalonOrg.Id.ToString();

                this.OrganizationName_txt.Text = currentEtalonOrg.FullName;
                this.OrganizationName_txt.Visible = false;
                this.OrganizationName_hyperlynk.Visible = true;
                this.OrganizationName_hyperlynk.Text = currentEtalonOrg.FullName;
                this.OrganizationName_hyperlynk.NavigateUrl = string.Format(
                    "../../Organizations/Administrators/OrgCard_Edit.aspx?OrgID={0}", currentEtalonOrg.Id);

                this.ddlOrganizationRegion.Enabled = false;
                this.ddlOrganizationRegion.SelectedValue = currentEtalonOrg.Region.Id.ToString();
                this.ddlOrganizationRegion.ToolTip = "Поле зафиксировано выбором организации";

                this.ddlEducationInstitutionType.Enabled = false;
                this.ddlEducationInstitutionType.SelectedValue = currentEtalonOrg.OrgType.Id.ToString();
                this.ddlEducationInstitutionType.ToolTip = "Поле зафиксировано выбором организации";
            }
            else
            {
                this.OrganizationName_txt.Text = this.CurrentUser.RequestedOrganization.FullName;
            }

            string backUrl = "./Administration/Accounts/Users/Edit.aspx?login=" + this.CurrentUser.login;
            if (this.Request.QueryString.AllKeys.Contains(RequestIDParamName))
            {
                backUrl += string.Format("&{0}={1}", RequestIDParamName, this.RequestID);
            }

            backUrl = HttpUtility.UrlEncode(backUrl);

            this.btnChangeOrg.PostBackUrl = string.Format(
                "/SelectOrg.aspx?BackUrl={0}&RegID={1}&OrgTypeID={2}&OrgName={3}",
                backUrl,
                this.ddlOrganizationRegion.SelectedValue,
                this.ddlEducationInstitutionType.SelectedValue,
                this.OrganizationName_txt.Text);

            this.VReqUserName.ErrorMessage = string.Format(this.VReqUserName.ErrorMessage, GeneralSystemManager.GetSystemName(2));

            // Нажатие на кнопку не должно инициировать проверку изменений на форме
            this.btnUpdate.Attributes.Add("handleClick", "false");
            this.BtAddToOrgs.Attributes.Add("handleClick", "false");

            this.divChangeDoc.Visible = typeof(AdministratorAccount) == Account.GetType(Account.ClientLogin);

            // Установлю заголовок страницы
            this.PageTitle = string.Format("Редактирование “{0}”", this.CurrentUser.login);

            this.ToggleValidators(); // Отключить валидаторы для пользователей, привязанных к организации из справочника
            if (Request.UrlReferrer != null && Request.UrlReferrer.LocalPath != null)
            {
                if (Request.UrlReferrer.LocalPath.Contains("List.aspx") || Request.UrlReferrer.LocalPath.Contains("RequestForm.aspx"))
                {
                    Session["BackLinkEdit.HRef"] = Request.UrlReferrer.ToString();
                }
                BackLink.HRef = (string)Session["BackLinkEdit.HRef"];
            }
        }

        private bool CanAccessFbd(Organization org)
        {
            if (org == null)
                return false;
            if ((org.OrgType.Id.Equals(Constants.OrganizationType2010.VUZ))
                || (org.OrgType.Id.Equals(Constants.OrganizationType2010.SSUZ))
                || (org.OrgType.Id.Equals(Constants.OrganizationType2010.Owner)))
                return true;
            return false;
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
            // проверю валидность контролов страницы
            if (!this.Page.IsValid)
            {
                return;
            }

            // регистрирую нового пользователя
            if (!this.UpdateUser(this.CurrentUser))
            {
                return;
            }

            // выполню действия после успешной регистрации
            this.ProcessSuccess();
        }

        /// <summary>
        /// Получаем организацию, к которой привязан пользователь
        /// </summary>
        /// <returns>
        /// </returns>
        private Organization GetOrg()
        {
            int OrgId = 0;

            // Пользователь привязывается к организации - организация выбрана поиском из справочника (кнопка "Выбрать")
            if (!string.IsNullOrEmpty(this.Request.QueryString["OrgID"]))
            {
                int.TryParse(this.Request.QueryString["OrgID"], out OrgId);
            }

            // Пользователь уже привязан к организации
            else if (this.CurrentUser.RequestedOrganization.OrganizationId != null)
            {
                OrgId = (int)this.CurrentUser.RequestedOrganization.OrganizationId;
            }

            if (OrgId != 0)
            {
                return OrganizationDataAccessor.Get(OrgId);
            }
            else
            {
                return null;

                // Обнаружить организацию не удалось - пользователь не был привязан и не был произведен поиск
            }
        }

        private void ManageUI()
        {

        }

        private void ProcessSuccess()
        {
            // перейду на страницу успеха
            if (this.Request.QueryString.AllKeys.Contains(RequestIDParamName))
            {
                this.Response.Redirect(
                    string.Format(SuccessUri + "&{1}={2}", this.CurrentUser.login, RequestIDParamName, this.RequestID),
                    true);
            }
            else
            {
                this.Response.Redirect(string.Format(SuccessUri, this.CurrentUser.login), true);
            }
        }

        private void SendNotification(OrgUser user)
        {
            // Подготовлю email сообщение 
            var template = new EmailTemplate(EmailTemplateTypeEnum.Consideration);
            EmailMessage message = template.ToEmailMessage();
            message.To = user.email;
            message.Params = Utility.CollectEmailMetaVariables(user);

            // Отправлю уведомление
            TaskManager.SendEmail(message);
        }

        private void ToggleValidators()
        {
            if (this.UserIsLinkedToOrg)
            {
                this.VReqDirectorName.Enabled = false;
                this.VReqFactAddress.Enabled = false;
                this.VReqFounderName.Enabled = false;
                this.VReqLawAddress.Enabled = false;
                this.VReqOrgKind.Enabled = false;
            }
        }

        /// <summary>
        /// "Собираем" пользователя из контролов страницы
        /// </summary>
        /// <param name="user">
        /// </param>
        /// <returns>
        /// The update user.
        /// </returns>
        private bool UpdateUser(OrgUser user)
        {
            user.RequestedOrganization.RCModelId = this.rcModel;
            user.RequestedOrganization.RCDescription = this.rcDescriptionText;
            user.RequestedOrganization.AccreditationSertificate = this.TBAccred.Text.Trim();
            user.RequestedOrganization.DirectorPosition = this.TbDirectorPosition.Text.Trim();
            user.RequestedOrganization.EMail = this.TBEMail.Text.Trim();
            user.RequestedOrganization.Site = this.TBSite.Text.Trim();
            user.RequestedOrganization.FactAddress = this.TbFactAddress.Text.Trim();
            user.RequestedOrganization.INN = this.tbINN.Text.Trim();
            user.RequestedOrganization.OGRN = this.tbOGRN.Text.Trim();
            user.RequestedOrganization.KPP = this.tbKPP.Text.Trim();
            user.RequestedOrganization.PhoneCityCode = this.TbPhoneCode.Text.Trim();
            user.RequestedOrganization.ShortName = this.TBShortName.Text.Trim();

            if (this.DLLOrgKind.SelectedItem.Value != string.Empty)
            {
                user.RequestedOrganization.Kind = new CatalogElement(
                    Convert.ToInt32(this.DLLOrgKind.SelectedItem.Value));
            }

            user.RequestedOrganization.IsFilial = this.ChIsFilial.Checked;
            user.RequestedOrganization.IsPrivate = this.DDLOPF.SelectedValue == "1";

            user.lastName = this.txtFullName.Text.Trim();

            user.RequestedOrganization.LawAddress = this.txtOrganizationAddress.Text.Trim();
            user.RequestedOrganization.DirectorFullName = this.txtOrganizationChiefName.Text.Trim();
            user.RequestedOrganization.Fax = this.txtOrganizationFax.Text.Trim();
            user.RequestedOrganization.OwnerDepartment = this.txtOrganizationFounderName.Text.Trim();
            user.RequestedOrganization.FullName = this.OrganizationName_txt.Text.Trim();
            user.RequestedOrganization.Phone = this.txtOrganizationPhone.Text.Trim();
            user.RequestedOrganization.Region =
                new CatalogElement(Convert.ToInt32(this.ddlOrganizationRegion.SelectedItem.Value));
            user.phone = this.txtPhone.Text.Trim();
            user.position = this.txtPosition.Text.Trim();

            if (string.IsNullOrEmpty(this.ddlEducationInstitutionType.SelectedValue))
            {
                user.RequestedOrganization.OrgType = null;
            }
            else
            {
                user.RequestedOrganization.OrgType =
                    new CatalogElement(Convert.ToInt32(this.ddlEducationInstitutionType.SelectedValue));
            }

            if ((this.OrganizationName_hiddenField.Value == "0")
                || (this.OrganizationName_hiddenField.Value == string.Empty))
            {
                user.RequestedOrganization.OrganizationId = null;
            }
            else
            {
                user.RequestedOrganization.OrganizationId = int.Parse(this.OrganizationName_hiddenField.Value);
            }

            if (this.ddlEducationInstitutionType.SelectedValue == Constants.OrganizationType2010.VUZ.ToString() || this.ddlEducationInstitutionType.SelectedValue == Constants.OrganizationType2010.SSUZ.ToString())
            {
                this.CurrentUser.RequestedOrganization.ReceptionOnResultsCNE = this.rblReceptionOnResultsCNE.SelectedValue == string.Empty ? (int?)null : Convert.ToInt32(this.rblReceptionOnResultsCNE.SelectedValue);
            }
            else
            {
                this.CurrentUser.RequestedOrganization.ReceptionOnResultsCNE = null;
            }

            // если задан документ регистрации, то добавлю его
            if (!string.IsNullOrEmpty(this.fuRegistrationDocument.FileName)
                && this.fuRegistrationDocument.FileBytes.Length > 0)
            {
                // throw new NotImplementedException();
                user.registrationDocument = this.fuRegistrationDocument.FileBytes;
                user.registrationDocumentContentType = this.fuRegistrationDocument.PostedFile.ContentType;
            }

            var items = this.cblSystems.Items;
            var checkboxAccessFbd = items.FindByValue("3");

            // если некорректная организация, снимаем ФБД
            if (!CanAccessFbd(user.RequestedOrganization))
            {
                if (checkboxAccessFbd != null)
                {
                    checkboxAccessFbd.Selected = false;
                }
            }

            this.Page.Validate();
            if (!this.Page.IsValid)
            {
                return false;
            }

            // Сохраню статус
            UserAccount.UserAccountStatusEnum prevStatus = user.status;

            try
            {
                // Очистка существующих связей пользователя с ИС
                user.SystemsId.Add("0");

                foreach (ListItem item in items)
                {
                    if (item.Selected)
                    {
                        user.SystemsId.Add(item.Value);
                    }
                }

                //Активация сотрудника по олимпиадам - автоматическая
                bool isOlympicStaff = false;
                if ((checkboxAccessFbd != null) && (checkboxAccessFbd.Selected))
                {
                    foreach (ListItem groupItem in chblGroup.Items)
                    {
                        if ((groupItem.Selected) && (groupItem.Value == FbdManager.OlympicStaffGroupCode))
                        {
                            isOlympicStaff = true;
                        }
                    }
                }
                if (isOlympicStaff)
                {
                    user.status = UserAccount.UserAccountStatusEnum.Activated;
                }
                OrgUserDataAccessor.UpdateUserAccount(new List<OrgUser> { user }, this.RequestID > 0 ? (int?)this.RequestID : null, isOlympicStaff);
            }
            catch (DbException exc)
            {
                this.Page.AddError(exc.Message);
                return false;
            }

            if ((checkboxAccessFbd != null) && (checkboxAccessFbd.Selected))
            {
                foreach (ListItem groupItem in chblGroup.Items)
                {
                    if (groupItem.Selected)
                    {
                        SqlConstructor_GetUsersIS.SetUserGroup(this.CurrentUser.login, groupItem.Value, false);
                    }
                    else
                    {
                        SqlConstructor_GetUsersIS.DeleteUserGroup(this.CurrentUser.login, groupItem.Value);
                    }
                }
            }

            // Если статус изменился на "на согласовании" - отправлю уведомление
            if (user.status == UserAccount.UserAccountStatusEnum.Consideration && user.status != prevStatus)
            {
                this.SendNotification(user);
            }

            return true;
        }

        #endregion
    }
}