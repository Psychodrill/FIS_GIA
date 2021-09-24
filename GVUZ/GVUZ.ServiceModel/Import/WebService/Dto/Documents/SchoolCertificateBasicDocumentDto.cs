using System.ComponentModel;
using GVUZ.Model.Entrants.Documents;
using GVUZ.ServiceModel.Import.WebService.Dto.Documents.Base;

namespace GVUZ.ServiceModel.Import.WebService.Dto.Documents
{
    [Description("Аттестат об основном общем образовании")]
    public class SchoolCertificateBasicDocumentDto : ApplicationDocumentDto
    {
        public override EntrantDocumentType EntrantDocumentType
        {
            get { return EntrantDocumentType.SchoolCertificateBasicDocument; }
        }
    }
}