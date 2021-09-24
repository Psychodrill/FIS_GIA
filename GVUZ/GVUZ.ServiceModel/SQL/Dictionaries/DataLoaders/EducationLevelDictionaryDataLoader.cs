using GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders.Base;

namespace GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders
{
    /// <summary>
    /// Загрузка данных справочника "Уровни образования" (код 2)
    /// </summary>
    public class EducationLevelDictionaryDataLoader : DictionaryItemDtoDataLoader
    {
        private const string Query = @"select ItemTypeId, Name from [dbo].[AdmissionItemType] where ItemLevel = 2 order by DisplayOrder, Name";

        protected override string GetQueryText()
        {
            return Query;
        }
    }
}