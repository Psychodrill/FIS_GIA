using System.Data;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries
{
    public class CompetitiveGroupProgramVoc : VocabularyBase<CompetitiveGroupProgramVocDto>
    {
        public CompetitiveGroupProgramVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class CompetitiveGroupProgramVocDto : VocabularyBaseDto
    {
        public long ProgramID { get; set; }
        public override int ID
        {
            get { return (int)ProgramID; }
            set { ProgramID = value; }
        }

        public int CompetitiveGroupID { get; set; }
        public string CompetitiveGroupUID { get; set; }

        public string Code { get; set; }
    }
}
