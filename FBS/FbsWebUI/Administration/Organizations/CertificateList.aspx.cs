namespace Fbs.Web.Administration.Organizations
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.UI.WebControls;

    using Fbs.Core;
    using Fbs.Core.CNEChecks;
    using Fbs.Core.Organizations;
    using Fbs.Utility;
    using Fbs.Web.Common.Templates;

    using FbsServices;

    using FbsWebViewModel.CNEC;

    /// <summary>
    ///     История проверок свидетельств организацией
    /// </summary>
    public partial class CertificateList : BasePage
    {
        #region Constants

        /// <summary>
        ///     Контентный тип для документа
        /// </summary>
        private const string FileContentType = "application/msword";

        #endregion

        #region Fields

        private readonly CNECService cnecService = new CNECService();

        private int? orgId;

        private string uniqueIdentifier;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the note id.
        /// </summary>
        public Guid NoteId { get; set; }

        /// <summary>
        ///     Значение чекбокса "Уникальные проверки"
        /// </summary>
        public string cb { get; set; }

        #endregion

        #region Properties

        /// <summary>
        ///     id организации
        /// </summary>
        protected int OrgId
        {
            get
            {
                if (this.orgId == null)
                {
                    if (string.IsNullOrEmpty(this.Request.QueryString["OrgId"]))
                    {
                        throw new ArgumentException("OrgId");
                    }

                    this.orgId = int.Parse(this.Request.QueryString["OrgId"]);
                }

                return this.orgId.Value;
            }
        }

        private Organization Organization { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Обработчик события нажатия на кнопку "Распечатать"
        /// </summary>
        /// <param name="sender">
        /// Источник события
        /// </param>
        /// <param name="e">
        /// Параметры события
        /// </param>
        protected void BtnPrintAllNoteClick(object sender, EventArgs e)
        {
            List<HistoryCheckCertificateForOrganizationView> historyList = Utility.GetHistoryListToSession();
            var notesList = new List<CNEInfo>();
            foreach (HistoryCheckCertificateForOrganizationView item in historyList)
            {
                if (string.IsNullOrEmpty(item.PrintNote.Text) || item.Status == "Не найдено")
                {
                    continue;
                }

                DataTable table = this.cnecService.GetDataForPrint(
                    this.User.Identity.Name, 
                    HttpContext.Current.Request.UserHostAddress, 
                    item.Certificate.Text, 
                    item.FirstName, 
                    item.LastName, 
                    item.PatronymicName);
                if (table.Rows.Count > 0)
                {
                    var certificates = table.ToCertificatesCollection(!Config.IsOpenFbs);
                    if (certificates.Count > 0)
                        notesList.Add(certificates[0]);
                }
            }

            const string FileName = @"Справки.doc";

            try
            {
                this.uniqueIdentifier = Guid.NewGuid().ToString();
                string virPath = IOUtility.CheckAndCreatePhisicalFolder(
                    Config.CNEPrintFolder, this.Server.MapPath("~/"));
                string docName = PrintNotesTemplate.GetDocument(notesList, virPath, this.uniqueIdentifier, this.OrgId);
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["EnableNotFoundNote"]))
                {
                    docName = PrintNotesTemplate.AddNotFoundNotes(
                        virPath, 
                        historyList.Where(x => x.Status == "Не найдено")
                                   .Select(
                                       x =>
                                       new CNEInfo
                                           {
                                               CertificateNumber = x.Certificate.Text, 
                                               FirstName = !Config.IsOpenFbs ? x.FirstName : string.Empty, 
                                               LastName = !Config.IsOpenFbs ? x.LastName : string.Empty, 
                                               PatronymicName = !Config.IsOpenFbs ? x.PatronymicName : string.Empty, 
                                               TypographicNumber = x.TypographicNumber, 
                                               Year = x.Year ?? string.Empty, 
                                               Marks = x.Marks == null ? new MarkList() : MarkList.FromMarksString(x.Marks), 
                                               PassportSeria =
                                                   x.Document != null
                                                   && x.Document.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                                       .Length > 1
                                                       ? x.Document.Split(
                                                           new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0]
                                                       : null, 
                                               PassportNumber =
                                                   x.Document != null
                                                   && x.Document.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                                       .Length > 1
                                                       ? x.Document.Split(
                                                           new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1]
                                                       : x.Document
                                           }),
                        this.uniqueIdentifier,
                        this.OrgId);
                }

                // Отдам документ
                ResponseWriter.WriteFile(FileName, FileContentType, docName);

                File.Delete(docName);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// События происходящее до вызова Select
        /// </summary>
        /// <param name="sender">
        /// Источник события
        /// </param>
        /// <param name="e">
        /// Параметры события
        /// </param>
        protected void CountCNECCheckHystoryByOrgIdOnSelecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["typeCheck"] = string.IsNullOrEmpty(this.Request.QueryString["typeCheck"])
                                                 ? null
                                                 : this.Request.QueryString["typeCheck"];

            e.InputParameters["dats"] = string.IsNullOrEmpty(this.Request.QueryString["dateS"])
                                                 ? null
                                                 : this.Request.QueryString["dateS"];

            e.InputParameters["datf"] = string.IsNullOrEmpty(this.Request.QueryString["dateF"])
                                                 ? null
                                                 : this.Request.QueryString["dateF"];

            e.InputParameters["family"] = Config.IsOpenFbs || string.IsNullOrEmpty(this.Request.QueryString["family"])
                                              ? null
                                              : this.Request.QueryString["family"];
            e.InputParameters["orgId"] = this.OrgId;
            e.InputParameters["isUniqueCheck"] = !string.IsNullOrEmpty(this.Request.QueryString["cbisUniqueCheck"]);
        }

        /// <summary>
        /// Обработчик события, возникающий после вызова метода Select
        /// </summary>
        /// <param name="sender">
        /// Источник события
        /// </param>
        /// <param name="e">
        /// Параметры события
        /// </param>
        protected void HistoryDataSourceOnSelected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            var result = (List<HistoryCheckCertificateForOrganizationView>)e.ReturnValue;

            // Записываю в сессию выбранные записи
            Utility.SaveHistoryListToSession(result);

            // Если записей 0, то скрываем чекбокс и кнопку, распечатать
            if (result == null || result.Count() == 0)
            {
                this.btnPrintAllNote.Visible = false;
            }
        }

        /// <summary>
        /// Обработчик события OnSelecting
        /// </summary>
        /// <param name="sender">
        /// Источник события
        /// </param>
        /// <param name="e">
        /// Параметры события
        /// </param>
        protected void HistoryDataSourceOnSelecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["startRow"] = this.Page.Request.QueryString["start"] != null
                                                ? Convert.ToInt32(this.Page.Request.QueryString["start"])
                                                : 0;
            int maxRow = Convert.ToInt32(e.InputParameters["startRow"])
                         + (this.Request.Cookies.Get("count") != null
                                ? Convert.ToInt32(this.Request.Cookies.Get("count").Value)
                                : 10);
            e.InputParameters["maxRow"] = maxRow;

            e.InputParameters["typeCheck"] = string.IsNullOrEmpty(this.Request.QueryString["typeCheck"])
                                                 ? null
                                                 : this.Request.QueryString["typeCheck"];

            e.InputParameters["dats"] = string.IsNullOrEmpty(this.Request.QueryString["dateS"])
                                                 ? null
                                                 : this.Request.QueryString["dateS"];

            e.InputParameters["datf"] = string.IsNullOrEmpty(this.Request.QueryString["dateF"])
                                                 ? null
                                                 : this.Request.QueryString["dateF"];

            e.InputParameters["family"] = Config.IsOpenFbs || string.IsNullOrEmpty(this.Request.QueryString["family"])
                                              ? null
                                              : this.Request.QueryString["family"];
            e.InputParameters["orgId"] = this.OrgId;
            e.InputParameters["sortorder"] = string.IsNullOrEmpty(this.Request.QueryString["sortorder"])
                                                 ? 1
                                                 : int.Parse(this.Request.QueryString["sortorder"]);
            e.InputParameters["sortBy"] = string.IsNullOrEmpty(this.Request.QueryString["sort"])
                                              ? "id"
                                              : this.Request.QueryString["sort"];
            e.InputParameters["isUniqueCheck"] = !string.IsNullOrEmpty(this.Request.QueryString["cbisUniqueCheck"]);
        }

        /// <summary>
        /// Обработчик события "Загрузка страницы"
        /// </summary>
        /// <param name="sender">
        /// Источник события
        /// </param>
        /// <param name="e">
        /// Параметры события
        /// </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.cb = string.IsNullOrEmpty(this.Request.QueryString["cbisUniqueCheck"]) ? "false" : "true";

            if (this.Page.IsPostBack)
            {
                return;
            }

            // Запрашиваем данные по организации чтобы узнать ее имя
            Organization org = OrganizationDataAccessor.Get(this.OrgId);
            this.Organization = org;
            const string TemplateTitle = "История проверок свидетельств организацией \"{0}\"";
            this.PageTitle = string.Format(TemplateTitle, "Организация не найдена");
            if (org != null)
            {
                if (string.IsNullOrEmpty(org.ShortName))
                {
                    this.PageTitle = string.IsNullOrEmpty(org.FullName)
                                         ? string.Format(TemplateTitle, "Название не найдено")
                                         : string.Format(
                                             TemplateTitle, 
                                             org.FullName.Length > 40 ? org.FullName.Remove(40) : org.FullName);
                }
                else
                {
                    this.PageTitle = string.Format(TemplateTitle, org.ShortName);
                }

                var master = (Administration)this.Master;
                if (master != null)
                {
                    master.CaptionToolTip = string.Format(
                        TemplateTitle, string.IsNullOrEmpty(org.FullName) ? "Название не найдено" : org.FullName);
                }
            }
        }

        /// <summary>
        /// Обработчик события нажатия на ссылку "Распечатать справку"
        /// </summary>
        /// <param name="sender">
        /// Источник события
        /// </param>
        /// <param name="e">
        /// Параметры события
        /// </param>
        protected void Print(object sender, CommandEventArgs e)
        {
            List<HistoryCheckCertificateForOrganizationView> historyList = Utility.GetHistoryListToSession();
            HistoryCheckCertificateForOrganizationView item =
                historyList.FirstOrDefault(x => x.NumberRow == Convert.ToInt32(e.CommandArgument));

            if (item != null)
            {
                if (item.Status == "Не найдено")
                {
                    var values = new Dictionary<string, string>();
                    if (!string.IsNullOrEmpty(item.FirstName))
                    {
                        values.Add("FirstName", item.FirstName);
                    }

                    if (!string.IsNullOrEmpty(item.LastName))
                    {
                        values.Add("LastName", item.LastName);
                    }

                    if (!string.IsNullOrEmpty(item.PatronymicName))
                    {
                        values.Add("GivenName", item.PatronymicName);
                    }

                    if (!string.IsNullOrEmpty(item.TypographicNumber))
                    {
                        values.Add("TypographicNumber", item.TypographicNumber);
                    }

                    if (!string.IsNullOrEmpty(item.Number))
                    {
                        values.Add("CertNumber", item.Number);
                    }

                    if (!string.IsNullOrEmpty(item.Document))
                    {
                        string[] pass = item.Document.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (pass.Length > 1)
                        {
                            values.Add("Series", pass[0]);
                            values.Add("PassportNumber", pass[1]);
                        }
                        else
                        {
                            values.Add("PassportNumber", item.Document);
                        }
                    }

                    if (item.Marks != null)
                    {
                        values.Add("SubjectMarks", item.Marks);
                    }

                    this.Session["OrgId"] = this.OrgId;
                    this.Session["NoteInfo"] = values;
                    this.Response.Redirect("~/Certificates/CommonNationalCertificates/PrintNotFoundNote.aspx", true);
                }

                DataTable table = this.cnecService.GetDataForPrint(
                    this.User.Identity.Name, 
                    HttpContext.Current.Request.UserHostAddress, 
                    item.Certificate.Text, 
                    item.FirstName, 
                    item.LastName, 
                    item.PatronymicName);

                if (table.Rows.Count > 0)
                {
                    var certificates = table.ToCertificatesCollection(!Config.IsOpenFbs);
                    if (certificates.Count > 0)
                        this.SaveCertificateInfo(certificates[0]);

                    this.Session["OrgId"] = this.OrgId;
                    this.Response.Redirect(
                        string.Format("/Certificates/CommonNationalCertificates/PrintNote.aspx?id={0}", this.NoteId));
                }
            }
        }

        /// <summary>
        /// нужно скрыть ФИО в зависимости от системы
        /// </summary>
        /// <param name="sender">
        /// отправитель события
        /// </param>
        /// <param name="e">
        /// аргументы
        /// </param>
        protected void gvChecks_OnDataBound(object sender, EventArgs e)
        {
            if (Config.IsOpenFbs)
            {
                this.gvChecks.Columns[4].Visible = false;
                this.gvChecks.Columns[5].Visible = false;
                this.gvChecks.Columns[6].Visible = false;
            }
        }

        private void SaveCertificateInfo(CNEInfo item)
        {
            Guid key = Guid.NewGuid();
            this.NoteId = key;

            var val = new KeyValuePair<Guid, CNEInfo>(this.NoteId, item);
            this.Session["CertificateInfo"] = val;
        }

        #endregion
    }
}