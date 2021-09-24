using Fbs.Web.Certificates.CommonNationalCertificates;

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
    public partial class HistoryCheckListCommon : BaseControl
    {
        private Organization _org;
        private bool _orgFetched;

        private Organization GetOrganization()
        {
            if (!_orgFetched)
            {
                _org = OrganizationDataAccessor.GetByLogin(CurrentUserName);
                _orgFetched = true;
            }

            return _org;
        }

        /// <summary>
        /// Отображать историю интерактивных проверок (true - не отображать, false - отображать)
        /// </summary>
        protected bool OrganizationLogDisabled
        {
            get
            {
                // для пакетных проверок история отображается
                if (CheckMode == CertificateCheckMode.Batch)
                {
                    return false;
                }
                // для админов в интерактивных проверках история отображается
                if (IsAdmin)
                {
                    return false;
                }
                // для пользователей не привязанных к организации история не отображается
                if (GetOrganization() == null)
                {
                    return true;
                }
                // для пользователей, привязанных к организации, история отображается
                return GetOrganization().DisableLog;
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            historyContents.Visible = !OrganizationLogDisabled;
            TemplateColumn deleteLinkColumn = dgUserList.Columns[dgUserList.Columns.Count - 1] as TemplateColumn;
            TemplateColumn viewLinkColumn = dgUserList.Columns[dgUserList.Columns.Count - 2] as TemplateColumn;
            
            if (deleteLinkColumn != null && viewLinkColumn != null)
            {
                deleteLinkColumn.Visible = CanDeleteHistory;
                if (deleteLinkColumn.Visible)
                {
                    deleteLinkColumn.HeaderStyle.CssClass = "right-th";
                    viewLinkColumn.HeaderStyle.CssClass = string.Empty;
                }
                else
                {
                    deleteLinkColumn.HeaderStyle.CssClass = string.Empty;
                    viewLinkColumn.HeaderStyle.CssClass = "right-th";
                }
            }
        }

        #region Properties

        /// <summary>
        /// Тип проверки
        /// </summary>
        public CommonCheckType CheckType { get; set; }

        /// <summary>
        /// Вид проверки
        /// </summary>
        public CertificateCheckMode CheckMode { get; set; }

        /// <summary>
        /// Возможность удаления записи из истории проверки (только для интерактивных проверок)
        /// </summary>
        public bool ShowDeleteHistoryLink { get; set; }

        /// <summary>
        /// Разрешать удалять историю проверок
        /// </summary>
        protected bool CanDeleteHistory
        {
            get
            {
                // удаление только в рамках пакетных проверок
                if (CheckMode != CertificateCheckMode.Batch)
                {
                    return false;
                }

                //// админ может удалять
                //if (IsAdmin)
                //{
                //    return true;
                //}

                // пользователь не привязанный к организации не может удалять
                if (GetOrganization() == null)
                {
                    return false;
                }

                // пользователь привязанный к организации может удалять в зависимости от настроек
                return GetOrganization().DisableLog;
            }
        }

        protected string GetResultPageUrl(long id)
        {
            string page = GetResultPage();

            if (page == null)
            {
                return "#";
            }

            return string.Format("/Certificates/CommonNationalCertificates/{0}?id={1}", page, id);
        }

        private string GetResultPage()
        {
            IHistoryNavigator page = Page as IHistoryNavigator;

            if (page != null)
            {
                return page.GetPageName();
            }

            return null;
        }

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
            args.Cancel = !isCount && OrganizationLogDisabled;
            args.InputParameters["login"] = IsAdmin ? null : CurrentUserName;
            args.InputParameters["checkType"] = CheckType;
            args.InputParameters["checkMode"] = CheckMode;

            if (!isCount)
            {
                args.InputParameters["startRowIndex"] = this.Page.Request.QueryString["start"] != null ? Convert.ToInt32(this.Page.Request.QueryString["start"]) : 0;
                args.InputParameters["pageSize"] = this.Request.Cookies.Get("count") != null ? Convert.ToInt32(this.Request.Cookies.Get("count").Value) : 10;
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

        protected void ProcessCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "RemoveHistoryEntry")
            {
                long checkId;

                if (Int64.TryParse(e.CommandArgument.ToString(), out checkId))
                {
                    CertificateCheckHistoryService.DeleteBatchCheckHistoryById(IsAdmin ? null : CurrentUserName, checkId);
                }
            }

            Response.Redirect(Request.RawUrl);
        }

        protected bool RenderDeleteLinkVisible(DataGridItem container)
        {
            return CanDeleteHistory && DataBinder.Eval(container, "DataItem.Status", "{0}").ToLower() != "ожидает проверки";
        }
    }
}