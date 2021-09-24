using GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders
{
    /// <summary>
    /// Загрузка данных справочника 41 - Тип населенного пункта (таблица TownType)
    /// </summary>
    public class TownTypeDictionaryDataLoader : DictionaryItemDtoDataLoader
    {
        private const string Query = @"select [TownTypeID], [Name] from [TownType] ORDER BY 1";

        protected override string GetQueryText()
        {
            return Query;
        }
    }
}
