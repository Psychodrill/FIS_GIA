using System.ComponentModel;
using GVUZ.Model.Entrants.Documents;
using GVUZ.ServiceModel.Import.WebService.Dto.Documents.Base;

namespace GVUZ.ServiceModel.Import.WebService.Dto.Documents
{
    [Description("Иной документ об образовании")]
    public class EduCustomDocumentDto : ApplicationDocumentDto
    {
        public override EntrantDocumentType EntrantDocumentType
        {
            get { return EntrantDocumentType.EduCustomDocument; }
        }
    }
}