namespace Fbs.Web.AllUsers
{
    using System;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Web;

    using Fbs.Core.BatchCheck;

    /// <summary>
    /// The batch check file format by passport.
    /// </summary>
    public partial class BatchCheckFileFormatByPassport : BasePage
    {
        #region Constants

        private const string FailedUri = "/AllUsers/BatchCheckFileFormatByPassportResult.aspx";

        private const int FieldNumber = 16;

        private const int ResultNumber = 14;

        private const int StartField = 2;

        private const int StartResultField = 7;

        private const string SuccessUri = "/AllUsers/BatchCheckFileFormatByPassportResult.aspx?FileName={0}";

        #endregion

        #region Fields

        private string[] mFileLines;

        #endregion

        #region Properties

        private string[] FileLines
        {
            get
            {
                if (this.mFileLines == null)
                {
                    this.mFileLines = Encoding.Default.GetString(this.fuData.FileBytes).Split(
                        new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < this.mFileLines.Length; i++)
                    {
                        this.mFileLines[i] = this.mFileLines[i].Trim();
                    }
                }

                return this.mFileLines;
            }
        }

        private DataTable ReportTable
        {
            get
            {
                return this.Session["ResultsListPbc"] as DataTable;
            }

            set
            {
                this.Session["ResultsListPbc"] = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The show error msg.
        /// </summary>
        /// <param name="errorMsg">
        /// The error msg.
        /// </param>
        public void ShowErrorMsg(string errorMsg)
        {
            this.FileErrorMsg.Visible = true;

            this.FileErrorMsg.Text = errorMsg;
            this.resultLbl.Visible = false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The page_ error.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Page_Error(object sender, EventArgs e)
        {
            // Получу ошибку
            var checkException = this.Server.GetLastError() as HttpException;

            // Если ошибка "Maximum request length exceeded"
            // !! Не работает на asp.net development server
            if (checkException != null && checkException.ErrorCode == -2147467259)
            {
                this.Session["FileTooLarge"] = true;
                this.Server.ClearError();

                // TODO: странное поведение. отрефакторить.
                this.Response.Redirect(FailedUri, true);
            }
        }

        /// <summary>
        /// The page_ load.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// The start parse btn_click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void StartParseBtn_click(object sender, EventArgs e)
        {
            try
            {
                this.resultLbl.Visible = false;
                this.FileErrorMsg.Visible = false;

                // BatchCheckResultSubjectNoticePnl.Visible = false;

                if (this.fuData.HasFile)
                {
                    if (this.fuData.PostedFile.ContentLength > 1048576)
                    {
                        this.ShowErrorMsg(
                            "Размер файла превышает максимально допустимый (1 Мб). Пожалуйста, уменьшите размер файла. Например, загрузите последовательно несколько файлов со сведениями о свидетельствах");
                    }
                    else if (!this.fuData.FileName.EndsWith(".csv"))
                    {
                        this.ShowErrorMsg("Неверный формат файла: необходимо передать текстовый файл в формате csv.");
                    }
                    else
                    {
                        // Начинаем парсинг файла данные в бд
                        this.Parse();

                        this.resultLbl.Visible = true;
                        if (this.ReportTable != null && this.ReportTable.Rows.Count == 0)
                        {
                            this.resultLbl.Text = "Проверка выполнена успешно!";
                            return;
                        }

                        string fileName = Guid.NewGuid().ToString();
                        this.Session[fileName] = this.ReportTable;
                        this.Response.Redirect(string.Format(SuccessUri, fileName), true);
                    }
                }
                else
                {
                    this.ShowErrorMsg(
                        "Вы загрузили пустой файл. Пожалуйста, укажите сведения хотя бы об одном свидетельстве о результатах ЕГЭ!");
                }
            }
            catch (Exception excp)
            {
                this.ShowErrorMsg(
                    string.Format(
                        "При валидации файла произошла ошибка. Попробуйте загрузить другой файл.{0}", excp.Message));
            }
        }

        private DataTable InitialReportTable()
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
            reportTabe.Columns.Add("RowId");
            reportTabe.Columns.Add("Комментарий");
            return reportTabe;
        }

        private bool Parse()
        {
            this.ReportTable = this.InitialReportTable();
            DataRow reportRow;

            Stream batchStream = null;
            BatchCheckFilter filter = null;

            DataTable dataTable = this.InitialReportTable();

            using (var reader = new StreamReader(this.fuData.FileContent, Encoding.GetEncoding(1251), true))
            {
                string firstRow = reader.ReadLine();
                batchStream = new MemoryStream();
                var writer = new StreamWriter(batchStream, Encoding.GetEncoding(1251));
                if (firstRow.StartsWith(BatchCheckFormat.FILTER_TOKEN))
                {
                    filter = BatchCheckFilter.Parse(firstRow, ref dataTable);
                }
                else
                {
                    writer.WriteLine(firstRow);
                }

                writer.Write(reader.ReadToEnd());
                writer.Flush();
            }

            if (dataTable.Rows.Count > 0)
            {
                this.ReportTable = dataTable;

                // return false;
            }

            batchStream.Position = 0;
            using (var reader = new StreamReader(batchStream, Encoding.GetEncoding(1251)))
            {
                string line;
                int lineIndex = 0;
                string errorMessage;
                string passportSeria = null;
                string passportNumber = null;
                string errorcomment;
                while ((line = reader.ReadLine()) != null)
                {
                    reportRow = this.ReportTable.NewRow();

                    line = line.Trim();

                    errorcomment = string.Empty;
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
                    {
                        continue;
                    }

                    string[] parts = line.Split('%');
                    var partsCopy = new string[40];
                    errorMessage = string.Empty;
                    if (parts.Length != FieldNumber)
                    {
                        errorMessage =
                            string.Format(
                                "Неверное число полей, необходимо указать {0} значений через разделитель - '%'", 
                                FieldNumber);
                        errorcomment += errorMessage;
                        parts[0] = errorMessage;
                        reportRow["RowIndex"] = lineIndex + " [НЕВЕРЕН ФОРМАТ]";
                        reportRow["RowId"] = lineIndex;
                        reportRow["Комментарий"] = "П";
                        this.ReportTable.Rows.Add(reportRow);
                    }
                    else
                    {
                        int j = StartResultField;
                        partsCopy[2] = parts[0];
                        partsCopy[3] = parts[1];

                        for (int i = StartField; i < FieldNumber; i++)
                        {
                            partsCopy[j] = parts[i];
                            j += 2;
                        }

                        passportNumber = parts[1].Trim();

                        if (string.IsNullOrEmpty(passportNumber))
                        {
                            partsCopy[3] += "\t" + "[НЕВЕРЕН ФОРМАТ]";
                            errorMessage = "НП,";
                            errorcomment += errorMessage;
                        }

                        int resultNullCount = 0;
                        for (int i = StartResultField; i < 35; i += 2)
                        {
                            if (!string.IsNullOrEmpty(partsCopy[i].Trim()))
                            {
                                float mark;
                                if (
                                    !float.TryParse(
                                        partsCopy[i].Replace(',', '.'), 
                                        NumberStyles.Float, 
                                        NumberFormatInfo.InvariantInfo, 
                                        out mark) || mark < 0 || mark > 100)
                                {
                                    partsCopy[i] += "\t" + "[НЕВЕРЕН ФОРМАТ]";
                                    errorMessage = "БП,";
                                    if (!errorcomment.Contains("БП"))
                                    {
                                        errorcomment += errorMessage;
                                    }
                                }
                            }
                            else
                            {
                                resultNullCount++;
                            }
                        }

                        errorcomment = errorcomment.TrimEnd(',');
                        string currentLine = string.Empty;

                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            for (int i = 0; i < StartResultField; i++)
                            {
                                reportRow[i] = partsCopy[i];
                            }

                            j = StartResultField;
                            for (int i = StartResultField; i < 34; i ++)
                            {
                                reportRow[j] = partsCopy[i];
                                j ++;
                            }

                            currentLine = currentLine + "\t\t" + errorcomment;
                            reportRow["RowIndex"] = lineIndex;
                            reportRow["RowId"] = lineIndex;
                            reportRow["Комментарий"] = errorcomment;
                            this.ReportTable.Rows.Add(reportRow);
                        }
                    }
                }
            }

            return true;
        }

        #endregion
    }
}