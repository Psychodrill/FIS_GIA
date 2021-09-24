using System;
using System.Collections.Generic;
using System.Linq;
using Esrp.DB.EsrpADODB;

namespace Esrp.EIISIntegration.Import.Importers
{
    internal class OrganizationLimitationsImporter : ImporterBase<OrganizationLimitation>
    {
        public override string EIISObjectCode
        {
            get { return EIISObjectCodes.OrganizationLimitations; }
        }

        public override string Name
        {
            get { return "Запреты приема"; }
        }         

        Dictionary<string, int> organizationsCache_;
        protected override void SetDBObjectFields(OrganizationLimitation dbObject, EIISObject eIISObject, bool isNew)
        {
            EnsureRelationsCaches();

            string organizationEIISId = eIISObject.GetFieldStringValue(OrganizationIdField);
            dbObject.OrganizationId = organizationsCache_[organizationEIISId];

            dbObject.DocumentName = eIISObject.GetFieldStringValue(DocumentNameField);
            dbObject.DocumentNumber = eIISObject.GetFieldStringValue(DocumentNumberField);
            dbObject.DocumentDate = eIISObject.GetFieldDateTimeValue(DocumentDateField);
        }

        protected override bool ValidateObjectFieldValues(EIISObject eIISObject, out ErrorMessage message)
        {
            EnsureRelationsCaches();

            string organizationEIISId = eIISObject.GetFieldStringValue(OrganizationIdField);
            if ((String.IsNullOrEmpty(organizationEIISId)) || (!organizationsCache_.ContainsKey(organizationEIISId)))
            {
                message = new ErrorMessage(ErrorMessage.RelatedOrganizationNotFoundMessage, String.Format("связанный объект (организация) с идентификатором {0} не найден", organizationEIISId));
                return false;
            }

            return base.ValidateObjectFieldValues(eIISObject, out message);
        }

        protected void EnsureRelationsCaches()
        {
            if (organizationsCache_ == null)
            {
                organizationsCache_ = repository_.GetWithNotEmptyEiisId<Organization2010>()
                    .ToDictionary((x) => x.Eiis_Id, (x) => x.Id, StringComparer.OrdinalIgnoreCase);
            }
        }

        protected override bool SkipObject(EIISObject eIISObject, out bool retry, out ErrorMessage message)
        {
            string typeEIISId = eIISObject.GetFieldStringValue(DocumentTypeIdField);
            if ((typeEIISId != "98") && (typeEIISId != "100"))
            {
                message = new ErrorMessage(ErrorMessage.ObjectSkippedMessage, String.Format("документ об ограничении деятельности организации с типом {0} не подлежит импорту", typeEIISId));
                retry = false;
                return true;
            }

            return base.SkipObject(eIISObject, out retry, out   message);
        }

        protected override OrganizationLimitation GetExistingObject(EIISObject eIISObject)
        {
            return null;
        }

        protected override IEnumerable<string> RequiredFields
        {
            get { return new List<string>() { OrganizationIdField }; }
        }

        private const string OrganizationIdField = "SCHOOL_FK";
        private const string DocumentNameField = "DOCUMENT_NAME";
        private const string DocumentNumberField = "DOCUMENT_NUMBER";
        private const string DocumentDateField = "DOCUMENT_DATE";

        private const string DocumentTypeIdField = "DOCUMENT_TYPE_FK";
    }
}
