using GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders.Base;

namespace GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders
{
    /// <summary>
    /// �������� ������ ����������� "�����������" (��� 21)
    /// </summary>
    public class CitizenshipDictionaryDataLoader : DictionaryItemDtoDataLoader
    {
        protected override string GetQueryText()
        {
            return CountryDictionaryDataLoader.Query;
        }
    }
}