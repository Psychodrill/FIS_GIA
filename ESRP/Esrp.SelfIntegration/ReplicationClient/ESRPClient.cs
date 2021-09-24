using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using Esrp.DB;
using Esrp.Integration.Common;

namespace Esrp.SelfIntegration.ReplicationClient
{
    internal class ESRPClient
    {
        private HashSet<string> failedRecords_ = new HashSet<string>();

        private ESRPServiceClientFactory clientFactory;
        private string connectionString_;
        private int rowCount_;
        private bool useEnsureCommands_;
        public ESRPClient(string serviceUrl, string connectionString, int rowCount, bool useEnsureCommands)
        {
            if (rowCount == 0)
                throw new ArgumentException("rowCount");

            clientFactory = new ESRPServiceClientFactory(serviceUrl);
            connectionString_ = connectionString;
            rowCount_ = rowCount;
            useEnsureCommands_=useEnsureCommands;
        }

        public void RunReplication()
        {
            Prepare();
            try
            {
                using (EsrpRepository repository = EsrpRepository.Create())
                {
                    int count = repository.DataContext.ReplicationDatas.Count();
                    int packagesCount = (int)Math.Floor((decimal)count / rowCount_);
                    packagesCount += ((count % rowCount_ == 0) ? 0 : 1);
                    RaiseMessage(String.Format("Требуется обработать {0} записей ({1} пакетов)", count, packagesCount));
                }
                while (ProcessBatch()) { }
            }
            catch (Exception ex)
            {
                RaiseCriticalError(ex);
            }
        }

        private bool ProcessBatch()
        {
            using (EsrpRepository repository = EsrpRepository.Create())
            {
                string sql = String.Format(@"
SELECT TOP {0} * FROM ReplicationData
ORDER BY
CASE ReplicationType 
	WHEN 'A' THEN 999
    WHEN 'B' THEN 999
	ELSE 0
END,
CASE ReplicationTableName 
    WHEN 'rpt_ConnectStatV2' THEN 0
    WHEN 'Region' THEN 1
    WHEN 'OrganizationStatusEIISMap' THEN 2
    WHEN 'OrganizationKindEIISMap' THEN 3
    WHEN 'EducationalLevelEIISMap' THEN 4
    WHEN 'FounderType' THEN 5
    WHEN 'EducationalDirectionType' THEN 6
    WHEN 'EducationalDirectionGroup' THEN 7

    WHEN 'Account' THEN 50
    WHEN 'Group' THEN 51

    WHEN 'GroupAccount' THEN 100
    WHEN 'GroupRole' THEN 101
    WHEN 'UserAccountPassword' THEN 102    
    
    WHEN 'EducationalDirection' THEN 200

    WHEN 'Organization2010' THEN 300
    WHEN 'OrganizationRequest2010' THEN 301
    WHEN 'OrganizationRequestAccount' THEN 302

    WHEN 'Founder' THEN 400
    WHEN 'OrganizationFounder' THEN 401

    WHEN 'OrganizationLimitation' THEN 450

    WHEN 'License' THEN 500
    WHEN 'License_ISLOD' THEN 501
    WHEN 'LicenseSupplement' THEN 502
    WHEN 'AllowedEducationalDirection' THEN 503

    ELSE 999
END,
Id
", rowCount_);
             
                repository.DataContext.ExecuteQuery<ReplicationData>(sql);
                IEnumerable<ReplicationData> records = repository.DataContext.ExecuteQuery<ReplicationData>(sql)
                    .ToArray()
                    .Where(x => !failedRecords_.Contains(GetRecordKey(x)))
                    .ToArray();

                RaiseMessage(String.Format("Осталось {0} записей", repository.DataContext.ReplicationDatas.Count() - failedRecords_.Count));

                if (!records.Any())
                    return false; 

                XmlElement batch = GetBatch(repository.DataContext.Connection, records);
                string errorInfo;

                long[] failedIds;
                try
                {
                    failedIds = clientFactory.Client.ApplyBatch(batch, out errorInfo); 
                }
                catch (Exception ex)
                {
                    throw new Exception(String.Format("Ошибка при обращении к веб-сервису ЕСРП: {0}", ex.Message), ex);
                }
                
                repository.DataContext.ReplicationDatas.DeleteAllOnSubmit(records);
                repository.DataContext.SubmitChanges();
                 
                if (failedIds != null)
                {
                    RaiseMessage(String.Format("Не удалось обработать {0} записей, они будут обработаны повторно", failedIds.Count()));
                    foreach (ReplicationData failedRecord in records.Where(x => (failedIds.Contains(x.Id))))
                    {
                        string failedRecordKey = GetRecordKey(failedRecord);
                        if (!failedRecords_.Contains(failedRecordKey))
                        {
                            failedRecords_.Add(failedRecordKey);
                        }

                        ReplicationData retryRecord = new ReplicationData();
                        retryRecord.ReplicationTableName = failedRecord.ReplicationTableName;
                        retryRecord.ReplicationRecordId = failedRecord.ReplicationRecordId;
                        if ((failedRecord.ReplicationType == CommandTypes.Delete) || (failedRecord.ReplicationType == CommandTypes.RetryDelete))
                        {
                            retryRecord.ReplicationType = CommandTypes.RetryDelete;
                        }
                        else
                        {
                            retryRecord.ReplicationType = CommandTypes.RetryEnsure;
                        }
                        repository.DataContext.ReplicationDatas.InsertOnSubmit(retryRecord);
                    }
                }  
                repository.DataContext.SubmitChanges();

                if (!String.IsNullOrEmpty(errorInfo))
                {
                    RaiseMessage(errorInfo);
                } 
                 
                return true;
            }
        }

        private string GetRecordKey(ReplicationData record)
        {
            return String.Format("{0}_{1}", record.ReplicationTableName, record.ReplicationRecordId);
        }

        private XmlElement GetBatch(IDbConnection connection, IEnumerable<ReplicationData> records)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            StringBuilder resultStr = new StringBuilder();
            resultStr.AppendLine("<Batch>");
            foreach (ReplicationData record in records)
            {
                char replicationType = record.ReplicationType;
                if (replicationType == CommandTypes.RetryDelete)
                {
                    replicationType = CommandTypes.Delete;
                }
                else if (replicationType == CommandTypes.RetryEnsure)
                {
                    replicationType = CommandTypes.Ensure;
                }

                if (replicationType == CommandTypes.Delete)
                {
                    resultStr.AppendFormat("<{0} id=\"{1}\" command=\"{2}\" recordId=\"{3}\" />", record.ReplicationTableName, record.Id, replicationType, record.ReplicationRecordId).AppendLine();
                }
                else if ((replicationType == CommandTypes.Insert)
                        ||(replicationType == CommandTypes.Update)
                        ||(replicationType == CommandTypes.Ensure))
                {
                    IDbCommand command = connection.CreateCommand();
                    string idColumn = Hardcoded.GetESRPIdColumnName(record.ReplicationTableName);
                    command.CommandText = String.Format("SELECT * FROM [{0}] WHERE [{1}] = @id", record.ReplicationTableName, idColumn);

                    IDbDataParameter paramter = command.CreateParameter();
                    paramter.ParameterName = "id";
                    paramter.Value = record.ReplicationRecordId;
                    command.Parameters.Add(paramter);
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        Schema schema = new Schema(reader);
                        if (reader.Read())
                        {
                            if (useEnsureCommands_)
                            {
                                resultStr.AppendFormat("<{0} id=\"{1}\" command=\"{2}\" recordId=\"{3}\" >", record.ReplicationTableName, record.Id, CommandTypes.Ensure, record.ReplicationRecordId).AppendLine();
                            }
                            else
                            {
                                resultStr.AppendFormat("<{0} id=\"{1}\" command=\"{2}\" recordId=\"{3}\" >", record.ReplicationTableName, record.Id, replicationType, record.ReplicationRecordId).AppendLine();
                            }                            
                            resultStr.AppendFormat("<Data ");
                            foreach (SchemaColumn column in schema.Columns)
                            {
                                if (column.Name == idColumn)
                                    continue;

                                object value = reader[column.Name];
                                string valueStr = GetStringValue(column, value);
                                resultStr.AppendFormat("{0}=\"{1}\" ", column.Name, valueStr);
                            }
                            resultStr.AppendFormat("></Data>").AppendLine();
                            resultStr.AppendFormat("</{0}>", record.ReplicationTableName).AppendLine();
                        }
                    }
                }
            }
            resultStr.AppendLine("</Batch>");
            XmlDocument result = new XmlDocument();
            result.LoadXml(resultStr.ToString());
            return result.DocumentElement;
        }

        private string GetStringValue(SchemaColumn column, object value)
        {
            if ((value == null) || (value == DBNull.Value))
                return OtherConstants.Null;

            switch (column.SqlDataType)
            {
                case OtherConstants.SqlDataTypeNames.Image:
                case OtherConstants.SqlDataTypeNames.VarBinary:
                    return Convert.ToBase64String((byte[])value);
                case OtherConstants.SqlDataTypeNames.DateTime:
                    return ((DateTime)value).ToString(CultureInfo.InvariantCulture);
                default:
                    return XmlStrings.Escape(value.ToString());
            }
        }

        private bool prepared_;
        private void Prepare()
        {
            if (prepared_)
                return;

            try
            {
                EsrpRepository.Init(connectionString_);
                clientFactory.CreateClient();
            }
            catch (Exception ex)
            {
                RaiseCriticalError(ex);
                return;
            }

            prepared_ = true;
        }

        private void RaiseCriticalError(Exception ex)
        {
            if (CriticalError != null)
            {
                CriticalError(this, new ExceptionEventArgs(ex));
            }
        }

        private void RaiseMessage(string message)
        {
            if (Message != null)
            {
                Message(this, new MessageEventArgs(message));
            }
        }

        public event EventHandler<ExceptionEventArgs> CriticalError;
        public event EventHandler<MessageEventArgs> Message;
    }
}
