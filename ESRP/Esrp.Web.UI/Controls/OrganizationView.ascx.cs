namespace Esrp.Web.Controls
{
    using System;
    using System.Linq;
    using System.Configuration;
    using System.Security.Principal;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    using Esrp.Core.Organizations;
    using Esrp.Core.CatalogElements;

    /// <summary>
    /// Просмотр организации или ее версии
    /// </summary>
    public partial class OrganizationView : UserControl
    {
        #region Public Properties

        /// <summary>
        /// Сообщение для вывода в заголовке
        /// </summary>
        public string Message
        {
            get
            {
                return (string)this.ViewState["Message"];
            }

            set
            {
                this.ViewState["Message"] = value;
            }
        }

        /// <summary>
        /// доступна ли организация для редактирования (по умолчанию да)
        /// </summary>
        public bool CanEdit
        {
            get
            {
                if (this.ViewState["CanEdit"] != null)
                {
                    return (bool)this.ViewState["CanEdit"];
                }

                return true;
            }

            set
            {
                this.ViewState["CanEdit"] = value;
            }
        }

        /// <summary>
        /// доступна ли смена модели приемной комиссии (по умолчанию false)
        /// </summary>
        public bool CanChangeRCModel
        {
            get
            {
                if (this.ViewState["CanChangeRCModel"] != null)
                {
                    return (bool)this.ViewState["CanChangeRCModel"];
                }

                return false;
            }

            set
            {
                this.ViewState["CanChangeRCModel"] = value;
            }
        }

        /// <summary>
        /// id организации
        /// </summary>
        public int? OrganizationId
        {
            get
            {
                object o = this.ViewState["OrganizationId"];
                if (o == null)
                {
                    return null;
                }

                return int.Parse(o.ToString());
            }

            set
            {
                this.ViewState["OrganizationId"] = value;
            }
        }

        /// <summary>
        /// версия организации
        /// </summary>
        public int? Version
        {
            get
            {
                object o = this.ViewState["Version"];
                if (o == null)
                {
                    return null;
                }

                return int.Parse(o.ToString());
            }

            set
            {
                this.ViewState["Version"] = value;
            }
        }

        /// <summary>
        /// Gets User.
        /// </summary>
        public IPrincipal User
        {
            get
            {
                return HttpContext.Current.User;
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
                if (!value.HasValue)
                {
                    value = 999;
                }
                this.rblRecruitmentCampaigns.SelectedValue = value.ToString();
                if (value != 999)
                {
                    this.txtBxDescription.Text = string.Empty;
                    this.txtBxDescription.Enabled = false;
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

        #endregion

        #region Methods

        /// <summary>
        /// установить поля организации
        /// </summary>
        /// <param name="e">Параметры события</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!this.OrganizationId.HasValue)
                return;

            Organization org;
            if ((this.Version ?? 0) > 0)
            {
                org = OrganizationDataAccessor.Get(this.OrganizationId.Value, this.Version.Value);
            }
            else
            {
                org = OrganizationDataAccessor.Get(this.OrganizationId.Value);
            }

            if (!Page.IsPostBack)
            {
                this.ddlOrgKind.DataSource = CatalogDataAcessor.GetAll(CatalogDataAcessor.Catalogs.OrganizationKinds);
                this.ddlOrgKind.DataBind();

                this.ddlIS.DataSource = CatalogDataAcessor.GetAll(CatalogDataAcessor.Catalogs.OrganizationIS);
                this.ddlIS.DataBind(); 

                // Заполняем простые текстовые поля
                this.LFullName.Text = org.FullName;
                this.tbShortName.Text = org.ShortName;
                //this.lblFederalBudget.Text = org.CNFederalBudget.ToString();
                //this.lblTargeted.Text = org.CNTargeted.ToString();
                //this.lblLocalBudget.Text = org.CNLocalBudget.ToString();
                //this.lblPaying.Text = org.CNPaying.ToString();

                // Модель приемной кампании
                this.rcModel = org.RCModelId;
                this.rcDescriptionText = org.RCDescription;

                // Блок "Сведения о защищенном подключении"
                this.lLetterToReschedule.Text = org.LetterToReschedule != null
                    ? string.Format("<a target=_blank href=\"/Administration/Organizations/LetterToReschedule.aspx?orgId={0}&versionNumber={1}\" title=\"Письмо о переносе сроков\">Скачать</a>", org.Id, (this.Version ?? 0) > 0 ? this.Version.Value : (int?)null)
                                                    : "Не загружено";
                this.lConnectionScheme.Text = org.ConnectionScheme.Name;
                this.lConnectionStatus.Text = org.ConnectionStatus.Name;
                this.lTimeConnectionToSecureNetwork.Text = org.TimeConnectionToSecureNetwork != null
                                                     ? Convert.ToDateTime(org.TimeConnectionToSecureNetwork).ToShortDateString()
                                                     : string.Empty;

                this.lTimeEnterInformationInFIS.Text = org.TimeEnterInformationInFIS != null
                                         ? Convert.ToDateTime(org.TimeEnterInformationInFIS).ToShortDateString()
                                         : string.Empty;

                if (org.DateChangeStatus != null)
                {
                    var dateChangeStatus = (DateTime)org.DateChangeStatus;
                    var dateChangeStatusString = dateChangeStatus.ToShortDateString();
                    this.lblDateChangeStatus.Text = dateChangeStatusString;
                    this.lblReasonChangeStatus.Text = org.Reason;
                    this.hfIsChangeStatus.Value = "true";
                }

                this.LINN.Text = org.INN;
                this.LOGRN.Text = org.OGRN;
                this.tbKPP.Text = org.KPP; 

                this.LDirectorName.Text = org.DirectorFullName;
                this.LDirectorNameInGenetive.Text = org.DirectorFullNameInGenetive;

                this.LDirectorPosition.Text = org.DirectorPosition;
                this.LDirectorPositionInGenetive.Text = org.DirectorPositionInGenetive;

                // Инструкция к применению
                this.Instruction.Text = ConfigurationManager.AppSettings["OrgCardInstruction"];

                // Должность руководителя
                this.HDNComboBoxDirectorPosition.Value = org.DirectorPosition;
                this.TBDirectorPosInGenetive.Text = org.DirectorPositionInGenetive;
                this.TBNameDirector.Text = org.DirectorFullName;
                this.HDNNameDirector.Value = org.DirectorFullName;
                this.TBNameInGenetive.Text = org.DirectorFullNameInGenetive;
                this.TBFirstName.Text = org.DirectorFirstName;
                this.TBLastName.Text = org.DirectorLastName;
                this.TBPatronymicName.Text = org.DirectorPatronymicName;

                this.LFactAddress.Text = org.FactAddress;
                this.LJurAddress.Text = org.LawAddress;

                this.TBFactAddress.Text = org.FactAddress;
                this.TBJurAddress.Text = org.LawAddress;

                this.LCityCode.Text = org.PhoneCityCode;
                this.LPhone.Text = org.Phone;
                this.LFax.Text = org.Fax;
                this.LEMail.Text = org.EMail;
                this.LSite.Text = org.Site;

                this.TBCityCode.Text = org.PhoneCityCode;
                this.TBPhone.Text = org.Phone;
                this.TBFax.Text = org.Fax;
                this.TBEMail.Text = org.EMail;
                this.TBWebPage.Text = org.Site;

                // Модель приемной кампании
                string modelName = org.RCModelId != 999 ? org.RCModelName : org.RCDescription;
                this.lblModelName.Text = string.IsNullOrEmpty(modelName) ? "—" : modelName;

                // Сведения об объеме и структуре приема
                //this.lblFBFullTime.Text = org.CNFullTime.ToString();
                //this.lblFBEvening.Text = org.CNEvening.ToString();
                //this.lblFBPostal.Text = org.CNPostal.ToString();

                // Краткое наименование
                this.tbShortName.Text = org.ShortName;

                // Заполняем списки и чекбокс
                this.LRegion.Text = org.Region.Name;
                this.lTownName.Text = org.TownName;
                this.lblOrgTypes.Text = org.OrgType.Name;
                if ((org.Kind != null) && (org.Kind.Id.HasValue))
                {
                    this.ddlOrgKind.SelectedValue = org.Kind.Id.ToString();
                }

                if ((org.IS != null) && (org.IS.Id.HasValue))
                {
                    this.ddlIS.SelectedValue = org.IS.Id.ToString();
                }

                this.LOPF.Text = org.IsPrivate ? "Негосударственный" : "Государственный";
                this.LIsFilial.Text = org.IsFilial ? "Да" : "Нет";
                this.phMainOrgName.Visible = org.IsFilial;
                this.lblMainOrgName.Text = org.MainFullName;

                //Лицензия
                this.lLicenseNumber.Text = org.LicenseRegNumber;
                if (org.LicenseOrderDocumentDate.HasValue)
                {
                    this.lLicenseIssueDate.Text = org.LicenseOrderDocumentDate.Value.ToShortDateString();
                }
                this.lLicenseStatus.Text = org.LicenseStatusName;
                if ((org.Status.Id == (int)OrganizationStatus.WithoutLicense) && (org.OrgType.Id == 1))
                {
                    lLicenseStatusWarning.Text = "Ваша лицензия не действительна, в ФИС ГИА и приема заявления абитуриентов нельзя будет включить в приказ о зачислении.";
                }

                //Приложение к лицензии FIS-1777 - added by akopylov 30.10.2017
                this.lSupplementNumber.Text = org.SupplementNumber;
                if (org.SupplementOrderDocumentDate.HasValue)
                {
                    this.lSupplementOrderDocumentDate.Text = org.SupplementOrderDocumentDate.Value.ToShortDateString();
                }
                this.lSupplementStatusName.Text = org.SupplementStatusName;

                // Запоминаем идентификатор типа организации для того, 
                // что бы скрыть ненужные поля для учредителя
                this.hiddenKindId.Value = org.OrgType.Id.ToString();

                this.OrgStatusLabel.Text = org.Status.Name;
                if (org.Status.Id == (int)OrganizationStatus.Reorganized)
                {
                    this.NewOrgNameLabel.Text = string.Format("Новая организация: <a href=\"/Administration/Organizations/UserDepartments/OrgCardInfo.aspx?OrgID={1}\">{0}</a>", org.NewOrgFullName, org.NewOrgId);
                }
                else
                {
                    this.NewOrgNameLabel.Visible = false;
                }
            }

            if (org != null)
            {
                tdFounders.InnerHtml = new OrgFoundersWidget(false , org.Founders).Html;
            }
        }

        public void SaveRCModel(int orgID)
        {
            this.TBNameDirector.Text = this.HDNNameDirector.Value;

            if (this.Page.IsValid)
            {
                var org = OrganizationDataAccessor.Get(orgID);

                // Модель приемной кампании
                org.RCModelId = this.rcModel;
                org.RCDescription = this.rcDescriptionText;

                // Руководитель организации
                org.DirectorPosition = this.ComboBoxDirectorPosition.SelectedItem.Text;
                org.DirectorPositionInGenetive = this.TBDirectorPosInGenetive.Text;
                org.DirectorFullName = this.TBNameDirector.Text;
                org.DirectorFullNameInGenetive = this.TBNameInGenetive.Text;
                org.DirectorFirstName = this.TBFirstName.Text;
                org.DirectorLastName = this.TBLastName.Text;
                org.DirectorPatronymicName = this.TBPatronymicName.Text;

                // Фактический/Юридический адрес
                org.FactAddress = this.TBFactAddress.Text;
                org.LawAddress = this.TBJurAddress.Text;

                org.ShortName = this.tbShortName.Text;
                org.KPP = this.tbKPP.Text;

                // Телефон, Код города, Факс, Email, Сайт
                org.PhoneCityCode = this.TBCityCode.Text;
                org.Phone = this.TBPhone.Text;
                org.Fax = this.TBFax.Text;
                org.EMail = this.TBEMail.Text;
                org.Site = this.TBWebPage.Text;
                org.OUConfirmation = true;

                //if (!String.IsNullOrEmpty(ddlFounders.SelectedValue))
                //{
                //    org.DepartmentId = Int32.Parse(ddlFounders.SelectedValue);
                //}
                //else
                //{
                //    org.DepartmentId = null;
                //}

                if (!String.IsNullOrEmpty(ddlOrgKind.SelectedValue))
                {
                    org.Kind = new CatalogElement(Convert.ToInt32(this.ddlOrgKind.SelectedValue))
                    {
                        Name = this.ddlOrgKind.SelectedItem.Text
                    };
                }

                if (!String.IsNullOrEmpty(ddlIS.SelectedValue))
                {
                    org.IS = new CatalogElement(Convert.ToInt32(this.ddlIS.SelectedValue))
                    {
                        Name = this.ddlIS.SelectedItem.Text
                    };
                }

                OrganizationDataAccessor.UpdateOrCreate(org, this.User.Identity.Name);

                this.Response.Redirect(
                    string.Format("OrgCardInfo.aspx?IsNew={1}&OrgID={0}", org.Id, orgID != org.Id), true);
            }
        }

        #endregion
    }
}