using System.Data;
using System.Data.SqlClient;

namespace Fbs.Web.Administration.Organizations
{
    public static class MainFunctions
    {
        public static DataTable CreateTable_FromSQLDataReader(SqlDataReader reader)
        {
            //Если тут возникает ошибка - разкомментировать блок ниже (старая версия), этот удалить
            DataTable Result = new DataTable();
            Result.Load(reader);
            return Result;

            /*
            DataTable table = new DataTable();
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
            }
            return table;
            */
        }
    }

}