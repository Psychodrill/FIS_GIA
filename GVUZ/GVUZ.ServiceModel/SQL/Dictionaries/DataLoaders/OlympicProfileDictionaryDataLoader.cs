using GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders.Base;

namespace GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders
{
    /// <summary>
    /// Загрузка данных справочника "Профили олимпиады" (код 39)
    /// </summary>
    public class OlympicProfileDictionaryDataLoader : DictionaryItemDtoDataLoader
    {
        private const string Query = @"SELECT [OlympicProfileID], [ProfileName] FROM [OlympicProfile] ORDER BY [OlympicProfileID]";

        protected override string GetQueryText()
        {
            return Query;
        }
    }
}
