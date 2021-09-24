using Fbs.Web.Certificates.CommonNationalCertificates;

namespace Fbs.Web.Controls.CommonNationalCertificates
{
    public partial class PrintNoteTemplate : System.Web.UI.UserControl
    {
        public PrintNoteData Cert { get; set; }
        public string OrganizationName { get; set; }
    }
}