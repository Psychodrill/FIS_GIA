namespace Fbs.Web.Certificates.CommonNationalCertificates.HashedCheck
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Web.UI;

    /// <summary>
    /// The batch check by mark sum result.
    /// </summary>
    public partial class BatchCheckByMarkSumResult : Page
    {
        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether InProgress.
        /// </summary>
        public bool InProgress
        {
            get
            {
                return this.Session["BatchCheckByMarkSumResult"] == null;
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
        /// <exception cref="HttpException">
        /// </exception>
        protected void Page_Load(object sender, EventArgs e)
        {
            long batchId;
            if (!long.TryParse(this.Request.QueryString["batchId"], out batchId))
            {
                throw new HttpException(404, "Нет такой страницы");
            }

            using (
                var conn =
                    new SqlConnection(
                        Config.Configuration.ConnectionStrings.ConnectionStrings[
                            "Fbs.Core.Properties.Settings.FbsConnectionString"].ConnectionString))
            {
                using (SqlCommand command = conn.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "CommonNationalExamCertificateSumCheckResult";
                    command.Parameters.AddWithValue("@batchId", batchId);
                    conn.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        string name = string.Format(
                            "{0}_{1}.csv",
                            this.Request.QueryString["type"] == "4"
                                ? "ФИОСумма"
                                : this.Request.QueryString["type"] == "3" ? "ФИОБаллы" : "ФИОПредметы",
                            DateTime.Now.ToString("dd.MM.yyyy_hh.mm.ss.fff"));
                        string fileName = Path.Combine(Path.GetTempPath(), name);
                        using (
                            var writer = new StreamWriter(
                                new FileStream(fileName, FileMode.OpenOrCreate), Encoding.GetEncoding(1251)))
                        {
                            while (reader.Read())
                            {
                                string status = reader["Status"].ToString();
                                writer.WriteLine(
                                    string.Format(
                                        "{0}{1}{2}%{3}",
                                        reader["Name"],
                                        this.Request.QueryString["type"] == "5" ? "%" : "%" + reader["Sum"] + "%",
                                        status,
                                        reader["NameSake"]));
                            }
                        }

                        this.waitBox.Visible = false;
                        this.resultPanel.Visible = true;
                        this.ResultFileLink.NavigateUrl = string.Format("ResultFileHandler.ashx?file={0}", name);
                    }
                }
            }

            #endregion
        }
    }
}