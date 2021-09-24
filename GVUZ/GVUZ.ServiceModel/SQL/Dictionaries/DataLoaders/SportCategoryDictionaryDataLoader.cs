using GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders.Base;

namespace GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders
{
    /// <summary>
    /// Загрузка данных справочника 43 - Тип диплома в области спорта (значения из таблицы DocumentType, CategoryID = 7)
    /// </summary>
    public class SportCategoryDictionaryDataLoader : DictionaryItemDtoDataLoader
    {
        private const string Query = @"SELECT DocumentID, Name FROM [DocumentType] where categoryID= 7 ORDER BY 1";

        protected override string GetQueryText()
        {
            return Query;
        }
    }
}
