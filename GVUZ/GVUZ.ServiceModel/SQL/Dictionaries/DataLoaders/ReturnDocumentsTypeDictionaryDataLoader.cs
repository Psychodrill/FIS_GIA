using GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders.Base;

namespace GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders
{
    /// <summary>
    /// Загрузка данных справочника 49 - Способ возврата документов (ReturnDocumentsTypeId)
    /// </summary>
    public class ReturnDocumentsTypeDictionaryDataLoader : DictionaryItemDtoDataLoader
    {
        private const string Query = @"SELECT [ApplicationReturnDocumentsTypeId], [Name] FROM [ApplicationReturnDocumentsType] ORDER BY 1";

        protected override string GetQueryText()
        {
            return Query;
        }
    }
}