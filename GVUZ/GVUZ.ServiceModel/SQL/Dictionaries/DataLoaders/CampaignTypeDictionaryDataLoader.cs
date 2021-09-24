using GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders.Base;

namespace GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders
{
    /// <summary>
    /// Загрузка данных справочника "Тип приемной кампании" (код 38)
    /// </summary>
    public class CampaignTypeDictionaryDataLoader : DictionaryItemDtoDataLoader
    {
        private const string Query = @"SELECT [CampaignTypeID], [Name] FROM [dbo].[CampaignTypes] Order by [CampaignTypeID]";

        protected override string GetQueryText()
        {
            return Query;
        }
    }
}