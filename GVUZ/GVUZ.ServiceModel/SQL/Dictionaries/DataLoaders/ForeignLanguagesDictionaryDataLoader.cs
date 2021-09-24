using GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders.Base;

namespace GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders
{
    /// <summary>
    /// Загрузка данных справочника "Иностранные языки" (код 32)
    /// </summary>
    public class ForeignLanguagesDictionaryDataLoader : DictionaryItemDtoDataLoader
    {
        private const string Query = @"select LanguageId, Name from [dbo].[ForeignLanguageType] order by LanguageId";

        protected override string GetQueryText()
        {
            return Query;
        }
    }
}