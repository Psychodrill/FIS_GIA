using GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders.Base;

namespace GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders
{
    /// <summary>
    /// Загрузка данных справочника "Страна" (код 7)
    /// </summary>
    public class CountryDictionaryDataLoader : DictionaryItemDtoDataLoader
    {
        public const string Query = @"select CountryID, Name from [dbo].[CountryType] order by Name";

        protected override string GetQueryText()
        {
            return Query;
        }
    }
}