using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries
{
    public class OlympicDiplomTypeVoc : VocabularyBase<OlympicDiplomTypeVocDto>
    {
        public OlympicDiplomTypeVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class OlympicDiplomTypeVocDto : VocabularyBaseDto
    {
        public int OlympicDiplomTypeID { get; set; }
        public override int ID
        {
            get { return OlympicDiplomTypeID; }
            set { OlympicDiplomTypeID = value; }
        }
        //public string Name { get; set; }
    }
}
