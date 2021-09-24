using System.Data;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries.Olympic
{

    public class OlympicTypeProfileVoc : VocabularyBase<OlympicTypeProfileVocDto>
    {
        public OlympicTypeProfileVoc(DataTable dataTable)
            : base(dataTable)
        {
        }
    }

    public class OlympicTypeProfileVocDto : VocabularyBaseDto
    {
        public int OlympicTypeID { get; set; }
        public int OlympicLevelID { get; set; }
        public int OlympicProfileID { get; set; }
        public int SubjectID { get; set; }
    }
}
