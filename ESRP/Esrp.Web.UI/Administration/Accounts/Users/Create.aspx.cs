namespace Esrp.Web.Administration.Accounts.Users
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Esrp.Core;
    using Esrp.Core.CatalogElements;
    using Esrp.Core.Organizations;
    using Esrp.Core.Systems;
    using Esrp.Core.Users;
    using Esrp.Utility;

    using Esrp.Services;
    using System.Security.Cryptography;
    using Esrp.Web.Extentions;
    using System.Text;

    using Esrp.Web.Administration.SqlConstructor.UserAccounts;

    /// <summary>
    /// The create.
    /// </summary>
    public partial class Create : Page
    {
        #region Constants and Fields

        private const string SuccessUri = "/Administration/Accounts/Users/CreateSuccess.aspx?login={0}";

        private readonly InformationSystemsService informationSystemsService = new InformationSystemsService();

        private Organization FoundedOrg_;

        #endregion

        #region Properties

        private Organization FoundedOrg
        {
            get
            {
                return this.FoundedOrg_ ?? (this.FoundedOrg_ = OrganizationDataAccessor.Get(this.OrgID));
            }
        }

        private int OrgID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Request.QueryString["OrgID"]))
                {
                    int orgId;
                    if (int.TryParse(this.Request.QueryString["OrgID"], out orgId))
                    {
                        return orgId;
                    }
                }

                return 0;
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

            this.chblGroup.DataSource = SqlConstructor_GetUsersOU.GetAvailableOUGroups(this.User.Identity.Name);
            this.chblGroup.DataBind();

            if (this.OrgID == 0)
            {
                // Пользователь еще не выбирал организацию
                this.RCModelvalidator.Enabled = false;
            }
            else
            {
                if (!this.UserCreatesNewOrg())
                {
                    // если организация выбрана
                    this.ShowOrgFounded();
                }
            }

            // Список checkbox: Информационные системы
            this.cblSystems.DataSource = this.informationSystemsService.GetAvailableSystems();
            this.cblSystems.DataBind();

            // Нажатие на кнопку не должно инициировать проверку изменений на форме
            this.btnUpdate.Attributes.Add("handleClick", "false");
            this.rvFbs1.ErrorMessage = string.Format(this.rvFbs1.ErrorMessage, GeneralSystemManager.GetSystemName(2));

            // Если организация не выбрана или тип организации не ВУЗ или ССУЗ, то...
            if (!CanAccessFbd(this.FoundedOrg))
            {
                // Запрещаем выбирать "ФИС ЕГЭ и приема"
                var checkboxAccessFbd = this.cblSystems.Items.FindByValue(Constants.Systems.FBD.ToString());
                if (checkboxAccessFbd != null)
                {
                    checkboxAccessFbd.Enabled = false;
                    checkboxAccessFbd.Selected = false;
                }
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

            try
            {
                // регистрирую нового пользователя
                string password;
                // UserAccount user;
                OrgUser user;

                bool sendEmail;

                UpdateUser(out user, out password, out sendEmail);
                // выполню действия после успешной регистрации
                ProcessSuccess(user, password, sendEmail);
            }
            catch (Exception ex)
            {
                customErrors.ErrorMessage = ex.Message;
                customErrors.IsValid = false;
            }

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

        private void ProcessSuccess(OrgUser user, string password, bool sendEMail)
        {
            if (sendEMail)
            {
                // подготовлю email сообщение 
                var template = new EmailTemplate(EmailTemplateTypeEnum.RegistrationSetPassword);
                EmailMessage message = template.ToEmailMessage();
                message.To = user.email;

                //message.Params = Utility.CollectEmailMetaVariables(user, password, Utility.GetSeverPath(this.Request));
                string toEncrypt = String.Format("{0};{1};{2}", user.login, DateTime.Now.ToString(), user.passwordHash);
                RSACryptoServiceProvider Rsa = Helper.GetCurrentRsaProvider(true);

                toEncrypt = Uri.EscapeDataString(Convert.ToBase64String(Rsa.Encrypt(Encoding.Default.GetBytes(toEncrypt), false)));
                message.Params = Utility.CollectEmailMetaVariables(user, user.password, Utility.GetSeverPath(this.Request), String.Format("{0}/WebUserAccount/ResetPassword.aspx?userId={1}&isNew=1", Utility.GetSeverPath(this.Request), toEncrypt));

                // отправлю email сообщение
                TaskManager.SendEmail(message);
            }
            // перейду на страницу успеха
            this.Response.Redirect(string.Format(SuccessUri, user.login), true);
        }

        private void ShowOrgFounded()
        {
            this.hiddenOrgTypes.Value = this.FoundedOrg.OrgType.Id.ToString();
            this.txtOrganizationName.Text = this.FoundedOrg.FullName;
            this.txtOrganizationName.Enabled = false;
            this.txtOrganizationName.ToolTip = "Название организации зафиксировано.";

            this.ddlOrganizationRegion.SelectedValue = this.FoundedOrg.Region.Id.ToString();
            this.ddlOrganizationRegion.Enabled = false;
            this.ddlOrganizationRegion.ToolTip = "Регион организации зафиксирован.";

            this.txtOrganizationFounderName.Text = this.FoundedOrg.DepartmentShortName;
            this.txtOrganizationFounderName.Enabled = false;

            if (!(this.hiddenOrgTypes.Value == Constants.OrganizationType2010.VUZ.ToString() || this.hiddenOrgTypes.Value == Constants.OrganizationType2010.SSUZ.ToString()))
            {
                this.RCModelvalidator.Enabled = false;
            }
        }

        private void UpdateUser(out OrgUser user, out string password, out bool sendEmail)
        {
            // сгенерирую пароль               
            password = Utility.GeneratePassword();

            // Fbs.Core.Users.OrgUserDataAccessor.UpdateOrCreate()
            user = new OrgUser
                {
                    password = password,
                    lastName = this.txtFullName.Text.Trim(),
                    email = this.txtEmail.Text.Trim(),
                    status = UserAccount.UserAccountStatusEnum.Registration,
                    RequestedOrganization =
                        {
                            OrgType = new CatalogElement((int)this.FoundedOrg.OrgType.Id),
                            LawAddress = this.txtOrganizationAddress.Text.Trim(),
                            DirectorFullName = this.txtOrganizationChiefName.Text.Trim(),
                            Fax = this.txtOrganizationFax.Text.Trim(),
                            OwnerDepartment = this.txtOrganizationFounderName.Text.Trim(),
                            FullName = this.txtOrganizationName.Text.Trim(),
                            Phone = this.txtOrganizationPhone.Text.Trim(),
                            Region = new CatalogElement(Convert.ToInt32(this.ddlOrganizationRegion.SelectedItem.Value))

                        },
                    phone = this.txtPhone.Text.Trim(),
                    position = this.txtPosition.Text.Trim()
                };

            user.RequestedOrganization.RCModelId = this.BehaviorModelList.SelectedValue == string.Empty
                                                      ? 999
                                                      : Convert.ToInt32(this.BehaviorModelList.SelectedValue);

            if (user.RequestedOrganization.RCModelId == 999)
            {
                user.RequestedOrganization.RCModelName = string.Empty;
                user.RequestedOrganization.RCDescription = this.AnotherRCModelName.Text;
            }
            else
            {
                user.RequestedOrganization.RCModelName = this.BehaviorModelList.SelectedItem.Text;
                user.RequestedOrganization.RCDescription = string.Empty;
            }

            // 5);//"Другое"

            // если задан документ регистрации, то добавлю его
            if (!string.IsNullOrEmpty(this.fuRegistrationDocument.FileName)
                && this.fuRegistrationDocument.FileBytes.Length > 0)
            {
                user.registrationDocumentContentType = this.fuRegistrationDocument.PostedFile.ContentType;

                // user.RegistrationDocument.Extension = Path.GetExtension(fuRegistrationDocument.FileName);
                user.registrationDocument = this.fuRegistrationDocument.FileBytes;
            }

            var items = this.cblSystems.Items;
            var checkboxAccessFbd = items.FindByValue(Constants.Systems.FBD.ToString());

            // если некорректная организация, снимаем ФБД
            if (!CanAccessFbd(this.FoundedOrg))
            {
                checkboxAccessFbd.Selected = false;
            }

            this.Page.Validate();
            if (!this.Page.IsValid)
            {
                sendEmail = false;
                return;
            }

            // Проставляем организацию у пользователя
            user.RequestedOrganization.OrganizationId = this.FoundedOrg.Id;

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
            OrgUserDataAccessor.UpdateUserAccount(new List<OrgUser> { user }, null, isOlympicStaff);

            sendEmail = true;
            if ((checkboxAccessFbd != null) && (checkboxAccessFbd.Selected))
            {
                foreach (ListItem groupItem in chblGroup.Items)
                {
                    if ((groupItem.Selected) && (groupItem.Value == FbdManager.OlympicStaffGroupCode))
                    {
                        sendEmail = false;
                    }
                    if (groupItem.Selected)
                    {
                        SqlConstructor_GetUsersIS.SetUserGroup(user.login, groupItem.Value, false);
                    }
                    else
                    {
                        SqlConstructor_GetUsersIS.DeleteUserGroup(user.login, groupItem.Value);
                    }
                }
            }
        }

        private bool UserCreatesNewOrg()
        {
            return this.OrgID <= 0;
        }

        #endregion
    }
}