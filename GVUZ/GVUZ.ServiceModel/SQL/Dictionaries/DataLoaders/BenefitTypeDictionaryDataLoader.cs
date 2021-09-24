using GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders.Base;

namespace GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders
{
    /// <summary>
    /// �������� ������ ����������� "��� ������" (��� 30)
    /// </summary>
    public class BenefitTypeDictionaryDataLoader : DictionaryItemDtoDataLoader
    {
        private const string Query = @"select BenefitId, Name from [dbo].[Benefit] order by BenefitId";

        protected override string GetQueryText()
        {
            return Query;
        }
    }
}