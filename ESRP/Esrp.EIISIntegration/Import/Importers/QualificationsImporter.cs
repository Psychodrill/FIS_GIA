using System.Collections.Generic;
using Esrp.DB.EsrpADODB;
using Esrp.EIISIntegration.Catalogs;

namespace Esrp.EIISIntegration.Import.Importers
{
    internal class QualificationsImporter : ImporterBase<Qualification>
    {
        public override string EIISObjectCode
        {
            get { return EIISObjectCodes.Qualification; }
        }

        public override string Name
        {
            get { return "Квалификации"; }
        }

        protected override bool AllowDeleteObjects { get { return false; } }

        protected override void SetDBObjectFields(Qualification dbObject, EIISObject eIISObject, bool isNew)
        {
            dbObject.Name = eIISObject.GetFieldStringValue(NameField);
            dbObject.Code = eIISObject.GetFieldStringValue(CodeField);
            dbObject.IsActual = eIISObject.GetFieldBooleanValue(IsActualField).GetValueOrDefault(true);
        }

        protected override Qualification GetExistingObject(EIISObject eIISObject)
        {
            return null;
        }

        protected override IEnumerable<string> RequiredFields
        {
            get { return new List<string>() { NameField }; }
        }

        private const string NameField = "NAME";
        private const string CodeField = "CODE";
        private const string IsActualField = "ACTUAL";
    }
}
