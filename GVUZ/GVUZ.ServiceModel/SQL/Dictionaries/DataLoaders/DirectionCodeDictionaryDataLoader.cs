using GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders.Base;

namespace GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders
{
    /// <summary>
    /// Загрузка данных справочника "Коды направлений подготовки" (код 9)
    /// </summary>
    public class DirectionCodeDictionaryDataLoader : DictionaryItemDtoDataLoader
    {
        private const string Query = @"select DirectionID, Code from [dbo].[Direction] Where IsVisible = 1 order by Code";

        protected override string GetQueryText()
        {
            return Query;
        }
    }
}