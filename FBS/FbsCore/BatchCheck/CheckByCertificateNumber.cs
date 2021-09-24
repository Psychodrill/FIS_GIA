using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace Fbs.Core.BatchCheck
{
    public class BatchCheckByCertificateNumber
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
                var partsCopy = new string[40];

                if (parts.Length != 18)
                {
                    errorMessage = "Неверное число полей, необходимо указать 18 значений через разделитель - '%'";
                    errorcomment += errorMessage;
                    partsCopy[0] = errorMessage;
                    reportRow["RowIndex"] = lineIndex + " [НЕВЕРЕН ФОРМАТ]";
                    reportRow["Комментарий"] = "П";
                    reportTable.Rows.Add(reportRow);
                    isCorrectBatch = false;
                }
                else
                {
                    int j = 10;
                    partsCopy[0] = parts[0];
                    partsCopy[2] = parts[1];
                    partsCopy[3] = parts[2];
                    partsCopy[4] = parts[3];

                    for (int i = 4; i < 18; i++) //12
                    {
                        partsCopy[j] = parts[i];
                        j += 2;
                    }

                    errorMessage = null;

                    // Проверка поля номер свидетельства
                    var number = partsCopy[0];
                    if (string.IsNullOrEmpty(number))
                    {
                        partsCopy[0] += "\t" + "[НЕВЕРЕН ФОРМАТ]";
                        errorMessage = "НС,";
                        errorcomment += errorMessage;
                    }
                    else if (!Regex.IsMatch(number, @"^\d{2}-\d{9}-\d{2}$"))
                    {
                        partsCopy[0] += "\t" + "[НЕВЕРЕН ФОРМАТ]";
                        errorMessage = "НС,";
                        errorcomment += errorMessage;
                    }

                    // Проверка поля ФАМИЛИЯ
                    string lastName = partsCopy[2].Trim();

                    if ((string.IsNullOrEmpty(lastName) ||
                         (!Regex.IsMatch(lastName, @"^(\s*[а-яёА-ЯЁ]\s*(-(?!-))*\s*)+$") ||
                          lastName.StartsWith("-") || lastName.EndsWith("-"))))
                    {
                        partsCopy[2] += "\t" + "[НЕВЕРЕН ФОРМАТ]";
                        errorMessage = "Ф,";
                        errorcomment += errorMessage;
                    }

                    // Проверка поля ИМЯ
                    string firstName = partsCopy[3].Trim();
                    if (!string.IsNullOrEmpty(firstName) &&
                        (!Regex.IsMatch(firstName, @"^(\s*[а-яёА-ЯЁ]\s*(-(?!-))*\s*)+$") ||
                         firstName.StartsWith("-") || firstName.EndsWith("-")))
                    {
                        partsCopy[3] += "\t" + "[НЕВЕРЕН ФОРМАТ]";
                        errorMessage = "И,";
                        errorcomment += errorMessage;
                    }

                    // Проверка поля ОТЧЕСТВО
                    string patronymicName = partsCopy[4].Trim();
                    if (!string.IsNullOrEmpty(patronymicName) &&
                        (!Regex.IsMatch(patronymicName, @"^(\s*[а-яёА-ЯЁ]\s*(-(?!-))*\s*)+$")
                         || patronymicName.StartsWith("-") || patronymicName.EndsWith("-")))
                    {
                        partsCopy[4] += "\t" + "[НЕВЕРЕН ФОРМАТ]";
                        errorMessage = "О,";
                        errorcomment += errorMessage;
                    }

                    for (int i = 10; i < 38; i += 2) //24 //38
                        if (!string.IsNullOrEmpty(partsCopy[i].Trim()))
                        {
                            float mark;
                            if (!float.TryParse(partsCopy[i].Replace(',', '.'),
                                                NumberStyles.Float,
                                                NumberFormatInfo.InvariantInfo,
                                                out mark) || mark < 0 || mark > 100)
                            {
                                partsCopy[i] += "\t" + "[НЕВЕРЕН ФОРМАТ]";
                                errorMessage = "БП,";
                                errorcomment += errorMessage;
                            }
                        }

                    if (!String.IsNullOrEmpty(errorMessage)) // Выдавать в отчете только ошибочные стоки.отключено.
                    {
                        for (int i = 0; i < 37; i++)
                            reportRow[i] = partsCopy[i];

                        errorMessage = errorMessage.TrimEnd(',');
                        errorcomment = errorcomment.TrimEnd(',');
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
