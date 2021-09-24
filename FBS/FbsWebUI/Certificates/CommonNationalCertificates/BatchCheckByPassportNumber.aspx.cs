namespace Fbs.Web.Certificates.CommonNationalCertificates
{
    using System;
    using System.Data;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Web.UI.WebControls;
    using System.Xml;

    using Fbs.Core;
    using Fbs.Core.BatchCheck;
    using Fbs.Core.WebServiceCheck;

    using FbsChecksClient;
    using FbsChecksClient.WSChecksReference;

    using FbsServices;

    /// <summary>
    /// The batch check by passport number.
    /// </summary>
    public partial class BatchCheckByPassportNumber : BasePage
    {
        #region Constants

        private const string FailedUri = "/Certificates/CommonNationalCertificates/BatchCheckByPassportNumber.aspx";

        private const string SuccessUri =
            "/Certificates/CommonNationalCertificates/BatchCheckByPassportNumberResult.aspx?id={0}";

        #endregion

        #region Fields

        private readonly CNECService cnecService = new CNECService();

        #endregion

        #region Properties

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
        /// The get result.
        /// </summary>
        /// <param name="isProcess">
        /// The is process.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The get result.
        /// </returns>
        public string GetResult(bool isProcess, object id)
        {
            string enable =
                string.Format(
                    "<a href=\"/Certificates/CommonNationalCertificates/BatchCheckByPassportNumberResult.aspx?id={0}\">результат</a>", id);
            string disable = string.Empty;

            return isProcess ? disable : enable;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The data grid on item data bound.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void DataGridOnItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.DataItem == null)
            {
                return;
            }

            if (Convert.ToBoolean(((DataRowView)e.Item.DataItem)["IsProcess"]))
            {
                long id = Convert.ToInt64(((DataRowView)e.Item.DataItem)["Id"]);
                var checkClient = new WSCheckClient();
                XmlElement xml = null;

                if(checkClient.SearchCommonNationalExamCertificateCheckByOuterId(this.User.Identity.Name, id, ref xml) == (int)WebServiceReplyCodes.UserIsBanned)
                {
                    this.Response.Redirect("CheckBanPage.aspx");
                }

                DataTable resultTable = this.cnecService.AddCheckBatchResult(xml, id);
                if (resultTable.Rows.Count > 0)
                {
                    DataRow row = resultTable.Rows[0];
                    e.Item.Cells[2].Text = string.Format("найдено {0} из {1}", row["Found"], row["Total"]);
                    e.Item.Cells[3].Text = this.GetResult(Convert.ToBoolean(row["IsProcess"]), id);
                }
            }
        }

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
            this.cnecService.DeleteCheckFromCommonNationalExamCertificateCheckBatch(Convert.ToInt64(e.CommandArgument));
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
            if (!this.Page.IsPostBack)
            {
                for (int year = 2008; year <= DateTime.Now.Year; year++)
                {
                    this.ddlYear.Items.Add(new ListItem(string.Format("Поиск по {0}", year), year.ToString()));
                }

                // Если была ошибка "Maximum request length exceeded"
                if (this.Session["FileTooLarge"] != null)
                {
                    this.cvFileTooLarge.IsValid = false;
                    this.cvFileTooLarge.ErrorMessage = string.Format(
                        this.cvFileTooLarge.ErrorMessage, Utility.FormatKbToMb(Config.MaxRequestLength, "0.00"));
                    this.Session.Remove("FileTooLarge");
                }

                this.plhParseFileError.Visible = false;
                return;
            }

            this.dgResultsList.DataSource = this.ResultData;
            this.dgResultsList.DataBind();
            this.plhParseFileError.Visible = this.dgResultsList.Items.Count != 0;
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
        protected void StartParseBtn_Click(object sender, EventArgs e)
        {
            try
            {
                this.FileErrorMsg.Visible = false;
                this.dgResultsList.DataSource = null;
                this.dgResultsList.DataBind();
                if (this.fuData.HasFile)
                {
                    // Вынести проверку на расширение и объем в отдельный класс.
                    if (this.fuData.PostedFile.ContentLength > 1048576)
                    {
                        this.ShowErrorMsg(
                            "Размер файла превышает максимально допустимый (1 Мб). Пожалуйста, уменьшите размер файла. Например, загрузите последовательно несколько файлов со сведениями о свидетельствах");

                        return;
                    }

                    if (!this.fuData.FileName.EndsWith(".csv"))
                    {
                        this.ShowErrorMsg("Неверный формат файла: необходимо передать текстовый файл в формате csv.");
                        return;
                    }

                    string userGroup = Account.GetGroup(this.CurrentUserName);

                    Stream batchStream = null;
                    BatchCheckFilter filter = null;
                    using (var reader = new StreamReader(this.fuData.FileContent, Encoding.GetEncoding(1251), true))
                    {
                        string firstRow = reader.ReadLine();

                        batchStream = new MemoryStream();
                        var writer = new StreamWriter(batchStream, Encoding.GetEncoding(1251));

                        if (firstRow.StartsWith(BatchCheckFormat.FILTER_TOKEN))
                        {
                            filter = BatchCheckFilter.Parse(firstRow);
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
                    using (var reader = new StreamReader(batchStream, Encoding.GetEncoding(1251), true))
                    {
                        var checkByCertificateNumber = new CheckByPassportAndSubjectValues();
                        DataTable errorTable = this.ResultData;

                        if (!checkByCertificateNumber.Parse(reader, userGroup, ref errorTable))
                        {
                            this.ResultData = errorTable;
                            this.dgResultsList.Visible = true;

                            this.dgResultsList.DataSource = this.ResultData;
                            this.dgResultsList.DataBind();
                        }
                        else
                        {
                            // Добавлю данные в бд
                            reader.BaseStream.Position = 0;
                            string batchText = reader.ReadToEnd();
                            long id = CommonNationalCertificateContext.UpdateCheckBatch(
                                batchText, 
                                filter != null ? filter.FilterString : null, 
                                CurrentUser.ClietnLogin, 
                                2, 
                                null, 
                                string.IsNullOrEmpty(this.ddlYear.SelectedValue)
                                    ? null
                                    : (int?)Convert.ToInt32(this.ddlYear.SelectedValue));

                            var checkClient = new WSCheckClient();

                            checkClient.StartBatchCheck(
                                this.User.Identity.Name,
                                batchText, 
                                2, 
                                id, 
                                string.IsNullOrEmpty(this.ddlYear.SelectedValue)
                                    ? null
                                    : (int?)Convert.ToInt32(this.ddlYear.SelectedValue));

                            this.Response.Redirect(string.Format(SuccessUri, id), false);
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
            args.IsValid = true;
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
            args.IsValid = true;
        }

        /// <summary>
        /// The dg results list_ page index changed.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void dgResultsList_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            this.dgResultsList.CurrentPageIndex = e.NewPageIndex;
            this.dgResultsList.DataBind();
        }

        protected void cvFileTooLarge_ServerValidate(object source, ServerValidateEventArgs args)
        {

        }


        private void ShowErrorMsg(string errorMsg)
        {
            this.FileErrorMsg.Visible = true;
            this.FileErrorMsg1.Text = errorMsg;
        }

        #endregion
    }
}