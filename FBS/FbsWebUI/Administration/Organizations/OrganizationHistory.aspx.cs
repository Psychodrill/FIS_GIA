namespace Fbs.Web.Administration.Organizations
{
    using System;
    using System.Web.UI.WebControls;

    /// <summary>
    /// История изменений организации.
    /// </summary>
    public partial class OrganizationHistory : BasePage
    {
        private int? organizationId;

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

        #region Methods

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