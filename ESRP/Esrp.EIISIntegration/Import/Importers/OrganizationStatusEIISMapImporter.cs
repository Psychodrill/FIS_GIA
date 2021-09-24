using System.Collections.Generic;
using Esrp.DB.EsrpADODB;
using Esrp.EIISIntegration.Catalogs;

namespace Esrp.EIISIntegration.Import.Importers
{
    internal class OrganizationStatusEIISMapImporter : ImporterBase<OrganizationStatusEIISMap>
    {
        public override string EIISObjectCode
        {
            get { return EIISObjectCodes.OrganizationStatuses; }
        }

        public override string Name
        {
            get { return "Сопоставление статусов организации ЕСРП-ЕИИС"; }
        }

        protected override bool AllowDeleteObjects
        {
            get { return false; }
        }

        protected override bool AllowUpdateObjects
        {
            get { return false; }
        }

        protected override void SetDBObjectFields(OrganizationStatusEIISMap dbObject, EIISObject eIISObject, bool isNew)
        {
            dbObject.Name = eIISObject.GetFieldStringValue(NameField);
        }

        private GenericCatalog<OrganizationStatusEIISMap> cache_;
        protected override OrganizationStatusEIISMap GetExistingObject(EIISObject eIISObject)
        {
            if (cache_ == null)
            {
                cache_ = new GenericCatalog<OrganizationStatusEIISMap>(repository_.GetAll<OrganizationStatusEIISMap>(), true, true, null);
            }

            OrganizationStatusEIISMap result = null;

            string cacheKey = eIISObject.GetFieldStringValue(NameField);
            int? id = cache_.GetIdByNaturalKey(cacheKey);
            if (id.HasValue)
            {
                result = repository_.Get<OrganizationStatusEIISMap>(id.Value);
            }

            return result;
        }

        protected override IEnumerable<string> RequiredFields
        {
            get { return new List<string>() { NameField }; }
        }

        private const string NameField = "NAME";
    }
}
