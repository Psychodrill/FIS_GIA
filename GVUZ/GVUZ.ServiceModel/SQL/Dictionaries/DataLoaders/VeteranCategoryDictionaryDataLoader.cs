using GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders.Base;

namespace GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders
{
    /// <summary>
    /// Загрузка данных справочника 44 - Тип документа, подтверждающего принадлежность к соотечественникам ([CompatriotCategory])
    /// </summary>
    public class VeteranCategoryDictionaryDataLoader : DictionaryItemDtoDataLoader
    {
        private const string Query = @"SELECT [VeteranCategoryID], [Name] FROM [VeteranCategory] ORDER BY 1";

        protected override string GetQueryText()
        {
            return Query;
        }
    }
}