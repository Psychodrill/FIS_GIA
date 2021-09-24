using System.Collections.Generic;
using Esrp.DB.EsrpADODB;
using Esrp.EIISIntegration.Catalogs;

namespace Esrp.EIISIntegration.Import.Importers
{
    internal class FounderTypesImporter : ImporterBase<FounderType>
    {
        public override string EIISObjectCode
        {
            get { return EIISObjectCodes.FounderTypes; }
        }

        public override string Name
        {
            get { return "Типы учредителей"; }
        }

        protected override bool AllowDeleteObjects { get { return false; } }

        protected override void SetDBObjectFields(FounderType dbObject, EIISObject eIISObject, bool isNew)
        {
            dbObject.Name = eIISObject.GetFieldStringValue(NameField);
            dbObject.Code = eIISObject.GetFieldStringValue(CodeField);
            dbObject.IsDeleted = eIISObject.GetFieldBooleanValue(IsDeletedField).GetValueOrDefault();
        }

        private GenericCatalog<FounderType> cache_;
        protected override FounderType GetExistingObject(EIISObject eIISObject)
        {
            if (cache_ == null)
            {
                cache_ = new GenericCatalog<FounderType>(repository_.GetAll<FounderType>(), true, true, null);
            }

            FounderType result = null;

            string cacheKey = eIISObject.GetFieldStringValue(NameField);
            int? id = cache_.GetIdByNaturalKey(cacheKey);
            if (id.HasValue)
            {
                result = repository_.Get<FounderType>(id.Value);
            }

            return result;
        }

        protected override IEnumerable<string> RequiredFields
        {
            get { return new List<string>() { NameField }; }
        }

        private const string NameField = "NAME";
        private const string CodeField = "CODE";
        private const string IsDeletedField = "NOT_TRUE";
    }
}
