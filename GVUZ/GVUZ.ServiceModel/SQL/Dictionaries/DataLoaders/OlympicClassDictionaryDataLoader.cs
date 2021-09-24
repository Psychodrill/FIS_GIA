using GVUZ.ServiceModel.Import.WebService;
using GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders.Base;

namespace GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders
{
    /// <summary>
    /// Загрузка данных справочника "Классы олимпиады" (код 40)
    /// </summary>
    public class OlympicClassDictionaryDataLoader : IDictionaryDataLoader<DictionaryItemDto>
    {
        private DictionaryItemDto[] data = new DictionaryItemDto[]
        {
            new DictionaryItemDto { ID = "1", Name="7" },
            new DictionaryItemDto { ID = "2", Name="8" },
            new DictionaryItemDto { ID = "3", Name="7, 8" },
            new DictionaryItemDto { ID = "4", Name="9" },
            new DictionaryItemDto { ID = "5", Name="7, 9" },
            new DictionaryItemDto { ID = "6", Name="8, 9" },
            new DictionaryItemDto { ID = "7", Name="7, 8, 9" },
            new DictionaryItemDto { ID = "8", Name="10" },
            new DictionaryItemDto { ID = "9", Name="7, 10" },
            new DictionaryItemDto { ID = "10", Name="8, 10" },
            new DictionaryItemDto { ID = "11", Name="7, 8, 10" },
            new DictionaryItemDto { ID = "12", Name="9, 10" },
            new DictionaryItemDto { ID = "13", Name="7, 9, 10" },
            new DictionaryItemDto { ID = "14", Name="8, 9, 10" },
            new DictionaryItemDto { ID = "15", Name="7, 8, 9, 10" },
            new DictionaryItemDto { ID = "16", Name="11" },
            new DictionaryItemDto { ID = "17", Name="7, 11" },
            new DictionaryItemDto { ID = "18", Name="8, 11" },
            new DictionaryItemDto { ID = "19", Name="7, 8, 11" },
            new DictionaryItemDto { ID = "20", Name="9, 11" },
            new DictionaryItemDto { ID = "21", Name="7, 9, 11" },
            new DictionaryItemDto { ID = "22", Name="8, 9, 11" },
            new DictionaryItemDto { ID = "23", Name="7, 8, 9, 11" },
            new DictionaryItemDto { ID = "24", Name="10, 11" },
            new DictionaryItemDto { ID = "25", Name="7, 10, 11" },
            new DictionaryItemDto { ID = "26", Name="8, 10, 11" },
            new DictionaryItemDto { ID = "27", Name="7, 8, 10, 11" },
            new DictionaryItemDto { ID = "28", Name="9, 10, 11" },
            new DictionaryItemDto { ID = "29", Name="7, 9, 10, 11" },
            new DictionaryItemDto { ID = "30", Name="8, 9, 10, 11" },
            new DictionaryItemDto { ID = "31", Name="7, 8, 9, 10, 11" },
            new DictionaryItemDto { ID = "255", Name="все классы" },
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
