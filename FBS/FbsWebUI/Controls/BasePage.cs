using System;
using System.Configuration;

namespace Fbs.Web
{
    using System.Web;
    using System.Web.UI;

    /// <summary>
    /// The base page.
    /// </summary>
    public class BasePage : Page
    {
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            var node = SiteMap.CurrentNode;
            if (!node.ShowInOpenedFbs() && Convert.ToBoolean(ConfigurationManager.AppSettings["EnableOpenedFbs"])                                
                || !node.ShowInClosedFbs() && !Convert.ToBoolean(ConfigurationManager.AppSettings["EnableOpenedFbs"]))
            {
                Response.Redirect(string.Format(Resources.Errors.ErrorPageFormat, Resources.Errors.PageAccessDenied));
            }
        }

        #region Public Properties

        /// <summary>
        /// Gets CurrentUrl.
        /// </summary>
        public string CurrentUrl
        {
            get
            {
                return this.Request.Url.AbsoluteUri;
            }
        }

        /// <summary>
        /// Gets CurrentUserIp.
        /// </summary>
        public string CurrentUserIp
        {
            get
            {
                return this.Request.UserHostAddress;
            }
        }

        /// <summary>
        /// Gets CurrentUserName.
        /// </summary>
        public string CurrentUserName
        {
            get
            {
                return this.User.Identity.Name;
            }
        }

        /// <summary>
        /// Gets or sets PageTitle.
        /// </summary>
        public string PageTitle
        {
            get
            {
                return SiteMap.CurrentNode.GetActualTitle();
            }

            set
            {
                SiteMap.CurrentNode.StoreProperties(new SiteMapNodeProperties(this.Request.Url.PathAndQuery, value));
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// получить get параметр типа инт
        /// </summary>
        /// <param name="name">
        /// название параметра
        /// </param>
        /// <returns>
        /// значение параметра
        /// </returns>
        public int GetParamInt(string name)
        {
            if (this.Page.Request.QueryString[name] != null)
            {
                int returnVal;
                if (int.TryParse(this.Page.Request.QueryString[name], out returnVal))
                {
                    return returnVal;
                }
            }

            return 0;
        }

        public long GetParamLong(string name)
        {
            if (this.Page.Request.QueryString[name] != null)
            {
                long returnVal;
                if (long.TryParse(this.Page.Request.QueryString[name], out returnVal))
                {
                    return returnVal;
                }
            }

            return 0;
        }

        protected bool IsAdmin
        {
            get
            {
                return this.User.IsInRole("EditAdministratorAccount");
            }
        }

        /// <summary>
        /// Скрывает узел в левом меню
        /// </summary>
        public void PageHideInLeftMenu()
        {
            SiteMap.CurrentNode.HideNodeInLeftMenu();
        }

        #endregion
    }
}