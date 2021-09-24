using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace GVUZ.ImportService2016.Core.Main.Repositories
{
    public class ADOApplicationRepository
    {
        public static int GetBenefitEgeMinValue(int CompetitiveGroupID, int SubjectID, int OlympicID)
        {
            string sql = @"
SELECT 
isnull(MIN(isnull(bic.EgeMinValue, 0)) , 0)
FROM BenefitItemC bic
LEFT JOIN BenefitItemCOlympicType AS bict ON bict.BenefitItemID = bic.BenefitItemID
INNER JOIN EntranceTestItemC AS etic ON etic.EntranceTestItemID = bic.EntranceTestItemID
where etic.SubjectID = @subjectId 
and bic.CompetitiveGroupID = @competitiveGroupId
and (bict.OlympicTypeID is null or  bict.OlympicTypeID = @olympicId);
";

            var parameters = new Dictionary<string, object>();
            parameters.Add("@competitiveGroupId", CompetitiveGroupID);
            parameters.Add("@subjectId", SubjectID);
            parameters.Add("@olympicId", OlympicID);

            var queryRes = ADOBaseRepository.ExecuteScalar(sql, parameters);
            int result = 0;
            return (queryRes != null && int.TryParse(queryRes.ToString(), out result)) ? result : 0;
            
        }

        public static Tuple<int, Guid> GetEntrantDocumentByData(int entrantId, int documentTypeId, string documentSeries, string documentNumber, DateTime documentDate, string documentOrganization, string uid)
        {
            Tuple<int, Guid> res = new Tuple<int,Guid>(0, Guid.Empty);
            string sql = "";

            if (!string.IsNullOrWhiteSpace(uid))
                sql = @"
select top(1) EntrantDocumentID, EntrantDocumentGUID From EntrantDocument (NOLOCK)
Where EntrantID = @entrantID 
AND (DocumentTypeID = @documentTypeId)
AND (UID = @uid)";

            else
            {
                return res;

//                if (!string.IsNullOrWhiteSpace(documentNumber) || !string.IsNullOrWhiteSpace(documentSeries))
//                    sql = @"
//select top(1) EntrantDocumentID, EntrantDocumentGUID From EntrantDocument
//Where EntrantID = @entrantID 
//AND (DocumentTypeID = @documentTypeId)
//AND (DocumentSeries is null OR DocumentSeries = @documentSeries)
//AND (DocumentNumber is null OR @documentNumber is null OR DocumentNumber = @documentNumber)
//AND (DocumentDate is null OR @documentDate is null OR DocumentDate = @documentDate)
//AND (DocumentOrganization is null OR @documentOrganization is null OR DocumentOrganization = @documentOrganization)
//AND (UID is null OR UID = @uid);
//";

            }
            // -- OR UID = @uid)

            var parameters = new Dictionary<string, object>();
            parameters.Add("@entrantId", entrantId);
            parameters.Add("@documentTypeId", documentTypeId);
            parameters.Add("@documentSeries", documentSeries != null ? documentSeries : (object)DBNull.Value);
            parameters.Add("@documentNumber", documentNumber != null ? documentNumber : (object)DBNull.Value);
            parameters.Add("@documentDate", documentDate!=null && documentDate.Year>1977 ? documentDate : (object)DBNull.Value);
            parameters.Add("@documentOrganization", documentOrganization!=null ? documentOrganization: (object)DBNull.Value);
            parameters.Add("@uid", uid!=null ? uid : (object)DBNull.Value);

            var ds = ADOBaseRepository.ExecuteQuery(sql, parameters);
            
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int item1 = (int)ds.Tables[0].Rows[0][0];
                Guid item2;
                if (!Guid.TryParse(ds.Tables[0].Rows[0][1].ToString(), out item2))
                    item2 = new Guid(item1.ToString("00000000000000000000000000000000")); // Guid.Empty;
                res = new Tuple<int,Guid>(item1 , item2);
            }

            
            return res;
        }


        public static List<AcgiDates> CheckEntrantAgreedDate(int entrantId, int applicationId = 0)
        {
            string sql = @"
--declare @entrantId int = 185;
--declare @applicationId int = 0;

select distinct acgi.IsAgreedDate, acgi.IsDisagreedDate, a.ApplicationID, a.UID, acgi.CompetitiveGroupID
From ApplicationCompetitiveGroupItem acgi (NOLOCK)
inner join application a (NOLOCK) on acgi.ApplicationId = a.ApplicationID
inner join CompetitiveGroup cg (NOLOCK) on acgi.CompetitiveGroupId = cg.CompetitiveGroupID
--inner join Campaign c (NOLOCK) on cg.CampaignID = c.CampaignID
Where a.EntrantID = @entrantId 
--AND a.ApplicationID != @applicationId
AND cg.EducationFormId in (11, 12)
AND cg.EducationSourceId != 15 --Paid
AND cg.EducationLevelID in (2, 5)
AND acgi.IsAgreedDate is not null;
";

            var parameters = new Dictionary<string, object>();
            parameters.Add("@entrantId", entrantId);
            //parameters.Add("@applicationId", applicationId);

            var ds = ADOBaseRepository.ExecuteQuery(sql, parameters);
            var res = new List<AcgiDates>();

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach(DataRow row in ds.Tables[0].Rows)
                {
                    res.Add(new AcgiDates(row));
                }
            }

            return res;
        }

        public static List<AcgiDates> CheckEntrantDisagreedDate(int entrantId, int applicationId = 0)
        {
            string sql = @"
--declare @entrantId int = 185;
--declare @applicationId int = 0;

select distinct acgi.IsAgreedDate, acgi.IsDisagreedDate, a.ApplicationID, a.UID, acgi.CompetitiveGroupID
From ApplicationCompetitiveGroupItem acgi (NOLOCK)
inner join application a (NOLOCK) on acgi.ApplicationId = a.ApplicationID
inner join CompetitiveGroup cg (NOLOCK) on acgi.CompetitiveGroupId = cg.CompetitiveGroupID
Where a.EntrantID = @entrantId 
AND acgi.IsDisagreedDate is not null;
";

            var parameters = new Dictionary<string, object>();
            parameters.Add("@entrantId", entrantId);
            //parameters.Add("@applicationId", applicationId);

            var ds = ADOBaseRepository.ExecuteQuery(sql, parameters);
            var res = new List<AcgiDates>();

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    res.Add(new AcgiDates(row));
                }
            }

            return res;
        }

        public static Tuple<string, string, int?> CheckBenefitOlympic(int entranceTestItemID, int groupID, int olympicTypeProfileID,
                int diplomaTypeID, int olympicID, int formNumberID, int docID)
        {
            //string res = "";
            //var ds = new DataSet();
            var connectionString = ConfigurationManager.ConnectionStrings["Main"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                //using (SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted)) 
                using (SqlCommand cmd = new SqlCommand("ChectBenefitOlympic", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ADOBaseRepository.TIMEOUT;

                    //foreach (var p in parameters)
                    //    cmd.Parameters.Add(new SqlParameter(p.Key, p.Value));
                    //cmd.Parameters.Add("@entranceTestItemID", SqlDbType.Int, entranceTestItemID);
                    cmd.Parameters.AddWithValue("@entranceTestItemID", entranceTestItemID != 0 ? entranceTestItemID : (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@groupID", groupID != 0 ? groupID : (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@olympicTypeProfileID", olympicTypeProfileID != 0 ? olympicTypeProfileID : (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@diplomaTypeID", diplomaTypeID != 0 ? diplomaTypeID : (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@olympicID", olympicID != 0 ? olympicID : (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@formNumberID", formNumberID != 0 ? formNumberID : (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@docID", docID != 0 ? docID : (object)DBNull.Value);

                    SqlParameter outputErrorMessageParam = new SqlParameter("@errorMessage", SqlDbType.VarChar, 4000)
                    {
                        Direction = ParameterDirection.Output
                    };
                    SqlParameter outputViolationMessageParam = new SqlParameter("@violationMessage", SqlDbType.VarChar, 4000)
                    {
                        Direction = ParameterDirection.Output
                    };
                    SqlParameter outputViolationIdParam = new SqlParameter("@violationId", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };

                    cmd.Parameters.Add(outputErrorMessageParam);
                    cmd.Parameters.Add(outputViolationMessageParam);
                    cmd.Parameters.Add(outputViolationIdParam);

                    cmd.ExecuteNonQuery();
                    connection.Close();

                    var errorMessage = cmd.Parameters["@errorMessage"].Value.ToString();
                    var violationMessage = cmd.Parameters["@violationMessage"].Value.ToString();
                    var violationId = cmd.Parameters["@violationId"].Value as int?;

                    return new Tuple<string, string, int?>(errorMessage, violationMessage, violationId);

                }
            }
        }
    }

    public class AcgiDates
    {
        public AcgiDates() { }
        public AcgiDates(DataRow row)
        {
            IsAgreedDate = row[0] as DateTime?;
            IsDisagreedDate = row[1] as DateTime?;
            ApplicationID = (int)row[2];
            UID = row[3].ToString();
            CompetitiveGroupID = (int)row[4];
        }
        public DateTime? IsAgreedDate { get; set; }
        public DateTime? IsDisagreedDate { get; set; }
        public int ApplicationID { get; set; }
        public string UID { get; set; }
        public int CompetitiveGroupID { get; set; }
    }
}
