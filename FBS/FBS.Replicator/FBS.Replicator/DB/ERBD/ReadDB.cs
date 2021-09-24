using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using FBS.Common;
using FBS.Replicator.DB.FBS;
using FBS.Replicator.Entities;
using FBS.Replicator.Entities.ERBD;
using FBS.Replicator.Entities.FBS;

namespace FBS.Replicator.DB.ERBD
{
    public class ERBDReadDB
    {
        private const int MessageSize = 300000;

        private int _year;
        private bool _loadCompositions2015;
        private bool _loadCompositions2016Plus;

        public int SqlCommandTimeout { get; set; }

        public ERBDReadDB(int year, bool loadCompositions2015, bool loadCompositions2016Plus)
        {
            _year = year;
            _loadCompositions2015 = loadCompositions2015;
            _loadCompositions2016Plus = loadCompositions2016Plus;
        }

        public bool GetAllParticipantsWithData(out Dictionary<ParticipantId, ERBDParticipant> participants, out Dictionary<CertificateId, ERBDCertificate> certificatesWithoutParticipant)
        {
            try
            {
                participants = GetAllParticipants();
                certificatesWithoutParticipant = new Dictionary<CertificateId, ERBDCertificate>();

                Dictionary<CertificateId, ERBDCertificate> certificates = GetAllCertificates();
                Dictionary<FBSMinimalMarkId, FBSMinimalMark> minimalMarks = FBSReadDB.GetAllMinimalMarks();
                Dictionary<short, FBSExpireDate> expireDates = FBSReadDB.GetAllExpireDates();
                IEnumerable<ERBDCertificateMark> certificateMarks = GetAllCertificateMarks(expireDates, minimalMarks);
                IEnumerable<ERBDCancelledCertificate> cancelledCertificates = GetAllCancelledCertificates();

                Logger.WriteLine(string.Format("Получено свидетельств: {0}", certificates.Count));
                Logger.WriteLine(string.Format("         мин. баллов: {0}", minimalMarks.Count));
                Logger.WriteLine(string.Format("         дат истечния: {0}", expireDates.Count));

                foreach (ERBDCancelledCertificate cancelledCertificate in cancelledCertificates)
                {
                    if (certificates.ContainsKey(cancelledCertificate.Id))
                    {
                        certificates[cancelledCertificate.Id].SetCancelledCertificate(cancelledCertificate);
                    }
                }

                foreach (ERBDCertificateMark certificateMark in certificateMarks)
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
                        }
                    }
                    else
                    {
                        ParticipantId participantId = new ParticipantId(certificateMark);
                        if (participants.ContainsKey(participantId))
                        {
                            participants[participantId].AddCertificateMark(certificateMark);
                        }
                    }
                }

                foreach (ERBDCertificate certificate in certificates.Values)
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

                if (_loadCompositions2015 || _loadCompositions2016Plus)
                {
                    //Logger.WriteLine("(_loadCompositions2015 || _loadCompositions2016Plus)");
                    var compositionInfos = GetAllCompositionInfos(_loadCompositions2015 && (_year == 2015), _loadCompositions2016Plus && (_year >= 2016));
                    Logger.WriteLine(string.Format("Получена информация о сочинениях: {0}", compositionInfos.Count));

                    foreach (ERBDCertificateMark certificateMark in certificateMarks)
                    {
                        ERBDParticipant participant = certificateMark.Participant;

                        if ((participant == null) && (certificateMark.Certificate != null))
                        {
                            participant = certificateMark.Certificate.Participant;
                        }
                        if (participant == null)
                        {
                            //Logger.WriteLine(string.Format("participant == null, continue"));
                            continue;
                        }
                        //Logger.WriteLine(string.Format("participant = {0}, certificateMark.Id.UseYear = {1}", participant.Id.ParticipantID.ToString(), certificateMark.Id.UseYear.ToString()));

                        if (certificateMark.Id.UseYear <= 2015)
                        {
                            //Logger.WriteLine("LoadCompositions 2015");
                            if (!_loadCompositions2015)
                                continue;

                            string key = participant.Id.ParticipantID.ToString();
                            //if (compositionInfos.ContainsKey(key))
                            //var compositionInfo = compositionInfos.FirstOrDefault(t => t.Key == key);
                            var compositionInfo = compositionInfos.ContainsKey(key) ? compositionInfos[key] : null;
                            if (compositionInfo != null)
                            {
                                string paths = CompositionPathsHelper.GetCompositionPaths2015(Connections.CompositionsStaticPath2015, participant.Id.ParticipantID, participant.DocumentNumberStr, participant.NameStr, participant.SurnameStr, participant.SecondNameStr, compositionInfo.PagesCount);
                                certificateMark.SetCompositionsData(compositionInfo.PagesCount, paths);
                            }
                        }
                        else//2016+
                        {
                            if (!_loadCompositions2016Plus)
                            {
                                //Logger.WriteLine("LoadCompositions2016Plus - continue1");
                                continue;
                            }

                            if ((certificateMark.CompositionProjectName == null)
                               || (!certificateMark.ExamDate.HasValue)
                               || (!certificateMark.CompositionProjectBatchID.HasValue)
                               || (certificateMark.CompositionBarcode == null))
                            {

                                Logger.WriteLine(string.Format("Пустые значения в оценках сочинения (humantest: {0}): пр: {1}, дата {2}, id пакета {3} ш/к {4}", certificateMark.Id.CertificateMarkID
                                    , certificateMark.CompositionProjectName == null
                                    , !certificateMark.ExamDate.HasValue
                                    , !certificateMark.CompositionProjectBatchID.HasValue
                                    , certificateMark.CompositionBarcode == null
                                    ));
                                continue;
                            }

                            string key = certificateMark.CompositionBarcodeStr;
                            //Logger.WriteLine(string.Format("LoadCompositions2016Plus - key: {0}", key));

                            //var compositionInfo = compositionInfos.FirstOrDefault(t => t.Key == key);
                            var compositionInfo = compositionInfos.ContainsKey(key) ? compositionInfos[key] : null;
                            if (compositionInfo != null)
                            {
                                string paths = CompositionPathsHelper.GetCompositionPaths2016Plus(Connections.CompositionsStaticPath2016Plus, certificateMark.CompositionBarcodeStr, certificateMark.CompositionProjectBatchID.Value, certificateMark.CompositionProjectNameStr, certificateMark.ExamDate.Value, compositionInfo.PagesCount);
                                certificateMark.SetCompositionsData(compositionInfo.PagesCount, paths);
                                //Logger.WriteLine("LoadCompositions2016Plus after SetCompositionsData");
                            }
                            else
                            {
                                //Logger.WriteLine(string.Format("LoadCompositions2016Plus - continue3 (no key)"));
                                Logger.WriteLine(string.Format("ВНИМАНИЕ! Нет данных (k: {0}, test_id: {1})", key, certificateMark.Id.CertificateMarkID));
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.WriteLine("ОШИБКА получения данных из ЕРБД: " + ex.Message + " (" + ex.StackTrace + ")");
                participants = null;
                certificatesWithoutParticipant = null;
                return false;
            }
        }

        private Dictionary<ParticipantId, ERBDParticipant> GetAllParticipants()
        {
            Dictionary<ParticipantId, ERBDParticipant> result = new Dictionary<ParticipantId, ERBDParticipant>(1000000);
            using (SqlConnection connection = Connections.CreateERBDConnection())
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandTimeout = this.SqlCommandTimeout;
                Queries.Read_ERBD.ParticipantsQuery(cmd, _year);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    FastDataReader fReader = new FastDataReader(reader);

                    int counter = 0;
                    while (reader.Read())
                    {
                        ERBDParticipant entity = new ERBDParticipant(fReader);
                        result.Add(entity.Id, entity);
                        counter++;
                        if (counter % MessageSize == 0)
                        {
                            Logger.WriteLine(String.Format("Загружено {0} участников ЕГЭ ЕРБД", counter));
                        }
                    }
                    Logger.WriteLine(String.Format("Загружено {0} участников ЕГЭ ЕРБД", counter));
                }
            }
            return result;
        }

        private IEnumerable<ERBDCertificateMark> GetAllCertificateMarks(Dictionary<short, FBSExpireDate> expireDates, Dictionary<FBSMinimalMarkId, FBSMinimalMark> minimalMarks)
        {
            Dictionary<CertificateMarkId, ERBDCertificateMark> resultDictionary = new Dictionary<CertificateMarkId, ERBDCertificateMark>(1000000);
            using (SqlConnection connection = Connections.CreateERBDConnection())
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandTimeout = this.SqlCommandTimeout;
                Queries.Read_ERBD.CertificateMarksQuery(cmd, _year);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    FastDataReader fReader = new FastDataReader(reader);

                    int counter = 0;
                    while (reader.Read())
                    {
                        ERBDCertificateMark entity = new ERBDCertificateMark(fReader, expireDates, minimalMarks);

                        resultDictionary.Add(entity.Id, entity);


                        counter++;
                        if (counter % MessageSize == 0)
                        {
                            Logger.WriteLine(String.Format("Загружено {0} баллов ЕРБД", counter));
                        }
                    }
                    Logger.WriteLine(String.Format("Загружено {0} баллов ЕРБД", counter));
                }

                Queries.Read_ERBD.HumanTestsQuery(cmd, _year, _loadCompositions2015 || _loadCompositions2016Plus);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    FastDataReader fReader = new FastDataReader(reader);

                    int counter = 0;
                    while (reader.Read())
                    {
                        ERBDHumanTest humanTest = new ERBDHumanTest(fReader);

                        ERBDCertificateMark entity = new ERBDCertificateMark(humanTest, expireDates, minimalMarks);

                        if (!resultDictionary.ContainsKey(entity.Id))
                        {
                            resultDictionary.Add(entity.Id, entity);
                        }
                        else
                        {
                            if (entity.CompositionStatus.GetValueOrDefault() == CompositionsHelper.OKStatus)
                            {
                                resultDictionary[entity.Id] = entity;
                            }
                        }

                        counter++;
                        if (counter % MessageSize == 0)
                        {
                            Logger.WriteLine(String.Format("Загружено {0} результатов ЕРБД", counter));
                        }
                    }
                    Logger.WriteLine(String.Format("Загружено {0} результатов ЕРБД", counter));
                }
            }

            List<ERBDCertificateMark> result = new List<ERBDCertificateMark>(resultDictionary.Count);
            foreach (ERBDCertificateMark entity in resultDictionary.Values)
            {
                result.Add(entity);
            }

            return result;
        }

        private Dictionary<CertificateId, ERBDCertificate> GetAllCertificates()
        {
            Dictionary<CertificateId, ERBDCertificate> result = new Dictionary<CertificateId, ERBDCertificate>(1000000);
            using (SqlConnection connection = Connections.CreateERBDConnection())
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandTimeout = this.SqlCommandTimeout;

                Logger.WriteLine(String.Format("Установлен таймаут на операции со свидетельствами: {0}", this.SqlCommandTimeout));
                Queries.Read_FBS_ERBD.CertificatesQuery(null, cmd, _year);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    FastDataReader fReader = new FastDataReader(reader);

                    int counter = 0;
                    while (reader.Read())
                    {
                        ERBDCertificate entity = new ERBDCertificate(fReader);
                        result.Add(entity.Id, entity);
                        counter++;
                        if (counter % MessageSize == 0)
                        {
                            Logger.WriteLine(String.Format("Загружено {0} свидетельств ЕРБД", counter));
                        }
                    }
                    Logger.WriteLine(String.Format("Загружено {0} свидетельств ЕРБД", counter));
                }

                Queries.Read_ERBD.QueryForCertificatesByHumanTests(cmd, _year);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    FastDataReader fReader = new FastDataReader(reader);

                    while (reader.Read())
                    {
                        ERBDHumanTest humanTest = new ERBDHumanTest(fReader);
                        ERBDCertificate entity = new ERBDCertificate(humanTest);
                        if (!result.ContainsKey(entity.Id))
                        {
                            result.Add(entity.Id, entity);
                        }
                    }
                }

                Queries.Read_ERBD.QueryForCertificatesByMarks(cmd, _year);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    FastDataReader fReader = new FastDataReader(reader);

                    while (reader.Read())
                    {
                        ERBDCertificateMark certificateMark = new ERBDCertificateMark(fReader, null, null);
                        ERBDCertificate entity = new ERBDCertificate(certificateMark);
                        if (!result.ContainsKey(entity.Id))
                        {
                            result.Add(entity.Id, entity);
                        }
                    }
                }
            }
            return result;
        }

        private IEnumerable<ERBDCancelledCertificate> GetAllCancelledCertificates()
        {
            List<ERBDCancelledCertificate> result = new List<ERBDCancelledCertificate>(10000);
            using (SqlConnection connection = Connections.CreateERBDConnection())
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandTimeout = this.SqlCommandTimeout;
                Queries.Read_FBS_ERBD.CancelledCertificatesQuery(null, cmd, _year);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    FastDataReader fReader = new FastDataReader(reader);

                    int counter = 0;
                    while (reader.Read())
                    {
                        ERBDCancelledCertificate entity = new ERBDCancelledCertificate(fReader);
                        result.Add(entity);
                        counter++;
                    }
                    Logger.WriteLine(String.Format("Загружено {0} аннулированных свидетельств ЕРБД", counter));
                }
            }
            return result;
        }

        private Dictionary<string, ERBDCompositionInfo> GetAllCompositionInfos(bool for2015, bool for2016)
        {
            return CompositionsHelper.GetAllCompositionInfos(for2015, for2016);
        }

        //private List<ERBDCompositionInfo> GetAllCompositionInfos(bool for2015, bool for2016)
        //{
        //    List<ERBDCompositionInfo> res = null;
        //    using (ICompositionExportService service = new CompositionExportServiceClient())
        //    {
        //        var info = service.GetAllCompositionInfos(for2015, for2016);
        //        res = info.Items;
        //    }
        //    return res;
        //}
    }
}
