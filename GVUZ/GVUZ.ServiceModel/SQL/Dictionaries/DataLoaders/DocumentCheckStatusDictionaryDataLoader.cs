using GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders.Base;

namespace GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders
{
    /// <summary>
    /// Загрузка данных справочника "Статус проверки документа" (код 13)
    /// </summary>
    public class DocumentCheckStatusDictionaryDataLoader : DictionaryItemDtoDataLoader
    {
        private const string Query = @"select StatusID, Name from [dbo].[DocumentCheckStatus] order by StatusID";

        protected override string GetQueryText()
        {
            return Query;
        }
    }
}