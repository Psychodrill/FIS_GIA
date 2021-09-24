using GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders.Base;

namespace GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders
{
    /// <summary>
    /// Загрузка данных справочника "Тип вступительных испытаний" (код 11)
    /// </summary>
    public class EntranceTestTypeDictionaryDataLoader : DictionaryItemDtoDataLoader
    {
        private const string Query = @"select EntranceTestTypeID, Name from [dbo].[EntranceTestType] where EntranceTestTypeID in (1, 2, 3) order by Name";

        protected override string GetQueryText()
        {
            return Query;
        }
    }
}