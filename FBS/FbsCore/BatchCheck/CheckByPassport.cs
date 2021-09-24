using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;
using System.IO;
using Fbs.Core.Shared;

namespace Fbs.Core.BatchCheck
{
    public class BatchCheckByPassport
    {
        #region Public Methods

        public bool Parse(StreamReader reader, string userGroup, ref DataTable resultData)
        {
            DataTable reportTable = this.InitialReportTable().Clone();
            bool isCorrectBatch = true;

            string line;
            int lineIndex = 0;
            string errorMessage;
            string lastName = null;
            string firstName = null;
            string patronymicName = null;
            string passportSeria = null;
            string passportNumber = null;
            DataRow reportRow;
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
                if (parts.Length != 5)
                {
                    errorMessage = "Неверное число полей, необходимо указать 5 значений через разделитель - '%'";
                    errorcomment += errorMessage;
                    parts[0] = errorMessage;
                    //reportRow[0] = parts[0];
                    reportRow["RowIndex"] = lineIndex + " [НЕВЕРЕН ФОРМАТ]";
                    reportRow["Комментарий"] = "П";
                    reportTable.Rows.Add(reportRow);
                    isCorrectBatch = false;
                } 
                else 
                {
                    // Проверка поля ФАМИЛИЯ
                    lastName = parts[0].Trim();

                    if (string.IsNullOrEmpty(lastName))
                    {
                        parts[0] += "\t" + "[НЕВЕРЕН ФОРМАТ]";
                        //errorMessage = "Поле Фамилия должно быть заполнено и должно содержать только русские буквы.";
                        errorMessage = "Ф,";
                        errorcomment += errorMessage;
                    }
                    else if (!Regex.IsMatch(lastName, @"^(\s*[а-яёА-ЯЁ]\s*(-(?!-))*\s*)+$") || lastName.StartsWith("-") || lastName.EndsWith("-"))
                    {
                        parts[0] += "\t" + "[НЕВЕРЕН ФОРМАТ]";
                        errorMessage = "Ф,";
                        //errorMessage = "Поле Фамилия должно быть заполнено и должно содержать только русские буквы.";
                        errorcomment += errorMessage;
                    }

                    // Проверка поля ИМЯ
                    firstName = parts[1].Trim();
                    if (!string.IsNullOrEmpty(firstName))
                        if (!Regex.IsMatch(firstName, @"^(\s*[а-яёА-ЯЁ]\s*(-(?!-))*\s*)+$") || firstName.StartsWith("-") || firstName.EndsWith("-"))
                        {
                            parts[1] += "\t" + "[НЕВЕРЕН ФОРМАТ]";
                            //errorMessage = "Поле ИМЯ должно быть заполнено и должно содержать только русские буквы.";
                            errorMessage = "И,";
                            errorcomment += errorMessage;
                        }

                    // Проверка поля ОТЧЕСТВО
                    patronymicName = parts[2].Trim();
                    if (!string.IsNullOrEmpty(patronymicName))
                    {
                        if (!Regex.IsMatch(patronymicName, @"^(\s*[а-яёА-ЯЁ]\s*(-(?!-))*\s*)+$") || patronymicName.StartsWith("-") || patronymicName.EndsWith("-"))
                        {
                            parts[2] += "\t" + "[НЕВЕРЕН ФОРМАТ]";
                            //errorMessage = "Поле ИМЯ должно быть заполнено и должно содержать только русские буквы.";
                            errorMessage = "О,";
                            errorcomment += errorMessage;
                        }
                    }

                    // Проверка поля СЕРИЯ ПАСПОРТА
                    passportSeria = parts[3].Trim();
                    List<string> sErrors = DocumentCheck.DocSeriesCheck(passportSeria);
                    if (sErrors.Count > 0)
                    {
                        parts[3] += "\t" + "[НЕВЕРЕН ФОРМАТ]";
                        errorMessage = "СП,";
                        errorcomment += errorMessage;
                    }

                    // Проверка поля НОМЕР ПАСПОРТА
                    passportNumber = parts[4].Trim();
                    List<string> nErrors = DocumentCheck.DocNumberCheck(passportNumber);
                    if (nErrors.Count > 0 || string.IsNullOrEmpty(passportNumber))
                    {
                        parts[4] += "\t" + "[НЕВЕРЕН ФОРМАТ]";
                        errorMessage = "НП";
                        errorcomment += errorMessage;
                    }

                    errorcomment = errorcomment.TrimEnd(',');
                    string currentLine = "";

                    if (!String.IsNullOrEmpty(errorMessage)) // Выдавать в отчете только ошибочные стоки.отключено.
                    {
                        //for (int i = 0; i < 5; i++)
                        //{
                        reportRow["Фамилия"] = parts[0];
                        reportRow["Имя"] = parts[1];
                        reportRow["Отчество"] = parts[2];
                        reportRow["Серия паспорта"] = parts[3];
                        reportRow["Номер паспорта"] = parts[4];

                        //}
                        currentLine = currentLine + "\t\t" + errorcomment;
                        reportRow["RowIndex"] = lineIndex;
                        reportRow["Комментарий"] = errorcomment;
                        reportTable.Rows.Add(reportRow);
                        isCorrectBatch = false;
                    }
                }
            }

            resultData = reportTable;
            reportTable = null;

            return isCorrectBatch;
        }

        #endregion


        #region Private Methods

        private DataTable InitialReportTable()
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
