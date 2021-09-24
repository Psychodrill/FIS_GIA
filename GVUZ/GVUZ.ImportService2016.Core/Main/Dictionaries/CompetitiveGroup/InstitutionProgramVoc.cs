using System.Data;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries
{
    public class InstitutionProgramVoc : VocabularyBase<InstitutionProgramVocDto>
    {
        public InstitutionProgramVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class InstitutionProgramVocDto : VocabularyBaseDto
    {
        public long ProgramID { get; set; }
        public override int ID
        {
            get { return (int)ProgramID; }
            set { ProgramID = value; }
        }

        //public int CompetitiveGroupID { get; set; }
        public string Code { get; set; }
    }
}
