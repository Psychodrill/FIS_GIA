namespace Esrp.Web.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Esrp.Core;
    using Esrp.Core.CatalogElements;
    using Esrp.Core.Organizations;
    using Esrp.Core.Systems;
    using Esrp.Core.Users;
    using Esrp.Utility;

    /// <summary>
    /// The user profile edit.
    /// </summary>
    public partial class UserProfileEdit : UserControl
    {
        #region Constants and Fields

        public string BackUrl = "/Profile/Edit.aspx";

        private const string SuccessUri = "/Profile/View.aspx";

        private OrgUser mCurrentUser;

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
                this.BehaviorModelList.SelectedValue = value.ToString();
                if (value != 999)
                {
                    this.AnotherRCModelName.Text = string.Empty;
                    this.AnotherRCModelName.Enabled = false;
                }
            }
        }

        #endregion

        #region Public Properties

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
                    this.mCurrentUser = OrgUserDataAccessor.Get(Account.ClientLogin);

                    if (this.mCurrentUser == null)
                    {
                        throw new NullReferenceException(
                            string.Format("Пользователь \"{0}\" не найден", Account.ClientLogin));
                    }
                }

                return this.mCurrentUser;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The vld is email uniq_ server validate.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        public void vldIsEmailUniq_ServerValidate(object source, ServerValidateEventArgs args)
        {
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
            // Выйду если постбэк
            if (!this.Page.IsPostBack)
            {
                // Выставлю сначала значение по умолчанию, а затем перепишу его, если оно есть
                this.rblReceptionOnResultsCNE.SelectedValue = "0";

               this.lblSystemNamesForFio.Text = this.lblSystemNamesForPhone.Text = string.Empty;
               var systemNames = GeneralSystemManager.AccessedSystems(Account.ClientLogin);

                this.lblSystemNamesForFio.Text = this.lblSystemNamesForPhone.Text = string.Join(", ", systemNames.Cast<object>().Select(c => c.ToString()).ToArray());
                char[] charsToTrim = { ',' };
                this.lblSystemNamesForFio.Text = this.lblSystemNamesForPhone.Text = this.lblSystemNamesForPhone.Text.Trim(charsToTrim);

                // Модель приемной кампании
                if (this.CurrentUser.RequestedOrganization.RCModelId == -1)
                {
                    throw new NullReferenceException(
                            string.Format("Не задана модель приемной кампании"));
                }

                this.rcModel = this.CurrentUser.RequestedOrganization.RCModelId;
                this.rcDescriptionText = this.CurrentUser.RequestedOrganization.RCDescription;

                // Заполню контролы
                this.litUserName.Text = this.CurrentUser.login;

                if (this.CurrentUser.RequestedOrganization.ReceptionOnResultsCNE != null)
                {
                    this.rblReceptionOnResultsCNE.SelectedValue = this.CurrentUser.RequestedOrganization.ReceptionOnResultsCNE.ToString();
                }
                
                this.litStatus.Text = UserAccountExtentions.GetUserAccountStatusName(this.CurrentUser.status);
                this.litNewStatus.Text = UserAccountExtentions.GetUserAccountNewStatusName(this.CurrentUser.status);
                this.litStatusDescription.Text = UserAccountExtentions.GetEditStatusDescription(this.CurrentUser);

                this.txtOrganizationChiefName.Text = this.CurrentUser.RequestedOrganization.DirectorFullName;

                this.txtPhone.Text = this.CurrentUser.phone;

                this.txtFullName.Text = this.CurrentUser.GetFullName();
                this.txtOrganizationAddress.Text = this.CurrentUser.RequestedOrganization.LawAddress;
                this.txtOrganizationFax.Text = this.CurrentUser.RequestedOrganization.Fax;
                this.tbKPP.Text = this.CurrentUser.RequestedOrganization.KPP;
                this.tbINN.Text = this.CurrentUser.RequestedOrganization.INN;
                this.tbOGRN.Text = this.CurrentUser.RequestedOrganization.OGRN;
                this.txtOrganizationFounderName.Text = this.CurrentUser.RequestedOrganization.OwnerDepartment;
                this.txtOrganizationName.Text = this.CurrentUser.RequestedOrganization.FullName;
                this.txtOrganizationPhone.Text = this.CurrentUser.RequestedOrganization.Phone;
                this.hfEtalonOrgID.Value = this.CurrentUser.RequestedOrganization.OrganizationId != null
                                               ? this.CurrentUser.RequestedOrganization.OrganizationId.ToString()
                                               : string.Empty;

                this.ddlOrganizationRegion.DataSource = RegionDataAcessor.GetAllInEtalon(null);
                this.ddlOrganizationRegion.DataBind();
                if (this.CurrentUser.RequestedOrganization.Region.Id == null)
                {
                    this.ddlOrganizationRegion.SelectedValue = "0";
                }
                else
                {
                    this.ddlOrganizationRegion.SelectedValue =
                        this.CurrentUser.RequestedOrganization.Region.Id.ToString();
                }

                this.ddlOrgType.DataSource = CatalogDataAcessor.GetAll(CatalogDataAcessor.Catalogs.OrganizationTypes);

                this.ddlOrgType.DataBind();
                this.ddlOrgType.SelectedValue = this.CurrentUser.RequestedOrganization.OrgType.Id.ToString();

                if (!string.IsNullOrEmpty(this.Request.QueryString["OrgID"]))
                {
                    int orgID;
                    if (int.TryParse(this.Request.QueryString["OrgID"], out orgID))
                    {
                        Organization NewOrg = OrganizationDataAccessor.Get(orgID);
                        if (NewOrg != null)
                        {
                            this.txtOrganizationName.Text = NewOrg.FullName;
                            this.ddlOrganizationRegion.SelectedValue = NewOrg.Region.Id.ToString();
                            this.ddlOrgType.SelectedValue = NewOrg.OrgType.Id.ToString();
                            this.hfEtalonOrgID.Value = NewOrg.Id.ToString();
                        }
                        else
                        {
                            this.hfEtalonOrgID.Value = string.Empty;
                        }
                    }
                }

                // если есть связь с Эталонной организацией, то блокируем поля входа
                if (this.hfEtalonOrgID.Value != string.Empty)
                {
                    this.txtOrganizationName.ReadOnly = true;
                    this.ddlOrganizationRegion.Enabled = false;
                    this.ddlOrgType.Enabled = false;
                }

                this.btnUpdate.Attributes.Add("handleClick", "false");

                this.btnChangeOrg.PostBackUrl =
                    string.Format(
                        "/SelectOrg.aspx?BackUrl={0}&RegID={1}&OrgTypeID={2}&OrgName={3}", 
                        this.BackUrl, 
                        this.ddlOrganizationRegion.SelectedValue, 
                        this.ddlOrgType.SelectedValue, 
                        this.txtOrganizationName.Text);
                this.vldFullName.ErrorMessage = string.Format(
                    this.vldFullName.ErrorMessage, GeneralSystemManager.GetSystemName(2));
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
            this.Page.Validate();
            if (!this.Page.IsValid)
            {
                return;
            }

            // Получу текуший статус пользователя
            UserAccount.UserAccountStatusEnum status = this.CurrentUser.status;
            this.UpdateUser();

            // Если статус пользователя изменился на "на доработке" - отправлю уведомление
            if (this.CurrentUser.status == UserAccount.UserAccountStatusEnum.Consideration
                && this.CurrentUser.status != status)
            {
                this.SendNotification();
            }

            // Выполню действия после успешной регистрации
            this.ProcessSuccess();
        }

        private void ProcessSuccess()
        {
            // Перейду на страницу успеха
            this.Response.Redirect(SuccessUri, true);
        }

        private void SendNotification()
        {
            // Подготовлю email сообщение 
            var template = new EmailTemplate(EmailTemplateTypeEnum.Consideration);
            EmailMessage message = template.ToEmailMessage();
            message.To = this.CurrentUser.email;
            message.Params = Utility.CollectEmailMetaVariables(this.CurrentUser);

            // Отправлю уведомление
            TaskManager.SendEmail(message);
        }

        private void UpdateUser()
        {
            this.CurrentUser.RequestedOrganization.RCModelId = this.rcModel;
            this.CurrentUser.RequestedOrganization.RCDescription = this.rcDescriptionText;

            this.CurrentUser.RequestedOrganization.OrgType = string.IsNullOrEmpty(this.ddlOrgType.SelectedValue) ? null : new CatalogElement(Convert.ToInt32(this.ddlOrgType.SelectedValue));

            this.CurrentUser.lastName = this.txtFullName.Text.Trim();
            this.CurrentUser.phone = this.txtPhone.Text.Trim();
            this.CurrentUser.email = this.CurrentUser.login;
            this.CurrentUser.RequestedOrganization.LawAddress = this.txtOrganizationAddress.Text.Trim();
            this.CurrentUser.RequestedOrganization.DirectorFullName = this.txtOrganizationChiefName.Text.Trim();
            this.CurrentUser.RequestedOrganization.Fax = this.txtOrganizationFax.Text.Trim();
            this.CurrentUser.RequestedOrganization.KPP = this.tbKPP.Text.Trim();
            this.CurrentUser.RequestedOrganization.INN = this.tbINN.Text.Trim();
            this.CurrentUser.RequestedOrganization.OGRN = this.tbOGRN.Text.Trim();
            this.CurrentUser.RequestedOrganization.OwnerDepartment = this.txtOrganizationFounderName.Text.Trim();
            this.CurrentUser.RequestedOrganization.FullName = this.txtOrganizationName.Text.Trim();
            this.CurrentUser.RequestedOrganization.Phone = this.txtOrganizationPhone.Text.Trim();
            this.CurrentUser.RequestedOrganization.Region =
                new CatalogElement(Convert.ToInt32(this.ddlOrganizationRegion.SelectedItem.Value));

            if (this.ddlOrgType.SelectedValue == "1" || this.ddlOrgType.SelectedValue == "2")
            {
                this.CurrentUser.RequestedOrganization.ReceptionOnResultsCNE = this.rblReceptionOnResultsCNE.SelectedValue == string.Empty ? (int?)null : Convert.ToInt32(this.rblReceptionOnResultsCNE.SelectedValue);
            }
            else
            {
                this.CurrentUser.RequestedOrganization.ReceptionOnResultsCNE = null;
            }


            if (this.hfEtalonOrgID.Value == string.Empty)
            {
                this.CurrentUser.RequestedOrganization.OrganizationId = null;
            }
            else
            {
                this.CurrentUser.RequestedOrganization.OrganizationId = int.Parse(this.hfEtalonOrgID.Value);
            }

            // TODO: в метод сохранения передаем заявку, к которой пользователь привязан и уже существующий доступ к системам.
            OrgRequest orgRequest = OrgRequestManager.GetRequest(this.CurrentUser.RequestedOrganization.Id);
            OrgUserBrief orgUserBrief = orgRequest.LinkedUsers.SingleOrDefault(x => x.Login == this.CurrentUser.login);
            if (orgUserBrief == null)
            {
                this.Page.AddError("Данный пользователь не привязан к заявке. Сохранение невозможно.");
                return;
            }

            // Список идентификаторов систем, в которых регистрируется пользователь
            var systemsId = GeneralSystemManager.AccessedSystemsId(this.CurrentUser.login);

            // Заполнения списка идентификаторов систем для пользователя
            this.CurrentUser.SystemsId = systemsId;

            // Создание списка пользователь, нужно что бы вызвать процедуру обновления
            var user = new List<OrgUser> { this.CurrentUser };
            OrgUserDataAccessor.UpdateUserAccount(user, this.CurrentUser.RequestedOrganization.Id, false);
        }

        #endregion
    }
}