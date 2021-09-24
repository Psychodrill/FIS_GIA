using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries.Direction
{
    public class DirectionSubjectLinkSubjectVoc : VocabularyBase<DirectionSubjectLinkSubjectVocDto>
    {
        public DirectionSubjectLinkSubjectVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class DirectionSubjectLinkSubjectVocDto : VocabularyBaseDto
    {
        public int SubjectID { get; set; }
        public int LinkID { get; set; }
        public int? ProfileSubjectID { get; set; }
    }
}
