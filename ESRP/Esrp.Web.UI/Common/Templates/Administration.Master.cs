namespace Esrp.Web.Common.Templates
{
    using System;
    using System.Web;
    using Esrp.Core.Systems;
    using Esrp.Web.Administration.IPCheck;
    using Esrp.Web.Controls;
    using Esrp.Utility;
    /// <summary>
    /// The administration.
    /// </summary>
    public partial class Administration : BaseMasterPage
    {
        
        #region Methods

        /// <summary>
        /// ОБработчик события "Загрузка страницы"
        /// </summary>
        /// <param name="sender"> Источник события </param>
        /// <param name="e"> Параметры события </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SiteMap.CurrentNode.IsIpCheckDisabled())
            {
                if (!IPChecker.IsAdminIP_InAllowedRage(this.Request.UserHostAddress))          
                {
                    this.Response.Redirect("~/Administration/IPCheck/IPNotValid.aspx");
                }
            }
            if (!GeneralSystemManager.IsUserActivated(this.User.Identity.Name))
            {
                this.Response.Redirect(RedirectManager.DefaultRedirectUrl);
            }
        }

        /// <summary>
        /// Контрол вывода сообщений.
        /// </summary>
        public MessageControl MessageControl
        {
            get { return this.messageControl; }
        }

        #endregion
    }
}