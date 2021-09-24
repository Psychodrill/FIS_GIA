using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Web;

namespace Fbs.Core.Deliveries
{
    /// <summary>
    /// Класс доступа к почтовым рассылкам
    /// </summary>
    public static class DeliveryDataAccessor
    {
        internal sealed class TableColumns
        {
            public const string Id = "Id";
            public const string Title = "Title";
            public const string Message = "Message";
            public const string DeliveryDate = "DeliveryDate";
            public const string DeliveryType = "TypeCode";
            public const string RecipientId = "RecipientCode";
        }

        public static void Delete(long[] ids)
        {
            using (SqlConnection Conn = new SqlConnection(DBSettings.ConnectionString))
            {
                Conn.Open();

                SqlCommand Cmd = Conn.CreateCommand();
                Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                Cmd.CommandText = "DeleteDeliveries";

                SqlParameter IdParam = new SqlParameter("@ids", System.Data.SqlDbType.NVarChar);
                IdParam.Value = Fbs.Utility.Conversions.ArrayToString(ids);
                SqlParameter EditorLoginParam = new SqlParameter("@editorLogin", System.Data.SqlDbType.NVarChar);
                EditorLoginParam.Value = EditorLogin;
                SqlParameter EditorIpParam = new SqlParameter("@editorIp", System.Data.SqlDbType.NVarChar);
                EditorIpParam.Value = EditorIp;

                Cmd.Parameters.Add(IdParam);
                Cmd.Parameters.Add(EditorLoginParam);
                Cmd.Parameters.Add(EditorIpParam);

                Cmd.ExecuteNonQuery();

                Conn.Close();
            }
        }

        public static long UpdateOrCreate(Delivery delivery)
        {
            long Result = 0;
            using (SqlConnection Conn = new SqlConnection(DBSettings.ConnectionString))
            {
                Conn.Open();

                SqlCommand Cmd = Conn.CreateCommand();
                Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                Cmd.CommandText = "UpdateDelivery";

                SqlParameter IdParam = new SqlParameter("@id", System.Data.SqlDbType.BigInt);
                IdParam.Direction = System.Data.ParameterDirection.InputOutput;
                IdParam.Value = delivery.Id;

                SqlParameter TitleParam = new SqlParameter("@title", System.Data.SqlDbType.NVarChar);
                TitleParam.Value = delivery.Title;
                SqlParameter MessageParam = new SqlParameter("@message", System.Data.SqlDbType.NVarChar);
                MessageParam.Value = delivery.Message;
                SqlParameter DeliveryDateParam = new SqlParameter("@deliveryDate", System.Data.SqlDbType.DateTime);
                DeliveryDateParam.Value = delivery.DeliveryDate;
                SqlParameter DeliveryTypeParam = new SqlParameter("@deliveryType", System.Data.SqlDbType.NVarChar);
                DeliveryTypeParam.Value = delivery.DeliveryType.ToString();
                SqlParameter RecipientIdsParam = new SqlParameter("@recipientIds", System.Data.SqlDbType.NVarChar);
                RecipientIdsParam.Value = Fbs.Utility.Conversions.ListToString(delivery.RecipientCodes);
                SqlParameter EditorLoginParam = new SqlParameter("@editorLogin", System.Data.SqlDbType.NVarChar);
                EditorLoginParam.Value = EditorLogin;
                SqlParameter EditorIpParam = new SqlParameter("@editorIp", System.Data.SqlDbType.NVarChar);
                EditorIpParam.Value = EditorIp;

                Cmd.Parameters.Add(IdParam);
                Cmd.Parameters.Add(TitleParam);
                Cmd.Parameters.Add(MessageParam);
                Cmd.Parameters.Add(DeliveryDateParam);
                Cmd.Parameters.Add(DeliveryTypeParam);
                Cmd.Parameters.Add(RecipientIdsParam);
                Cmd.Parameters.Add(EditorLoginParam);
                Cmd.Parameters.Add(EditorIpParam);

                Cmd.ExecuteNonQuery();

                Conn.Close();
                Result = (long)IdParam.Value;
            }

            delivery.Id = Result; //Вдруг понадобится повторно обратиться к объекту
            return Result;
        }


        public static Delivery Get(long id)
        {
            Delivery Result = null;

            using (SqlConnection Conn = new SqlConnection(DBSettings.ConnectionString))
            {
                Conn.Open();

                SqlCommand Cmd = Conn.CreateCommand();
                Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                Cmd.CommandText = "GetDelivery";

                SqlParameter IdParam = new SqlParameter("@id", System.Data.SqlDbType.BigInt);
                IdParam.Value = id;

                Cmd.Parameters.Add(IdParam);

                using (SqlDataReader Reader = Cmd.ExecuteReader(System.Data.CommandBehavior.SingleRow))
                {
                    if (Reader.Read())
                    {
                        Result = new Delivery(Reader);
                    }
                    Reader.Close();
                }

                if (Result != null)
                {
                    Cmd.CommandText = "GetDeliveryRecipients";
                    using (SqlDataReader Reader = Cmd.ExecuteReader())
                    {
                        Result.FillRecipients(Reader);
                        Reader.Close();
                    }
                }

                Conn.Close();
            }

            return Result;
        }

        private static string EditorLogin
        {
            get
            {
                if (HttpContext.Current == null)
                    return null;
                return HttpContext.Current.User.Identity.Name;
            }
        }

        private static string EditorIp
        {
            get
            {
                if (HttpContext.Current == null)
                    return null;
                return HttpContext.Current.Request.UserHostAddress;
            }
        }

        public  static DeliveryTypes TypeFromString(string typeCode)
        {
            DeliveryTypes Result;
            try
            {
                Result = (DeliveryTypes)Enum.Parse(typeof(DeliveryTypes), typeCode);
            }
            catch
            {
                Result = DeliveryTypes.Unknown;
            }
            return Result;
        }
    }
}
