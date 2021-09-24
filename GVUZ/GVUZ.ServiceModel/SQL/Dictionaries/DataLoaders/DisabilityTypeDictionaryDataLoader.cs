using GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders.Base;

namespace GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders
{
    /// <summary>
    /// �������� ������ ����������� "������ ������������" (��� 23)
    /// </summary>
    public class DisabilityTypeDictionaryDataLoader : DictionaryItemDtoDataLoader
    {
        private const string Query = @"select DisabilityId, Name from [dbo].[DisabilityType] order by Name";

        protected override string GetQueryText()
        {
            return Query;
        }
    }
}