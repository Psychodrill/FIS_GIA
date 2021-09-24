namespace FbsBatchSearchUtility.BLL
{
    /// <summary>
    /// VO ��� �������� ������ � �������������� �������
    /// </summary>
    public class ReportEntry
    {
        /// <summary>
        /// ID �������� � ������� ��
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// ������ ������������ �����������
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// ������� ������������ �����������
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// ��� �����������
        /// </summary>
        public string INN { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public string OGRN { get; set; }

        /// <summary>
        /// ���������� �����������
        /// </summary>
        public string OwnerDepartment { get; set; }
    }
}
