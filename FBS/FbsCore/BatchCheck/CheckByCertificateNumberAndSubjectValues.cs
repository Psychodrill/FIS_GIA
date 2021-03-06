using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace Fbs.Core.BatchCheck
{
    using System.Configuration;

    public class CheckByCertificateNumberAndSubjectValues
    {
        private const int ResultNumber = 14;
        private const int FieldNumber = 15;
        private const int StartField = 1;
        private const int StartResultField = 7;
        #region Public Methods

        public bool Parse(StreamReader filereader, string userGroup, ref DataTable resultTable)
        {
            DataTable reportTable = InitialReportTable().Clone();
            bool isCorrectBatch = true;
            string line;
            int lineIndex = 0;
            string errorMessage;

            string maxLinesConfig = ConfigurationManager.AppSettings["maxBatchCheckLines"];
            int maxLines;
            if (string.IsNullOrEmpty(maxLinesConfig) || !int.TryParse(maxLinesConfig, out maxLines))
            {
                maxLines = 10000;
            }

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

                if (lineIndex > maxLines)
                {
                    reportRow["RowIndex"] = string.Format("[Максимально разрешенное количество строк = {0}]", maxLines);
                    reportRow["Комментарий"] = "C";
                    reportTable.Rows.Add(reportRow);
                    isCorrectBatch = false;
                    break;
                }

                if (string.IsNullOrEmpty(line))
                    continue;
                string[] parts = line.Split('%');
                var partsCopy = new string[40];

                if (parts.Length != FieldNumber)
                {
                    errorMessage = string.Format("Неверное число полей, необходимо указать {0} значений через разделитель - '%'", FieldNumber);
                    errorcomment += errorMessage;
                    partsCopy[0] = errorMessage;
                    reportRow["RowIndex"] = lineIndex + " [НЕВЕРЕН ФОРМАТ]";
                    reportRow["Комментарий"] = "П";
                    reportTable.Rows.Add(reportRow);
                    isCorrectBatch = false;
                }
                else
                {                 
                    int j = 7;
                    partsCopy[0] = parts[0];


                    for (int i = StartField; i < FieldNumber; i++) //12
                    {
                        partsCopy[j] = parts[i];
                        j += 2;
                    }

                    errorMessage = null;

                    //Проверка поля номер свидетельства
                    var number = partsCopy[0].Trim();
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

                    var resultNullCount = 0;
                    for (int i = StartResultField; i < 35; i += 2)
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
                                if (!errorcomment.Contains("БП"))
                                    errorcomment += errorMessage;
                            }
                        }
                        else
                        {
                            resultNullCount++;
                        }
                    if (ResultNumber - resultNullCount < 2)
                    {
                        errorMessage = "БП,";
                        if (!errorcomment.Contains("БП"))
                            errorcomment += errorMessage;
                    }

                    if (!String.IsNullOrEmpty(errorMessage))
                    {

                        for (int i = 0; i < StartResultField; i++)
                        {
                            reportRow[i] = partsCopy[i];
                        }
                        j = StartResultField;
                        for (int i = StartResultField; i < 34; i ++)
                        {
                            reportRow[j] = partsCopy[i];
                            j++;
                        }

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

            return isCorrectBatch;
        }

        #endregion


        #region Private Methods

        private static DataTable InitialReportTable()
        {
            var reportTabe = new DataTable();

            reportTabe.Columns.Add("Номер свидетельства");
            reportTabe.Columns.Add("Типографский номер");
            reportTabe.Columns.Add("Серия паспорта");
            reportTabe.Columns.Add("Номер паспорта");
            reportTabe.Columns.Add("Регион");
            reportTabe.Columns.Add("Год");
            reportTabe.Columns.Add("Статус");
            reportTabe.Columns.Add("Русский язык");
            reportTabe.Columns.Add("Апелляция по русскому языку");
            reportTabe.Columns.Add("Математика");
            reportTabe.Columns.Add("Апелляция по математике");
            reportTabe.Columns.Add("Физика");
            reportTabe.Columns.Add("Апелляция по физике");
            reportTabe.Columns.Add("Химия");
            reportTabe.Columns.Add("Апелляция по химии");
            reportTabe.Columns.Add("Биология");
            reportTabe.Columns.Add("Апелляция по биологии");
            reportTabe.Columns.Add("История России");
            reportTabe.Columns.Add("Апелляция по истории России");
            reportTabe.Columns.Add("География");
            reportTabe.Columns.Add("Апелляция по географии");
            reportTabe.Columns.Add("Английский язык");
            reportTabe.Columns.Add("Апелляция по английскому языку");
            reportTabe.Columns.Add("Немецкий язык");
            reportTabe.Columns.Add("Апелляция по немецкому языку");
            reportTabe.Columns.Add("Французский язык");
            reportTabe.Columns.Add("Апелляция по французскому языку");
            reportTabe.Columns.Add("Обществознание");
            reportTabe.Columns.Add("Апелляция по обществознанию");
            reportTabe.Columns.Add("Литература");
            reportTabe.Columns.Add("Апелляция по литературе");
            reportTabe.Columns.Add("Испанский язык");
            reportTabe.Columns.Add("Апелляция по испанскому языку");
            reportTabe.Columns.Add("Информатика");
            reportTabe.Columns.Add("Апелляция по информатике");
            reportTabe.Columns.Add("RowIndex");
            reportTabe.Columns.Add("Комментарий"); 

            return reportTabe;
        }

        #endregion
    }
}
