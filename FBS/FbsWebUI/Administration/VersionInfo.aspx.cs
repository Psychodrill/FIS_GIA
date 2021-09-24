namespace Fbs.Web.Administration
{
    using System;
    using System.Reflection;
    using System.Web.UI;

    /// <summary>
    /// Страница с версией файла
    /// </summary>
    public partial class VersionInfo : Page
    {
        #region Methods

        /// <summary> Обработчик события "Загрузка страницы" </summary>
        /// <param name="sender"> Источник события </param>
        /// <param name="e"> Параметры события </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.version.InnerText = string.Format("v. {0}", Assembly.GetExecutingAssembly().GetName().Version);
        }

        #endregion
    }
}