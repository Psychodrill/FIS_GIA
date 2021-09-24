using System.Data;
using System.Data.SqlClient;

namespace FbsReportSender.Excel
{
    public static class MainFunctions
    {
        public static DataTable CreateTableFromSqlDataReader(SqlDataReader reader)
        {
            DataTable table = new DataTable();

            table.Load(reader);
            /*
            for (int i = 0; i < reader.FieldCount; i++)
            {
                table.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
            }

            while (reader.Read())
            {
                DataRow dr = table.NewRow();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    dr[i] = reader[i];
                }
                table.Rows.Add(dr);
            }*/
            return table;
        }
    }
}
