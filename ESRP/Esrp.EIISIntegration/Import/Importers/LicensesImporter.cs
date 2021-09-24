using System;
using System.Collections.Generic;
using System.Linq;
using Esrp.DB.EsrpADODB;
using Esrp.Integration.Common;

namespace Esrp.EIISIntegration.Import.Importers
{
    internal class LicensesImporter : ImporterBase<License>
    {
        public override string EIISObjectCode
        {
            get { return EIISObjectCodes.Licenses; }
        }

        public override string Name
        {
            get { return "Лицензии"; }
        } 

        protected override void SetDBObjectFields(License dbObject, EIISObject eIISObject, bool isNew)
        {  
            string organizationEIISId = eIISObject.GetFieldStringValue(OrganizationIdField);
            dbObject.OrganizationId = organizationsCache_[organizationEIISId];

            string regionEIISId = eIISObject.GetFieldStringValue(OrganizationIdField);
            if ((!String.IsNullOrEmpty(regionEIISId)) && (regionsCache_.ContainsKey(regionEIISId)))
            {
                dbObject.RegionId = regionsCache_[regionEIISId];
            }

            dbObject.RegNumber = eIISObject.GetFieldStringValue(RegNumberField);

            string documentTypeEiisId = eIISObject.GetFieldStringValue(DocumentTypeIdField);
            if (!String.IsNullOrEmpty(documentTypeEiisId))
            {
                ExtendedEIISObject documentType = licenseDocumentTypes_.FirstOrDefault(x => x.Eiis_Id == documentTypeEiisId);
                if (documentType != null)
                {
                    dbObject.BaseDocumentTypeName = documentType.GetFieldStringValue(DocumentTypeNameField);
                }
            }

            string statusEiisId = eIISObject.GetFieldStringValue(StatusIdField);
            if (!String.IsNullOrEmpty(statusEiisId))
            {
                ExtendedEIISObject status = licenseStatuses_.FirstOrDefault(x => x.Eiis_Id == statusEiisId);
                if (status != null)
                {
                    dbObject.StatusName = status.GetFieldStringValue(StatusNameField);
                }
            }

            dbObject.IsTermless = eIISObject.GetFieldBooleanValue(IsTermlessField).GetValueOrDefault();
            dbObject.EndDate = eIISObject.GetFieldDateTimeValue(EndDateField);

            dbObject.OrderDocumentNumber = eIISObject.GetFieldStringValue(OrderDocumentNumberField);
            dbObject.OrderDocumentDate = eIISObject.GetFieldDateTimeValue(OrderDocumentDateField);
            dbObject.ReasonOfSuspension = eIISObject.GetFieldStringValue(ReasonOfSuspensionField);
            dbObject.DateOfSuspension = eIISObject.GetFieldDateTimeValue(DateOfSuspensionField);
            dbObject.AdministrativeSuspensionOrder = eIISObject.GetFieldStringValue(AdministrativeSuspensionOrderField);
            dbObject.SuspensionDecision = eIISObject.GetFieldStringValue(SuspensionDecisionField);
            dbObject.CourtRevokingDecision = eIISObject.GetFieldStringValue(CourtRevokingDecisionField);

            string oldLicenseEIISId = eIISObject.GetFieldStringValue(OldLicenseField);
            if ((!String.IsNullOrEmpty(oldLicenseEIISId)) && (licensesCache_.ContainsKey(oldLicenseEIISId)))
            {
                dbObject.OldLicenseId = licensesCache_[oldLicenseEIISId];
            }
        }

        private IEnumerable<ExtendedEIISObject> licenseDocumentTypes_;
        private IEnumerable<ExtendedEIISObject> licenseStatuses_;
        private Dictionary<string, int> organizationsCache_;
        private Dictionary<string, int> regionsCache_;
        private Dictionary<string, int> licensesCache_;
        protected override void BeforeImportInternal()
        {
            MemoryImporter licenseDocumentTypesImporter = new MemoryImporter(EIISObjectCodes.LicenseDocumentTypes);
            licenseDocumentTypesImporter.Init(this.sessionId_, this.client_, this.connectionString_);
            licenseDocumentTypesImporter.OnMessage += new EventHandler<MessageEventArgs>(MemoryImporter_OnMessage);
            licenseDocumentTypesImporter.ImportData();
            licenseDocumentTypes_ = licenseDocumentTypesImporter.Objects;

            MemoryImporter licenseStatusesImporter = new MemoryImporter(EIISObjectCodes.LicenseStatuses);
            licenseStatusesImporter.Init(this.sessionId_, this.client_, this.connectionString_);
            licenseStatusesImporter.OnMessage += new EventHandler<MessageEventArgs>(MemoryImporter_OnMessage);            
            licenseStatusesImporter.ImportData();
            licenseStatuses_ = licenseStatusesImporter.Objects;

            organizationsCache_ = repository_.GetWithNotEmptyEiisId<Organization2010>()
                .ToDictionary((x) => x.Eiis_Id, (x) => x.Id, StringComparer.OrdinalIgnoreCase);

            regionsCache_ = repository_.GetWithNotEmptyEiisId<Region>()
                .ToDictionary((x) => x.Eiis_Id, (x) => x.Id, StringComparer.OrdinalIgnoreCase);
        }

        protected void MemoryImporter_OnMessage(object sender, MessageEventArgs e)
        {
            RaiseMessage(String.Format("(вспомогательный объект) {0}", e.Message));
        }

        protected override void BeforeImportStepInternal()
        {
            licensesCache_ = repository_.GetWithNotEmptyEiisId<License>()
                .ToDictionary((x) => x.Eiis_Id, (x) => x.Id, StringComparer.OrdinalIgnoreCase);
        }

        protected override bool ValidateObjectFieldValues(EIISObject eIISObject, out ErrorMessage message)
        {
            string organizationEIISId = eIISObject.GetFieldStringValue(OrganizationIdField);
            if ((String.IsNullOrEmpty(organizationEIISId)) || (!organizationsCache_.ContainsKey(organizationEIISId)))
            {
                message = new ErrorMessage(ErrorMessage.RelatedOrganizationNotFoundMessage, String.Format("связанный объект (организация) с идентификатором {0} не найден", organizationEIISId));
                return false;
            }

            return base.ValidateObjectFieldValues(eIISObject, out message);
        }

        protected override bool SkipObject(EIISObject eIISObject, out bool retry, out ErrorMessage message)
        {
            string oldLicenseEIISId = eIISObject.GetFieldStringValue(OldLicenseField);
            if ((!String.IsNullOrEmpty(oldLicenseEIISId)) && (!licensesCache_.ContainsKey(oldLicenseEIISId)))
            {
                message = new ErrorMessage(ErrorMessage.RelatedOldLicenseNotFoundMessage, String.Format("связанный объект (переоформленная лицензия) с идентификатором {0} не найден", oldLicenseEIISId));
                retry = true;
                return true;
            } 

            return base.SkipObject(eIISObject, out retry, out   message);
        }

        protected override License GetExistingObject(EIISObject eIISObject)
        {
            return null;
        }

        protected override IEnumerable<string> RequiredFields
        {
            get { return new List<string>() { OrganizationIdField }; }
        }

        private const string OrganizationIdField = "SCHOOL_FK"; 
        private const string RegNumberField = "LICENSE_REG_NUM";
        private const string IsTermlessField = "IS_TERMLESS";
        private const string EndDateField = "DATA_END";
        private const string OrderDocumentNumberField = "NUMLICDOC";
        private const string OrderDocumentDateField = "DATELICDOC";
        private const string OldLicenseField = "LICENSE_OLD_FK";
        private const string ReasonOfSuspensionField = "REASON_OF_SUSPENSION";
        private const string DateOfSuspensionField = "DATE_OF_SUSPENSION";
        private const string AdministrativeSuspensionOrderField = "ADM_SUSP_ORDERS";
        private const string SuspensionDecisionField = "ORG_SUSP_DECISIONS";
        private const string CourtRevokingDecisionField = "COURT_REVOKING_DECISIONS";

        private const string DocumentTypeIdField = "BASEDOC_TYPE_FK";
        private const string DocumentTypeNameField = "NAME";
        private const string StatusIdField = "LICENSE_STATUSE_FK";
        private const string StatusNameField = "NAME";
    }
}
