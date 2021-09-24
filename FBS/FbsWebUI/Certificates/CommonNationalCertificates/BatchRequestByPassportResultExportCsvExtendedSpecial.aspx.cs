using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;

namespace Fbs.Web.Certificates.CommonNationalCertificates
{
    public partial class BatchRequestByPassportResultExportCsvExtendedSpecial : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            rptResultsList.DataSource = GetPreparedDataSet();
            rptResultsList.DataBind();
        }
        private static SqlParameter[] Params()
        {
            SqlParameter[] parameters = new SqlParameter[] 
                {
                new SqlParameter("@login", System.Data.SqlDbType.NVarChar),
                new SqlParameter("@batchId", System.Data.SqlDbType.BigInt),
                new SqlParameter("@isExtended", System.Data.SqlDbType.Bit)
                };
            return parameters;
        }
        private long BatchId
        {
            get
            {
                long testId = -1;
                Int64.TryParse(Request.QueryString["id"], out testId);
                return testId;
            }
        }
        private DataSet GetPreparedDataSet()
        {
            DataSet ds = GetData();
            DataTable table = ds.Tables[0];
            DataRow deniedRow = null;
            bool isDenied = false;
            string deniedCertNum = null;
            string certNum = null;
            DataRow correctRow = null;

            for (int j = table.Rows.Count - 1; j >= 0; j--)
            {
                deniedRow = table.Rows[j];
                isDenied = Convert.ToBoolean(deniedRow["IsDeny"]);
                deniedCertNum = Convert.ToString(deniedRow["DenyNewCertificateNumber"]);
                if (isDenied)
                {
                    deniedRow["RussianMark"] = 0;
                    deniedRow["RussianHasAppeal"] = 0;
                    deniedRow["MathematicsMark"] = 0;
                    deniedRow["MathematicsHasAppeal"] = 0;
                    deniedRow["PhysicsMark"] = 0;
                    deniedRow["PhysicsHasAppeal"] = 0;
                    deniedRow["ChemistryMark"] = 0;
                    deniedRow["ChemistryHasAppeal"] = 0;
                    deniedRow["BiologyMark"] = 0;
                    deniedRow["BiologyHasAppeal"] = 0;
                    deniedRow["RussiaHistoryMark"] = 0;
                    deniedRow["RussiaHistoryHasAppeal"] = 0;
                    deniedRow["GeographyMark"] = 0;
                    deniedRow["GeographyHasAppeal"] = 0;
                    deniedRow["EnglishMark"] = 0;
                    deniedRow["EnglishHasAppeal"] = 0;
                    deniedRow["GermanMark"] = 0;
                    deniedRow["GermanHasAppeal"] = 0;
                    deniedRow["FranchMark"] = 0;
                    deniedRow["FranchHasAppeal"] = 0;
                    deniedRow["SocialScienceMark"] = 0;
                    deniedRow["SocialScienceHasAppeal"] = 0;
                    deniedRow["LiteratureMark"] = 0;
                    deniedRow["LiteratureHasAppeal"] = 0;
                    deniedRow["SpanishMark"] = 0;
                    deniedRow["SpanishHasAppeal"] = 0;
                    deniedRow["InformationScienceMark"] = 0;
                    deniedRow["InformationScienceHasAppeal"] = 0;
                    deniedRow.AcceptChanges();

                    for (int i = 0; i < table.Rows.Count; i++ )
                    {
                        correctRow = table.Rows[i];
                        isDenied = Convert.ToBoolean(correctRow["IsDeny"]);
                        certNum = Convert.ToString(correctRow["CertificateNumber"]);

                        if (!isDenied && deniedCertNum == certNum)
                        {
                            deniedRow["RussianMark"] = correctRow["RussianMark"];
                            deniedRow["RussianHasAppeal"] = correctRow["RussianHasAppeal"];
                            deniedRow["MathematicsMark"] = correctRow["MathematicsMark"];
                            deniedRow["MathematicsHasAppeal"] = correctRow["MathematicsHasAppeal"];
                            deniedRow["PhysicsMark"] = correctRow["PhysicsMark"];
                            deniedRow["PhysicsHasAppeal"] = correctRow["PhysicsHasAppeal"];
                            deniedRow["ChemistryMark"] = correctRow["ChemistryMark"];
                            deniedRow["ChemistryHasAppeal"] = correctRow["ChemistryHasAppeal"];
                            deniedRow["BiologyMark"] = correctRow["BiologyMark"];
                            deniedRow["BiologyHasAppeal"] = correctRow["BiologyHasAppeal"];
                            deniedRow["RussiaHistoryMark"] = correctRow["RussiaHistoryMark"];
                            deniedRow["RussiaHistoryHasAppeal"] = correctRow["RussiaHistoryHasAppeal"];
                            deniedRow["GeographyMark"] = correctRow["GeographyMark"];
                            deniedRow["GeographyHasAppeal"] = correctRow["GeographyHasAppeal"];
                            deniedRow["EnglishMark"] = correctRow["EnglishMark"];
                            deniedRow["EnglishHasAppeal"] = correctRow["EnglishHasAppeal"];
                            deniedRow["GermanMark"] = correctRow["GermanMark"];
                            deniedRow["GermanHasAppeal"] = correctRow["GermanHasAppeal"];
                            deniedRow["FranchMark"] = correctRow["FranchMark"];
                            deniedRow["FranchHasAppeal"] = correctRow["FranchHasAppeal"];
                            deniedRow["SocialScienceMark"] = correctRow["SocialScienceMark"];
                            deniedRow["SocialScienceHasAppeal"] = correctRow["SocialScienceHasAppeal"];
                            deniedRow["LiteratureMark"] = correctRow["LiteratureMark"];
                            deniedRow["LiteratureHasAppeal"] = correctRow["LiteratureHasAppeal"];
                            deniedRow["SpanishMark"] = correctRow["SpanishMark"];
                            deniedRow["SpanishHasAppeal"] = correctRow["SpanishHasAppeal"];
                            deniedRow["InformationScienceMark"] = correctRow["InformationScienceMark"];
                            deniedRow["InformationScienceHasAppeal"] = correctRow["InformationScienceHasAppeal"];
                            deniedRow.AcceptChanges();
                            correctRow.Delete();
                            break;
                        }
                    }
                    table.AcceptChanges();
                }
            }
            return ds;
        }

        private DataSet GetData()
        {
            string connectionString =
                ConfigurationManager.ConnectionStrings["Fbs.Core.Properties.Settings.FbsConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "dbo.SearchCommonNationalExamCertificateRequest";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddRange(Params());
                cmd.Parameters["@login"].Value = HttpContext.Current.User.Identity.Name;
                cmd.Parameters["@batchId"].Value = BatchId;
                cmd.Parameters["@isExtended"].Value = 1;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                return ds;
            }
        }
    }
}
