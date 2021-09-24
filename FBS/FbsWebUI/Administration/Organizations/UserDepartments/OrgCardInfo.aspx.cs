namespace Fbs.Web.Administration.Organizations.UserDepartments
{
    using System;

    /// <summary>
    /// карточка просмотра организации
    /// </summary>
    public partial class OrgCardInfo : BasePage
    {
        /// <summary>
        /// инициализировать состояние контролов
        /// </summary>
        /// <param name="e">у</param>
        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);
            var orgId = this.GetParamInt("OrgId");
            if (orgId == 0)
            {
                throw new ArgumentException("OrgId cannot be empty");
            }

            this.OrganizationView.OrganizationId = orgId;
        }
    }
}
