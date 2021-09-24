using System.Data;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;
using Fbs.Core;
using FbsServices;

namespace Fbs.Web.Certificates.CommonNationalCertificates
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Web;
    using System.Web.UI;
    using Fbs.Core.Organizations;
    using Fbs.Core.Users;

    /// <summary>
    /// Справка о свидетельстве
    /// </summary>
    public partial class PrintNoteBatchCommon : Page
    {
        private CertificateCheckHistoryService _service;

        private string CurrentUserName
        {
            get
            {
                return this.User.Identity.Name;
            }
        }

        private bool IsAdmin
        {
            get
            {
                return this.User.IsInRole("EditAdministratorAccount");
            }
        }

        private long CheckId
        {
            get { return GetParamLong("checkId"); }
        }

        private long GroupId
        {
            get { return GetParamLong("groupId"); }
        }

        private long GetParamLong(string name)
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

        private CertificateCheckHistoryService GetService()
        {
            return _service ?? (_service = new CertificateCheckHistoryService(IsAdmin ? null : CurrentUserName, CheckId));
        }

        #region Public Properties

        protected PrintNoteData Cert { get; set; }

        protected PrintNoteNotFoundData NotFound { get; set; }

        protected string NotFoundHtml { get; set; }

        private string _organizationName;

        public string OrganizationName
        {
            get { return _organizationName ?? (_organizationName = GetOrganizationName() ?? string.Empty); }
        }

        /// <summary>
        /// Настройка web.config 
        /// </summary>
        public bool EnableOpenedFbs
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["EnableOpenedFbs"]);
            }
        }

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
            if (!this.Page.IsPostBack)
            {
                DataTable details = GetService().GetDetails(GroupId);

                if (details != null && details.Rows.Count > 0)
                {
                    Cert = PrintNoteData.Parse(new DataView(details));
                    phNoteFound.Visible = true;
                    phNoteNotFound.Visible = false;
                    phEmpty.Visible = false;
                }
                else
                {
                    DataTable notFoundData = _service.GetNotFoundData();
                    
                    if (notFoundData != null && notFoundData.Rows.Count > 0)
                    {
                        NotFound = PrintNoteNotFoundData.Parse(notFoundData.Rows[0]);

                        Table t = new Table();
                        t.Style.Add(HtmlTextWriterStyle.Width, "100%");
                        if (!string.IsNullOrEmpty(NotFound.CertNumber))
                        {
                            TableRow tr = new TableRow();
                            tr.Cells.Add(new TableCell { Text = String.Format("Номер свидетельства о результатах ЕГЭ: {0}", NotFound.CertNumber) });
                            t.Rows.Add(tr);

                        }
                        if (!string.IsNullOrEmpty(NotFound.LastName) && !Config.IsOpenFbs)
                        {
                            TableRow tr = new TableRow();
                            tr.Cells.Add(new TableCell { Text = String.Format("Фамилия участника ЕГЭ: {0}", NotFound.LastName) });
                            t.Rows.Add(tr);
                        }
                        if (!string.IsNullOrEmpty(NotFound.FirstName) && !Config.IsOpenFbs)
                        {
                            TableRow tr = new TableRow();
                            tr.Cells.Add(new TableCell { Text = String.Format("Имя участника  ЕГЭ: {0}", NotFound.FirstName) });
                            t.Rows.Add(tr);
                        }
                        if (!string.IsNullOrEmpty(NotFound.GivenName) && !Config.IsOpenFbs)
                        {
                            TableRow tr = new TableRow();
                            tr.Cells.Add(new TableCell { Text = String.Format("Отчество участника  ЕГЭ: {0}", NotFound.GivenName) });
                            t.Rows.Add(tr);
                        }
                        if (!string.IsNullOrEmpty(NotFound.TypographicNumber))
                        {
                            TableRow tr = new TableRow();
                            tr.Cells.Add(new TableCell { Text = String.Format("Типографский номер: {0}", NotFound.TypographicNumber) });
                            t.Rows.Add(tr);
                        }
                        if (!string.IsNullOrEmpty(NotFound.Series))
                        {
                            TableRow tr = new TableRow();
                            tr.Cells.Add(new TableCell { Text = String.Format("Серия документа удостоверяющего личность: {0}", NotFound.Series) });
                            t.Rows.Add(tr);
                        }
                        if (!string.IsNullOrEmpty(NotFound.PassportNumber))
                        {
                            TableRow tr = new TableRow();
                            tr.Cells.Add(new TableCell { Text = String.Format("Номер документа удостоверяющего личность: {0}", NotFound.PassportNumber) });
                            t.Rows.Add(tr);

                        }

                        StringBuilder sb = new StringBuilder();
                        using (StringWriter sw = new StringWriter(sb))
                        {
                            using (HtmlTextWriter tw = new HtmlTextWriter(sw))
                            {
                                t.RenderControl(tw);
                            }
                        }

                        this.NotFoundHtml = sb.ToString();

                        phNoteFound.Visible = false;
                        phNoteNotFound.Visible = true;
                        phEmpty.Visible = false;
                    }

                    else
                    {
                        phNoteFound.Visible = false;
                        phNoteNotFound.Visible = false;
                        phEmpty.Visible = true;
                    }
                }
            }
        }

        private string GetOrganizationName()
        {
            Organization org = null;

            long orgId = GetParamLong("OrgId");
            if (orgId > 0)
            {
                org = OrganizationDataAccessor.Get(orgId);
            }
            else
                org = OrgUserDataAccessor.Get(HttpContext.Current.User.Identity.Name).RequestedOrganization;

            if (org != null)
            {
                return org.FullName;
            }

            return "";
        }

        #endregion
    }

    public class PrintNoteNotFoundData
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string GivenName { get; set; }
        public string CertNumber  { get; set; }
        public string PassportNumber { get; set; }
        public string Series { get; set; }
        public string TypographicNumber { get; set; }

        public static PrintNoteNotFoundData Parse(DataRow row)
        {
            var result = new PrintNoteNotFoundData();
            result.CertNumber = row.IsNull("LicenseNumber") ? string.Empty : row.Field<string>("LicenseNumber");
            result.GivenName = row.IsNull("SecondName") ? string.Empty : row.Field<string>("SecondName");
            result.FirstName = row.IsNull("Name") ? string.Empty : row.Field<string>("Name");
            result.LastName = row.IsNull("Surname") ? string.Empty : row.Field<string>("Surname");
            result.PassportNumber = row.IsNull("DocumentNumber") ? string.Empty : row.Field<string>("DocumentNumber");
            result.Series = row.IsNull("DocumentSeries") ? string.Empty : row.Field<string>("DocumentSeries");
            result.TypographicNumber =  row.IsNull("TypographicNumber") ? string.Empty : row.Field<string>("TypographicNumber");

            return result;
        }

    }
}