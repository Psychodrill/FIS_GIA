namespace Fbs.Core.BatchCheck
{
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// The batch check by mark sum parser.
    /// </summary>
    public class BatchCheckByMarkSumParser
    {
        #region Public Methods and Operators

        /// <summary>
        /// The parse.
        /// </summary>
        /// <param name="reader">
        /// The reader.
        /// </param>
        /// <param name="userGroup">
        /// The user group.
        /// </param>
        /// <param name="resultData">
        /// The result data.
        /// </param>
        /// <param name="ignoreSum">
        /// The ignore sum.
        /// </param>
        /// <returns>
        /// The parse.
        /// </returns>
        public bool Parse(StreamReader reader, string userGroup, ref DataTable resultData, bool ignoreSum,bool allowSumEmpty)
        {
            var reportTable = new DataTable();
            reportTable.Columns.Add("ФИО");
            reportTable.Columns.Add("Сумма баллов");
            reportTable.Columns.Add("Предмет");
            reportTable.Columns.Add("RowIndex");
            reportTable.Columns.Add("Комментарий");

            bool isCorrectBatch = true;

            string line;
            int lineIndex = 0;
            string errorMessage;
            string name = null;
            string markSum = null;
            DataRow reportRow;
            string errorcomment;

            while ((line = reader.ReadLine()) != null)
            {
                reportRow = reportTable.NewRow();

                line = line.Trim();

                errorcomment = string.Empty;
                lineIndex++;

                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                string[] parts = line.Split('%');
                errorMessage = string.Empty;
                if (parts.Length < (ignoreSum ? 13 : 14))
                {
                    errorMessage = "Неверное число полей, необходимо указать 14 значений через разделитель - '%'";
                    parts[0] = errorMessage;
                    reportRow["RowIndex"] = lineIndex + " [НЕВЕРЕН ФОРМАТ]";
                    reportRow["Комментарий"] = "П";
                    reportTable.Rows.Add(reportRow);
                    isCorrectBatch = false;
                    continue;
                }

                name = parts[0].Trim();

                if (string.IsNullOrEmpty(name))
                {
                    parts[0] += "\t" + "[НЕВЕРЕН ФОРМАТ]";

                    // errorMessage = "Поле ФИО должно быть заполнено и должно содержать только русские буквы.";
                    errorMessage = "Ф,";
                    errorcomment += errorMessage;
                }

                // Проверка поля СУММА БАЛЛОВ
                markSum = parts[1].Trim();
                if (!allowSumEmpty && !ignoreSum && (string.IsNullOrEmpty(markSum) || !Regex.IsMatch(markSum, @"^[\d]+$")))
                {
                    parts[1] += "\t" + "[НЕВЕРЕН ФОРМАТ]";

                    errorMessage = "СБ,";
                    errorcomment += errorMessage;
                }

                string subjectError = string.Empty;
                {
                    if (!parts.Where((x, y) => y > 1 && y < (ignoreSum ? 12 : 13) && !string.IsNullOrEmpty(x)).Any())
                    {
                        errorMessage = "П,";
                        subjectError = "Необходимо указать хотя бы один предмет,";
                        errorcomment += subjectError;
                    }
                }

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    reportRow["ФИО"] = parts[0];
                    reportRow["Сумма баллов"] = parts[1];
                    reportRow["Предмет"] = subjectError;
                    reportRow["RowIndex"] = lineIndex;
                    reportRow["Комментарий"] = !string.IsNullOrEmpty(errorcomment) ? errorcomment.Trim().TrimEnd(new[] {','}) : string.Empty;
                    reportTable.Rows.Add(reportRow);
                    isCorrectBatch = false;
                }
            }

            resultData = reportTable;
            return isCorrectBatch;
        }

        #endregion
    }
}