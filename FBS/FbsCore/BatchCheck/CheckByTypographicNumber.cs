using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;

namespace Fbs.Core.BatchCheck
{
    public class BatchCheckByTypographicNumber
    {
        #region Public Methods

        public bool Parse(StreamReader reader, string userGroup, ref DataTable resultData)
        {
            DataTable reportTable = InitialReportTable().Clone();
            DataRow reportRow;
            bool isCorrectBatch = true;

            string line;
            int lineIndex = 0;
            string errorMessage;
            string lastName = null;
            string firstName = null;
            string patronymicName = null;
            string typeNumber = null;
            string errorcomment;

            while ((line = reader.ReadLine()) != null)
            {
                if (userGroup != "UserRCOI" && reportTable.Rows.Count >= 30)
                {
                    break;
                }
                reportRow = reportTable.NewRow();

                line = line.Trim();

                errorcomment = "";
                lineIndex++;

                if (string.IsNullOrEmpty(line))
                    continue;
                string[] parts = line.Split('%');
                errorMessage = "";

                if (parts.Length != 4)
                {
                    errorMessage = "Неверное число полей, необходимо указать 4 значения через разделитель - '%'";
                    errorcomment += errorMessage;

                    reportRow["RowIndex"] = lineIndex + " [НЕВЕРЕН ФОРМАТ]";
                    reportRow["Комментарий"] = "П";
                    reportTable.Rows.Add(reportRow);
                    isCorrectBatch = false;
                }
                else
                {


                    //Проверка поля ТИПОГРАФСКИЙ НОМЕР
                    typeNumber = parts[0].Trim();
                    if (string.IsNullOrEmpty(typeNumber))// || (typeNumber.Length!=7))
                    {
                        parts[0] += "\t" + "[НЕВЕРЕН ФОРМАТ]";

                        errorMessage = "ТН,";
                        errorcomment += errorMessage;
                    }
                    else if (!Regex.IsMatch(typeNumber, @"^[0-9]{7,8}$"))
                    {
                        parts[0] += "\t" + "[НЕВЕРЕН ФОРМАТ]";

                        errorMessage = "ТН,";
                        errorcomment += errorMessage;

                    }

                    //Проверка поля ФАМИЛИЯ
                    lastName = parts[1].Trim();

                    if (string.IsNullOrEmpty(lastName))
                    {
                        parts[1] += "\t" + "[НЕВЕРЕН ФОРМАТ]";
                        errorMessage = "Ф,";
                        errorcomment += errorMessage;
                    }

                    else if (!Regex.IsMatch(lastName, @"^(\s*[а-яёА-ЯЁ]\s*(-(?!-))*\s*)+$") || lastName.StartsWith("-") || lastName.EndsWith("-"))
                    {
                        parts[1] += "\t" + "[НЕВЕРЕН ФОРМАТ]";
                        errorMessage = "Ф,";
                        errorcomment += errorMessage;
                    }

                    //Проверка поля ИМЯ
                    firstName = parts[2].Trim();
                    if (!string.IsNullOrEmpty(firstName))
                        if (!Regex.IsMatch(firstName, @"^(\s*[а-яёА-ЯЁ]\s*(-(?!-))*\s*)+$") || firstName.StartsWith("-") || firstName.EndsWith("-"))
                        {
                            parts[2] += "\t" + "[НЕВЕРЕН ФОРМАТ]";
                            errorMessage = "И,";
                            errorcomment += errorMessage;
                        }

                    //Проверка поля ОТЧЕСТВО
                    patronymicName = parts[3].Trim();
                    if (!string.IsNullOrEmpty(patronymicName))
                        if (!Regex.IsMatch(patronymicName, @"^(\s*[а-яёА-ЯЁ]\s*(-(?!-))*\s*)+$") || patronymicName.StartsWith("-") || patronymicName.EndsWith("-"))
                        {
                            parts[3] += "\t" + "[НЕВЕРЕН ФОРМАТ]";
                            errorMessage = "О,";
                            errorcomment += errorMessage;
                        }


                    errorcomment = errorcomment.TrimEnd(',');
                    string currentLine = "";

                    if (!String.IsNullOrEmpty(errorMessage))
                    {
                        reportRow["Типографский номер"] = parts[0];
                        reportRow["Фамилия"] = parts[1];
                        reportRow["Имя"] = parts[2];
                        reportRow["Отчество"] = parts[3];

                        currentLine = currentLine + "\t\t" + errorcomment;

                        reportRow["RowIndex"] = lineIndex;
                        reportRow["Комментарий"] = errorcomment;
                        isCorrectBatch = false;

                        reportTable.Rows.Add(reportRow);
                    }
                }
            }
            resultData = reportTable;
            reportTable = null;
            return isCorrectBatch;
        }

        #endregion


        #region Private Methods

        private static DataTable InitialReportTable()
        {
            DataTable reportTabe = new DataTable();

            reportTabe.Columns.Add("Номер свидетельства");
            reportTabe.Columns.Add("Типографский номер");
            reportTabe.Columns.Add("Фамилия");
            reportTabe.Columns.Add("Имя");
            reportTabe.Columns.Add("Отчество");
            reportTabe.Columns.Add("Серия паспорта");
            reportTabe.Columns.Add("Номер паспорта");
            reportTabe.Columns.Add("Регион");
            reportTabe.Columns.Add("Год");
            reportTabe.Columns.Add("Статус");// 10.

            reportTabe.Columns.Add("Русский язык");
            reportTabe.Columns.Add("Апелляция по русскому языку");

            reportTabe.Columns.Add("Математика");
            reportTabe.Columns.Add("Апелляция по математике");
            reportTabe.Columns.Add("Физика");
            reportTabe.Columns.Add("Апелляция по физике");
            reportTabe.Columns.Add("Химия");
            reportTabe.Columns.Add("Апелляция по химии");
            reportTabe.Columns.Add("Биология");
            reportTabe.Columns.Add("Апелляция по биологии"); //20

            reportTabe.Columns.Add("История России");
            reportTabe.Columns.Add("Апелляция по истории России");
            reportTabe.Columns.Add("География");
            reportTabe.Columns.Add("Апелляция по географии");
            reportTabe.Columns.Add("Английский язык");
            reportTabe.Columns.Add("Апелляция по английскому языку");
            reportTabe.Columns.Add("Немецкий язык");
            reportTabe.Columns.Add("Апелляция по немецкому языку");
            reportTabe.Columns.Add("Французский язык");
            reportTabe.Columns.Add("Апелляция по французскому языку"); //30

            reportTabe.Columns.Add("Обществознание");
            reportTabe.Columns.Add("Апелляция по обществознанию");
            reportTabe.Columns.Add("Литература");
            reportTabe.Columns.Add("Апелляция по литературе");
            reportTabe.Columns.Add("Испанский язык");
            reportTabe.Columns.Add("Апелляция по испанскому языку");
            reportTabe.Columns.Add("Информатика");
            reportTabe.Columns.Add("Апелляция по информатике");
            reportTabe.Columns.Add("RowIndex"); //39
            reportTabe.Columns.Add("Комментарий"); //39

            return reportTabe;
        }

        #endregion
    }
}
