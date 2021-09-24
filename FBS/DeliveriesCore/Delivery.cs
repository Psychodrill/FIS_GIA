using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DeliveriesCore
{
    public class Delivery
    {
        #region Properties
        string DeliveryId_;
        public string DeliveryId
        {
            get { return DeliveryId_; }
            private set { DeliveryId_ = value; }
        }

        string Title_;
        public string Title
        {
            get { return Title_; }
            private set { Title_ = value; }
        }

        string Message_;
        public string Message
        {
            get { return Message_; }
            private set { Message_ = value; }
        }
        string DeliveryType_;
        public string DeliveryType
        {
            get { return DeliveryType_; }
            private set { DeliveryType_ = value; }
        }

        //List<string> RecipientCodes_ = new List<string >();
        //public List<string > RecipientCodes
        //{
        //    get { return RecipientCodes_; }
        //}
        #endregion

        public Delivery(IDataReader reader)
        {
            DeliveryId = reader["Id"].ToString();
            Title = reader["Title"].ToString();
            Message = reader["Message"].ToString();
            DeliveryType = reader["TypeCode"].ToString();
        }

        //public void FillRecipients(IDataReader reader)
        //{
        //    while (reader.Read())
        //    {
        //        RecipientCodes.Add(reader["RecipientCode"].ToString());
        //    }
        //}
    }
}
