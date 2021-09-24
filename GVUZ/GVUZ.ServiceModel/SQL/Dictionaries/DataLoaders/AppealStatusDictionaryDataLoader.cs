using GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders.Base;

namespace GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders
{
    /// <summary>
    /// �������� ������ ����������� "������ ���������, ������������" (��� 37)
    /// </summary>
    public class AppealStatusDictionaryDataLoader : DictionaryItemDtoDataLoader
    {
        private const string Query = @"select AppealStatusID, StatusName from [dbo].[AppealStatus] order by AppealStatusID";

        protected override string GetQueryText()
        {
            return Query;
        }
    }
}