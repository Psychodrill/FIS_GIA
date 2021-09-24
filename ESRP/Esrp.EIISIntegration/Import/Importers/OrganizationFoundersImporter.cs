using System;
using System.Collections.Generic;
using System.Linq;
using Esrp.DB.EsrpADODB;

namespace Esrp.EIISIntegration.Import.Importers
{
    internal class OrganizationFoundersImporter : ImporterBase<OrganizationFounder>
    {
        public override string EIISObjectCode
        {
            get { return EIISObjectCodes.FoundersToOrganizationsLink; }
        }

        public override string Name
        {
            get { return "Учредители ОО"; }
        }

        protected override void SetDBObjectFields(OrganizationFounder dbObject, EIISObject eIISObject, bool isNew)
        {
            dbObject.FounderId = foundersCache_[eIISObject.GetFieldStringValue(FounderIdField)];
            dbObject.OrganizationId = organizationsCache_[eIISObject.GetFieldStringValue(OrganizationIdField)];
        }

        private Dictionary<string, int> organizationsCache_;
        private Dictionary<string, int> foundersCache_;
        protected override void BeforeImportInternal()
        {
            organizationsCache_ = repository_.GetWithNotEmptyEiisId<Organization2010>()
                .ToDictionary((x) => x.Eiis_Id, (x) => x.Id, StringComparer.OrdinalIgnoreCase);
            foundersCache_ = repository_.GetWithNotEmptyEiisId<Founder>()
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
            string founderEIISId = eIISObject.GetFieldStringValue(FounderIdField);
            if ((String.IsNullOrEmpty(founderEIISId)) || (!foundersCache_.ContainsKey(founderEIISId)))
            {
                message = new ErrorMessage(ErrorMessage.RelatedFounderNotFoundMessage, String.Format("связанный объект (учредитель) с идентификатором {0} не найден", founderEIISId));
                return false;
            }

            return base.ValidateObjectFieldValues(eIISObject, out message);
        }

        protected override OrganizationFounder GetExistingObject(EIISObject eIISObject)
        {
            return null;
        }

        protected override IEnumerable<string> RequiredFields
        {
            get { return new List<string>() { OrganizationIdField, FounderIdField }; }
        }

        private const string OrganizationIdField = "SCHOOL_FK";
        private const string FounderIdField = "FOUNDER_FK"; 
    }
}
