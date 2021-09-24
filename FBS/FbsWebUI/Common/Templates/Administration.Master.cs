namespace Fbs.Web.Common.Templates
{
    using System;

    using Fbs.Web.Administration.IPCheck;

    /// <summary>
    /// мастер страница для раздела Администрирование
    /// </summary>
    public partial class Administration : BaseMasterPage
    {
        /// <summary>
        /// Тултип для заголовка страницы
        /// </summary>
        public string CaptionToolTip { get; set; }

        #region Methods

        /// <summary>
        /// Обработчик события "загрузка страницы
        /// </summary>
        /// <param name="sender">Параметры события </param>
        /// <param name="e">Источник события </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (this.User.IsInRole("ViewAdministrationSection"))
            //{
            //    if (!IPChecker.IsAdminIP_InAllowedRage(this.Request.UserHostAddress))
            //    {
            //        this.Response.Redirect("~/Administration/IPCheck/IPNotValid.aspx");
            //    }
            //}
            //else if (!IPChecker.IsAdminIP_InAllowedRage(this.Request.UserHostAddress)
            //         &&
            //         !(this.Request.Url.LocalPath.StartsWith("/Administration/Organizations/UserDepartments/")
            //           || this.Request.Url.LocalPath.StartsWith("/Administration/Organizations/Administrators/OrgCard")))
            //{
            //    this.Response.Redirect("~/Administration/IPCheck/IPNotValid.aspx");
            //}

            this.lblTitle.ToolTip = this.CaptionToolTip;
        }

        #endregion
    }
}