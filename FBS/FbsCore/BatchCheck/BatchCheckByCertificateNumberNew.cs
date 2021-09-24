using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace Fbs.Core.BatchCheck
{
    public class BatchCheckByCertificateNumberNew
    {
        #region Public Methods

        public bool Parse(StreamReader filereader, string userGroup, ref DataTable resultTable)
        {
            DataTable reportTable = InitialReportTable().Clone();
            bool isCorrectBatch = true;
            string line;
            int lineIndex = 0;
            string errorMessage;

            while ((line = filereader.ReadLine()) != null)
            {
                if (userGroup != "UserRCOI" && reportTable.Rows.Count >= 30)
                {
                    break;
                }

                DataRow reportRow = reportTable.NewRow();
                line = line.Trim();

                string errorcomment = string.Empty;
                lineIndex++;

                if (string.IsNullOrEmpty(line))
                    continue;
                string[] parts = line.Split('%');
                //var partsCopy = new string[40];

                if (parts.Length != 4)
                {
                    errorMessage = "Неверное число полей, необходимо указать 4 значения через разделитель - '%'";
                    errorcomment += errorMessage;
                    reportRow["RowIndex"] = lineIndex + " [НЕВЕРЕН ФОРМАТ]";
                    reportRow["Комментарий"] = errorcomment;// "П";
                    reportTable.Rows.Add(reportRow);
                    isCorrectBatch = false;
                }
                else
                {
                   errorMessage = null;

                    // Проверка поля номер свидетельства
                    var number = (parts[0] ?? string.Empty).Trim();
                    if (string.IsNullOrEmpty(number))
                    {
                        parts[0] += "\t" + "[НЕВЕРЕН ФОРМАТ]";
                        errorMessage = "НС,";
                        errorcomment += errorMessage;
                    }
                    else if (!Regex.IsMatch(number, @"^\d{2}-\d{9}-\d{2}$"))
                    {
                        parts[0] += "\t" + "[НЕВЕРЕН ФОРМАТ]";
                        errorMessage = "НС,";
                        errorcomment += errorMessage;
                    }

                    // Проверка поля ФАМИЛИЯ
                    string lastName = (parts[1] ?? string.Empty).Trim();

                    if ((string.IsNullOrEmpty(lastName) ||
                         (!Regex.IsMatch(lastName, @"^(\s*[а-яёА-ЯЁ]\s*(-(?!-))*\s*)+$") ||
                          lastName.StartsWith("-") || lastName.EndsWith("-"))))
                    {
                        parts[1] += "\t" + "[НЕВЕРЕН ФОРМАТ]";
                        errorMessage = "Ф,";
                        errorcomment += errorMessage;
                    }

                    // Проверка поля ИМЯ
                    string firstName = (parts[2] ?? string.Empty).Trim();
                    if (!string.IsNullOrEmpty(firstName) &&
                        (!Regex.IsMatch(firstName, @"^(\s*[а-яёА-ЯЁ]\s*(-(?!-))*\s*)+$") ||
                         firstName.StartsWith("-") || firstName.EndsWith("-")))
                    {
                        parts[2] += "\t" + "[НЕВЕРЕН ФОРМАТ]";
                        errorMessage = "И,";
                        errorcomment += errorMessage;
                    }

                    // Проверка поля ОТЧЕСТВО
                    string patronymicName = (parts[3] ?? string.Empty).Trim();
                    if (!string.IsNullOrEmpty(patronymicName) &&
                        (!Regex.IsMatch(patronymicName, @"^(\s*[а-яёА-ЯЁ]\s*(-(?!-))*\s*)+$")
                         || patronymicName.StartsWith("-") || patronymicName.EndsWith("-")))
                    {
                        parts[3] += "\t" + "[НЕВЕРЕН ФОРМАТ]";
                        errorMessage = "О,";
                        errorcomment += errorMessage;
                    }

                    if (!String.IsNullOrEmpty(errorMessage)) // Выдавать в отчете только ошибочные стоки.отключено.
                    {
                        errorcomment = errorcomment.TrimEnd(',');
                        reportRow["Номер свидетельства"] = parts[0];
                        reportRow["Фамилия"] = parts[1];
                        reportRow["Имя"] = parts[2];
                        reportRow["Отчество"] = parts[3];
                        reportRow["RowIndex"] = lineIndex;
                        reportRow["Комментарий"] = errorcomment;
                        reportTable.Rows.Add(reportRow);
                        isCorrectBatch = false;
                    }
                }
            }

            resultTable = reportTable;
            reportTable = null;

            return isCorrectBatch;
        }

        #endregion


        #region Private Methods

        private static DataTable InitialReportTable()
        {
            var reportTabe = new DataTable();

            reportTabe.Columns.Add("Номер свидетельства");
            reportTabe.Columns.Add("Типографский номер");
            reportTabe.Columns.Add("Фамилия");
            reportTabe.Columns.Add("Имя");
            reportTabe.Columns.Add("Отчество");
            reportTabe.Columns.Add("Серия паспорта");
            reportTabe.Columns.Add("Номер паспорта");
            reportTabe.Columns.Add("Регион");
            reportTabe.Columns.Add("Год");
            reportTabe.Columns.Add("Статус"); // 10.

            reportTabe.Columns.Add("Русский язык");
            reportTabe.Columns.Add("Апелляция по русскому языку");

            reportTabe.Columns.Add("Математика");
            reportTabe.Columns.Add("Апелляция по математике");
            reportTabe.Columns.Add("Физика");
            reportTabe.Columns.Add("Апелляция по физике");
            reportTabe.Columns.Add("Химия");
            reportTabe.Columns.Add("Апелляция по химии");
            reportTabe.Columns.Add("Биология");
            reportTabe.Columns.Add("Апелляция по биологии"); // 20

            reportTabe.Columns.Add("История России");
            reportTabe.Columns.Add("Апелляция по истории России");
            reportTabe.Columns.Add("География");
            reportTabe.Columns.Add("Апелляция по географии");
            reportTabe.Columns.Add("Английский язык");
            reportTabe.Columns.Add("Апелляция по английскому языку");
            reportTabe.Columns.Add("Немецкий язык");
            reportTabe.Columns.Add("Апелляция по немецкому языку");
            reportTabe.Columns.Add("Французский язык");
            reportTabe.Columns.Add("Апелляция по французскому языку"); // 30

            reportTabe.Columns.Add("Обществознание");
            reportTabe.Columns.Add("Апелляция по обществознанию");
            reportTabe.Columns.Add("Литература");
            reportTabe.Columns.Add("Апелляция по литературе");
            reportTabe.Columns.Add("Испанский язык");
            reportTabe.Columns.Add("Апелляция по испанскому языку");
            reportTabe.Columns.Add("Информатика");
            reportTabe.Columns.Add("Апелляция по информатике");
            reportTabe.Columns.Add("RowIndex"); // 39
            reportTabe.Columns.Add("Комментарий"); // 39

            return reportTabe;
        }

        #endregion
    }
}
