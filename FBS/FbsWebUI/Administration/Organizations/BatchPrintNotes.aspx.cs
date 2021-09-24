using System;
using System.Text;
using System.Web.UI;
using Fbs.Core.Organizations;
using Fbs.Utility;
using Fbs.Web.Certificates.CommonNationalCertificates;
using Fbs.Web.Controls.CommonNationalCertificates;
using FbsServices;

namespace Fbs.Web.Administration.Organizations
{
    public partial class BatchPrintNotes : Page
    {
        private bool _organizationFetched;
        private bool? _wordFormat;
        private bool _firstPageBreak = true;

        private Organization _organization;
        private HtmlTextWriter _html;

        private bool WordFormat
        {
            get
            {
                if (!_wordFormat.HasValue)
                {
                    int word;
                    _wordFormat = Int32.TryParse(Request.QueryString["word"], out word) && word == 1;
                }

                return _wordFormat.Value;
            }
        }

        private long OrganizationId
        {
            get
            {
                long id;
                if (Int64.TryParse(Request.QueryString["OrgId"], out id))
                {
                    return id;
                }

                return 0;
            }
        }

        private int Skip
        {
            get
            {
                return Request.QueryString["skip"] != null ? Convert.ToInt32(Request.QueryString["skip"]) : 0;
            }
        }

        private int Take
        {
            get
            {
                return Request.QueryString["take"] != null ? Convert.ToInt32(Request.QueryString["take"]) : 10;
            }
        }

        private bool UniqueOnly
        {
            get
            {
                return Request.QueryString["unique"] != null && Convert.ToInt32(Request.QueryString["unique"]) == 1;
            }
        }

        private string GetOrganizationName()
        {
            if (!_organizationFetched)
            {
                _organization = OrganizationDataAccessor.Get(OrganizationId);
                _organizationFetched = true;
            }

            return _organization == null ? null : _organization.FullName;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (WordFormat)
            {
                ResponseWriter.PrepareHeaders("OrganizationPrintNotes.doc", "application/msword", Encoding.UTF8);
            }
            else
            {
                Response.ContentEncoding = Encoding.UTF8;
                Response.ContentType = "text/html; charset=utf-8";    
            }

            Response.Flush();
            
            Response.BufferOutput = false;

            _html = new HtmlTextWriter(Response.Output);
            _html.WriteBeginTag("html");
            _html.Write(Html32TextWriter.TagRightChar);
            _html.WriteBeginTag("head");
            _html.Write(Html32TextWriter.TagRightChar);
            _html.WriteBeginTag("meta");
            _html.WriteAttribute("http-equiv", "Content-type");
            _html.WriteAttribute("content", "text/html; charset=utf-8");
            _html.Write(Html32TextWriter.TagRightChar);
            _html.WriteEndTag("head");
            Response.Flush();

            _html.WriteBeginTag("body");
            _html.Write(Html32TextWriter.TagRightChar);

            var printService = new BatchPrintService(OrganizationId, Take);
            printService.PrintNote += OnPrintNoteDataFetched;
            printService.RunPage(UniqueOnly, Skip);
            printService.PrintNote -= OnPrintNoteDataFetched;

            _html.WriteEndTag("body");
            _html.WriteEndTag("html");
            
            _html.Flush();
            _html.Dispose();
        }

        private PrintNoteTemplate _template;

        private PrintNoteTemplate GetTemplate()
        {
            return _template ?? (_template = (PrintNoteTemplate)LoadControl("~/Controls/CommonNationalCertificates/PrintNoteTemplate.ascx"));
        }

        private void OnPrintNoteDataFetched(object sender, PrintNoteEventArgs printNoteEventArgs)
        {
            PrintNoteTemplate ctl = GetTemplate();
            ctl.Cert = PrintNoteData.Parse(printNoteEventArgs.PrintNoteDataSource);
            ctl.OrganizationName = GetOrganizationName();
            ctl.InitializeAsUserControl(this);
            
            WritePageBreak();
            ctl.RenderControl(_html);
        }

        private void WritePageBreak()
        {
            if (_firstPageBreak)
            {
                _firstPageBreak = false;
                return;
            }

            _html.WriteBeginTag("br");
            _html.WriteAttribute("style", "mso-special-character:line-break;page-break-after:always");
            _html.Write(Html32TextWriter.TagRightChar);
        }
    }
}