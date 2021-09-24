using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Timers;
using System.Xml;
using System.Xml.Linq;
using Esrp.Integration.Common;
using Esrp.EIISIntegration;
using Esrp.SelfIntegration.ReplicationClient;

namespace ReplicationService
{
    public partial class EsrpReplicationService : ServiceBase
    {
        private static Logger _logger;
        private System.Timers.Timer _timer;
        private DateTime? _lastEiisImport;
        private DateTime? _lastEsrpReplication;
        private DateTime? _lastFisReplication;
        private int? _esrpReplicationMinutes;
        private int? _fisReplicationMinutes;
        private int? _eiisImportMinutes;
        private bool _esrpReplicationEnabled;
        private bool _fisReplicationEnabled;
        private bool _eiisImportEnabled;

        private IEnumerable<HourAndMinute> _eiisImportSchedule;
        private IEnumerable<HourAndMinute> _esrpReplicationSchedule;
        private IEnumerable<HourAndMinute> _fisReplicationSchedule;

        public EsrpReplicationService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _lastEiisImport = null;
            _lastEsrpReplication = null;
            _lastFisReplication = null;

            _logger = new Logger("Log_EsrpService");
            _logger.WriteLine("Служба ЕСРП запущена");

            int? esrpReplicationMinutes;
            IEnumerable<HourAndMinute> esrpReplicationSchedule;
            int? fisReplicationMinutes;
            IEnumerable<HourAndMinute> fisReplicationSchedule;
            int? eiisImportMinutes;
            IEnumerable<HourAndMinute> eiisImportSchedule;


            if (!CheckSettings(out esrpReplicationMinutes, out esrpReplicationSchedule, out fisReplicationMinutes, out fisReplicationSchedule, out eiisImportMinutes, out eiisImportSchedule))
            {
                _logger.WriteLine("Ошибка в настройках службы ЕСРП");
                throw new Exception("Ошибка в настройках службы ЕСРП");
            }

            _esrpReplicationEnabled = StringsHelper.IsTrueString(ConfigurationManager.AppSettings["ESRPReplicationEnabled"]);
            _fisReplicationEnabled = StringsHelper.IsTrueString(ConfigurationManager.AppSettings["FISReplicationEnabled"]);
            _eiisImportEnabled = StringsHelper.IsTrueString(ConfigurationManager.AppSettings["EIISImportEnabled"]);

            _esrpReplicationMinutes = esrpReplicationMinutes;
            _esrpReplicationSchedule = esrpReplicationSchedule;
            _fisReplicationMinutes = fisReplicationMinutes;
            _fisReplicationSchedule = fisReplicationSchedule;
            _eiisImportMinutes = eiisImportMinutes;
            _eiisImportSchedule = eiisImportSchedule;

            _timer = new System.Timers.Timer(60 * 1000);
            _timer.AutoReset = false;
            _timer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
            _timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                RunReplication();
            }
            catch (Exception ex)
            {
                _logger.WriteLine(String.Format("Критическая ошибка службы ЕСРП: {0} ({1})", ex.Message, ex.StackTrace));
            }
            finally
            {
                _timer.Start();
            }
        }

        protected override void OnStop()
        {
            _timer.Stop();
            _logger.WriteLine("Служба ЕСРП остановлена");
        }

        private bool CheckSettings(out int? esrpReplicationMinutes, out IEnumerable<HourAndMinute> esrpReplicationSchedule, out int? fisReplicationMinutes, out IEnumerable<HourAndMinute> fisReplicationSchedule, out int? eiisImportMinutes, out IEnumerable<HourAndMinute> eiisImportSchedule)
        {
            esrpReplicationMinutes = null;
            esrpReplicationSchedule = null;
            fisReplicationMinutes = null;
            fisReplicationSchedule = null;
            eiisImportMinutes = null;
            eiisImportSchedule = null;

            string esrpReplicationMinutesStr = ConfigurationManager.AppSettings["ESRPReplicationMinutes"];
            string esrpReplicationScheduleStr = ConfigurationManager.AppSettings["ESRPReplicationSchedule"];
            string fisReplicationMinutesStr = ConfigurationManager.AppSettings["FISReplicationMinutes"];
            string fisReplicationScheduleStr = ConfigurationManager.AppSettings["FISReplicationSchedule"];
            string eiisImportMinutesStr = ConfigurationManager.AppSettings["EIISImportMinutes"];
            string eiisImportScheduleStr = ConfigurationManager.AppSettings["EIISImportSchedule"];
            if (((String.IsNullOrEmpty(esrpReplicationMinutesStr)) && (String.IsNullOrEmpty(esrpReplicationMinutesStr)))
                || ((String.IsNullOrEmpty(fisReplicationMinutesStr)) && (String.IsNullOrEmpty(fisReplicationScheduleStr)))
                || ((String.IsNullOrEmpty(eiisImportMinutesStr)) && (String.IsNullOrEmpty(eiisImportScheduleStr))))
                return false;

            int esrpReplicationMinutesTemp = 0;
            int fisReplicationMinutesTemp = 0;
            int eiisImportMinutesTemp = 0;

            bool esrpReplicationMinutesParsed = Int32.TryParse(esrpReplicationMinutesStr, out esrpReplicationMinutesTemp);
            bool esrpReplicationScheduleParsed = TryParseSchedule(esrpReplicationScheduleStr, out esrpReplicationSchedule);
            bool fisReplicationMinutesParsed = Int32.TryParse(fisReplicationMinutesStr, out fisReplicationMinutesTemp);
            bool fisReplicationScheduleParsed = TryParseSchedule(fisReplicationScheduleStr, out fisReplicationSchedule);
            bool eiisImportMinutesParsed = Int32.TryParse(eiisImportMinutesStr, out eiisImportMinutesTemp);
            bool eiisImportScheduleParsed = TryParseSchedule(eiisImportScheduleStr, out eiisImportSchedule);

            if (((!esrpReplicationMinutesParsed) && (!esrpReplicationScheduleParsed))
                || ((!fisReplicationMinutesParsed) && (!fisReplicationScheduleParsed))
                || ((!eiisImportMinutesParsed) && (!eiisImportScheduleParsed)))
                return false;

            if (((esrpReplicationMinutesTemp <= 0) && (esrpReplicationSchedule == null))
                || ((fisReplicationMinutesTemp <= 0) && (fisReplicationSchedule == null))
                || ((eiisImportMinutesTemp <= 0) && (eiisImportSchedule == null)))
                return false;

            if (esrpReplicationMinutesTemp > 0)
            {
                esrpReplicationMinutes = esrpReplicationMinutesTemp;
            }
            if (fisReplicationMinutesTemp > 0)
            {
                fisReplicationMinutes = fisReplicationMinutesTemp;
            }
            if (eiisImportMinutesTemp > 0)
            {
                eiisImportMinutes = eiisImportMinutesTemp;
            }

            return true;
        }

        private void RunReplication()
        {
            Exception eiisException = null;
            Exception esrpException = null;
            Exception fisException = null;

            if ((_eiisImportEnabled)
                && (TimeToRun(_lastEiisImport, _eiisImportMinutes, _eiisImportSchedule)))
            {
                try
                {
                    EIISEntryPoint.Run(false);
                }
                catch (Exception ex)
                {
                    eiisException = ex;
                }
                finally
                {
                    _lastEiisImport = DateTime.Now;
                }
            }
            if ((_esrpReplicationEnabled)
                    && (TimeToRun(_lastEsrpReplication, _esrpReplicationMinutes, _esrpReplicationSchedule)))
            {
                try
                {
                    ESRPClientEntryPoint.Run(false);
                }
                catch (Exception ex)
                {
                    esrpException = ex;
                }
                finally
                {
                    _lastEsrpReplication = DateTime.Now;
                }
            }
            if ((_fisReplicationEnabled)
                    && (TimeToRun(_lastFisReplication, _fisReplicationMinutes, _fisReplicationSchedule)))
            {
                try
                {
                    FISClientEntryPoint.Run(false);
                }
                catch (Exception ex)
                {
                    fisException = ex;
                }
                finally
                {
                    _lastFisReplication = DateTime.Now;
                }
            }

            string exceptionMessage = null;
            if (eiisException != null)
            {
                exceptionMessage += String.Format("Критическая ошибка импорта из ЕИИС: {0}", eiisException.Message);
            }
            if (esrpException != null)
            {
                exceptionMessage += String.Format("Критическая ошибка репликации ЕСРП: {0}", esrpException.Message);
            }
            if (fisException != null)
            {
                exceptionMessage += String.Format("Критическая ошибка репликации ФИС: {0}", fisException.Message);
            }
            if (!String.IsNullOrEmpty(exceptionMessage))
            {
                throw new Exception(exceptionMessage);
            }
        }

        private bool TryParseSchedule(string str, out IEnumerable<HourAndMinute> schedule)
        {
            if (String.IsNullOrEmpty(str))
            {
                schedule = null;
                return false;
            }

            List<HourAndMinute> scheduleList = null;
            foreach (string hourAndMinuteStr in str.Split(';'))
            {
                HourAndMinute hourAndMinute;
                if (!HourAndMinute.TryParse(hourAndMinuteStr, out hourAndMinute))
                    continue;

                if (scheduleList == null)
                {
                    scheduleList = new List<HourAndMinute>();
                }
                scheduleList.Add(hourAndMinute);
            }
            if ((scheduleList == null) || (scheduleList.Count == 0))
            {
                schedule = null;
                return false;
            }
            else
            {
                schedule = scheduleList;
                return true;
            }
        }

        private bool TimeToRun(DateTime? lastRun, int? runIntervalMinutes, IEnumerable<HourAndMinute> schedule)
        {
            if (runIntervalMinutes.HasValue)
            {
                if ((!lastRun.HasValue) || (DateTime.Now.Subtract(lastRun.Value).TotalMinutes > runIntervalMinutes.Value))
                    return true;
            }
            if ((schedule != null) && (schedule.Any()))
            {
                if (schedule.Any(x => x.Hour == DateTime.Now.Hour && x.Minute == DateTime.Now.Minute))
                    return true;
            }

            return false;
        }

        private class HourAndMinute
        {
            public static bool TryParse(string str, out HourAndMinute parseResult)
            {
                if ((String.IsNullOrEmpty(str)) || (str.Split(':').Length != 2))
                {
                    parseResult = null;
                    return false;
                }

                string hourStr = str.Split(':')[0];
                string minuteStr = str.Split(':')[1];

                int hour;
                int minute;
                if ((!Int32.TryParse(hourStr, out hour)) || (!Int32.TryParse(minuteStr, out minute)))
                {
                    parseResult = null;
                    return false;
                }

                parseResult = new HourAndMinute() { Hour = hour, Minute = minute };
                return true;
            }

            private HourAndMinute()
            {
            }

            public int Hour { get; private set; }
            public int Minute { get; private set; }
        }
    }
}
