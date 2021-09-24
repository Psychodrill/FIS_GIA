using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using FBS.Replicator.Entities;
using FBS.Replicator.Entities.ERBD;
using FBS.Replicator.Entities.FBS;
using FBS.Replicator.Replication.ERBDToFBS;
using FBS.Replicator.Replication.FBSToGVUZ;
using FBS.Common;

namespace FBS.Replicator.DB.FBS
{
    public class FBSWriteDB_Bulk
    {
        private int bulk_size = 10000;
        private const int MessageSize = 10000;
        private int cmd_timeout = 600;
        private int attempts = 5;

        public int BulkSize
        {
            get { return bulk_size; }
            set { bulk_size = value; }
        }
        public int CommandTimeout
        {
            get { return cmd_timeout; }
            set { cmd_timeout = value; }
        }
        public int DBOperationAttempts
        {
            get { return attempts; }
            set { attempts = value; }
        }
        private Tables _tables;
        public FBSWriteDB_Bulk(Tables tables)
        {
            _tables = tables;

            _participantIdsToHide = new List<ParticipantId>();

            _participantsToInsertHidden = new List<ERBDParticipant>();
            _participantsToUpdate = new List<ERBDParticipant>();
            _participantsToDelete = new List<FBSParticipant>();

            _certificatesToInsert = new List<ERBDCertificate>();
            _certificatesToUpdate = new List<ERBDCertificate>();
            _certificatesToDelete = new List<FBSCertificate>();

            _certificateMarksToInsert = new List<ERBDCertificateMark>();
            _certificateMarksToUpdate = new List<ERBDCertificateMark>();
            _certificateMarksToDelete = new List<FBSCertificateMark>();

            _cancelledCertificatesToInsert = new List<ERBDCancelledCertificate>();
            _cancelledCertificatesToUpdate = new List<ERBDCancelledCertificate>();
            _cancelledCertificatesToDelete = new List<FBSCancelledCertificate>();

            _personsToLink = new List<FBSPerson>();
        }

        public bool HasErrors { get; private set; }

        private List<ParticipantId> _participantIdsToHide;

        private List<ERBDParticipant> _participantsToInsertHidden;
        private List<ERBDParticipant> _participantsToUpdate;
        private List<FBSParticipant> _participantsToDelete;

        private List<ERBDCertificate> _certificatesToInsert;
        private List<ERBDCertificate> _certificatesToUpdate;
        private List<FBSCertificate> _certificatesToDelete;

        private List<ERBDCertificateMark> _certificateMarksToInsert;
        private List<ERBDCertificateMark> _certificateMarksToUpdate;
        private List<FBSCertificateMark> _certificateMarksToDelete;

        private List<ERBDCancelledCertificate> _cancelledCertificatesToInsert;
        private List<ERBDCancelledCertificate> _cancelledCertificatesToUpdate;
        private List<FBSCancelledCertificate> _cancelledCertificatesToDelete;

        private List<FBSPerson> _personsToLink;

        public bool WriteChanges(Dictionary<ParticipantId, FBSParticipant> fbsParticipants, Dictionary<CertificateId, FBSCertificate> fbsCertificatesWithoutParticipant, Dictionary<CertificateMarkId, FBSCertificateMark> certificateMarksWithoutParticipant, Dictionary<ParticipantId, ERBDParticipant> erbdParticipants, Dictionary<CertificateId, ERBDCertificate> erbdCertificatesWithoutParticipant)
        {
            using (SqlConnection connection = Connections.CreateFBSConnection())
            {
                int counter = 0;
                int bulkCounter = 0;
                connection.Open();
                foreach (FBSParticipant fbsParticipant in fbsParticipants.Values)
                {
                    if ((fbsParticipant.Action != ERBDToFBSActions.Delete) && (fbsParticipant.Action != ERBDToFBSActions.UpdateRelated))
                        continue;
                    if ((fbsParticipant.Action == ERBDToFBSActions.UpdateRelated) && (!erbdParticipants.ContainsKey(fbsParticipant.Id)))
                    {
                        Logger.WriteLine("ОШИБКА обработки записи (участник ЕГЭ ФБС: ParticipantID = " + fbsParticipant.Id.ParticipantID.ToString() + ", UseYear = " + fbsParticipant.Id.UseYear.ToString() + ", REGION=" + fbsParticipant.Id.REGION.ToString() + "; действие: " + fbsParticipant.Action.ToString() + "): Конфликт операций");
                        continue;
                    }

                    PrepareForBulkHide(connection, fbsParticipant);

                    foreach (FBSCertificateMark fbsCertificateMark in fbsParticipant.CertificateMarks)
                    {
                        PrepareForBulk(connection, fbsCertificateMark);
                    }

                    foreach (FBSCertificate fbsCertificate in fbsParticipant.Certificates)
                    {
                        foreach (FBSCertificateMark fbsCertificateMark in fbsCertificate.CertificateMarks)
                        {
                            PrepareForBulk(connection, fbsCertificateMark);
                        }
                        if (fbsCertificate.CancelledCertificate != null)
                        {
                            PrepareForBulk(connection, fbsCertificate.CancelledCertificate);
                        }

                        PrepareForBulk(connection, fbsCertificate);
                    }

                    ERBDParticipant erbdParticipant = null;
                    if (erbdParticipants.ContainsKey(fbsParticipant.Id))
                    {
                        erbdParticipant = erbdParticipants[fbsParticipant.Id];
                        foreach (ERBDCertificate erbdCertificate in erbdParticipant.Certificates)
                        {
                            PrepareForBulk(connection, erbdCertificate);

                            foreach (ERBDCertificateMark erbdCertificateMark in erbdCertificate.CertificateMarks)
                            {
                                PrepareForBulk(connection, erbdCertificateMark);
                            }
                            if (erbdCertificate.CancelledCertificate != null)
                            {
                                PrepareForBulk(connection, erbdCertificate.CancelledCertificate);
                            }
                        }

                        foreach (ERBDCertificateMark erbdCertificateMark in erbdParticipant.CertificateMarks)
                        {
                            PrepareForBulk(connection, erbdCertificateMark);
                        }
                    }

                    if (fbsParticipant.Action == ERBDToFBSActions.Delete)
                    {
                        PrepareForBulk(connection, fbsParticipant);
                    }
                    else if (fbsParticipant.Action == ERBDToFBSActions.UpdateRelated)
                    {
                        PrepareForBulk(connection, erbdParticipant);
                    }

                    counter++;
                    if (counter % MessageSize == 0)
                    {
                        Logger.WriteLine(String.Format("Обработано {0} участников ЕГЭ", counter));
                    }
                    bulkCounter++;
                    ProcessDataBlock(connection, bulkCounter);
                }

                ProcessDataBlock(connection, null);

                bulkCounter = 0;
                foreach (ERBDParticipant erbdParticipant in erbdParticipants.Values)
                {
                    if ((erbdParticipant.Action != ERBDToFBSActions.Insert) && (erbdParticipant.Action != ERBDToFBSActions.Update) && (erbdParticipant.Action != ERBDToFBSActions.UpdateRelated))
                        continue;

                    PrepareForBulkHide(connection, erbdParticipant);

                    foreach (ERBDCertificate erbdCertificate in erbdParticipant.Certificates)
                    {
                        PrepareForBulk(connection, erbdCertificate);

                        foreach (ERBDCertificateMark erbdCertificateMark in erbdCertificate.CertificateMarks)
                        {
                            PrepareForBulk(connection, erbdCertificateMark);
                        }
                        if (erbdCertificate.CancelledCertificate != null)
                        {
                            PrepareForBulk(connection, erbdCertificate.CancelledCertificate);
                        }
                    }

                    foreach (ERBDCertificateMark erbdCertificateMark in erbdParticipant.CertificateMarks)
                    {
                        PrepareForBulk(connection, erbdCertificateMark);
                    }

                    PrepareForBulk(connection, erbdParticipant);

                    counter++;
                    if (counter % MessageSize == 0)
                    {
                        Logger.WriteLine(String.Format("Обработано {0} участников ЕГЭ", counter));
                    }

                    bulkCounter++;
                    ProcessDataBlock(connection, bulkCounter);
                }
                ProcessDataBlock(connection, null);


                bulkCounter = 0;
                foreach (FBSCertificate fbsCertificate in fbsCertificatesWithoutParticipant.Values)
                {
                    PrepareForBulk(connection, fbsCertificate);
                    bulkCounter++;
                    ProcessDataBlock(connection, bulkCounter);
                }
                ProcessDataBlock(connection, null);

                bulkCounter = 0;
                foreach (ERBDCertificate erbdCertificate in erbdCertificatesWithoutParticipant.Values)
                {
                    PrepareForBulk(connection, erbdCertificate);
                    bulkCounter++;
                    ProcessDataBlock(connection, bulkCounter);
                }
                ProcessDataBlock(connection, null);

                bulkCounter = 0;
                foreach (FBSCertificateMark fbsCertificateMark in certificateMarksWithoutParticipant.Values)
                {
                    PrepareForBulk(connection, fbsCertificateMark);
                    bulkCounter++;
                    ProcessDataBlock(connection, bulkCounter);
                }
                ProcessDataBlock(connection, null);
            }
            return !HasErrors;
        }

        public bool WriteChanges(Dictionary<int, IEnumerable<FBSPerson>> fbsPersons)
        {
            using (SqlConnection connection = Connections.CreateFBSConnection())
            {
                int counter = 0;
                int bulkCounter = 0;
                connection.Open();
                foreach (IEnumerable<FBSPerson> fbsPersonsByName in fbsPersons.Values)
                {
                    foreach (FBSPerson fbsPerson in fbsPersonsByName)
                    {
                        if (fbsPerson.Action != FBSToGVUZActions.Link)
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
            if (_participantsToInsertHidden.Count > 0)
            {
                DataTable participantsToInsertHiddenTable = CreateParticipantsTable("ParticipantsToInsertHidden");
                foreach (ERBDParticipant participant in _participantsToInsertHidden)
                {
                    AddHiddenRow(participantsToInsertHiddenTable, participant);
                }
                BulkWrite(connection, Queries.TableNames_FBS.Participants(_tables), participantsToInsertHiddenTable);

                _participantsToInsertHidden.Clear();
            }
            if (_participantIdsToHide.Count > 0)
            {
                DataTable participantsToHideTable = CreateParticipantsToHideTable("ParticipantsToHide");
                foreach (ParticipantId participantId in _participantIdsToHide)
                {
                    AddHiddenRow(participantsToHideTable, participantId);
                }

                CreateTempTable(connection, Queries.TempTables_FBS.CreateParticipantsHidden);
                BulkWrite(connection, Queries.TableNames_FBS.ParticipantsHidden, participantsToHideTable);

                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandTimeout = CommandTimeout;
                Queries.TempTables_FBS.MoveParticipantsHiddenQuery(_tables, cmd);
                cmd.ExecuteNonQuery();

                DropTempTable(connection, Queries.TableNames_FBS.ParticipantsHidden);

                _participantIdsToHide.Clear();
            }

            if (_certificatesToInsert.Count > 0)
            {
                DataTable certificatesToInsertTable = CreateCertificatesTable("CertificatesToInsert");
                foreach (ERBDCertificate certificate in _certificatesToInsert)
                {
                    AddRow(certificatesToInsertTable, certificate);
                }
                BulkWrite(connection, Queries.TableNames_FBS.Certificates(_tables), certificatesToInsertTable);
                foreach (ERBDCertificate certificate in _certificatesToInsert)
                {
                    certificate.Action = ERBDToFBSActions.Done;
                }
                _certificatesToInsert.Clear();

            }
            if (_certificatesToUpdate.Count > 0)
            {
                DataTable certificatesToUpdateTable = CreateCertificatesTable("CertificatesToUpdate");
                foreach (ERBDCertificate certificate in _certificatesToUpdate)
                {
                    AddRow(certificatesToUpdateTable, certificate);
                }

                CreateTempTable(connection, Queries.TempTables_FBS.CreateCertificatesTemp);
                BulkWrite(connection, Queries.TableNames_FBS.CertificatesTemp, certificatesToUpdateTable);

                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandTimeout = CommandTimeout;
                Queries.TempTables_FBS.MoveCertificatesTempQuery(_tables, cmd);
                cmd.ExecuteNonQuery();

                DropTempTable(connection, Queries.TableNames_FBS.CertificatesTemp);
                foreach (ERBDCertificate certificate in _certificatesToUpdate)
                {
                    certificate.Action = ERBDToFBSActions.Done;
                }
                _certificatesToUpdate.Clear();
            }
            if (_certificateMarksToInsert.Count > 0)
            {
                DataTable certificateMarksToInsertTable = CreateCertificateMarksTable("CertificateMarksToInsert");
                foreach (ERBDCertificateMark certificateMark in _certificateMarksToInsert)
                {
                    AddRow(certificateMarksToInsertTable, certificateMark);
                }
                BulkWrite(connection, Queries.TableNames_FBS.CertificateMarks(_tables), certificateMarksToInsertTable);
                foreach (ERBDCertificateMark certificateMark in _certificateMarksToInsert)
                {
                    certificateMark.Action = ERBDToFBSActions.Done;
                }
                _certificateMarksToInsert.Clear();
            }
            if (_certificateMarksToUpdate.Count > 0)
            {
                DataTable certificateMarksToUpdateTable = CreateCertificateMarksTable("CertificateMarksToUpdate");
                foreach (ERBDCertificateMark certificateMark in _certificateMarksToUpdate)
                {
                    AddRow(certificateMarksToUpdateTable, certificateMark);
                }
                CreateTempTable(connection, Queries.TempTables_FBS.CreateCertificateMarksTemp);
                BulkWrite(connection, Queries.TableNames_FBS.CertificateMarksTemp, certificateMarksToUpdateTable);

                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandTimeout = CommandTimeout;
                Queries.TempTables_FBS.MoveCertificateMarksTempQuery(_tables, cmd);
                cmd.ExecuteNonQuery();

                DropTempTable(connection, Queries.TableNames_FBS.CertificateMarksTemp);

                foreach (ERBDCertificateMark certificateMark in _certificateMarksToUpdate)
                {
                    certificateMark.Action = ERBDToFBSActions.Done;
                }
                _certificateMarksToUpdate.Clear();
            }
            if (_cancelledCertificatesToInsert.Count > 0)
            {
                DataTable cancelledCertificatesToInsertTable = CreateCancelledCertificatesTable("CancelledCertificatesToInsert");
                foreach (ERBDCancelledCertificate cancelledCertificate in _cancelledCertificatesToInsert)
                {
                    AddRow(cancelledCertificatesToInsertTable, cancelledCertificate);
                }
                BulkWrite(connection, Queries.TableNames_FBS.CancelledCertificates(_tables), cancelledCertificatesToInsertTable);
                foreach (ERBDCancelledCertificate cancelledCertificate in _cancelledCertificatesToInsert)
                {
                    cancelledCertificate.Action = ERBDToFBSActions.Done;
                }
                _cancelledCertificatesToInsert.Clear();
            }
            if (_cancelledCertificatesToUpdate.Count > 0)
            {
                DataTable cancelledCertificatesToUpdateTable = CreateCancelledCertificatesTable("CancelledCertificatesToUpdate");
                foreach (ERBDCancelledCertificate cancelledCertificate in _cancelledCertificatesToUpdate)
                {
                    AddRow(cancelledCertificatesToUpdateTable, cancelledCertificate);
                }

                CreateTempTable(connection, Queries.TempTables_FBS.CreateCancelledCertificatesTemp);
                BulkWrite(connection, Queries.TableNames_FBS.CancelledCertificatesTemp, cancelledCertificatesToUpdateTable);

                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandTimeout = CommandTimeout;
                Queries.TempTables_FBS.MoveCancelledCertificatesTempQuery(_tables, cmd);
                cmd.ExecuteNonQuery();

                DropTempTable(connection, Queries.TableNames_FBS.CancelledCertificatesTemp);

                foreach (ERBDCancelledCertificate cancelledCertificate in _cancelledCertificatesToUpdate)
                {
                    cancelledCertificate.Action = ERBDToFBSActions.Done;
                }
                _cancelledCertificatesToUpdate.Clear();
            }

            if (_cancelledCertificatesToDelete.Count > 0)
            {
                DataTable cancelledCertificatesToDeleteTable = CreateCancelledCertificateIdsTable("CancelledCertificatesToDelete");
                foreach (FBSCancelledCertificate cancelledCertificate in _cancelledCertificatesToDelete)
                {
                    AddIdRow(cancelledCertificatesToDeleteTable, cancelledCertificate.Id);
                }

                CreateTempTable(connection, Queries.TempTables_FBS.CreateCancelledCertificateIdsTemp);
                BulkWrite(connection, Queries.TableNames_FBS.CancelledCertificateIdsTemp, cancelledCertificatesToDeleteTable);

                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandTimeout = CommandTimeout;
                Queries.TempTables_FBS.DeleteByCancelledCertificateIdsQuery(_tables, cmd);
                cmd.ExecuteNonQuery();

                DropTempTable(connection, Queries.TableNames_FBS.CancelledCertificateIdsTemp);

                foreach (FBSCancelledCertificate cancelledCertificate in _cancelledCertificatesToDelete)
                {
                    cancelledCertificate.Action = ERBDToFBSActions.Done;
                }
                _cancelledCertificatesToDelete.Clear();
            }
            if (_certificateMarksToDelete.Count > 0)
            {
                DataTable certificateMarksToDeleteTable = CreateCertificateMarkIdsTable("CertificateMarksToDelete");
                foreach (FBSCertificateMark certificateMark in _certificateMarksToDelete)
                {
                    AddIdRow(certificateMarksToDeleteTable, certificateMark.Id);
                }

                CreateTempTable(connection, Queries.TempTables_FBS.CreateCertificateMarkIdsTemp);
                BulkWrite(connection, Queries.TableNames_FBS.CertificateMarkIdsTemp, certificateMarksToDeleteTable);

                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandTimeout = CommandTimeout;
                Queries.TempTables_FBS.DeleteByCertificateMarkIdsQuery(_tables, cmd);
                cmd.ExecuteNonQuery();

                DropTempTable(connection, Queries.TableNames_FBS.CertificateMarkIdsTemp);

                foreach (FBSCertificateMark certificateMark in _certificateMarksToDelete)
                {
                    certificateMark.Action = ERBDToFBSActions.Done;
                }
                _certificateMarksToDelete.Clear();
            }
            if (_certificatesToDelete.Count > 0)
            {
                DataTable certificatesToDeleteTable = CreateCertificateIdsTable("CertificatesToDelete");
                foreach (FBSCertificate certificate in _certificatesToDelete)
                {
                    AddIdRow(certificatesToDeleteTable, certificate.Id);
                }

                CreateTempTable(connection, Queries.TempTables_FBS.CreateCertificateIdsTemp);
                BulkWrite(connection, Queries.TableNames_FBS.CertificateIdsTemp, certificatesToDeleteTable);

                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandTimeout = CommandTimeout;
                Queries.TempTables_FBS.DeleteByCertificateIdsQuery(_tables, cmd);
                cmd.ExecuteNonQuery();

                DropTempTable(connection, Queries.TableNames_FBS.CertificateIdsTemp);

                foreach (FBSCertificate certificate in _certificatesToDelete)
                {
                    certificate.Action = ERBDToFBSActions.Done;
                }
                _certificatesToDelete.Clear();
            }
            if (_participantsToDelete.Count > 0)
            {
                DataTable participantsToDeleteTable = CreateParticipantIdsTable("ParticipantsToDelete");
                foreach (FBSParticipant participant in _participantsToDelete)
                {
                    AddIdRow(participantsToDeleteTable, participant.Id);
                }
                CreateTempTable(connection, Queries.TempTables_FBS.CreateParticipantIdsTemp);
                BulkWrite(connection, Queries.TableNames_FBS.ParticipantIdsTemp, participantsToDeleteTable);

                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandTimeout = CommandTimeout;
                Queries.TempTables_FBS.DeleteByParticipantIdsQuery(_tables, cmd);
                cmd.ExecuteNonQuery();

                DropTempTable(connection, Queries.TableNames_FBS.ParticipantIdsTemp);

                foreach (FBSParticipant participant in _participantsToDelete)
                {
                    participant.Action = ERBDToFBSActions.Done;
                }
                _participantsToDelete.Clear();
            }

            if (_participantsToUpdate.Count > 0)
            {
                DataTable participantsToUpdateTable = CreateParticipantsTable("ParticipantsToUpdate");
                foreach (ERBDParticipant participant in _participantsToUpdate)
                {
                    AddRow(participantsToUpdateTable, participant);
                }

                CreateTempTable(connection, Queries.TempTables_FBS.CreateParticipantsTemp);
                BulkWrite(connection, Queries.TableNames_FBS.ParticipantsTemp, participantsToUpdateTable);

                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandTimeout = CommandTimeout;
                Queries.TempTables_FBS.MoveParticipantsTempQuery(_tables, cmd);
                cmd.ExecuteNonQuery();

                DropTempTable(connection, Queries.TableNames_FBS.ParticipantsTemp);

                foreach (ERBDParticipant participant in _participantsToUpdate)
                {
                    participant.Action = ERBDToFBSActions.Done;
                }
                _participantsToUpdate.Clear();
            }

            if (_personsToLink.Count > 0)
            {
                DataTable personsToLinkTable = CreatePersonsTable("PersonsToLink");
                foreach (FBSPerson person in _personsToLink)
                {
                    AddRow(personsToLinkTable, person);
                }

                CreateTempTable(connection, Queries.TempTables_FBS.CreatePersonsTemp);
                BulkWrite(connection, Queries.TableNames_FBS.PersonsTemp, personsToLinkTable);

                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandTimeout = CommandTimeout;
                Queries.TempTables_FBS.LinkPersonsTempQuery(_tables, cmd);
                cmd.ExecuteNonQuery();

                DropTempTable(connection, Queries.TableNames_FBS.PersonsTemp);

                foreach (FBSPerson person in _personsToLink)
                {
                    person.Action = FBSToGVUZActions.Done;
                }
                _personsToLink.Clear();
            }
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

        private void PrepareForBulkHide(SqlConnection connection, FBSParticipant participant)
        {
            if ((participant.Action == ERBDToFBSActions.Delete) || (participant.Action == ERBDToFBSActions.UpdateRelated))
            {
                _participantIdsToHide.Add(participant.Id);
            }
        }

        private void PrepareForBulkHide(SqlConnection connection, ERBDParticipant participant)
        {
            if ((participant.Action == ERBDToFBSActions.UpdateRelated) || (participant.Action == ERBDToFBSActions.Update))
            {
                _participantIdsToHide.Add(participant.Id);
            }
            if (participant.Action == ERBDToFBSActions.Insert)
            {
                _participantsToInsertHidden.Add(participant);
            }
        }

        private void PrepareForBulk(SqlConnection connection, FBSParticipant participant)
        {
            if (participant.Action == ERBDToFBSActions.Delete)
            {
                _participantsToDelete.Add(participant);
            }
        }

        private void PrepareForBulk(SqlConnection connection, ERBDParticipant participant)
        {
            _participantsToUpdate.Add(participant);
        }

        private void PrepareForBulk(SqlConnection connection, FBSCertificate certificate)
        {
            if (certificate.Action == ERBDToFBSActions.Delete)
            {
                _certificatesToDelete.Add(certificate);
            }
        }

        private void PrepareForBulk(SqlConnection connection, ERBDCertificate certificate)
        {
            if (certificate.Action == ERBDToFBSActions.Update)
            {
                _certificatesToUpdate.Add(certificate);
            }
            if (certificate.Action == ERBDToFBSActions.Insert)
            {
                _certificatesToInsert.Add(certificate);
            }
        }

        private void PrepareForBulk(SqlConnection connection, FBSCertificateMark certificateMark)
        {
            if (certificateMark.Action == ERBDToFBSActions.Delete)
            {
                _certificateMarksToDelete.Add(certificateMark);
            }
        }

        private void PrepareForBulk(SqlConnection connection, ERBDCertificateMark certificateMark)
        {
            if (certificateMark.Action == ERBDToFBSActions.Update)
            {
                _certificateMarksToUpdate.Add(certificateMark);
            }
            if (certificateMark.Action == ERBDToFBSActions.Insert)
            {
                _certificateMarksToInsert.Add(certificateMark);
            }
        }

        private void PrepareForBulk(SqlConnection connection, FBSCancelledCertificate cancelledCertificate)
        {
            if (cancelledCertificate.Action == ERBDToFBSActions.Delete)
            {
                _cancelledCertificatesToDelete.Add(cancelledCertificate);
            }
        }

        private void PrepareForBulk(SqlConnection connection, ERBDCancelledCertificate cancelledCertificate)
        {
            if (cancelledCertificate.Action == ERBDToFBSActions.Update)
            {
                _cancelledCertificatesToUpdate.Add(cancelledCertificate);
            }
            if (cancelledCertificate.Action == ERBDToFBSActions.Insert)
            {
                _cancelledCertificatesToInsert.Add(cancelledCertificate);
            }
        }

        private void PrepareForBulk(SqlConnection connection, FBSPerson person)
        {
            if (person.Action == FBSToGVUZActions.Link)
            {
                _personsToLink.Add(person);
            }
        }

        private void AddHiddenRow(DataTable table, ParticipantId participantId)
        {
            DataRow row = table.NewRow();

            row["ParticipantID"] = participantId.ParticipantID;
            row["UseYear"] = participantId.UseYear;
            row["REGION"] = participantId.REGION;

            row["Surname"] = Guid.Empty.ToString();
            row["Name"] = Guid.Empty.ToString();
            row["SecondName"] = Guid.Empty.ToString();
            row["SurnameTrimmed"] = Guid.Empty.ToString();
            row["NameTrimmed"] = Guid.Empty.ToString();
            row["SecondNameTrimmed"] = Guid.Empty.ToString();

            table.Rows.Add(row);
        }

        private void AddHiddenRow(DataTable table, ERBDParticipant participant)
        {
            DataRow row = table.NewRow();

            row["ParticipantID"] = participant.Id.ParticipantID;
            row["UseYear"] = participant.Id.UseYear;
            row["REGION"] = participant.Id.REGION;
            row["ParticipantCode"] = DataHelper.ReplaceNullToDBNull(participant.ParticipantCodeStr);
            row["Surname"] = Guid.Empty.ToString();
            row["Name"] = Guid.Empty.ToString();
            row["SecondName"] = Guid.Empty.ToString();
            row["SurnameTrimmed"] = Guid.Empty.ToString();
            row["NameTrimmed"] = Guid.Empty.ToString();
            row["SecondNameTrimmed"] = Guid.Empty.ToString();
            row["DocumentSeries"] = DataHelper.ReplaceNullToDBNull(participant.DocumentSeriesStr);
            row["DocumentNumber"] = DataHelper.ReplaceNullToDBNull(participant.DocumentNumberStr);
            row["DocumentTypeCode"] = participant.DocumentTypeCode;
            row["Sex"] = participant.Sex;
            row["BirthDay"] = participant.BirthDay;
            row["FinishRegion"] = DataHelper.ReplaceNullToDBNull(participant.FinishRegion);
            row["ParticipantCategoryFK"] = participant.ParticipantCategoryFK;
            row["CreateDate"] = DateTime.Now;
            row["UpdateDate"] = DateTime.Now;
            row["ImportCreateDate"] = DateTime.Now;
            row["ImportUpdateDate"] = DateTime.Now;
            row["TestTypeID"] = participant.TestTypeID;

            table.Rows.Add(row);
        }

        private void AddIdRow(DataTable table, ParticipantId id)
        {
            DataRow row = table.NewRow();

            row["ParticipantID"] = id.ParticipantID;
            row["UseYear"] = id.UseYear;
            row["REGION"] = id.REGION;

            table.Rows.Add(row);
        }

        private void AddRow(DataTable table, ERBDParticipant participant)
        {
            DataRow row = table.NewRow();

            row["ParticipantID"] = participant.Id.ParticipantID;
            row["UseYear"] = participant.Id.UseYear;
            row["REGION"] = participant.Id.REGION;
            row["ParticipantCode"] = DataHelper.ReplaceNullToDBNull(participant.ParticipantCodeStr);
            row["Surname"] = DataHelper.ReplaceNullToDBNull(participant.SurnameStr);
            row["Name"] = DataHelper.ReplaceNullToDBNull(participant.NameStr);
            row["SecondName"] = DataHelper.ReplaceNullToDBNull(participant.SecondNameStr);
            row["SurnameTrimmed"] = DataHelper.ReplaceNullToDBNull(participant.SurnameTrimmedStr);
            row["NameTrimmed"] = DataHelper.ReplaceNullToDBNull(participant.NameTrimmedStr);
            row["SecondNameTrimmed"] = DataHelper.ReplaceNullToDBNull(participant.SecondNameTrimmedStr);
            row["DocumentSeries"] = DataHelper.ReplaceNullToDBNull(participant.DocumentSeriesStr);
            row["DocumentNumber"] = DataHelper.ReplaceNullToDBNull(participant.DocumentNumberStr);
            row["DocumentTypeCode"] = participant.DocumentTypeCode;
            row["Sex"] = participant.Sex;
            row["BirthDay"] = participant.BirthDay;
            row["FinishRegion"] = DataHelper.ReplaceNullToDBNull(participant.FinishRegion);
            row["ParticipantCategoryFK"] = participant.ParticipantCategoryFK;
            row["CreateDate"] = DateTime.Now;
            row["UpdateDate"] = DateTime.Now;
            row["ImportCreateDate"] = DateTime.Now;
            row["ImportUpdateDate"] = DateTime.Now;
            row["TestTypeID"] = participant.TestTypeID;

            table.Rows.Add(row);
        }

        private void AddRow(DataTable table, ERBDCertificate certificate)
        {
            DataRow row = table.NewRow();

            row["CertificateID"] = certificate.Id.CertificateID;
            row["UseYear"] = certificate.Id.UseYear;
            row["REGION"] = certificate.Id.REGION;
            row["Wave"] = certificate.Wave;
            row["LicenseNumber"] = DataHelper.ReplaceNullToDBNull(certificate.LicenseNumberStr);
            row["TypographicNumber"] = DataHelper.ReplaceNullToDBNull(certificate.TypographicNumberStr);
            row["ParticipantFK"] = DataHelper.ReplaceNullToDBNull(certificate.ParticipantFK);
            row["Cancelled"] = certificate.Cancelled;
            row["CreateDate"] = DateTime.Now;
            row["UpdateDate"] = DateTime.Now;
            row["ImportCreateDate"] = DateTime.Now;
            row["ImportUpdateDate"] = DateTime.Now;

            table.Rows.Add(row);
        }

        private void AddIdRow(DataTable table, CertificateMarkId id)
        {
            DataRow row = table.NewRow();

            row["CertificateMarkID"] = id.CertificateMarkID;
            row["UseYear"] = id.UseYear;
            row["REGION"] = id.REGION;
            row["CertificateFK"] = id.CertificateFK;

            table.Rows.Add(row);
        }

        private void AddRow(DataTable table, ERBDCertificateMark certificateMark)
        {
            DataRow row = table.NewRow();

            row["CertificateMarkID"] = certificateMark.Id.CertificateMarkID;
            row["UseYear"] = certificateMark.Id.UseYear;
            row["REGION"] = certificateMark.Id.REGION;
            row["CertificateFK"] = certificateMark.Id.CertificateFK;
            row["ParticipantFK"] = certificateMark.ParticipantFK;
            row["SubjectCode"] = certificateMark.SubjectCode;
            row["Mark"] = certificateMark.Mark;
            row["HasAppeal"] = certificateMark.HasAppeal;
            row["PrintedMarkID"] = DataHelper.ReplaceNullToDBNull(certificateMark.PrintedMarkID);
            row["TestTypeID"] = certificateMark.TestTypeID;
            row["ProcessCondition"] = certificateMark.ProcessCondition;
            row["VariantCode"] = DataHelper.ReplaceNullToDBNull(certificateMark.VariantCode);
            row["AppealStatusID"] = DataHelper.ReplaceNullToDBNull(certificateMark.AppealStatusID);
            row["ExamDate"] = DataHelper.ReplaceNullToDBNull(certificateMark.ExamDate);
            row["CompositionBarcode"] = DataHelper.ReplaceNullToDBNull(certificateMark.CompositionBarcodeStr);
            row["CompositionPagesCount"] = DataHelper.ReplaceNullToDBNull(certificateMark.CompositionPagesCount);
            row["CompositionStatus"] = DataHelper.ReplaceNullToDBNull(certificateMark.CompositionStatus);
            row["CompositionPaths"] = DataHelper.ReplaceNullToDBNull(certificateMark.CompositionPathsStr);

            table.Rows.Add(row);
        }

        private void AddIdRow(DataTable table, CertificateId id)
        {
            DataRow row = table.NewRow();

            row["UseYear"] = id.UseYear;
            row["REGION"] = id.REGION;
            row["CertificateID"] = id.CertificateID;

            table.Rows.Add(row);
        }

        private void AddRow(DataTable table, ERBDCancelledCertificate cancelledCertificate)
        {
            DataRow row = table.NewRow();

            row["UseYear"] = cancelledCertificate.Id.UseYear;
            row["REGION"] = cancelledCertificate.Id.REGION;
            row["CertificateFK"] = cancelledCertificate.Id.CertificateID;
            row["Reason"] = DataHelper.ReplaceNullToDBNull(cancelledCertificate.ReasonStr);

            table.Rows.Add(row);
        }

        private void AddRow(DataTable table, FBSPerson person)
        {
            DataRow row = table.NewRow();

            row["ParticipantID"] = person.Id.ParticipantID;
            row["UseYear"] = person.Id.UseYear;
            row["REGION"] = person.Id.REGION;
            row["PersonId"] = person.PersonId.Value;
            row["PersonLinkDate"] = DateTime.Now;

            table.Rows.Add(row);
        }

        private DataTable CreateParticipantIdsTable(string tableName)
        {
            DataTable result = new DataTable(tableName);
            result.Columns.Add("ParticipantID", typeof(Guid));
            result.Columns.Add("UseYear", typeof(int));
            result.Columns.Add("REGION", typeof(int));
            return result;
        }

        private DataTable CreateParticipantsToHideTable(string tableName)
        {
            DataTable result = new DataTable(tableName);
            result.Columns.Add("ParticipantID", typeof(Guid));
            result.Columns.Add("UseYear", typeof(int));
            result.Columns.Add("REGION", typeof(int));
            result.Columns.Add("Surname", typeof(string));
            result.Columns.Add("Name", typeof(string));
            result.Columns.Add("SecondName", typeof(string));
            result.Columns.Add("SurnameTrimmed", typeof(string));
            result.Columns.Add("NameTrimmed", typeof(string));
            result.Columns.Add("SecondNameTrimmed", typeof(string));
            return result;
        }
        private DataTable CreateParticipantsTable(string tableName)
        {
            DataTable result = new DataTable(tableName);
            result.Columns.Add("ParticipantID", typeof(Guid));
            result.Columns.Add("UseYear", typeof(int));
            result.Columns.Add("REGION", typeof(int));
            result.Columns.Add("ParticipantCode", typeof(string));
            result.Columns.Add("Surname", typeof(string));
            result.Columns.Add("Name", typeof(string));
            result.Columns.Add("SecondName", typeof(string));
            result.Columns.Add("DocumentSeries", typeof(string));
            result.Columns.Add("DocumentNumber", typeof(string));
            result.Columns.Add("DocumentTypeCode", typeof(int));
            result.Columns.Add("Sex", typeof(bool));
            result.Columns.Add("BirthDay", typeof(DateTime));
            result.Columns.Add("FinishRegion", typeof(int));
            result.Columns.Add("ParticipantCategoryFK", typeof(int));
            result.Columns.Add("CreateDate", typeof(DateTime));
            result.Columns.Add("UpdateDate", typeof(DateTime));
            result.Columns.Add("ImportCreateDate", typeof(DateTime));
            result.Columns.Add("ImportUpdateDate", typeof(DateTime));
            result.Columns.Add("TestTypeID", typeof(int));
            result.Columns.Add("SurnameTrimmed", typeof(string));
            result.Columns.Add("NameTrimmed", typeof(string));
            result.Columns.Add("SecondNameTrimmed", typeof(string));
            return result;
        }

        private DataTable CreateCertificateIdsTable(string tableName)
        {
            DataTable result = new DataTable(tableName);
            result.Columns.Add("CertificateID", typeof(Guid));
            result.Columns.Add("UseYear", typeof(int));
            result.Columns.Add("REGION", typeof(int));
            return result;
        }

        private DataTable CreateCertificatesTable(string tableName)
        {
            DataTable result = new DataTable(tableName);
            result.Columns.Add("CertificateID", typeof(Guid));
            result.Columns.Add("UseYear", typeof(int));
            result.Columns.Add("REGION", typeof(int));
            result.Columns.Add("Wave", typeof(int));
            result.Columns.Add("LicenseNumber", typeof(string));
            result.Columns.Add("TypographicNumber", typeof(string));
            result.Columns.Add("ParticipantFK", typeof(Guid));
            result.Columns.Add("Cancelled", typeof(bool));
            result.Columns.Add("CreateDate", typeof(DateTime));
            result.Columns.Add("UpdateDate", typeof(DateTime));
            result.Columns.Add("ImportCreateDate", typeof(DateTime));
            result.Columns.Add("ImportUpdateDate", typeof(DateTime));
            return result;
        }

        private DataTable CreateCertificateMarkIdsTable(string tableName)
        {
            DataTable result = new DataTable(tableName);
            result.Columns.Add("CertificateMarkID", typeof(Guid));
            result.Columns.Add("UseYear", typeof(int));
            result.Columns.Add("REGION", typeof(int));
            result.Columns.Add("CertificateFK", typeof(Guid));
            return result;
        }

        private DataTable CreateCertificateMarksTable(string tableName)
        {
            DataTable result = new DataTable(tableName);
            result.Columns.Add("CertificateMarkID", typeof(Guid));
            result.Columns.Add("UseYear", typeof(int));
            result.Columns.Add("REGION", typeof(int));
            result.Columns.Add("CertificateFK", typeof(Guid));
            result.Columns.Add("ParticipantFK", typeof(Guid));
            result.Columns.Add("SubjectCode", typeof(int));
            result.Columns.Add("Mark", typeof(int));
            result.Columns.Add("HasAppeal", typeof(bool));
            result.Columns.Add("PrintedMarkID", typeof(Guid));
            result.Columns.Add("TestTypeID", typeof(int));
            result.Columns.Add("ProcessCondition", typeof(int));
            result.Columns.Add("VariantCode", typeof(int));
            result.Columns.Add("AppealStatusID", typeof(int));

            result.Columns.Add("ExamDate", typeof(DateTime));
            result.Columns.Add("CompositionBarcode", typeof(string));
            result.Columns.Add("CompositionPagesCount", typeof(int));
            result.Columns.Add("CompositionStatus", typeof(int));
            result.Columns.Add("CompositionPaths", typeof(string));
             
            return result;
        }

        private DataTable CreateCancelledCertificateIdsTable(string tableName)
        {
            DataTable result = new DataTable(tableName);
            result.Columns.Add("UseYear", typeof(int));
            result.Columns.Add("REGION", typeof(int));
            result.Columns.Add("CertificateID", typeof(Guid));
            return result;
        }

        private DataTable CreateCancelledCertificatesTable(string tableName)
        {
            DataTable result = new DataTable(tableName);
            result.Columns.Add("UseYear", typeof(int));
            result.Columns.Add("REGION", typeof(int));
            result.Columns.Add("CertificateFK", typeof(Guid));
            result.Columns.Add("Reason", typeof(string));
            return result;
        }

        private DataTable CreatePersonsTable(string tableName)
        {
            DataTable result = new DataTable(tableName);
            result.Columns.Add("ParticipantID", typeof(Guid));
            result.Columns.Add("UseYear", typeof(int));
            result.Columns.Add("REGION", typeof(int));
            result.Columns.Add("PersonId", typeof(int));
            result.Columns.Add("PersonLinkDate", typeof(DateTime));
            return result;
        }
    }
}
