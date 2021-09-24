using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries
{
    public class OlympicTypeVoc : VocabularyBase<OlympicTypeVocDto>
    {
        public OlympicTypeVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class OlympicTypeVocDto : VocabularyBaseDto
    {
        public int OlympicID { get; set; }
        public override int ID
        {
            get { return OlympicID; }
            set { OlympicID = value; }
        }
        
        //public int OlympicLevelID{ get; set; }
        //public string OrganizerName{ get; set; }
        public int OlympicNumber{ get; set; }
        public int OlympicYear { get; set; }
    }
}
