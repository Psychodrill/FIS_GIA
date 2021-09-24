using System.Data;
namespace GVUZ.ImportService2016.Core.Main.Dictionaries.Olympic
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
    }
}
