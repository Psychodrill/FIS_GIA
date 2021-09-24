namespace Fbs.Web.Certificates.CommonNationalCertificates
{
    using System;
    using System.Data;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.UI.WebControls;

    using Fbs.Core;
    using Fbs.Core.BatchCheck;

    using FbsServices;

    public partial class BatchRequestByPassport : BasePage, IHistoryNavigator
    {
        #region Constants and Fields

        private readonly CNECService cnecService = new CNECService();

        #endregion

        private const string FailedUri = "/Certificates/CommonNationalCertificates/BatchRequestByPassport.aspx";
        private const string SuccessUri = "/Certificates/CommonNationalCertificates/BatchRequestByPassportResultObsolete.aspx?batchId={0}";

        // Количество частей, разделенных %, в строке пакетного файла.
        private const int FileLinePartsCount = 5;

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

        private DataTable ResultData
        {
            get { return Session["ResultsList"] as DataTable; }
            set { Session["ResultsList"] = value; }
        }

        /*
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
         */


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                // Если была ошибка "Maximum request length exceeded"
                if (Session["FileTooLarge"] != null)
                {
                    cvFileTooLarge.IsValid = false;
                    cvFileTooLarge.ErrorMessage = string.Format(cvFileTooLarge.ErrorMessage,
                            Utility.FormatKbToMb(Config.MaxRequestLength, "0.00"));
                    Session.Remove("FileTooLarge");
                }
                return;
            }
            dgReportResultsList.DataSource = ResultData;
            dgReportResultsList.DataBind();

            //Page.Validate();
            //if (Page.IsValid)
            //{
            //    // Преобразую данные из файла в строку
            //    string data = Encoding.Default.GetString(fuData.FileBytes);

            //    // Добавлю данные в бд
            //    long id = CommonNationalCertificateContext.UpdateRequestBatch(data, false);

            //    Response.Redirect(String.Format(SuccessUri, id));
            //}
        }
        protected void dgReportResultsList_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            dgReportResultsList.CurrentPageIndex = e.NewPageIndex;
            dgReportResultsList.DataBind();
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

        protected void cvFileTooLarge_ServerValidate(object source, ServerValidateEventArgs args)
        {
            // Временно убрана проверка на размер файла. Закомментировал на будущее.
            //if (fuData.PostedFile.ContentLength > MaxFileSize)
            //{
            //    args.IsValid = false;
            //    cvFileTooLarge.ErrorMessage = string.Format(cvFileTooLarge.ErrorMessage,
            //                Utility.FormatKbToMb(MaxFileSize, "0.00"));
            //} 
            //  args.IsValid = false;
        }

        protected void cvFileEmpty_ServerValidate(object source, ServerValidateEventArgs args)
        {
            // Проверю размер загружаемого файла.
            // args.IsValid = fuData.PostedFile.ContentLength > 0 && FileLines.Length > 0;
            //  args.IsValid = true;
        }

        protected void cvFileFormat_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //if (FileLines.Length == 0)
            //    return;

            //int errorLine;
            //FileFormatErrors error = CheckFileFormat(out errorLine);

            //if (error != FileFormatErrors.None)
            //{
            //    args.IsValid = false;
            //    cvFileFormat.ErrorMessage = GetFileFormatErrorMsg(error, errorLine);
            //}
            //     args.IsValid = true;
        }


        /*

        private bool Parse(StreamReader reader)
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

                            //for (int i = 0; i < 5; i++)
                            //{
                               

                               reportRow["Фамилия"] =  parts[0];
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


                this.ResultData = reportTable;
                reportTable = null;

            return isCorrectBatch;
        }
         */

        public void ShowErrorMsg(string errorMsg)
        {
            FileErrorMsg.Visible = true;
            FileErrorMsg.Text = errorMsg;
        }
        public void StartParseBtn_Click(object sender, EventArgs e)
        {
            try
            {
                dgReportResultsList.DataSource = null;
                dgReportResultsList.DataBind();

                FileErrorMsg.Visible = false;
                if (fuData.HasFile)
                {
                    // Преобразую данные из файла в строку
                    if (fuData.PostedFile.ContentLength > 1048576)
                    {
                        ShowErrorMsg("Размер файла превышает максимально допустимый (1 Мб). Пожалуйста, уменьшите размер файла. Например, загрузите последовательно несколько файлов со сведениями о свидетельствах");

                        return;
                    }
                    else if (!fuData.FileName.EndsWith(".csv"))
                    {
                        //fuData.
                        ShowErrorMsg("Неверный формат файла: необходимо передать текстовый файл в формате csv.");
                        return;
                    }

                    Stream batchStream = null;
                    BatchCheckFilter filter = null;

                    string userGroup = Account.GetGroup(CurrentUserName);

                    using (var reader = new StreamReader(fuData.FileContent, Encoding.GetEncoding(1251), true))
                    {
                        string firstRow = reader.ReadLine();
                        batchStream = new MemoryStream();
                        StreamWriter writer = new StreamWriter(batchStream, Encoding.GetEncoding(1251));
                        if (firstRow.StartsWith(BatchCheckFormat.FILTER_TOKEN))
                        {
                            filter = BatchCheckFilter.Parse(firstRow);
                        }
                        else
                        {
                            writer.WriteLine(firstRow);
                        }
                        writer.Write(reader.ReadToEnd());
                        writer.Flush();
                    }
                    batchStream.Position = 0;
                    using (var reader = new StreamReader(batchStream, Encoding.GetEncoding(1251), true))
                    {
                        BatchCheckByPassport checkByPassport = new BatchCheckByPassport();
                        DataTable errorTable = ResultData;
                        if (!checkByPassport.Parse(reader, userGroup, ref errorTable))
                        {
                            ResultData = errorTable;
                            dgReportResultsList.Visible = true;

                            dgReportResultsList.DataSource = ResultData;
                            dgReportResultsList.DataBind();
                        }
                        else
                        {
                            // Добавлю данные в бд
                            batchStream.Position = 0;
                            long id = CommonNationalCertificateContext.UpdateRequestBatch(
                                reader.ReadToEnd(), //текст пакетной проверки
                                filter != null ? filter.FilterString : null,
                                false, //пакетная проверка не является типографской
                                null, // string.IsNullOrEmpty(ddlYear.SelectedValue) ? null : ddlYear.SelectedValue, //год для пакетной проверки
                                CurrentUser.ClietnLogin
                                );
                            Response.Redirect(String.Format(SuccessUri, id));
                        }
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



        #region Проверка формата файла

        private enum FileFormatErrors : int
        {
            None = -1,              // нет ошибок
            LastName = 0,           // ошибка в фамилии
            FirstName = 1,          // ошибка в имени
            PatronymicName = 2,     // ошибка в отчестве
            PassportNumber = 3,     // ошибка в номере паспорта
            Other = 4
        }

        private FileFormatErrors CheckFileFormat(out int errorLine)
        {
            string[] lines = FileLines;
            for (int lineNum = 0; lineNum <= lines.Length - 1; lineNum++)
            {
                errorLine = lineNum + 1;
                string[] parts = lines[lineNum].Split(new char[] { '%' });

                if (parts.Length != FileLinePartsCount)
                    return FileFormatErrors.Other;

                else if (!Regex.IsMatch(parts[0], @"^[А-Яа-яЁё\s-]+$"))
                    return FileFormatErrors.LastName;

                else if (!Regex.IsMatch(parts[1], @"^[А-Яа-яЁё\s-]+$"))
                    return FileFormatErrors.FirstName;

                else if (!Regex.IsMatch(parts[2],
                        @"^[А-Яа-яЁё\s-]*$"))
                    return FileFormatErrors.PatronymicName;

                else if (!Regex.IsMatch(parts[4], @"^\d+$"))
                    return FileFormatErrors.PassportNumber;
            }
            errorLine = -1;
            return FileFormatErrors.None;
        }

        // Получение текста ошибки.
        private string GetFileFormatErrorMsg(FileFormatErrors Error, int LineNumber)
        {
            switch (Error)
            {
                case FileFormatErrors.Other:
                    return String.Format("Строка {0} не соответствует формату.", LineNumber);

                case FileFormatErrors.LastName:
                    return String.Format("Неверно указана фамилия. Строка {0}.", LineNumber);

                case FileFormatErrors.FirstName:
                    return String.Format("Неверно указано имя. Строка {0}.", LineNumber);

                case FileFormatErrors.PatronymicName:
                    return String.Format("Неверно указано отчество. Строка {0}.", LineNumber);

                case FileFormatErrors.PassportNumber:
                    return String.Format("Неверно указан номер документа. Строка {0}.", LineNumber);
                default:
                    return String.Empty;
            }
        }

        #endregion

        /// <summary>
        /// Удаление проверки по id
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">параметры события</param>
        protected void DeleteCheck(object sender, CommandEventArgs e)
        {
            this.cnecService.DeleteCheckFromCommonNationalExamCertificateRequestBatch(Convert.ToInt64(e.CommandArgument));
            this.Response.Redirect(this.Request.RawUrl);
        }

        public string GetPageName()
        {
            return "BatchRequestByPassportResultObsolete.aspx";
        }
    }
}
