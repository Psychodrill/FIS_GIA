namespace Fbs.Web.Certificates.CommonNationalCertificates
{
    using System;
    using System.Web.UI.WebControls;

    using Fbs.Web.ViewBase;

    /// <summary>
    /// The wildcard request result.
    /// </summary>
    public partial class WildcardRequestResult : CertificateCheckResultBase
    {
        // Номер колонки "Документ".
        #region Constants and Fields

        private static int DocumentColumnNumber = 4;

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
            // this.PageTitle = AccountExtentions.GetFullName(Request.QueryString["LastName"], 
            // Request.QueryString["FirstName"], Request.QueryString["PatronymicName"]);
        }

        /// <summary>
        /// The dg search_ init.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void dgSearch_Init(object sender, EventArgs e)
        {
            // Покажу колонку "Документ" если пользователь имеет роль ViewCommonNationalCertificateDocument.
            this.dgSearch.Columns[DocumentColumnNumber].Visible =
                this.User.IsInRole("ViewCommonNationalCertificateDocument");
        }

        /// <summary>
        /// The dg search_ pre render.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void dgSearch_PreRender(object sender, EventArgs e)
        {
            this.phUniqueChecks.Visible = this.dgSearch.Items.Count > 0;
        }

        /// <summary>
        /// The ds search_ selecting.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void dsSearch_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            // Позволяем команде выполняться несколько минут (см. конфиг)
            e.Command.CommandTimeout = Config.WildcardCommandTimeout;
        }

        #endregion
    }
}