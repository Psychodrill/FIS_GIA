﻿using GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders.Base;

namespace GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders
{
    /// <summary>
    /// Загрузка данных справочника 44 - Тип документа, подтверждающего принадлежность к соотечественникам ([CompatriotCategory])
    /// </summary>
    public class StateEmployeeCategoryDictionaryDataLoader : DictionaryItemDtoDataLoader
    {
        private const string Query = @"SELECT [StateEmployeeCategoryID], [Name] FROM [StateEmployeeCategory] ORDER BY 1";

        protected override string GetQueryText()
        {
            return Query;
        }
    }
}