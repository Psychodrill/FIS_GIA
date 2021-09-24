namespace Esrp.Web.Administration.Organizations
{
    using System;
    using System.Web.UI.WebControls;
using Esrp.Core.Organizations;

    /// <summary>
    /// История изменений организации.
    /// </summary>
    public partial class OrganizationHistory : BasePage
    {
        private int? organizationId;
        private Organization mcurrentOrg;
        private const string ErrorOrgNotFound = "Организация \"{0}\" не найдена";

        /// <summary>
        /// id организации из реквеста
        /// </summary>
        protected int OrganizationId
        {
            get
            {
                if (this.organizationId == null)
                {
                    string requestValue = this.Request.Params["OrgId"];
                    if (string.IsNullOrEmpty(requestValue))
                    {
                        throw new ArgumentException("OrgId must not be empty");
                    }

                    int tryParse;
                    if (!int.TryParse(requestValue, out tryParse))
                    {
                        throw new ArgumentException("OrgId should be of integer value");
                    }

                    this.organizationId = tryParse;
                }

                return this.organizationId.Value;
            }
        }

        public Organization currentOrg
        {
            get
            {
                if (mcurrentOrg == null)
                {
                    mcurrentOrg = OrganizationDataAccessor.Get(OrganizationId);
                }

                if (mcurrentOrg == null)
                {
                    throw new NullReferenceException(string.Format(ErrorOrgNotFound, OrganizationId));
                }

                return mcurrentOrg;
            }
        }

        #region Methods

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

        /// <summary>
        /// добавить пейджинг и организацию при селекте истории
        /// </summary>
        /// <param name="sender">дата сорс грида</param>
        /// <param name="e">параметры</param>
        protected void dsHistoryList_OnObjectCreating(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["organizationId"] = this.OrganizationId;
            e.InputParameters["startRow"] = DataSourcePagerHead.CurrentStartRowIndex;
            e.InputParameters["maxRow"] = DataSourcePagerHead.ActualDefaultMaxRowCount;
        }

        /// <summary>
        /// добавить аргументы для получения числа строк 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dsHistoryListCount_OnObjectCreating(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["organizationId"] = this.OrganizationId;
        }
        #endregion
    }
}