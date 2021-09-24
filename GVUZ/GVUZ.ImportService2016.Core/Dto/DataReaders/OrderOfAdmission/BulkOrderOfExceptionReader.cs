using GVUZ.ImportService2016.Core.Dto.Import;
using System;

namespace GVUZ.ImportService2016.Core.Dto.DataReaders.OrderOfAdmission
{
    public class BulkOrderOfExceptionReader : BulkReaderBase<PackageDataOrdersOrderOfException>
    {
        public BulkOrderOfExceptionReader(PackageData packageData)
        {
            _records = packageData.OrderOfExceptionToImport();

            AddGetter("Id", dto => dto.ID);
            AddGetter("ImportPackageId", dto => packageData.ImportPackageId);
            AddGetter("InstitutionID", dto => packageData.InstitutionId);

            AddGetter("GUID", dto => dto.GUID);
            AddGetter("UID", dto => dto.UID);

            AddGetter("ApplicationID", dto => DBNull.Value);  // dto => dto.Application != null ? dto.Application.ApplicationID : (object)DBNull.Value);
            AddGetter("ApplicationLevelBudgetID", dto => DBNull.Value);  //dto => dto.Application != null && dto.Application.OrderIdLevelBudget != 0 ? dto.Application.OrderIdLevelBudget : (object)DBNull.Value);
            AddGetter("ApplicationCGItemID", dto => DBNull.Value); // dto.ApplicationCompetitiveGroupItemID);

            AddGetter("DirectionID", dto => DBNull.Value); // dto => dto.DirectionIDSpecified ? dto.DirectionID : (object)DBNull.Value);
            AddGetter("EducationFormID", dto => dto.EducationFormIDSpecified ? dto.EducationFormID : (object)DBNull.Value);
            AddGetter("EducationLevelID", dto => dto.EducationLevelIDSpecified ? dto.EducationLevelID : (object)DBNull.Value);
            AddGetter("FinanceSourceID", dto => dto.FinanceSourceIDSpecified ? dto.FinanceSourceID : (object)DBNull.Value);

            AddGetter("IsBeneficiary", dto => false); // dto.IsBeneficiarySpecified ? dto.IsBeneficiary : false);
            AddGetter("IsForeigner", dto => false); // dto.IsForeignerSpecified ? dto.IsForeigner : false);

            AddGetter("OrderDate", dto => dto.OrderDateSpecified ? dto.OrderDate : (object)DBNull.Value);
            AddGetter("OrderDatePublished", dto => dto.OrderDatePublishedSpecified ? dto.OrderDatePublished : (object)DBNull.Value);
            AddGetter("OrderName", dto => GetStringOrNull(dto.OrderName));
            AddGetter("OrderNumber", dto => GetStringOrNull(dto.OrderNumber));

            AddGetter("Stage", dto => dto.StageSpecified ? dto.Stage : (object)DBNull.Value);

            AddGetter("CampaignID", dto => dto.CampaignID);
            AddGetter("Course", dto => 2); // OrderOfAdmissionType

            AddGetter("OrderStatus", dto => dto.OrderStatus);

        }
    }
}
