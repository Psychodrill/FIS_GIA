using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace Esrp.Web.Profile
{
    public class ConfirmedDocumentHandler:IHttpHandler
    {
        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;

            string login = Request.QueryString["login"];
            if (string.IsNullOrEmpty(login))
            {
                //WriteMessage(Response, "Логин пользователя не задан (login)!");
                return;
            }

            //Создаем подключение к БД и получаем картинку по login-у пользователя
            string connectionString = ConfigurationManager.ConnectionStrings["Esrp.Core.Properties.Settings.EsrpConnectionString"].ToString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT RegistrationDocument, RegistrationDocumentContentType FROM dbo.Account WHERE [Login]=@Login", connection);
                cmd.Parameters.Add(new SqlParameter("Login", SqlDbType.VarChar,255));
                cmd.Parameters["Login"].Value = login;
                connection.Open();

                using(SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SingleResult))
                {
                    if (reader.Read())
                    {
                        if (reader[0]!=DBNull.Value)
                        {
                            if (reader.GetSqlBinary(0).Length > 0)
                            {
                                //Устанавливаем правильный ContentType
                                Response.ContentType = reader["RegistrationDocumentContentType"].ToString();
                                //И пишем содержимое картинки клиенту.
                                Response.OutputStream.Write(reader.GetSqlBinary(0).Value, 0,
                                                            reader.GetSqlBinary(0).Length);
                            }
                            //else WriteMessage(Response, string.Format("Cкан документа у пользователя с логином '{0}' пуст!", login));
                        }
                        //else WriteMessage(Response, string.Format("Cкан документа у пользователя с логином '{0}' пуст!", login));

                    }
                    //else WriteMessage(Response, string.Format("Пользователь и скан документа с логином '{0}' не найден!",login));
                }
            }
            Response.End();
        }

        /*public void WriteMessage(HttpResponse response, string message)
        {
            response.ContentType = "text/HTML";
            response.Output.Write(message);
            response.End();
        }*/

        #endregion
    }
}
