namespace Esrp.Web.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Security.Principal;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    using Esrp.Core;
    using Esrp.Core.Organizations;
    using Esrp.Core.Systems;
    using Esrp.Utility;
    using Esrp.Core.CatalogElements;

    /// <summary>
    /// The user profile view.
    /// </summary>
    public partial class UserProfileView : UserControl
    {
        #region Constants and Fields

        private UserAccount mCurrentUser;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets CurrentUser.
        /// </summary>
        /// <exception cref="NullReferenceException">
        /// </exception>
        public UserAccount CurrentUser
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
        /// Gets User.
        /// </summary>
        public IPrincipal User
        {
            get
            {
                return HttpContext.Current.User;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The get organization id.
        /// </summary> 
        /// <returns>
        /// The get organization id.
        /// </returns>
        protected int OrganizationId
        {
            get
            {
                return Organization.Id;
            }
        }

        private Organization _org;
        protected Organization Organization
        {
            get
            {
                if (_org == null)
                {
                    _org = OrganizationDataAccessor.GetByLogin(this.User.Identity.Name);
                }
                return _org;
            }
        }

        /// <summary>
        /// The on pre render.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnPreRender(EventArgs e)
        {
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

            this.Button1.Visible = GeneralSystemManager.IsOpenSystem();

            // Модель приемной кампании
            string modelName = this.CurrentUser.RCModelID != 999
                                   ? this.CurrentUser.ModelName
                                   : this.CurrentUser.RCDescription;
            this.lblModelName.Text = string.IsNullOrEmpty(modelName) ? "—" : modelName;

            this.lblSystem.Text = this.lblSystemNamesForFio.Text = this.lblSystemNamesForPhone.Text = string.Empty;
            List<string> systemNames = GeneralSystemManager.AccessedSystems(this.CurrentUser.Login);

            this.lblSystem.Text =
                this.lblSystemNamesForFio.Text =
                this.lblSystemNamesForPhone.Text =
                string.Join(", ", systemNames.Cast<object>().Select(c => c.ToString()).ToArray());
            char[] charsToTrim = { ',' };
            this.lblSystem.Text =
                this.lblSystemNamesForFio.Text =
                this.lblSystemNamesForPhone.Text = this.lblSystem.Text.Trim(charsToTrim);

            if (this.CurrentUser.OrgTypeId == 1 || this.CurrentUser.OrgTypeId == 2)
            {
                this.trModel.Visible = true; 
            }

            //if ((this.CurrentUser.TimeConnectionToSecureNetwork != null) && (this.CurrentUser.IsAgreedTimeConnection == true))
            //{
            //    this.lblTimeConnectionToSecureNetwork.Text = this.CurrentUser.TimeConnectionToSecureNetwork is DateTime ? ((DateTime)this.CurrentUser.TimeConnectionToSecureNetwork).ToShortDateString() : string.Empty;
            //    this.trTimeConnectionToSecureNetwork.Visible = true;
            //}

            //if ((this.CurrentUser.TimeEnterInformationInFIS != null) && (this.CurrentUser.IsAgreedTimeEnterInformation == true))
            //{
            //    this.lblTimeEnterInformationInFIS.Text = this.CurrentUser.TimeEnterInformationInFIS is DateTime ? ((DateTime)this.CurrentUser.TimeEnterInformationInFIS).ToShortDateString() : string.Empty;
            //    this.trTimeEnterInformationInFIS.Visible = true;
            //}

            //this.lblTimeConnectionToSecureNetwork.ForeColor = this.lblTimeEnterInformationInFIS.ForeColor = 
            //this.lblTimeConnectionToSecureNetworkTitle.ForeColor = this.lblTimeEnterInformationInFISTitle.ForeColor = Color.Red;

            if (Organization != null)
            {
                tdFounders.InnerHtml = new OrgFoundersWidget(false, Organization.Founders).Html;
            }
        }

        private bool ShouldRefreshUserRolesLis()
        {
            UserAccount.UserAccountStatusEnum a = this.CurrentUser.Status;
            return (this.CurrentUser.Status == UserAccount.UserAccountStatusEnum.Activated
                    && !this.User.IsInRole("ViewCertificateSection"))
                   ||
                   (this.CurrentUser.Status != UserAccount.UserAccountStatusEnum.Activated
                    && this.User.IsInRole("ViewCertificateSection"));
        }

        #endregion
    }
}