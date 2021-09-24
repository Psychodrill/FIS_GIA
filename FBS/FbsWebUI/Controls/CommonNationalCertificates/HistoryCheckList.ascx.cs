namespace Fbs.Web.Controls.CommonNationalCertificates
{
    using System;
    using System.Data;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using FbsServices;
    using Fbs.Core.Organizations;

    /// <summary>
    /// The history check list.
    /// </summary>
    public partial class HistoryCheckList : UserControl
    {
        #region Properties

        /// <summary>
        /// Тип проверки
        /// </summary>
        public CheckType Type { get; set; }

        #endregion

        /// <summary>
        /// Обработчик события OnSelecting для ObjectDataSource
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        protected void СheckHistoryDataSourceOnSelecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            this.SetParameters(e, false);
        }

        /// <summary>
        /// Обработчик события OnSelecting для ObjectDataSource
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        protected void СheckHistoryCountDataSourceOnSelecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            this.SetParameters(e, true);
        }

        protected void SetParameters(ObjectDataSourceSelectingEventArgs args, bool isCount)
        {
            args.InputParameters["login"] = HttpContext.Current != null ? HttpContext.Current.User.Identity.Name : string.Empty;
            args.InputParameters["type"] = this.Type;

            if (!isCount)
            {
                args.InputParameters["startRowIndex"] = this.Page.Request.QueryString["start"] != null ? Convert.ToInt32(this.Page.Request.QueryString["start"]) : 0;
                args.InputParameters["maxRowCount"] = this.Request.Cookies.Get("count") != null ? Convert.ToInt32(this.Request.Cookies.Get("count").Value) : 10;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.dgUserList.ItemCommand += new DataGridCommandEventHandler(dgUserList_ItemCommand);
            base.OnInit(e);
        }

        void dgUserList_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "RedirectToNote")
            {
                string login = e.CommandArgument.ToString().Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries)[0];
                Organization orgByLogin = OrganizationDataAccessor.GetByLogin(login);
                if (orgByLogin != null)
                {
                    this.Session["OrgId"] = orgByLogin.Id;
                }

                this.Response.Redirect(e.CommandArgument.ToString().Replace(login+",",""));
            }
        }
    }
}