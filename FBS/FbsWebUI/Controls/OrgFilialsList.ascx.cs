namespace Fbs.Web.Controls
{
    using System.Web.UI;
    using System.Web.UI.WebControls;

    /// <summary>
    /// Рендер таблицы филиалов организации
    /// </summary>
    public partial class OrgFilialsList : UserControl
    {
        #region Constants and Fields

        private int orgId;

        #endregion

        #region Public Properties

        /// <summary>
        /// ID головной организации
        /// </summary>
        public int OrganizationID
        {
            get
            {
                if (this.Page.Request.QueryString["OrgID"] != null)
                {
                    int returnVal;
                    if (int.TryParse(this.Page.Request.QueryString["OrgID"], out returnVal))
                    {
                        return returnVal;
                    }
                }

                return 0;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Инициализирует dataSource
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void DataSourceOnSelectingHandler(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@MainOrgId"].Value = this.OrganizationID;
        }

        #endregion
    }
}