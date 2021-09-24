using System.Data;
namespace GVUZ.ImportService2016.Core.Main.Dictionaries
{
    public class OrphanCategoryVoc : VocabularyBase<VocabularyBaseDto>
    {
        public OrphanCategoryVoc(DataTable dataTable) : base(dataTable) { }
    }
}
