using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fbs.Core.Organizations;
using Fbs.Core.Users;
using Fbs.Core;
using System.Configuration;

namespace Fbs.Web.Certificates.CommonNationalCertificates
{
    public partial class PrintNotFoundNote : System.Web.UI.Page
    {
        /// <summary>
        /// Gets or sets OrganizationName.
        /// </summary>
        public string OrganizationName { get; set; }

        protected override void OnInit(EventArgs e)
        {
            if (!Convert.ToBoolean(ConfigurationManager.AppSettings["EnableNotFoundNote"]))
            {
                throw new HttpException(404, "Нет такой страницы");
            }
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                Dictionary<String, String> values = this.Session["NoteInfo"] as Dictionary<String,String>;
                if(values==null)
                    throw new HttpException(404, "Нет такой страницы");
                // Загружаем организацию
                this.OrganizationName = this.GetOrganizationName();
                try
                {
                    Table t = new Table();
                    t.Style.Add(HtmlTextWriterStyle.Width, "100%");
                    if(values.ContainsKey("CertNumber"))
                    {
                        TableRow tr = new TableRow();
                        tr.Cells.Add(new TableCell { Text = String.Format("Номер свидетельства о результатах ЕГЭ: {0}", values["CertNumber"]) });
                        t.Rows.Add(tr);

                    }
                    if (values.ContainsKey("LastName") && !Config.IsOpenFbs)
                    {
                        TableRow tr = new TableRow();
                        tr.Cells.Add(new TableCell { Text = String.Format("Фамилия участника ЕГЭ: {0}", values["LastName"]) });
                        t.Rows.Add(tr);
                    }
                    if (values.ContainsKey("FirstName") && !Config.IsOpenFbs)
                    {
                        TableRow tr = new TableRow();
                        tr.Cells.Add(new TableCell { Text = String.Format("Имя участника  ЕГЭ: {0}", values["FirstName"]) });
                        t.Rows.Add(tr);
                    }
                    if (values.ContainsKey("GivenName") && !Config.IsOpenFbs)
                    {
                        TableRow tr = new TableRow();
                        tr.Cells.Add(new TableCell { Text = String.Format("Отчество участника  ЕГЭ: {0}", values["GivenName"]) });
                        t.Rows.Add(tr);
                    }

                    if (values.ContainsKey("Year"))
                    {
                        TableRow tr = new TableRow();
                        tr.Cells.Add(new TableCell { Text = String.Format("Год: {0}", values["Year"]) });
                        t.Rows.Add(tr);
                    }

                    if (values.ContainsKey("TypographicNumber"))
                    {
                        TableRow tr = new TableRow();
                        tr.Cells.Add(new TableCell { Text = String.Format("Типографский номер: {0}", values["TypographicNumber"]) });
                        t.Rows.Add(tr);
                    }
                    if (values.ContainsKey("Series"))
                    {
                        TableRow tr = new TableRow();
                        tr.Cells.Add(new TableCell { Text = String.Format("Серия документа удостоверяющего личность: {0}", values["Series"]) });
                        t.Rows.Add(tr);
                    }
                    if (values.ContainsKey("PassportNumber"))
                    {
                        TableRow tr = new TableRow();
                        tr.Cells.Add(new TableCell { Text = String.Format("Номер документа удостоверяющего личность: {0}", values["PassportNumber"]) });
                        t.Rows.Add(tr);

                    }
                    if(values.ContainsKey("SubjectMarks"))
                        foreach (string subjectMarks in values["SubjectMarks"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            TableRow tr = new TableRow();
                            string[] marks = subjectMarks.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                            Subject s = Subject.GetSubjectById(int.Parse(marks[0]));
                            tr.Cells.Add(new TableCell { Text = String.Format("Баллы по {0}: {1}", s.Name, marks[1]) });
                            t.Rows.Add(tr);

                        }
                    this.requestPanel.Controls.Add(t);
                }

                catch (Exception ex)
                {
                    throw new HttpException(404, ex.Message);
                }
            }
        }

        private string GetOrganizationName()
        {
            int orgId=0;
            Organization org=null;
            if ( this.Session["OrgId"] != null && !String.IsNullOrEmpty(this.Session["OrgId"].ToString()) && Int32.TryParse(this.Session["OrgId"].ToString(), out orgId))
            {
                org = OrganizationDataAccessor.Get(orgId);
            }
            else
                org = OrgUserDataAccessor.Get(HttpContext.Current.User.Identity.Name).RequestedOrganization;

            if (org != null)
            {
                return org.FullName;
            }

            return null;
        }

    }
}