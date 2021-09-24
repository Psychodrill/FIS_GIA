using GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders.Base;

namespace GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders
{
    /// <summary>
    /// Загрузка данных справочника "Статус проверки заявлений" (код 12)
    /// </summary>
    public class ApplicationCheckStatusDictionaryDataLoader : DictionaryItemDtoDataLoader
    {
        private const string Query = @"select StatusID, Name from [dbo].[ApplicationCheckStatus] order by StatusID";

        protected override string GetQueryText()
        {
            return Query;
        }
    }
}