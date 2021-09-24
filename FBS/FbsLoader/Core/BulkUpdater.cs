using System.Collections.Generic;
using System.Data.SqlClient;

namespace Core
{
    public class BulkUpdater
    {
        public BulkUpdater()
        {
            FlushRowCount = Config.UpdateFlushRowsCount();
            Values = new List<List<SqlParameter>>();
        }

        public string CommandText { get; set; }
        private int FlushRowCount { get; set; }
        private List<List<SqlParameter>> Values { get; set; } // Коллекция значений для вставки в таблицу
        private SqlConnection DestConnection { get; set; }

        public void Init(SqlConnection destConnection)
        {
            DestConnection = destConnection;
        }

        public void Add(List<SqlParameter> parameters)
        {
            Values.Add(parameters);

            if (Values.Count >= FlushRowCount)
            {
                Flush();
            }
        }

        public void Flush()
        {
            SqlCommand cmd = DestConnection.CreateCommand();
            cmd.CommandTimeout = Config.CommandTimeout();
            int i = 0;
            foreach (List<SqlParameter> val in Values)
            {
                cmd.Parameters.Clear();
                cmd.CommandText = CommandText;

                foreach (SqlParameter prm in val)
                {
                    cmd.Parameters.Add(prm);
                }

                cmd.ExecuteNonQuery();
                i++;
            }
            Values.Clear();

            Logger.Instance.WriteLog("Обновление выполнено", 2);
        }
    }
}