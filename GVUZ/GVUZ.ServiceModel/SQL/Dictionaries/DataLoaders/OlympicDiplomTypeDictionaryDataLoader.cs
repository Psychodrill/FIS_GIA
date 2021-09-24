using GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders.Base;

namespace GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders
{
    /// <summary>
    /// Загрузка данных справочника "Тип диплома" (код 18)
    /// </summary>
    public class OlympicDiplomTypeDictionaryDataLoader : DictionaryItemDtoDataLoader
    {
        private const string Query = @"select OlympicDiplomTypeID, Name from [dbo].[OlympicDiplomType] where OlympicDiplomTypeID <> 3 order by Name";

        protected override string GetQueryText()
        {
            return Query;
        }
    }
}