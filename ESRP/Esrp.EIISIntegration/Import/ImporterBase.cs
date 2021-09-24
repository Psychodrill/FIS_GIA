using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using Esrp.DB;
using Esrp.DB.Common;
using Esrp.DB.EsrpADODB;
using Esrp.EIISIntegration.EIISClient;
using Esrp.EIISIntegration.Import.Importers;
using Esrp.EIISIntegration.Import.MessageParsers;
using Esrp.Integration.Common;

namespace Esrp.EIISIntegration.Import
{
    internal interface IImporter
    {
        void ImportData();
        string EIISObjectCode { get; }
        string Name { get; }
        int CreatedObjects { get; }
        int UpdatedObjects { get; }
        int DeletedObjects { get; }
        int FailedObjects { get; }
        int SkippedObjects { get; }
        int TotalObjects { get; }

        void Init(string sessionId, BaseService client, string connectionString);
        event EventHandler<MessageEventArgs> OnMessage;
    }

    internal abstract class ImporterBase<DBObjectType> : IImporter where DBObjectType : EntityBase, IEIISIdable, new()
    {
        public event EventHandler<MessageEventArgs> OnMessage;

        protected string sessionId_;
        protected BaseService client_;
        protected string connectionString_;
        protected EsrpADORepository repository_;

        protected Dictionary<string, HashSet<string>> validationMessages_ = new Dictionary<string, HashSet<string>>();
        public IEnumerable<string> ValidationMessages
        {
            get
            {
                return FormatMessages(validationMessages_, false);
            }
        }
        public int ValidationMessagesTotalCount
        {
            get
            {
                return GetMessagesCount(validationMessages_);
            }
        }

        protected Dictionary<string, HashSet<string>> skipMessages_ = new Dictionary<string, HashSet<string>>();
        public IEnumerable<string> SkipMessages
        {
            get
            {
                return FormatMessages(skipMessages_, true);
            }
        }
        public int SkipMessagesTotalCount
        {
            get
            {
                return GetMessagesCount(skipMessages_);
            }
        }

        private int GetMessagesCount(Dictionary<string, HashSet<string>> messages)
        {
            return messages.Sum(x => x.Value.Count);
        }

        private const int SamplesCount = 5;
        private IEnumerable<string> FormatMessages(Dictionary<string, HashSet<string>> messages, bool isSkipReason)
        {
            List<string> result = new List<string>();
            foreach (string messageType in messages.Keys)
            {
                result.Add(String.Format("{0} типа {1} (всего: {2}, примеры: {3})", isSkipReason ? "Причина пропуска" : "Ошибка", messageType, messages[messageType].Count, String.Join(";", messages[messageType].Take(SamplesCount).ToArray())));
            }
            return result;
        }

        private List<IEIISIdable> entitiesToInsert_ = new List<IEIISIdable>();
        private List<IEIISIdable> entitiesToUpdate_ = new List<IEIISIdable>();

        protected bool Initialized { get; private set; }
        public void Init(string sessionId, BaseService client, string connectionString)
        {
            if (String.IsNullOrEmpty(sessionId))
                throw new ArgumentException("sessionId");
            if (client == null)
                throw new ArgumentNullException("client");
            if (String.IsNullOrEmpty(connectionString))
                throw new ArgumentException("connectionString");

            sessionId_ = sessionId;
            client_ = client;
            connectionString_ = connectionString;

            Initialized = true;
        }

        public abstract string EIISObjectCode { get; }
        public abstract string Name { get; }
        public int CreatedObjects { get; private set; }
        public int UpdatedObjects { get; private set; }
        public int DeletedObjects { get; private set; }
        public int FailedObjects { get; private set; }
        public int SkippedObjects { get; private set; }
        public int TotalObjects { get; private set; }

        protected virtual bool AllowInsertObjects { get { return true; } }

        protected virtual bool AllowUpdateObjects { get { return true; } }

        protected virtual bool AllowDeleteObjects { get { return true; } }

        public void ImportData()
        {
            RaiseMessage(String.Format("Обработка объекта {0} начата", EIISObjectCode));

            if (!Initialized)
                throw new InvalidOperationException("Необходимо инициализировать объект");

            BeforeImport();

            string packageId = GetPackageId();

            int partsCount = GetPackagePartsCount(packageId);

            BeforeImportStep();

            RaiseMessage(String.Format("Обработка пакета (всего {0} частей)", partsCount));

            for (int partIndex = 1; partIndex <= partsCount; partIndex++)
            {
                IEnumerable<EIISObject> objects = GetObjects(packageId, partIndex);

                TotalObjects += objects.Count();

                SaveObjects(objects);

                RaiseMessage(String.Format("Обработка части {0} (всего {1} частей) - завершено", partIndex, partsCount));
            }

            if (objectsToRetry_.Any())
            {
                RaiseMessage("Обработка циклических ссылок");
                for (int i = 0; i < 10; i++)
                {
                    if (objectsToRetry_.Any())
                    {
                        BeforeImportStep();
                        IEnumerable<EIISObject> objects = objectsToRetry_.Select(x => x.ObjectToRetry).ToArray();
                        objectsToRetry_.Clear();
                        SaveObjects(objects);
                    }
                }
                RaiseMessage("Обработка циклических ссылок - завершено");
            }

            FailedObjects += objectsToRetry_.Count;
            foreach (RetryInfo retryInfo in objectsToRetry_)
            {
                string message = String.Format("объект с идентификатором {0} невалиден: {1}", retryInfo.EIISId, retryInfo.ErrorMessage.Message);
                if (!validationMessages_.ContainsKey(retryInfo.ErrorMessage.Type))
                {
                    validationMessages_.Add(retryInfo.ErrorMessage.Type, new HashSet<string>());
                }
                if (!validationMessages_[retryInfo.ErrorMessage.Type].Contains(message))
                {
                    validationMessages_[retryInfo.ErrorMessage.Type].Add(message);
                }
            }

            if (AllowDeleteObjects)
            {
                RaiseMessage("Обработка удаленных объектов");
                DeleteObjects();
                RaiseMessage("Обработка удаленных объектов - завершено");
            }

            string summary = null;
            if (ValidationMessages.Count() > 0)
            {
                string validationMessage = String.Join("; ", ValidationMessages.ToArray());
                validationMessage += String.Format("; (всего {0} ошибок валидации)", ValidationMessagesTotalCount);
                summary += "Ошибки валидации: " + validationMessage;
            }
            if (SkipMessages.Count() > 0)
            {
                string skipMessage = String.Join("; ", SkipMessages.ToArray());
                skipMessage += String.Format("; (всего {0} пропущено)", SkipMessagesTotalCount);
                if (!String.IsNullOrEmpty(summary))
                {
                    summary += "; ";
                }
                summary += "Пропущенные объекты: " + skipMessage;
            }

            SetSuccess(packageId);

            RaiseMessage(String.Format("Обработка объекта {0} завершена", EIISObjectCode));

            if (!String.IsNullOrEmpty(summary))
                throw new ImportValidationException(EIISObjectCode, summary);
        }

        protected void RaiseMessage(string message)
        {
            if (OnMessage != null)
            { 
                MessageEventArgs eventArgs = new MessageEventArgs(message);
                OnMessage(this, eventArgs);
            }
        }

        protected void BeforeImport()
        {
            EsrpADORepository.Init(connectionString_);
            EnsureRepository();
            BeforeImportInternal();
        }

        protected virtual void BeforeImportInternal()
        {

        }

        protected void BeforeImportStep()
        {
            ClearExistingCache();
            EnsureRepository();
            BeforeImportStepInternal();
        }
        protected virtual void BeforeImportStepInternal()
        {

        }

        private void EnsureRepository()
        {
            repository_ = EsrpADORepository.Create();
        }

        private string GetPackageId()
        {
            string createPackageResponse = client_.CreatePackage(sessionId_, EIISObjectCode, false, false, null);
            CreatePackageParser createPackageParser = new CreatePackageParser(createPackageResponse);
            if (createPackageParser.ResponseIsError)
                throw new ImportException(EIISObjectCode, createPackageParser.ErrorDescription);

            return createPackageParser.PackageId;
        }

        private int GetPackagePartsCount(string packageId)
        {
            string getPackageMetaResponse = client_.GetPackageMeta(sessionId_, packageId);
            GetPackageMetaParser getPackageMetaParser = new GetPackageMetaParser(getPackageMetaResponse);

            int attempts = 0;
            while (getPackageMetaParser.ResponseIsError)
            {
                attempts++;
                Thread.Sleep(5000);
                if (attempts > 2)
                {
                    Thread.Sleep(30000);
                }
                getPackageMetaResponse = client_.GetPackageMeta(sessionId_, packageId);
                getPackageMetaParser = new GetPackageMetaParser(getPackageMetaResponse);
                if (getPackageMetaParser.ResponseIsCriticalError)
                    throw new ImportException(EIISObjectCode, getPackageMetaParser.ErrorDescription);

                if (attempts > 10)
                    throw new ImportException(EIISObjectCode, getPackageMetaParser.ErrorDescription);
            }

            return getPackageMetaParser.PackagePartsCount;
        }

        private void SetSuccess(string packageId)
        {
            client_.SetOk(sessionId_, packageId);
        }

        private IEnumerable<EIISObject> GetObjects(string packageId, int partIndex)
        {
            string getPackageResult = client_.GetPackage(sessionId_, packageId, partIndex);
            GetPackageParser getPackageParser = new GetPackageParser(getPackageResult);
            if (getPackageParser.ResponseIsError)
                throw new ImportException(EIISObjectCode, getPackageParser.ErrorDescription);

            return getPackageParser.Objects;
        }

        private HashSet<string> preservableEiisIds_ = new HashSet<string>();
        private List<RetryInfo> objectsToRetry_ = new List<RetryInfo>();
        protected virtual void SaveObjects(IEnumerable<EIISObject> objects)
        {
            EnsureRepository();
            foreach (EIISObject eIISObject in objects)
            {
                string eIISId = GetEIISId(eIISObject); 
                if ((!String.IsNullOrEmpty(eIISId)) && (!preservableEiisIds_.Contains(eIISId)))
                {
                    preservableEiisIds_.Add(eIISId);
                }

                ErrorMessage skipMessage;
                bool retry;
                if (SkipObject(eIISObject, out retry, out skipMessage))
                { 
                    if (retry)
                    {
                        objectsToRetry_.Add(new RetryInfo(eIISId, eIISObject, skipMessage));
                    }
                    else
                    {
                        string message = String.Format("объект с идентификатором {0} был пропущен: {1}", eIISId, skipMessage.Message);
                        if (!skipMessages_.ContainsKey(skipMessage.Type))
                        {
                            skipMessages_.Add(skipMessage.Type, new HashSet<string>());
                        }
                        if (!skipMessages_[skipMessage.Type].Contains(message))
                        {
                            skipMessages_[skipMessage.Type].Add(message);
                        }
                        SkippedObjects++;
                    }
                    continue;
                }

                ErrorMessage validationMessage;
                if (!ValidateObject(eIISObject, out validationMessage))
                {
                    string message = String.Format("объект с идентификатором {0} невалиден: {1}", eIISId, validationMessage.Message);
                    if (!validationMessages_.ContainsKey(validationMessage.Type))
                    {
                        validationMessages_.Add(validationMessage.Type, new HashSet<string>());
                    }
                    if (!validationMessages_[validationMessage.Type].Contains(message))
                    {
                        validationMessages_[validationMessage.Type].Add(message);
                    }
                    FailedObjects++;
                    continue;
                }

                DBObjectType existingObject = GetExistingObject(eIISId);
                if (existingObject == null)
                {
                    existingObject = GetExistingObject(eIISObject);
                }

                DBObjectType dbObject = null;
                if (existingObject == null)
                {
                    if (AllowInsertObjects)
                    {
                        dbObject = new DBObjectType();

                        SetEIISIdToDBObject(dbObject, eIISId);

                        SetDBObjectFields(dbObject, eIISObject, true);
                        MarkAsInsertable(dbObject);

                        CreatedObjects++;
                    }
                }
                else
                {
                    dbObject = existingObject;
                    SetEIISIdToDBObject(dbObject, eIISId);

                    if (AllowUpdateObjects)
                    {
                        SetDBObjectFields(dbObject, eIISObject, false);
                    }

                    MarkAsUpdatable(dbObject);
                    UpdatedObjects++;
                }
            }
            try
            {
                foreach (DBObjectType entityToInsert in entitiesToInsert_)
                {
                    repository_.Insert(entityToInsert);
                }
                entitiesToInsert_.Clear();

                foreach (DBObjectType entityToUpdate in entitiesToUpdate_)
                {
                    repository_.Update(entityToUpdate);
                }
                entitiesToUpdate_.Clear();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                if (ex.InnerException != null)
                {
                    message += " " + ex.InnerException.Message;
                }
                throw new ImportException(EIISObjectCode, String.Format("Ошибка при сохранении данных из ЕИИС (подробности: {0})", message));
            }
            finally
            {
                repository_.Dispose();
            }
        }

        protected virtual void DeleteObjects()
        {
            using (SqlConnection connection = new SqlConnection(connectionString_))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "CREATE TABLE #PreservableEiisIds(Eiis_Id nvarchar(50))";
                command.ExecuteNonQuery();

                command.CommandText = "INSERT INTO #PreservableEiisIds(Eiis_Id) VALUES(@id)";
                command.Parameters.Add("id", SqlDbType.NVarChar, 50);
                foreach (string preservableEiisId in preservableEiisIds_)
                {
                    command.Parameters[0].Value = preservableEiisId;
                    command.ExecuteNonQuery();
                }

                command.Parameters.Clear();

                string tableName = repository_.GetTableName<DBObjectType>();

                command.CommandText = String.Format("SELECT Eiis_Id FROM {0} WHERE (Eiis_Id IS NOT NULL) AND (Eiis_Id NOT IN (SELECT Eiis_Id FROM #PreservableEiisIds))", tableName);

                List<string> deletableIds = new List<string>();
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        deletableIds.Add(reader.GetString(0));
                    }
                }
                DeletedObjects = deletableIds.Count; 

                if (tableName == "License")
                {
                    command.CommandText = "DELETE FROM AllowedEducationalDirection WHERE (LicenseSupplementId IN (SELECT Id FROM LicenseSupplement WHERE LicenseId IN (SELECT Id FROM License WHERE (Eiis_Id IS NOT NULL) AND (Eiis_Id NOT IN (SELECT Eiis_Id FROM #PreservableEiisIds)))))";
                    command.CommandText += "DELETE FROM LicenseSupplement WHERE LicenseId IN (SELECT Id FROM License WHERE (Eiis_Id IS NOT NULL) AND (Eiis_Id NOT IN (SELECT Eiis_Id FROM #PreservableEiisIds)))";
                    command.CommandText += "DELETE FROM License WHERE (Eiis_Id IS NOT NULL) AND (Eiis_Id NOT IN (SELECT Eiis_Id FROM #PreservableEiisIds))";
                }
                else if (tableName == "LicenseSupplement")
                {
                    command.CommandText =  "DELETE FROM AllowedEducationalDirection WHERE (LicenseSupplementId IN (SELECT Id FROM LicenseSupplement WHERE (Eiis_Id IS NOT NULL) AND (Eiis_Id NOT IN (SELECT Eiis_Id FROM #PreservableEiisIds))))";
                    command.CommandText +=  "DELETE FROM LicenseSupplement WHERE (Eiis_Id IS NOT NULL) AND (Eiis_Id NOT IN (SELECT Eiis_Id FROM #PreservableEiisIds))";
                }
                else if (tableName == "Founder")
                {
                    command.CommandText = "DELETE FROM OrganizationFounder WHERE (FounderId IN (SELECT Id FROM Founder WHERE (Eiis_Id IS NOT NULL) AND (Eiis_Id NOT IN (SELECT Eiis_Id FROM #PreservableEiisIds))))";
                    command.CommandText += "DELETE FROM Founder WHERE (Eiis_Id IS NOT NULL) AND (Eiis_Id NOT IN (SELECT Eiis_Id FROM #PreservableEiisIds))";
                }
                else if (tableName == "EducationalDirection")
                {
                    command.CommandText = String.Format("UPDATE EducationalDirection SET IsActual = 0 WHERE (Eiis_Id IS NOT NULL) AND (Eiis_Id NOT IN (SELECT Eiis_Id FROM #PreservableEiisIds))", OrganizationsImporter.DeletedStatusId);
                }
                else if (tableName == "EducationalDirectionGroup")
                {
                    command.CommandText = String.Format("UPDATE EducationalDirectionGroup SET IsActual = 0 WHERE (Eiis_Id IS NOT NULL) AND (Eiis_Id NOT IN (SELECT Eiis_Id FROM #PreservableEiisIds))", OrganizationsImporter.DeletedStatusId);
                }
                else if (tableName == "Organization2010")
                {
                    command.CommandText = String.Format("UPDATE Organization2010 SET StatusId = {0} WHERE (Eiis_Id IS NOT NULL) AND (Eiis_Id NOT IN (SELECT Eiis_Id FROM #PreservableEiisIds))", OrganizationsImporter.DeletedStatusId);
                }
                else
                {
                    command.CommandText = String.Format("DELETE FROM {0} WHERE (Eiis_Id IS NOT NULL) AND (Eiis_Id NOT IN (SELECT Eiis_Id FROM #PreservableEiisIds))", tableName);
                }
                command.ExecuteNonQuery();
            }
        }

        protected void ClearExistingCache()
        {
            if (_existingByEiisId != null)
            {
                _existingByEiisId.Clear();
                _existingByEiisId = null;
            }
        }

        private Dictionary<string, DBObjectType> _existingByEiisId;
        protected virtual DBObjectType GetExistingObject(string eIISId)
        {
            if (_existingByEiisId == null)
            {
                _existingByEiisId = new Dictionary<string, DBObjectType>();
                foreach (DBObjectType dbObject in repository_.GetWithNotEmptyEiisId<DBObjectType>())
                {
                    _existingByEiisId.Add(dbObject.Eiis_Id, dbObject);
                }
            }

            if (_existingByEiisId.ContainsKey(eIISId))
                return _existingByEiisId[eIISId];

            return null;
        }

        protected virtual string GetEIISIdFromDBObject(DBObjectType dbObject)
        {
            return dbObject.Eiis_Id;
        }

        protected virtual void SetEIISIdToDBObject(DBObjectType dbObject, string value)
        {
            dbObject.Eiis_Id = value;
        }

        protected void MarkAsInsertable(DBObjectType dbObject)
        {
            entitiesToInsert_.Add(dbObject);
        }

        protected void MarkAsUpdatable(DBObjectType dbObject)
        {
            entitiesToUpdate_.Add(dbObject);
        }

        private bool ValidateObject(EIISObject eIISObject, out ErrorMessage message)
        {
            if (!ValidateRequiredFields(eIISObject, out message))
                return false;

            return ValidateObjectFieldValues(eIISObject, out message);
        }

        private bool ValidateRequiredFields(EIISObject eIISObject, out ErrorMessage message)
        {
            foreach (string requiredField in RequiredFields)
            {
                if (!eIISObject.FieldExists(requiredField))
                {
                    message = new ErrorMessage(ErrorMessage.RequiredFieldMessage, String.Format("у объекта с идентификатором {0} отсутствует поле {1}", GetEIISId(eIISObject), requiredField));
                    return false;
                }
                if (String.IsNullOrEmpty(eIISObject.GetFieldStringValue(requiredField)))
                {
                    message = new ErrorMessage(ErrorMessage.RequiredFieldMessage, String.Format("у объекта с идентификатором {0} не заполнено поле {1}", GetEIISId(eIISObject), requiredField));
                    return false;
                }
            }

            message = null;
            return true;
        }

        protected virtual bool SkipObject(EIISObject eIISObject, out bool retry, out ErrorMessage message)
        {
            message = null;
            retry = false;
            return false;
        }

        protected virtual bool ValidateObjectFieldValues(EIISObject eIISObject, out ErrorMessage message)
        {
            message = null;
            return true;
        }

        protected virtual string GetEIISId(EIISObject eIISObject)
        {
            return eIISObject.PrimaryKeyFields.First().Value;
        }

        protected IEnumerable<DBObjectType> GetWithNotEmptyEiisId()
        {
            return repository_.GetWithNotEmptyEiisId<DBObjectType>();
        }

        protected abstract void SetDBObjectFields(DBObjectType dbObject, EIISObject eIISObject, bool isNew);

        protected abstract DBObjectType GetExistingObject(EIISObject eIISObject);

        protected abstract IEnumerable<string> RequiredFields { get; }
    }
}
