using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GVUZ.Data.Model;
using Dapper;

namespace GVUZ.DAL.Dapper.Repository.Model
{
    public class OrderOfAdmissionRepository : GvuzRepository
    {
        public IEnumerable<OrderOfAdmission> GetAll()
        {
            return DbConnection(db =>
            {
                return db.Query<OrderOfAdmission>("select * from OrderOfAdmission");
            });
        }

        public IEnumerable<OrderOfAdmission> Find(int institutionId)
        {
            return DbConnection(db =>
            {
                return db.Query<OrderOfAdmission>(@"
select * from OrderOfAdmission
where InstitutionID = @institutionId"
                    , new { institutionId = institutionId });
            });
        }

        public IEnumerable<OrderOfAdmission> Find(int institutionId, int campaignId)
        {
            return DbConnection(db =>
            {
                return db.Query<OrderOfAdmission>(@"
select * from OrderOfAdmission
where InstitutionID = @institutionId
    and CampaignID = @campaignId "
                    , new { institutionId = institutionId, campaignId = campaignId });
            });
        }

        public IEnumerable<OrderOfAdmission> Find(int institutionId, string uid)
        {
            return DbConnection(db =>
            {
                return db.Query<OrderOfAdmission>(@"
select * from OrderOfAdmission
where InstitutionID = @institutionId
    and UID = @uid "
                    , new { institutionId = institutionId, uid = uid });
            });
        }

        public IEnumerable<OrderOfAdmission> Find(int institutionId, int campaignId, int eduLevelId, int eduFormId, int eduSourceId)
        {
            return DbConnection(db =>
            {
                return db.Query<OrderOfAdmission>(@"
select * from OrderOfAdmission
where InstitutionID = @institutionId
    and CampaignID = @campaignId 
    and EducationLevelID = @eduLevelId 
    and EducationFormID = @eduFormId
    and EducationSourceID = @eduSourceId"
                    , new { institutionId = institutionId, campaignId = campaignId, eduLevelId = eduLevelId, eduFormId = eduFormId, eduSourceId = eduSourceId });
            });
        }

        public OrderOfAdmission Get(int id)
        {
            return DbConnection(db =>
            {
                return db.Query<OrderOfAdmission>("select * from OrderOfAdmission where OrderID = @id", new { id = id }).FirstOrDefault();
            });
        }

        public int CountApplications(int orderId)
        {
            return DbConnection(db =>
            {
                return db.Query<int>("select count(*) from Application (NOLOCK) where OrderOfAdmissionID = @orderId", new { orderId = orderId }).FirstOrDefault();
            });
        }

        public void Insert(OrderOfAdmission order)
        {
            WithTransaction(tx =>
            {
                tx.Execute(@"
insert into OrderOfAdmission
           ([DateCreated]
           ,[DateEdited]
           ,[DatePublished]
           ,[InstitutionID]
           ,[UID]
           ,[CampaignID]
           ,[EducationLevelID]
           ,[EducationFormID]
           ,[EducationSourceID]insert
           ,[Stage]
           ,[IsForBeneficiary]
           ,[IsForeigner]
           ,[OrderName]
           ,[OrderNumber]
           ,[OrderDate]
           ,[OrderOfAdmissionStatusID]
           ,[OrderOfAdmissionTypeID])
     values
           (@DateCreated 
           ,@DateEdited 
           ,@DatePublished 
           ,@InstitutionID 
           ,@UID 
           ,@CampaignID 
           ,@EducationLevelID 
           ,@EducationFormID 
           ,@EducationSourceID 
           ,@Stage 
           ,@IsForBeneficiary 
           ,@IsForeigner 
           ,@OrderName 
           ,@OrderNumber 
           ,@OrderDate 
           ,@OrderOfAdmissionStatusID 
           ,@OrderOfAdmissionTypeID)", order);

                order.OrderID = tx.Query<int>("select @@identity").First();
            });
        }

        public void Update(OrderOfAdmission order)
        {
            WithTransaction(tx =>
            {
                tx.Execute(@"
update [OrderOfAdmission]
   set [DateCreated] = @DateCreated
      ,[DateEdited] = @DateEdited
      ,[DatePublished] = @DatePublished
      ,[InstitutionID] = @InstitutionID
      ,[UID] = @UID
      ,[CampaignID] = @CampaignID
      ,[EducationLevelID] = @EducationLevelID
      ,[EducationFormID] = @EducationFormID
      ,[EducationSourceID] = @EducationSourceID
      ,[Stage] = @Stage
      ,[IsForBeneficiary] = @IsForBeneficiary
      ,[IsForeigner] = @IsForeigner
      ,[OrderName] = @OrderName
      ,[OrderNumber] = @OrderNumber
      ,[OrderDate] = @OrderDate
      ,[OrderOfAdmissionStatusID] = @OrderOfAdmissionStatusID
      ,[OrderOfAdmissionTypeID] = @OrderOfAdmissionTypeID
 where OrderID = @OrderID", order);
            });
        }

        public void Delete(int id)
        {
            WithTransaction(tx =>
            {
                tx.Execute("delete from [OrderOfAdmission] where OrderID = @id", new { id = id });
            });
        }

        public void HistoryItem_Insert(OrderOfAdmissionHistory historyItem)
        {
            WithTransaction(tx =>
            {
                tx.Execute(@"
insert into [OrderOfAdmissionHistory]
           ([OrderID]
           ,[ApplicationID]
           ,[DatePublished]
           ,[CreatedDate]
           ,[ModifiedDate])
     values
           (@OrderID
           ,@ApplicationID
           ,@DatePublished
           ,@CreatedDate
           ,@ModifiedDate)", historyItem);

                historyItem.ID = tx.Query<int>("select @@identity").First();
            });
        }

        public IEnumerable<IdNameModel> OrderStatuses_GetAll()
        {
            return DbConnection(db =>
            {
                return db.Query<IdNameModel>("select OrderOfAdmissionStatusID as Id, Name from OrderOfAdmissionStatus");
            });
        }

        public IEnumerable<IdNameModel> OrderStatuses_GetAllNotDeleted()
        {
            return DbConnection(db =>
            {
                return db.Query<IdNameModel>("select OrderOfAdmissionStatusID as Id, Name from OrderOfAdmissionStatus where OrderOfAdmissionStatusID <> @deletedId", new { deletedId = OrderOfAdmissionStatus.Deleted });
            });
        }

        public IEnumerable<IdNameModel> OrderTypes_GetAll()
        {
            return DbConnection(db =>
            {
                return db.Query<IdNameModel>("select OrderOfAdmissionTypeID as Id, Name from OrderOfAdmissionType");
            });
        }

        public string OrderStatus_GetName(int statusId)
        {
            return DbConnection(db =>
            {
                return db.Query<string>("select * from OrderOfAdmissionStatus where OrderOfAdmissionStatusID = @id", new { id = statusId }).FirstOrDefault();
            });
        }

        public string OrderType_GetName(int typeId)
        {
            return DbConnection(db =>
            {
                return db.Query<string>("select * from OrderOfAdmissionType where OrderOfAdmissionTypeID = @id", new { id = typeId }).FirstOrDefault();
            });
        }
    }
}
