using GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders.Base;

namespace GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders
{
    /// <summary>
    /// �������� ������ ����������� "������� �������" (��� 35)
    /// </summary>
    public class LevelBudgetDictionaryDataLoader : DictionaryItemDtoDataLoader
    {
        private const string Query = @"select IdLevelBudget, BudgetName from [dbo].[LevelBudget] order by IdLevelBudget";

        protected override string GetQueryText()
        {
            return Query;
        }
    }
}