using System.Data;
namespace GVUZ.ImportService2016.Core.Main.Dictionaries
{
    public class VeteranCategoryVoc : VocabularyBase<VocabularyBaseDto>
    {
        public VeteranCategoryVoc(DataTable dataTable) : base(dataTable) { }
    }
}