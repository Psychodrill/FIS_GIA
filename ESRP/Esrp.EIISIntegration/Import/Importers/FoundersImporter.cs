using System;
using System.Collections.Generic;
using System.Linq;
using Esrp.DB.EsrpADODB;

namespace Esrp.EIISIntegration.Import.Importers
{
    internal class FoundersImporter : ImporterBase<Founder>
    {
        public override string EIISObjectCode
        {
            get { return EIISObjectCodes.Founders; }
        }

        public override string Name
        {
            get { return "Учредители"; }
        }

        private const int UndefinedRegionId = 1000;
        protected override void SetDBObjectFields(Founder dbObject, EIISObject eIISObject, bool isNew)
        {
            string typeEIISId = eIISObject.GetFieldStringValue(TypeIdField);
            if ((!String.IsNullOrEmpty(typeEIISId)) && (founderTypesCache_.ContainsKey(typeEIISId)))
            {
                dbObject.TypeId = founderTypesCache_[typeEIISId];
            }

            string factAddressRegionEIISId = eIISObject.GetFieldStringValue(FactAddressRegionIdField);
            if ((!String.IsNullOrEmpty(factAddressRegionEIISId)) && (regionsCache_.ContainsKey(factAddressRegionEIISId)))
            {
                dbObject.FactAddressRegionId = regionsCache_[factAddressRegionEIISId];
            }
            else
            {
                dbObject.FactAddressRegionId = UndefinedRegionId;
            }

            string lawAddressRegionEIISId = eIISObject.GetFieldStringValue(LawAddressRegionIdField);
            if ((!String.IsNullOrEmpty(lawAddressRegionEIISId)) && (regionsCache_.ContainsKey(lawAddressRegionEIISId)))
            {
                dbObject.LawAddressRegionId = regionsCache_[lawAddressRegionEIISId];
            }
            else
            {
                dbObject.LawAddressRegionId = UndefinedRegionId;
            }

            dbObject.OrganizationFullName = eIISObject.GetFieldStringValue(OrganizationFullNameField);
            dbObject.OrganizationShortName = eIISObject.GetFieldStringValue(OrganizationShortNameField);
            dbObject.PersonFirstName = eIISObject.GetFieldStringValue(PersonFirstNameField);
            dbObject.PersonLastName = eIISObject.GetFieldStringValue(PersonLastNameField);
            dbObject.PersonPatronymic = eIISObject.GetFieldStringValue(PersonPatronymicField);

            dbObject.Inn = eIISObject.GetFieldStringValue(InnField);
            dbObject.Ogrn = eIISObject.GetFieldStringValue(OgrnField);
            dbObject.Kpp = eIISObject.GetFieldStringValue(KppField);

            dbObject.FactAddress = eIISObject.GetFieldStringValue(FactAddressField);
            dbObject.FactAddressDistrict = eIISObject.GetFieldStringValue(FactAddressDistrictField);
            dbObject.FactAddressTown = eIISObject.GetFieldStringValue(FactAddressTownNameField);
            dbObject.FactAddressStreet = eIISObject.GetFieldStringValue(FactAddressStreetField);
            dbObject.FactAddressHouseNumber = eIISObject.GetFieldStringValue(FactAddressHouseNumberField);
            dbObject.FactAddressPostalCode = eIISObject.GetFieldStringValue(FactAddressPostalCodeField);

            dbObject.LawAddress = eIISObject.GetFieldStringValue(LawAddressField);
            dbObject.LawAddressDistrict = eIISObject.GetFieldStringValue(LawAddressDistrictField);
            dbObject.LawAddressTown = eIISObject.GetFieldStringValue(LawAddressTownNameField);
            dbObject.LawAddressStreet = eIISObject.GetFieldStringValue(LawAddressStreetField);
            dbObject.LawAddressHouseNumber = eIISObject.GetFieldStringValue(LawAddressHouseNumberField);
            dbObject.LawAddressPostalCode = eIISObject.GetFieldStringValue(LawAddressPostalCodeField);

            dbObject.Emails = eIISObject.GetFieldStringValue(EMailField);
            dbObject.Faxes = eIISObject.GetFieldStringValue(FaxField);
            dbObject.Phones = eIISObject.GetFieldStringValue(PhoneField);
        }

        private Dictionary<string, int> regionsCache_;
        private Dictionary<string, int> founderTypesCache_;
        protected override void BeforeImportInternal()
        {
            regionsCache_ = repository_.GetWithNotEmptyEiisId<Region>()
                .ToDictionary((x) => x.Eiis_Id, (x) => x.Id, StringComparer.OrdinalIgnoreCase);
            founderTypesCache_ = repository_.GetWithNotEmptyEiisId<FounderType>()
                .ToDictionary((x) => x.Eiis_Id, (x) => x.Id, StringComparer.OrdinalIgnoreCase);
        }

        protected override Founder GetExistingObject(EIISObject eIISObject)
        {
            return null;
        }

        protected override IEnumerable<string> RequiredFields
        {
            get { return new List<string>(); }
        }

        protected override bool ValidateObjectFieldValues(EIISObject eIISObject, out ErrorMessage message)
        {
            string organizationFullName = eIISObject.GetFieldStringValue(OrganizationFullNameField);
            string lastName = eIISObject.GetFieldStringValue(PersonLastNameField);
            string firstName = eIISObject.GetFieldStringValue(PersonFirstNameField);
            string patronymic = eIISObject.GetFieldStringValue(PersonPatronymicField);
            if (String.IsNullOrEmpty(organizationFullName) && String.IsNullOrEmpty(lastName) && String.IsNullOrEmpty(firstName))
            {
                message = new ErrorMessage(ErrorMessage.RequiredFieldMessage, "для учредителя не заполнены поля ни название организации, ни ФИО");
                return false;
            }

            return base.ValidateObjectFieldValues(eIISObject, out message);
        }

        private const string TypeIdField = "FOUNDER_TYPE_FK";

        private const string LawAddressRegionIdField = "L_ADDRESS_REGION_FK";
        private const string FactAddressRegionIdField = "P_ADDRESS_REGION_FK";

        private const string OrganizationFullNameField = "ORGANIZATION_FULLNAME";
        private const string OrganizationShortNameField = "ORGANIZATION_SHORTNAME";
        private const string PersonLastNameField = "LASTNAME";
        private const string PersonFirstNameField = "FIRSTNAME";
        private const string PersonPatronymicField = "PATRONYMIC";

        private const string InnField = "INN";
        private const string KppField = "KPP";
        private const string OgrnField = "OGRN";

        private const string PhoneField = "PHONES";
        private const string FaxField = "FAXES";
        private const string EMailField = "EMAILS";

        private const string LawAddressField = "L_ADDRESS";
        private const string FactAddressField = "P_ADDRESS";
        private const string LawAddressDistrictField = "L_ADDRESS_DISTRICT";
        private const string FactAddressDistrictField = "P_ADDRESS_DISTRICT";
        private const string LawAddressTownNameField = "L_ADDRESS_TOWN";
        private const string FactAddressTownNameField = "P_ADDRESS_TOWN";
        private const string LawAddressStreetField = "L_ADDRESS_STREET";
        private const string FactAddressStreetField = "P_ADDRESS_STREET";
        private const string LawAddressHouseNumberField = "L_ADDRESS_HOUSE_NUMBER";
        private const string FactAddressHouseNumberField = "P_ADDRESS_HOUSE_NUMBER";
        private const string LawAddressPostalCodeField = "L_ADDRESS_POSTAL_CODE";
        private const string FactAddressPostalCodeField = "P_ADDRESS_POSTAL_CODE";
    }
}
