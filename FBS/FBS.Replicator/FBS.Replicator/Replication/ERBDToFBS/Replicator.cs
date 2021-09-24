using System;
using System.Collections.Generic;
using System.Linq;
using FBS.Replicator.DB;
using FBS.Replicator.DB.ERBD;
using FBS.Replicator.DB.FBS;
using FBS.Replicator.Entities;
using FBS.Replicator.Entities.ERBD;
using FBS.Replicator.Entities.FBS;
using FBS.Common;

namespace FBS.Replicator.Replication.ERBDToFBS
{
    public class ERBDToFBSReplicator
    {
        private Tables _tables;
        private bool _loadCompositions2015;
        private bool _loadCompositions2016Plus;
        private IEnumerable<Guid> _debugIds;

        public ERBDToFBSReplicator(Tables tables, bool loadCompositions2015, bool loadCompositions2016Plus, IEnumerable<Guid> debugIds)
        {
            _tables = tables;
            _loadCompositions2015 = loadCompositions2015;
            _loadCompositions2016Plus = loadCompositions2016Plus;
            _debugIds = debugIds;
        }

        public bool HasCriticalErrors { get; private set; }
        public bool HasErrors { get; private set; }

        public bool Replicate(int year)
        {
            string sql_timeout_s = System.Configuration.ConfigurationManager.AppSettings["SqlCommandsTimeout"];
            int timeout = 300;
            if (!int.TryParse(sql_timeout_s, out timeout))
                timeout = 300;

            Logger.WriteLine(string.Format("Установлен таймаут операций SQL: {0}", timeout));

            if (_debugIds.Any())
            {
                Logger.WriteLine("Отладка синхронизации данных за " + year.ToString() + " год");
            }
            else
            {
                Logger.WriteLine("Синхронизация данных за " + year.ToString() + " год");
            }
            Logger.WriteLine("Получение данных ФБС"); 
            Dictionary<ParticipantId, FBSParticipant> fbsParticipants;
            Dictionary<CertificateId, FBSCertificate> fbsCertificatesWithoutParticipant;
            Dictionary<CertificateMarkId, FBSCertificateMark> fbsCertificateMarksWithoutParticipant;
            bool fbsReadSuccess = new FBSReadDB(_tables, year).GetAllParticipantsWithData(out fbsParticipants, out fbsCertificatesWithoutParticipant, out fbsCertificateMarksWithoutParticipant);
            GC.Collect();
            Logger.WriteLine("Получение данных ФБС завершено");
            if (!fbsReadSuccess)
            {
                Logger.WriteLine("При получении данных ФБС произошла ошибка");
                Logger.WriteLine("При синхронизации данных за " + year.ToString() + " год произошла ошибка");
                ProcessError("Синхронизация невозможна", true);
                return false;
            }

            Logger.WriteLine("Получение данных ЕРБД");
            Dictionary<ParticipantId, ERBDParticipant> erbdParticipants;
            Dictionary<CertificateId, ERBDCertificate> erbdCertificatesWithoutParticipant;
            var erbd_rdb = new ERBDReadDB(year, _loadCompositions2015, _loadCompositions2016Plus);
            erbd_rdb.SqlCommandTimeout = timeout;
            bool erbdReadSuccess = erbd_rdb.GetAllParticipantsWithData(out erbdParticipants, out erbdCertificatesWithoutParticipant);
            GC.Collect();
            Logger.WriteLine("Получение данных ЕРБД завершено");
            if (!erbdReadSuccess)
            {
                Logger.WriteLine("При получении данных ЕРБД произошла ошибка");
                Logger.WriteLine("При синхронизации данных за " + year.ToString() + " год произошла ошибка");
                ProcessError("Синхронизация невозможна", true);
                return false;
            } 

            Logger.WriteLine("Сравнение данных");
            bool mergeSuccess = ERBDToFBSMerger.Merge(fbsParticipants, fbsCertificatesWithoutParticipant, fbsCertificateMarksWithoutParticipant, erbdParticipants, erbdCertificatesWithoutParticipant);
            Logger.WriteLine("Сравнение данных завершено");
            if (!mergeSuccess)
            {
                Logger.WriteLine("При сравнении данных произошла ошибка");
                Logger.WriteLine("При синхронизации данных за " + year.ToString() + " год произошла ошибка");
                ProcessError("Синхронизация невозможна", true);
                return false;
            }

            ShowStats(fbsParticipants, fbsCertificatesWithoutParticipant, fbsCertificateMarksWithoutParticipant, erbdParticipants, erbdCertificatesWithoutParticipant);

            if (_debugIds.Any())
            {
                foreach (Guid debugId in _debugIds)
                {
                    Logger.DetailedLogger.WriteLine(String.Format("НАЧАЛО ОТЛАДКИ: {0}", debugId));

                    IEnumerable<FBSParticipant> debugFbsParticipants = fbsParticipants.Values.Where(x => x.Id.ParticipantID == debugId).ToArray();

                    if (!debugFbsParticipants.Any())
                    {
                        Logger.DetailedLogger.WriteLine("Идентификатор не найден в ФБС");
                    }
                    else
                    {
                        Logger.DetailedLogger.WriteLine(String.Format("Идентификатор найден в ФБС ({0} записей)", debugFbsParticipants));
                        foreach (FBSParticipant debugFbsParticipant in debugFbsParticipants)
                        {
                            Logger.DetailedLogger.WriteLine(String.Format("Данные: Action={0}; HashCode={1}", debugFbsParticipant.Action, debugFbsParticipant.HashCode));
                        }
                    }

                    IEnumerable<ERBDParticipant> debugErbdParticipants = erbdParticipants.Values.Where(x => x.Id.ParticipantID == debugId).ToArray();
                    if (!debugErbdParticipants.Any())
                    {
                        Logger.DetailedLogger.WriteLine("Идентификатор не найден в ЕРБД");
                    }
                    else
                    {
                        Logger.DetailedLogger.WriteLine(String.Format("Идентификатор найден в ЕРБД ({0} записей)", debugErbdParticipants));
                        foreach (ERBDParticipant debugErbdParticipant in debugErbdParticipants)
                        {
                            Logger.DetailedLogger.WriteLine(String.Format("Данные: Action={0}; DocumentNumber={1}; DocumentSeries={2}; Surname={3}; Name={4}; SecondName={5}; HashCode={6}", debugErbdParticipant.Action, debugErbdParticipant.DocumentNumberStr, debugErbdParticipant.DocumentSeriesStr, debugErbdParticipant.SurnameStr, debugErbdParticipant.NameStr, debugErbdParticipant.SecondNameStr, debugErbdParticipant.HashCode));
                        }
                    }

                    Logger.DetailedLogger.WriteLine(String.Format("ЗАВЕРШЕНИЕ ОТЛАДКИ: {0}", debugId));
                }

                ProcessError("Режим отладки, синхронизация данных за " + year.ToString() + " не выполняналась", false);
                return true;
            }

            sql_timeout_s = System.Configuration.ConfigurationManager.AppSettings["BulkTimeout"];
            string bulk_size_s = System.Configuration.ConfigurationManager.AppSettings["BulkSize"];
            string op_attempts_s = System.Configuration.ConfigurationManager.AppSettings["OperationAttempts"];
            timeout = 600;
            if (!int.TryParse(sql_timeout_s, out timeout))
                timeout = 600;
            int bulk_size = 10000;
            if (!int.TryParse(bulk_size_s, out bulk_size))
                bulk_size = 10000;
            int op_count = 3;
            if (!int.TryParse(op_attempts_s, out op_count))
                op_count = 3;
            Logger.WriteLine("Настройки в конфигурационном файле:");
            Logger.WriteLine(string.Format("Размер пакета: {0} таймаут: {1} число попыток: {2}", bulk_size, timeout, op_count));

            Logger.WriteLine("Пакетная запись данных...");
            FBSWriteDB_Bulk fbs_bw = new FBSWriteDB_Bulk(_tables);
            fbs_bw.CommandTimeout = timeout;
            fbs_bw.BulkSize = bulk_size;
            fbs_bw.DBOperationAttempts = op_count;
            bool writeSuccess = fbs_bw.WriteChanges(fbsParticipants, fbsCertificatesWithoutParticipant, fbsCertificateMarksWithoutParticipant, erbdParticipants, erbdCertificatesWithoutParticipant);
            Logger.WriteLine("Пакетная запись данных завершена");

            if (!writeSuccess)
            {
                Logger.WriteLine("При пакетной записи данных возникли ошибки, будет произведена пошаговая обработка");
                Logger.WriteLine("Пошаговая запись данных");
                new FBSWriteDB(_tables).WriteChanges(fbsParticipants, fbsCertificatesWithoutParticipant, fbsCertificateMarksWithoutParticipant, erbdParticipants, erbdCertificatesWithoutParticipant);
                Logger.WriteLine("Пошаговая запись данных завершена");
            }

            ShowStats(fbsParticipants, fbsCertificatesWithoutParticipant, fbsCertificateMarksWithoutParticipant, erbdParticipants, erbdCertificatesWithoutParticipant);

            Logger.WriteLine("Синхронизация данных за " + year.ToString() + " год завершена");

            return true;
        }

        private static void ShowStats(Dictionary<ParticipantId, FBSParticipant> fbsParticipants, Dictionary<CertificateId, FBSCertificate> fbsCertificatesWithoutParticipant, Dictionary<CertificateMarkId, FBSCertificateMark> fbsCertificateMarksWithoutParticipant, Dictionary<ParticipantId, ERBDParticipant> erbdParticipants, Dictionary<CertificateId, ERBDCertificate> erbdCertificatesWithoutParticipant)
        {
            int participantsDoneCount = 0;
            int participantsForProcessCount = 0;
            int participantsUndefinedCount = 0;
            int certificatesWithoutParticipantDoneCount = 0;
            int certificatesWithoutParticipantForProcessCount = 0;
            int certificatesWithoutParticipantUndefinedCount = 0;
            int certificateMarksWithoutParticipantDoneCount = 0;
            int certificateMarksWithoutParticipantForProcessCount = 0;
            int certificateMarksWithoutParticipantUndefinedCount = 0;

            foreach (FBSParticipant fbsParticipant in fbsParticipants.Values)
            {
                if (fbsParticipant.Action == ERBDToFBSActions.Done)
                {
                    participantsDoneCount++;
                }
                else if ((fbsParticipant.Action != ERBDToFBSActions.None) && (fbsParticipant.Action != ERBDToFBSActions.Undefined))
                {
                    participantsForProcessCount++;
                }
            }

            foreach (ERBDParticipant erbdParticipant in erbdParticipants.Values)
            {
                if (erbdParticipant.Action == ERBDToFBSActions.Done)
                {
                    participantsDoneCount++;
                }
                else if ((erbdParticipant.Action != ERBDToFBSActions.None) && (erbdParticipant.Action != ERBDToFBSActions.Undefined))
                {
                    participantsForProcessCount++;
                }
                else if (erbdParticipant.Action == ERBDToFBSActions.Undefined)
                {
                    participantsUndefinedCount++;
                }
            }

            foreach (FBSCertificate fbsCertificate in fbsCertificatesWithoutParticipant.Values)
            {
                if (fbsCertificate.Action == ERBDToFBSActions.Done)
                {
                    certificatesWithoutParticipantDoneCount++;
                }
                else if ((fbsCertificate.Action != ERBDToFBSActions.None) && (fbsCertificate.Action != ERBDToFBSActions.Undefined))
                {
                    certificatesWithoutParticipantForProcessCount++;
                }
            }

            foreach (ERBDCertificate erbdCertificate in erbdCertificatesWithoutParticipant.Values)
            {
                if (erbdCertificate.Action == ERBDToFBSActions.Done)
                {
                    certificatesWithoutParticipantDoneCount++;
                }
                else if ((erbdCertificate.Action != ERBDToFBSActions.None) && (erbdCertificate.Action != ERBDToFBSActions.Undefined))
                {
                    certificatesWithoutParticipantForProcessCount++;
                }
                else if (erbdCertificate.Action == ERBDToFBSActions.Undefined)
                {
                    certificatesWithoutParticipantUndefinedCount++;
                }
            }

            foreach (FBSCertificateMark fbsCertificateMark in fbsCertificateMarksWithoutParticipant.Values)
            {
                if (fbsCertificateMark.Action == ERBDToFBSActions.Done)
                {
                    certificateMarksWithoutParticipantDoneCount++;
                }
                else if ((fbsCertificateMark.Action != ERBDToFBSActions.None) && (fbsCertificateMark.Action != ERBDToFBSActions.Undefined))
                {
                    certificateMarksWithoutParticipantForProcessCount++;
                }
                else if (fbsCertificateMark.Action == ERBDToFBSActions.Undefined)
                {
                    certificateMarksWithoutParticipantUndefinedCount++;
                }
            }

            Logger.WriteLine("Участников ЕГЭ для обработки: " + participantsForProcessCount.ToString());
            Logger.WriteLine("Участников ЕГЭ неопределенных: " + participantsUndefinedCount.ToString());
            Logger.WriteLine("Участников ЕГЭ обработано: " + participantsDoneCount.ToString());
            Logger.WriteLine("Свидетельств ЕГЭ без связи с участником для обработки: " + certificatesWithoutParticipantForProcessCount.ToString());
            Logger.WriteLine("Свидетельств ЕГЭ без связи с участником неопределенных: " + certificatesWithoutParticipantUndefinedCount.ToString());
            Logger.WriteLine("Свидетельств ЕГЭ без связи с участником обработано: " + certificatesWithoutParticipantDoneCount.ToString());
            Logger.WriteLine("Баллов без связи с участником для обработки: " + certificateMarksWithoutParticipantForProcessCount.ToString());
            Logger.WriteLine("Баллов без связи с участником неопределенных: " + certificateMarksWithoutParticipantUndefinedCount.ToString());
            Logger.WriteLine("Баллов без связи с участником обработано: " + certificateMarksWithoutParticipantDoneCount.ToString());
        }

        private void ProcessError(string message, bool isCritical)
        {
            HasErrors = true;
            if (isCritical)
            {
                HasCriticalErrors = true;
            }
            Logger.WriteLine(message.ToUpper());
        }
    }
}
