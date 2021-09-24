using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace Esrp.Core.ApplicationFCT
{
    public class ApplicationFCTDataAccessor
    {
        static string[] AppFields =
        {"OrganizationID", "FillingStage", "ScanOrderContentType", "ScanOrder", "PersonFullName", "PersonPosition", "PersonWorkPhone", "PersonMobPhone", "PersonEmail", "IsThereAttestatK1More",
                "NumARMs", "NumPDNs", "DictOperationSystemID", "IsIPStatic", "IPAddress", "IPMask4ARMs", "FISLogin", "DictAntivirusID", "DictUnauthAccessProtectID", 
                "DictElectronicLockID", "DictTNScreenID", "IP4TNS", "DictVipNetCryptoID"};

        static string[] AppFieldParams = {"@OrganizationID", "@FillingStage", "@ScanOrderContentType", "@ScanOrder", "@PersonFullName", "@PersonPosition", "@PersonWorkPhone", "@PersonMobPhone", "@PersonEmail", "@IsThereAttestatK1More",
                "@NumARMs", "@NumPDNs", "@DictOperationSystemID", "@IsIPStatic", "@IPAddress", "@IPMask4ARMs", "@FISLogin", "@DictAntivirusID", "@DictUnauthAccessProtectID", 
                "@DictElectronicLockID", "@DictTNScreenID", "@IP4TNS", "@DictVipNetCryptoID"};


        public static ApplicationFCT Get( int OrgID)
        {
            ApplicationFCT app = null;
            DataTable dt = new DataTable();

            using (var conn = new SqlConnection(DBSettings.ConnectionString))
            {
                var cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"SELECT ApplicationFCT.ID, FillingStage, ScanOrderContentType,ScanOrder,PersonFullName,PersonPosition  ,PersonWorkPhone ,PersonMobPhone,PersonEmail,
                                    IsThereAttestatK1More,NumARMs ,NumPDNs,DictOperationSystemID,IsIPStatic ,IPAddress,IPMask4ARMs,FISLogin,
                                    DictAntivirusID,DictUnauthAccessProtectID,DictElectronicLockID,DictTNScreenID,IP4TNS ,DictVipNetCryptoID,
                                    DictAntivirus.Name AS DictAntivirusName,  DictElectronicLock.Name as DictElectronicLockName,
                                    DictOperationSystem.Name as DictOperationSystemName,  DictTNScreen.Name as DictTNScreenName,
                                    DictUnauthAccessProtect.Name as DictUnauthAccessProtectName,   DictVipNetCrypto.Name as DictVipNetCryptoName
	                                FROM ApplicationFCT    
                                    LEFT OUTER JOIN    DictAntivirus on    DictAntivirusID = DictAntivirus.ID
                                    LEFT OUTER JOIN    DictElectronicLock on    DictElectronicLockID = DictElectronicLock.ID
                                    LEFT OUTER JOIN    DictOperationSystem on    DictOperationSystemID = DictOperationSystem.ID
                                    LEFT OUTER JOIN    DictTNScreen on    DictTNScreenID = DictTNScreen.ID
                                    LEFT OUTER JOIN    DictUnauthAccessProtect on    DictUnauthAccessProtectID = DictUnauthAccessProtect.ID
                                    LEFT OUTER JOIN    DictVipNetCrypto on    DictVipNetCryptoID = DictVipNetCrypto.ID
                                    where OrganizationID=" + OrgID.ToString();                
                conn.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                
                ad.Fill(dt);
                conn.Close();
            }

            if (dt.Rows.Count == 0)
                return null;
            
            app = new ApplicationFCT(OrgID, (int)dt.Rows[0]["ID"]);
            
            app.FillingStage = (int)dt.Rows[0]["FillingStage"];
            app.ScanOrderContentType = dt.Rows[0]["ScanOrderContentType"] as string;
            app.ScanOrder = dt.Rows[0]["ScanOrder"] as byte[];
            app.PersonFullName = dt.Rows[0]["PersonFullName"] as string;
            app.PersonPosition = dt.Rows[0]["PersonPosition"] as string;
            app.PersonWorkPhone = dt.Rows[0]["PersonWorkPhone"] as string;
            app.PersonMobPhone = dt.Rows[0]["PersonMobPhone"] as string;
            app.PersonEmail = dt.Rows[0]["PersonEmail"] as string;
            app.IsThereAttestatK1More = (bool)dt.Rows[0]["IsThereAttestatK1More"];
            app.NumARMs = (int)dt.Rows[0]["NumARMs"];
            app.NumPDNs = (int)dt.Rows[0]["NumPDNs"];
            app.DictOperationSystemID = dt.Rows[0]["DictOperationSystemID"] == DBNull.Value ? null : (int?)dt.Rows[0]["DictOperationSystemID"];
            app.IsIPStatic = dt.Rows[0]["IsIPStatic"] == DBNull.Value ? null : (bool?)dt.Rows[0]["IsIPStatic"];
            app.IPAddress = dt.Rows[0]["IPAddress"] as string;
            app.IPMask4ARMs = dt.Rows[0]["IPMask4ARMs"] as string;
            app.FISLogin = dt.Rows[0]["FISLogin"] as string;
            app.DictAntivirusID = dt.Rows[0]["DictAntivirusID"] == DBNull.Value ? null : (int?)dt.Rows[0]["DictAntivirusID"];
            app.DictUnauthAccessProtectID = dt.Rows[0]["DictUnauthAccessProtectID"] == DBNull.Value ? null : (int?)dt.Rows[0]["DictUnauthAccessProtectID"];
            app.DictElectronicLockID = dt.Rows[0]["DictElectronicLockID"] == DBNull.Value ? null : (int?)dt.Rows[0]["DictElectronicLockID"];
            app.DictTNScreenID = dt.Rows[0]["DictTNScreenID"] == DBNull.Value ? null : (int?)dt.Rows[0]["DictTNScreenID"];
            app.IP4TNS = dt.Rows[0]["IP4TNS"] as string;
            app.DictVipNetCryptoID = dt.Rows[0]["DictVipNetCryptoID"] == DBNull.Value ? null : (int?)dt.Rows[0]["DictVipNetCryptoID"];
            app.DictAntivirusName = dt.Rows[0]["DictAntivirusName"] as string;
            app.DictElectronicLockName = dt.Rows[0]["DictElectronicLockName"] as string;
            app.DictOperationSystemName = dt.Rows[0]["DictOperationSystemName"] as string;
            app.DictTNScreenName = dt.Rows[0]["DictTNScreenName"] as string;
            app.DictUnauthAccessProtectName = dt.Rows[0]["DictUnauthAccessProtectName"] as string;
            app.DictVipNetCryptoName = dt.Rows[0]["DictVipNetCryptoName"] as string;

            return app;

        }


        public static int GetFillingStage(int OrgID)
        {
            int nRes = 0;
            object Res;
            using (var conn = new SqlConnection(DBSettings.ConnectionString))
            {
                var cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select FillingStage from ApplicationFCT where OrganizationID=" + OrgID.ToString();
                conn.Open();
                Res = cmd.ExecuteScalar();
                if (Res != null)
                    nRes = (int)Res;
            }
            return nRes;             
        }

        static void SetCommandParams (SqlParameterCollection parameters, ApplicationFCT app)
        {
            parameters.AddWithValue("OrganizationID", app.OrganizationID);
            parameters.AddWithValue("FillingStage", app.FillingStage);
            if (app.ScanOrderContentType != null)
                parameters.AddWithValue("ScanOrderContentType", app.ScanOrderContentType);
            else
                parameters.AddWithValue("ScanOrderContentType", DBNull.Value);

            parameters.Add(new SqlParameter("@ScanOrder", SqlDbType.Image));
            if (app.ScanOrder != null)            
             parameters["@ScanOrder"].Value = app.ScanOrder;            
            else
             parameters["@ScanOrder"].Value = DBNull.Value;

            parameters.AddWithValue("PersonFullName", app.PersonFullName);
            parameters.AddWithValue("PersonPosition", app.PersonPosition);
            parameters.AddWithValue("PersonWorkPhone", app.PersonWorkPhone);
            parameters.AddWithValue("PersonMobPhone", app.PersonMobPhone);
            parameters.AddWithValue("PersonEmail", app.PersonEmail);
            parameters.AddWithValue("IsThereAttestatK1More", app.IsThereAttestatK1More);
            parameters.AddWithValue("NumARMs", app.NumARMs);
            parameters.AddWithValue("NumPDNs", app.NumPDNs);

            if (app.DictOperationSystemID != null)            
                parameters.AddWithValue("DictOperationSystemID", app.DictOperationSystemID);
            else  
                parameters.AddWithValue("DictOperationSystemID", DBNull.Value);

            if (app.IsIPStatic != null)
                parameters.AddWithValue("IsIPStatic", app.IsIPStatic);
            else
                parameters.AddWithValue("IsIPStatic", DBNull.Value);

            if (app.IPAddress != null)
                parameters.AddWithValue("IPAddress", app.IPAddress);
            else
                parameters.AddWithValue("IPAddress", DBNull.Value);

            if (app.IPMask4ARMs != null)
                parameters.AddWithValue("IPMask4ARMs", app.IPMask4ARMs);
            else
            {
                parameters.Add("IPMask4ARMs", SqlDbType.VarChar);
                parameters["IPMask4ARMs"].Value = DBNull.Value;
            }

            if (app.FISLogin != null)
                parameters.AddWithValue("FISLogin", app.FISLogin);
            else
                parameters.AddWithValue("FISLogin", DBNull.Value);

            if (app.DictAntivirusID != null)            
                parameters.AddWithValue("DictAntivirusID", app.DictAntivirusID);
            else
            {
                parameters.Add("DictAntivirusID", SqlDbType.Int);
                parameters["DictAntivirusID"].SqlValue = DBNull.Value;
            }

            if (app.DictUnauthAccessProtectID != null)            
                parameters.AddWithValue("DictUnauthAccessProtectID", app.DictUnauthAccessProtectID);
            else
            {
                parameters.Add("DictUnauthAccessProtectID", SqlDbType.Int);
                parameters["DictUnauthAccessProtectID"].SqlValue = DBNull.Value;
            }

            if (app.DictElectronicLockID != null)
                parameters.AddWithValue("DictElectronicLockID", app.DictElectronicLockID);
            else
            {
                parameters.Add("DictElectronicLockID", SqlDbType.Int);
                parameters["DictElectronicLockID"].SqlValue = DBNull.Value;
            }


            if (app.DictTNScreenID != null)            
                parameters.AddWithValue("DictTNScreenID", app.DictTNScreenID);
            else
            {
                parameters.Add("DictTNScreenID", SqlDbType.Int);
                parameters["DictTNScreenID"].SqlValue = DBNull.Value;
            }

            if (app.IP4TNS != null)
                parameters.AddWithValue("IP4TNS", app.IP4TNS);
            else
            {
                parameters.Add("IP4TNS", SqlDbType.VarChar);                
                parameters["IP4TNS"].SqlValue = DBNull.Value;
            }

            if (app.DictVipNetCryptoID != null)            
                parameters.AddWithValue("DictVipNetCryptoID", app.DictVipNetCryptoID);
            else
            {
                parameters.Add("DictVipNetCryptoID", SqlDbType.Int);
                parameters["DictVipNetCryptoID"].SqlValue = DBNull.Value;
            }
        }




        public static void InsertOrUpdate(ApplicationFCT appl)
        {

            string tail = "", names = "", values = "";

            using (var conn = new SqlConnection(DBSettings.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                SetCommandParams(cmd.Parameters, appl);

                // Новая
                if (appl.ID == 0)
                {
                    for (int i = 0; i < AppFields.Length; i++)
                    {
                        names += AppFields[i];
                        values += AppFieldParams[i];
                        if (i != AppFields.Length - 1)
                        {
                            names += ", ";
                            values += ", ";
                        }
                    }
                    cmd.CommandText = string.Format("INSERT INTO 	ApplicationFCT ({0}) VALUES  ({1})", names, values);
                }
                else
                {
                    for (int i = 1; i < AppFields.Length; i++)
                    {
                        tail += AppFields[i] + "=" + AppFieldParams[i];
                        if (i != AppFields.Length - 1)
                            tail += ", ";

                    }


                    cmd.CommandText = "UPDATE ApplicationFCT SET " + tail + " WHERE Id=@Id";
                    cmd.Parameters.AddWithValue("Id", appl.ID);
                }

                cmd.ExecuteNonQuery();

                if (appl.ID == 0)
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "SELECT @@Identity";
                    appl.ID = Convert.ToInt32(cmd.ExecuteScalar());
                }

                conn.Close();
            }
        }

    }
}
