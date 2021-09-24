using System.Collections.Generic;
using System.ComponentModel;
using GVUZ.ServiceModel.Import.WebService.Dto.Documents.Base;

namespace GVUZ.ServiceModel.Import.WebService.Dto.Documents.Collections
{
    [Description("Документы-основания для оценки")]
    public class EntranceTestResultDocumentsDto
    {
        public string EgeDocumentID;
        public EntranceTestInstitutionDocumentDto InstitutionDocument;
        public OlympicDocumentDto OlympicDocument;
        public OlympicTotalDocumentDto OlympicTotalDocument;

        public List<ApplicationDocumentDto> GetDocuments()
        {
            return new List<ApplicationDocumentDto>
                {
                    OlympicDocument,
                    OlympicTotalDocument
                };
        }
    }
}