using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries
{
    public class OlympicTypeSubjectLinkVoc : VocabularyBase<OlympicTypeSubjectLinkVocDto>
    {
        public OlympicTypeSubjectLinkVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class OlympicTypeSubjectLinkVocDto : VocabularyBaseDto
    {
        public int OlympicID { get; set; }
        public int SubjectID { get; set; }
        public int SubjectLevelID { get; set; }
    }
}
