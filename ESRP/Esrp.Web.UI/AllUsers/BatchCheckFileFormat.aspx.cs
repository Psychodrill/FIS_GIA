using System;
using System.Globalization;
using System.IO;
using System.Web;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;


namespace Esrp.Web.AllUsers
{
    public partial class BatchCheckFileFormat : BasePage
    {
        private const string FailedUri = "/AllUsers/BatchCheckFileFormatResult.aspx";
        private const string SuccessUri = "/AllUsers/BatchCheckFileFormatResult.aspx?FileName={0}";
        // Допустимые типы содержимого файла
        private const string TextContentType = "text/plain";
        private const string CsvContentType = "application/vnd.ms-excel";
        //private const int MaxFileSize = 1024;

        // Количество частей, разделенных %, в строке пакетного файла.
        private const int FileLinePartsCount = 19;

        //DataTable ReportTable;
        private DataTable ReportTable
        {
            get { return Session["ResultsList"] as DataTable; }
            set { Session["ResultsList"] = value; }
        }
        
      //  private string SuccessUri;

        private string[] mFileLines;

        private bool ScrollToResults
        {
            get
            {
                if (Session["ScrollToResults"] == null)
                    return false;
                else
                    return (bool)Session["ScrollToResults"];
            }
            set { Session["ScrollToResults"] = value; }
        }


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
      //  private FileStream SourceFileStream = null;

        private DataTable InitialReportTable()
        { 
        
            DataTable reportTabe = new DataTable();

            reportTabe.Columns.Add("Номер свидетельства");
            reportTabe.Columns.Add("Типографский номер");
            // reportTabe.Columns.Add("Оригинал");
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


     

        protected void Page_Load(object sender, EventArgs e)
        {
            //if ()
            //{



            //try
            //{
            //    Session["ResultsListTbc"] = null;
            //    Session["ResultsListPbc"] = null;

            //    if (ReportTable != null)
            //    {
            //        dgResultsList.DataSource = ReportTable;
            //        dgResultsList.DataBind();
            //    }

            //}
            //catch (Exception)
            //{
            //    ReportTable = null;
            //    dgResultsList.DataSource = null;
            //    dgResultsList.Visible = false;
                
            //}
            
            
            //if (ScrollToResults)
            //{
            //   Response.Redirect("/AllUsers/BatchCheckFileFormat.aspx#resultshook");
            //    ScrollToResults = false;

            //}

            //    Response.Redirect(String.Format("{0}?#resultshook",CurrentUrl));
            //}   

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

        private string ConvertNumber(string[] parts)
        {
            return parts[0].Trim();
        }
     
        private bool Parse()
        {
            //SourceFileStream = File.Open(Server.MapPath(String.Format("/Shared/tmp/{0}.csv", FileName)), FileMode.Open, FileAccess.Read, FileShare.None);
            ReportTable = this.InitialReportTable();
            DataRow reportRow;
            using (StreamReader reader = new StreamReader(fuData.FileContent, Encoding.GetEncoding(1251)))
            {
                string line;
                int lineIndex = 0;
                string errorMessage;
                string number = null;
                string lastName = null;
                string firstName = null;
                string patronymicName = null;
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
                    string[] partsCopy = new string[40]; ;

                    if (parts.Length != 18)
                    {
                        errorMessage = "Неверное число полей, необходимо указать 18 значений через разделитель - '%'";
                        errorcomment += errorMessage;
                        partsCopy[0] = errorMessage;
                        reportRow["RowIndex"] = lineIndex + " [НЕВЕРЕН ФОРМАТ]";
                        reportRow["Комментарий"] = "П";
                        ReportTable.Rows.Add(reportRow);

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
                        //Проверка поля ФАМИЛИЯ
                        lastName = partsCopy[2].Trim();

                        if (string.IsNullOrEmpty(lastName))             {
                            partsCopy[2] += "\t" + "[НЕВЕРЕН ФОРМАТ]";
                            errorMessage = "Ф,";
                            errorcomment += errorMessage;
                        }

                        else if (!Regex.IsMatch(lastName, @"^(\s*[а-яёА-ЯЁ]\s*(-(?!-))*\s*)+$") || lastName.StartsWith("-") || lastName.EndsWith("-"))
                        {
                            partsCopy[2] += "\t" + "[НЕВЕРЕН ФОРМАТ]";
                            errorMessage = "Ф,";
                            errorcomment += errorMessage;
                        }

                        //Проверка поля ИМЯ
                        firstName = partsCopy[3].Trim();
                        if (!string.IsNullOrEmpty(firstName))
                            if (!Regex.IsMatch(firstName, @"^(\s*[а-яёА-ЯЁ]\s*(-(?!-))*\s*)+$") || firstName.StartsWith("-") || firstName.EndsWith("-"))
                            {
                                partsCopy[3] += "\t" + "[НЕВЕРЕН ФОРМАТ]";
                                errorMessage = "И,";
                                errorcomment += errorMessage;
                            }

                        //Проверка поля ОТЧЕСТВО

                        patronymicName = partsCopy[4].Trim();
                        if (!string.IsNullOrEmpty(patronymicName))
                            if (!Regex.IsMatch(patronymicName, @"^(\s*[а-яёА-ЯЁ]\s*(-(?!-))*\s*)+$") || patronymicName.StartsWith("-") || patronymicName.EndsWith("-"))
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
                            {
                                reportRow[i] = partsCopy[i];
                            }

                            errorMessage = errorMessage.TrimEnd(',');
                            errorcomment = errorcomment.TrimEnd(',');
                            reportRow["RowIndex"] = lineIndex;
                            reportRow["Комментарий"] = errorcomment;
                            ReportTable.Rows.Add(reportRow);
                        }
                    }
                }
          

               //dgResultsList.DataSource = ReportTable;
               //dgResultsList.DataBind();
             
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
                ShowErrorMsg(String.Format("При валидации файла произошла ошибка. Попробуйте загрузить другой файл.{0}",excp.Message.ToString()));
            }
            
            
        }

    }

}