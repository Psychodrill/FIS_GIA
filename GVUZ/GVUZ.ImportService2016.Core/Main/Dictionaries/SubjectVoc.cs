using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries
{
    public class SubjectVoc : VocabularyBase<SubjectVocDto>
    {
        public SubjectVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class SubjectVocDto : VocabularyBaseDto
    {
        public int SubjectID { get; set; }
        public override int ID
        {
            get { return SubjectID; }
            set { SubjectID = value; }
        }
        public bool IsEge { get; set; }
        public bool IsOlympic { get; set; }
    }
}
