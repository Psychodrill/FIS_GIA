namespace Esrp.Web.Controls
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Esrp.Core;
    using Esrp.Core.CatalogElements;
    using Esrp.Core.Organizations;
    using Esrp.Core.Systems;
    using Esrp.Core.Users;
    using Esrp.Utility;

    using Esrp.Web.ViewModel.InformationSystems;

    using FogSoft.Helpers;
    using System.Security.Cryptography;
    using Esrp.Web.Extentions;
    using System.Text;

    /// <summary>
    /// Контрол добавления информационных систем для регистрации
    /// </summary>
    public partial class InformationSystemsRegistration : UserControl
    {
        #region Constants and Fields

        /// <summary>
        /// The register send mail user list.
        /// </summary>
        public List<string> RegisterSendMailUserList = new List<string>();

        /// <summary>
        /// Список измененных itemListView
        /// </summary>
        private List<InformationSystemsRegistrationView> UpdateInformationSystemsRegistrashionList = new List<InformationSystemsRegistrationView>();

        /// <summary>
        /// Страница "Успешной регистрации"
        /// </summary>
        private const string SuccessUri = "/Profile/RegistrationSuccess.aspx";

        /// <summary>
        /// Словарь для хранения элементов типа DictionaryReqField
        /// </summary>
        private readonly List<DictionaryReqField> dictionary = new List<DictionaryReqField>();

        /// <summary>
        /// Список пар: логин зарегистрированного пользователя : пароль
        /// </summary>
        private Dictionary<string, string> users = new Dictionary<string, string>(); 

        private List<OrgUser> usersList = new List<OrgUser>(); 

        private Organization FoundedOrg_;

        #endregion

        #region Public Properties

        /// <summary>
        /// ClientID чекбокса "Доступ к ФБД"
        /// </summary>
        public string AllowFbd
        {
            get
            {
                return this.ViewState["AllowFbd"] != null ? this.ViewState["AllowFbd"].ToString() : null;
            }

            set
            {
                this.ViewState["AllowFbd"] = value;
            }
        }

        /// <summary>
        /// ClientID чекбокса "Доступ к ФБС"
        /// </summary>
        public string AllowFbs
        {
            get
            {
                return this.ViewState["AllowFbs"] != null ? this.ViewState["AllowFbs"].ToString() : null;
            }

            set
            {
                this.ViewState["AllowFbs"] = value;
            }
        }

        /// <summary>
        /// Идентификатор организации в виде строки
        /// </summary>
        public string OrgId
        {
            get
            {
                return this.ViewState["OrgId"].ToString();
            }

            set
            {
                this.ViewState["OrgId"] = value;
            }
        }

        /// <summary>
        /// ClientID чекбокса "Совпадает с ФБС"
        /// </summary>
        public string SameFbs
        {
            get
            {
                return this.ViewState["SameFbs"] != null ? this.ViewState["SameFbs"].ToString() : null;
            }

            set
            {
                this.ViewState["SameFbs"] = value;
            }
        }

        /// <summary>
        /// ClientID чекбокса "Совпадает с ФБС" при смене пользователя
        /// </summary>
        public string SameFbsChange
        {
            get
            {
                return this.ViewState["SameFbsChange"] != null ? this.ViewState["SameFbsChange"].ToString() : null;
            }

            set
            {
                this.ViewState["SameFbsChange"] = value;
            }
        }

        /// <summary>
        /// Gets _fbdAuthorizedStaff.
        /// </summary>
        public UserAccount _fbdAuthorizedStaff
        {
            get
            {
                var id = 0;
                if (!string.IsNullOrEmpty(this.OrgId))
                {
                    int.TryParse(this.OrgId, out id);
                }

                var fbdAuthorizedStaff = FbdManager.GetLastCreatedUserByGroup(
                    id, FbdManager.AuthorizedStaffGroupCode);

                return fbdAuthorizedStaff;
            }
        }

        /// <summary>
        /// Gets fbdAuthorizedStaffLastName.
        /// </summary>
        public string fbdAuthorizedStaffLastName
        {
            get
            {
                if (this._fbdAuthorizedStaff != null)
                {
                    return this._fbdAuthorizedStaff.LastName;
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets fbdAuthorizedStaffLogin.
        /// </summary>
        public string fbdAuthorizedStaffLogin
        {
            get
            {
                if (this._fbdAuthorizedStaff != null)
                {
                    return this._fbdAuthorizedStaff.Login;
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets fbdAuthorizedStaffPosition.
        /// </summary>
        public string fbdAuthorizedStaffPosition
        {
            get
            {
                if (this._fbdAuthorizedStaff != null)
                {
                    return this._fbdAuthorizedStaff.Position;
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets fbdAuthorizedStaffStatus.
        /// </summary>
        public string fbdAuthorizedStaffStatus
        {
            get
            {
                if (this._fbdAuthorizedStaff != null)
                {
                    return this._fbdAuthorizedStaff.Status.ToString();
                }

                return string.Empty;
            }
        }

        public bool EnabledFbdBlock
        {
            get
            {
                return this.FoundedOrg != null && this.FoundedOrg.OrgType.Id.HasValue && this.FoundedOrg.OrgType.Id.Value != (int)OrganizationType.VUZ && this.FoundedOrg.OrgType.Id.Value != (int)OrganizationType.SSUZ;
            }
        }

        #endregion

        #region Properties

        private Organization FoundedOrg
        {
            get
            {
                if (this.FoundedOrg_ == null)
                {
                    this.FoundedOrg_ = OrganizationDataAccessor.Get(this.orgID);
                }

                return this.FoundedOrg_;
            }
        }

        /// <summary>
        /// Идентификатор организации
        /// </summary>
        private int orgID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.OrgId))
                {
                    int orgId;
                    if (int.TryParse(this.Page.Request.QueryString["OrgID"], out orgId))
                    {
                        return orgId;
                    }
                }

                return 0;
            }
        }

        #endregion

        #region Public Methods and Operators

        private void AddUserToUsersList(OrgUser user)
        {
            var existing = this.usersList.FirstOrDefault(x => x.email == user.email);
            if (existing == null)
            {
                this.usersList.Add(user);
            }
            else
            {
                existing.SystemsId.AddRange(user.SystemsId);
                existing.login = user.login;
                existing.passwordHash = existing.SystemsId.Contains("1")?existing.passwordHash:user.passwordHash;
                existing.lastName = user.lastName;
                existing.firstName = user.firstName;
                existing.patronymicName = user.patronymicName;
                existing.phone = user.phone;
                existing.email = user.email;
                existing.position = user.position;
                existing.ipAddresses = user.ipAddresses;
                existing.registrationDocument = user.registrationDocument;
                existing.registrationDocumentContentType = user.registrationDocumentContentType;
                existing.editorLogin = user.editorLogin;
                existing.editorIp = user.editorIp;
                existing.password = existing.SystemsId.Contains("1") ? existing.password : user.password;
                existing.hasFixedIp = user.hasFixedIp;
            }
        }


        /// <summary>
        /// Сохранение информационных систем
        /// </summary>
        /// <param name="commonUser">
        /// Пользователь, в котором содержится общая информация о организации
        /// </param>
        public void Save(OrgUser commonUser)
        {
            
            var hasSystem = this.HasSelectSystem();
            if (hasSystem == false)
            {
                this.Page.AddError("Выберите хотя бы одну Информационную систему для регистрации пользователя");
                return;
            }

            var systemFbs = this.UpdateInformationSystemsRegistrashionList.Where(x => x.SystemId == 2).FirstOrDefault();
            var systemFbd = this.UpdateInformationSystemsRegistrashionList.Where(x => x.SystemId == 3).FirstOrDefault();

            var user = new OrgUser();

            if (systemFbs != null && systemFbd != null && systemFbd.AccessToSystem && systemFbs.AccessToSystem && systemFbd.SameDataAsFbs)
            {
                user = this.CreateUserFromRegistrationData(UserRegistrationType.FbsUser, systemFbs, systemFbd);
                this.FillOrgUserWithOrgData(commonUser, user);
                user.SystemsId.Add("2");
                user.SystemsId.Add("3");
                this.AddUserToUsersList(user);
            }
            else
            {
                if (systemFbd != null && systemFbd.AccessToSystem)
                {
                    user = this.CreateUserFromRegistrationData(UserRegistrationType.FbdUser, systemFbs, systemFbd);
                    this.FillOrgUserWithOrgData(commonUser, user);
                    user.SystemsId.Add("3");
                    this.AddUserToUsersList(user);
                }

                if (systemFbs != null && systemFbs.AccessToSystem)
                {
                    user = this.CreateUserFromRegistrationData(UserRegistrationType.FbsUser, systemFbs, systemFbd);
                    this.FillOrgUserWithOrgData(commonUser, user);
                    user.SystemsId.Add("2");
                    this.AddUserToUsersList(user);      
                }
            }

            foreach (InformationSystemsRegistrationView informationSystemsRegistrationView in this.UpdateInformationSystemsRegistrashionList)
                {
                    if (informationSystemsRegistrationView.SystemId != 2 && informationSystemsRegistrationView.SystemId != 3 && informationSystemsRegistrationView.AccessToSystem)
                    {
                        user = this.CreateUserFromRegistrationData(informationSystemsRegistrationView);
                        this.FillOrgUserWithOrgData(commonUser, user);
                        user.SystemsId.Add(informationSystemsRegistrationView.SystemId.ToString());
                       
                        this.AddUserToUsersList(user);
                    }
                }
            try
            {
                this.usersList = OrgRequestManager.RegisteredRequest(this.usersList, null, false);
            }
            catch (SqlException ex)
            {
                if (ex.Message.StartsWith("001"))
                {
                    var processMessage = ex.Message.Remove(0, 3).Split(new[] { ".001" }, StringSplitOptions.RemoveEmptyEntries);
                    this.Page.AddError(processMessage[0]);
                    return;
                }

                throw;
            }
            catch (Exception ex2)
            {
                this.Page.AddError(ex2.Message);
                return;
            }
            
            this.AddUsers();
            this.SendMailOnSuccess(this.usersList);
            this.ProcessSuccess(this.usersList.Last());
        }

        /// <summary>
        /// 1) Просмотр всех контролов
        /// 2) Выбор из них только RequiredFieldValidator, RegularExpressionValidator и выбор соответствующих им ControlToValidate
        /// 3) Если ControlToValidate ReadOnly, то отключение валидацию
        /// </summary>
        public void UpdateValidators()
        {
            this.listView.SaveAndNoClear();
            var controls = this.Controls;
            foreach (Control control in controls)
            {
                this.FindControlForInformationSystemBlock(control);
            }

            var collection = this.dictionary;

            foreach (var dictionaryReqField in collection)
            {
                if (dictionaryReqField.HiddenField.Value != string.Empty)
                {
                    if (dictionaryReqField.RegularExpression != null)
                    {
                        dictionaryReqField.RegularExpression.Enabled = false;
                    }

                    if (dictionaryReqField.RequiredField != null)
                    {
                        dictionaryReqField.RequiredField.Enabled = false;
                    }
                }
                else
                {
                    if (dictionaryReqField.RegularExpression != null)
                    {
                        dictionaryReqField.RegularExpression.Enabled = true;
                    }

                    if (dictionaryReqField.RequiredField != null)
                    {
                        dictionaryReqField.RequiredField.Enabled = true;
                    }
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Функция для того чтобы показывать или скрывать элементы страницы
        /// </summary>
        /// <param name="eval">
        /// false - скрываем, true - показываем
        /// </param>
        /// <returns>
        /// строка none|пустая строка
        /// </returns>
        protected string CheckVisibility(bool eval)
        {
            if (eval == false)
            {
                return "none";
            }

            return string.Empty;
        }

        /// <summary>
        /// Событие происходит перед SelectMethod для ObjectDataSource
        /// </summary>
        /// <param name="sender"> Источник события </param>
        /// <param name="e"> параметры события </param>
        protected void OdsOnSelecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["orgId"] = this.OrgId;
        }

        /// <summary>
        /// Обработчик события, возникающего после Update
        /// Метод Update еще не происходил, составляем список грязных объектов для дальнейшего использования
        /// </summary>
        /// <param name="sender">
        /// Источник события
        /// </param>
        /// <param name="e">
        /// Параметры события
        /// </param>
        protected void OnUpdatingInformationSystem(object sender, ObjectDataSourceMethodEventArgs e)
        {
            IDictionary paramsFromPage = e.InputParameters;
            var changeInformationSystemRegistration = (InformationSystemsRegistrationView)paramsFromPage["informationSystemsRegistrationView"];
            this.UpdateInformationSystemsRegistrashionList.Add(changeInformationSystemRegistration);
            
            e.Cancel = true;
        }

        /// <summary>
        /// После вызова метода Update для ListView делаем чтобы не было перезагрузки страницы(сброса введенных данных)
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        protected void LvOnItemUpdated(object sender, ListViewUpdatedEventArgs e)
        {
            e.KeepInEditMode = true;
        }

        /// <summary>
        /// The lbl user email text.
        /// </summary>
        /// <param name="userEmailReadOnly">
        /// The user email read only.
        /// </param>
        /// <param name="sameDataAsFbsChecked">
        /// The same data as fbs checked.
        /// </param>
        /// <param name="systemId">
        /// The system id.
        /// </param>
        /// <returns>
        /// The lbl user email text.
        /// </returns>
        protected string lblUserEmailText(bool userEmailReadOnly, bool sameDataAsFbsChecked, int systemId)
        {
            if (systemId != 3)
            {
                return "<b>E-mail (будет являться логином)</b>{0}:".FormatWith(this.Page.Required());
            }

            return userEmailReadOnly && sameDataAsFbsChecked
                       ? "<b>Логин (E-mail, указанный при регистрации)</b>:"
                       : "<b>E-mail (будет являться логином)</b>{0}:".FormatWith(this.Page.Required());
        }

        protected void lvOnItemDataBound(object sender, EventArgs e)
        {
            this.ManageUI(e);
        }

        /// <summary>
        /// Добавляем в словарь пару имя пользователя и пароль
        /// </summary>
        private void AddUsers()
        {
            foreach (var user in this.usersList)
            {
                if (!this.users.ContainsKey(user.login))
                {
                    this.users.Add(user.login, user.password);
                }
                else
                {
                    this.users.Remove(user.login);
                    this.users.Add(user.login, user.password);
                }
            }
        }

        private OrgUser CreateUserFromRegistrationData(
            InformationSystemsRegistrationView informationSystemsRegistrationView)
        {
            var password = Utility.GeneratePassword();
            var user = new OrgUser { password = password };
            this.FillUserWithFormData(ref user, informationSystemsRegistrationView);
            return user;
        }

        private OrgUser CreateUserFromRegistrationData(
            UserRegistrationType userType, 
            InformationSystemsRegistrationView systemFbs, 
            InformationSystemsRegistrationView systemFbd)
        {
            string password = Utility.GeneratePassword();
            var user = new OrgUser { password = password };

            switch (userType)
            {
                case UserRegistrationType.FbsUser:
                    this.FillUserWithFormData(ref user, userType, systemFbs, systemFbd);
                    break;
                case UserRegistrationType.FbdUser:
                    bool theSameInfoAsFbs = systemFbs.AccessToSystem
                                            && systemFbd.SameDataAsFbs
                                            && // проверка, когда подается заявление на активацию и одновременно на ФБС 
                                            // в этом случае выполняется подача заявки на разных пользователей и данные не те же
                                            !this.IsFbdAccountForActivation();
                    this.FillUserWithFormData(
                        ref user, theSameInfoAsFbs ? UserRegistrationType.FbsUser : userType, systemFbs, systemFbd);
                    break;
                default:
                    throw new Exception("Unsupported user type.");
            }

            return user;
        }

        private void FillOrgUserWithOrgData(OrgUser commonUser, OrgUser user)
        {
            user.RequestedOrganization.KPP = commonUser.RequestedOrganization.KPP;
            user.RequestedOrganization.INN = commonUser.RequestedOrganization.INN;
            user.RequestedOrganization.OGRN = commonUser.RequestedOrganization.OGRN;
            user.RequestedOrganization.FactAddress = commonUser.RequestedOrganization.FactAddress;
            user.RequestedOrganization.LawAddress = commonUser.RequestedOrganization.LawAddress;
            user.RequestedOrganization.TownName = commonUser.RequestedOrganization.TownName;
            user.RequestedOrganization.DirectorFullName = commonUser.RequestedOrganization.DirectorFullName;
            user.RequestedOrganization.OwnerDepartment = commonUser.RequestedOrganization.OwnerDepartment;
            user.RequestedOrganization.FullName = commonUser.RequestedOrganization.FullName;
            user.RequestedOrganization.ShortName = commonUser.RequestedOrganization.ShortName;
            user.RequestedOrganization.Phone = commonUser.RequestedOrganization.Phone;
            user.RequestedOrganization.Fax = commonUser.RequestedOrganization.Fax;
            user.RequestedOrganization.RCModelId = commonUser.RequestedOrganization.RCModelId;
            user.RequestedOrganization.RCModelName = commonUser.RequestedOrganization.RCModelName;
            user.RequestedOrganization.RCDescription = commonUser.RequestedOrganization.RCDescription;
            user.RequestedOrganization.Region = commonUser.RequestedOrganization.Region;
            user.RequestedOrganization.OrgType =
                new CatalogElement(Convert.ToInt32(commonUser.RequestedOrganization.OrgType.Id))
                    {
                       Name = commonUser.RequestedOrganization.OrgType.Name 
                    };
            user.RequestedOrganization.ReceptionOnResultsCNE = commonUser.RequestedOrganization.ReceptionOnResultsCNE;

            // А если создается новая организация, форма "большая", берем еще данные
            if (this.UserCreatesNewOrg())
            {
                user.RequestedOrganization.ShortName = commonUser.RequestedOrganization.ShortName;
                user.RequestedOrganization.DirectorPosition = commonUser.RequestedOrganization.DirectorPosition;
                user.RequestedOrganization.AccreditationSertificate =
                    commonUser.RequestedOrganization.AccreditationSertificate;
                user.RequestedOrganization.PhoneCityCode = commonUser.RequestedOrganization.PhoneCityCode;
                user.RequestedOrganization.EMail = commonUser.RequestedOrganization.EMail;
                user.RequestedOrganization.Site = commonUser.RequestedOrganization.Site;
                user.RequestedOrganization.IsFilial = commonUser.RequestedOrganization.IsFilial;
                user.RequestedOrganization.IsPrivate = commonUser.RequestedOrganization.IsPrivate;
                user.RequestedOrganization.Kind =
                    new CatalogElement(Convert.ToInt32(commonUser.RequestedOrganization.Kind.Id));
            }
            else if (this.FoundedOrg != null)
            {
                // Нашли организацию
                user.RequestedOrganization.OrganizationId = commonUser.RequestedOrganization.OrganizationId;
                user.RequestedOrganization.IsPrivate = commonUser.RequestedOrganization.IsPrivate;
                user.RequestedOrganization.OrgType =
                    new CatalogElement(Convert.ToInt32(user.RequestedOrganization.OrgType.Id));
            }
        }

        private void FillUserWithFormData(
            ref OrgUser user, InformationSystemsRegistrationView informationSystemsRegistrationView)
        {
            user.lastName = informationSystemsRegistrationView.FullName.TrimEnd();
            user.email = informationSystemsRegistrationView.Email.Trim();
            user.position = informationSystemsRegistrationView.Position != null
                                ? informationSystemsRegistrationView.Position.TrimEnd()
                                : string.Empty;
            user.phone = informationSystemsRegistrationView.Phone != null
                             ? informationSystemsRegistrationView.Phone.TrimEnd()
                             : string.Empty;
        }

        private void FillUserWithFormData(
            ref OrgUser user, 
            UserRegistrationType userType, 
            InformationSystemsRegistrationView systemFbs, 
            InformationSystemsRegistrationView systemFbd)
        {
            string lastName, email, position, phone;
            switch (userType)
            {
                case UserRegistrationType.FbsUser:
                    lastName = systemFbs.FullName;
                    email = systemFbs.Email;
                    position = systemFbs.Position;
                    phone = systemFbs.Phone;
                    break;
                case UserRegistrationType.FbdUser:
                    lastName = systemFbd.FullName;
                    email = systemFbd.Email;
                    position = systemFbd.Position;
                    phone = systemFbd.Phone;
                    break;
                default:
                    throw new Exception("Unsupported user type.");
            }

            // ReSharper disable PossibleNullReferenceException
            user.lastName = lastName != null ? lastName.TrimEnd() : string.Empty;
            user.email = email != null ? email.TrimEnd() : string.Empty;
            user.position = position != null ? position.TrimEnd() : string.Empty;
            user.phone = phone != null ? phone.TrimEnd() : string.Empty;

            // всегда происходит добавление нового пользователя кроме ситуации, когда
            // УС ОУ имеет статус "только чтение". Можно подать заявку на статус "activated".
            if (userType == UserRegistrationType.FbdUser && this.IsFbdAccountForActivation()
                && this._fbdAuthorizedStaff.Email.ToLower().CompareTo(user.email) == 0)
            {
                user.phone = this._fbdAuthorizedStaff.Phone;
                user.position = this._fbdAuthorizedStaff.Position;
                user.login = user.email;
            }
            else
            {
                user.login = string.Empty;
            }
        }

        private void FindControlForInformationSystemBlock(Control control)
        {
            var requiredField = (RequiredFieldValidator)control.FindControl("rfvUserEmail");
            if (requiredField != null)
            {
                var req = new DictionaryReqField
                    {
                        RequiredField = requiredField, 
                        ControlToValidate = (TextBox)control.FindControl(requiredField.ControlToValidate)
                    };

                var hiddenId = string.Format("{0}_hidden", req.ControlToValidate.ID);
                req.HiddenField = (HiddenField)control.FindControl(hiddenId);
                req.UserPosition = (TextBox)control.FindControl("tbUserPosition");
                req.UserPhone = (TextBox)control.FindControl("tbUserPhone");

                this.dictionary.Add(req);

                requiredField = (RequiredFieldValidator)control.FindControl("rfvUserFullName");
                req = new DictionaryReqField
                    {
                        RequiredField = requiredField, 
                        ControlToValidate = (TextBox)control.FindControl(requiredField.ControlToValidate)
                    };

                hiddenId = string.Format("{0}_hidden", req.ControlToValidate.ID);
                req.HiddenField = (HiddenField)control.FindControl(hiddenId);
                req.UserPosition = (TextBox)control.FindControl("tbUserPosition");
                req.UserPhone = (TextBox)control.FindControl("tbUserPhone");
                this.dictionary.Add(req);

                var regularExpression = (RegularExpressionValidator)control.FindControl("revUserEmail");
                req = new DictionaryReqField
                    {
                        RegularExpression = regularExpression, 
                        ControlToValidate = (TextBox)control.FindControl(regularExpression.ControlToValidate)
                    };

                hiddenId = string.Format("{0}_hidden", req.ControlToValidate.ID);
                req.HiddenField = (HiddenField)control.FindControl(hiddenId);
                req.UserPosition = (TextBox)control.FindControl("tbUserPosition");
                req.UserPhone = (TextBox)control.FindControl("tbUserPhone");
                this.dictionary.Add(req);
                return;
            }

            foreach (Control childControl in control.Controls)
            {
                this.FindControlForInformationSystemBlock(childControl);
            }
        }

        private bool IsFbdAccountForActivation()
        {
            return this._fbdAuthorizedStaff != null
                   && this._fbdAuthorizedStaff.Status == UserAccount.UserAccountStatusEnum.Readonly;
        }

        private void ManageFbdModule(EventArgs e)
        {
            var dataItem =
                (InformationSystemsRegistrationView)((ListViewDataItem)((ListViewItemEventArgs)e).Item).DataItem;

            if (dataItem.SystemId == 3)
            {
                var item = ((ListViewItemEventArgs)e).Item;

                var checkBoxSameAsFbs = (CheckBox)item.FindControl("CBSameDataAsFbs");
                var checkBoxAllowSystem = (CheckBox)item.FindControl("cbAllowSystem");

                this.AllowFbd = checkBoxAllowSystem.ClientID;
                this.SameFbs = checkBoxSameAsFbs.ClientID;
            }
        }

        private void ManageFbsModule(EventArgs e)
        {
            var dataItem =
                (InformationSystemsRegistrationView)((ListViewDataItem)((ListViewItemEventArgs)e).Item).DataItem;

            var item = ((ListViewItemEventArgs)e).Item;
            var checkBox = (CheckBox)item.FindControl("cbAllowSystem");

            if (dataItem.SystemId == 2)
            {
                this.AllowFbs = checkBox.ClientID;
            }

            if (dataItem.SystemId != 3)
            {
                var labelFbdInfo = (Label)item.FindControl("lFbdInfo");
                labelFbdInfo.Text = @"<b>Регистрация</b> уполномоченного сотрудника по работе с <b>"
                                    + dataItem.ShortName + "</b>:";
            }
        }

        /// <summary>
        /// Логика для отображания блоков АИС ФБС и ФБД
        /// </summary>
        /// <param name="e">
        /// Параметры события
        /// </param>
        private void ManageUI(EventArgs e)
        {
            this.ManageFbdModule(e);
            this.ManageFbsModule(e);
            this.SetLabelNames(e);
        }

        private void ProcessSuccess(OrgUser user)
        {
            // авторизую вновь созданного пользователя
            //FormsAuthentication.SetAuthCookie(user.login, false);

            // зашифрую и сохраню пароль в сессию
            Utility.SaveUsersAndPasswordsToSession(this.users);

            string toEncrypt = String.Format("{0};{1};{2}", user.login, DateTime.Now.ToString(), user.passwordHash);
            RSACryptoServiceProvider Rsa = Helper.GetCurrentRsaProvider(true);

            toEncrypt = Uri.EscapeDataString(Convert.ToBase64String(Rsa.Encrypt(Encoding.Default.GetBytes(toEncrypt), false)));
            // перейду на страницу успеха
            this.Response.Redirect(String.Format("WebUserAccount/ResetPassword.aspx?userId={0}&isNew=1", toEncrypt), true);
        }

        private void SendMailOnSuccess(List<OrgUser> users)
        {
            foreach (var user in users)
            {
                if (!this.RegisterSendMailUserList.Contains(user.login) && user!=users.Last())
                {
                    // подготовлю email сообщение пользователю
                    var template = new EmailTemplate(EmailTemplateTypeEnum.RegistrationSetPassword);
                    EmailMessage message = template.ToEmailMessage();
                    message.To = user.email;

                    string toEncrypt = String.Format("{0};{1};{2}", user.login, DateTime.Now.ToString(), user.passwordHash);
                    RSACryptoServiceProvider Rsa = Helper.GetCurrentRsaProvider(true);

                    toEncrypt = Uri.EscapeDataString(Convert.ToBase64String(Rsa.Encrypt(Encoding.Default.GetBytes(toEncrypt), false)));
                    message.Params = Utility.CollectEmailMetaVariables(user, user.password, Utility.GetSeverPath(this.Request), String.Format("{0}/WebUserAccount/ResetPassword.aspx?userId={1}&isNew=1", Utility.GetSeverPath(this.Request), toEncrypt));
                    

                    // отправлю email сообщение				
                    TaskManager.SendEmail(message);
                    this.RegisterSendMailUserList.Add(user.login);
                }

                if (user.RequestedOrganization.OrganizationId == null)
                {
                    if (!string.IsNullOrEmpty(Config.AdminEMail))
                    {
                        // подготовлю email сообщение администратору, если организация не связана
                        var template_admin = new EmailTemplate(EmailTemplateTypeEnum.Registration_NewOrg);
                        EmailMessage message_admin = template_admin.ToEmailMessage();
                        message_admin.To = Config.AdminEMail;
                        message_admin.Params = Utility.CollectEmailMetaVariables_Admin(
                            user, Utility.GetSeverPath(this.Request));

                        // отправлю email сообщение
                        TaskManager.SendEmail(message_admin);
                    }
                }
            }
        }

        /// <summary>
        /// Заполнение сообщений об ошибках для обязательных полей
        /// </summary>
        /// <param name="e">Параметры события</param>
        private void SetLabelNames(EventArgs e)
        {
            var dataItem =
                (InformationSystemsRegistrationView)((ListViewDataItem)((ListViewItemEventArgs)e).Item).DataItem;

            var rfvUserFullName = (RequiredFieldValidator)((ListViewItemEventArgs)e).Item.FindControl("rfvUserFullName");
            rfvUserFullName.ErrorMessage = string.Format(rfvUserFullName.ErrorMessage, dataItem.ShortName);

            var rfvUserEmail = (RequiredFieldValidator)((ListViewItemEventArgs)e).Item.FindControl("rfvUserEmail");
            rfvUserEmail.ErrorMessage = string.Format(rfvUserEmail.ErrorMessage, dataItem.ShortName);

            var revUserEmail = (RegularExpressionValidator)((ListViewItemEventArgs)e).Item.FindControl("revUserEmail");
            revUserEmail.ErrorMessage = string.Format(revUserEmail.ErrorMessage, dataItem.ShortName);
        }

        private bool UserCreatesNewOrg()
        {
            return this.orgID <= 0;
        }

        /// <summary>
        /// Отмечена ли хоть одна ИС
        /// </summary>
        /// <returns>true - да, false - нет</returns>
        private bool HasSelectSystem()
        {
            var hasSelectSystem = false;

            foreach (var system in this.UpdateInformationSystemsRegistrashionList)
            {
                if (system.AccessToSystem)
                {
                    hasSelectSystem = true;
                }
            }

            return hasSelectSystem;
        }

        #endregion

        /// <summary>
        /// Класс для контроля обязательных полей и регулярных выражений для текстовых полей
        /// </summary>
        public class DictionaryReqField
        {
            #region Public Properties

            /// <summary>
            /// Текстовое поле
            /// </summary>
            public TextBox ControlToValidate { get; set; }

            /// <summary>
            /// Скрытое поле, по Value которого можно определить нужно ли отключить валидацию
            /// </summary>
            public HiddenField HiddenField { get; set; }

            /// <summary>
            /// Контрол RegularExpressionValidator для текстового поля
            /// </summary>
            public RegularExpressionValidator RegularExpression { get; set; }

            /// <summary>
            /// Контрол RequiredFieldValidator для текстового поля
            /// </summary>
            public RequiredFieldValidator RequiredField { get; set; }

            /// <summary>
            /// Текстовое поле "Должность"
            /// </summary>
            public TextBox UserPosition { get; set; }

            /// <summary>
            /// Текстовое поле "Телефон"
            /// </summary>
            public TextBox UserPhone { get; set; }

            #endregion
        }
    }
}