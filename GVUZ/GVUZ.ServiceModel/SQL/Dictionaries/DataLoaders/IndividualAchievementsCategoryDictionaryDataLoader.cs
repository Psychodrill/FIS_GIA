using GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders.Base;

namespace GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders
{
    /// <summary>
    /// Загрузка данных справочника "Категории индивидуальных достижений" (код 36)
    /// </summary>
    public class IndividualAchievementsCategoryDictionaryDataLoader : DictionaryItemDtoDataLoader
    {
        private const string Query = @"select IdCategory, CategoryName from [dbo].[IndividualAchievementsCategory] order by IdCategory";

        protected override string GetQueryText()
        {
            return Query;
        }
    }
}