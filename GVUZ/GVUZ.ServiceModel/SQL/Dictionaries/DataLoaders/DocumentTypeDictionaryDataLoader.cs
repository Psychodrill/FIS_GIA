using GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders.Base;

namespace GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders
{
    /// <summary>
    /// �������� ������ ����������� "��� ���������" (��� 31)
    /// </summary>
    public class DocumentTypeDictionaryDataLoader : DictionaryItemDtoDataLoader
    {
        private const string Query = @"select DocumentId, Name from [dbo].[DocumentType] order by DocumentId";

        protected override string GetQueryText()
        {
            return Query;
        }
    }
}