namespace Esrp.Web.Administration.Requests
{
    using System;
    using System.Drawing;
    using System.Linq;
    using System.Security.Principal;
    using System.Web;
    using System.Web.UI.WebControls;

    using Esrp.Core;
    using Esrp.Core.Organizations;
    using Esrp.Core.Systems;
    using Esrp.Core.Users;
    using Esrp.Utility;

    using FogSoft.Helpers;

    /// <summary>
    /// The request form.
    /// </summary>
    public partial class RequestForm : BasePage
    {
        #region Constants and Fields

        protected Organization EtalonOrg;

        protected OrgRequest OrgRequest;

        private UserAccount mCurrentUser;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether IsRegistrationDocumentExists.
        /// </summary>
        public bool IsRegistrationDocumentExists
        {
            get
            {
                return this.CurrentUser.RegistrationDocument.Document != null;
            }
        }

        /// <summary>
        /// Gets RequestID.
        /// </summary>
        public int RequestID
        {
            get
            {
                return this.Request.QueryString["RequestID"].To(0);
            }
        }

        /// <summary>
        /// Gets User.
        /// </summary>
        public new IPrincipal User
        {
            get
            {
                return HttpContext.Current.User;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets CurrentUser.
        /// </summary>
        /// <exception cref="NullReferenceException">
        /// </exception>
        protected UserAccount CurrentUser
        {
            get
            {
                if (this.mCurrentUser == null)
                {
                    this.mCurrentUser = UserAccount.GetUserAccount(Account.ClientLogin);
                }

                if (this.mCurrentUser == null)
                {
                    throw new NullReferenceException(
                        string.Format("Пользователь \"{0}\" не найден", Account.ClientLogin));
                }

                return this.mCurrentUser;
            }
        }

        #endregion

        #region Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                if (Request.UrlReferrer != null && Request.UrlReferrer.LocalPath!=null)
                {
                    if (Request.UrlReferrer.LocalPath.Contains("List.aspx"))
                    {
                        Session["BackLink.HRef"] = Request.UrlReferrer.ToString();
                    }
                    BackLink.HRef = (string)Session["BackLink.HRef"];
                }
            }
        }

        /// <summary>
        /// The on init.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.CommentPanel.OrganizationRequestID = this.RequestID;
            this.usersInfo.RequestID = this.RequestID;
        }

        /// <summary>
        /// The on load.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.LoadData();
            this.CompareAll_WithEtalonOrgInfo();
        }

        /// <summary>
        /// The on pre render.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnPreRender(EventArgs e)
        {
            // Установлю заголовок страницы
            this.PageTitle = string.Format("Заявление на регистрацию №{0}", this.OrgRequest.RequestID);

            // Обновляю список ролей пользователя если это необходимо
            if (this.ShouldRefreshUserRolesLis())
            {
                // Очищаю список ролей текущего пользователя, для того, чтобы этот самый список 
                // обновить, не дожидаясь, пока истечет время жизни кукиса.
                if (this.Request.Cookies[Config.RoleManagerCookeiName] != null)
                {
                    this.Request.Cookies[Config.RoleManagerCookeiName].Value = string.Empty;
                }

                if (this.Response.Cookies[Config.RoleManagerCookeiName] != null)
                {
                    this.Response.Cookies[Config.RoleManagerCookeiName].Value = string.Empty;
                }
            }

            this.ManageUI();
        }

        private void CompareAll_WithEtalonOrgInfo()
        {
            int etalonOrgId = 0;
            if (this.OrgRequest.Organization.OrganizationId != 0)
            {
                etalonOrgId = this.OrgRequest.Organization.OrganizationId;
            }

            var org = OrganizationDataAccessor.Get(etalonOrgId);

            if (org != null)
            {
                this.CompareWithEtalonOrgInfo(
                    this.OrgRequest.Organization.OrgFullName, org.FullName, this.lblEtalonOrgName);
                this.CompareWithEtalonOrgInfo(
                    this.OrgRequest.Organization.LawAddress, org.LawAddress, this.lblEtalonAddress);
                this.CompareWithEtalonOrgInfo(
                    this.OrgRequest.Organization.DirectorFullName, org.DirectorFullName, this.lblEtalonChief);
                this.CompareWithEtalonOrgInfo(
                    this.OrgRequest.Organization.FounderName, org.OwnerDepartment, this.lblEtalonFounder);
                this.CompareWithEtalonOrgInfo(this.OrgRequest.Organization.Phone, org.Phone, this.lblEtalonOrgPhone);
                this.CompareWithEtalonOrgInfo(
                    this.OrgRequest.Organization.OrgTypeName, org.OrgType.Name, this.lblEtalonOrgType);
                this.CompareWithEtalonOrgInfo(
                    this.OrgRequest.Organization.RegionName, org.Region.Name, this.lblEtalonRegion);

                this.CompareWithEtalonOrgInfo(this.OrgRequest.Organization.KPP, org.KPP, this.lblEtalonKPP);
                this.CompareWithEtalonOrgInfo(this.OrgRequest.Organization.INN, org.INN, this.lblEtalonINN);
                this.CompareWithEtalonOrgInfo(this.OrgRequest.Organization.OGRN, org.OGRN, this.lblEtalonOGRN);
                if (this.OrgRequest.Organization.OrgTypeId == 1 || this.OrgRequest.Organization.OrgTypeId == 2)
                {
                    this.trReceptionOnResultsCNE.Visible = true;
                    var currentReceptionOnResultsCNE = string.Empty;
                    if (this.OrgRequest.Organization.ReceptionOnResultsCNE != null)
                    {
                        currentReceptionOnResultsCNE = Convert.ToInt32(this.OrgRequest.Organization.ReceptionOnResultsCNE) != 0 ? "Не проводится" : "Проводится";
                    }

                    this.lblReceptionOnResultsCNE.Text = currentReceptionOnResultsCNE;

                    var orgReceptionOnResultsCNE = string.Empty;
                    if (org.ReceptionOnResultsCNE != null)
                    {
                        orgReceptionOnResultsCNE = Convert.ToInt32(org.ReceptionOnResultsCNE) != 0 ? "Не проводится" : "Проводится";
                    }

                    this.CompareWithEtalonOrgInfo(currentReceptionOnResultsCNE, orgReceptionOnResultsCNE, this.lblEtalonReceptionOnResultsCNE);
                }
            }
        }

        private void CompareWithEtalonOrgInfo(string text, string etalonText, Label label)
        {
            label.Text = string.IsNullOrEmpty(etalonText) ? "[нет данных]" : etalonText;
            label.ToolTip = @"Данные эталонной организации";
            label.ForeColor = text == etalonText ? Color.Gray : Color.Red;
        }

        private void LoadData()
        {
            this.OrgRequest = OrgRequestManager.GetRequest(this.RequestID);

            var etalonOrgId = 0;
            if (this.OrgRequest.Organization.OrganizationId != 0)
            {
                etalonOrgId = this.OrgRequest.Organization.OrganizationId;
            }

            this.EtalonOrg = OrganizationDataAccessor.Get(etalonOrgId);
            if (this.EtalonOrg == null)
            {
                this.EtalonOrg = new Organization(
                    string.Empty, 0, 0, 0, true, string.Empty, string.Empty, 1, null, string.Empty, 0, string.Empty);
            }
        }

        private void ManageUI()
        {
            string linkUrl = "/Administration/Requests/Action.aspx?requestID=" + RequestID + "&action=";
            this.liActivate.Visible = this.OrgRequest.CanBeActivated();
            this.liActivate.HRef = linkUrl + (int)UserAccount.UserAccountStatusEnum.Activated;
            this.liDeactivate.Visible = this.OrgRequest.CanBeDeactivated();
            this.liDeactivate.HRef = linkUrl + (int)UserAccount.UserAccountStatusEnum.Deactivated;
            this.liRevision.Visible = this.OrgRequest.CanBeSentOnRevision();
            this.liRevision.HRef = linkUrl + (int)UserAccount.UserAccountStatusEnum.Revision;
        }

        // Установление необходимости обновления списка ролей пользователя
        private bool ShouldRefreshUserRolesLis()
        {
            // TODO: выделить статичесский класс под роли и перечислить их там
            // Если пользователь активирован и у него нет роли на просмотр секции свидетельств или,
            // наоборот, пользователь неактивен, а роль у него есть,то обновлю список ролей 
            // пользователя
            UserAccount.UserAccountStatusEnum a = this.CurrentUser.Status;
            bool b = this.User.IsInRole("ViewCertificateSection");
            bool k = this.User.IsInRole("ViewStatisticSubordinate");
            bool iii = this.User.IsInRole("EditSelfOrganization");

            return (this.CurrentUser.Status == UserAccount.UserAccountStatusEnum.Activated
                    && !this.User.IsInRole("ViewCertificateSection"))
                   ||
                   (this.CurrentUser.Status != UserAccount.UserAccountStatusEnum.Activated
                    && this.User.IsInRole("ViewCertificateSection"));
        }

        #endregion
    }
}