using System.Collections.Generic;
using Esrp.DB.EsrpADODB;
using Esrp.EIISIntegration.Catalogs;

namespace Esrp.EIISIntegration.Import.Importers
{
    internal class OrganizationKindEIISMapImporter : ImporterBase<OrganizationKindEIISMap>
    {
        public override string EIISObjectCode
        {
            get { return EIISObjectCodes.OrganizationKinds; }
        }

        public override string Name
        {
            get { return "Сопоставление видов организации ЕСПР-ЕИИС"; }
        }

        protected override bool AllowDeleteObjects
        {
            get { return false; }
        }

        protected override void SetDBObjectFields(OrganizationKindEIISMap dbObject, EIISObject eIISObject, bool isNew)
        {
            dbObject.Name = eIISObject.GetFieldStringValue(NameField);
            dbObject.Code = eIISObject.GetFieldStringValue(CodeField);
        }

        private GenericCatalog<OrganizationKindEIISMap> cache_;
        protected override OrganizationKindEIISMap GetExistingObject(EIISObject eIISObject)
        {
            if (cache_ == null)
            {
                cache_ = new GenericCatalog<OrganizationKindEIISMap>(repository_.GetAll<OrganizationKindEIISMap>(), true, true, null);
            }

            OrganizationKindEIISMap result = null;

            string cacheKey = eIISObject.GetFieldStringValue(NameField);
            int? id = cache_.GetIdByNaturalKey(cacheKey);
            if (id.HasValue)
            {
                result = repository_.Get<OrganizationKindEIISMap>(id.Value);
            }

            return result;
        }

        protected override IEnumerable<string> RequiredFields
        {
            get { return new List<string>() { NameField }; }
        }

        private const string NameField = "NAME";
        private const string CodeField = "CODE";
    }
}
