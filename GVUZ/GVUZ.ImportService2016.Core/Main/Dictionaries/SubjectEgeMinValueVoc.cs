using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries
{
    public class SubjectEgeMinValueVoc : VocabularyBase<SubjectEgeMinValueVocDto>
    {
        public SubjectEgeMinValueVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class SubjectEgeMinValueVocDto : VocabularyBaseDto
    {
        public int ScoreID {get; set;}
        public int SubjectID { get; set; }
        public int MinValue { get; set; }
    }
}
