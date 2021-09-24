using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data;
using Microsoft.Extensions.Configuration;
using Admin.Models;
using System.Threading.Tasks;

namespace Admin.Data
{
    public class ApplicationRepository
    {
        public static List<EntrantViewModel> LoadApplications(string institutionID, string app_number)
        {
            int instID = (!String.IsNullOrEmpty(institutionID)) ? Convert.ToInt32(institutionID) : 0;
            string appNumber = (!String.IsNullOrEmpty(app_number)) ? app_number.Trim() : "";

            List<EntrantViewModel> list = new List<EntrantViewModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(Startup.connString))
                {
                    string sql = @"	SELECT a.ApplicationID, a.ApplicationNumber, a.InstitutionID, a.EntrantID, e.IdentityDocumentID, e.LastName, e.FirstName, e.MiddleName
	                            FROM  Application AS a
	                            INNER JOIN Entrant AS e ON a.EntrantID = e.EntrantID
	                            WHERE a.IsDisabled = 0 and a.InstitutionID = " + instID.ToString();
                    if (!String.IsNullOrEmpty(appNumber))
                    {
                        sql += " and a.ApplicationNumber = " + appNumber;
                    }
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        EntrantViewModel entrant = new EntrantViewModel();
                        entrant.InstitutionId = (reader["InstitutionID"] != DBNull.Value) ? (int)reader["InstitutionID"] : 0;

                        Entrant entr = new Entrant();
                        entr.EntrantId = (reader["EntrantID"] != DBNull.Value) ? (int)reader["EntrantID"] : 0;
                        entr.IdentityDocumentId = (reader["IdentityDocumentID"] != DBNull.Value) ? (int)reader["IdentityDocumentID"] : 0;
                        entr.LastName = (reader["LastName"] != DBNull.Value) ? (string)reader["LastName"] : "";
                        entr.FirstName = (reader["FirstName"] != DBNull.Value) ? (string)reader["FirstName"] : "";
                        entr.MiddleName = (reader["MiddleName"] != DBNull.Value) ? (string)reader["MiddleName"] : "";
                        if (entr != null) { entrant.Entrant = entr; }

                        EntrantApplication entrapp = new EntrantApplication();
                        entrapp.ApplicationId = (reader["ApplicationID"] != DBNull.Value) ? (int)reader["ApplicationID"] : 0;
                        entrapp.ApplicationNumber = (reader["ApplicationNumber"] != DBNull.Value) ? (string)reader["ApplicationNumber"] : "";
                        if (entrapp != null) { entrant.Application = entrapp; }

                        if (entrant != null) { list.Add(entrant); }
                    }

                }
            }
            catch (Exception e)
            {
                string error = e.InnerException.ToString();
            }
            
            return list;
        }

        public static void ApplicationRemove(int applicationID, int entrantID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Startup.connString))
                {
                    string sql = "ApplicationRemove";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@applicationID", SqlDbType.Int).Value = applicationID;
                    cmd.Parameters.Add("@entrantID", SqlDbType.Int).Value = entrantID;
                    cmd.CommandTimeout = 2000;
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                string error = e.InnerException.ToString();
            }
        }









    }
}
