using GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders.Base;

namespace GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders
{
    /// <summary>
    /// Загрузка данных справочника "Основание для оценки" (код 6)
    /// </summary>
    public class TestResultSourceDictionaryDataLoader : DictionaryItemDtoDataLoader
    {
        private const string Query = @"select SourceID, Description from [dbo].[EntranceTestResultSource] order by Description";

        protected override string GetQueryText()
        {
            return Query;
        }
    }
}