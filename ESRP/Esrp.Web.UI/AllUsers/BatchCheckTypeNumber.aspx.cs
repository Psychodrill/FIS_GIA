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
    public partial class BatchCheckTypeNumber : BasePage
    {
        private const string FailedUri = "/AllUsers/BatchCheckTypeNumberResult.aspx";
        private const string SuccessUri = "/AllUsers/BatchCheckTypeNumberResult.aspx?FileName={0}";
        //private const string SuccessUri = "/Certificates/CommonNationalCertificates/BatchCheckTypeNumberResult.aspx";
        // Допустимые типы содержимого файла
        private const string TextContentType = "text/plain";
        private const string CsvContentType = "application/vnd.ms-excel";
        //private const int MaxFileSize = 1024;

        // Количество частей, разделенных %, в строке пакетного файла.
        private const int FileLinePartsCount = 19;

        private DataTable ReportTable
        {
            get { return Session["ResultsListTbc"] as DataTable; }
            set { Session["ResultsListTbc"] = value; }
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
        reportTabe.Columns.Add("Типографский номер");
        reportTabe.Columns.Add("Фамилия");
        reportTabe.Columns.Add("Имя");
        reportTabe.Columns.Add("Отчество");
        reportTabe.Columns.Add("Комментарий"); //5
        reportTabe.Columns.Add("RowIndex"); 

        return reportTabe;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
          
            
            //  if (!Page.IsPostBack)
          //  {
               // Если была ошибка "Maximum request length exceeded"
               // dgUserList.DataBind();
           
         //   Page.Validate();

           // if (Page.IsValid)
        //    {
                // Преобразую данные из файла в строку
              

                //            Response.Redirect(String.Format(SuccessUri, id));
         //  }
          
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
                    string typeNumber = null;
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
                        
                        if (parts.Length != 4)
                        {
                            errorMessage = "Неверное число полей, необходимо указать 4 значения через разделитель - '%'";
                            errorcomment += errorMessage;

                            reportRow["RowIndex"] = lineIndex + " [НЕВЕРЕН ФОРМАТ]";
                            reportRow["Комментарий"] = "П";
                            ReportTable.Rows.Add(reportRow);
                        }
                        else
                        {


                        //Проверка поля ТИПОГРАФСКИЙ НОМЕР

                        typeNumber = parts[0].Trim();

                        ///note: нельзя использовать bitwise оператор single pipe (|) для формирования проверки, так как, 
                        /// например, в случае, когда typeNumber == null, пройдет проверка на null, мы получим false, и
                        /// поскольку это битовый оператор, то мы обязаны будем вычислить второе значение предиката, 
                        /// чтобы их потом сложить по-битово, но там, обращаясь к Length, мы вылетем; 
                        /// поэтому, кстати, нужно в цикле иметь try{} catch{} блок, чтобы отлавливать 
                        /// неизвестные ошибки и при этом не вываливаться отсюда, а продолжать обрабатывать
                            
                        if (string.IsNullOrEmpty(typeNumber) || (typeNumber.Length != 7))
                        {
                           parts[0] += "\t" + "[НЕВЕРЕН ФОРМАТ]";
                        
                            errorMessage = "ТН,";
                           errorcomment += errorMessage;
                        }
                        else if (!Regex.IsMatch(typeNumber, @"^[0-9]*$"))
                        {
                            parts[0] += "\t" + "[НЕВЕРЕН ФОРМАТ]";
                        
                            errorMessage = "ТН,";
                           errorcomment += errorMessage;
                        
                        }
                        
                        ///в любом случае, здесь нужно применять регулярное выражение, коль мы его уже формируем и тратим силы:
                        
                        //if (string.IsNullOrEmpty(typeNumber) || !Regex.IsMatch(typeNumber, @"^[0-9]{7}$"))
                        //{
                        //    parts[0] += "\t" + "[НЕВЕРЕН ФОРМАТ]";
                        //    errorMessage = "ТН,";
                        //    errorcomment += errorMessage;
                        //}
                         


                        //Проверка поля ФАМИЛИЯ
                        lastName = parts[1].Trim();

                        if (string.IsNullOrEmpty(lastName))             {
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
                            if (!Regex.IsMatch(patronymicName, @"^(\s*[а-яёА-ЯЁ]\s*(-(?!-))*\s*)+$")
                             || patronymicName.StartsWith("-") || patronymicName.EndsWith("-"))
                            {
                                parts[3] += "\t" + "[НЕВЕРЕН ФОРМАТ]";
                                errorMessage = "О,";
                                errorcomment += errorMessage;
                            }

                      
                       errorcomment = errorcomment.TrimEnd(',');
                       string currentLine = "";

                       if (!String.IsNullOrEmpty(errorMessage))
                            {
                                   for (int i = 0; i < 4; i++)
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



       
        

        // Проверка баллов в массиве, сформированном на основе строки из пакетного файла.


        // Получение текста ошибки.
 


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
                ShowErrorMsg(String.Format("При валидации файла произошла ошибка. Попробуйте загрузить другой файл.{0}", excp.Message.ToString()));
            }


        }

    

    }
}
