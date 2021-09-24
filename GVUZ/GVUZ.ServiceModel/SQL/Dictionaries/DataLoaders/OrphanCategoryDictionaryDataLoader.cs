using GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders.Base;

namespace GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders
{
    /// <summary>
    /// Загрузка данных справочника 42 - Тип документа, подтверждающего сиротство (OrphanCategory)
    /// </summary>
    public class OrphanCategoryDictionaryDataLoader : DictionaryItemDtoDataLoader
    {
        private const string Query = @"SELECT [OrphanCategoryID], [Name] FROM [OrphanCategory] ORDER BY 1";

        protected override string GetQueryText()
        {
            return Query;
        }
    }
}
