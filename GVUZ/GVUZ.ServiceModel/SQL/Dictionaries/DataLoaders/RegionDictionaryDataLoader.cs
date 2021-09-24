using GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders.Base;

namespace GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders
{
    /// <summary>
    /// Загрузка данных справочника "Регион" (код 8)
    /// </summary>
    public class RegionDictionaryDataLoader : DictionaryItemDtoDataLoader
    {
        private const string Query = @"select RegionID, Name from [dbo].[RegionType] order by Name";

        protected override string GetQueryText()
        {
            return Query;
        }
    }
}