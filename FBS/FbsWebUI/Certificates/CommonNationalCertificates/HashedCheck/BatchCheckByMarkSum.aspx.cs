namespace Fbs.Web.Certificates.CommonNationalCertificates.HashedCheck
{
    using System;
    using System.Data;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.UI.WebControls;

    using Fbs.Core;
    using Fbs.Core.BatchCheck;

    /// <summary>
    /// The batch check by mark sum.
    /// </summary>
    public partial class BatchCheckByMarkSum : BasePage
    {
        #region Constants

        private const string SuccessUri =
            "/Certificates/CommonNationalCertificates/HashedCheck/BatchCheckByMarkSumResult.aspx?batchId={0}&type={1}";

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

        #region Methods

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
        /// The upload checks.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void UploadChecks(object sender, EventArgs e)
        {
            if (this.IsValid)
            {
                string userGroup = Account.GetGroup(this.CurrentUserName);
                string fileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                int checkType = int.Parse(this.rblTypeSelector.SelectedValue);

                this.fuUploader.SaveAs(fileName);

                using (var reader = new StreamReader(fileName, Encoding.GetEncoding(1251), true))
                {
                    var checkByMarkSum = new BatchCheckByMarkSumParser();
                    DataTable errorTable = this.ResultData;
                    if (!checkByMarkSum.Parse(reader, userGroup, ref errorTable, false, checkType == 5))
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
                        string inputBatch = reader.ReadToEnd().Replace('ё', 'е').Replace(" ", string.Empty);
                        long id =
                            CommonNationalCertificateContext.UpdateCheckBatch(
                                checkType == 5 ? this.PrepareBatch(inputBatch) : this.PrepareBatchWithSum(inputBatch),
                                null, // текст пакетной проверки
                                CurrentUser.ClietnLogin,
                                checkType,
                                null,
                                null);
                        this.Response.Redirect(string.Format(SuccessUri, id, checkType));
                    }
                }

                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
            }
        }

        /// <summary>
        /// The validate input files.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void ValidateInputFiles(object sender, ServerValidateEventArgs e)
        {
            e.IsValid = this.fuUploader.FileBytes.Length > 0;
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

        private string MakeOneInputString(
            string name,
            string sum,
            string russian,
            string math,
            string phys,
            string chem,
            string bio,
            string hist,
            string geo,
            string eng,
            string germ,
            string french,
            string social,
            string lit,
            string span,
            string comp)
        {
            return string.Format(
                "{0}%{1}{2}%{3}%{4}%{5}%{6}%{7}%{8}%{9}%{10}%{11}%{12}%{13}%{14}%{15}",
                name,
                sum == null ? string.Empty : sum + "%",
                russian,
                math,
                phys,
                chem,
                bio,
                hist,
                geo,
                eng,
                germ,
                french,
                social,
                lit,
                span,
                comp);
        }

        private string PrepareBatchWithSum(string inputBatch)
        {
            string pattern = @".+%\d+%\d*%\d*%\d*%\d*%\d*%\d*%\d*%\d*%\d*%\d*%\d*%\d*";

            MatchCollection matches = Regex.Matches(inputBatch, pattern);
            var sb = new StringBuilder();
            foreach (Match match in matches)
            {
                string[] fields = match.Value.Split(new[] { '%' });

                // льготы не обрабатываем
                if (!string.IsNullOrEmpty(fields[fields.Length - 1]))
                {
                    continue;
                }

                // размножить иностранный язык на 4 разных языка
                if (fields[9].Length > 0)
                {
                    string withEnglish = this.MakeOneInputString(
                        fields[0],
                        fields[1],
                        fields[2],
                        fields[3],
                        fields[4],
                        fields[5],
                        fields[6],
                        fields[7],
                        fields[8],
                        fields[9],
                        string.Empty,
                        string.Empty,
                        fields[10],
                        fields[11],
                        string.Empty,
                        fields[12]);

                    string withGerm = this.MakeOneInputString(
                        fields[0],
                        fields[1],
                        fields[2],
                        fields[3],
                        fields[4],
                        fields[5],
                        fields[6],
                        fields[7],
                        fields[8],
                        string.Empty,
                        fields[9],
                        string.Empty,
                        fields[10],
                        fields[11],
                        string.Empty,
                        fields[12]);

                    string withFrench = this.MakeOneInputString(
                        fields[0],
                        fields[1],
                        fields[2],
                        fields[3],
                        fields[4],
                        fields[5],
                        fields[6],
                        fields[7],
                        fields[8],
                        string.Empty,
                        string.Empty,
                        fields[9],
                        fields[10],
                        fields[11],
                        string.Empty,
                        fields[12]);

                    string withSpanish = this.MakeOneInputString(
                        fields[0],
                        fields[1],
                        fields[2],
                        fields[3],
                        fields[4],
                        fields[5],
                        fields[6],
                        fields[7],
                        fields[8],
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        fields[10],
                        fields[11],
                        fields[9],
                        fields[12]);

                    sb.AppendLine(withEnglish);
                    sb.AppendLine(withGerm);
                    sb.AppendLine(withFrench);
                    sb.AppendLine(withSpanish);
                }
                else
                {
                    string withNoForeignLang = this.MakeOneInputString(
                        fields[0],
                        fields[1],
                        fields[2],
                        fields[3],
                        fields[4],
                        fields[5],
                        fields[6],
                        fields[7],
                        fields[8],
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        fields[10],
                        fields[11],
                        string.Empty,
                        fields[12]);
                    sb.AppendLine(withNoForeignLang);
                }
            }

            return sb.ToString();
        }


        private string PrepareBatch(string inputBatch)
        {
            string pattern = @".+%\d*%\d*%\d*%\d*%\d*%\d*%\d*%\d*%\d*%\d*%\d*%\d*";

            MatchCollection matches = Regex.Matches(inputBatch, pattern);
            var sb = new StringBuilder();
            foreach (Match match in matches)
            {
                string[] fields = match.Value.Split(new[] { '%' });

                // льготы не обрабатываем
                if (!string.IsNullOrEmpty(fields[fields.Length - 1]))
                {
                    continue;
                }

                // размножить иностранный язык на 4 разных языка
                if (fields[8].Length > 0)
                {
                    string withEnglish = this.MakeOneInputString(
                        fields[0],
                        null,
                        fields[1],
                        fields[2],
                        fields[3],
                        fields[4],
                        fields[5],
                        fields[6],
                        fields[7],
                        fields[8],
                        string.Empty,
                        string.Empty,
                        fields[9],
                        fields[10],
                        string.Empty,
                        fields[11]);

                    string withGerm = this.MakeOneInputString(
                        fields[0],
                        null,
                        fields[1],
                        fields[2],
                        fields[3],
                        fields[4],
                        fields[5],
                        fields[6],
                        fields[7],
                        string.Empty,
                        fields[8],
                        string.Empty,
                        fields[9],
                        fields[10],
                        string.Empty,
                        fields[11]);

                    string withFrench = this.MakeOneInputString(
                        fields[0],
                        null,
                        fields[1],
                        fields[2],
                        fields[3],
                        fields[4],
                        fields[5],
                        fields[6],
                        fields[7],
                        string.Empty,
                        string.Empty,
                        fields[8],
                        fields[9],
                        fields[10],
                        string.Empty,
                        fields[11]);

                    string withSpanish = this.MakeOneInputString(
                        fields[0],
                        null,
                        fields[1],
                        fields[2],
                        fields[3],
                        fields[4],
                        fields[5],
                        fields[6],
                        fields[7],
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        fields[9],
                        fields[10],
                        fields[8],
                        fields[11]);

                    sb.AppendLine(withEnglish);
                    sb.AppendLine(withGerm);
                    sb.AppendLine(withFrench);
                    sb.AppendLine(withSpanish);
                }
                else
                {
                    string withNoForeignLang = this.MakeOneInputString(
                        fields[0],
                        null,
                        fields[1],
                        fields[2],
                        fields[3],
                        fields[4],
                        fields[5],
                        fields[6],
                        fields[7],
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        fields[9],
                        fields[10],
                        string.Empty,
                        fields[11]);

                    sb.AppendLine(withNoForeignLang);
                }
            }

            return sb.ToString();
        }

        #endregion
    }
}