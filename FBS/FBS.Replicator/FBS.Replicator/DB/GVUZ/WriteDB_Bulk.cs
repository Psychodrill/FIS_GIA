using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using FBS.Replicator.Entities;
using FBS.Replicator.Entities.GVUZ;
using FBS.Replicator.Entities.FBS;
using FBS.Replicator.Replication.FBSToGVUZ;
using FBS.Common;

namespace FBS.Replicator.DB.GVUZ
{
    public class GVUZWriteDB_Bulk
    {
        private const int BulkSize = 10000;
        private const int MessageSize = 10000;
        private const int CommandTimeout = 600;
        private const int DBOperationAttempts = 5;

        public GVUZWriteDB_Bulk()
        {
            _personsToInsert = new List<FBSPerson>();
            _personsToUpdate = new List<FBSPerson>();

            _documentsToInsert = new List<FBSIdentityDocument>();
        }

        private List<FBSPerson> _personsToInsert;
        private List<FBSPerson> _personsToUpdate;

        private List<FBSIdentityDocument> _documentsToInsert;

        public bool HasErrors { get; private set; }

        public bool WriteChanges(Dictionary<int, IEnumerable<FBSPerson>> fbsPersons)
        {
            using (SqlConnection connection = Connections.CreateGVUZConnection())
            {
                int counter = 0;
                int bulkCounter = 0;
                connection.Open();
                foreach (IEnumerable<FBSPerson> fbsPersonsByName in fbsPersons.Values)
                {
                    foreach (FBSPerson fbsPerson in fbsPersonsByName)
                    {
                        if ((fbsPerson.Action != FBSToGVUZActions.Update) && (fbsPerson.Action != FBSToGVUZActions.Insert))
                            continue;

                        PrepareForBulk(connection, fbsPerson);

                        counter++;
                        if (counter % MessageSize == 0)
                        {
                            Logger.WriteLine(String.Format("Обработано {0} данных о физических лицах", counter));
                        }
                        bulkCounter++;
                        ProcessDataBlock(connection, bulkCounter);
                    }
                }

                ProcessDataBlock(connection, null);
            }
            return !HasErrors;
        }

        private void ProcessDataBlock(SqlConnection connection, int? bulkCounter)
        {
            if (!bulkCounter.HasValue)
            {
                TryBulkWrite(connection);
            }
            else if (bulkCounter.Value % BulkSize == 0)
            {
                TryBulkWrite(connection);
            }
        }

        private void TryBulkWrite(SqlConnection connection)
        {
            bool success = false;
            int attempts = 0;
            while ((!success) && (attempts < DBOperationAttempts))
            {
                attempts++;
                try
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    BulkWrite(connection);
                    success = true;
                }
                catch (Exception ex)
                {
                    Logger.WriteLine("ОШИБКА записи блока данных: " + ex.Message + " (" + ex.StackTrace + ")");
                    System.Threading.Thread.Sleep(2 * 60 * 1000);
                }
            }
            if (!success)
            {
                Logger.WriteLine("ОШИБКА записи блока данных: не удалось произвести запись блока данных с " + DBOperationAttempts + " попыток");
                HasErrors = true;
            }
        }

        private void BulkWrite(SqlConnection connection)
        {
            if (_personsToInsert.Count > 0)
            {
                DataTable personsToInsertTable = CreatePersonsTable("PersonsToInsert");
                int maxId = GetMaxId(connection, Queries.Read_GVUZ.MaxPersonId) + 1;
                foreach (FBSPerson person in _personsToInsert)
                {
                    person.SetPersonId(maxId);
                    maxId++;
                    AddRow(personsToInsertTable, person);
                }
                try
                {
                    BulkWrite(connection, Queries.TableNames_GVUZ.Persons, personsToInsertTable);
                }
                catch
                {
                    foreach (FBSPerson person in _personsToInsert)
                    {
                        person.SetPersonId(null);
                    }
                    throw;
                }
                foreach (FBSPerson person in _personsToInsert)
                {
                    person.Action = FBSToGVUZActions.Link;
                }
                _personsToInsert.Clear();
            }
            if (_documentsToInsert.Count > 0)
            {
                DataTable documentsToInsertTable = CreateDocumentsTable("DocumentsToInsert");
                int maxId = GetMaxId(connection, Queries.Read_GVUZ.MaxDocumentId) + 1;
                foreach (FBSIdentityDocument document in _documentsToInsert)
                {
                    int documentId = maxId;
                    maxId++;
                    AddRow(documentsToInsertTable, document, documentId);
                }
                BulkWrite(connection, Queries.TableNames_GVUZ.Documents, documentsToInsertTable);

                _documentsToInsert.Clear();
            }
            if (_personsToUpdate.Count > 0)
            {
                DataTable personsToUpdateTable = CreatePersonsTable("PersonsToUpdate");
                foreach (FBSPerson person in _personsToUpdate)
                {
                    AddRow(personsToUpdateTable, person);
                }

                CreateTempTable(connection, Queries.TempTables_GVUZ.CreatePersonsTemp);
                BulkWrite(connection, Queries.TableNames_GVUZ.PersonsTemp, personsToUpdateTable);

                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandTimeout = CommandTimeout;
                Queries.TempTables_GVUZ.MovePersonsTempQuery(cmd);
                cmd.ExecuteNonQuery();

                DropTempTable(connection, Queries.TableNames_GVUZ.PersonsTemp);
                foreach (FBSPerson person in _personsToUpdate)
                {
                    person.Action = FBSToGVUZActions.Link;
                }
                _personsToUpdate.Clear();
            }
        }

        private int GetMaxId(SqlConnection connection, string sql)
        {
            SqlCommand createTempTableCmd = connection.CreateCommand();
            createTempTableCmd.CommandText = sql;
            return (int)createTempTableCmd.ExecuteScalar();
        }

        private void CreateTempTable(SqlConnection connection, string tempTableName)
        {
            SqlCommand createTempTableCmd = connection.CreateCommand();
            createTempTableCmd.CommandText = tempTableName;
            createTempTableCmd.ExecuteNonQuery();
        }

        private void DropTempTable(SqlConnection connection, string tempTableName)
        {
            SqlCommand dropTempTableCmd = connection.CreateCommand();
            dropTempTableCmd.CommandText = "DROP TABLE " + tempTableName;
            dropTempTableCmd.ExecuteNonQuery();
        }

        private void BulkWrite(SqlConnection connection, string destinationTableName, DataTable table)
        {
            using (SqlBulkCopy bulk = new SqlBulkCopy(connection))
            {
                bulk.BulkCopyTimeout = CommandTimeout;
                bulk.DestinationTableName = destinationTableName;
                bulk.WriteToServer(table);
            }
            table.Rows.Clear();
        }

        private void PrepareForBulk(SqlConnection connection, FBSPerson person)
        {
            if (person.Action == FBSToGVUZActions.Update)
            {
                _personsToUpdate.Add(person);
            }
            if (person.Action == FBSToGVUZActions.Insert)
            {
                _personsToInsert.Add(person);
                _documentsToInsert.Add(person.Document);
            }
        }

        private void AddRow(DataTable table, FBSPerson person)
        {
            DataRow row = table.NewRow();

            row["PersonId"] = person.PersonId.Value;
            row["IsRecordDeleted"] = false;
            row["NormSurname"] = person.NormSurnameStr;
            row["NormName"] = DataHelper.ReplaceNullToDBNull(person.NormNameStr);
            row["NormSecondName"] = DataHelper.ReplaceNullToDBNull(person.NormSecondNameStr);
            row["BirthDay"] = DataHelper.ReplaceNullToDBNull(person.BirthDay);
            row["Sex"] = DataHelper.ReplaceNullToDBNull(person.Sex);
            row["CreateDate"] = DateTime.Now;
            row["UpdateDate"] = DateTime.Now;
            row["IntegralUpdateDate"] = DateTime.Now;

            table.Rows.Add(row);
        }

        private void AddRow(DataTable table, FBSIdentityDocument document, int documentId)
        {
            DataRow row = table.NewRow();

            row["PersonIdentDocID"] = documentId;
            row["PersonId"] = document.Person.PersonId.Value;
            row["DocumentTypeCode"] = document.RVIDocumentTypeCode;
            row["DocumentSeries"] = DataHelper.ReplaceNullToDBNull(document.DocumentSeriesStr);
            row["DocumentNumber"] = document.DocumentNumberStr;
            row["CreateDate"] = DateTime.Now;
            row["UpdateDate"] = DateTime.Now;

            table.Rows.Add(row);
        }

        private DataTable CreatePersonsTable(string tableName)
        {
            DataTable result = new DataTable(tableName);
            result.Columns.Add("PersonId", typeof(int));
            result.Columns.Add("IsRecordDeleted", typeof(bool));
            result.Columns.Add("NormSurname", typeof(string));
            result.Columns.Add("NormName", typeof(string));
            result.Columns.Add("NormSecondName", typeof(string));
            result.Columns.Add("BirthDay", typeof(DateTime));
            result.Columns.Add("Sex", typeof(bool));
            result.Columns.Add("Email", typeof(string));
            result.Columns.Add("MobilePhone", typeof(string));
            result.Columns.Add("SNILS", typeof(string));
            result.Columns.Add("INN", typeof(string));
            result.Columns.Add("CreateDate", typeof(DateTime));
            result.Columns.Add("UpdateDate", typeof(DateTime));
            result.Columns.Add("IntegralUpdateDate", typeof(DateTime));
            return result;
        }

        private DataTable CreateDocumentsTable(string tableName)
        {
            DataTable result = new DataTable(tableName);
            result.Columns.Add("PersonIdentDocID", typeof(int));
            result.Columns.Add("PersonId", typeof(int));
            result.Columns.Add("DocumentTypeCode", typeof(int));
            result.Columns.Add("DocumentSeries", typeof(string));
            result.Columns.Add("DocumentNumber", typeof(string));
            result.Columns.Add("DocumentDate", typeof(DateTime));
            result.Columns.Add("DocumentOrganization", typeof(string));
            result.Columns.Add("DocumentName", typeof(string));
            result.Columns.Add("CreateDate", typeof(DateTime));
            result.Columns.Add("UpdateDate", typeof(DateTime));
            return result;
        }
    }
}
