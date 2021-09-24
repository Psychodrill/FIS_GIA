using System.Data;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries
{
    public class TownTypeVoc : VocabularyBase<VocabularyBaseDto>
    {
        public TownTypeVoc(DataTable dataTable) : base(dataTable) { }
    }
}
