using System.Data;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries.Olympic
{
    public class OlympicProfileVoc : VocabularyBase<OlympicProfileVocDto>
    {
        public OlympicProfileVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class OlympicProfileVocDto : VocabularyBaseDto
    {
        //public int OlympicProfileID { get; set; }
        //public override int ID
        //{
        //    get { return OlympicProfileID; }
        //    set { OlympicProfileID = value; }
        //}
        ////public string Name { get; set; }
    }
}
