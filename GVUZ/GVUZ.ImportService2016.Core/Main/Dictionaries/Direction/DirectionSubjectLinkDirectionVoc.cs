using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries.Direction
{
    public class DirectionSubjectLinkDirectionVoc : VocabularyBase<DirectionSubjectLinkDirectionVocDto>
    {
        public DirectionSubjectLinkDirectionVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class DirectionSubjectLinkDirectionVocDto : VocabularyBaseDto
    {
        public int DirectionID { get; set; }
        public int LinkID { get; set; }
        public int? ProfileSubjectID { get; set; }
    }
}
