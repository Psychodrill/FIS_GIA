using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace Esrp.Web
{
    public partial class LoginBlocked : Page
    {
        Control GetHeader()
        {
            return Master.FindControl("cphHead");
        }

        Control GetMain()
        {
            return Master.FindControl("cphContent");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int delay = LoginAttemptsInfo.GetDelay(Request.UserHostAddress);
            if (delay>0)
            {
                GetMain().Controls.Add(new LiteralControl(string.Format(
                    "Вы превысили максимальное количество неверных вводов логина/пароля.<BR/><B>Ждите {0} сек.</B>", delay)));

                int refresh = delay%5;
                refresh = refresh > 0 ? refresh : 5;

                GetMain().Controls.Add(new LiteralControl(string.Format("<br/>(Страница обновится через {0} сек.)", refresh)));
                GetHeader().Controls.Add(new LiteralControl(string.Format("<meta http-equiv='refresh' content='{0}'>",refresh)));
            }
            else
            {
                GetMain().Controls.Add(new LiteralControl(string.Format(
                    "Можете снова попробовать  <a href='./login.aspx?ReturnUrl={0}'>войти в систему</a>.",
                    System.Web.HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]))));
            }
        }
    }

    public class LoginAttemptsInfo
    {
        private readonly static int MaxAttemptCount=Config.LoginTrace_MaxAttemptCount;
        private readonly static int TracedTimeInterval = Config.LoginTrace_TimeInterval;
        private readonly static int WaitTimout = Config.LoginTrace_WaitTimout;

        public LoginAttemptsInfo(string ipAddress)
        {
            IPAddress = ipAddress;
        }

        public readonly string IPAddress;
        public int Delay
        {
            get
            {
                int val = 0;
                if (!IsValid)
                {
                    val = (int)Math.Ceiling((LastLoginDate.AddSeconds(WaitTimout)-CheckedDate).TotalSeconds);
                }

                return val>0?val:0;
            }
        }

        private DateTime m_LastLoginDate;
        private DateTime m_CheckedDate;
        private int m_FailAttemptsCount = 0;
        
        public DateTime LastLoginDate
        {get { return m_LastLoginDate; }}
        public DateTime CheckedDate
        {get { return m_CheckedDate; }}
        public int FailAttemptsCount
        {get { return m_FailAttemptsCount; }}

        public bool IsValid
        {
            get
            {
                if (FailAttemptsCount >= MaxAttemptCount)
                    return false;
                return true;
            }
        }


        private static SqlCommand cmdCreate()
        {
            SqlCommand cmd = new SqlCommand("dbo.GetLoginAttemptsInfo");
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("IP",SqlDbType.VarChar,32));
            cmd.Parameters.Add(new SqlParameter("TimeInterval", SqlDbType.Int));
            return cmd;
        }

        public static int GetDelay(string ipAddress)
        {
            return Get(ipAddress).Delay;
        }

        public static LoginAttemptsInfo Get(string ipAddress)
        {
            LoginAttemptsInfo logAttempt=new LoginAttemptsInfo(ipAddress);

            SqlCommand cmd = cmdCreate();
            cmd.Parameters["IP"].Value = ipAddress;
            cmd.Parameters["TimeInterval"].Value = TracedTimeInterval;

            string connectionString = ConfigurationManager.ConnectionStrings["Esrp.Core.Properties.Settings.EsrpConnectionString"].ToString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                cmd.Connection = connection;
                connection.Open();

                 using (SqlDataReader reader=cmd.ExecuteReader(CommandBehavior.SingleRow))
                 {
                     if (reader.Read())
                     {
                         logAttempt.m_LastLoginDate = (DateTime)reader["LastLoginDate"];
                         logAttempt.m_CheckedDate = (DateTime)reader["CheckedDate"];
                         logAttempt.m_FailAttemptsCount = (int)reader["AttemptsFail"];
                     }
                 }
            }

            return logAttempt;
        }
    }

        
    
}
