namespace Esrp.Web.Administration.InformationSystems
{
    using System;
    using System.Web.UI;
    using Esrp.Services;

    /// <summary>
    /// The information systems list.
    /// </summary>
    public partial class InformationSystemsList : Page
    {
        private readonly InformationSystemsService informationSystemsService = new InformationSystemsService();

        #region Methods

        /// <summary>
        /// Обработчик события "Загрузка страницы"
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            var currentSystemId = this.informationSystemsService.GetCurrentSystemId();
            this.CreateInformationSystem.HRef = string.Format("/Administration/InformationSystems/EditInformationSystems.aspx?SystemId={0}", currentSystemId + 1);
        }

        #endregion
    }
}