using GVUZ.ImportService2016.Core.Dto.Import;
using GVUZ.Model.Institutions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using GVUZ.ImportService2016.Core.Main.Extensions;

namespace GVUZ.ImportService2016.Core.Dto.DataReaders
{
    public class BulkApplicationReader : BulkReaderBase<PackageDataApplication>
    {
        public BulkApplicationReader(PackageData packageData)
        {
            _records = packageData.ApplicationsToImport();

            AddGetter("ID", dto => dto.ID);
            AddGetter("GUID", dto => dto.GUID);
            AddGetter("InstitutionID", dto => packageData.InstitutionId);
            AddGetter("ImportPackageId", dto => packageData.ImportPackageId);

            AddGetter("UID", dto => dto.UID);
            AddGetter("EntrantUID", dto => dto.EntrantUID);

            AddGetter("ApplicationNumber", dto => dto.ApplicationNumber);
            AddGetter("RegistrationDate", dto => dto.RegistrationDate);
            AddGetter("NeedHostel", dto => dto.NeedHostel);
            AddGetter("StatusID", dto => dto.StatusID);
            AddGetter("StatusComment", dto => dto.StatusComment);

            // Не уверен, что эти поля вообще нужны. Уточнить!
            AddGetter("IsRequiresBudgetO", dto => dto.SelectedCompetitiveGroupsFull.Any(t => t.EducationSourceId == EDSourceConst.Budget && t.EducationFormId == EDFormsConst.O));
            AddGetter("IsRequiresBudgetOZ", dto => dto.SelectedCompetitiveGroupsFull.Any(t => t.EducationSourceId == EDSourceConst.Budget && t.EducationFormId == EDFormsConst.OZ));
            AddGetter("IsRequiresBudgetZ", dto => dto.SelectedCompetitiveGroupsFull.Any(t => t.EducationSourceId == EDSourceConst.Budget && t.EducationFormId == EDFormsConst.Z));
            AddGetter("IsRequiresPaidO", dto => dto.SelectedCompetitiveGroupsFull.Any(t => t.EducationSourceId == EDSourceConst.Paid && t.EducationFormId == EDFormsConst.O));
            AddGetter("IsRequiresPaidOZ", dto => dto.SelectedCompetitiveGroupsFull.Any(t => t.EducationSourceId == EDSourceConst.Paid && t.EducationFormId == EDFormsConst.OZ));
            AddGetter("IsRequiresPaidZ", dto => dto.SelectedCompetitiveGroupsFull.Any(t => t.EducationSourceId == EDSourceConst.Paid && t.EducationFormId == EDFormsConst.Z));
            AddGetter("IsRequiresTargetO", dto => dto.SelectedCompetitiveGroupsFull.Any(t => t.EducationSourceId == EDSourceConst.Target && t.EducationFormId == EDFormsConst.O));
            AddGetter("IsRequiresTargetOZ", dto => dto.SelectedCompetitiveGroupsFull.Any(t => t.EducationSourceId == EDSourceConst.Target && t.EducationFormId == EDFormsConst.OZ));
            AddGetter("IsRequiresTargetZ", dto => dto.SelectedCompetitiveGroupsFull.Any(t => t.EducationSourceId == EDSourceConst.Target && t.EducationFormId == EDFormsConst.Z));

            //дата возврата документов для отозванных заявлений(27.11.2017)
            AddGetter("ReturnDocumentsTypeId", dto => dto.ReturnDocumentsTypeId);
            AddGetter("ReturnDocumentsDate", dto => GetDateOrNull(dto.ReturnDocumentsDate));

            // OLD VERSION
            //AddGetter("ApplicationId", dto => (object)DBNull.Value); // 

            //AddGetter("EntrantId", dto => 9578); //(object)DBNull.Value); // TODO: сделать nullable или вообще выкинуть? не, не выкидывать, можно найти entrant'а заранее
            //AddGetter("RegistrationDate", dto => dto.RegistrationDate);
            //AddGetter("InstitutionID", dto => packageData.InstitutionId);
            //AddGetter("ApproveInstitutionCount", dto => true);
            //AddGetter("NeedHostel", dto => dto.NeedHostel);

            //AddGetter("FirstHigherEducation", dto => true);
            //AddGetter("ApprovePersonalData", dto => true);
            //AddGetter("FamiliarWithLicenseAndRules", dto => true);
            //AddGetter("FamiliarWithAdmissionType", dto => true);
            //AddGetter("FamiliarWithOriginalDocumentDeliveryDate", dto => true);

            //AddGetter("StatusID", dto => dto.StatusID);
            //AddGetter("WizardStepID", dto => 2);
            //AddGetter("ViolationID", dto => 0);

            //AddGetter("StatusDecision", dto => (object)DBNull.Value);
            //AddGetter("LastCheckDate", dto => (object)DBNull.Value);
            //AddGetter("ViolationErrors", dto => (object)DBNull.Value);
            //AddGetter("PublishDate", dto => (object)DBNull.Value);

            //AddGetter("SourceID", dto => 2);
            //AddGetter("ApplicationNumber", dto => dto.ApplicationNumber);

            //AddGetter("OriginalDocumentsReceived", dto => false);

            //AddGetter("OrderCompetitiveGroupID", dto => (object)DBNull.Value);
            //AddGetter("OrderOfAdmissionID", dto => (object)DBNull.Value);
            //AddGetter("OrderCompetitiveGroupItemID", dto => (object)DBNull.Value);
            //AddGetter("OrderCalculatedRating", dto => (object)DBNull.Value);
            //AddGetter("OrderCalculatedBenefitID", dto => (object)DBNull.Value);
            //AddGetter("OrderEducationFormID", dto => (object)DBNull.Value);
            //AddGetter("OrderEducationSourceID", dto => (object)DBNull.Value);

            //AddGetter("LastDenyDate", dto => (object)DBNull.Value);

            //AddGetter("UID", dto => dto.UID);

            //AddGetter("IsRequiresBudgetO", dto => dto.SelectedCompetitiveGroupsFull.Any(t => t.EducationSourceId == EDSourceConst.Budget && t.EducationFormId == EDFormsConst.O));
            //AddGetter("IsRequiresBudgetOZ", dto => dto.SelectedCompetitiveGroupsFull.Any(t => t.EducationSourceId == EDSourceConst.Budget && t.EducationFormId == EDFormsConst.OZ));
            //AddGetter("IsRequiresBudgetZ", dto => dto.SelectedCompetitiveGroupsFull.Any(t => t.EducationSourceId == EDSourceConst.Budget && t.EducationFormId == EDFormsConst.Z));
            //AddGetter("IsRequiresPaidO", dto => dto.SelectedCompetitiveGroupsFull.Any(t => t.EducationSourceId == EDSourceConst.Paid && t.EducationFormId == EDFormsConst.O));
            //AddGetter("IsRequiresPaidOZ", dto => dto.SelectedCompetitiveGroupsFull.Any(t => t.EducationSourceId == EDSourceConst.Paid && t.EducationFormId == EDFormsConst.OZ));
            //AddGetter("IsRequiresPaidZ", dto => dto.SelectedCompetitiveGroupsFull.Any(t => t.EducationSourceId == EDSourceConst.Paid && t.EducationFormId == EDFormsConst.Z));

            //AddGetter("CreatedDate", dto => DateTime.Now);
            //AddGetter("ModifiedDate", dto => DateTime.Now);

            //AddGetter("OriginalDocumentsReceivedDate", dto => (object)DBNull.Value);
            //AddGetter("LastEgeDocumentsCheckDate", dto => (object)DBNull.Value);
            //AddGetter("OrderCompetitiveGroupTargetID", dto => (object)DBNull.Value);

            //AddGetter("IsRequiresTargetO", dto => dto.SelectedCompetitiveGroupsFull.Any(t => t.EducationSourceId == EDSourceConst.Target && t.EducationFormId == EDFormsConst.O));
            //AddGetter("IsRequiresTargetOZ", dto => dto.SelectedCompetitiveGroupsFull.Any(t => t.EducationSourceId == EDSourceConst.Target && t.EducationFormId == EDFormsConst.OZ));
            //AddGetter("IsRequiresTargetZ", dto => dto.SelectedCompetitiveGroupsFull.Any(t => t.EducationSourceId == EDSourceConst.Target && t.EducationFormId == EDFormsConst.Z));


            //AddGetter("ApplicationGUID", dto => dto.GUID);

            //AddGetter("Priority", dto => (object)DBNull.Value);

            //AddGetter("EntrantUID", dto => dto.EntrantUID);
            //AddGetter("ImportPackageID", dto => packageData.ImportPackageId);
        }
    }
}
