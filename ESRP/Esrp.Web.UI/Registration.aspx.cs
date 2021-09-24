namespace Esrp.Web
{
    using System;
    using System.Collections.Generic;

    using Esrp.Core.CatalogElements;
    using Esrp.Core.Organizations;
    using Esrp.Core.Users;

    /// <summary>
    /// The registration.
    /// </summary>
    public partial class Registration : BasePage
    {
        #region Constants and Fields

        /// <summary>
        /// The register send mail user list.
        /// </summary>
        public List<string> RegisterSendMailUserList = new List<string>();

        /// <summary>
        /// The founded org_.
        /// </summary>
        private Organization FoundedOrg_;

        #endregion

        #region Properties

        /// <summary>
        /// Gets FoundedOrg.
        /// </summary>
        private Organization FoundedOrg
        {
            get
            {
                if (this.FoundedOrg_ == null)
                {
                    this.FoundedOrg_ = OrganizationDataAccessor.Get(this.OrgID);
                }

                return this.FoundedOrg_;
            }
        }

        /// <summary>
        /// Gets OrgID.
        /// </summary>
        private int OrgID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrgID"]))
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

        #region Methods

        /// <summary>
        /// The on init.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.informationSystemList.OrgId = string.IsNullOrEmpty(this.Request.QueryString["OrgID"]) ? string.Empty : this.Request.QueryString["OrgID"];
        }

        /// <summary>
        /// Обработка события "Загрузка страницы"
        /// </summary>
        /// <param name="sender"> Источник события </param>
        /// <param name="e"> Параметры события </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.DDLOrgTypes.DataSource = CatalogDataAcessor.GetAll(CatalogDataAcessor.Catalogs.OrganizationTypes);

                this.DDLOrgTypes.DataBind();

                this.DDLOrganizationRegion.DataSource = RegionDataAcessor.GetAllInOrganization();
                this.DDLOrganizationRegion.DataBind();

                if (string.IsNullOrEmpty(this.Request.QueryString["OrgID"]))
                {
                    // Пользователь еще не выбирал организацию
                    this.ShowFirstTime();
                }
                else
                {
                    if (!this.UserCreatesNewOrg())
                    {
                        // если организация выбрана
                        this.ShowOrgFounded();
                    }
                    else
                    {
                        // пользователь не нашел свою организацию,создаем новую
                        this.ShowForNewOrg();
                    }
                }
            }
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Сохранить"
        /// </summary>
        /// <param name="sender">
        /// Источник события
        /// </param>
        /// <param name="e">
        /// TПараметры события
        /// </param>
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            this.informationSystemList.UpdateValidators();
            this.Validate();

            // проверю валидность контролов страницы
            if (!this.Page.IsValid)
            {
                return;
            }

            var user = new OrgUser();
            this.FillOrgUserWithOrgData(user);

            this.informationSystemList.Save(user);
        }

        /// <summary>
        /// The disable inputs.
        /// </summary>
        private void DisableInputs()
        {
            this.tbKPP.Enabled = false;
            this.TBFax.Enabled = false;
            this.TbFullName.Enabled = false;
            this.tbINN.Enabled = false;
            this.TBTownName.Enabled = false;
            this.TBJurAddress.Enabled = false;
            this.tbOGRN.Enabled = false;
            this.TBPhone.Enabled = false;
            this.DDLOPF.Enabled = false;
            this.DDLOrganizationRegion.Enabled = false;
            this.DDLOrgTypes.Enabled = false;
        }

        /// <summary>
        /// The fill org user with org data.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        private void FillOrgUserWithOrgData(OrgUser user)
        {
            user.RequestedOrganization.INN = this.tbINN.Text.Trim();
            user.RequestedOrganization.OGRN = this.tbOGRN.Text.Trim();
            user.RequestedOrganization.KPP = this.tbKPP.Text.Trim();
            user.RequestedOrganization.FactAddress = this.FoundedOrg.FactAddress;
            user.RequestedOrganization.LawAddress = this.FoundedOrg.LawAddress;
            user.RequestedOrganization.TownName = this.FoundedOrg.TownName;
            user.RequestedOrganization.DirectorFullName = this.FoundedOrg.DirectorFullName;
            user.RequestedOrganization.OwnerDepartment = this.FoundedOrg.OwnerDepartment;
            user.RequestedOrganization.FullName = this.TbFullName.Text.Trim();
            user.RequestedOrganization.ShortName = this.FoundedOrg.ShortName;
            user.RequestedOrganization.Phone = this.TBPhone.Text.Trim();
            user.RequestedOrganization.Fax = this.TBFax.Text.Trim();

            user.RequestedOrganization.RCModelId = this.FoundedOrg.RCModelId;
            user.RequestedOrganization.RCModelName = this.FoundedOrg.RCModelName;
            user.RequestedOrganization.RCDescription = this.FoundedOrg.RCDescription;

            user.RequestedOrganization.Region =
                new CatalogElement(Convert.ToInt32(this.DDLOrganizationRegion.SelectedValue))
                    {
                        Name = this.DDLOrganizationRegion.SelectedItem.Text
                    };

            user.RequestedOrganization.OrgType = new CatalogElement(Convert.ToInt32(this.DDLOrgTypes.SelectedValue))
                {
                    Name = this.DDLOrgTypes.SelectedItem.Text
                };

            user.RequestedOrganization.ReceptionOnResultsCNE = this.FoundedOrg.ReceptionOnResultsCNE;

            // А если создается новая организация, форма "большая", берем еще данные
            if (this.UserCreatesNewOrg())
            {
                user.RequestedOrganization.ShortName = this.FoundedOrg.ShortName;
                user.RequestedOrganization.DirectorPosition = this.FoundedOrg.DirectorPosition;
                user.RequestedOrganization.AccreditationSertificate = this.FoundedOrg.AccreditationSertificate;
                user.RequestedOrganization.PhoneCityCode = this.FoundedOrg.PhoneCityCode;
                user.RequestedOrganization.EMail = this.FoundedOrg.EMail;
                user.RequestedOrganization.Site = this.FoundedOrg.Site;
                user.RequestedOrganization.IsFilial = this.FoundedOrg.IsFilial;
                user.RequestedOrganization.IsPrivate = this.FoundedOrg.IsPrivate;
                user.RequestedOrganization.Kind = this.FoundedOrg.Kind;
            }
            else if (this.FoundedOrg != null)
            {
                // Нашли организацию
                user.RequestedOrganization.OrganizationId = this.FoundedOrg.Id;
                user.RequestedOrganization.IsPrivate = this.FoundedOrg.IsPrivate;
                user.RequestedOrganization.OrgType = new CatalogElement((int)this.FoundedOrg.OrgType.Id);
            }
        }

        /// <summary>
        /// The show first time.
        /// </summary>
        private void ShowFirstTime()
        {
            this.DisableInputs();
            this.TbFullName.ToolTip = "Попробуйте сначала найти организацию (нажмите кнопку [Выбрать]).";
            this.DDLOrgTypes.ToolTip = "Попробуйте сначала найти организацию (нажмите кнопку [Выбрать]).";
            this.DDLOrganizationRegion.ToolTip = "Попробуйте сначала найти организацию (нажмите кнопку [Выбрать]).";
        }

        /// <summary>
        /// The show for new org.
        /// </summary>
        private void ShowForNewOrg()
        {
            this.TbFullName.Focus();
            this.pnlNotification.Visible = true;
            this.btnChangeOrg.Visible = false;

            this.DDLOrgTypes.Items.Clear();
            this.DDLOrgTypes.DataSource = OrgTypeDataAccessor.GetAllForAddNew();
            this.DDLOrgTypes.DataBind();
        }

        /// <summary>
        /// Пользователь выбрал свою организацию на странице поиска
        /// </summary>
        private void ShowOrgFounded()
        {
            this.TbFullName.Text = this.FoundedOrg.FullName;
            this.TbFullName.Enabled = false;

            this.DDLOrgTypes.SelectedValue = this.FoundedOrg.OrgType.Id.ToString();
            this.DDLOrgTypes.Enabled = false;

            if (this.FoundedOrg.IsPrivate)
            {
                DDLOPF.SelectedValue = "1";
            }
            else
            {
                DDLOPF.SelectedValue = "0";
            }
            this.DDLOPF.Enabled = false;


            this.DDLOrgTypes.SelectedValue = this.FoundedOrg.OrgType.Id.ToString();
            this.DDLOrgTypes.Enabled = false;

            this.DDLOrganizationRegion.SelectedValue = this.FoundedOrg.Region.Id.ToString();
            this.DDLOrganizationRegion.Enabled = false;

            this.TBTownName.Text = this.FoundedOrg.TownName;
            this.TBTownName.Enabled = false;

            this.TBJurAddress.Text = this.FoundedOrg.LawAddress;
            this.TBJurAddress.Enabled = false;

            this.TBPhone.Text = this.FoundedOrg.Phone;
            this.TBPhone.Enabled = false;

            this.tbKPP.Text = this.FoundedOrg.KPP;
            this.tbKPP.Enabled = false;

            this.tbOGRN.Text = this.FoundedOrg.OGRN;
            this.tbOGRN.Enabled = false;

            this.tbINN.Text = this.FoundedOrg.INN;
            this.tbINN.Enabled = false;

            this.TBFax.Text = this.FoundedOrg.Fax;
            this.TBFax.Enabled = false;
        }

        /// <summary>
        /// The user creates new org.
        /// </summary>
        /// <returns>
        /// The user creates new org.
        /// </returns>
        private bool UserCreatesNewOrg()
        {
            return this.OrgID <= 0;
        }

        #endregion
    }
}