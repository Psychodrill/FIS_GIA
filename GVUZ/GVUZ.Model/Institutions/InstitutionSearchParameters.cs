using System.Collections.Generic;

namespace GVUZ.Model.Institutions
{
    public class InstitutionSearchHierarchyParameters : InstitutionSearchParameters<List<InstitutionSearchResult>>
    {
        public override ConvertResults Convert
        {
            get
            {
                if (base.Convert == null)
                    return (items, parameters) =>
                           SearchResultHelper.CreateHierarchy(items, (InstitutionSearchHierarchyParameters) parameters);
                return base.Convert;
            }
            set { base.Convert = value; }
        }
    }

    /// <summary>
    ///     ��������� ��� ������ ��������������� ����������. <see cref="IsVUZ" /> � <see cref="IsSSUZ" /> �� ��������� true.
    /// </summary>
    public class InstitutionSearchParameters<TResult> : SearchParameters<TResult>
    {
        public InstitutionSearchParameters()
        {
            IsVUZ = true;
            IsSSUZ = true;
        }

        /// <summary>
        ///     ����� �������� ��
        /// </summary>
        public string NamePart { get; set; }

        /// <summary>
        ///     ������ �� �� ����� (�� ��������� true)
        /// </summary>
        public bool IsVUZ { get; set; }

        /// <summary>
        ///     ������ �� �� ������ (�� ��������� true)
        /// </summary>
        public bool IsSSUZ { get; set; }

        /// <summary>
        ///     ����� - ������������ ��������, ���������� ������������ ���.
        /// </summary>
        public string Snils { get; set; }

        /// <summary>
        ///     ����������� �� ������ ��������� ������ ������ <see cref="InstitutionSearchResult.AdmissionItemID" />.
        /// </summary>
        public int? ParentStructureID { get; set; }

        /// <summary>
        ///     ����������� �� ������� ������������� ������.
        /// </summary>
        public short? DepthLimit { get; set; }

        /// <summary>
        ///     ����������� (�������������)
        /// </summary>
        public string DirectionName { get; set; }

        /// <summary>
        ///     ��� ����������� (�������������)
        /// </summary>
        public string DirectionCode { get; set; }

        /// <summary>
        ///     ������
        /// </summary>
        public string RegionName { get; set; }

        /// <summary>
        ///     ��������������-�������� �����
        /// </summary>
        public int? FormOfLawID { get; set; }

        /// <summary>
        ///     ������� �����������
        /// </summary>
        public short? EducationLevelID { get; set; }

        /// <summary>
        ///     ����� ��������
        /// </summary>
        public short? StudyID { get; set; }

        /// <summary>
        ///     ����� ��������
        /// </summary>
        public short? AdmissionTypeID { get; set; }

        /// <summary>
        ///     ���� �� ������� �������
        /// </summary>
        public bool? HasMilitaryDepartment { get; set; }

        /// <summary>
        ///     ���� �� ���������������� �����
        /// </summary>
        public bool? HasPreparatoryCourses { get; set; }

        /// <summary>
        ///     ���� �� ������ ��� �����������
        /// </summary>
        public bool? HasOlympics { get; set; }

        /// <summary>
        ///     ������ ��������, �� ��������� 50 (�� ����� ������, ���� ����� <see cref="ParentStructureID" />)
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        ///     ����� ��������, �� ��������� 1 (�� ����� ������, ���� ����� <see cref="ParentStructureID" />)
        /// </summary>
        public int? PageNumber { get; set; }
    }
}