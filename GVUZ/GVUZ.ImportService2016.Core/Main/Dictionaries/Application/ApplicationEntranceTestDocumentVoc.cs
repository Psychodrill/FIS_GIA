using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries.Application
{
    public class ApplicationEntranceTestDocumentVoc : VocabularyBase<ApplicationEntranceTestDocumentVocDto>
    {
        public ApplicationEntranceTestDocumentVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class ApplicationEntranceTestDocumentVocDto : VocabularyBaseDto
    {
        public int ApplicationID { get; set; }
        public int SubjectID { get; set; }
        public int EntrantDocumentID { get; set; }
        public int EntranceTestTypeID { get; set; }
        public int EntranceTestItemID { get; set; }

        public int BenefitID { get; set; }
        public int CompetitiveGroupID { get; set; }
        public string ApplicationUID { get; set; }
    }
}
