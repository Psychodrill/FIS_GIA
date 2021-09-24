using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using FBS.Replicator.Entities;
using FBS.Replicator.Entities.ERBD;
using FBS.Replicator.Entities.FBS;
using FBS.Replicator.Replication.ERBDToFBS;
using FBS.Replicator.Replication.FBSToGVUZ;
using FBS.Common;

namespace FBS.Replicator.DB.FBS
{
    public class FBSWriteDB
    {
        private const int MessageSize = 10000;
        private const int CommandTimeout = 300;

        private Tables _tables;
        public FBSWriteDB(Tables tables)
        {
            _tables = tables;
        }

        public void WriteChanges(Dictionary<ParticipantId, FBSParticipant> fbsParticipants, Dictionary<CertificateId, FBSCertificate> fbsCertificatesWithoutParticipant, Dictionary<CertificateMarkId, FBSCertificateMark> certificateMarksWithoutParticipant, Dictionary<ParticipantId, ERBDParticipant> erbdParticipants, Dictionary<CertificateId, ERBDCertificate> erbdCertificatesWithoutParticipant)
        {
            using (SqlConnection connection = Connections.CreateFBSConnection())
            {
                int counter = 0;
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

                    try
                    {
                        ExecuteHideCommand(connection, fbsParticipant);

                        foreach (FBSCertificateMark fbsCertificateMark in fbsParticipant.CertificateMarks)
                        {
                            ExecuteCommand(connection, fbsCertificateMark);
                        }

                        foreach (FBSCertificate fbsCertificate in fbsParticipant.Certificates)
                        {
                            foreach (FBSCertificateMark fbsCertificateMark in fbsCertificate.CertificateMarks)
                            {
                                ExecuteCommand(connection, fbsCertificateMark);
                            }
                            if (fbsCertificate.CancelledCertificate != null)
                            {
                                ExecuteCommand(connection, fbsCertificate.CancelledCertificate);
                            }

                            ExecuteCommand(connection, fbsCertificate);
                        }

                        ERBDParticipant erbdParticipant = null;
                        if (erbdParticipants.ContainsKey(fbsParticipant.Id))
                        {
                            erbdParticipant = erbdParticipants[fbsParticipant.Id];
                            foreach (ERBDCertificate erbdCertificate in erbdParticipant.Certificates)
                            {
                                ExecuteCommand(connection, erbdCertificate);

                                foreach (ERBDCertificateMark erbdCertificateMark in erbdCertificate.CertificateMarks)
                                {
                                    ExecuteCommand(connection, erbdCertificateMark);
                                }
                                if (erbdCertificate.CancelledCertificate != null)
                                {
                                    ExecuteCommand(connection, erbdCertificate.CancelledCertificate);
                                }
                            }

                            foreach (ERBDCertificateMark erbdCertificateMark in erbdParticipant.CertificateMarks)
                            {
                                ExecuteCommand(connection, erbdCertificateMark);
                            }
                        }

                        if (fbsParticipant.Action == ERBDToFBSActions.Delete)
                        {
                            ExecuteCommand(connection, fbsParticipant);
                        }
                        else if (fbsParticipant.Action == ERBDToFBSActions.UpdateRelated)
                        {
                            ExecuteCommand(connection, erbdParticipant);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLine("ОШИБКА обработки записи (участник ЕГЭ ФБС: ParticipantID = " + fbsParticipant.Id.ParticipantID.ToString() + ", UseYear = " + fbsParticipant.Id.UseYear.ToString() + ", REGION=" + fbsParticipant.Id.REGION.ToString() + "; действие: " + fbsParticipant.Action.ToString() + "): " + ex.Message + " (" + ex.StackTrace + ")");
                    }
                    counter++;
                    if (counter % MessageSize == 0)
                    {
                        Logger.WriteLine(String.Format("Обработано {0} участников ЕГЭ", counter));
                    }
                }

                foreach (ERBDParticipant erbdParticipant in erbdParticipants.Values)
                {
                    if ((erbdParticipant.Action != ERBDToFBSActions.Insert) && (erbdParticipant.Action != ERBDToFBSActions.Update) && (erbdParticipant.Action != ERBDToFBSActions.UpdateRelated))
                        continue;

                    try
                    {
                        ExecuteHideCommand(connection, erbdParticipant);

                        foreach (ERBDCertificate erbdCertificate in erbdParticipant.Certificates)
                        {
                            ExecuteCommand(connection, erbdCertificate);

                            foreach (ERBDCertificateMark erbdCertificateMark in erbdCertificate.CertificateMarks)
                            {
                                ExecuteCommand(connection, erbdCertificateMark);
                            }
                            if (erbdCertificate.CancelledCertificate != null)
                            {
                                ExecuteCommand(connection, erbdCertificate.CancelledCertificate);
                            }
                        }

                        foreach (ERBDCertificateMark erbdCertificateMark in erbdParticipant.CertificateMarks)
                        {
                            ExecuteCommand(connection, erbdCertificateMark);
                        }

                        ExecuteCommand(connection, erbdParticipant);
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLine("ОШИБКА обработки записи (участник ЕГЭ ЕРБД: ParticipantID = " + erbdParticipant.Id.ParticipantID.ToString() + ", UseYear = " + erbdParticipant.Id.UseYear.ToString() + ", REGION=" + erbdParticipant.Id.REGION.ToString() + "; действие: " + erbdParticipant.Action.ToString() + "): " + ex.Message + " (" + ex.StackTrace + ")");
                    }

                    counter++;
                    if (counter % MessageSize == 0)
                    {
                        Logger.WriteLine(String.Format("Обработано {0} участников ЕГЭ", counter));
                    }
                }

                foreach (FBSCertificate fbsCertificate in fbsCertificatesWithoutParticipant.Values)
                {
                    try
                    {
                        ExecuteCommand(connection, fbsCertificate);
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLine("ОШИБКА обработки записи (свидетельство ФБС без связи с участником ЕГЭ: CertificateID = " + fbsCertificate.Id.CertificateID.ToString() + ", UseYear = " + fbsCertificate.Id.UseYear.ToString() + ", REGION=" + fbsCertificate.Id.REGION.ToString() + "; действие: " + fbsCertificate.Action.ToString() + "): " + ex.Message + " (" + ex.StackTrace + ")");
                    }
                }
                foreach (ERBDCertificate erbdCertificate in erbdCertificatesWithoutParticipant.Values)
                {
                    try
                    {
                        ExecuteCommand(connection, erbdCertificate);
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLine("ОШИБКА обработки записи (свидетельство ЕРБД без связи с участником ЕГЭ: CertificateID = " + erbdCertificate.Id.CertificateID.ToString() + ", UseYear = " + erbdCertificate.Id.UseYear.ToString() + ", REGION=" + erbdCertificate.Id.REGION.ToString() + "; действие: " + erbdCertificate.Action.ToString() + "): " + ex.Message + " (" + ex.StackTrace + ")");
                    }
                }
                foreach (FBSCertificateMark fbsCertificateMark in certificateMarksWithoutParticipant.Values)
                {
                    try
                    {
                        ExecuteCommand(connection, fbsCertificateMark);
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLine("ОШИБКА обработки записи (балл ФБС ЕРБД без связи с участником ЕГЭ: CertificateMarkID = " + fbsCertificateMark.Id.CertificateMarkID.ToString() + ", UseYear = " + fbsCertificateMark.Id.UseYear.ToString() + ", REGION=" + fbsCertificateMark.Id.REGION.ToString() + "; действие: " + fbsCertificateMark.Action.ToString() + "): " + ex.Message + " (" + ex.StackTrace + ")");
                    }
                }
            }
        }

        public void WriteChanges(Dictionary<int, IEnumerable<FBSPerson>> fbsPersons)
        {
            using (SqlConnection connection = Connections.CreateFBSConnection())
            {
                int counter = 0;
                connection.Open();
                foreach (IEnumerable<FBSPerson> fbsPersonsByName in fbsPersons.Values)
                {
                    foreach (FBSPerson fbsPerson in fbsPersonsByName)
                    {
                        if (fbsPerson.Action != FBSToGVUZActions.Link)
                            continue;

                        try
                        {
                            ExecuteCommand(connection, fbsPerson);
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteLine("ОШИБКА обработки записи (физическое лицо ФБС: ParticipantID = " + fbsPerson.Id.ParticipantID.ToString() + ", UseYear = " + fbsPerson.Id.UseYear.ToString() + ", REGION=" + fbsPerson.Id.REGION.ToString() + "; действие: " + fbsPerson.Action.ToString() + "): " + ex.Message + " (" + ex.StackTrace + ")");
                        }
                        counter++;
                        if (counter % MessageSize == 0)
                        {
                            Logger.WriteLine(String.Format("Обработано {0} физическое лиц", counter));
                        }
                    }
                }
            }
        }

        private void ExecuteHideCommand(SqlConnection connection, FBSParticipant participant)
        {
            if ((participant.Action == ERBDToFBSActions.Delete) || (participant.Action == ERBDToFBSActions.UpdateRelated))
            {
                SqlCommand hideCommand = connection.CreateCommand();
                hideCommand.CommandTimeout = CommandTimeout;
                Queries.Write_FBS.UpdateToHiddenParticipantQuery(_tables, hideCommand, participant.Id);
                hideCommand.ExecuteNonQuery();
            }
        }

        private void ExecuteHideCommand(SqlConnection connection, ERBDParticipant participant)
        {
            if ((participant.Action == ERBDToFBSActions.UpdateRelated) || (participant.Action == ERBDToFBSActions.Update))
            {
                SqlCommand hideCommand = connection.CreateCommand();
                hideCommand.CommandTimeout = CommandTimeout;
                Queries.Write_FBS.UpdateToHiddenParticipantQuery(_tables, hideCommand, participant.Id);
                hideCommand.ExecuteNonQuery();
            }
            if (participant.Action == ERBDToFBSActions.Insert)
            {
                SqlCommand insertHideCommand = connection.CreateCommand();
                insertHideCommand.CommandTimeout = CommandTimeout;
                Queries.Write_FBS.InsertHiddenParticipantQuery(_tables, insertHideCommand, participant);
                insertHideCommand.ExecuteNonQuery();
            }
        }

        private void ExecuteCommand(SqlConnection connection, FBSParticipant participant)
        {
            if (participant.Action == ERBDToFBSActions.Delete)
            {
                SqlCommand deleteCommand = connection.CreateCommand();
                deleteCommand.CommandTimeout = CommandTimeout;
                Queries.Write_FBS.DeleteParticipantQuery(_tables, deleteCommand, participant.Id);
                deleteCommand.ExecuteNonQuery();
                participant.Action = ERBDToFBSActions.Done;
            }
        }

        private void ExecuteCommand(SqlConnection connection, ERBDParticipant participant)
        {
            SqlCommand updateCommand = connection.CreateCommand();
            updateCommand.CommandTimeout = CommandTimeout;
            Queries.Write_FBS.UpdateParticipantQuery(_tables, updateCommand, participant);
            updateCommand.ExecuteNonQuery();
            participant.Action = ERBDToFBSActions.Done;
        }

        private void ExecuteCommand(SqlConnection connection, FBSCertificate certificate)
        {
            if (certificate.Action == ERBDToFBSActions.Delete)
            {
                SqlCommand deleteCommand = connection.CreateCommand();
                deleteCommand.CommandTimeout = CommandTimeout;
                Queries.Write_FBS.DeleteCertificateQuery(_tables, deleteCommand, certificate.Id);
                deleteCommand.ExecuteNonQuery();
                certificate.Action = ERBDToFBSActions.Done;
            }
        }

        private void ExecuteCommand(SqlConnection connection, ERBDCertificate certificate)
        {
            if (certificate.Action == ERBDToFBSActions.Update)
            {
                SqlCommand updateCommand = connection.CreateCommand();
                updateCommand.CommandTimeout = CommandTimeout;
                Queries.Write_FBS.UpdateCertificateQuery(_tables, updateCommand, certificate);
                updateCommand.ExecuteNonQuery();
                certificate.Action = ERBDToFBSActions.Done;
            }
            if (certificate.Action == ERBDToFBSActions.Insert)
            {
                SqlCommand insertCommand = connection.CreateCommand();
                insertCommand.CommandTimeout = CommandTimeout;
                Queries.Write_FBS.InsertCertificateQuery(_tables, insertCommand, certificate);
                insertCommand.ExecuteNonQuery();
                certificate.Action = ERBDToFBSActions.Done;
            }
        }

        private void ExecuteCommand(SqlConnection connection, FBSCertificateMark certificateMark)
        {
            if (certificateMark.Action == ERBDToFBSActions.Delete)
            {
                SqlCommand deleteCommand = connection.CreateCommand();
                deleteCommand.CommandTimeout = CommandTimeout;
                Queries.Write_FBS.DeleteCertificateMarkQuery(_tables, deleteCommand, certificateMark.Id);
                deleteCommand.ExecuteNonQuery();
                certificateMark.Action = ERBDToFBSActions.Done;
            }
        }

        private void ExecuteCommand(SqlConnection connection, ERBDCertificateMark certificateMark)
        {
            if (certificateMark.Action == ERBDToFBSActions.Update)
            {
                SqlCommand updateCommand = connection.CreateCommand();
                updateCommand.CommandTimeout = CommandTimeout;
                Queries.Write_FBS.UpdateCertificateMarkQuery(_tables, updateCommand, certificateMark);
                updateCommand.ExecuteNonQuery();
                certificateMark.Action = ERBDToFBSActions.Done;
            }
            if (certificateMark.Action == ERBDToFBSActions.Insert)
            {
                SqlCommand insertCommand = connection.CreateCommand();
                insertCommand.CommandTimeout = CommandTimeout;
                Queries.Write_FBS.InsertCertificateMarkQuery(_tables, insertCommand, certificateMark);
                insertCommand.ExecuteNonQuery();
                certificateMark.Action = ERBDToFBSActions.Done;
            }
        }

        private void ExecuteCommand(SqlConnection connection, FBSCancelledCertificate cancelledCertificate)
        {
            if (cancelledCertificate.Action == ERBDToFBSActions.Delete)
            {
                SqlCommand deleteCommand = connection.CreateCommand();
                deleteCommand.CommandTimeout = CommandTimeout;
                Queries.Write_FBS.DeleteCancelledCertificateQuery(_tables, deleteCommand, cancelledCertificate.Id);
                deleteCommand.ExecuteNonQuery();
                cancelledCertificate.Action = ERBDToFBSActions.Done;
            }
        }

        private void ExecuteCommand(SqlConnection connection, ERBDCancelledCertificate cancelledCertificate)
        {
            if (cancelledCertificate.Action == ERBDToFBSActions.Update)
            {
                SqlCommand updateCommand = connection.CreateCommand();
                updateCommand.CommandTimeout = CommandTimeout;
                Queries.Write_FBS.UpdateCancelledCertificateQuery(_tables, updateCommand, cancelledCertificate);
                updateCommand.ExecuteNonQuery();
                cancelledCertificate.Action = ERBDToFBSActions.Done;
            }
            if (cancelledCertificate.Action == ERBDToFBSActions.Insert)
            {
                SqlCommand insertCommand = connection.CreateCommand();
                insertCommand.CommandTimeout = CommandTimeout;
                Queries.Write_FBS.InsertCancelledCertificateQuery(_tables, insertCommand, cancelledCertificate);
                insertCommand.ExecuteNonQuery();
                cancelledCertificate.Action = ERBDToFBSActions.Done;
            }
        }

        private void ExecuteCommand(SqlConnection connection, FBSPerson person)
        {
            if (person.Action == FBSToGVUZActions.Link)
            {
                SqlCommand linkCommand = connection.CreateCommand();
                linkCommand.CommandTimeout = CommandTimeout;
                Queries.Write_FBS.LinkPersonQuery(_tables, linkCommand, person);
                linkCommand.ExecuteNonQuery();
                person.Action = FBSToGVUZActions.Done;
            }
        }
    }
}
