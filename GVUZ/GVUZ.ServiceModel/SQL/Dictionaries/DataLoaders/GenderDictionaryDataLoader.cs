using GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders.Base;

namespace GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders
{
    /// <summary>
    /// Загрузка данных справочника "Пол" (код 5)
    /// </summary>
    public class GenderDictionaryDataLoader : DictionaryItemDtoDataLoader
    {
        private const string Query = @"select GenderID, Name from [dbo].[GenderType] order by Name";

        protected override string GetQueryText()
        {
            return Query;
        }
    }
}