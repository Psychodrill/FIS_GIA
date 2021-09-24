using GVUZ.ImportService2016.Core.Dto.Import;
using System;
using System.Collections.Generic;
using GVUZ.ImportService2016.Core.Main.Extensions;

namespace GVUZ.ImportService2016.Core.Dto.DataReaders.OrderOfAdmission
{
   public class BulkApplicationInOrderReader : BulkReaderBase<PackageDataOrdersApplication>
    {
        public BulkApplicationInOrderReader(PackageData packageData, PackageDataOrdersApplication application)
        {
            _records = application != null ? new List<PackageDataOrdersApplication> { application } : packageData.OrderApplicationsToImport();

            AddGetter("Id", dto => dto.OrderID);
            AddGetter("ImportPackageId", dto => packageData.ImportPackageId);
            AddGetter("InstitutionID", dto => packageData.InstitutionId);

            AddGetter("GUID", dto => dto.GUID);
            AddGetter("UID", dto => dto.UID);

            AddGetter("ApplicationID", dto => dto.ID);  
            AddGetter("ApplicationLevelBudgetID", dto => dto.OrderIdLevelBudgetSpecified ? dto.OrderIdLevelBudget.To(0) : (object)DBNull.Value);
            AddGetter("ApplicationCGItemID", dto => dto.ApplicationCompetitiveGroupItemID);

            AddGetter("DirectionID", dto => DBNull.Value); 
            AddGetter("EducationFormID", dto => DBNull.Value);
            AddGetter("EducationLevelID", dto => DBNull.Value);
            AddGetter("FinanceSourceID", dto => DBNull.Value);

            AddGetter("IsBeneficiary", dto => false);
            AddGetter("IsForeigner", dto => false); 

            AddGetter("OrderDate", dto => dto.IsDisagreedDateSpecified ? dto.IsDisagreedDate : (object)DBNull.Value); // сюда передадим DisagreeDate
            AddGetter("OrderDatePublished", dto => DBNull.Value);
            AddGetter("OrderName", dto => DBNull.Value);
            AddGetter("OrderNumber", dto => DBNull.Value);

            AddGetter("Stage", dto => DBNull.Value);

            AddGetter("CampaignID", dto => dto.BenefitKindIDSpecified ? dto.BenefitKindID.To(0) : (object)DBNull.Value); // Сюда BenefitKindID, кстати, что с ним делать???
            AddGetter("Course", dto => DBNull.Value); 

            AddGetter("OrderStatus", dto => dto.OrderTypeID.To(0)); // Сюда передадим 0, чтобы не было ошибки

        }
    }
}
