using GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders.Base;

namespace GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders
{
    /// <summary>
    /// Загрузка данных справочника "Статус заявления" (код 4)
    /// </summary>
    public class ApplicationStatusDictionaryDataLoader : DictionaryItemDtoDataLoader
    {
        private const string Query = @"select StatusID, Name from [dbo].[ApplicationStatusType] order by Name";

        protected override string GetQueryText()
        {
            return Query;
        }
    }
}