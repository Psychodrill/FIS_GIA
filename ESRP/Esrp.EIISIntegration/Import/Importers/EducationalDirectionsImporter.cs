using System;
using System.Collections.Generic;
using System.Linq;
using Esrp.DB.EsrpADODB;

namespace Esrp.EIISIntegration.Import.Importers
{
    internal class EducationalDirectionsImporter : ImporterBase<EducationalDirection>
    {
        public override string EIISObjectCode
        {
            get { return EIISObjectCodes.EducationalDirections; }
        }

        public override string Name
        {
            get { return "Направления подготовки"; }
        } 

        Dictionary<string, int> educationalDirectionGroupsCache_;
        Dictionary<string, int> educationalDirectionTypesCache_;
        Dictionary<string, int> educationalLevelsMapCache_;
        protected override void SetDBObjectFields(EducationalDirection dbObject, EIISObject eIISObject, bool isNew)
        {
            dbObject.Name = eIISObject.GetFieldStringValue(NameField);
            dbObject.Code = eIISObject.GetFieldStringValue(CodeField);
            dbObject.Period = eIISObject.GetFieldStringValue(PeriodField);
            dbObject.IsActual = eIISObject.GetFieldBooleanValue(IsActualField).GetValueOrDefault(true);

            string groupEIISId = eIISObject.GetFieldStringValue(GroupIdField);
            if ((!String.IsNullOrEmpty(groupEIISId)) && (educationalDirectionGroupsCache_.ContainsKey(groupEIISId)))
            {
                dbObject.EducationalDirectionGroupId = educationalDirectionGroupsCache_[groupEIISId];
            }

            string typeEIISId = eIISObject.GetFieldStringValue(TypeIdField);
            if ((!String.IsNullOrEmpty(typeEIISId)) && (educationalDirectionTypesCache_.ContainsKey(typeEIISId)))
            {
                dbObject.EducationalDirectionTypeId = educationalDirectionTypesCache_[typeEIISId];
            }

            string educationalLevelEIISId = eIISObject.GetFieldStringValue(EducationalLevelIdField);
            if ((!String.IsNullOrEmpty(educationalLevelEIISId)) && (educationalLevelsMapCache_.ContainsKey(educationalLevelEIISId)))
            {
                dbObject.MappedEducationalLevelId = educationalLevelsMapCache_[educationalLevelEIISId];
            }
        }

        protected override void BeforeImportInternal()
        {
            educationalDirectionGroupsCache_ = repository_.GetWithNotEmptyEiisId<EducationalDirectionGroup>()
                .ToDictionary((x) => x.Eiis_Id, (x) => x.Id, StringComparer.OrdinalIgnoreCase);

            educationalDirectionTypesCache_ = repository_.GetWithNotEmptyEiisId<EducationalDirectionType>()
                .ToDictionary((x) => x.Eiis_Id, (x) => x.Id, StringComparer.OrdinalIgnoreCase);

            educationalLevelsMapCache_ = repository_.GetWithNotEmptyEiisId<EducationalLevelEIISMap>()
                .ToDictionary((x) => x.Eiis_Id, (x) => x.MappedEducationalLevelId.GetValueOrDefault(), StringComparer.OrdinalIgnoreCase);
        }

        protected override EducationalDirection GetExistingObject(EIISObject eIISObject)
        {
            return null;
        }

        protected override IEnumerable<string> RequiredFields
        {
            get { return new List<string>() { NameField, CodeField }; }
        }

        private const string NameField = "NAME";
        private const string CodeField = "CODE"; 
        private const string PeriodField = "PERIOD";
        private const string IsActualField = "ACTUAL"; 
        private const string GroupIdField = "UGS_FK";
        private const string TypeIdField = "EDU_PROGRAM_TYPE_FK";
        private const string EducationalLevelIdField = "EDULEVEL_FK";
    }
}
