using GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders.Base;

namespace GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders
{
    /// <summary>
    /// Загрузка данных справочника "Источник финансирования" (код 15)
    /// </summary>
    public class EducationSourceDictionaryDataLoader : DictionaryItemDtoDataLoader
    {
        private const string Query = @"select ItemTypeID, Name from [dbo].[AdmissionItemType] where ItemLevel = 8 order by DisplayOrder, Name";

        protected override string GetQueryText()
        {
            return Query;
        }
    }
}