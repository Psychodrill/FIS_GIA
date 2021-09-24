namespace Fbs.Web.Administration.Organizations.Administrators
{
    using System;

    /// <summary>
    /// Карточка просмотра организации
    /// </summary>
    public partial class OrgCard_Edit_Success : BasePage
    {
        /// <summary>
        /// инициализировать состояние контролов
        /// </summary>
        /// <param name="e">у</param>
        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);
            if ((!string.IsNullOrEmpty(this.Request["IsNew"])) && (this.Request["IsNew"].ToLower() == "true"))
            {
                this.OrganizationView.Message = "Новая организация успешно создана!";
            }
            else
            {
                this.OrganizationView.Message = "Данные организации успешно обновлены!";
            }

            var orgId = this.GetParamInt("OrgId");
            if (orgId == 0)
            {
                throw new ArgumentException("OrgId cannot be empty");
            }

            this.OrganizationView.OrganizationId = orgId;
        }
    }
}