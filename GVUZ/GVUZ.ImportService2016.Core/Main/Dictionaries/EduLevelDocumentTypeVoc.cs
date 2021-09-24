using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries
{
    public class EduLevelDocumentTypeVoc : VocabularyBase<EduLevelDocumentTypeVocDto>
    {
        public EduLevelDocumentTypeVoc(DataTable dataTable) : base(dataTable) { }

    }

    public class EduLevelDocumentTypeVocDto : VocabularyBaseDto
    {
        public int AdmissionItemTypeID { get; set; }
        public int DocumentTypeId { get; set; }

    }
}
