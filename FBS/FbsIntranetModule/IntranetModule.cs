namespace FbsIntranetModule
{
    using System;
    using System.Configuration;
    using System.Web;

    /// <summary>
    /// Пользовательский модуль
    /// </summary>
    public class IntranetModule : IHttpModule
    {
        /// <summary>
        /// Метод инициализирует модуль и подготавливает его к обработке запроса
        /// </summary>
        /// <param name="app">Объект обслуживающий запрос</param>
        public void Init(HttpApplication app)
        {
            app.BeginRequest += this.OnBeginRequest;
        }

        /// <summary>
        /// Метод освобождает используемые модулем ресурсы (все, кроме памяти!)
        /// </summary>
        public void Dispose()
        {
        }

        #region "Event Handlers / Events"

        /// <summary>
        /// Обработка запроса
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        public void OnBeginRequest(object sender, EventArgs e)
        {
            var app = (HttpApplication)sender;
            var ctx = app.Context;
            var selectedNode = SiteMap.CurrentNode;
            if (selectedNode == null)
            {
                return;
            }

            if (Convert.ToBoolean(ConfigurationManager.AppSettings["EnableOpenedFbs"]))
            {
                // Открытая версия приложения
                if (!ShowInOpenedFbs(selectedNode))
                {
                    //ctx.Response.Redirect("/Страница с ошибкой");
                }
            }
            else
            {
                // Закрытая версия приложения
                if (!ShowInClosedFbs(selectedNode))
                {
                   //ctx.Response.Redirect("/Страница с ошибкой");
                }
            }
        }

        /// <summary>
        /// Нужно ли показывать узел в закрытой версии приложения
        /// </summary>
        /// <param name="node">Узел</param>
        /// <returns>нужно / не нужно</returns>
        private static bool ShowInClosedFbs(SiteMapNode node)
        {
            if (node == null)
            {
                return false;
            }

            var showAttr = node["showinclosedfbs"];
            return string.IsNullOrEmpty(showAttr) || showAttr.ToLower().Trim() == "true";
        }

        /// <summary>
        /// Нужно ли показывать узел в открытой версии приложения
        /// </summary>
        /// <param name="node">Узел</param>
        /// <returns>нужно / не нужно</returns>
        private static bool ShowInOpenedFbs(SiteMapNode node)
        {
            if (node == null)
            {
                return false;
            }

            var showAttr = node["showinopenedfbs"];
            return string.IsNullOrEmpty(showAttr) || showAttr.ToLower().Trim() == "true";
        }
        #endregion
    }
}
