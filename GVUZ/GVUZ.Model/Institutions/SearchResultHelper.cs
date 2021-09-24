using System;
using System.Collections.Generic;

namespace GVUZ.Model.Institutions
{
    /// <summary>
    ///     Содержит дополнительную обработку результатов поиска.
    /// </summary>
    public static class SearchResultHelper
    {
        /// <summary>
        ///     Создает из обычного списка иерархию (<see cref="InstitutionSearchResult.Children" />).
        /// </summary>
        /// <remarks>
        ///     Cписок отсотрирован по depth и lineage,
        ///     поэтому можно бежать по нему только вперед (сохраняя всех потенциальных родителей
        ///     для последующего добавления детей).
        /// </remarks>
        public static List<InstitutionSearchResult> CreateHierarchy(IEnumerable<InstitutionSearchResult> items,
                                                                    InstitutionSearchHierarchyParameters parameters)
        {
            var result = new List<InstitutionSearchResult>();
            var parentList = new List<InstitutionSearchResult>();

            int parentIndex = 0;
            int rootStructureID = parameters.ParentStructureID ?? -1;
            InstitutionSearchResult parent = null;

            foreach (InstitutionSearchResult item in items)
            {
                if (item.ShouldHasChildren)
                    parentList.Add(item);

                if (!item.ParentID.HasValue || rootStructureID == item.AdmissionStructureID)
                {
                    result.Add(item);
                    continue;
                }

                int parentID = item.ParentID.Value;
                parent = parentList[parentIndex];

                if (parent.AdmissionStructureID == parentID)
                {
                    parent.Children.Add(item);
                    continue;
                }

                // сортируем листы родителя, потому что первоначальный список нельзя отсортировать по названию

                SortParentChildren(parent.Children);

                while (++parentIndex < parentList.Count)
                {
                    parent = parentList[parentIndex];
                    if (parent.AdmissionStructureID == parentID)
                    {
                        parent.Children.Add(item);
                        break;
                    }
                }
                if (parentIndex == parentList.Count)
                    throw new InvalidOperationException
                        (String.Format("Не найден родительский узел для '{0}'.", item.Name));
            }
            if (parent != null)
                SortParentChildren(parent.Children); // сортируем листы последнего родителя
            result.Sort((x, y) => x.Name.CompareTo(y.Name));
            return result;
        }

        private static void SortParentChildren(List<InstitutionSearchResult> list)
        {
            list.Sort((x, y) => x.Name.CompareTo(y.Name));
        }
    }
}