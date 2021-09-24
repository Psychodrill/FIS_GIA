using GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders.Base;

namespace GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders
{
    /// <summary>
    /// �������� ������ ����������� "��� ��������� ��� �������������� ��������� ��" (��� 33)
    /// </summary>
    public class InstitutionDocumentTypeDictionaryDataLoader : DictionaryItemDtoDataLoader
    {
        private const string Query = @"select InstitutionDocumentTypeID, Name from [dbo].[InstitutionDocumentType] order by InstitutionDocumentTypeID";

        protected override string GetQueryText()
        {
            return Query;
        }
    }
}