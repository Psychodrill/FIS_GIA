namespace Esrp.Web.Administration.Organizations
{
    using System;
    using Esrp.Core.Organizations;

    /// <summary>
    /// страница просмотра версии организации
    /// </summary>
    public partial class OrganizationHistoryVersion : BasePage
    {
        private Organization mcurrentOrg;
        private const string ErrorOrgNotFound = "Организация \"{0}\" не найдена";

        public Organization currentOrg
        {
            get
            {
                if (mcurrentOrg == null)
                {
                    mcurrentOrg = OrganizationDataAccessor.Get(this.GetParamInt("OrgId"));
                }

                if (mcurrentOrg == null)
                {
                    throw new NullReferenceException(string.Format(ErrorOrgNotFound, this.GetParamInt("OrgId")));
                }

                return mcurrentOrg;
            }
        }

        /// <summary>
        /// инициализировать состояние контролов
        /// </summary>
        /// <param name="e">у</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            var orgId = this.GetParamInt("OrgId");
            if (orgId == 0)
            {
                throw new ArgumentException("OrgId cannot be empty");
            }

            this.OrganizationView.OrganizationId = orgId;

            var version = this.GetParamInt("Version");
            if (version == 0)
            {
                throw new ArgumentException("Version cannot be empty");
            }

            this.OrganizationView.OrganizationId = orgId;
            this.OrganizationView.Version = version;
            this.OrganizationView.Message = string.Format("Версия {0}", version);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                if (Request.UrlReferrer != null && Request.UrlReferrer.LocalPath!=null)
                {
                    if (Request.UrlReferrer.LocalPath.Contains("OrgList.aspx"))
                    {
                        Session["BackLink.HRef"] = Request.UrlReferrer.ToString();
                    }
                    BackLink.HRef = (string)Session["BackLink.HRef"];
                }
            }
        }
    }
}