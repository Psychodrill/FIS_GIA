namespace Esrp.Web.Administration.Accounts.Users
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Web.UI.WebControls;

    using Esrp.Core;
    using Esrp.Core.Organizations;
    using Esrp.Core.Systems;
    using Esrp.Core.Users;
    using System.Web;
    using Esrp.Core.Loggers;
    using System.Configuration;
    using Esrp.Web.Administration.SqlConstructor.UserAccounts;

    /// <summary>
    /// The view.
    /// </summary>
    public partial class View : BasePage
    {
        #region Constants and Fields

        //private string _errorMessage;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets CurrentUser.
        /// </summary>
        public OrgUser CurrentUser { get; set; }

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
        /// The is user admin or support.
        /// </summary>
        /// <returns>
        /// The is user admin or support.
        /// </returns>
        public bool isUserAdminOrSupport()
        {
            Type accountType = Account.GetType(Account.ClientLogin);

            // Определить страницу перехода
            if (accountType == typeof(AdministratorAccount))
            {
                return true;
            }

            if (accountType == typeof(SupportAccount))
            {
                return true;
            }

            return false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The on pre render.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            //this.ManageUI();
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
        /// <exception cref="NullReferenceException">
        /// </exception>
        protected void Page_Load(object sender, EventArgs e)
        {
              
            // CurrentUser = UserAccount.GetUserAccount(Login);        	
            this.CurrentUser = OrgUserDataAccessor.Get(this.Login);

            if (this.CurrentUser == null)
            {
                throw new NullReferenceException(string.Format("Пользователь \"{0}\" не найден", this.Login));
            }
            if(Convert.ToBoolean(ConfigurationManager.AppSettings["LogUserView"]))
                AccountEventLogger.LogAccountViewEvent(this.Login);
            // Установлю заголовок страницы
            this.PageTitle = string.Format("Регистрационные данные “{0}”", this.CurrentUser.login);

            this.CompareAll_WithEtalonOrgInfo();

            this.lblSystem.Text = string.Empty;
            var systemNames = GeneralSystemManager.AccessedSystems(this.CurrentUser.login);

            this.lblSystem.Text = string.Join(", ", systemNames.Cast<object>().Select(c => c.ToString()).ToArray());
            char[] charsToTrim = { ',' };
            this.lblSystem.Text = this.lblSystem.Text.Trim(charsToTrim);

            this.lblGroup.Text = SqlConstructor_GetUsersIS.GetUserGroupNames(this.CurrentUser.login);

            this.BindUsers();

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
        /// The btn deactivate_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void btnDeactivate_Click(object sender, EventArgs e)
        {
        }

        private void BindUsers()
        {
            if (this.CurrentUser.RequestedOrganization.OrganizationId != null)
            {
                var OrgId = (int)this.CurrentUser.RequestedOrganization.OrganizationId;
                DataTable table = OrgUserDataAccessor.GetByOrgAsTable(OrgId); // GetUsers(OrgId);
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
                this.rptUsers.Visible = false;
                this.lblNoUsers.Visible = true;
            }
        }

        private void CompareAll_WithEtalonOrgInfo()
        {
            int etalonOrgId = 0;
            if (this.CurrentUser.RequestedOrganization.OrganizationId != null)
            {
                etalonOrgId = (int)this.CurrentUser.RequestedOrganization.OrganizationId;
            }

            Organization Org = OrganizationDataAccessor.Get(etalonOrgId);

            if (Org != null)
            {
                this.CompareWithEtalonOrgInfo(this.CurrentUser.RequestedOrganization.KPP, Org.KPP, this.lblEtalonKPP);
                this.CompareWithEtalonOrgInfo(
                    this.CurrentUser.RequestedOrganization.FullName, Org.FullName, this.lblEtalonOrgName);
                this.CompareWithEtalonOrgInfo(
                    this.CurrentUser.RequestedOrganization.LawAddress, Org.LawAddress, this.lblEtalonAddress);
                this.CompareWithEtalonOrgInfo(
                    this.CurrentUser.RequestedOrganization.DirectorFullName, Org.DirectorFullName, this.lblEtalonChief);
                this.CompareWithEtalonOrgInfo(
                    this.CurrentUser.RequestedOrganization.OwnerDepartment, Org.OwnerDepartment, this.lblEtalonFounder);
                this.CompareWithEtalonOrgInfo(
                    this.CurrentUser.RequestedOrganization.Phone, Org.Phone, this.lblEtalonOrgPhone);
                this.CompareWithEtalonOrgInfo(
                    this.CurrentUser.RequestedOrganization.OrgType.Name, Org.OrgType.Name, this.lblEtalonOrgType);
                this.CompareWithEtalonOrgInfo(
                    this.CurrentUser.RequestedOrganization.Region.Name, Org.Region.Name, this.lblEtalonRegion);

                if (this.CurrentUser.RequestedOrganization.OrgType.Id == 1 || this.CurrentUser.RequestedOrganization.OrgType.Id == 2)
                {
                    this.trReceptionOnResultsCNE.Visible = true;
                    var currentReceptionOnResultsCNE = string.Empty;
                    if (this.CurrentUser.RequestedOrganization.ReceptionOnResultsCNE != null)
                    {
                        currentReceptionOnResultsCNE = Convert.ToInt32(this.CurrentUser.RequestedOrganization.ReceptionOnResultsCNE) != 0 ? "Не проводится" : "Проводится";
                    }

                    this.lblReceptionOnResultsCNE.Text = currentReceptionOnResultsCNE;

                    var orgReceptionOnResultsCNE = string.Empty;
                    if (Org.ReceptionOnResultsCNE != null)
                    {
                        orgReceptionOnResultsCNE = Convert.ToInt32(Org.ReceptionOnResultsCNE) != 0 ? "Не проводится" : "Проводится";
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

        private void ManageUI()
        {
            //this.pErrorMessage.Visible = false;

            //if (!string.IsNullOrEmpty(this._errorMessage))
            //{
            //    this.pErrorMessage.Visible = true;
            //    this.pErrorMessage.InnerText = this._errorMessage;
            //}

        }
        
        #endregion
    }
}