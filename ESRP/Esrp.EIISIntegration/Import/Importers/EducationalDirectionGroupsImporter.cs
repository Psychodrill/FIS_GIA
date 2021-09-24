using System;
using System.Collections.Generic;
using Esrp.DB.EsrpADODB;

namespace Esrp.EIISIntegration.Import.Importers
{
    internal class EducationalDirectionGroupsImporter : ImporterBase<EducationalDirectionGroup>
    {
        public override string EIISObjectCode
        {
            get { return EIISObjectCodes.EducationalDirectionGroups; }
        }

        public override string Name
        {
            get { return "Укрупненные группы специальностей"; }
        } 

        protected override void SetDBObjectFields(EducationalDirectionGroup dbObject, EIISObject eIISObject, bool isNew)
        {
            if (isNew)
            {
                dbObject.CreatedDate = DateTime.Now;
            }

            dbObject.Name = eIISObject.GetFieldStringValue(NameField);
            dbObject.Code = eIISObject.GetFieldStringValue(CodeField);
            dbObject.IsActual = eIISObject.GetFieldBooleanValue(IsActualField).GetValueOrDefault(true);

            if (dbObject.HasChanges)
            {
                dbObject.ModifiedDate = DateTime.Now;
            }
        }

        protected override EducationalDirectionGroup GetExistingObject(EIISObject eIISObject)
        {
            return null;
        }

        protected override IEnumerable<string> RequiredFields
        {
            get { return new List<string>() { NameField, CodeField }; }
        } 
        
        private const string NameField = "NAME";
        private const string CodeField = "CODE";
        private const string IsActualField = "ACTUAL";
    }
}
