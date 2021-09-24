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
    public class GVUZWriteDB
    {
        private const int MessageSize = 10000;
        private const int CommandTimeout = 300;

        public GVUZWriteDB()
        {
        }

        public void WriteChanges(Dictionary<int, IEnumerable<FBSPerson>> fbsPersons)
        {
            using (SqlConnection connection = Connections.CreateGVUZConnection())
            {
                int counter = 0;
                connection.Open();
                foreach (IEnumerable<FBSPerson> fbsPersonsByName in fbsPersons.Values)
                {
                    foreach (FBSPerson fbsPerson in fbsPersonsByName)
                    {
                        if ((fbsPerson.Action != FBSToGVUZActions.Update) && (fbsPerson.Action != FBSToGVUZActions.Insert))
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

        private void ExecuteCommand(SqlConnection connection, FBSPerson person)
        {
            if (person.Action == FBSToGVUZActions.Update)
            {
                SqlCommand updateCommand = connection.CreateCommand();
                updateCommand.CommandTimeout = CommandTimeout;
                Queries.Write_GVUZ.UpdatePersonQuery(updateCommand, person);
                updateCommand.ExecuteNonQuery();

                person.Action = FBSToGVUZActions.Link;
            }
            if (person.Action == FBSToGVUZActions.Insert)
            {
                SqlCommand insertPersonCommand = connection.CreateCommand();
                insertPersonCommand.CommandTimeout = CommandTimeout;
                person.SetPersonId(GetMaxId(connection, Queries.Read_GVUZ.MaxPersonId) + 1);
                Queries.Write_GVUZ.InsertPersonQuery(insertPersonCommand, person);
                try
                {
                    insertPersonCommand.ExecuteNonQuery();
                }
                catch
                {
                    person.SetPersonId(null);
                    throw;
                }

                person.Action = FBSToGVUZActions.Link;

                SqlCommand insertDocumentCommand = connection.CreateCommand();
                insertDocumentCommand.CommandTimeout = CommandTimeout;
                int documentId = GetMaxId(connection, Queries.Read_GVUZ.MaxDocumentId) + 1;
                Queries.Write_GVUZ.InsertDocumentQuery(insertDocumentCommand, person.Document, documentId);
                insertDocumentCommand.ExecuteNonQuery();
            }
        }

        private int GetMaxId(SqlConnection connection, string sql)
        {
            SqlCommand createTempTableCmd = connection.CreateCommand();
            createTempTableCmd.CommandText = sql;
            return (int)createTempTableCmd.ExecuteScalar();
        }
    }
}
