using GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders.Base;

namespace GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders
{
    /// <summary>
    /// Загрузка данных справочника "Общеобразовательные предметы" (код 1)
    /// </summary>
    public class SubjectDictionaryDataLoader : DictionaryItemDtoDataLoader
    {
        private const string Query = @"select SubjectID, Name from [dbo].[Subject] order by SubjectID";

        protected override string GetQueryText()
        {
            return Query;
        }
    }
}