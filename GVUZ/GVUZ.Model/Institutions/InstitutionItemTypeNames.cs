using System;
using System.Collections.Generic;
using System.Data.Objects;

namespace GVUZ.Model.Institutions
{
    /// <summary>
    ///     Хранилище названий элементов структуры института
    /// </summary>
    public static class InstitutionItemTypeNames
    {
        /// <summary>
        ///     ленивая инициализация
        /// </summary>
        private static readonly Lazy<Dictionary<InstitutionItemType, string>> _names =
            new Lazy<Dictionary<InstitutionItemType, string>>(InitNames);

        private static Dictionary<InstitutionItemType, string> InitNames()
        {
            using (var dbContext = new InstitutionsEntities())
            {
                var res = new Dictionary<InstitutionItemType, string>();
                ObjectResult<TypeNames> query =
                    dbContext.ExecuteStoreQuery<TypeNames>("SELECT * FROM InstitutionItemType");
                foreach (TypeNames typeNames in query)
                    res[(InstitutionItemType) typeNames.ItemTypeID] = typeNames.Name;
                return res;
            }
        }

        public static string GetName(this InstitutionItemType type)
        {
            return _names.Value[type];
        }

        private class TypeNames
        {
            public short ItemTypeID { get; set; }
            public string Name { get; set; }
        }
    }
}