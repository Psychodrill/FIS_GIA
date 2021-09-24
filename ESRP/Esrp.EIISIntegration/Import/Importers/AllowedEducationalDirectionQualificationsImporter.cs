using System;
using System.Collections.Generic;
using System.Linq;
using Esrp.DB.EsrpADODB;

namespace Esrp.EIISIntegration.Import.Importers
{
    internal class AllowedEducationalDirectionQualificationsImporter : ImporterBase<AllowedEducationalDirectionQualification>
    {
        public override string EIISObjectCode
        {
            get { return EIISObjectCodes.AllowedEducationalDirectionQualificationLink; }
        }

        public override string Name
        {
            get { return "Квалификации разрешенных направлений подготовки"; }
        }

        protected override void SetDBObjectFields(AllowedEducationalDirectionQualification dbObject, EIISObject eIISObject, bool isNew)
        {
            dbObject.QualificationId = qualificationsCache_[eIISObject.GetFieldStringValue(QualificationIdField)];
            dbObject.AllowedEducationalDirectionId = allowedEducationalDirectionsCache_[eIISObject.GetFieldStringValue(AllowedEducationalDirectionIdField)];
        }

        private Dictionary<string, int> allowedEducationalDirectionsCache_;
        private Dictionary<string, int> qualificationsCache_;
        protected override void BeforeImportInternal()
        {
            allowedEducationalDirectionsCache_ = repository_.GetWithNotEmptyEiisId<AllowedEducationalDirection>()
                .ToDictionary((x) => x.Eiis_Id, (x) => x.Id, StringComparer.OrdinalIgnoreCase);
            qualificationsCache_ = repository_.GetWithNotEmptyEiisId<Qualification>()
                .ToDictionary((x) => x.Eiis_Id, (x) => x.Id, StringComparer.OrdinalIgnoreCase);
        }

        protected override bool ValidateObjectFieldValues(EIISObject eIISObject, out ErrorMessage message)
        {
            string qualificationEIISId = eIISObject.GetFieldStringValue(QualificationIdField);
            if ((String.IsNullOrEmpty(qualificationEIISId)) || (!qualificationsCache_.ContainsKey(qualificationEIISId)))
            {
                message = new ErrorMessage(ErrorMessage.RelatedQualificationNotFoundMessage, String.Format("связанный объект (квалификация) с идентификатором {0} не найден", qualificationEIISId));
                return false;
            }
            string allowedEducationalDirectionEIISId = eIISObject.GetFieldStringValue(AllowedEducationalDirectionIdField);
            if ((String.IsNullOrEmpty(allowedEducationalDirectionEIISId)) || (!allowedEducationalDirectionsCache_.ContainsKey(allowedEducationalDirectionEIISId)))
            {
                message = new ErrorMessage(ErrorMessage.RelatedAllowedDirectionNotFoundMessage, String.Format("связанный объект (лицензированное направление подготовки) с идентификатором {0} не найден", allowedEducationalDirectionEIISId));
                return false;
            }

            return base.ValidateObjectFieldValues(eIISObject, out message);
        }

        protected override AllowedEducationalDirectionQualification GetExistingObject(EIISObject eIISObject)
        {
            return null;
        }

        protected override IEnumerable<string> RequiredFields
        {
            get { return new List<string>() { QualificationIdField, AllowedEducationalDirectionIdField }; }
        }

        private const string QualificationIdField = "QUALIFICATION_FK";
        private const string AllowedEducationalDirectionIdField = "LICENSED_PROGRAM_FK"; 
    }
}
