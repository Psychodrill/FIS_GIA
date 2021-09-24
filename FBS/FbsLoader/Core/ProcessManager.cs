using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using Core.Tables;

namespace Core
{
    public class ProcessManager
    {
        #region Properties

        private List<CustomTable> tables { get; set; }
        private Organizations2010 organizations { get; set; }
        private Certificates certificates { get; set; }
        private CheckLog2010 checkLog { get; set; }
        private RegistrationLog2010 registrationLog { get; set; }
        private CalculateOrgAggregates calculateOrgAggregates { get; set; }
        private CalculateCertAggregates calculateCertAggregates { get; set; }
        private int year { get; set; }

        #endregion

        public ProcessManager(int year)
        {
            Logger.Instance.WriteLog("Запуск процесса");
            tables = new List<CustomTable>();
            this.year = year;
            CreateTables();
        }

        private void CreateTables()
        {
            // Таблицы
            organizations = year == 2010 ? new Organizations2010(year) : new Organizations2011(year); tables.Add(organizations);
            certificates = new Certificates(year); tables.Add(certificates);
            checkLog = year == 2010 ? new CheckLog2010(year) : new CheckLog2011(year); tables.Add(checkLog);
            registrationLog = year == 2010 ? new RegistrationLog2010(year) : new RegistrationLog2011(year); tables.Add(registrationLog);
            // Процедуры
            calculateCertAggregates = new CalculateCertAggregates(); tables.Add(calculateCertAggregates);
            calculateOrgAggregates = new CalculateOrgAggregates(); tables.Add(calculateOrgAggregates);
        }

        public void Do()
        {
            switch (year)
            {
                case 2010:
                    Do2010();
                    break;
                case 2011:
                    Do2011();
                    break;
            }
        }

        private void Do2011()
        {
            using (SqlConnection sourceConnection = new SqlConnection(Config.Source2011ConnectionString()))
            using (SqlConnection esrpConnection = new SqlConnection(Config.EsrpConnectionString()))
            using (SqlConnection destConnection = new SqlConnection(Config.DestConnectionString()))
            {
                sourceConnection.Open();
                esrpConnection.Open();
                destConnection.Open();

                foreach (CustomTable table in tables)
                {
                    Logger.Instance.WriteLog("Обработка объекта " + table.TableName);

                    table.Init(sourceConnection, destConnection, esrpConnection);

                    Logger.Instance.WriteLog("Удаление. Старт", 1);
                    table.DoDelete();
                    Logger.Instance.WriteLog("Удаление. Выполнено", 1);

                    Logger.Instance.WriteLog("Загрузка. Старт", 1);
                    table.DoInsert();
                    Logger.Instance.WriteLog("Загрузка. Выполнено", 1);

                    Logger.Instance.WriteLog("Обновление. Старт", 1);
                    table.DoUpdate();
                    Logger.Instance.WriteLog("Обновление. Выполнено", 1);
                }
            }
        }

        private void Do2010()
        {
            using (SqlConnection sourceConnection = new SqlConnection(Config.Source2010ConnectionString()))
            using (SqlConnection destConnection = new SqlConnection(Config.DestConnectionString()))
            {
                sourceConnection.Open();
                destConnection.Open();

                foreach (CustomTable table in tables)
                {
                    Logger.Instance.WriteLog("Обработка объекта " + table.TableName);

                    table.Init(sourceConnection, destConnection, null);

                    Logger.Instance.WriteLog("Удаление. Старт", 1);
                    table.DoDelete();
                    Logger.Instance.WriteLog("Удаление. Выполнено", 1);

                    Logger.Instance.WriteLog("Загрузка. Старт", 1);
                    table.DoInsert();
                    Logger.Instance.WriteLog("Загрузка. Выполнено", 1);

                    Logger.Instance.WriteLog("Обновление. Старт", 1);
                    table.DoUpdate();
                    Logger.Instance.WriteLog("Обновление. Выполнено", 1);
                }
            }
        }
    }
}
