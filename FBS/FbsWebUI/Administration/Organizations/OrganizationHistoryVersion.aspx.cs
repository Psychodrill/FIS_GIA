namespace Fbs.Web.Administration.Organizations
{
    using System;

    /// <summary>
    /// страница просмотра версии организации
    /// </summary>
    public partial class OrganizationHistoryVersion : BasePage
    {
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
    }
}