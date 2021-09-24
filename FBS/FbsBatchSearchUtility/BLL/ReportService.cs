namespace FbsBatchSearchUtility.BLL
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    /// <summary>
    /// Сервис получения и обработки результатов поиска
    /// </summary>
    public class ReportService
    {
        #region Constants and Fields

        private const string OPEN_BRACKET = "<";

        private const string CLOSE_BRACKET = ">";


        private readonly SqlConnection sqlConnection;

        private ConnectionStringManager connectionStringManager = new ConnectionStringManager();

        private ReportFilter reportFilter = new ReportFilter();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportService"/> class.
        /// </summary>
        public ReportService()
        {
            this.sqlConnection = new SqlConnection();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Возвращает true если подключение к БД открыто.
        /// </summary>
        public bool Connected
        {
            get
            {
                return this.sqlConnection.State == ConnectionState.Open;
            }
        }

        /// <summary>
        /// Gets or sets ConnectionStringManager.
        /// </summary>
        public ConnectionStringManager ConnectionStringManagerInstance
        {
            get
            {
                return this.connectionStringManager;
            }

            set
            {
                this.connectionStringManager = value;
            }
        }

        /// <summary>
        /// Экземпляр текущего фильтра для выборки
        /// </summary>
        public ReportFilter ReportFilterInstance
        {
            get
            {
                return this.reportFilter;
            }

            set
            {
                this.reportFilter = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Производит подключение к БД на сонове текущих параметров подключения.
        /// </summary>
        /// <returns>
        /// Если все прошло успешно, возвращается true.
        /// </returns>
        public bool Connect()
        {
            if (this.sqlConnection.State == ConnectionState.Open)
            {
                return true;
            }

            this.sqlConnection.ConnectionString = this.connectionStringManager.GetConnectionString();

            try
            {
                this.sqlConnection.Open();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Закрывает текущее соединение с БД
        /// </summary>
        /// <returns>true, если операция цспешна</returns>
        public bool Disconnect()
        {
            if (this.sqlConnection.State == ConnectionState.Closed)
            {
                return true;
            }

            try
            {
                this.sqlConnection.Close();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Подсчитывает общее количество результатов выборки, соответсвующих текущему фильтру.
        /// </summary>
        /// <returns>
        /// Количество результатов
        /// </returns>
        public int Count()
        {
            if (!this.Connect() || this.ReportFilterInstance.FilterData == null)
            {
                return 0;
            }

            // Construct the SQL Request
            string filterSqlString = this.ConstructFilterSqlString();
            string sqlString = string.Format(
                "SELECT COUNT(*) FROM [dbo].[Organization2010]{0}",
                filterSqlString.Length > 0 ? " WHERE " + filterSqlString : string.Empty);

            using (var cmd = new SqlCommand(sqlString, this.sqlConnection))
            {
                try
                {
                    int rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                    return rowCount;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Ошибка базы данных.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            this.Disconnect();
            return 0;
        }

        /// <summary>
        /// Сохраняет результаты в CSV файл
        /// </summary>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        public void SaveResultsToCsvFile(string fileName)
        {
            StreamWriter fileWriter;

            try
            {
                fileWriter = new StreamWriter(fileName, false, Encoding.Default);
            }
            catch (IOException)
            {
                MessageBox.Show(
                    "Невозможно записать в файл: нет места на диске либо файл уже используется.",
                    "Ошибка экспорта",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            List<string> resultList = this.ExportResultsAsCsvStrings();
            foreach (string s in resultList)
            {
                fileWriter.WriteLine(s);
            }

            fileWriter.Close();
        }

        /// <summary>
        /// Производит выборку данных из БД на основе текущего фильтра
        /// </summary>
        /// <param name="rowFrom">
        /// Пейджинг. С какой строки выдать результаты
        /// </param>
        /// <param name="rowTo">
        /// Пейджинг. По какую строку выдать результаты.
        /// </param>
        /// <returns>
        /// Данные выборки в виде списка элементов ReportEntry
        /// </returns>
        public List<ReportEntry> Select(int rowFrom, int rowTo)
        {
            if (!this.Connect() || this.ReportFilterInstance.FilterData == null)
            {
                return new List<ReportEntry>();
            }

            var resultList = new List<ReportEntry>();

            // Construct the SQL Request
            string filterSqlString = this.ConstructFilterSqlString();
            string sqlString =
                string.Format(
                    "SELECT Id, ShortName, FullName, INN, OGRN, OwnerDepartment FROM " +
                    "(SELECT *, row_number() OVER (ORDER BY [Id] ASC) AS rn FROM [dbo].[Organization2010]{2}) tbl " +
                    "WHERE ({0} <= rn AND rn <= {1})",
                    rowFrom,
                    rowTo,
                    filterSqlString.Length > 0 ? " WHERE (" + filterSqlString + ")" : string.Empty);

            // Execute SQL
            using (var cmd = new SqlCommand(sqlString, this.sqlConnection))
            {
                try
                {
                    SqlDataReader row = cmd.ExecuteReader();

                    while (row.Read())
                    {
                        resultList.Add(this.MarkFilterDifference(
                            new ReportEntry
                            {
                                ID = (int)row["Id"],
                                FullName = row["FullName"] == DBNull.Value ? string.Empty : (string)row["FullName"],
                                INN = row["INN"] == DBNull.Value ? string.Empty : (string)row["INN"],
                                OGRN = row["OGRN"] == DBNull.Value ? string.Empty : (string)row["OGRN"],
                                ShortName = row["ShortName"] == DBNull.Value ? string.Empty : (string)row["ShortName"],
                                OwnerDepartment =
                                    row["OwnerDepartment"] == DBNull.Value
                                        ? string.Empty
                                        : (string)row["OwnerDepartment"]
                            }));
                    }

                    row.Close();
                }
                catch (IndexOutOfRangeException e)
                {
                    MessageBox.Show(
                        string.Format("Используемое поле таблицы не найдено: {0}", e.Message),
                        "Ошибка базы данных.",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return null;
                }
                catch (SqlException e)
                {
                    MessageBox.Show(e.Message, "Ошибка базы данных.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }

            this.Disconnect();
            return resultList;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Находит строку фильтра, которой соответствует данный результат. Маркирует слова не из фильтра
        /// при помощи MarkWordsInString.
        /// </summary>
        /// <param name="row">Объект результата</param>
        /// <returns>Маркированный объект результата</returns>
        private ReportEntry MarkFilterDifference(ReportEntry row)
        {
            foreach (DataRow filterRow in this.ReportFilterInstance.FilterData.Rows)
            {
                // Создаем регулярные выражения для каждого из фильтров
                var fullNameExp = new Regex(filterRow[ReportFilter.FULL_ORG_NAME].ToString().Trim().ToLower());
                var shortNameExp = new Regex(filterRow[ReportFilter.SHORT_ORG_NAME].ToString().Trim().ToLower());
                var founderExp = new Regex(filterRow[ReportFilter.FOUNDER].ToString().Trim().ToLower());

                var innExp = new Regex(filterRow[ReportFilter.INN].ToString().Trim().Replace('?', '.').Replace("*", ".*"));
                var ogrnExp = new Regex(filterRow[ReportFilter.OGRN].ToString().Trim().Replace('?', '.').Replace("*", ".*"));

                // Проверяем соотвествие по каждому из полей фильтра.
                var fullNameMatch = !this.ReportFilterInstance.UseFullOrgName || fullNameExp.Match(row.FullName.ToLower()).Success;
                var shortNameMatch = !this.ReportFilterInstance.UseShortOrgName || shortNameExp.Match(row.ShortName.ToLower()).Success;
                var founderMatch = !this.ReportFilterInstance.UseFounder || founderExp.Match(row.OwnerDepartment.ToLower()).Success;

                var innMatch = !this.ReportFilterInstance.UseInn || innExp.Match(row.INN).Success;
                var ogrnMatch = !this.ReportFilterInstance.UseOgrn || ogrnExp.Match(row.OGRN).Success;

                // Маркируем результат в соответствии с фильтром.
                if (fullNameMatch && shortNameMatch && founderMatch && innMatch && ogrnMatch)
                {
                    var fullOrgNameWordsList =
                        new List<string>(filterRow[ReportFilter.FULL_ORG_NAME].ToString().ToLower().Replace("  ", " ").Split(' '));

                    var shortOrgNameWordsList =
                        new List<string>(filterRow[ReportFilter.SHORT_ORG_NAME].ToString().ToLower().Replace("  ", " ").Split(' '));

                    var founderNameWordsList =
                        new List<string>(filterRow[ReportFilter.FOUNDER].ToString().ToLower().Replace("  ", " ").Split(' '));

                    row.FullName = this.MarkWordsInString(row.FullName, fullOrgNameWordsList);
                    row.ShortName = this.MarkWordsInString(row.ShortName, shortOrgNameWordsList);
                    row.OwnerDepartment = this.MarkWordsInString(row.OwnerDepartment, founderNameWordsList);

                    return row;
                }

            }

            return row;
        }


        private string MarkWordsInString(string str, List<string> words)
        {
            if (str.Trim() == string.Empty)
            {
                return str.Trim();
            }

            var splttedData = str.Trim().Split(' ');
            string result = string.Empty;

            var openBracket = false;
            foreach (var part in splttedData)
            {
                if (!words.Contains(part.Replace("\"", string.Empty).Replace("'", string.Empty).Trim().ToLower()))
                {
                    if (openBracket)
                    {
                        result += string.Format(" {0}", part);
                    }
                    else
                    {
                        openBracket = true;
                        result += string.Format(" {1}{0}", part, OPEN_BRACKET);
                    }
                }
                else
                {
                    if (openBracket)
                    {
                        openBracket = false;
                        result += string.Format("{1} {0}", part, CLOSE_BRACKET);
                    }
                    else
                    {
                        result += string.Format(" {0}", part);
                    }
                }
            }

            if (openBracket)
            {
                openBracket = false;
                result += CLOSE_BRACKET;
            }

            return result.Trim();
        }


        private string ConstructFilterSqlString()
        {
            string filterSqlString = string.Empty;

            try
            {
                foreach (DataRow row in this.ReportFilterInstance.FilterData.Rows)
                {
                    string rowReqest = string.Empty;

                    string founder = row[ReportFilter.FOUNDER].ToString().Trim();
                    string fullOrgName = row[ReportFilter.FULL_ORG_NAME].ToString().Trim();
                    string shortOrgName = row[ReportFilter.SHORT_ORG_NAME].ToString().Trim();
                    string inn = row[ReportFilter.INN].ToString().Trim();
                    string ogrn = row[ReportFilter.OGRN].ToString().Trim();

                    inn = inn.Replace('?', '_').Replace('*', '%');
                    ogrn = ogrn.Replace('?', '_').Replace('*', '%');

                    if (founder.Length > 0 && this.ReportFilterInstance.UseFounder)
                    {
                        rowReqest += string.Format("CONTAINS ([OwnerDepartment], '\"{0}\"')", founder);
                    }

                    if (fullOrgName.Length > 0 && this.ReportFilterInstance.UseFullOrgName)
                    {
                        rowReqest += (rowReqest.Length > 0 ? " AND " : string.Empty)
                                     + string.Format("CONTAINS ([Fullname], '\"{0}\"')", fullOrgName);
                    }

                    if (inn.Length > 0 && this.ReportFilterInstance.UseInn)
                    {
                        rowReqest += (rowReqest.Length > 0 ? " AND " : string.Empty)
                                     + string.Format("[INN] LIKE '{0}'", inn);
                    }

                    if (ogrn.Length > 0 && this.ReportFilterInstance.UseOgrn)
                    {
                        rowReqest += (rowReqest.Length > 0 ? " AND " : string.Empty)
                                     + string.Format("[OGRN] LIKE '{0}'", ogrn);
                    }

                    if (shortOrgName.Length > 0 && this.ReportFilterInstance.UseShortOrgName)
                    {
                        rowReqest += (rowReqest.Length > 0 ? " AND " : string.Empty)
                                     + string.Format("CONTAINS ([ShortName], '\"{0}\"')", shortOrgName);
                    }

                    filterSqlString += rowReqest.Length > 2 ? string.Format("({0}) OR ", rowReqest) : string.Empty;
                }
            }
            catch (ArgumentException e)
            {
                MessageBox.Show(
                    string.Format("Используемый столбец таблицы входных данных не найден. {0}", e.Message),
                    "Ошибка в пакетном файле.",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return null;
            }

            if (filterSqlString.Length >= 4)
            {
                filterSqlString = filterSqlString.Substring(0, filterSqlString.Length - 4); // -Length of last " OR ";
            }

            return filterSqlString;
        }

        private List<string> ExportResultsAsCsvStrings()
        {
            if (!this.Connect() || this.ReportFilterInstance.FilterData == null)
            {
                return new List<string>();
            }

            var resultList = new List<string>();

            // Add headers string
            resultList.Add(
                string.Format(
                    "{0};{1};{2};{3};{4}",
                    ReportFilter.FULL_ORG_NAME,
                    ReportFilter.SHORT_ORG_NAME,
                    ReportFilter.INN,
                    ReportFilter.OGRN,
                    ReportFilter.FOUNDER));

            // Construct the SQL Request
            string filterSqlString = this.ConstructFilterSqlString();
            string sqlString =
                string.Format(
                    "SELECT FullName, ShortName, INN, OGRN, OwnerDepartment FROM "
                    + "(SELECT *, row_number() OVER (ORDER BY [Id] ASC) AS rn FROM [dbo].[Organization2010]{0}) tbl ",
                    filterSqlString.Length > 0 ? " WHERE (" + filterSqlString + ")" : string.Empty);

            // Execute SQL
            using (var cmd = new SqlCommand(sqlString, this.sqlConnection))
            {
                try
                {
                    SqlDataReader row = cmd.ExecuteReader();
                    while (row.Read())
                    {
                        string csvString = string.Empty;
                        for (int i = 0; i < row.FieldCount; i++)
                        {
                            string value = row[i].ToString();
                            csvString += value.Contains("\"") || value.Contains(";")
                                             ? string.Format("\"{0}\";", value.Replace("\"", "\"\""))
                                             : value + ";";
                        }

                        resultList.Add(csvString.Substring(0, csvString.Length - 1)); // Trim the last ";"
                    }

                    row.Close();
                }
                catch (IndexOutOfRangeException e)
                {
                    MessageBox.Show(
                        string.Format("Используемое поле таблицы не найдено: {0}", e.Message),
                        "Ошибка базы данных.",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return null;
                }
            }

            this.Disconnect();
            return resultList;
        }

        #endregion
    }
}