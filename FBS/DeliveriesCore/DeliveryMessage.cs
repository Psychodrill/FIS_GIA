using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DeliveriesCore
{
   public  class DeliveryMessage
   {
       #region Properties
       Delivery OwnerDelivery_;
       Delivery OwnerDelivery
       {
           get
           {
               if (OwnerDelivery_ == null)
               {
                   throw new Exception("Отсутствуют общие данные о рассылке");
               }
               return OwnerDelivery_; 
           }
            set { OwnerDelivery_ = value; }
       }

       public string DeliveryId
       {
           get { return OwnerDelivery.DeliveryId; }
       }

       public string Title
       {
           get { return OwnerDelivery.Title; }
       }

       string ReciverEMail_;
       public string ReciverEMail
       {
           get { return ReciverEMail_; }
           private set { ReciverEMail_ = value; }
       }

       string ReciverFullName_;
       string ReciverFullName
       {
           get { return ReciverFullName_; }
            set { ReciverFullName_ = value; }
       }

       string ReciverOrgName_;
        string ReciverOrgName
       {
           get { return ReciverOrgName_; }
            set { ReciverOrgName_ = value; }
       }

       public string PersonalMessage
       {
           get
           {
               string OrgHeader="";
               if (!String.IsNullOrEmpty(ReciverOrgName))
               {
                   OrgHeader = "В " + ReciverOrgName + "<br /><br />";
               }
               string PersonHeader="";
               if (!String.IsNullOrEmpty(ReciverFullName))
               {
                   PersonHeader = "Уважаемый(ая) " + ReciverFullName + "!<br /><br />";
               }
               return OrgHeader + PersonHeader + OwnerDelivery.Message;
           }
       }
       #endregion

       public DeliveryMessage(IDataReader reader,Delivery ownerDelivery)
       {
           ReciverEMail = reader["EMail"].ToString().Replace(';', ',');
           ReciverOrgName = reader["OrgName"].ToString();
           CreateFullNameFromReader(reader);

           OwnerDelivery = ownerDelivery;
       }

       private void CreateFullNameFromReader(IDataReader reader)
       {
           if (!String.IsNullOrEmpty(reader["LastName"].ToString()))
           {
               ReciverFullName = reader["LastName"].ToString();
           }
           if (!String.IsNullOrEmpty(reader["FirstName"].ToString()))
           {
               ReciverFullName += " " + reader["FirstName"].ToString();
           }
           if (!String.IsNullOrEmpty(reader["PatronymicName"].ToString()))
           {
               ReciverFullName += " " + reader["PatronymicName"].ToString();
           }
       }
   }
}
