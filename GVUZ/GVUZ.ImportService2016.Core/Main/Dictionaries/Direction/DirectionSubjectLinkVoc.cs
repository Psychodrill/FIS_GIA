using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries.Direction
{
    public class DirectionSubjectLinkVoc : VocabularyBase<DirectionSubjectLinkVocDto>
    {
        public DirectionSubjectLinkVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class DirectionSubjectLinkVocDto : VocabularyBaseDto
    {
        public int? ProfileSubjectID { get; set; }
    }
}
