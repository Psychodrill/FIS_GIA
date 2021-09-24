using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using GVUZ.DAL.Dapper;
using GVUZ.DAL.Dapper.Model.TargetOrganization;
using GVUZ.DAL.Dapper.ViewModel.Dictionary;
using SqlClient = System.Data.SqlClient;
using GVUZ.DAL.Dapper.Model.Campaigns;
using GVUZ.DAL.Dapper.ViewModel.Common;
using GVUZ.DAL.Dapper.Model.Benefit;
using GVUZ.DAL.Dapper.ViewModel.Olympic;
using Dapper;
using GVUZ.DAL.Dapper.Repository.Interfaces.CompetitiveGroups;
using GVUZ.DAL.Dapper.ViewModel.CompetitiveGroups;
using CGM = GVUZ.DAL.Dapper.Model.CompetitiveGroups;
using System.Globalization;
using System.Web.Mvc;




namespace GVUZ.ServiceModel.SQL {
	public static class SQL {

		private static string _connectionString;

		public static string ConnectionString {
			get {
				if(String.IsNullOrEmpty(_connectionString)) {
					ConnectionStringSettings css=ConfigurationManager.ConnectionStrings["Main"];
					_connectionString=css.ConnectionString;
				}
				return _connectionString;
			}
		}

        // Я начал это делать, но потом просто сделал индекс I_Application_ApplicationNumber и все залетало!
//        public static List<Tuple<int, int, string>> GetApplicationId(List<string> applicationNumbers)
//        {
//            var sql = @"
//SELECT a.ApplicationID, a.StatusID, a.ApplicationNumber 
//FROM [Application] AS a (NOLOCK) 
//WHERE a.ApplicationNumber in (SELECT name FROM @ApplicationNumbers);
//";
//            var result = new List<Tuple<int, int, string>>();

//            DataTable tbNames = new DataTable("Names");
//            tbNames.Columns.Add("name", typeof(string));
//            applicationNumbers.ForEach(x => tbNames.Rows.Add(x));

//            using (SqlConnection con = new SqlConnection(ConnectionString))
//            {
//                try
//                {
//                    SqlCommand com = new SqlCommand(sql, con);

//                    var EntranceTestsParam = new System.Data.SqlClient.SqlParameter("@ApplicationNumbers", SqlDbType.Structured);
//                    EntranceTestsParam.TypeName = "dbo.Names";
//                    EntranceTestsParam.Value = tbNames;

//                    //com.Parameters.Add(new SqlParameter("Names", applicationNumber));

//                    con.Open();
//                    SqlDataReader sdr = com.ExecuteReader();
//                    while (sdr.Read())
//                    {
//                        var res = new Tuple<int, int, string>(
//                                (int)sdr["ApplicationID"],
//                                (int)sdr["StatusID"],
//                                sdr["ApplicationNumber"].ToString()
//                            );

//                    }
//                    con.Close();
//                }
//                catch (SqlException)
//                {
//                    // throw e; // Пробросить дальше
//                }
//                catch (Exception)
//                {
//                    //throw e; // Пробросить дальше
//                }
//                finally
//                {
//                    if (con.State != ConnectionState.Closed)
//                    {
//                        con.Close();
//                    }
//                }
//                return result;
//            }
//        }

        public static int[] GetApplicationId(string applicationNumber)
        {
            int[] result = {0, 0};
            string sql = @"SELECT a.ApplicationID, a.StatusID FROM [Application] AS a (NOLOCK) WHERE a.ApplicationNumber=@ApplicationNumber";

            using(SqlConnection con = new SqlConnection(ConnectionString)) {
                try {
                    SqlCommand com = new SqlCommand(sql, con);
                    com.Parameters.Add(new SqlParameter("ApplicationNumber", applicationNumber));

                    con.Open();
                    SqlDataReader sdr = com.ExecuteReader();
                    if(sdr.Read()) {
                        result[0] = Convert.ToInt32(sdr["ApplicationID"] as Int32?);
                        result[1] = Convert.ToInt32(sdr["StatusID"] as Int32?);
                    }
                    con.Close();
                } catch(SqlException) {
                    // throw e; // Пробросить дальше
                } catch(Exception) {
                    //throw e; // Пробросить дальше
                } finally {
                    if(con.State != ConnectionState.Closed) {
                        con.Close();
                    }
                }
                return result;
            }
        }

        public static int[] GetIDFromUIDandInstId(string[] INValues, int InstitutionID, int count, string table , string FieldName)
        {
            int[] result = new int[count];
            string OneStr = "";
            int i = 0;
            foreach (var One in INValues)
            {
                OneStr += "'" + String.Join("|", One) + "'" + ",";
            }
            OneStr = OneStr.Remove(OneStr.Length - 1);

            string sql = $@"SELECT DISTINCT {FieldName} as Value FROM {table}
            WHERE UID IN ({OneStr}) and  InstitutionId = @InstitutionID";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                try
                {
                    SqlCommand com = new SqlCommand(sql, con);
                    con.Open();
                    com.Parameters.Add(new SqlParameter("InstitutionID", InstitutionID));
                        SqlDataReader sdr = com.ExecuteReader();
                        while (sdr.Read())
                        {
                            result[i] = Convert.ToInt32(sdr["Value"] as Int32?);
                            i++;
                        }
                    sdr.Close();
                }
                catch (SqlException e)
                {
                    throw e; // Пробросить дальше
                }
                catch (Exception ex)
                {
                    throw ex; // Пробросить дальше
                }
                finally
                {
                    if (con.State != ConnectionState.Closed)
                    {
                        con.Close();
                    }
                }
                return result;
            }
        }
    }
}

