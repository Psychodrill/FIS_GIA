using GVUZ.ImportService2016.Core.Dto.Partial;
using GVUZ.ImportService2016.Core.Main.Dictionaries;
using GVUZ.ImportService2016.Core.Main.Log;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace GVUZ.ImportService2016.Core.Main.Repositories
{
    public class ADOPackageRepository
    {
        public static void GetStaticVocabularies()
        {
            var sw = new Stopwatch();
            try
            {
                var _connectionString = ConfigurationManager.ConnectionStrings["Main"].ConnectionString;
                GVUZ.DAL.Dapper.Repository.Model.GvuzRepository.Initialize(_connectionString);

                using (var connection = new SqlConnection(_connectionString))
                {
                    sw.Start();

                    using (SqlCommand cmd = new SqlCommand("GetStaticVocabularies", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 60000; // todo import: вынести в настройки

                        connection.Open();
                        var ds = new DataSet();
                        var adapter = new SqlDataAdapter(cmd);

                        adapter.Fill(ds);

                        if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                            return;

                        // Fill static Vocabularies
                        VocabularyStatic.Init(ds);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(ex.Message, ex);
                throw;
            }
        }

        public static void ReleasePackage(int PackageID)
        {
            var sw = new Stopwatch();
            var sql = @"Update ImportPackage Set StatusID=1, comment = '', processresultinfo='', InProgressDate = null, CompleteDate = null Where PackageID = @PackageID";
            try
            {
                var _connectionString = ConfigurationManager.ConnectionStrings["Main"].ConnectionString;

                using (var connection = new SqlConnection(_connectionString))
                {
                    sw.Start();

                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 60;

                        cmd.Parameters.Add(new SqlParameter("@PackageID", PackageID));

                        connection.Open();
                        cmd.ExecuteNonQuery();
                    }

                    sw.Stop();
                    var message = " № {1}. Время освобождения пакета: {0} сек";
                    LogHelper.Log.InfoFormat(message, sw.Elapsed.TotalSeconds, PackageID);
                    Console.WriteLine(message, sw.Elapsed.TotalSeconds, PackageID);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(ex.Message, ex);
                throw;
            }
        }

        public static ImportPackage GetUnprocessedPackage(int PackageID)
        {
            var sw = new Stopwatch();
            var res = new ImportPackage();

            try
            {
                var _connectionString = ConfigurationManager.ConnectionStrings["Main"].ConnectionString;

                using (var connection = new SqlConnection(_connectionString))
                {

                    sw.Start();

                    using (SqlCommand cmd = new SqlCommand("GetNextPackage", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 60000; // todo import: вынести в настройки

                        if (PackageID != 0)
                            cmd.Parameters.Add(new SqlParameter("@PackageID", PackageID));

                        connection.Open();
                        var ds = new DataSet();
                        var adapter = new SqlDataAdapter(cmd);

                        adapter.Fill(ds);

                        if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                            return null;

                        // Get ImportPackage
                        var ipRow = ds.Tables[0].Rows[0];
                        res.PackageData = ipRow["PackageData"].ToString();
                        res.InstitutionID = (int)ipRow["InstitutionID"];
                        res.PackageID = (int)ipRow["PackageID"];
                        res.StatusID = ipRow["StatusID"] != null ? (int)ipRow["StatusID"] : 1;
                        res.CheckStatusID = ipRow["CheckStatusID"] != DBNull.Value ? (int)ipRow["CheckStatusID"] : 0;
                        res.ImportedAppIDs = ipRow["ImportedAppIDs"] != null ? ipRow["ImportedAppIDs"].ToString() : "";
                        res.TypeID = (int)ipRow["TypeID"];

                        if (res.TypeID == 101)
                            res.TypeID = (int)GVUZ.ServiceModel.Import.Core.Packages.PackageType.Import;
                        //res.UserLogin = ipRow["UserLogin"].ToString();

                        VocabularyStorage vocabularyStorage = new VocabularyStorage(ds);
                        res.VocabularyStorage = vocabularyStorage;
                    }

                    sw.Stop();
                    var message = " № {1}, ОО {2}. Время получения данных для импорта: {0} сек";
                    LogHelper.Log.InfoFormat(message, sw.Elapsed.TotalSeconds, res.PackageID, res.InstitutionID);
                    Console.WriteLine(message, sw.Elapsed.TotalSeconds, res.PackageID, res.InstitutionID);
                    return res;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(ex.Message, ex);
                throw;
            }
        }


        public static ImportPackage GetUncheckedPackage(int PackageID)
        {
            var sw = new Stopwatch();
            var res = new ImportPackage();

            try
            {
                var _connectionString = ConfigurationManager.ConnectionStrings["Main"].ConnectionString;

                using (var connection = new SqlConnection(_connectionString))
                {

                    sw.Start();

                    using (SqlCommand cmd = new SqlCommand("GetNextUncheckedPackage", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 60000; // todo import: вынести в настройки

                        if (PackageID != 0)
                            cmd.Parameters.Add(new SqlParameter("@PackageID", PackageID));

                        connection.Open();
                        var ds = new DataSet();
                        var adapter = new SqlDataAdapter(cmd);

                        adapter.Fill(ds);

                        if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                            return null;

                        // Get ImportPackage
                        var ipRow = ds.Tables[0].Rows[0];
                        res.PackageData = ipRow["PackageData"].ToString();
                        res.InstitutionID = (int)ipRow["InstitutionID"];
                        res.PackageID = (int)ipRow["PackageID"];
                        res.StatusID = ipRow["StatusID"] != null ? (int)ipRow["StatusID"] : 1;
                        res.CheckStatusID = ipRow["CheckStatusID"] != DBNull.Value ? (int)ipRow["CheckStatusID"] : 0;
                        res.ImportedAppIDs = ipRow["ImportedAppIDs"] != null ? ipRow["ImportedAppIDs"].ToString() : "";
                        res.TypeID = (int)ipRow["TypeID"];

                        if (res.TypeID == 101)
                            res.TypeID = (int)GVUZ.ServiceModel.Import.Core.Packages.PackageType.Import;
                    }

                    sw.Stop();
                    var message = " № {1}, ОО {2}. Время получения данных для проверки: {0} сек";
                    LogHelper.Log.InfoFormat(message, sw.Elapsed.TotalSeconds, res.PackageID, res.InstitutionID);
                    Console.WriteLine(message, sw.Elapsed.TotalSeconds, res.PackageID, res.InstitutionID);
                    return res;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(ex.Message, ex);
                throw;
            }
        }

        public static List<ImportPackage> GetUncheckedPackages()
        {
            var sw = new Stopwatch();
            var result = new List<ImportPackage>();

            try
            {
                var _connectionString = ConfigurationManager.ConnectionStrings["Main"].ConnectionString;
                using (var connection = new SqlConnection(_connectionString))
                {
                    sw.Start();
                    using (SqlCommand cmd = new SqlCommand("GetNextUncheckedPackages", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 60000; // todo import: вынести в настройки
                        connection.Open();
                        var ds = new DataSet();
                        var adapter = new SqlDataAdapter(cmd);

                        adapter.Fill(ds);

                        if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                            return result;

                        // Get ImportPackage
                        foreach (DataRow ipRow in ds.Tables[0].Rows)
                        {
                            //var ipRow = ds.Tables[0].Rows[0];
                            var res = new ImportPackage();
                            res.PackageData = ipRow["PackageData"].ToString();
                            res.InstitutionID = (int)ipRow["InstitutionID"];
                            res.PackageID = (int)ipRow["PackageID"];
                            res.StatusID = ipRow["StatusID"] != null ? (int)ipRow["StatusID"] : 1;
                            res.CheckStatusID = ipRow["CheckStatusID"] != DBNull.Value ? (int)ipRow["CheckStatusID"] : 0;
                            res.ImportedAppIDs = ipRow["ImportedAppIDs"] != null ? ipRow["ImportedAppIDs"].ToString() : "";
                            res.TypeID = (int)ipRow["TypeID"];

                            if (res.TypeID == 101)
                                res.TypeID = (int)GVUZ.ServiceModel.Import.Core.Packages.PackageType.Import;
                            result.Add(res);
                        }
                    }

                    sw.Stop();
                    var message = "GetNextUncheckedPackages: загружено {1} пакетов. Время получения данных для проверки: {0} сек";
                    LogHelper.Log.InfoFormat(message, sw.Elapsed.TotalSeconds, result.Count);
                    //Console.WriteLine(message, sw.Elapsed.TotalSeconds, res.PackageID, res.InstitutionID);
                    return result;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(ex.Message, ex);
                throw;
            }
        }

        public static void ResetDeadlockImportPackage(int importPackageId)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Main"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                int iRes;
                string sql = @"Update ImportPackage 
                    Set StatusID=1, CheckStatusID=1
                    Where PackageId = @importPackageId;";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 60000;

                    cmd.Parameters.Add(new SqlParameter("@importPackageId", importPackageId));
                    iRes = cmd.ExecuteNonQuery();
                }
            }
        }

        public static void ResetDeadlockCheckPackage(int importPackageId)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Main"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                int iRes;
                string sql = @"Update ImportPackage 
                    Set CheckStatusID=1, CheckResultInfo=null
                    Where PackageId = @importPackageId;";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 60000;

                    cmd.Parameters.Add(new SqlParameter("@importPackageId", importPackageId));
                    iRes = cmd.ExecuteNonQuery();
                }
            }
        }

        public static void ResetImportPackages()
        {
            ResetImportPackages(true, true, true, 0);
        }

        public static void ResetImportPackages(bool resetStatus, bool resetCheckStatus, bool deleteBulk, int packageID)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Main"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "";

                if (resetStatus)
                {
                    sql = @"
Update ImportPackage 
Set StatusID = 1,
CheckStatusID = null,
ProcessResultInfo = null,
CheckResultInfo = null,
LastDateChanged = GETDATE(),
ImportedAppIDs = null,
Comment = ''
where StatusID=2 and CreateDate>'" + new DateTime(DateTime.Now.Year, 1, 1).ToString("yyyyMMdd") + "';";
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 600000;
                        var res = cmd.ExecuteNonQuery();
                        LogHelper.Log.InfoFormat("Сброс StatusID пакетов с 2 на 1 - успешно произведен у {0} пакетов", res);
                    }
                }

                if (resetCheckStatus)
                {
                    sql = @"
Update ImportPackage 
Set CheckStatusID = 1
where CheckStatusID = 2 and CreateDate>'" + new DateTime(DateTime.Now.Year, 1, 1).ToString("yyyyMMdd") + "';";
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 600000;
                        var res = cmd.ExecuteNonQuery();
                        LogHelper.Log.InfoFormat("Сброс CheckStatusID пакетов с 2 на 1 - успешно произведен у {0} пакетов", res);
                    }
                }

                if (deleteBulk)
                {
                    string where = packageID != 0 ? " Where ImportPackageId = " + packageID : "";

                    sql = string.Format(@"
delete from bulk_AdmissionVolume{0};
delete from bulk_ApplicationCompetitiveGroupItem{0};
delete from bulk_Application{0};
delete from bulk_ApplicationEntranceTestDocument{0};
delete from bulk_ApplicationIndividualAchievements{0};

delete from bulk_ApplicationSelectedCompetitiveGroup{0};
delete from bulk_ApplicationSelectedCompetitiveGroupItem{0};
delete from bulk_ApplicationSelectedCompetitiveGroupTarget{0};

delete from bulk_ApplicationShortUpdate{0};

delete from bulk_BenefitItemC{0};
delete from bulk_BenefitItemData{0};

delete from bulk_Campaign{0};
delete from bulk_CampaignDate{0};
delete from bulk_CompetitiveGroup{0};
delete from bulk_CompetitiveGroupItem{0};
delete from bulk_CompetitiveGroupProgram{0};
delete from bulk_CompetitiveGroupTarget{0};
delete from bulk_CompetitiveGroupTargetItem{0};
delete from bulk_InstitutionProgram{0};

delete from bulk_Delete{0};
delete from bulk_DistributedAdmissionVolume{0};

delete from bulk_EntranceTestItemC{0};
delete from bulk_Entrant{0};
delete from bulk_EntrantDocument{0};
delete from bulk_EntrantDocumentSubject{0};

delete from bulk_InstitutionAchievements{0};
delete from bulk_OrderOfAdmission{0};
"
                        , where);

                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 600000;
                        var res = cmd.ExecuteNonQuery();
                        LogHelper.Log.InfoFormat("Очистка балк-таблиц успешно произведена - удалено {0} записей", res);
                    }
                }
            }
        }

        public static void UpdateImportPackage(GVUZ.ServiceModel.Import.Package.ImportResultPackage resultPackage, string applicationIds, int importPackageId, string comment)
        {
            string resXml = string.IsNullOrEmpty(comment) ?
                GVUZ.ServiceModel.Import.Package.PackageHelper.GenerateXmlPackageIntoString(resultPackage)
                : "<Error xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><ErrorCode>0</ErrorCode><ErrorText>Произошла ошибка во время обработки пакета</ErrorText></Error>";
            UpdateImportPackage(resXml, applicationIds, importPackageId, comment);
        }

        public static void UpdateImportPackage(string resXml, string applicationIds, int importPackageId, string comment)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Main"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                //string resXml = GVUZ.ServiceModel.Import.Package.PackageHelper.GenerateXmlPackageIntoString(resultPackage);

                int iRes;
                string sql = @"Update ImportPackage With (ROWLOCK)
                    Set StatusID=3,
                    CheckStatusID = @checkStatusID,
                    ProcessResultInfo = @resultInfo,
                    LastDateChanged = GETDATE(),
                    ImportedAppIDs = @importedAppIDs,
                    Comment = @comment
                    where PackageId = @importPackageId;";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 600000; // todo import: вынести в настройки

                    cmd.Parameters.Add(new SqlParameter("@resultInfo", string.IsNullOrEmpty(comment) ? resXml : (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@importedAppIDs", !string.IsNullOrEmpty(applicationIds) ? applicationIds : (object)DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@importPackageId", importPackageId));
                    cmd.Parameters.Add(new SqlParameter("@comment", comment));
                    cmd.Parameters.Add(new SqlParameter("@checkStatusID", !string.IsNullOrEmpty(applicationIds) ? 1 : (object)DBNull.Value)); // 2
                    iRes = cmd.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateImportPackageCheckResult(int packageId, string resultXml, string comment)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Main"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = @"
UPDATE 
    ImportPackage With (ROWLOCK)
SET
    CheckResultInfo = @checkResultInfo
    ,CheckStatusID = 3
    ,LastDateChanged = GETDATE()
WHERE
    PackageId = @packageID";
                //     ,StatusID = 3

                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.CommandTimeout = 600000; // todo import: вынести в настройки

                    cmd.Parameters.Add(new SqlParameter("@packageID", packageId));
                    cmd.Parameters.Add(new SqlParameter("@checkResultInfo", !String.IsNullOrEmpty(resultXml) ? resultXml : (object)DBNull.Value));

                    cmd.ExecuteNonQuery();
                }

                if (!string.IsNullOrEmpty(comment))
                {
                    sql = @"
UPDATE 
    ImportPackage With (ROWLOCK)
SET
    Comment = @comment
WHERE
    PackageId = @packageID AND (Comment is null OR Comment = '')";

                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.CommandTimeout = 600000; // todo import: вынести в настройки

                        cmd.Parameters.Add(new SqlParameter("@packageID", packageId));
                        cmd.Parameters.Add(new SqlParameter("@comment", comment));

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public static Tuple<DataSet, List<string>> BulkInsertData(GVUZ.ImportService2016.Core.Dto.Import.PackageData packageData,
                                                List<Tuple<string, IDataReader>> bulks,
                                                string StoredProcedureName,
                                                bool deleteBulk,
                                                log4net.ILog c_logger = null)
        {
            var repo = new ADOPackageRepository();
            return repo.BulkInsertData(packageData.InstitutionId, packageData.ImportPackageId, bulks, StoredProcedureName, deleteBulk, c_logger);
        }

        List<string> errors = new List<string>();
        public Tuple<DataSet, List<string>> BulkInsertData(
                                                int institutionID,
                                                int importPackageId,
                                                List<Tuple<string, IDataReader>> bulks,
                                                string StoredProcedureName,
                                                bool deleteBulk,
                                                log4net.ILog c_logger = null)
        {
            errors = new List<string>();
            Stopwatch sw = new Stopwatch();
            // Строка подключения.
            var connectionString = ConfigurationManager.ConnectionStrings["Main"].ConnectionString;
            string ipt_s = "300";
            if (System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains("ImportPackageTimeout"))
                ipt_s = System.Configuration.ConfigurationManager.AppSettings["ImportPackageTimeout"];
            int timeout = 0;
            if (!int.TryParse(ipt_s, out timeout))
                timeout = 300;
            if (c_logger != null)
                c_logger.DebugFormat("Установлено ограничение на массовые операции копирования: {0} c.", timeout);

            // Создаем объект загрузчика SqlBulkCopy, указываем таблицу назначения и загружаем.
            try
            {
                //using (var loader = new SqlBulkCopy(cnt, SqlBulkCopyOptions.CheckConstraints, trn))
                using (var loader = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.CheckConstraints))
                {
                    foreach (var bulk in bulks)
                    {
                        loader.BulkCopyTimeout = timeout;
                        loader.DestinationTableName = bulk.Item1;
                        loader.WriteToServer(bulk.Item2);
                    }
                }
                //trn.Commit();
                //cnt.Close();


                var ds = new DataSet();
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.InfoMessage += (sndr, evt) =>
                    {
                        List<int> error_codes = new List<int>();
                        int error_count = 0;
                        foreach (System.Data.SqlClient.SqlError msg in evt.Errors)
                        {
                            error_count++;
                            string error = string.Format("{0} {1} {2}: {3}", msg.Source, msg.State, msg.Number, msg.Message);
                            Console.WriteLine(error);
                            //messages.Add(error);
                            if (c_logger != null)
                            {
                                if (msg.State >= 10)
                                    c_logger.ErrorFormat("Пакет №{0} -> {1} [{2} {3}]: {4}", importPackageId, msg.Source, msg.State, msg.Number, msg.Message);
                                else
                                    c_logger.DebugFormat("Пакет №{0} -> {1} [{2} {3}]: {4}", importPackageId, msg.Source, msg.State, msg.Number, msg.Message);

                            }
                        }
                        return;
                    };
                    if (c_logger != null)
                    {
                        c_logger.DebugFormat("Импорт пакета №{0} (таблицы очищаются? - {1})", importPackageId, deleteBulk ? "да" : "нет");
                    }
                    connection.Open();
                    sw.Start();
                    using (SqlCommand cmd = new SqlCommand(StoredProcedureName, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 600000; // todo import: вынести в настройки

                        cmd.Parameters.Add(new SqlParameter("@InstitutionId", institutionID));
                        cmd.Parameters.Add(new SqlParameter("@ImportPackageId", importPackageId));
                        if (!deleteBulk)
                            cmd.Parameters.Add(new SqlParameter("@DeleteBulk", deleteBulk));

                        var adapter = new SqlDataAdapter(cmd);
                        //adapter.FillError += adapter_FillError;
                        adapter.FillError += new FillErrorEventHandler(adapter_FillError);

                        adapter.ContinueUpdateOnError = true;

                        // DEADLOCK fighting 
                        //lock (ADOBaseRepository.DB_LOCK_OBJECT)
                        {
                            adapter.Fill(ds);
                        }
                    }
                    sw.Stop();

                    var message = "№ {1} " + StoredProcedureName + " - {0} сек";
                    Console.WriteLine(message, sw.Elapsed.TotalSeconds, importPackageId);
                    LogHelper.Log.InfoFormat(message, sw.Elapsed.TotalSeconds, importPackageId);
                }
                return new Tuple<DataSet, List<string>>(ds, errors);
            }
            catch (Exception ex)
            {
                if (c_logger != null)
                {
                    StringBuilder message = new StringBuilder($"Import package error: {ex.Message}\n");

                    bool hasInner = false;
                    Exception inner = ex;
                    while (inner.InnerException != null)
                    {
                        inner = inner.InnerException;
                        hasInner = true;
                    }

                    if (hasInner)
                    {
                        message.Append($"Inner Exception: {inner.Message}\n");
                        message.Append($"StackTrace: {inner.StackTrace}\n");
                    }
                    c_logger.Error(message.ToString());

                }
                throw ex;
            }
        }

        protected static void FillError(object sender, FillErrorEventArgs args)
        {

        }
        private void adapter_FillError(object sender, FillErrorEventArgs e)
        {
            errors.Add(e.Errors.Message);
            e.Continue = true;
        }

        public static Tuple<DataSet, List<string>> BulkDeleteData(Dto.Delete.DataForDelete dataForDelete,
                                                                  List<Tuple<string, IDataReader>> bulks,
                                                                  string storedProcedureName,
                                                                  bool deleteBulk,
                                                                  log4net.ILog c_logger = null)
        {
            var repo = new ADOPackageRepository();
            return repo.BulkInsertData(dataForDelete.InstitutionId, dataForDelete.ImportPackageId, bulks, storedProcedureName, deleteBulk, c_logger);
        }


    }
}
