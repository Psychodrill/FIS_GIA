using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using FBS.Replicator.Entities.GVUZ;
using FBS.Common;

namespace FBS.Replicator.DB.GVUZ
{
    public class GVUZReadDB
    {
        private const int MessageSize = 300000;

        public bool GetAllPersonsWithDocuments(out Dictionary<int, IEnumerable<GVUZPerson>> persons)
        {
            try
            {
                Dictionary<int, GVUZPerson> personsById = GetAllPersons();
                IEnumerable<GVUZIdentityDocument> documents = GetAllIdentityDocuments();
                foreach (GVUZIdentityDocument document in documents)
                {
                    if (personsById.ContainsKey(document.PersonId))
                    {
                        personsById[document.PersonId].AddDocument(document);
                    }
                }

                persons = new Dictionary<int, IEnumerable<GVUZPerson>>();
                foreach (GVUZPerson person in personsById.Values)
                {
                    if (!persons.ContainsKey(person.NameHashCode))
                    {
                        persons.Add(person.NameHashCode, new List<GVUZPerson>() { person });
                    }
                    else
                    {
                        (persons[person.NameHashCode] as IList<GVUZPerson>).Add(person);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.WriteLine("ОШИБКА получения данных из РВИ: " + ex.Message + " (" + ex.StackTrace + ")");
                persons = null;
                return false;
            }
        }

        private Dictionary<int, GVUZPerson> GetAllPersons()
        {
            Dictionary<int, GVUZPerson> result = new Dictionary<int, GVUZPerson>(1000000);
            using (SqlConnection connection = Connections.CreateGVUZConnection())
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandTimeout = 300;
                Queries.Read_GVUZ.PersonsQuery(cmd);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    FastDataReader fReader = new FastDataReader(reader);

                    int counter = 0;
                    while (reader.Read())
                    {
                        GVUZPerson entity = new GVUZPerson(fReader);
                        result.Add(entity.Id, entity);
                        counter++;
                        if (counter % MessageSize == 0)
                        {
                            Logger.WriteLine(String.Format("Загружено {0} записей о физических лицах РВИ", counter));
                        }
                    }
                    Logger.WriteLine(String.Format("Загружено {0} записей о физических лицах РВИ", counter));
                }
            }
            return result;
        }

        private IEnumerable<GVUZIdentityDocument> GetAllIdentityDocuments()
        {
            List<GVUZIdentityDocument> result = new List<GVUZIdentityDocument>(1000000);
            using (SqlConnection connection = Connections.CreateGVUZConnection())
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandTimeout = 300;
                Queries.Read_GVUZ.IdentityDocumentsQuery(cmd);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    FastDataReader fReader = new FastDataReader(reader);

                    int counter = 0;
                    while (reader.Read())
                    {
                        GVUZIdentityDocument entity = new GVUZIdentityDocument(fReader);
                        result.Add(entity);
                        counter++;
                        if (counter % MessageSize == 0)
                        {
                            Logger.WriteLine(String.Format("Загружено {0} записей о документах РВИ", counter));
                        }
                    }
                    Logger.WriteLine(String.Format("Загружено {0} записей о документах РВИ", counter));
                }
            }
            return result;
        }
    }
}
