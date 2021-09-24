namespace GVUZ.Model.Institutions
{
    public enum AdmissionItemLevel : short
    {
        /// <summary>
        ///     "���"
        /// </summary>
        Institution = 0,

        /// <summary>
        ///     ����
        /// </summary>
        Course = 1,

        /// <summary>
        ///     ������� �����������
        /// </summary>
        EducationLevel = 2,

        /// <summary>
        ///     ���������
        /// </summary>
        Faculty = 3,

        /// <summary>
        ///     �������
        /// </summary>
        Department = 4,

        /// <summary>
        ///     ������ �����������
        /// </summary>
        DirectionGroup = 5,

        /// <summary>
        ///     ����������� (�������������)
        /// </summary>
        Direction = 6,

        /// <summary>
        ///     ����� ��������
        /// </summary>
        Study = 7,

        /// <summary>
        ///     ��� ������ (������)
        /// </summary>
        AdmissionType = 8
    }
}