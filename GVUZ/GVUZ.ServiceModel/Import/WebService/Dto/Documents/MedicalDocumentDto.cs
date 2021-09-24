using System.ComponentModel;
using GVUZ.Model.Entrants.Documents;
using GVUZ.ServiceModel.Import.WebService.Dto.Documents.Base;

namespace GVUZ.ServiceModel.Import.WebService.Dto.Documents
{
    [Description("Заключение психолого-медико-педагогической комиссии")]
    public class MedicalDocumentDto : ApplicationDocumentDto
    {
        public override EntrantDocumentType EntrantDocumentType
        {
            get { return EntrantDocumentType.MedicalDocument; }
        }
    }
}