namespace Fbs.Web.Certificates.CommonNationalCertificates
{
    using System;
    using System.Data;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Web.UI.WebControls;

    using Fbs.Core;
    using Fbs.Core.BatchCheck;

    using FbsServices;

    /// <summary>
    /// The batch request by typographic number.
    /// </summary>
    public partial class BatchRequestByTypographicNumber : BasePage, IHistoryNavigator
    {
        #region Constants and Fields

        private const string FailedUri = "/Certificates/CommonNationalCertificates/BatchRequestByTypographicNumber.aspx";

        // private const int MaxFileSize = 1024;

        // Количество частей, разделенных %, в строке пакетного файла.
        private const int FileLinePartsCount = 4;

        private const string SuccessUri =
            "/Certificates/CommonNationalCertificates/BatchRequestByTypographicNumberResultCommon.aspx?batchId={0}";

        private readonly CNECService cnecService = new CNECService();

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

        private DataTable ResultData
        {
            get
            {
                return this.Session["ResultsList"] as DataTable;
            }

            set
            {
                this.Session["ResultsList"] = value;
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
        }

        /// <summary>
        /// The start parse btn_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        public void StartParseBtn_Click(object sender, EventArgs e)
        {
            try
            {
                this.dgReportResultsList.DataSource = null;
                this.dgReportResultsList.DataBind();

                this.FileErrorMsg.Visible = false;
                if (this.fuData.HasFile)
                {
                    // Преобразую данные из файла в строку

                    if (this.fuData.PostedFile.ContentLength > 1048576)
                    {
                        this.ShowErrorMsg(
                            "Размер файла превышает максимально допустимый (1 Мб). Пожалуйста, уменьшите размер файла. Например, загрузите последовательно несколько файлов со сведениями о свидетельствах");
                        return;
                    }
                    else if (!this.fuData.FileName.EndsWith(".csv"))
                    {
                        // fuData.
                        this.ShowErrorMsg("Неверный формат файла: необходимо передать текстовый файл в формате csv.");
                        return;
                    }

                    Stream batchStream = null;
                    BatchCheckFilter filter = null;
                    string userGroup = Account.GetGroup(this.CurrentUserName);
                    using (var reader = new StreamReader(this.fuData.FileContent, Encoding.GetEncoding(1251), true))
                    {
                        string firstRow = reader.ReadLine();
                        batchStream = new MemoryStream();
                        var writer = new StreamWriter(batchStream, Encoding.GetEncoding(1251));
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
                        var checkByTypographicNumber = new BatchCheckByTypographicNumber();
                        DataTable errorTable = this.ResultData;

                        if (!checkByTypographicNumber.Parse(reader, userGroup, ref errorTable))
                        {
                            this.ResultData = errorTable;
                            this.dgReportResultsList.Visible = true;

                            this.dgReportResultsList.DataSource = this.ResultData;
                            this.dgReportResultsList.DataBind();
                        }
                        else
                        {
                            // Добавлю данные в бд
                            reader.BaseStream.Position = 0;

                            long id = CommonNationalCertificateContext.UpdateRequestBatch(
                                reader.ReadToEnd(), 
                                // текст пакетной проверки
                                filter != null ? filter.FilterString : null, 
                                true, 
                                // пакетная проверка является типографской
                                null, 
                                // год не нужен, так как типографский номер является уникальным
                                CurrentUser.ClietnLogin);
                            this.Response.Redirect(string.Format(SuccessUri, id));
                        }
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

        #endregion

        #region Methods

        /// <summary>
        /// Удаление проверки по id
        /// </summary>
        /// <param name="sender">
        /// Источник события
        /// </param>
        /// <param name="e">
        /// параметры события
        /// </param>
        protected void DeleteCheck(object sender, CommandEventArgs e)
        {
            this.cnecService.DeleteCheckFromCommonNationalExamCertificateRequestBatch(Convert.ToInt64(e.CommandArgument));
            this.Response.Redirect(this.Request.RawUrl);
        }

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
            if (!this.Page.IsPostBack)
            {
                // Если была ошибка "Maximum request length exceeded"
                if (this.Session["FileTooLarge"] != null)
                {
                    this.cvFileTooLarge.IsValid = false;
                    this.cvFileTooLarge.ErrorMessage = string.Format(
                        this.cvFileTooLarge.ErrorMessage, Utility.FormatKbToMb(Config.MaxRequestLength, "0.00"));
                    this.Session.Remove("FileTooLarge");
                }

                return;
            }

            this.dgReportResultsList.DataSource = this.ResultData;
            this.dgReportResultsList.DataBind();
        }

        /// <summary>
        /// The cv file empty_ server validate.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        protected void cvFileEmpty_ServerValidate(object source, ServerValidateEventArgs args)
        {
        }

        /// <summary>
        /// The cv file format_ server validate.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        protected void cvFileFormat_ServerValidate(object source, ServerValidateEventArgs args)
        {
        }

        /// <summary>
        /// The cv file too large_ server validate.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        protected void cvFileTooLarge_ServerValidate(object source, ServerValidateEventArgs args)
        {
        }

        /// <summary>
        /// The dg report results list_ page index changed.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void dgReportResultsList_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            this.dgReportResultsList.CurrentPageIndex = e.NewPageIndex;
            this.dgReportResultsList.DataBind();
        }

        #endregion

        public string GetPageName()
        {
            return "BatchRequestByTypographicNumberResultCommon.aspx";
        }
    }
}