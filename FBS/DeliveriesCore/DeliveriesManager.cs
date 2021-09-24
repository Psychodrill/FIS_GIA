using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Net.Mail;
using System.Configuration;

namespace DeliveriesCore
{
    public class DeliveriesManager
    {
        public DeliveriesManager(string fromAddress, string SMTPHost_In, string SMTPPort_In, string login, string password, bool useSSL, string replyTo, string connectionString, bool debugMode)
        {
            if (String.IsNullOrEmpty(fromAddress) ||
                String.IsNullOrEmpty(SMTPHost_In) ||
                !int.TryParse(SMTPPort_In, out SMTPPort_) ||
                String.IsNullOrEmpty(connectionString))
                throw new Exception("Invalid settings");
            From_ = fromAddress;
            SMTPHost_ = SMTPHost_In;
            Login_ = login;
            Password_ = password;
            UseSSL_ = useSSL;
            ReplyTo_ = replyTo;
            ConnectionString_ = connectionString;
            DebugMode_ = debugMode;
        }

        #region Properties
        string From_;
        string From
        {
            get
            {
                return From_;
            }
        }
        string SMTPHost_;
        string SMTPHost
        {
            get
            {
                return SMTPHost_;
            }
        }
        int SMTPPort_;
        int SMTPPort
        {
            get
            {
                return SMTPPort_;
            }
        }
        string Login_;
        string Login
        {
            get
            {
                return Login_;
            }
        }
        string Password_;
        string Password
        {
            get
            {
                return Password_;
            }
        }
        bool UseSSL_;
        bool UseSSL
        {
            get { return UseSSL_; }
        }
        string ReplyTo_;
        string ReplyTo
        {
            get { return ReplyTo_; }
        }

        string ConnectionString_;
        string ConnectionString
        {
            get { return ConnectionString_; }
        }



        bool DebugMode_;
        bool DebugMode
        {
            get { return DebugMode_; }
        }
        #endregion

        /// <summary>
        /// Возвращает первую найденную рассылку со статусом "ждет отправки"
        /// </summary>
        /// <returns></returns>
        private Delivery GetDelivery()
        {
            Delivery Result = null;
            using (SqlConnection Conn = new SqlConnection(ConnectionString))
            {
                Conn.Open();
                SqlCommand Cmd = Conn.CreateCommand();
                Cmd.CommandText = @"SELECT TOP 1 Del.Id,Del.Title,Del.Message,Del.TypeCode
                                    FROM Delivery Del WHERE Del.Status=1 AND Day(Del.DeliveryDate)>=Day(@NowDate)";
                Cmd.Parameters.AddWithValue("NowDate", DateTime.Now);
                using (SqlDataReader Reader = Cmd.ExecuteReader())
                {
                    if (Reader.Read())
                    {
                        Result = new Delivery(Reader);
                    }
                }
                if (Result != null)
                {
                    //Cmd.Parameters.Clear();
                    //Cmd.CommandText = "SELECT * FROM DeliveryRecipients WHERE DeliveryId=@DeliveryId";
                    //Cmd.Parameters.AddWithValue("DeliveryId", Result.DeliveryId);



                    SetDeliveryStatus(Conn, Result.DeliveryId, DeliveryStatus.Sending);
                }
                Conn.Close();
            }
            return Result;
        }

        //private SqlCommand GetCommand(Delivery delivery)
        //{
        //  //  CommandSolutions.Map[
        //    return new SqlCommand();
        //    /*    Cmd.CommandText = @"SELECT 
        //                        Del.Id,Del.Title,Del.Message,Acc.EMail,Acc.LastName,Acc.FirstName,Acc.PatronymicName
        //                        FROM Delivery Del 
        //                        RIGHT JOIN DeliveryRecipients Rec ON Del.Id=Rec.DeliveryId 
        //                        RIGHT JOIN GroupAccount GrAcc ON GrAcc.GroupId=Rec.RecipientCode 
        //                        RIGHT JOIN Account Acc ON Acc.Id=GrAcc.AccountId 
        //                        WHERE Del.Id IN 
        //                        (SELECT TOP 1 Id FROM Delivery WHERE Status=1 AND Day(DeliveryDate)>=Day(@NowDate ))
        //                        ORDER BY Del.Id";
        //        Cmd.Parameters.AddWithValue("NowDate", DateTime.Now);*/
        //}

        private List<DeliveryMessage> GetDeliveryMessages(Delivery delivery)
        {
            List<DeliveryMessage> Messages = new List<DeliveryMessage>(50);

            SqlCommand Cmd = CommandSolutions.GetCommand(delivery.DeliveryId, delivery.DeliveryType);
            if (Cmd == null)
            {
                Logger.Write("Не удалось найти комманду [" + delivery.DeliveryType + "]");
                SetDeliveryStatus(delivery.DeliveryId, DeliveryStatus.Failed);
                return Messages;
            }
            using (SqlConnection Conn = new SqlConnection(ConnectionString))
            {
                Conn.Open();
                Cmd.Connection = Conn;
                using (SqlDataReader Reader = Cmd.ExecuteReader())
                {
                    while (Reader.Read())
                    {
                        DeliveryMessage Message = new DeliveryMessage(Reader, delivery);
                        Messages.Add(Message);
                    }
                }
            }
            return Messages;
        }

        private enum DeliveryStatus
        {
            None,
            NotSent, //Ожидает отправки
            Sending, //В процессе отправки
            Sent, //Отправлена
            Failed //Отправлена с ошибками
        }

        private void SetDeliveryStatus(string deliveryId, DeliveryStatus status)
        {
            using (SqlConnection Conn = new SqlConnection(ConnectionString))
            {
                Conn.Open();
                SetDeliveryStatus(Conn, deliveryId, status);
                Conn.Close();
            }
        }

        private void SetDeliveryStatus(SqlConnection conn, string deliveryId, DeliveryStatus status)
        {
            SqlCommand Cmd = conn.CreateCommand();
            Cmd.CommandText = "UPDATE Delivery SET Status=@Status WHERE Id=@Id AND Status<>@FailedStatus";//Не меняем статус ошибочных рассылок.
            Cmd.Parameters.AddWithValue("Id", deliveryId);
            Cmd.Parameters.AddWithValue("Status", (int)status);
            Cmd.Parameters.AddWithValue("FailedStatus", (int)DeliveryStatus.Failed);
            Cmd.ExecuteNonQuery();

            if (DebugMode)
            {
                Logger.Write("[" + status.ToString() + "] was set to delivery with id [" + deliveryId.ToString() + "]");
            }
        }

        public void SendDeliveryMessage(SmtpClient client, DeliveryMessage message)
        {
            SendDeliveryMessage(client, message, false);
        }

        public void SendDeliveryMessage(SmtpClient client, DeliveryMessage message, bool forReply)
        {
            string To = "";
            try
            {
                if (forReply)
                {
                    To = ReplyTo;
                }
                else
                {
                    To = message.ReciverEMail;
                }
                MailMessage MailToSend = new MailMessage(From, To, message.Title, message.PersonalMessage);
                MailToSend.IsBodyHtml = true;
                client.Send(MailToSend);
                Logger.WriteDeliveryLogEvent(ConnectionString, message.DeliveryId, To, "");

                if (DebugMode)
                {
                    Logger.Write("Message with title [" + message.Title + "] was sent to [" + To + "]");
                }
            }
            catch (Exception ex)
            {
                SetDeliveryStatus(message.DeliveryId, DeliveryStatus.Failed);

                string LogMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    LogMessage += "  " + ex.InnerException.Message;
                }
                Logger.WriteDeliveryLogEvent(ConnectionString, message.DeliveryId, To, LogMessage);
                Logger.Write("Can't send message to [" + To + "]:   " + LogMessage);
            }
        }

        public void SendOneDelivery()
        {
            Delivery DeliveryToSend = GetDelivery();
            if (DeliveryToSend == null)
                return;

            List<DeliveryMessage> Messages = GetDeliveryMessages(DeliveryToSend);


            if (Messages.Count == 0)
                return;

            if (DebugMode)
            {
                Logger.Write("Total messages [" + Messages.Count + "]");
            }

            SmtpClient Client = new SmtpClient(SMTPHost, SMTPPort);
            Client.EnableSsl = UseSSL;
            if (Login != "")
                Client.Credentials = new System.Net.NetworkCredential(Login, Password);


            SendDeliveryMessage(Client, Messages[0], true);
            foreach (DeliveryMessage MessageToSend in Messages)
            {
                SendDeliveryMessage(Client, MessageToSend);
            }
            string CurrentDeliveryId = Messages[0].DeliveryId;
            SetDeliveryStatus(CurrentDeliveryId, DeliveryStatus.Sent);
        }
    }
}
