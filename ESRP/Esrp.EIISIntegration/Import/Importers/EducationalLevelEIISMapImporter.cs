using System.Collections.Generic;
using Esrp.DB.EsrpADODB;
using Esrp.EIISIntegration.Catalogs;

namespace Esrp.EIISIntegration.Import.Importers
{
    internal class EducationalLevelEIISMapImporter : ImporterBase<EducationalLevelEIISMap>
    {
        public override string EIISObjectCode
        {
            get { return EIISObjectCodes.EducationalLevels; }
        }

        public override string Name
        {
            get { return "Сопоставление уровней образования ЕСПР-ЕИИС"; }
        }

        protected override bool AllowDeleteObjects
        {
            get { return false; }
        }

        protected override void SetDBObjectFields(EducationalLevelEIISMap dbObject, EIISObject eIISObject, bool isNew)
        {
            dbObject.Code = eIISObject.GetFieldStringValue(CodeField);
            dbObject.Name = eIISObject.GetFieldStringValue(NameField);
            dbObject.ShortName = eIISObject.GetFieldStringValue(ShortNameField);
        }

        private GenericCatalog<EducationalLevelEIISMap> cache_;
        protected override EducationalLevelEIISMap GetExistingObject(EIISObject eIISObject)
        {
            if (cache_ == null)
            {
                cache_ = new GenericCatalog<EducationalLevelEIISMap>(repository_.GetAll<EducationalLevelEIISMap>(), true, true, null);
            }

            EducationalLevelEIISMap result = null;

            string cacheKey = eIISObject.GetFieldStringValue(NameField);
            int? id = cache_.GetIdByNaturalKey(cacheKey);
            if (id.HasValue)
            {
                result = repository_.Get<EducationalLevelEIISMap>(id.Value);
            }

            return result;
        }

        protected override IEnumerable<string> RequiredFields
        {
            get { return new List<string>() { NameField }; }
        }

        private const string CodeField = "CODE";
        private const string NameField = "NAME";
        private const string ShortNameField = "SHORTNAME";
    }
}
