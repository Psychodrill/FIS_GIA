using System.Data;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries.Olympic
{
    public class OlympicLevelVoc : VocabularyBase<OlympicLevelVocDto>
    {
        public OlympicLevelVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class OlympicLevelVocDto : VocabularyBaseDto
    {
    }
}
