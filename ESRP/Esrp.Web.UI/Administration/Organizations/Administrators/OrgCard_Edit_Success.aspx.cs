namespace Esrp.Web.Administration.Organizations.Administrators
{
    using System;
    using Esrp.Core.Organizations;

    /// <summary>
    /// Карточка просмотра организации
    /// </summary>
    public partial class OrgCard_Edit_Success : BasePage
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