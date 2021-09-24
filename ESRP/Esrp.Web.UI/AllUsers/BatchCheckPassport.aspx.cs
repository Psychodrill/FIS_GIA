using System;
using System.IO;
using System.Web;
using System.Data;
using System.Text;
using Esrp.Core;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

using System.Configuration;


namespace Esrp.Web.Certificates.CommonNationalCertificates
{
    public partial class BatchCheckPassport : BasePage
    {
        private const string FailedUri = "/AllUsers/BatchCheckPassport.aspx";
        private const string SuccessUri = "/AllUsers/BatchCheckPassportResult.aspx?FileName={0}";
       // private const string SuccessUri = "/Certificates/CommonNationalCertificates/BatchCheckPassportResult.aspx";
        // Допустимые типы содержимого файла
        private const string TextContentType = "text/plain";
        private const string CsvContentType = "application/vnd.ms-excel";
        //private const int MaxFileSize = 1024;

        // Количество частей, разделенных %, в строке пакетного файла.
        private const int FileLinePartsCount = 19;
        //private string SuccessUri;
        private DataTable ReportTable
        {
            get { return Session["ResultsListPbc"] as DataTable; }
            set { Session["ResultsListPbc"] = value; }
        }

        

        private string[] mFileLines;

        private string[] FileLines
        {
            get
            {
                if (mFileLines == null)
                {
                    mFileLines = Encoding.Default.GetString(fuData.FileBytes).Split(
                        new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < mFileLines.Length; i++)
                        mFileLines[i] = mFileLines[i].Trim();
                }
                return mFileLines;
            }
        }

        private DataTable InitialReportTable()
        { 
        
        DataTable reportTabe = new DataTable();

        reportTabe.Columns.Add("Фамилия");
        reportTabe.Columns.Add("Имя");
        reportTabe.Columns.Add("Отчество");
        reportTabe.Columns.Add("Серия паспорта");
        reportTabe.Columns.Add("Номер паспорта");
        reportTabe.Columns.Add("Комментарий"); //6
        reportTabe.Columns.Add("RowIndex"); 
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

              #region Проверка формата файла



        private bool Parse()
        {
            ReportTable = this.InitialReportTable();
            DataRow reportRow;
            using (StreamReader reader = new StreamReader(fuData.FileContent, Encoding.GetEncoding(1251)))
            {
                    string line;
                    int lineIndex = 0;
                    string errorMessage;
                    string lastName = null;
                    string firstName = null;
                    string patronymicName = null;
                    string passportSeria = null;
                    string passportNumber = null;
                    string errorcomment;
                    while ((line = reader.ReadLine()) != null)
                    {
                        reportRow = ReportTable.NewRow();

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
                            ReportTable.Rows.Add(reportRow);
                        }
                        else
                        {



                            //Проверка поля ФАМИЛИЯ
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

                            //Проверка поля ИМЯ
                            firstName = parts[1].Trim();
                            if (!string.IsNullOrEmpty(firstName))
                                if (!Regex.IsMatch(firstName, @"^(\s*[а-яёА-ЯЁ]\s*(-(?!-))*\s*)+$") || firstName.StartsWith("-") || firstName.EndsWith("-"))
                                {
                                    parts[1] += "\t" + "[НЕВЕРЕН ФОРМАТ]";
                                    //errorMessage = "Поле ИМЯ должно быть заполнено и должно содержать только русские буквы.";
                                    errorMessage = "И,";
                                    errorcomment += errorMessage;
                                }

                            //Проверка поля ОТЧЕСТВО

                            patronymicName = parts[2].Trim();

                            if (!string.IsNullOrEmpty(patronymicName))
                                if (!Regex.IsMatch(patronymicName, @"^(\s*[а-яёА-ЯЁ]\s*(-(?!-))*\s*)+$") || patronymicName.StartsWith("-") || patronymicName.EndsWith("-"))
                                {
                                    parts[2] += "\t" + "[НЕВЕРЕН ФОРМАТ]";
                                    //errorMessage = "Поле ИМЯ должно быть заполнено и должно содержать только русские буквы.";
                                    errorMessage = "О,";
                                    errorcomment += errorMessage;
                                }

                            passportSeria = parts[3].Trim();

                            if (string.IsNullOrEmpty(passportSeria))
                            {
                                parts[3] += "\t" + "[НЕВЕРЕН ФОРМАТ]";
                                errorMessage = "СП,";
                                errorcomment += errorMessage;
                            }


                            passportNumber = parts[4].Trim();

                            if (string.IsNullOrEmpty(passportNumber))
                            {
                                parts[4] += "\t" + "[НЕВЕРЕН ФОРМАТ]";
                                errorMessage = "НП";
                                errorcomment += errorMessage;
                            }

                            errorcomment = errorcomment.TrimEnd(',');
                            string currentLine = "";

                            if (!String.IsNullOrEmpty(errorMessage)) // Выдавать в отчете только ошибочные стоки.отключено.
                            {

                                for (int i = 0; i < 5; i++)
                                {
                                    reportRow[i] = parts[i];
                                    currentLine += parts[i];
                                    currentLine += "%";
                                }
                                currentLine = currentLine + "\t\t" + errorcomment;
                                reportRow["RowIndex"] = lineIndex;
                                reportRow["Комментарий"] = errorcomment;
                                ReportTable.Rows.Add(reportRow);
                        
                            }
                        }
                            
                    }



                 
                }
                return true;
        }


      

        #endregion

  



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
                ShowErrorMsg(String.Format("При валидации файла произошла ошибка. Попробуйте загрузить другой файл.{0}", excp.Message));
            }


        }

    

    }
}
