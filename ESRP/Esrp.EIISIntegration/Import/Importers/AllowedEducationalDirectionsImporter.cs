using System;
using System.Collections.Generic;
using System.Linq;
using Esrp.DB.EsrpADODB;

namespace Esrp.EIISIntegration.Import.Importers
{
    internal class AllowedEducationalDirectionsImporter : ImporterBase<AllowedEducationalDirection>
    {
        public override string EIISObjectCode
        {
            get { return EIISObjectCodes.AllowedEducationalDirections; }
        }

        public override string Name
        {
            get { return "Разрешенные направления подготовки"; }
        }

        Dictionary<string, int> licenseSupplementsCache_;
        Dictionary<string, int> educationalDirectionsCache_;
        protected override void SetDBObjectFields(AllowedEducationalDirection dbObject, EIISObject eIISObject, bool isNew)
        { 
            string licenseSupplementEIISId = eIISObject.GetFieldStringValue(LicenseSupplementIdField);
            dbObject.LicenseSupplementId = licenseSupplementsCache_[licenseSupplementEIISId];

            string directionEIISId = eIISObject.GetFieldStringValue(EducationalDirectionIdField);
            if ((!String.IsNullOrEmpty(directionEIISId)) && (educationalDirectionsCache_.ContainsKey(directionEIISId)))
            {
                dbObject.EducationalDirectionId = educationalDirectionsCache_[directionEIISId];
            } 
        }

        protected override void BeforeImportInternal()
        {
            licenseSupplementsCache_ = repository_.GetWithNotEmptyEiisId<LicenseSupplement>()
                .ToDictionary((x) => x.Eiis_Id, (x) => x.Id, StringComparer.OrdinalIgnoreCase);
            educationalDirectionsCache_ = repository_.GetWithNotEmptyEiisId<EducationalDirection>()
                .ToDictionary((x) => x.Eiis_Id, (x) => x.Id, StringComparer.OrdinalIgnoreCase);
        }

        protected override bool ValidateObjectFieldValues(EIISObject eIISObject, out ErrorMessage message)
        {
            string licenseSupplementEIISId = eIISObject.GetFieldStringValue(LicenseSupplementIdField);
            if ((String.IsNullOrEmpty(licenseSupplementEIISId)) || (!licenseSupplementsCache_.ContainsKey(licenseSupplementEIISId)))
            {
                message = new ErrorMessage(ErrorMessage.RelatedLicenseSupplementNotFoundMessage, String.Format("связанный объект (приложение к лицензии) с идентификатором {0} не найден", licenseSupplementEIISId));
                return false;
            }

            string directionEIISId = eIISObject.GetFieldStringValue(EducationalDirectionIdField);
            if ((String.IsNullOrEmpty(directionEIISId)) || (!educationalDirectionsCache_.ContainsKey(directionEIISId)))
            {
                message = new ErrorMessage(ErrorMessage.RelatedDirectionNotFoundMessage, String.Format("связанный объект (направление подготовки) с идентификатором {0} не найден", directionEIISId));
                return false;
            }

            return base.ValidateObjectFieldValues(eIISObject, out message);
        }

        protected override AllowedEducationalDirection GetExistingObject(EIISObject eIISObject)
        {
            return null;
        }

        protected override IEnumerable<string> RequiredFields
        {
            get { return new List<string>() { LicenseSupplementIdField, EducationalDirectionIdField }; }
        }

        private const string LicenseSupplementIdField = "LICENSE_APP_FK"; 
        private const string EducationalDirectionIdField = "EDU_PROGRAM_FK";
    }
}
