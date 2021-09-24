using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Fbs.Core;
using Fbs.Utility;
using Fbs.Web.Administration.Organizations;
using Fbs.Core.Users;
using Fbs.Core.Organizations;
using Fbs.Core.CatalogElements;

namespace Fbs.Web.Administration.Accounts.Users
{
    public partial class Edit : BasePage
    {
        private const string SuccessUri = "/Administration/Accounts/Users/EditSuccess.aspx?login={0}";

        //UserAccount mCurrentUser;

        //public UserAccount CurrentUser
        //{
        //    get
        //    {
        //        if (mCurrentUser == null)
        //            mCurrentUser = UserAccount.GetUserAccount(Login);

        //        if (mCurrentUser == null)
        //            throw new NullReferenceException(String.Format("Пользователь \"{0}\" не найден",
        //                Login));

        //        return mCurrentUser;
        //    }
        //}

        OrgUser mCurrentUser;

        public OrgUser CurrentUser
        {
            get
            {
                if (mCurrentUser == null)
                    mCurrentUser = OrgUserDataAccessor.Get(Login);

                if (mCurrentUser == null)
                    throw new NullReferenceException(String.Format("Пользователь \"{0}\" не найден",
                        Login));

                return mCurrentUser;
            }
        }

        public string Login
        {
            get
            {
                if (string.IsNullOrEmpty(Request.QueryString["login"]))
                    return string.Empty;

                return Request.QueryString["login"];
            }
        }

        public bool IsRegistrationDocumentExists
        {
            get { return CurrentUser.registrationDocument != null; }
        }

        public bool UserIsLinkedToOrg
        {
            get
            {
                return CurrentUser.RequestedOrganization.OrganizationId != null;
            }
        }

        /// <summary>
        /// Кнопка видна, если пользователь не привязан к организации и у редактора - право админа.
        /// </summary>
        private bool AddToOrgsVisible
        {
            get { return ((!UserIsLinkedToOrg) && (User.IsInRole("CreateOrganization"))); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
            {
                //vldIpAddresses.Enabled = !cbNoFixedIp.Checked;
                return;
            }

             
            BtAddToOrgs.Visible = AddToOrgsVisible;

            // заполню соответствующие контролы
            litUserName.Text = CurrentUser.login;

            litStatus.Text = UserAccountExtentions.GetUserAccountStatusName(CurrentUser.status);
            litStatusDescription.Text = UserAccountExtentions.GetEditStatusDescription(CurrentUser);

            txtOrganizationChiefName.Text = CurrentUser.RequestedOrganization.DirectorFullName;

            txtEmail.Text = CurrentUser.email;
            txtPhone.Text = CurrentUser.phone;
            txtFullName.Text = CurrentUser.GetFullName();
            //txtIpAddresses.Text = CurrentUser.GetIpAddressesAsEdit();
            txtOrganizationAddress.Text = CurrentUser.RequestedOrganization.LawAddress;
            txtOrganizationFax.Text = CurrentUser.RequestedOrganization.Fax;
            txtOrganizationFounderName.Text = CurrentUser.RequestedOrganization.OwnerDepartment;

            ddlOrganizationRegion.DataSource = RegionDataAcessor.GetAllInEtalon(null);
            ddlOrganizationRegion.DataBind();
            ddlOrganizationRegion.SelectedValue = CurrentUser.RequestedOrganization.Region.Id.ToString();

            ddlEducationInstitutionType.DataSource = CatalogDataAcessor.GetAll(CatalogDataAcessor.Catalogs.OrganizationTypes);// GetEducationInstitutionTypes();
            ddlEducationInstitutionType.DataBind();
            ddlEducationInstitutionType.SelectedValue = Convert.ToString(CurrentUser.RequestedOrganization.OrgType.Id);

            DLLOrgKind.DataSource = CatalogDataAcessor.GetAll(CatalogDataAcessor.Catalogs.OrganizationKinds);
            DLLOrgKind.DataBind();
            if (CurrentUser.RequestedOrganization.Kind.Id != null)
            {
                DLLOrgKind.SelectedValue = Convert.ToString(CurrentUser.RequestedOrganization.Kind.Id);
            }
            else
            {
                DLLOrgKind.SelectedValue = "";
            }

            txtOrganizationPhone.Text = CurrentUser.RequestedOrganization.Phone;



            TBAccred.Text = CurrentUser.RequestedOrganization.AccreditationSertificate;
            TbDirectorPosition.Text = CurrentUser.RequestedOrganization.DirectorPosition;
            TBEMail.Text = CurrentUser.RequestedOrganization.EMail;
            TBSite.Text = CurrentUser.RequestedOrganization.Site;
            TbFactAddress.Text = CurrentUser.RequestedOrganization.FactAddress;
            TBINN.Text = CurrentUser.RequestedOrganization.INN;
            if (TBINN.Text == "")
            {
                VReqINN.Enabled = false;
            }
            TBOGRN.Text = CurrentUser.RequestedOrganization.OGRN;
            if (TBOGRN.Text == "")
            {
                VReqOGRN.Enabled = false;
            }
            TbPhoneCode.Text = CurrentUser.RequestedOrganization.PhoneCityCode;
            TBShortName.Text = CurrentUser.RequestedOrganization.ShortName;

            ChIsFilial.Checked = CurrentUser.RequestedOrganization.IsFilial;
            if (CurrentUser.RequestedOrganization.IsPrivate)
                DDLOPF.SelectedValue = "1";
            else
                DDLOPF.SelectedValue = "0";
            if (CurrentUser.RequestedOrganization.Kind.Id != null)
                DLLOrgKind.SelectedValue = CurrentUser.RequestedOrganization.Kind.Id.ToString();
            else DLLOrgKind.SelectedValue = "0";

            //------------------------------------------------------------------------------
            Organization currentEtalonOrg = GetOrg();
            // если установлено соответствие с эталонной организацией
            if (currentEtalonOrg != null)
            {
                OrganizationName_hiddenField.Value = currentEtalonOrg.Id.ToString();

                OrganizationName_txt.Text = currentEtalonOrg.FullName;
                OrganizationName_txt.Visible = false;
                OrganizationName_hyperlynk.Visible = true;
                OrganizationName_hyperlynk.Text = currentEtalonOrg.FullName;
                OrganizationName_hyperlynk.NavigateUrl = string.Format("../../Organizations/Administrators/OrgCard_Edit.aspx?OrgID={0}", currentEtalonOrg.Id);

                ddlOrganizationRegion.Enabled = false;
                ddlOrganizationRegion.SelectedValue = currentEtalonOrg.Region.Id.ToString();
                ddlOrganizationRegion.ToolTip = "Поле зафиксировано выбором организации";


                ddlEducationInstitutionType.Enabled = false;
                ddlEducationInstitutionType.SelectedValue = currentEtalonOrg.OrgType.Id.ToString();
                ddlEducationInstitutionType.ToolTip = "Поле зафиксировано выбором организации";
            }
            else
            {
                OrganizationName_txt.Text = CurrentUser.RequestedOrganization.FullName;
            }
            btnChangeOrg.PostBackUrl = string.Format("/SelectOrg.aspx?BackUrl={0}&RegID={1}&OrgTypeID={2}&OrgName={3}",
                                  System.Web.HttpUtility.UrlEncode("./Administration/Accounts/Users/Edit.aspx?login=" + CurrentUser.login),
                                  ddlOrganizationRegion.SelectedValue,
                                  ddlEducationInstitutionType.SelectedValue,
                                  OrganizationName_txt.Text);
            //------------------------------------------------------------------------------



            // Нажатие на кнопку не должно инициировать проверку изменений на форме
            btnUpdate.Attributes.Add("handleClick", "false");
            BtAddToOrgs.Attributes.Add("handleClick", "false");

            // Установлю заголовок страницы
            this.PageTitle = string.Format("Редактирование “{0}”", CurrentUser.login);

            ToggleValidators();//Отключить валидаторы для пользователей, привязанных к организации из справочника
        }

        private void ToggleValidators()
        {
            if (UserIsLinkedToOrg)
            {
                //VRegexINN.Enabled = false;
                //VRegexOGRN.Enabled = false;
               // VRegexUserEMail.Enabled = false;
                VReqDirectorName.Enabled = false;
                VReqFactAddress.Enabled = false;
                VReqFounderName.Enabled = false;
                VReqINN.Enabled = false;
                VReqLawAddress.Enabled = false;
                VReqOGRN.Enabled = false;
                VReqOGRN.Enabled = false;
                VReqOrgKind.Enabled = false;
                VReqOrgPhone.Enabled = false;
                //VReqUserEMail.Enabled = false;
                //VReqUserName.Enabled = false;
                //VReqUserPhone.Enabled = false;
            }
        }

        /// <summary>
        /// Получаем организацию, к которой привязан пользователь
        /// </summary>
        /// <returns></returns>
        private Organization GetOrg()
        {
            int  OrgId = 0;
            //Пользователь привязывается к организации - организация выбрана поиском из справочника (кнопка "Выбрать")
            if (!string.IsNullOrEmpty(Request.QueryString["OrgID"]))
            {
                int.TryParse(Request.QueryString["OrgID"], out OrgId);
            }
            //Пользователь уже привязан к организации
            else if (CurrentUser.RequestedOrganization.OrganizationId != null)
            {
                OrgId = (int)CurrentUser.RequestedOrganization.OrganizationId;
            }
            if (OrgId != 0)
                return OrganizationDataAccessor.Get(OrgId);
            else
                return null;//Обнаружить организацию не удалось - пользователь не был привязан и не был произведен поиск
        }


        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            // проверю валидность контролов страницы
            if (!Page.IsValid)
                return;

            // регистрирую нового пользователя
            UpdateUser(CurrentUser);

            // выполню действия после успешной регистрации
            ProcessSuccess();
        }

        /// <summary>
        /// "Собираем" пользователя из контролов страницы
        /// </summary>
        /// <param name="user"></param>
        private void UpdateUser(OrgUser user)
        {
            user.RequestedOrganization.AccreditationSertificate = TBAccred.Text.Trim();
            user.RequestedOrganization.DirectorPosition = TbDirectorPosition.Text.Trim();
            user.RequestedOrganization.EMail = TBEMail.Text.Trim();
            user.RequestedOrganization.Site = TBSite.Text.Trim();
            user.RequestedOrganization.FactAddress = TbFactAddress.Text.Trim();
            user.RequestedOrganization.INN = TBINN.Text.Trim();
            user.RequestedOrganization.OGRN = TBOGRN.Text.Trim();
            user.RequestedOrganization.PhoneCityCode = TbPhoneCode.Text.Trim();
            user.RequestedOrganization.ShortName = TBShortName.Text.Trim();

            if (DLLOrgKind.SelectedItem.Value != "")
                user.RequestedOrganization.Kind = new CatalogElement(Convert.ToInt32(DLLOrgKind.SelectedItem.Value));
            user.RequestedOrganization.IsFilial = ChIsFilial.Checked;
            user.RequestedOrganization.IsPrivate = DDLOPF.SelectedValue == "1";

            user.lastName = txtFullName.Text.Trim();
            user.email = txtEmail.Text.Trim();
            //user.SetIpAddressesAsEdit(txtIpAddresses.Text);
            user.RequestedOrganization.LawAddress = txtOrganizationAddress.Text.Trim();
            user.RequestedOrganization.DirectorFullName = txtOrganizationChiefName.Text.Trim();
            user.RequestedOrganization.Fax = txtOrganizationFax.Text.Trim();
            user.RequestedOrganization.OwnerDepartment = txtOrganizationFounderName.Text.Trim();
            user.RequestedOrganization.FullName = OrganizationName_txt.Text.Trim();
            user.RequestedOrganization.Phone = txtOrganizationPhone.Text.Trim();
            user.RequestedOrganization.Region = new CatalogElement(Convert.ToInt32(ddlOrganizationRegion.SelectedItem.Value));
            user.phone = txtPhone.Text.Trim();
            //user.HasFixedIp = !cbNoFixedIp.Checked;
            //user.HasCrocEgeIntegration = cbHasCrocEgeIntegration.Checked;
            if (String.IsNullOrEmpty(ddlEducationInstitutionType.SelectedValue))
                user.RequestedOrganization.OrgType = null;
            else
                user.RequestedOrganization.OrgType = new CatalogElement(
                    Convert.ToInt32(ddlEducationInstitutionType.SelectedValue));

            if ((OrganizationName_hiddenField.Value == "0") || (OrganizationName_hiddenField.Value == ""))
                user.RequestedOrganization.OrganizationId = null;
            else
                user.RequestedOrganization.OrganizationId = int.Parse(OrganizationName_hiddenField.Value);

            // если задан документ регистрации, то добавлю его
            if (!String.IsNullOrEmpty(fuRegistrationDocument.FileName) && fuRegistrationDocument.FileBytes.Length > 0)
            {
               // throw new NotImplementedException();
                user.registrationDocument = fuRegistrationDocument.FileBytes;
                user.registrationDocumentContentType = fuRegistrationDocument.PostedFile.ContentType;
                //user.registrationDocument .ContentType = fuRegistrationDocument.PostedFile.ContentType;
                //user.registrationDocument .Extension = Path.GetExtension(fuRegistrationDocument.FileName);
                //user.RegistrationDocument.Document = fuRegistrationDocument.FileBytes;
            }

            // Сохраню статус
            UserAccount.UserAccountStatusEnum prevStatus = user.status;

            OrgUserDataAccessor.UpdateOrCreate(user);

            // Если статус изменился на "на согласовании" - отправлю уведомление
            if (user.status == UserAccount.UserAccountStatusEnum.Consideration &&
                    user.status != prevStatus)
                SendNotification(user);
        }

        private void SendNotification(OrgUser user)
        {
            // Подготовлю email сообщение 
            EmailTemplate template = new EmailTemplate(EmailTemplateTypeEnum.Consideration);
            EmailMessage message = template.ToEmailMessage();
            message.To = user.email;
            message.Params = Utility.CollectEmailMetaVariables(user);

            // Отправлю уведомление
            TaskManager.SendEmail(message);
        }

        private void ProcessSuccess()
        {
            // перейду на страницу успеха
            Response.Redirect(String.Format(SuccessUri, CurrentUser.login), true);
        }

        //Добавляем организацию из заявки в справочник и связываем заявку с ней
        protected void BtAddToOrgs_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;
            //Пользователь уже был привязан к организации из справочника, ее добавление заново - дубль
            if (CurrentUser.RequestedOrganization.OrganizationId > 0)
            {
                throw new Exception("Попытка создания дублирующей организации в справочнике заблокирована");
            }

            UpdateUser(CurrentUser);//Обновим пользователя

            int RequestId = CurrentUser.RequestedOrganization.Id;//Запомним 
            CurrentUser.RequestedOrganization.Id = 0;//Нужно чтобы орг. добавилась, а не обновилась
            //Создаем
            int NewOrgId = OrganizationDataAccessor.UpdateOrCreate((Organization)CurrentUser.RequestedOrganization, false);
            //Связываем заявку с организацией
            OrganizationDataAccessor.BindRequestToOrganization(RequestId, NewOrgId);

            Response.Redirect(String.Format(SuccessUri, CurrentUser.login));
        }
    }
}
