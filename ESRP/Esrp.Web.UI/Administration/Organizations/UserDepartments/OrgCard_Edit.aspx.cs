namespace Esrp.Web.Administration.Organizations.UserDepartments
{
    using System;
    using System.Data;
    using System.Drawing;
    using System.Web;
    using System.Web.UI;

    using Esrp.Core.CatalogElements;
    using Esrp.Core.Organizations;
    using Esrp.Core.Systems;
    using Esrp.Core.Users;

    /// <summary>
    /// The org card.
    /// </summary>
    public partial class OrgCard : Page
    {
        #region Properties

        private int? NewOrgId
        {
            get
            {
                return string.IsNullOrEmpty(this.hfNewOrgId.Value) ? (int?)null : Convert.ToInt32(this.hfNewOrgId.Value);
            }

            set
            {
                Organization newOrg = null;
                if (value != null)
                {
                    this.hfNewOrgId.Value = value.ToString();
                    newOrg = OrganizationDataAccessor.Get(value.Value);
                }
                else
                {
                    this.hfNewOrgId.Value = string.Empty;
                }

                if (newOrg == null)
                {
                    this.lblNewOrgName.ForeColor = Color.FromName("#999");
                    this.lblNewOrgName.Text = "Нет новой организации";
                }
                else if (newOrg.Id == this.GetParamInt("OrgID"))
                {
                    this.lblNewOrgName.Text = "Организация не может быть реорганизована сама в себя";
                    this.lblMainOrgName.ForeColor = Color.Red;
                }
                else
                {
                    this.lblNewOrgName.Text = string.Format("Новая организация: {0}", newOrg.FullName);
                }
            }
        }

        private string commonError
        {
            set
            {
                this.phCommonError.Visible = true;
                this.lblCommonError.Text = value;
            }
        }

        private int? mainId
        {
            get
            {
                return string.IsNullOrEmpty(this.hfMainOrgId.Value)
                           ? (int?)null
                           : Convert.ToInt32(this.hfMainOrgId.Value);
            }

            set
            {
                int? _mainId = value;
                this.hfMainOrgId.Value = _mainId.HasValue ? _mainId.Value.ToString() : string.Empty;

                Organization org = !_mainId.HasValue ? null : OrganizationDataAccessor.Get(_mainId.Value);

                if (org == null)
                {
                    this.cbFil.Checked = false;
                    this.btnFilial.Enabled = false;
                    this.lblMainOrgName.Enabled = false;
                    this.lblMainOrgName.ForeColor = Color.FromName("#999");
                    this.lblMainOrgName.Text = "Нет головной организации";
                }
                else if (org.MainId.HasValue)
                {
                    this.cbFil.Checked = true;
                    this.lblMainOrgName.Text = "Выбранная организация не может быть головной, т.к. является филиалом";
                    this.lblMainOrgName.ForeColor = Color.Red;
                    _mainId = null;
                }
                else if (org.Id == this.GetParamInt("OrgID"))
                {
                    this.cbFil.Checked = true;
                    this.lblMainOrgName.Text = "Организация не может быть филиалом сама для себя";
                    this.lblMainOrgName.ForeColor = Color.Red;
                    _mainId = null;
                }
                else
                {
                    this.cbFil.Checked = true;
                    this.btnFilial.Enabled = true;
                    this.lblMainOrgName.Enabled = true;
                    this.lblMainOrgName.Text = "Головная организация: " + org.FullName;
                }
            }
        }

        private string rcDescriptionText
        {
            get
            {
                int model = int.Parse(this.rblRecruitmentCampaigns.SelectedValue);
                return model == 999 ? this.txtBxDescription.Text : string.Empty;
            }

            set
            {
                this.txtBxDescription.Text = value;
            }
        }

        private int? rcModel
        {
            get
            {
                return int.Parse(this.rblRecruitmentCampaigns.SelectedValue);
            }

            set
            {
                this.rblRecruitmentCampaigns.SelectedValue = value.ToString();
                if (value != 999)
                {
                    this.txtBxDescription.Text = string.Empty;
                    this.txtBxDescription.Enabled = false;
                }
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get user status.
        /// </summary>
        /// <param name="sysStatus">
        /// The sys status.
        /// </param>
        /// <returns>
        /// The get user status.
        /// </returns>
        public static string GetUserStatus(string sysStatus)
        {
            return UserAccountExtentions.GetNewStatusName(sysStatus);
        }

        /// <summary>
        /// Получает значение параметра из строки запроса по его имени и преобразует в тип int
        /// </summary>
        /// <param name="name">
        /// Имя параметра
        /// </param>
        /// <returns>
        /// Значение параметра
        /// </returns>
        public int GetParamInt(string name)
        {
            if (this.Page.Request.QueryString[name] != null)
            {
                int returnVal;
                if (int.TryParse(this.Page.Request.QueryString[name], out returnVal))
                {
                    return returnVal;
                }
            }

            return 0;
        }

        /// <summary>
        /// Получает значение параметра из строки запроса по его имени и преобразует в тип string
        /// </summary>
        /// <param name="name">
        /// Имя параметра
        /// </param>
        /// <returns>
        /// Значение параметра
        /// </returns>
        public string GetParamStr(string name)
        {
            if (this.Page.Request.QueryString[name] != null)
            {
                return this.Page.Request.QueryString[name];
            }

            return string.Empty;
        }

        /// <summary>
        /// The validate.
        /// </summary>
        public override void Validate()
        {
            string status = this.HdnOrgStatus.Value;
            if (status != this.DDLOrgStatus.SelectedValue && status != string.Empty)
            {
                this.RequiredDateValidator.Enabled = true;
                this.RequiredReasonValidator.Enabled = true;
            }
            else
            {
                this.RequiredDateValidator.Enabled = false;
                this.RequiredReasonValidator.Enabled = false;
            }

            base.Validate();

            // Поле "Контрольные цифры приема граждан, обучающихся за счет средств федерального бюджета"
            int federalBudget = 0;
            if (!int.TryParse(this.txtBxFederalBudget.Text, out federalBudget))
            {
                this.lblFederalBudgetError.Text = "Должно быть введено целое положительное число, либо 0<br />";
                this.vlStruct.IsValid = false;
            }

            // Поле "Квоты по целевому приему"
            int targeted = 0;
            if (!int.TryParse(this.txtBxTargeted.Text, out targeted))
            {
                this.lblTargetedError.Text = "Должно быть введено целое положительное число, либо 0<br />";
                this.vlStruct.IsValid = false;
            }

            // Поле "Объем и структура приема обучающихся за счет средств бюджета субъектов Российской Федерации"
            int localBudget = 0;
            if (!int.TryParse(this.txtBxLocalBudget.Text, out localBudget))
            {
                this.lblLocalBudgetError.Text = "Должно быть введено целое положительное число, либо 0<br />";
                this.vlStruct.IsValid = false;
            }

            // Поле "Количество мест для обучения на основе договоров с оплатой стоимости обучения"
            int paying = 0;
            if (!int.TryParse(this.txtBxPaying.Text, out paying))
            {
                this.lblPayingError.Text = "Должно быть введено целое положительное число, либо 0<br />";
                this.vlStruct.IsValid = false;
            }

            // Поле "Очная"
            int fullTime = 0;
            if (!int.TryParse(this.txtBxFullTime.Text, out fullTime))
            {
                this.lblFullTimeError.Text = "Должно быть введено целое положительное число, либо 0<br />";
                this.vlStruct.IsValid = false;
            }

            // Поле "Очно-заочная"
            int evening = 0;
            if (!int.TryParse(this.txtBxEvening.Text, out evening))
            {
                this.lblEveningError.Text = "Должно быть введено целое положительное число, либо 0<br />";
                this.vlStruct.IsValid = false;
            }

            // Поле "Заочная"
            int postal = 0;
            if (!int.TryParse(this.txtBxPostal.Text, out postal))
            {
                this.lblPostalError.Text = "Должно быть введено целое положительное число, либо 0<br />";
                this.vlStruct.IsValid = false;
            }

            // Если ошибок конвертации не найдено, то начинаем алгоритмическую проверку
            if (!this.vlStruct.IsValid)
            {
                return;
            }

            // 1. Первая замороченная алгоритмическая проверка
            if (federalBudget == 0 && localBudget == 0)
            {
                this.lblFederalBudgetError.Text = "[1] ";
                this.lblLocalBudgetError.Text = "[2] ";
                this.commonError = "Одно из полей [1] или [2] должно быть не нулевым";
                this.vlStruct.IsValid = false;
                return;
            }

            if (federalBudget != 0 && localBudget != 0)
            {
                this.lblFederalBudgetError.Text = "[1] ";
                this.lblLocalBudgetError.Text = "[2] ";
                this.commonError = "Одно из полей [1] или [2] должно нулевым";
                this.vlStruct.IsValid = false;
                return;
            }

            // 2. Вторая замороченная алгоритмическая проверка
            if (federalBudget + localBudget + paying < fullTime + evening + postal)
            {
                this.lblFederalBudgetError.Text = "[1] ";
                this.lblLocalBudgetError.Text = "[2] ";
                this.lblPayingError.Text = "[3] ";
                this.lblFullTimeError.Text = "[4] ";
                this.lblEveningError.Text = "[5] ";
                this.lblPostalError.Text = "[6] ";
                this.commonError = "Сумма полей [1], [2], [3] не должна быть меньше суммы полей [4], [5], [6]";
                this.vlStruct.IsValid = false;
                return;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Обработчик нажатия кнопки сохранить
        /// </summary>
        /// <param name="sender">
        /// Источник события
        /// </param>
        /// <param name="e">
        /// Параметры события
        /// </param>
        protected void BtnUpdateClick(object sender, EventArgs e)
        {
            if (this.Page.IsValid)
            {
                int orgID = this.GetParamInt("OrgID");

                try
                {
                    var org = new Organization(
                        this.TBFullName.Text, 
                        int.Parse(this.DDLRegions.SelectedValue), 
                        int.Parse(this.DDLOrgTypes.SelectedValue), 
                        int.Parse(
                            this.hiddenKindsFounder.Value == "1"
                                ? this.DDLOrgKinds.SelectedValue
                                : this.DDLOrgKindsFounder.SelectedValue), 
                        this.DDLOPF.SelectedValue == "1", 
                        this.DDLOrgTypes.SelectedValue != "6" ? this.TBINN.Text : null, 
                        this.DDLOrgTypes.SelectedValue != "6" ? this.TBOGRN.Text : null, 
                        int.Parse(this.DDLOrgStatus.SelectedValue), 
                        this.TbDateChangeStatus.Text == string.Empty
                            ? (DateTime?)null
                            : Convert.ToDateTime(this.TbDateChangeStatus.Text), 
                        this.TBReason.Text,
                        int.Parse(this.DDLIS.SelectedValue),
                        "");
                    org.Id = orgID;
                    org.ShortName = this.TBShortName.Text;
                    org.AnotherName = "";
                    org.IsFilial = this.DDLOrgTypes.SelectedValue != "6" && this.mainId.HasValue;
                    org.MainId = (this.DDLOrgTypes.SelectedValue == "6") ? null : this.mainId;

                    org.NewOrgId = (this.DDLOrgStatus.SelectedValue == ((int)OrganizationStatus.Reorganized).ToString())
                                       ? this.NewOrgId
                                       : null;

                    org.OwnerDepartment = string.Empty;
                    org.DepartmentId = int.Parse(this.DDLOwnerDepartmentId.SelectedValue);

                    org.DirectorFullName = this.TBDirectorName.Text;
                    org.DirectorPosition = this.TBDirectorPosition.Text;

                    org.AccreditationSertificate = this.TBAccredCert.Text;

                    org.FactAddress = this.TBFactAddress.Text;
                    org.LawAddress = this.TBJurAddress.Text;

                    org.PhoneCityCode = this.TBCityCode.Text;
                    org.Phone = this.TBPhone.Text;
                    org.Fax = this.TBFax.Text;
                    org.EMail = this.TBEMail.Text;
                    org.Site = this.TBWebPage.Text;

                    // Модель приемной кампании
                    org.RCModelId = this.rcModel;
                    org.RCDescription = this.rcDescriptionText;

                    // Сведения о защищенном подключении
                    org.ConnectionScheme = new CatalogElement(int.Parse(this.ddlConnectionScheme.SelectedValue));
                    org.ConnectionStatus = new CatalogElement(int.Parse(this.ddlConnectionStatus.SelectedValue));
                    org.TimeConnectionToSecureNetwork = string.IsNullOrEmpty(this.tbTimeConnectionToSecureNetwork.Text)
                                                            ? (DateTime?)null
                                                            : Convert.ToDateTime(
                                                                this.tbTimeConnectionToSecureNetwork.Text);

                    org.TimeEnterInformationInFIS = string.IsNullOrEmpty(this.tbTimeEnterInformationInFIS.Text)
                                                            ? (DateTime?)null
                                                            : Convert.ToDateTime(
                                                                this.tbTimeEnterInformationInFIS.Text);
                    if (this.fuLetterToReschedule.FileBytes.Length > 0)
                    {
                        org.LetterToReschedule = this.fuLetterToReschedule.FileBytes;
                        org.LetterToRescheduleName = this.fuLetterToReschedule.FileName;
                        org.LetterToRescheduleContentType = this.fuLetterToReschedule.PostedFile.ContentType;
                    }
                    else
                    {
                        if (this.hfLetterToReschedule.Value != string.Empty && orgID > 0)
                        {
                            var orgEtalon = OrganizationDataAccessor.Get(orgID);
                            org.LetterToReschedule = orgEtalon.LetterToReschedule;
                            org.LetterToRescheduleName = orgEtalon.LetterToRescheduleName;
                            org.LetterToRescheduleContentType = orgEtalon.LetterToRescheduleContentType;
                        }
                    }

                    // Сведения об объеме и структуре приема
                    org.CNFederalBudget = this.DDLOrgTypes.SelectedValue != "6"
                                              ? int.Parse(this.txtBxFederalBudget.Text)
                                              : 0;
                    org.CNTargeted = int.Parse(this.txtBxTargeted.Text);
                    org.CNLocalBudget = int.Parse(this.txtBxLocalBudget.Text);
                    org.CNPaying = int.Parse(this.txtBxPaying.Text);
                    org.CNFullTime = int.Parse(this.txtBxFullTime.Text);
                    org.CNEvening = int.Parse(this.txtBxEvening.Text);
                    org.CNPostal = int.Parse(this.txtBxPostal.Text);

                    if (this.DDLOrgTypes.SelectedValue == "1" || this.DDLOrgTypes.SelectedValue == "2")
                    {
                        org.ReceptionOnResultsCNE = this.rblReceptionOnResultsCNE.SelectedValue == string.Empty ? (int?)null : Convert.ToInt32(this.rblReceptionOnResultsCNE.SelectedValue);
                    }
                    else
                    {
                        org.ReceptionOnResultsCNE = null;
                    }

                    org.KPP = this.tbKPP.Text;

                    OrganizationDataAccessor.UpdateOrCreate(org, this.User.Identity.Name);

                    this.Response.Redirect(
                        string.Format("OrgCardInfo.aspx?IsNew={1}&OrgID={0}", org.Id, orgID != org.Id), true);

                    this.lblError.Text = string.Empty;
                    this.lblError.Visible = false;
                }
                catch (Exception exp)
                {
                    if (orgID > 0)
                    {
                        this.lblError.Text = "Во время обновления организации произошла ошибка: " + exp.Message;
                    }
                    else
                    {
                        this.lblError.Text = "Во время создания организации произошла ошибка: " + exp.Message;
                    }

                    this.lblError.Visible = true;
                }
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
            int orgID = this.GetParamInt("OrgID");
            int filID = this.GetParamInt("FilID");
            string act = this.GetParamStr("act");
            int newOrgId = this.GetParamInt("NewOrgID");

            if (this.User.IsInRole("EditSelfOrganization"))
            {
                Organization org = OrganizationDataAccessor.GetByLogin(this.User.Identity.Name);
                if (org != null)
                {
                    if (orgID != org.Id)
                    {
                        this.Response.Redirect("OrgCard_Edit.aspx?OrgID=" + org.Id.ToString());
                    }
                }
                else
                {
                    if (
                        !GeneralSystemManager.HasAccessToGroup(
                            this.User.Identity.Name, EsrpManager.AdministratorGroupCode))
                    {
                        this.Response.Redirect("/Profile/View.aspx");
                    }
                }
            }

            if (!this.Page.IsPostBack)
            {
                // Редактирование организации
                if (orgID > 0)
                {
                    Organization org = OrganizationDataAccessor.Get(orgID);

                    // Заполняем список с радиобаттоном
                    if (org.ReceptionOnResultsCNE != null)
                    {
                        this.rblReceptionOnResultsCNE.SelectedValue = org.ReceptionOnResultsCNE.ToString();
                    }

                    // Заполняем простые текстовые поля
                    this.TBFullName.Text = org.FullName;
                    this.TBShortName.Text = org.ShortName;

                    this.TBINN.Text = org.INN;
                    if (this.TBINN.Text == string.Empty)
                    {
                        this.VReqINN.Enabled = false;
                    }

                    this.TBOGRN.Text = org.OGRN;
                    if (this.TBOGRN.Text == string.Empty)
                    {
                        this.VReqOGRN.Enabled = false;
                    }

                    this.tbKPP.Text = org.KPP;

                    if (org.DepartmentId == null)
                    {
                        this.DDLOwnerDepartmentId.SelectedValue = "-1";
                    }
                    else
                    {
                        this.DDLOwnerDepartmentId.SelectedValue = org.DepartmentId.Value.ToString();
                    }

                    this.TBDirectorName.Text = org.DirectorFullName;
                    this.TBDirectorPosition.Text = org.DirectorPosition;

                    this.TBAccredCert.Text = org.AccreditationSertificate;

                    this.TBFactAddress.Text = org.FactAddress;
                    this.TBJurAddress.Text = org.LawAddress;

                    this.TBCityCode.Text = org.PhoneCityCode;
                    this.TBPhone.Text = org.Phone;
                    this.TBFax.Text = org.Fax;
                    this.TBEMail.Text = org.EMail;
                    this.TBWebPage.Text = org.Site;

                    // Модель приемной кампании
                    this.rcModel = org.RCModelId;
                    this.rcDescriptionText = org.RCDescription;

                    // Сведения о защищенном подключении
                    if (org.LetterToReschedule != null)
                    {
                        this.lLetterToReschedule.Text =
                            string.Format(
                                "{0} <a target=_blank href=\"/Administration/Organizations/LetterToReschedule.aspx?orgId={1}\" title=\"Письмо о переносе сроков\">Скачать</a> {2}",
                                org.LetterToRescheduleName,
                                org.Id,
                                "<a id=\"deleteLetter\" onclick=\"return false;\" href=\"#\" title=\"Удалить письмо о переносе сроков\">Удалить</a>");
                        this.hfLetterToReschedule.Value = "existLetter";
                    }

                    this.ddlConnectionScheme.SelectedValue = org.ConnectionScheme.Id.ToString();
                    this.ddlConnectionStatus.SelectedValue = org.ConnectionStatus.Id.ToString();

                    this.tbTimeConnectionToSecureNetwork.Text = org.TimeConnectionToSecureNetwork != null
                                                                    ? Convert.ToDateTime(org.TimeConnectionToSecureNetwork).ToString()
                                                                    : string.Empty;

                    this.tbTimeEnterInformationInFIS.Text = org.TimeEnterInformationInFIS != null
                                                ? Convert.ToDateTime(org.TimeEnterInformationInFIS).ToString()
                                                : string.Empty;

                    // Сведения об объеме и структуре приема
                    this.txtBxFederalBudget.Text = org.CNFederalBudget.ToString();
                    this.txtBxTargeted.Text = org.CNTargeted.ToString();
                    this.txtBxLocalBudget.Text = org.CNLocalBudget.ToString();
                    this.txtBxPaying.Text = org.CNPaying.ToString();
                    this.txtBxFullTime.Text = org.CNFullTime.ToString();
                    this.txtBxEvening.Text = org.CNEvening.ToString();
                    this.txtBxPostal.Text = org.CNPostal.ToString();

                    // Заполняем списки и чекбокс
                    this.DDLRegions.SelectedValue = org.Region.Id.ToString();
                    this.DDLOrgTypes.SelectedValue = org.OrgType.Id.ToString();
                    if (org.OrgType.Id != 6)
                    {
                        this.DDLOrgKinds.SelectedValue = org.Kind.Id.ToString();
                    }
                    else
                    {
                        this.DDLOrgKindsFounder.SelectedValue = org.Kind.Id.ToString();
                    }

                    this.DDLOPF.SelectedValue = org.IsPrivate ? "1" : "0";
                    this.mainId = filID == 0 ? org.MainId : filID;

                    if (newOrgId > 0 || org.Status.Id == (int)OrganizationStatus.Reorganized)
                    {
                        this.HdnOrgStatus.Value =
                            this.DDLOrgStatus.SelectedValue = ((int)OrganizationStatus.Reorganized).ToString();
                    }
                    else
                    {
                        this.HdnOrgStatus.Value = this.DDLOrgStatus.SelectedValue = org.Status.Id.ToString();
                    }

                    this.NewOrgId = newOrgId == 0 ? org.NewOrgId : newOrgId;

                    DataTable table = OrgUserDataAccessor.GetByOrgAsTable(orgID);
                    if (table.Rows.Count == 0)
                    {
                        this.rptUsers.Visible = false;
                        this.lblNoUsers.Visible = table.Rows.Count == 0;
                    }
                    else
                    {
                        this.rptUsers.DataSource = table;
                        this.rptUsers.DataBind();
                    }
                }
                else
                {
                    // Создание организации
                    this.rptUsers.Visible = false;
                    this.lblNoUsers.Visible = true;
                    this.btnUpdate.Text = "Создать";

                    // Модель приемной кампании
                    this.rcModel = 1;
                    this.rcDescriptionText = string.Empty;

                    // Сведения об объеме и структуре приема
                    this.txtBxFederalBudget.Text = "0";
                    this.txtBxTargeted.Text = "0";
                    this.txtBxLocalBudget.Text = "0";
                    this.txtBxPaying.Text = "0";
                    this.txtBxFullTime.Text = "0";
                    this.txtBxEvening.Text = "0";
                    this.txtBxPostal.Text = "0";

                    this.mainId = filID > 0 ? filID : (int?)null;
                    this.NewOrgId = newOrgId;
                }
            }
        }

        /// <summary>
        /// The btn filial_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void btnFilial_Click(object sender, EventArgs e)
        {
            int orgID = this.GetParamInt("OrgID");

            this.Response.Redirect(
                string.Format(
                    "/SelectOrg.aspx?BackUrl={0}&ReturnPrmName=FilID", 
                    HttpUtility.UrlEncode(
                        "./Administration/Organizations/Administrators/OrgCard_Edit.aspx?OrgID=" + orgID)));
        }

        /// <summary>
        /// выбрать организацию, в которую была реорганизована текущая
        /// </summary>
        /// <param name="sender">
        /// кнопка выбора новой организации
        /// </param>
        /// <param name="e">
        /// e
        /// </param>
        protected void btnReorganizedTo_Click(object sender, EventArgs e)
        {
            int orgId = this.GetParamInt("OrgID");

            this.Response.Redirect(
                string.Format(
                    "/SelectOrg.aspx?BackUrl={0}&ReturnPrmName=NewOrgID", 
                    HttpUtility.UrlEncode(
                        "./Administration/Organizations/Administrators/OrgCard_Edit.aspx?OrgID=" + orgId)));
        }

        #endregion
    }
}