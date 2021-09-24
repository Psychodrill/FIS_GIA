using GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders.Base;

namespace GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders
{
    /// <summary>
    /// �������� ������ ����������� "��� ���������, ��������������� ��������" (��� 22)
    /// </summary>
    public class IdentityDocumentTypeDictionaryDataLoader : DictionaryItemDtoDataLoader
    {
        private const string Query = @"select IdentityDocumentTypeID, Name from [dbo].[IdentityDocumentType] order by IdentityDocumentTypeID";

        protected override string GetQueryText()
        {
            return Query;
        }
    }
}