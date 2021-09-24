using GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders.Base;

namespace GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders
{
    /// <summary>
    /// Загрузка данных справочника "Форма обучения" (код 14)
    /// </summary>
    public class EducationFormDictionaryDataLoader : DictionaryItemDtoDataLoader
    {
        private const string Query = @"select ItemTypeID, Name from [dbo].[AdmissionItemType] where ItemLevel = 7 order by DisplayOrder, Name";

        protected override string GetQueryText()
        {
            return Query;
        }
    }
}