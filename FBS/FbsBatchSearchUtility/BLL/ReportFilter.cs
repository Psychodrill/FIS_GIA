namespace FbsBatchSearchUtility.BLL
{
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    /// <summary>
    /// Хранит данные текущего фильтра для выборки
    /// </summary>
    public class ReportFilter
    {
        public const string FULL_ORG_NAME = "Полное наименование организации";
        public const string SHORT_ORG_NAME = "Краткое наименование организации";
        public const string INN = "ИНН";
        public const string OGRN = "ОГРН";
        public const string FOUNDER = "Наименование учредителя";

        /// <summary>
        /// Данные выборки в виде таблицы
        /// </summary>
        public DataTable FilterData { get; set; }

        /// <summary>
        /// Флаг использования полного наименования организации
        /// </summary>
        public bool UseFullOrgName { get; set; }

        /// <summary>
        /// Флаг использования краткого наименования организации
        /// </summary>
        public bool UseShortOrgName { get; set; }

        /// <summary>
        /// Флаг использования ИНН
        /// </summary>
        public bool UseInn { get; set; }

        /// <summary>
        /// Флаг использования ОГРН
        /// </summary>
        public bool UseOgrn { get; set; }

        /// <summary>
        /// Флаг использования учредителя
        /// </summary>
        public bool UseFounder { get; set; }

        /// <summary>
        /// Загружает данные фильтра из CSV файла
        /// </summary>
        /// <param name="fileName">
        /// Имя csv файла
        /// </param>
        public bool LoadFilterFromCsvFile(string fileName)
        {
            StreamReader fileReader;

            try
            {
                fileReader = new StreamReader(fileName, Encoding.Default);
            }
            catch (IOException e)
            {
                MessageBox.Show(
                    "Невозможно открыть файл: Файл используется.",
                    "Ошибка открытия пакетного файла",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }

            this.FilterData = new DataTable();

            // Match csv row element
            var rexp = new Regex("(\"\"([^\"\"]*|\"\"{2})*\"\"(;|$))|\"\"[^\"\"]*\"\"(;|$)|[^;]+(;|$)|(;)");

            // Read csv headers
            if (!fileReader.EndOfStream)
            {
                string s = fileReader.ReadLine();

                if (rexp.IsMatch(s))
                {
                    MatchCollection matches = rexp.Matches(s);

                    foreach (Group header in matches)
                    {
                        var column = new DataColumn();
                        column.DataType = typeof(string);
                        column.ColumnName = header.Value.EndsWith(";")
                                                ? header.Value.Substring(0, header.Value.Length - 1)
                                                : header.Value;

                        this.FilterData.Columns.Add(column);
                    }
                }
            }

            // Read csv data
            while (!fileReader.EndOfStream)
            {
                string s = fileReader.ReadLine();
                MatchCollection matches = rexp.Matches(s);

                DataRow dataRow = this.FilterData.NewRow();

                int i = 0;
                foreach (Group element in matches)
                {
                    var data = element.Value.EndsWith(";")
                                   ? element.Value.Substring(0, element.Value.Length - 1)
                                   : element.Value;

                    // remove any quotes, because they affect nothing
                    data = data.Replace("\"", string.Empty).Replace("'", string.Empty);

                    dataRow[i] = data;
                    i++;
                }

                this.FilterData.Rows.Add(dataRow);
            }

            return true;
        }
    }
}