using System.Data;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries.Olympic
{
    public class OlympicSubjectVoc : VocabularyBase<OlympicSubjectVocDto>
    {
        public OlympicSubjectVoc(DataTable dataTable)
            : base(dataTable)
        {
        }
    }

    public class OlympicSubjectVocDto : VocabularyBaseDto
    {
        public int OlympicSubjectID { get; set; }
        public int OlympicTypeProfileID { get; set; }
        public int SubjectID { get; set; }

        public override int ID { get { return OlympicSubjectID; } set { OlympicSubjectID = value; } }
    }
}
