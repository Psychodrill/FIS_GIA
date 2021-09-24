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

    public partial class BatchCheck : BasePage, IHistoryNavigator
    {
        #region Constants and Fields

        private readonly CNECService cnecService = new CNECService();

        #endregion


        private const string FailedUri = "/Certificates/CommonNationalCertificates/BatchCheck.aspx";
        private const string SuccessUri = "/Certificates/CommonNationalCertificates/BatchCheckResultCommon.aspx?batchId={0}";

        private DataTable ResultData
        {
            get { return Session["ResultsList"] as DataTable; }
            set { Session["ResultsList"] = value; }
        }

        protected void dgResultsList_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            dgResultsList.CurrentPageIndex = e.NewPageIndex;
            dgResultsList.DataBind();
        }

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

            this.dgResultsList.DataSource = this.ResultData;
            dgResultsList.DataBind();
        }

        private void ShowErrorMsg(string errorMsg)
        {
            FileErrorMsg.Visible = true;
            FileErrorMsg.Text = errorMsg;
        }

        protected void StartParseBtn_Click(object sender, EventArgs e)
        {
            try
            {
                FileErrorMsg.Visible = false;
                dgResultsList.DataSource = null;
                dgResultsList.DataBind();
                if (fuData.HasFile)
                {
                    // Вынести проверку на расширение и объем в отдельный класс.
                    if (fuData.PostedFile.ContentLength > 1048576)
                    {
                        this.ShowErrorMsg(
                            "Размер файла превышает максимально допустимый (1 Мб). Пожалуйста, уменьшите размер файла. Например, загрузите последовательно несколько файлов со сведениями о свидетельствах");

                        return;
                    }

                    if (!fuData.FileName.EndsWith(".csv"))
                    {
                        this.ShowErrorMsg("Неверный формат файла: необходимо передать текстовый файл в формате csv.");
                        return;
                    }

                    string userGroup = Account.GetGroup(CurrentUserName);

                    Stream batchStream = null;
                    BatchCheckFilter filter = null;
                    using (var reader = new StreamReader(fuData.FileContent, Encoding.GetEncoding(1251), true))
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
                        var checkByCertificateNumber = new BatchCheckByCertificateNumberNew();
                        var errorTable = this.ResultData;

                        if (!checkByCertificateNumber.Parse(reader, userGroup, ref errorTable))
                        {
                            this.ResultData = errorTable;
                            dgResultsList.Visible = true;

                            this.dgResultsList.DataSource = this.ResultData;
                            dgResultsList.DataBind();
                        }
                        else
                        {
                            // Добавлю данные в бд
                            reader.BaseStream.Position = 0;
                            var batchText = reader.ReadToEnd();
                            long id = CommonNationalCertificateContext.UpdateCheckBatch(
                                batchText,
                                filter != null ? filter.FilterString : null,
                                CurrentUser.ClietnLogin);
                            Response.Redirect(string.Format(SuccessUri, id), false);
                        }
                    }
                }
                else
                {
                    ShowErrorMsg(
                        "Вы загрузили пустой файл. Пожалуйста, укажите сведения хотя бы об одном свидетельстве о результатах ЕГЭ!");
                }
            }
            catch (Exception excp)
            {
                ShowErrorMsg(string.Format(
                                 "При валидации файла произошла ошибка. Попробуйте загрузить другой файл.{0}",
                                 excp.Message));
            }
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            // Получу ошибку
            var checkException = Server.GetLastError() as HttpException;

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
        }

        protected void cvFileEmpty_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
        }

        protected void cvFileFormat_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
        }

        /// <summary>
        /// Удаление проверки по id
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">параметры события</param>
        protected void DeleteCheck(object sender, CommandEventArgs e)
        {
            this.cnecService.DeleteCheckFromCommonNationalExamCertificateCheckBatch(Convert.ToInt64(e.CommandArgument));
            this.Response.Redirect(this.Request.RawUrl);
        }

        public string GetPageName()
        {
            return "BatchCheckResultCommon.aspx";
        }
    }
}
