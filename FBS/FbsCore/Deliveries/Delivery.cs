using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Fbs.Core.Deliveries
{
    /// <summary>
    /// Типы рассылок
    /// </summary>
    public enum DeliveryTypes
    {
        Unknown,Users, NotRegistredOrgs
    }

    /// <summary>
    /// Почтовая рассылка
    /// </summary>
    public class Delivery
    {
        long Id_;
        public long Id
        {
            get { return Id_; }
            internal  set { Id_ = value; }
        }

        string Title_;
        public string Title
        {
            get { return Title_; }
            set { Title_ = value; }
        }

        string Message_;
        public string Message
        {
            get { return Message_; }
            set { Message_ = value; }
        }

        DateTime?  DeliveryDate_;
        public DateTime? DeliveryDate
        {
            internal get { return DeliveryDate_; }
            set { DeliveryDate_ = value; }
        }

        List<int> RecipientCodes_ = new List<int>();
        public List<int> RecipientCodes
        {
            get { return RecipientCodes_; }
        }

        DeliveryTypes DeliveryType_;
        public DeliveryTypes DeliveryType
        {
            get { return DeliveryType_; }
            set { DeliveryType_ = value; }
        }

        /// <summary>
        /// Возвращает корректную строку даты
        /// </summary>
        public string DeliveryDateString
        {
            get
            {
                if (!DeliveryDate.HasValue)
                {
                    return "";
                }
                return DeliveryDate.Value.ToShortDateString();
            }
        }

        public Delivery()
        {
        }

        public Delivery(string title, string message,DeliveryTypes deliveryType, DateTime deliveryDate)
        {
            Title = title;
            Message = message;
            DeliveryDate = deliveryDate;
            DeliveryType = deliveryType;
        }

        internal Delivery(IDataReader reader)
        {
            Id = Convert.ToInt64(reader[DeliveryDataAccessor.TableColumns.Id]);
            Title = reader[DeliveryDataAccessor.TableColumns.Title ].ToString();
            Message = reader[DeliveryDataAccessor.TableColumns.Message].ToString();
            DeliveryType = DeliveryDataAccessor.TypeFromString(reader[DeliveryDataAccessor.TableColumns.DeliveryType].ToString());
            if (reader[DeliveryDataAccessor.TableColumns.DeliveryDate] != null)
            {
                DeliveryDate = Convert.ToDateTime(reader[DeliveryDataAccessor.TableColumns.DeliveryDate]);
            }
        }

        internal void FillRecipients(IDataReader reader)
        {
            while (reader.Read())
            {
                int Recipient;
                if (int.TryParse(reader[DeliveryDataAccessor.TableColumns.RecipientId].ToString(), out Recipient))
                {
                    RecipientCodes.Add(Recipient);
                }
            }
        }
    }
}
