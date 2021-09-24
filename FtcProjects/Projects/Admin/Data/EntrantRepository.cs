using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using Admin.DBContext;
using Admin.Models;
using Microsoft.SqlServer.Server;


namespace Admin.Data
{
    public class EntrantRepository
    {
        public static List<EntrantViewModel> GetEntrants (int institution_id, ApplicationContext db)
        {
            List<EntrantViewModel> list = new List<EntrantViewModel>();
            try
            {
                /*var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@InstitutionID", institution_id));
                parameters.Add(new SqlParameter("@Year", year));
                list = db.Database.ExecuteSqlCommand()
                */
                
                using (SqlConnection con = new SqlConnection(Startup.connString))
                {
                    string sql = "adm_GetEntrants";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@InstitutionID", SqlDbType.Int).Value = institution_id;
                    cmd.CommandTimeout = 2000;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        EntrantViewModel entrant = new EntrantViewModel();
                        entrant.InstitutionId = institution_id;
                        Entrant Entrant = new Entrant();
                        Entrant.EntrantId = (reader["EntrantId"] != DBNull.Value) ? (int)reader["EntrantId"] : 0;
                        Entrant.LastName = (reader["LastName"] != DBNull.Value) ? (string)reader["LastName"] : "";
                        Entrant.FirstName = (reader["FirstName"] != DBNull.Value) ? (string)reader["FirstName"] : "";
                        Entrant.MiddleName = (reader["MiddleName"] != DBNull.Value) ? (string)reader["MiddleName"] : "";
                        Entrant.CreatedDate = (reader["CreatedDate"] != null) ? (DateTime)reader["CreatedDate"] : DateTime.Now;
                        if (Entrant != null) { entrant.Entrant = Entrant; }
                        EntrantApplication Application = new EntrantApplication();
                        Application.ApplicationId = (reader["ApplicationID"] != DBNull.Value) ? (int)reader["ApplicationID"] : 0;
                        Application.ApplicationNumber = (reader["ApplicationNumber"] != DBNull.Value) ? (string)reader["ApplicationNumber"] : "";
                        if (Application != null) { entrant.Application = Application; }
                        if(entrant != null && entrant.Entrant.EntrantId != 0) { list.Add(entrant); }
                    }
               }
            }
            catch (Exception e)
            {
                string error = e.Message.ToString();
            }
            return list;
        }

        public static string EntrantRemove (int entrant_id)
        {
            string ret = "0";
            var tableSchema = new List<SqlMetaData>(1)
            {
                new SqlMetaData("Id", SqlDbType.Int)
            }.ToArray();

            var tableRow = new SqlDataRecord(tableSchema);
            tableRow.SetInt32(0, entrant_id);
            var entrantIdentifiersTable = new List<SqlDataRecord>(1){ tableRow };
        
            try
            {
                using (SqlConnection con = new SqlConnection(Startup.connString))
                {
                    string sql = "ftc_DeleteEntrantData";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ent_ids", SqlDbType.Structured) { Value = entrantIdentifiersTable });
                    cmd.Parameters.Add("@action", SqlDbType.Int).Value = 1;
                    cmd.CommandTimeout = 2000;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();  
                    ret = "1";
                }
            }
            catch (Exception e)
            {
                string error = e.InnerException.ToString();
            }
            return ret;
        }

    }
}
