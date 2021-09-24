using GVUZ.ServiceModel.Import.WebService;
using GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders.Base;

namespace GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders
{
    /// <summary>
    /// Загрузка данных справочника "Уровень олимпиады" (код 3)
    /// </summary>
    public class OlympicLevelDictionaryDataLoader : IDictionaryDataLoader<DictionaryItemDto>
    {
        //private const string Query = @"select OlympicLevelID, Name from [dbo].[OlympicLevel] order by Name";

        //protected override string GetQueryText()
        //{
        //    return Query;
        //}

        private DictionaryItemDto[] data = new DictionaryItemDto[]
        {
            new DictionaryItemDto { ID = "1", Name="I" },
            new DictionaryItemDto { ID = "2", Name="II" },
            new DictionaryItemDto { ID = "3", Name="I, II" },
            new DictionaryItemDto { ID = "4", Name="III" },
            new DictionaryItemDto { ID = "5", Name="I, III" },
            new DictionaryItemDto { ID = "6", Name="II, III" },
            new DictionaryItemDto { ID = "7", Name="I, II, III" },
            new DictionaryItemDto { ID = "255", Name="все уровни" },
        };

        public void Dispose()
        {
            data = null;
        }

        public DictionaryItemDto[] Load()
        {
            return data;
        }
    }
}