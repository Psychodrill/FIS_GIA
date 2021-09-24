using System.Collections.Generic;
using GVUZ.ServiceModel.Import.WebService.Dto.Documents.Base;

namespace GVUZ.ServiceModel.Import.WebService.Dto.Documents.Collections
{
    public class BenefitDocumentsDto
    {
        public DisabilityDocumentDto DisabilityDocument;
        public MedicalDocumentDto MedicalDocument;

        public List<ApplicationDocumentDto> GetDocuments()
        {
            var docs = new List<ApplicationDocumentDto>();
            if (DisabilityDocument != null) docs.Add(DisabilityDocument);
            if (MedicalDocument != null) docs.Add(MedicalDocument);
            return docs;
        }
    }
}