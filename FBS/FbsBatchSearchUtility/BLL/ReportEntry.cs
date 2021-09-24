namespace FbsBatchSearchUtility.BLL
{
    /// <summary>
    /// VO для передачи данных в результирующую таблицу
    /// </summary>
    public class ReportEntry
    {
        /// <summary>
        /// ID элемента в таблице БД
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Полное наименование организации
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Краткое наименование организации
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// ИНН организации
        /// </summary>
        public string INN { get; set; }

        /// <summary>
        /// ОГРН
        /// </summary>
        public string OGRN { get; set; }

        /// <summary>
        /// Учредитель организации
        /// </summary>
        public string OwnerDepartment { get; set; }
    }
}
