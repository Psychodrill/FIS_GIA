using System;
using System.Collections.Generic;
using System.Linq;
using Esrp.DB.EsrpADODB;
using Esrp.EIISIntegration.Catalogs;

namespace Esrp.EIISIntegration.Import.Importers
{
    internal class RegionsImporter : ImporterBase<Region>
    {
        public override string EIISObjectCode
        {
            get { return EIISObjectCodes.Regions; }
        }

        public override string Name
        {
            get { return "Субъекты РФ"; }
        }

        protected override bool AllowInsertObjects
        {
            get { return false; }
        }

        protected override bool AllowDeleteObjects
        {
            get { return false; }
        }

        protected override void SetDBObjectFields(Region dbObject, EIISObject eIISObject, bool isNew)
        {
            dbObject.Name = eIISObject.GetFieldStringValue(NameField);
            dbObject.Code = GetRegionCode(eIISObject); 
        }

        private GenericCatalog<Region> cache_;
        protected override Region GetExistingObject(EIISObject eIISObject)
        {
            if (cache_ == null)
            {
                cache_ = new GenericCatalog<Region>(repository_.GetAll<Region>(), true, true, new string[] { "автономная", "автономный", "город", "югра" });
            }

            Region result = null;

            string cacheKey = eIISObject.GetFieldStringValue(NameField);
            int? id = cache_.GetIdByNaturalKey(cacheKey);
            if (id.HasValue)
            {
                result = repository_.Get<Region>(id.Value);
            }

            if (result == null)
            {
                string code = GetRegionCode(eIISObject);
                if (!String.IsNullOrEmpty(code))
                {
                    result = repository_.GetAll<Region>().FirstOrDefault(obj => obj.Code.Equals(code));
                }
            }
            return result;
        }

        private string GetRegionCode(EIISObject eIISObject)
        {
            string result = eIISObject.GetFieldStringValue(CodeField);
            if (String.IsNullOrEmpty(result))
            {
                string eiisId = eIISObject.Eiis_Id;
                int temp;
                if ((Int32.TryParse(eiisId, out temp)) && (temp < 100))
                {
                    result = eiisId;
                }
            }
            if (String.IsNullOrEmpty(result))
                return "00";

            if (result.Length == 1)
            {
                result = "0" + result;
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
