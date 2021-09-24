using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using FBS.Replicator.Entities;
using FBS.Replicator.Entities.FBS;
using FBS.Common;

namespace FBS.Replicator.DB.FBS
{
    public class FBSReadDB
    {
        private const int MessageSize = 300000;
        private static int cmd_timeout = 300;

        public static int CommandTimeout
        {
            get { return cmd_timeout; }
            set { cmd_timeout = value; }
        }

        private Tables _tables;
        private int? _year;
        
        public FBSReadDB(Tables tables)
        {
            _tables = tables;
        }

        public FBSReadDB(Tables tables, int year )
        {
            _tables = tables;
            _year = year; 
        }

        #region Participants And Certificates

        public bool GetAllParticipantsWithData(out Dictionary<ParticipantId, FBSParticipant> participants, out Dictionary<CertificateId, FBSCertificate> certificatesWithoutParticipant, out Dictionary<CertificateMarkId, FBSCertificateMark> certificateMarksWithoutParticipant)
        {
            try
            {
                CheckYearAndERBDVersionSet();

                participants = GetAllParticipants();
                certificatesWithoutParticipant = new Dictionary<CertificateId, FBSCertificate>();
                certificateMarksWithoutParticipant = new Dictionary<CertificateMarkId, FBSCertificateMark>();

                Dictionary<CertificateId, FBSCertificate> certificates = GetAllCertificates();

                IEnumerable<FBSCertificateMark> certificateMarks = GetAllCertificateMarks();
                IEnumerable<FBSCancelledCertificate> cancelledCertificates = GetAllCancelledCertificates();

                foreach (FBSCancelledCertificate cancelledCertificate in cancelledCertificates)
                {
                    if (certificates.ContainsKey(cancelledCertificate.Id))
                    {
                        certificates[cancelledCertificate.Id].SetCancelledCertificate(cancelledCertificate);
                    }
                }

                foreach (FBSCertificateMark certificateMark in certificateMarks)
                {
                    CertificateId certificateId = new CertificateId(certificateMark);
                    if (certificates.ContainsKey(certificateId))
                    {
                        certificates[certificateId].AddCertificateMark(certificateMark);

                        if (!certificates[certificateId].ParticipantFK.HasValue)
                        {
                            ParticipantId participantId = new ParticipantId(certificateMark);
                            if (participants.ContainsKey(participantId))
                            {
                                participants[participantId].AddCertificateMark(certificateMark);
                            }
                            else
                            {
                                certificateMarksWithoutParticipant.Add(certificateMark.Id, certificateMark);
                            }
                        }
                    }
                    else
                    {
                        ParticipantId participantId = new ParticipantId(certificateMark);
                        if (participants.ContainsKey(participantId))
                        {
                            participants[participantId].AddCertificateMark(certificateMark);
                        }
                        else
                        {
                            certificateMarksWithoutParticipant.Add(certificateMark.Id, certificateMark);
                        }
                    }
                }

                foreach (FBSCertificate certificate in certificates.Values)
                {
                    if (!certificate.ParticipantFK.HasValue)
                    {
                        certificatesWithoutParticipant.Add(certificate.Id, certificate);
                    }
                    else
                    {
                        ParticipantId participantId = new ParticipantId(certificate);
                        if (participants.ContainsKey(participantId))
                        {
                            participants[participantId].AddCertificate(certificate);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.WriteLine("ОШИБКА получения данных из ФБС: " + ex.Message + " (" + ex.StackTrace + ")");

                participants = null;
                certificatesWithoutParticipant = null;
                certificateMarksWithoutParticipant = null;
                return false;
            }
        }

        public static Dictionary<FBSMinimalMarkId, FBSMinimalMark> GetAllMinimalMarks()
        {
            Dictionary<FBSMinimalMarkId, FBSMinimalMark> result = new Dictionary<FBSMinimalMarkId, FBSMinimalMark>();
            using (SqlConnection connection = Connections.CreateFBSConnection())
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandTimeout = CommandTimeout;
                Queries.Read_FBS.MinimalMarksQuery(cmd);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    FastDataReader fReader = new FastDataReader(reader);

                    while (reader.Read())
                    {
                        FBSMinimalMark entity = new FBSMinimalMark(fReader);
                        result.Add(entity.Id, entity);
                    }
                }
            }
            return result;
        }

        public static Dictionary<short, FBSExpireDate> GetAllExpireDates()
        {
            Dictionary<short, FBSExpireDate> result = new Dictionary<short, FBSExpireDate>();
            using (SqlConnection connection = Connections.CreateFBSConnection())
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandTimeout = CommandTimeout;
                Queries.Read_FBS.ExpireDatesQuery(cmd);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    FastDataReader fReader = new FastDataReader(reader);

                    while (reader.Read())
                    {
                        FBSExpireDate entity = new FBSExpireDate(fReader);
                        result.Add(entity.Year, entity);
                    }
                }
            }
            return result;
        }

        private Dictionary<ParticipantId, FBSParticipant> GetAllParticipants()
        {
            Dictionary<ParticipantId, FBSParticipant> result = new Dictionary<ParticipantId, FBSParticipant>(1000000);
            using (SqlConnection connection = Connections.CreateFBSConnection())
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandTimeout = CommandTimeout;
                Queries.Read_FBS.ParticipantsQuery(_tables, cmd, _year);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    FastDataReader fReader = new FastDataReader(reader);

                    int counter = 0;
                    while (reader.Read())
                    {
                        FBSParticipant entity = new FBSParticipant(fReader);
                        result.Add(entity.Id, entity);
                        counter++;
                        if (counter % MessageSize == 0)
                        {
                            Logger.WriteLine(String.Format("Загружено {0} участников ЕГЭ ФБС", counter));
                        }
                    }
                    Logger.WriteLine(String.Format("Загружено {0} участников ЕГЭ ФБС", counter));
                }
            }
            return result;
        }

        private IEnumerable<FBSCertificateMark> GetAllCertificateMarks()
        {
            List<FBSCertificateMark> result = new List<FBSCertificateMark>(1000000);
            using (SqlConnection connection = Connections.CreateFBSConnection())
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandTimeout = CommandTimeout;
                Queries.Read_FBS.CertificateMarksQuery(_tables, cmd, _year.Value);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    FastDataReader fReader = new FastDataReader(reader);

                    int counter = 0;
                    while (reader.Read())
                    {
                        FBSCertificateMark entity = new FBSCertificateMark(fReader);
                        result.Add(entity);
                        counter++;
                        if (counter % MessageSize == 0)
                        {
                            Logger.WriteLine(String.Format("Загружено {0} баллов ФБС", counter));
                        }
                    }
                    Logger.WriteLine(String.Format("Загружено {0} баллов ФБС", counter));
                }
            }

            return result;
        }

        private Dictionary<CertificateId, FBSCertificate> GetAllCertificates()
        {
            Dictionary<CertificateId, FBSCertificate> result = new Dictionary<CertificateId, FBSCertificate>(1000000);
            using (SqlConnection connection = Connections.CreateFBSConnection())
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandTimeout = CommandTimeout;
                Queries.Read_FBS_ERBD.CertificatesQuery(_tables, cmd, _year.Value);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    FastDataReader fReader = new FastDataReader(reader);

                    int counter = 0;
                    while (reader.Read())
                    {
                        FBSCertificate entity = new FBSCertificate(fReader);
                        result.Add(entity.Id, entity);
                        counter++;
                        if (counter % MessageSize == 0)
                        {
                            Logger.WriteLine(String.Format("Загружено {0} свидетельств ФБС", counter));
                        }
                    }
                    Logger.WriteLine(String.Format("Загружено {0} свидетельств ФБС", counter));
                }
            }
            return result;
        }

        private IEnumerable<FBSCancelledCertificate> GetAllCancelledCertificates()
        { 
            List<FBSCancelledCertificate> result = new List<FBSCancelledCertificate>(10000);
            using (SqlConnection connection = Connections.CreateFBSConnection())
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandTimeout = CommandTimeout;
                Queries.Read_FBS_ERBD.CancelledCertificatesQuery(_tables, cmd, _year.Value);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    FastDataReader fReader = new FastDataReader(reader);

                    int counter = 0;
                    while (reader.Read())
                    {
                        FBSCancelledCertificate entity = new FBSCancelledCertificate(fReader);
                        result.Add(entity);
                        counter++;
                    }
                    Logger.WriteLine(String.Format("Загружено {0} аннулированных свидетельств ФБС", counter));
                }
            }
            return result;
        }

        private void CheckYearAndERBDVersionSet()
        {
            if (!_year.HasValue)
                throw new Exception("Не задан год"); 
        }
        #endregion

        #region Persons
        public bool GetAllPersonsWithDocuments(out Dictionary<int,IEnumerable<FBSPerson>>   persons)
        {
            try
            {
                Helpers.ProcessData.LogMemoryStatus("Начало загрузки участников и документов...");
                List<FBSPerson> personsList = new List<FBSPerson>(1000000);
                IEnumerable<FBSGVUZDocumentTypesMapping> documentTypesMappings = GetAllDocumentTypesMappings();
                using (SqlConnection connection = Connections.CreateFBSConnection())
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandTimeout = CommandTimeout;
                    Queries.Read_FBS.ParticipantsQuery(_tables, cmd, null);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        FastDataReader fReader = new FastDataReader(reader);

                        int counter = 0;
                        while (reader.Read())
                        {
                            FBSPerson entity = new FBSPerson(fReader);
                            personsList.Add(entity);

                            FBSIdentityDocument document = new FBSIdentityDocument(entity, fReader, documentTypesMappings);

                            counter++;
                            if (counter % MessageSize == 0)
                            {
                                //Logger.WriteLine(String.Format("Загружено {0} записей о физических лицах ФБС", counter));
                                Helpers.ProcessData.LogMemoryStatus(String.Format("Загружено {0} записей о физических лицах ФБС", counter));
                            }
                        }
                        //Logger.WriteLine(String.Format("Загружено {0} записей о физических лицах ФБС", counter));
                        Helpers.ProcessData.LogMemoryStatus(String.Format("Загружено {0} записей о физических лицах ФБС", counter));
                    }
                }

                persons = new Dictionary<int, IEnumerable<FBSPerson>>();
                foreach (FBSPerson person in personsList)
                {
                    if (!persons.ContainsKey(person.NameHashCode))
                    {
                        persons.Add(person.NameHashCode, new List<FBSPerson>() { person });
                    }
                    else
                    {
                        (persons[person.NameHashCode] as IList<FBSPerson>).Add(person);
                    }
                }
                Helpers.ProcessData.LogMemoryStatus("Записи загружены");
                return true;
            }
            catch (Exception ex)
            {
                Logger.WriteLine("ОШИБКА получения данных из ФБС: " + ex.Message + " (" + ex.StackTrace + ")");
                Helpers.ProcessData.LogMemoryStatus();
                persons = null;
                return false;
            }
        }

        public static IEnumerable<FBSGVUZDocumentTypesMapping> GetAllDocumentTypesMappings()
        {
            List<FBSGVUZDocumentTypesMapping> result = new List<FBSGVUZDocumentTypesMapping>();
            using (SqlConnection connection = Connections.CreateFBSConnection())
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandTimeout = CommandTimeout;
                Queries.Read_FBS.DocumentTypesMappingQuery(cmd);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    FastDataReader fReader = new FastDataReader(reader);

                    while (reader.Read())
                    {
                        FBSGVUZDocumentTypesMapping entity = new FBSGVUZDocumentTypesMapping(fReader);
                        result.Add(entity);
                    }
                }
            }
            return result;
        }
        #endregion
    }
}
