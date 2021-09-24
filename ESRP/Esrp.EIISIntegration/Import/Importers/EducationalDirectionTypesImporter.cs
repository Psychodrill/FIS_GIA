using System.Collections.Generic;
using Esrp.DB.EsrpADODB;
using Esrp.EIISIntegration.Catalogs;

namespace Esrp.EIISIntegration.Import.Importers
{
    internal class EducationalDirectionTypesImporter : ImporterBase<EducationalDirectionType>
    {
        public override string EIISObjectCode
        {
            get { return EIISObjectCodes.EducationalDirectionTypes; }
        }

        public override string Name
        {
            get { return "Типы образовательных программ"; }
        }

        protected override void SetDBObjectFields(EducationalDirectionType dbObject, EIISObject eIISObject, bool isNew)
        {
            dbObject.Code = eIISObject.GetFieldStringValue(CodeField);
            dbObject.Name = eIISObject.GetFieldStringValue(NameField);
            dbObject.ShortName = eIISObject.GetFieldStringValue(ShortNameField);
        }

        private GenericCatalog<EducationalDirectionType> cache_;
        protected override EducationalDirectionType GetExistingObject(EIISObject eIISObject)
        {
            if (cache_ == null)
            {
                cache_ = new GenericCatalog<EducationalDirectionType>(repository_.GetAll<EducationalDirectionType>(), true, true, null);
            }

            EducationalDirectionType result = null;

            string cacheKey = eIISObject.GetFieldStringValue(NameField);
            int? id = cache_.GetIdByNaturalKey(cacheKey);
            if (id.HasValue)
            {
                result = repository_.Get<EducationalDirectionType>(id.Value);
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
