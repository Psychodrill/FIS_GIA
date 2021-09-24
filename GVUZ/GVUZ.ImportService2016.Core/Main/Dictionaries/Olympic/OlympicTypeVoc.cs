using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries.Olympic
{
    public class OlympicTypeVoc : VocabularyBase<OlympicTypeVocDto>
    {
        public OlympicTypeVoc(DataTable dataTable) : base(dataTable) { }


        public const int Min_Olympic_Level = 1;
        public const int Max_Olympic_Level = 7;
        public const int All_Olympic_Level = 255;

        public const int Min_Olympic_Class = 1;
        public const int Max_Olympic_Class = 31;
        public const int All_Olympic_Class = 255;
        public const int All_Olympic_Class_Vsosh = 28; // ВсОШ = только 9-11 классы!

        public const int All_Olympic_Profile = 255;

        public const int Vsosh_Olympic_Number = -1;
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

        [XmlIgnore()]
        public bool IsVshosh
        {
            get { return OlympicNumber == OlympicTypeVoc.Vsosh_Olympic_Number; }
        }
    }
}
