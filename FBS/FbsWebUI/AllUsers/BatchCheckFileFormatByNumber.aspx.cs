using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Fbs.Core.BatchCheck;

namespace Fbs.Web.AllUsers
{
    public partial class BatchCheckFileFormatByNumber : BasePage
    {
        private const int ResultNumber = 14;
        private const int FieldNumber = 15;
        private const int StartField = 1;
        private const int StartResultField = 7;
        private const string FailedUri = "/AllUsers/BatchCheckFileFormatByNumberResult.aspx";
        private const string SuccessUri = "/AllUsers/BatchCheckFileFormatByNumberResult.aspx?FileName={0}";        

        //DataTable ReportTable;
        private DataTable ReportTable
        {
            get { return Session["ResultsList"] as DataTable; }
            set { Session["ResultsList"] = value; }
        }


        private DataTable InitialReportTable()
        {

            DataTable reportTabe = new DataTable();

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
            reportTabe.Columns.Add("RowId"); 

            return reportTabe;
        }




        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_Error(object sender, EventArgs e)
        {
            // Получу ошибку
            HttpException checkException = Server.GetLastError() as HttpException;

            // Если ошибка "Maximum request length exceeded"
            // !! Не работает на asp.net development server
            if (checkException != null && checkException.ErrorCode == -2147467259)
            {
                Session["FileTooLarge"] = true;
                Server.ClearError();
                // TODO: странное поведение. отрефакторить.
                Response.Redirect(FailedUri, true);
            }
        }

        private string ConvertNumber(string[] parts)
        {
            return parts[0].Trim();
        }

        private bool Parse()
        {
            ReportTable = this.InitialReportTable();
            DataRow reportRow;

            Stream batchStream = null;
            BatchCheckFilter filter = null;

            DataTable dataTable = this.InitialReportTable();

            using (var reader = new StreamReader(fuData.FileContent, Encoding.GetEncoding(1251), true))
            {
                string firstRow = reader.ReadLine();
                batchStream = new MemoryStream();
                StreamWriter writer = new StreamWriter(batchStream, Encoding.GetEncoding(1251));
                if (firstRow.StartsWith(BatchCheckFormat.FILTER_TOKEN))
                {
                    filter = BatchCheckFilter.Parse(firstRow, ref dataTable);
                    if (dataTable.Rows.Count > 0)
                    {
                        ReportTable = dataTable;
                        return false;
                    }
                    batchStream = new BatchCheckPreprocessor(filter).Process(reader.BaseStream);
                }
                else
                {
                    writer.WriteLine(firstRow);
                }
                writer.Write(reader.ReadToEnd());
                writer.Flush();
            }
            batchStream.Position = 0;
            using (StreamReader reader = new StreamReader(batchStream, Encoding.GetEncoding(1251), true))
            {
                string line;
                int lineIndex = 0;
                string errorMessage;
                string errorcomment = string.Empty;
                string number = null;                

                while ((line = reader.ReadLine()) != null)
                {
                    reportRow = ReportTable.NewRow();
                    line = line.Trim();

                    errorcomment = "";
                    lineIndex++;

                    if (lineIndex > Config.MaxBatchCheckLines)
                    {
                        reportRow["RowIndex"] = string.Format("[Максимально разрешенное количество строк = {0}]", Config.MaxBatchCheckLines);
                        reportRow["RowId"] = lineIndex;
                        reportRow["Комментарий"] = "C";
                        this.ReportTable.Rows.Add(reportRow);
                        break;
                    }

                    if (string.IsNullOrEmpty(line))
                        continue;
                    string[] parts = line.Split('%');
                    string[] partsCopy = new string[40]; ;

                    if (parts.Length != FieldNumber)
                    {
                        errorMessage = string.Format("Неверное число полей, необходимо указать {0} значений через разделитель - '%'", FieldNumber);
                        errorcomment += errorMessage;
                        partsCopy[0] = errorMessage;
                        reportRow["RowIndex"] = lineIndex + " [НЕВЕРЕН ФОРМАТ]";
                        reportRow["RowId"] = lineIndex;
                        reportRow["Комментарий"] = "П";
                        ReportTable.Rows.Add(reportRow);

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
                        number = ConvertNumber(partsCopy);
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
                        if (ResultNumber-resultNullCount <2)
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
                            for (int i = StartResultField; i < 34; i++)
                            {
                                reportRow[j] = partsCopy[i];
                                j++;
                            }

                            errorMessage = errorMessage.TrimEnd(',');
                            errorcomment = errorcomment.TrimEnd(',');
                            reportRow["RowIndex"] = lineIndex;
                            reportRow["RowId"] = lineIndex;
                            reportRow["Комментарий"] = errorcomment;
                            ReportTable.Rows.Add(reportRow);
                        }
                    }
                }

            }
            return true;
        }




        public void ShowErrorMsg(string errorMsg)
        {

            FileErrorMsg.Visible = true;


            FileErrorMsg.Text = errorMsg;
            resultLbl.Visible = false;

        }

        protected void StartParseBtn_click(object sender, EventArgs e)
        {
            try
            {

                resultLbl.Visible = false;
                FileErrorMsg.Visible = false;

                //            BatchCheckResultSubjectNoticePnl.Visible = false;

                if (fuData.HasFile)
                {


                    if (fuData.PostedFile.ContentLength > 1048576)
                    {
                        ShowErrorMsg("Размер файла превышает максимально допустимый (1 Мб). Пожалуйста, уменьшите размер файла. Например, загрузите последовательно несколько файлов со сведениями о свидетельствах");
                    }
                    else if (!fuData.FileName.EndsWith(".csv"))
                    {
                        ShowErrorMsg("Неверный формат файла: необходимо передать текстовый файл в формате csv.");
                    }

                    else
                    {


                        // string fileName = Guid.NewGuid().ToString();

                        // Начинаем парсинг файла данные в бд

                        Parse();


                        resultLbl.Visible = true;

                        if (ReportTable != null && ReportTable.Rows.Count == 0)
                        {


                            resultLbl.Text = "Проверка выполнена успешно!";
                            return;
                        }
                        string fileName = Guid.NewGuid().ToString();
                        Session[fileName] = ReportTable;
                        Response.Redirect(String.Format(SuccessUri, fileName), true);

                    }
                }
                else
                {

                    ShowErrorMsg("Вы загрузили пустой файл. Пожалуйста, укажите сведения хотя бы об одном свидетельстве о результатах ЕГЭ!");
                }

            }
            catch (Exception excp)
            {
                ShowErrorMsg(String.Format("При валидации файла произошла ошибка. Попробуйте загрузить другой файл.{0}", excp.Message.ToString()));
            }


        }
    }
}