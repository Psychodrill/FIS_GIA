using System;
using System.Collections.Generic;
using System.Linq;
using Esrp.DB.EsrpADODB;
using Esrp.Integration.Common;

namespace Esrp.EIISIntegration.Import.Importers
{
    internal class LicenseSupplementsImporter : ImporterBase<LicenseSupplement>
    {
        public override string EIISObjectCode
        {
            get { return EIISObjectCodes.LicenseSupplements; }
        }

        public override string Name
        {
            get { return "Приложения к лицензиям"; }
        } 

        protected override void SetDBObjectFields(LicenseSupplement dbObject, EIISObject eIISObject, bool isNew)
        { 
            string organizationEIISId = eIISObject.GetFieldStringValue(OrganizationIdField);
            dbObject.OrganizationId = organizationsCache_[organizationEIISId];
            string licenseEIISId = eIISObject.GetFieldStringValue(LicenseIdField);
            dbObject.LicenseId = licensesCache_[licenseEIISId];

            dbObject.Number = eIISObject.GetFieldStringValue(NumberField);
            dbObject.FormNumber = eIISObject.GetFieldStringValue(FormNumberField);
            dbObject.FormSerialNumber = eIISObject.GetFieldStringValue(FormSerialNumberField);

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
                ExtendedEIISObject status = licenseSupplementStatuses_.FirstOrDefault(x => x.Eiis_Id == statusEiisId);
                if (status != null)
                {
                    dbObject.StatusName = status.GetFieldStringValue(StatusNameField);
                }
            }

            dbObject.EndDate = eIISObject.GetFieldDateTimeValue(EndDateField); 

            dbObject.OrderDocumentNumber = eIISObject.GetFieldStringValue(OrderDocumentNumberField);
            dbObject.OrderDocumentDate = eIISObject.GetFieldDateTimeValue(OrderDocumentDateField);
            dbObject.ReasonOfSuspension = eIISObject.GetFieldStringValue(ReasonOfSuspensionField);
            dbObject.DateOfSuspension = eIISObject.GetFieldDateTimeValue(DateOfSuspensionField);

            string oldSupplementEIISId = eIISObject.GetFieldStringValue(OldSupplementField);
            if ((!String.IsNullOrEmpty(oldSupplementEIISId)) && (licenseSupplementsCache_.ContainsKey(oldSupplementEIISId)))
            {
                dbObject.OldSupplementId = licenseSupplementsCache_[oldSupplementEIISId];
            }
        }

        private IEnumerable<ExtendedEIISObject> licenseDocumentTypes_;
        private IEnumerable<ExtendedEIISObject> licenseSupplementStatuses_;
        private Dictionary<string, int> licensesCache_;
        private Dictionary<string, int> organizationsCache_;
        private Dictionary<string, int> licenseSupplementsCache_;
        protected override void BeforeImportInternal()
        {
            MemoryImporter licenseDocumentTypesImporter = new MemoryImporter(EIISObjectCodes.LicenseDocumentTypes);
            licenseDocumentTypesImporter.Init(this.sessionId_, this.client_, this.connectionString_);
            licenseDocumentTypesImporter.OnMessage += new EventHandler<MessageEventArgs>(MemoryImporter_OnMessage);            
            licenseDocumentTypesImporter.ImportData();
            licenseDocumentTypes_ = licenseDocumentTypesImporter.Objects;

            MemoryImporter licenseSupplementStatusesImporter = new MemoryImporter(EIISObjectCodes.LicenseSupplementStatuses);
            licenseSupplementStatusesImporter.Init(this.sessionId_, this.client_, this.connectionString_);
            licenseSupplementStatusesImporter.OnMessage += new EventHandler<MessageEventArgs>(MemoryImporter_OnMessage);            
            licenseSupplementStatusesImporter.ImportData();
            licenseSupplementStatuses_ = licenseSupplementStatusesImporter.Objects;

            licensesCache_ = repository_.GetWithNotEmptyEiisId<License>()
                .ToDictionary((x) => x.Eiis_Id, (x) => x.Id, StringComparer.OrdinalIgnoreCase);

            organizationsCache_ = repository_.GetWithNotEmptyEiisId<Organization2010>()
                .ToDictionary((x) => x.Eiis_Id, (x) => x.Id, StringComparer.OrdinalIgnoreCase);
        }

        protected void MemoryImporter_OnMessage(object sender, MessageEventArgs e)
        {
            RaiseMessage(String.Format("(вспомогательный объект) {0}", e.Message));
        }

        protected override void BeforeImportStepInternal()
        {
            licenseSupplementsCache_ = repository_.GetWithNotEmptyEiisId<LicenseSupplement>()
                .ToDictionary((x) => x.Eiis_Id, (x) => x.Id, StringComparer.OrdinalIgnoreCase);
        }

        protected override bool ValidateObjectFieldValues(EIISObject eIISObject, out ErrorMessage message)
        {
            string licenseEIISId = eIISObject.GetFieldStringValue(LicenseIdField);
            if ((String.IsNullOrEmpty(licenseEIISId)) || (!licensesCache_.ContainsKey(licenseEIISId)))
            {
                message = new ErrorMessage(ErrorMessage.RelatedLicenseNotFoundMessage, String.Format("связанный объект (лицензия) с идентификатором {0} не найден", licenseEIISId));
                return false;
            }

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
            string oldSupplementEIISId = eIISObject.GetFieldStringValue(OldSupplementField);
            if ((!String.IsNullOrEmpty(oldSupplementEIISId)) && (!licenseSupplementsCache_.ContainsKey(oldSupplementEIISId)))
            {
                message = new ErrorMessage(ErrorMessage.RelatedOldLicenseSupplementNotFoundMessage, String.Format("связанный объект (переоформленное приложение к лицензии) с идентификатором {0} не найден", oldSupplementEIISId));
                retry = true;
                return true;
            } 

            return base.SkipObject(eIISObject, out retry, out   message);
        }

        protected override LicenseSupplement GetExistingObject(EIISObject eIISObject)
        {
            return null;
        }

        protected override IEnumerable<string> RequiredFields
        {
            get { return new List<string>() { LicenseIdField, OrganizationIdField }; }
        }

        private const string OrganizationIdField = "SCHOOL_FK";
        private const string LicenseIdField = "LICENSE_FK";

        private const string NumberField = "NUMBER";
        private const string FormSerialNumberField = "SER_DOC";
        private const string FormNumberField = "NUMBER_DOC";
        private const string EndDateField = "DATE_END";
        //private const string IsBranchField = "BRANCH";

        private const string OrderDocumentNumberField = "NUMLICDOC";
        private const string OrderDocumentDateField = "DATELICDOC";
        private const string OldSupplementField = "LICENSE_APP_OLD_FK";
        private const string ReasonOfSuspensionField = "REASON_OF_SUSPENSION";
        private const string DateOfSuspensionField = "DATE_OF_SUSPENSION";

        private const string DocumentTypeIdField = "BASEDOC_TYPE_FK";
        private const string DocumentTypeNameField = "NAME";
        private const string StatusIdField = "LICENSE_APP_STATUSE_FK";
        private const string StatusNameField = "NAME";
    }
}
