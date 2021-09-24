using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Fbs.Web.Administration.Organizations
{
//    public class Org
//    {
//        public int OrgID;
//        public int OrgTypeID;
//        public int RegionId;
//        public string Name;
//        public string Address;
//        public string Department;
//        public string FounderName;
//        public string ChiefName;
//        public string CityCode;
//        public string Phone;
//        public string Email;

//        public Org(int orgId)
//        {
//            OrgID = orgId;
//        }


//        public static string GetOrgType(int orgTypeID)
//        {
//            string connectionString = ConfigurationManager.ConnectionStrings["Fbs.Core.Properties.Settings.FbsConnectionString"].ToString();
//            string rerurnVal = "[Не определено]";

//            using (SqlConnection connection = new SqlConnection(connectionString))
//            {
//                using (SqlCommand cmd = new SqlCommand("SELECT @Name=Name FROM dbo.EducationInstitutionType2009 WHERE ID=@OrgTypeID"))
//                {
//                    cmd.Connection = connection;
//                    cmd.Parameters.Add(new SqlParameter("Name", SqlDbType.VarChar, 255));
//                    cmd.Parameters["Name"].Direction = ParameterDirection.Output;
//                    cmd.Parameters.Add(new SqlParameter("OrgTypeID", SqlDbType.Int));
//                    cmd.Parameters["OrgTypeID"].Value = orgTypeID;
//                    connection.Open();

//                    cmd.ExecuteNonQuery();

//                    if (cmd.Parameters["Name"].Value != DBNull.Value)
//                        rerurnVal = cmd.Parameters["Name"].Value.ToString();
//                }
//            }
//            return rerurnVal;
//        }

//        public static Org Get(int id)
//        {
//            SqlCommand cmd =
//                new SqlCommand(@"
//                SELECT RegionId, OrgTypeID, 
//                    OrgName, FounderName,  director_name as ChiefName, Address_UR, Department, 
//                    city_code CityCode, Phone, Email 
//                FROM dbo.OrgEtalon_2009_V2
//                WHERE Id=@OrgID");

//            cmd.Parameters.Add(new SqlParameter("OrgID", SqlDbType.Int));
//            cmd.Parameters["OrgID"].Value = id;

//            string connectionString =
//                ConfigurationManager.ConnectionStrings["Fbs.Core.Properties.Settings.FbsConnectionString"].ToString();

//            Org newOrg = new Org(0);

//            using (SqlConnection connection = new SqlConnection(connectionString))
//            {
//                cmd.Connection = connection;
//                connection.Open();

//                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
//                {
//                    if (reader.Read())
//                    {
//                        newOrg.OrgID = id;
//                        newOrg.Name = reader["OrgName"].ToString();
//                        newOrg.FounderName = reader["FounderName"].ToString();
//                        newOrg.ChiefName = reader["ChiefName"].ToString();
//                        newOrg.Address = reader["Address_UR"].ToString();
//                        newOrg.Department = reader["Department"].ToString();

//                        newOrg.CityCode = reader["CityCode"].ToString();
//                        newOrg.Phone = reader["Phone"].ToString();
//                        newOrg.Email = reader["Email"].ToString();

//                        if (reader["RegionId"] != DBNull.Value)
//                            newOrg.RegionId = (int)reader["RegionId"];
//                        if (reader["OrgTypeID"] != DBNull.Value)
//                            newOrg.OrgTypeID = (int)reader["OrgTypeID"];
//                    }
//                }
//            }
//            return newOrg;
//        }


//        private static void CreateParams(Org org, SqlCommand cmd)
//        {
//            cmd.Parameters.Add(new SqlParameter("OrgID", SqlDbType.Int));
//            cmd.Parameters.Add(new SqlParameter("RegionId", SqlDbType.Int));
//            cmd.Parameters.Add(new SqlParameter("OrgTypeID", SqlDbType.Int));
//            cmd.Parameters.Add(new SqlParameter("OrgName", SqlDbType.VarChar, 255));
//            cmd.Parameters.Add(new SqlParameter("ChiefName", SqlDbType.VarChar, 255));
//            cmd.Parameters.Add(new SqlParameter("FounderName", SqlDbType.VarChar, 512));
//            cmd.Parameters.Add(new SqlParameter("Department", SqlDbType.VarChar, 512));
//            cmd.Parameters.Add(new SqlParameter("Address", SqlDbType.VarChar, 512));
//            cmd.Parameters.Add(new SqlParameter("CityCode", SqlDbType.VarChar, 10));
//            cmd.Parameters.Add(new SqlParameter("Phone", SqlDbType.VarChar, 255));
//            cmd.Parameters.Add(new SqlParameter("Email", SqlDbType.VarChar, 255));

//            cmd.Parameters["OrgID"].Value = org.OrgID;

//            if (org.RegionId == 0)
//                cmd.Parameters["RegionId"].Value = DBNull.Value;
//            else
//                cmd.Parameters["RegionId"].Value = org.RegionId;

//            if (org.OrgTypeID == 0)
//                cmd.Parameters["OrgTypeID"].Value = DBNull.Value;
//            else
//                cmd.Parameters["OrgTypeID"].Value = org.OrgTypeID;

//            cmd.Parameters["OrgName"].Value = org.Name;
//            cmd.Parameters["FounderName"].Value = org.FounderName;
//            cmd.Parameters["ChiefName"].Value = org.ChiefName;
//            cmd.Parameters["Department"].Value = org.Department;
//            cmd.Parameters["Address"].Value = org.Address;
//            cmd.Parameters["CityCode"].Value = org.CityCode;
//            cmd.Parameters["Phone"].Value = org.Phone;
//            cmd.Parameters["Email"].Value = org.Email;
//        }

//        public static void Create(Org org)
//        {
//            SqlCommand cmd =
//                new SqlCommand(@"INSERT INTO dbo.OrgEtalon_2009_V2 
//                (RegionId,OrgTypeID,OrgName,director_name,FounderName,
//                [Address_UR],Department,city_code,Phone,Email) 
//                VALUES(@RegionId,@OrgTypeID,@OrgName,@ChiefName, @FounderName,
//                @Address,@Department,@CityCode,@Phone,@Email);
//                SELECT @OrgID=SCOPE_IDENTITY()");

//            CreateParams(org, cmd);
//            cmd.Parameters["OrgID"].Direction = ParameterDirection.Output;

//            string connectionString = ConfigurationManager.ConnectionStrings["Fbs.Core.Properties.Settings.FbsConnectionString"].ToString();
//            using (SqlConnection connection = new SqlConnection(connectionString))
//            {
//                cmd.Connection = connection;
//                connection.Open();

//                cmd.ExecuteNonQuery();

//                org.OrgID = (int)cmd.Parameters["OrgID"].Value;
//            }
//        }

//        public static void Save(Org org)
//        {
//            SqlCommand cmd =
//                new SqlCommand(@"
//                UPDATE dbo.OrgEtalon_2009_V2 
//				SET RegionId=@RegionId, OrgTypeID=@OrgTypeID, 
//                    OrgName=@OrgName,   director_name=@ChiefName, 
//				    FounderName=@FounderName,[Address_UR]=@Address,
//				    Department=@Department, 
//                    city_code=@CityCode, Phone=@Phone, Email=@Email 
//                WHERE Id=@OrgID");

//            CreateParams(org, cmd);

//            string connectionString = ConfigurationManager.ConnectionStrings["Fbs.Core.Properties.Settings.FbsConnectionString"].ToString();
//            using (SqlConnection connection = new SqlConnection(connectionString))
//            {
//                cmd.Connection = connection;
//                connection.Open();

//                cmd.ExecuteNonQuery();
//            }
//        }
//    }
}