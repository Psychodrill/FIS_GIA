namespace Fbs.Web.Certificates.CommonNationalCertificates
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Web;
    using System.Web.UI;

    using Fbs.Core.CNEChecks;
    using Fbs.Core.Organizations;
    using Fbs.Core.Shared;
    using Fbs.Core.Users;

    /// <summary>
    /// Справка о свидетельстве
    /// </summary>
    public partial class PrintNote : Page
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets Cert.
        /// </summary>
        public CNEInfo Cert { get; set; }

        /// <summary>
        /// Gets or sets OrganizationName.
        /// </summary>
        public string OrganizationName { get; set; }

        /// <summary>
        /// Настройка web.config 
        /// </summary>
        public bool EnableOpenedFbs
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["EnableOpenedFbs"]);
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
            if (!this.Page.IsPostBack)
            {
                Guid key;
                if (Utils.ParseGuid(this.Request.QueryString["id"], out key))
                {
                    this.Cert = this.LoadCertificateInfo(key);
                }

                if (this.Cert == null)
                {
                    this.phNote.Visible = false;
                    this.phEmpty.Visible = true;
                }
                else
                {
                    // Загружаем организацию
                    this.OrganizationName = this.GetOrganizationName();

                    this.phNote.Visible = true;
                    this.phEmpty.Visible = false;
                }
            }
        }

        private string GetOrganizationName()
        {
            int orgId = 0;
            Organization org = null;
            if (this.Session["OrgId"]!=null && !String.IsNullOrEmpty(this.Session["OrgId"].ToString()) && Int32.TryParse(this.Session["OrgId"].ToString(), out orgId))
            {
                org = OrganizationDataAccessor.Get(orgId);
            }
            else
                org = OrgUserDataAccessor.Get(HttpContext.Current.User.Identity.Name).RequestedOrganization;

            if (org != null)
            {
                return org.FullName;
            }
            return "";
        }

        private CNEInfo LoadCertificateInfo(Guid key)
        {
            if (this.Session["CertificateInfo"] != null
                && this.Session["CertificateInfo"] is KeyValuePair<Guid, CNEInfo>
                && ((KeyValuePair<Guid, CNEInfo>)this.Session["CertificateInfo"]).Key == key)
            {
                return ((KeyValuePair<Guid, CNEInfo>)this.Session["CertificateInfo"]).Value;
            }

            return null;
        }

        #endregion
    }
}