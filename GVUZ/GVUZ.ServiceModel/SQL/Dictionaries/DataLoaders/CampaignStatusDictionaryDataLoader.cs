using GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders.Base;

namespace GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders
{
    /// <summary>
    /// �������� ������ ����������� "������ �������� ��������" (��� 34)
    /// </summary>
    public class CampaignStatusDictionaryDataLoader : DictionaryItemDtoDataLoader
    {
        private const string Query = @"select StatusID, Name from [dbo].[CampaignStatus] order by StatusID";

        protected override string GetQueryText()
        {
            return Query;
        }
    }
}