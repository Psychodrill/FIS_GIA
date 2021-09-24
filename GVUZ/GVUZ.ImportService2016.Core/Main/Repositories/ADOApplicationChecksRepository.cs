using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Configuration;

namespace GVUZ.ImportService2016.Core.Main.Repositories
{
    public class ADOApplicationChecksRepository : IDisposable
    {
        private int _timeout = 600; //todo: вынести в настройки
        private int _applicationId;
        private string _userLogin;

        private const string _initSql = @"
            --declare @applicationID int = 169592;

            Select StatusID, isnull(ViolationErrors, ''), UID, ViolationID
            From Application (NOLOCK) Where ApplicationID = @applicationID;

            -- Для FindEntrtantOlympic и CheckBenefitOlympic
            select ed.EntrantDocumentID, EDO.OlympicID, EDO.OlympicTypeProfileID, EDO.DiplomaTypeID, EDO.ClassNumber
            , AETD.EntranceTestItemID, AETD.CompetitiveGroupID, ED.UID 
            , ED.DocumentDate, ED.DocumentNumber 
            From EntrantDocument ED (NOLOCK) 
            inner join ApplicationEntranceTestDocument AETD (NOLOCK) ON  AETD.EntrantDocumentID = ED.EntrantDocumentID
            inner join EntrantDocumentOlympic EDO (NOLOCK) ON EDO.EntrantDocumentID = ED.EntrantDocumentID
            Where AETD.ApplicationID = @applicationID and ED.DocumentTypeID in (9, 10)

            -- Для FindEntrantCompositionMarks
            select top(1) ia.IdAchievement
            From InstitutionAchievements ia (NOLOCK)
            inner join Campaign c (NOLOCK) on c.CampaignID = ia.CampaignID
            inner join CompetitiveGroup cg (NOLOCK) on cg.CampaignID = c.CampaignID
            inner join ApplicationCompetitiveGroupItem acgi (NOLOCK) on acgi.CompetitiveGroupID = cg.CompetitiveGroupID
            Where ia.IdCategory = 12 and acgi.ApplicationId = @ApplicationID;

            -- 
            select top(1) c.CampaignID
            From Campaign c (NOLOCK)
            inner join CompetitiveGroup cg (NOLOCK) on cg.CampaignID = c.CampaignID
            inner join ApplicationCompetitiveGroupItem acgi (NOLOCK) on acgi.CompetitiveGroupID = cg.CompetitiveGroupID
            Where c.CampaignTypeID in (1, 3) and acgi.ApplicationId = @ApplicationID;
        ";

        private class CheckOlympicData {
            public int DocID { get; set; }
            public string UID { get; set; }
            public int? OlympicID { get; set; }
            public int? OlympicTypeProfileID { get; set; }
            public int? DiplomTypeID { get; set; }
            public int? ClassNumber { get; set; }
            public int? EntranceTestItemID { get; set; }
            public int? CompetitiveGroupID { get; set; }

            public DateTime? DocumentDate { get; set; }
            public string DocumentNumber { get; set; }
        }

        private List<CheckOlympicData> _checkOlympics = new List<CheckOlympicData>();

        private bool _isCheckComposition = false;
        private bool _isCheckEge = false;
        private bool _isCheck = false;
        private bool _isSingle = false;
        public string UID { get; set; }

        public ADOApplicationChecksRepository(int applicationId, string userLogin, bool isSingle)
        {
            _applicationId = applicationId;
            _userLogin = userLogin;
            _isSingle = isSingle;

            string connectionString = ConfigurationManager.ConnectionStrings["Main"].ConnectionString;
            try
            {
                _connection = new SqlConnection(connectionString);
                _connection.Open();

                using (SqlCommand cmd = new SqlCommand(_initSql, _connection))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = _timeout;

                    cmd.Parameters.AddWithValue("applicationID", _applicationId);

                    var ds = new DataSet();
                    var adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(ds);

                    if (ds.Tables.Count >= 1 && ds.Tables[0].Rows.Count >= 1)
                    {
                        var row = ds.Tables[0].Rows[0];
                        var violationID = (int)row[3];

                        //_isCheck = string.IsNullOrEmpty(row[1].ToString()) || ((int)row[0] != 4 && (int)row[0] != 8);
                        _isCheck = _isSingle || (violationID == -1); // || ((int)row[0] != 4 && (int)row[0] != 8);

                        UID = row[2].ToString();

                        Log.LogHelper.Log.DebugFormat("ApplicationID = {0}, IsCheck = {1}", _applicationId, _isCheck);
                    }

                    if (ds.Tables.Count >= 2 && ds.Tables[1].Rows.Count >= 1)
                    {
                        foreach (DataRow row in ds.Tables[1].Rows)
                        {
                            var od = new CheckOlympicData();
                            od.DocID = (int)row[0];
                            od.OlympicID = row[1] as int?;
                            od.OlympicTypeProfileID = row[2] as int?;
                            od.DiplomTypeID = row[3] as int?;
                            od.ClassNumber = row[4] as int?;
                            od.EntranceTestItemID = row[5] as int?;
                            od.CompetitiveGroupID = row[6] as int?;
                            od.UID = row[7].ToString();
                            od.DocumentDate = row[8] as DateTime?;
                            od.DocumentNumber = row[9] as string;
                            _checkOlympics.Add(od);
                        }
                    }
                    if (ds.Tables.Count >= 3 && ds.Tables[2].Rows.Count >= 1)
                    {
                        var row = ds.Tables[2].Rows[0];
                        _isCheckComposition = (int)row[0] > 0;
                    }
                    if (ds.Tables.Count >= 4 && ds.Tables[3].Rows.Count >= 1)
                    {
                        var row = ds.Tables[3].Rows[0];
                        _isCheckEge = (int)row[0] > 0;
                    }

                }
            }
            catch (Exception ex)
            {
                ConnectionException = ex;
            }
        }

        private SqlConnection _connection;
        public Exception ConnectionException { get; private set; }

        /// <summary>
        /// Проверка баллов ЕГЭ
        /// </summary>
        /// <returns></returns>
        public ApplicationCheckResult CheckEgeMarks()
        {
            if (ConnectionException != null)
                return new ApplicationCheckResult(ConnectionException);

            if (!_isCheckEge || !_isCheck)
            {
                Log.LogHelper.Log.DebugFormat("{0} No need to CheckEgeMarks()", _applicationId);
                return null;
            }

            ApplicationCheckResult result = null;
            _swCheckEgeMarks.Start();
            using (SqlCommand cmd = new SqlCommand("FindEntrantEGEMarks", _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = _timeout;

                cmd.Parameters.AddWithValue("method", "ByIdentityDocument");
                cmd.Parameters.AddWithValue("app", _applicationId);
                AddParameter(cmd, "doc", null, SqlDbType.Int, 0, ParameterDirection.Input);
                AddParameter(cmd, "regNum", null, SqlDbType.VarChar, 100, ParameterDirection.Input);
                cmd.Parameters.AddWithValue("refr", false);
                cmd.Parameters.AddWithValue("currentYear", false);
                AddParameter(cmd, "userLogin", _userLogin, SqlDbType.VarChar, 255, ParameterDirection.Input);

                AddParameter(cmd, "errorMessage", null, SqlDbType.VarChar, 4000, ParameterDirection.Output);
                AddParameter(cmd, "violationMessage", null, SqlDbType.VarChar, 4000, ParameterDirection.Output);
                AddParameter(cmd, "violationId", null, SqlDbType.Int, 0, ParameterDirection.Output);
                AddParameter(cmd, "xmlResult_Get", null, SqlDbType.VarChar, -1, ParameterDirection.Output);
                AddParameter(cmd, "xmlResult_Check", null, SqlDbType.VarChar, -1, ParameterDirection.Output);

                try
                {
                    // DEADLOCK fighting!
                    //lock (ADOBaseRepository.DB_LOCK_OBJECT)
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Log.LogHelper.Log.ErrorFormat("{0} CheckEgeMarks: {1}", _applicationId, ex.Message);
                    return new ApplicationCheckResult(String.Format("проверке ЕГЭ заявления {0}", _applicationId), ex);
                }

                result = new ApplicationCheckResult(cmd);
            }
            _swCheckEgeMarks.Stop();
            return result;
        }

        

        /// <summary>
        /// Проверка результатов участия в олимпиадах
        /// </summary>
        /// <returns></returns>
        public ApplicationCheckResult CheckOlympiadMarks(string applicationXml)
        {
            if (ConnectionException != null)
                return new ApplicationCheckResult(ConnectionException);

            if (_checkOlympics.Count == 0 || !_isCheck)
            {
                Log.LogHelper.Log.DebugFormat("{0} No need to CheckOlympicMarks()", _applicationId);
                return null;
            }

            ApplicationCheckResult result = new ApplicationCheckResult();
            _swCheckOlympicMarks.Start();
            result.XmlResult_Get = applicationXml;

            if (_checkOlympics.Count > 0)
            {
                result.XmlResult_Get += "<OlympicDocuments>";
                foreach (var row in _checkOlympics)
                {
                    result.XmlResult_Get += "<OlympicDocument>";
                    result.XmlResult_Get += string.Format(@"
<UID>{0}</UID>
<OlympicID>{1}</OlympicID>
<OlympicDiplomTypeID>{2}</OlympicDiplomTypeID>
<OlympicTypeProfileID>{3}</OlympicTypeProfileID>
<ClassNumber>{4}</ClassNumber>
{5}
{6}
", row.UID, row.OlympicID, row.DiplomTypeID, row.OlympicTypeProfileID, row.ClassNumber
, row.DocumentDate.HasValue ? string.Format("<DocumentDate>{0}</DocumentDate>", row.DocumentDate.Value) : string.Empty
, !string.IsNullOrWhiteSpace(row.DocumentNumber) ? string.Format("<DocumentNumber>{0}</DocumentNumber>", row.DocumentNumber) : string.Empty
);


                    var resCheckOlympic = AddCheckOlympicResult(row);
                    var resCheckBenefit = AddCheckBenefitResult(row);

                    if (resCheckBenefit.Exception != null) 
                    {
                        result.XmlResult_Get += string.Format(@"<IncorrectResults><Comment>{0}</Comment></IncorrectResults>", resCheckBenefit.Exception.Message);
                    }
                    else if (resCheckOlympic.Exception != null)
                    {
                        result.XmlResult_Get += string.Format(@"<IncorrectResults><Comment>{0}</Comment></IncorrectResults>", resCheckOlympic.Exception.Message);
                    }
                    else if (resCheckBenefit.ViolationCode == 0 && resCheckOlympic.ViolationCode == 0)
                    {
                        result.XmlResult_Get += @"<CorrectResults>Проверка успешно пройдена</CorrectResults>";
                    }
                    else
                    {
                        result.XmlResult_Get += @"<IncorrectResults>";
                        if (resCheckOlympic.ViolationCode != 0)
                            result.XmlResult_Get += string.Format(@"<Comment>{0}</Comment>", resCheckOlympic.ViolationMessage);
                        if (resCheckBenefit.ViolationCode != 0)
                            result.XmlResult_Get += string.Format(@"<Comment>{0}</Comment>", resCheckBenefit.ViolationMessage);

                        result.XmlResult_Get += @"</IncorrectResults>";
                    }

                    result.XmlResult_Get += " </OlympicDocument>";
                }
                result.XmlResult_Get += "</OlympicDocuments>";
            }

            //using (SqlCommand cmd = new SqlCommand("FindEntrantOlympicMarksByApplication", _connection))
            //{
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.CommandTimeout = _timeout;

            //    cmd.Parameters.AddWithValue("app", _applicationId).Direction = ParameterDirection.Input;

            //    AddParameter(cmd, "errorMessage", null, SqlDbType.VarChar, 4000, ParameterDirection.Output);
            //    AddParameter(cmd, "violationMessage", null, SqlDbType.VarChar, 4000, ParameterDirection.Output);
            //    AddParameter(cmd, "violationId", null, SqlDbType.Int, 0, ParameterDirection.Output);
            //    AddParameter(cmd, "xmlResult_Get", null, SqlDbType.VarChar, -1, ParameterDirection.Output);
            //    AddParameter(cmd, "xmlResult_Check", null, SqlDbType.VarChar, -1, ParameterDirection.Output);

            //    try
            //    {
            //        cmd.ExecuteNonQuery();
            //    }
            //    catch (Exception ex)
            //    {
            //        return new ApplicationCheckResult(String.Format("проверке результатов олимпиад заявления {0}", _applicationId), ex);
            //    }

            //    result = new ApplicationCheckResult(cmd);
            //}
            _swCheckOlympicMarks.Stop();
            return result;
        }

        private ApplicationCheckResult AddCheckOlympicResult(CheckOlympicData row)
        {
            using (SqlCommand cmd = new SqlCommand("FindEntrantOlympic", _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = _timeout;

                cmd.Parameters.AddWithValue("docId", row.DocID).Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("olympicId", row.OlympicID.HasValue ? (object)row.OlympicID.Value : DBNull.Value).Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("olympicTypeProfileId", row.OlympicTypeProfileID.HasValue ? (object) row.OlympicTypeProfileID.Value : DBNull.Value ).Direction = ParameterDirection.Input;

                AddParameter(cmd, "errorMessage", null, SqlDbType.VarChar, 4000, ParameterDirection.Output);
                AddParameter(cmd, "violationMessage", null, SqlDbType.VarChar, 4000, ParameterDirection.Output);
                AddParameter(cmd, "violationId", null, SqlDbType.Int, 0, ParameterDirection.Output);

                try
                {
                    cmd.ExecuteNonQuery();
                    return new ApplicationCheckResult(cmd);
                }
                catch (Exception ex)
                {
                    Log.LogHelper.Log.ErrorFormat("{0} AddCheckOlympicResult: {1}", _applicationId, ex.Message);
                    return new ApplicationCheckResult(string.Format("проверке результатов олимпиад заявления {0}", _applicationId), ex);
                }
            }
        }

        private ApplicationCheckResult AddCheckBenefitResult(CheckOlympicData row)
        {
            using (SqlCommand cmd = new SqlCommand("ChectBenefitOlympic", _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = _timeout;

                //@entranceTestItemID int, @groupID int, @olympicTypeProfileID int, 
                //@diplomaTypeID int, @olympicID int, @formNumberID int, @docID int,
                cmd.Parameters.AddWithValue("docId", row.DocID).Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("olympicId", row.OlympicID.HasValue ? (object)row.OlympicID.Value : DBNull.Value).Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("olympicTypeProfileId", row.OlympicTypeProfileID.HasValue ? (object)row.OlympicTypeProfileID.Value : DBNull.Value).Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("entranceTestItemID", row.EntranceTestItemID.HasValue ? (object)row.EntranceTestItemID.Value : DBNull.Value).Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("groupID", row.CompetitiveGroupID.HasValue ? (object)row.CompetitiveGroupID.Value : DBNull.Value).Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("diplomaTypeID", row.DiplomTypeID.HasValue ? (object)row.DiplomTypeID.Value : DBNull.Value).Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("formNumberID", row.ClassNumber.HasValue ? (object)row.ClassNumber.Value : DBNull.Value).Direction = ParameterDirection.Input;

                AddParameter(cmd, "errorMessage", null, SqlDbType.VarChar, 4000, ParameterDirection.Output);
                AddParameter(cmd, "violationMessage", null, SqlDbType.VarChar, 4000, ParameterDirection.Output);
                AddParameter(cmd, "violationId", null, SqlDbType.Int, 0, ParameterDirection.Output);

                try
                {
                    cmd.ExecuteNonQuery();
                    return new ApplicationCheckResult(cmd);
                }
                catch (Exception ex)
                {
                    Log.LogHelper.Log.ErrorFormat("{0} AddCheckBenefitResult: {1}", _applicationId, ex.Message);
                    return new ApplicationCheckResult(string.Format("проверке результатов олимпиад заявления {0}", _applicationId), ex);
                }
            }
        }


        /// <summary>
        /// Проверка результатов сочинений
        /// </summary>
        /// <returns></returns>
        public ApplicationCheckResult CheckCompositionMarks()
        {
            if (ConnectionException != null)
                return new ApplicationCheckResult(ConnectionException);

            if (!_isCheckComposition || !_isCheck)
            {
                Log.LogHelper.Log.DebugFormat("{0} No need to CheckCompositionMarks()", _applicationId);
                return null;
            }

            ApplicationCheckResult result = null;
            _swCheckCompositionMarks.Start();
            using (SqlCommand cmd = new SqlCommand("FindEntrantCompositionMarks", _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = _timeout;

                cmd.Parameters.AddWithValue("app", _applicationId).Direction = ParameterDirection.Input;

                AddParameter(cmd, "xmlResult_Get", null, SqlDbType.VarChar, -1, ParameterDirection.Output);

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Log.LogHelper.Log.ErrorFormat("{0} CheckCompositionMarks: {1}", _applicationId, ex.Message);
                    return new ApplicationCheckResult(String.Format("проверке результатов сочинений заявления {0}", _applicationId), ex);
                }

                result = new ApplicationCheckResult(cmd);
            }
            _swCheckCompositionMarks.Stop();
            return result;
        }

        /// <summary>
        /// Проверка числа заявлений абитуриента, поданных  в другие ВУЗы
        /// </summary>
        /// <returns></returns>
        public ApplicationCheckResult CheckEntrantApplicationsCount()
        {
            if (ConnectionException != null)
                return new ApplicationCheckResult(ConnectionException);

            if (!_isCheck)
            {
                Log.LogHelper.Log.DebugFormat("{0} No need to CheckEntrantApplicationsCount()", _applicationId);
                return null;
            }

            ApplicationCheckResult result = null;
            _swCheckEntrantApplicationsCount.Start();
            using (SqlCommand cmd = new SqlCommand("gvuz_ValidateOtherApplicationsCount", _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = _timeout;

                cmd.Parameters.AddWithValue("applicationId", _applicationId).Direction = ParameterDirection.Input;

                AddParameter(cmd, "errorMessage", null, SqlDbType.VarChar, 4000, ParameterDirection.Output);
                AddParameter(cmd, "violationMessage", null, SqlDbType.VarChar, 4000, ParameterDirection.Output);
                AddParameter(cmd, "violationId", null, SqlDbType.Int, 0, ParameterDirection.Output);

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Log.LogHelper.Log.ErrorFormat("{0} CheckApplicationsCountMarks: {1}", _applicationId, ex.Message);
                    return new ApplicationCheckResult(String.Format("проверке количества заявлений, поданных в другие ОО, заявления {0}", _applicationId), ex);
                }

                result = new ApplicationCheckResult(cmd);
            }
            _swCheckEntrantApplicationsCount.Stop();
            return result;
        }

        public void SetApplicationStatusId(int statusId)
        {
            if (ConnectionException != null)
                return;
            string sql = @"
UPDATE [Application] with (Rowlock) 
SET [StatusID] = @statusId, 
[ViolationID] = case when ViolationID = -1 and @statusID = 4 then 0 else ViolationID end 
WHERE [ApplicationID] = @applicationId";
            using (SqlCommand cmd = new SqlCommand(sql, _connection))
            {
                cmd.CommandTimeout = _timeout;

                cmd.Parameters.AddWithValue("applicationId", _applicationId).Direction = ParameterDirection.Input;
                cmd.Parameters.AddWithValue("statusId", statusId).Direction = ParameterDirection.Input;
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch //(Exception ex)
                {
                    throw;
                }
            }
        }

        internal void SetApplicationStatus3()
        {
            if (ConnectionException != null)
                return;
            string sql = @"
UPDATE [Application] with (Rowlock) 
SET [StatusID] = 3
WHERE [ApplicationID] = @applicationId";
            using (SqlCommand cmd = new SqlCommand(sql, _connection))
            {
                cmd.CommandTimeout = _timeout;

                cmd.Parameters.AddWithValue("applicationId", _applicationId).Direction = ParameterDirection.Input;
                cmd.ExecuteNonQuery();
            }
        }
        internal void SetApplicationStatus4()
        {
            if (ConnectionException != null)
                return;
            string sql = @"
UPDATE [Application] with (Rowlock) 
SET [StatusID] = 4, 
[ViolationID] = case when ViolationID = -1 then 0 else ViolationID end 
WHERE [ApplicationID] = @applicationId";
            using (SqlCommand cmd = new SqlCommand(sql, _connection))
            {
                cmd.CommandTimeout = _timeout;

                cmd.Parameters.AddWithValue("applicationId", _applicationId).Direction = ParameterDirection.Input;
                cmd.ExecuteNonQuery();
            }
        }

        public string GetApplicationXML()
        {
            if (ConnectionException != null)
                return null;
            using (SqlCommand cmd = new SqlCommand("SELECT [dbo].[AppToXml] (@appId)", _connection))
            {
                cmd.CommandTimeout = _timeout;

                cmd.Parameters.AddWithValue("@appId", _applicationId).Direction = ParameterDirection.Input;

                try
                {
                    return cmd.ExecuteScalar().ToString();
                }
                catch //(Exception ex)
                {
                    throw;
                }
            }
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
            }
        }

        private void AddParameter(SqlCommand cmd, string name, object value, SqlDbType type, int size, ParameterDirection direction)
        {
            if (value == null)
            {
                value = DBNull.Value;
            }
            SqlParameter parameter = new SqlParameter(name, value);
            parameter.SqlDbType = type;
            parameter.Size = size;
            parameter.Direction = direction;

            cmd.Parameters.Add(parameter);
        }

        private Stopwatch _swCheckEgeMarks = new Stopwatch();
        private Stopwatch _swCheckOlympicMarks = new Stopwatch();
        private Stopwatch _swCheckCompositionMarks = new Stopwatch();
        private Stopwatch _swCheckEntrantApplicationsCount = new Stopwatch();
    }

    public class ApplicationCheckResult
    {
        public ApplicationCheckResult() { }

        public ApplicationCheckResult(SqlCommand cmd)
        {
            if (cmd.Parameters.Contains("errorMessage"))
            {
                ErrorMessage = cmd.Parameters["errorMessage"].Value as string;
            }
            if (cmd.Parameters.Contains("violationMessage"))
            {
                ViolationMessage = cmd.Parameters["violationMessage"].Value as string;
            }
            if (cmd.Parameters.Contains("violationId"))
            {
                ViolationCode = cmd.Parameters["violationId"].Value as int?;
            }

            if (cmd.Parameters.Contains("xmlResult_Get"))
            {
                string result_Get = cmd.Parameters["xmlResult_Get"].Value as string;
                XmlResult_Get = result_Get;
                if ((XmlResult_Get != null) && (XmlResult_Get.Contains("ApplicationNotFound")))
                {
                    ApplicationNotFound = true;
                    XmlResult_Get = null;
                }
            }
            if (cmd.Parameters.Contains("xmlResult_Check"))
            {
                string result_Check = cmd.Parameters["xmlResult_Check"].Value as string;
                XmlResult_Check = result_Check;
            }
        }

        public ApplicationCheckResult(Exception ex)
        {
            Exception = ex;
            throw ex;
        }

        public ApplicationCheckResult(string description, Exception ex)
        {
            Exception = ex;
            throw (new Exception(string.Format("Ошибка при {0}: {1}", description, ex.Message), ex));
        }

        public string ErrorMessage { get; private set; }
        public int? ViolationCode { get; set; }
        public string ViolationMessage { get; set; }

        public string XmlResult_Get { get; set; }
        public string XmlResult_Check { get; private set; }

        public bool ApplicationNotFound { get; private set; }

        public Exception Exception { get; private set; }
    }
}
