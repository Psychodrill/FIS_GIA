using System;
using System.Collections.Generic;

namespace GVUZ.Model.Institutions
{
    /// <summary>
    ///     �������� �������������� ��������� ����������� ������.
    /// </summary>
    public static class SearchResultHelper
    {
        /// <summary>
        ///     ������� �� �������� ������ �������� (<see cref="InstitutionSearchResult.Children" />).
        /// </summary>
        /// <remarks>
        ///     C����� ������������ �� depth � lineage,
        ///     ������� ����� ������ �� ���� ������ ������ (�������� ���� ������������� ���������
        ///     ��� ������������ ���������� �����).
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

                // ��������� ����� ��������, ������ ��� �������������� ������ ������ ������������� �� ��������

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
                        (String.Format("�� ������ ������������ ���� ��� '{0}'.", item.Name));
            }
            if (parent != null)
                SortParentChildren(parent.Children); // ��������� ����� ���������� ��������
            result.Sort((x, y) => x.Name.CompareTo(y.Name));
            return result;
        }

        private static void SortParentChildren(List<InstitutionSearchResult> list)
        {
            list.Sort((x, y) => x.Name.CompareTo(y.Name));
        }
    }
}