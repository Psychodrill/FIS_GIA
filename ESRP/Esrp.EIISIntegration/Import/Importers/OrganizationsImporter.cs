using System;
using System.Collections.Generic;
using System.Linq;
using Esrp.DB.EsrpADODB;
using Esrp.Integration.Common;

namespace Esrp.EIISIntegration.Import.Importers
{
    internal class OrganizationsImporter : ImporterBase<Organization2010>
    {
        public override string EIISObjectCode
        {
            get { return EIISObjectCodes.Organizations; }
        }

        public override string Name
        {
            get { return "Организации"; }
        }

        public const int DeletedStatusId = 3;
        public const int FoudnerOrganizationTypeId = 6;
        private const int OtherOrganizationKindId = 8;
        protected override void SetDBObjectFields(Organization2010 dbObject, EIISObject eIISObject, bool isNew)
        {
            if (isNew)
            {
                dbObject.CreateDate = DateTime.Now;
                dbObject.UpdateDate = DateTime.Now;
                dbObject.WasImportedAtStart = true;
                dbObject.UpdatedByUser = false;
                SetDBObjectFieldsInternal(dbObject, eIISObject, isNew);
            }
            else
            {
                SetDBObjectFieldsInternal(dbObject, eIISObject, isNew);
            }
        }

        private void SetDBObjectFieldsInternal(Organization2010 dbObject, EIISObject eIISObject, bool isNew)
        {
            if ((isNew) || (!dbObject.UpdatedByUser))
            {
                dbObject.DirectorPosition = eIISObject.GetFieldStringValue(DirectorPositionField);
                dbObject.DirectorPositionInGenetive = dbObject.DirectorPosition;
                dbObject.DirectorFullName = eIISObject.GetFieldStringValue(DirectorFullNameField);
                dbObject.LawAddress = eIISObject.GetFieldStringValue(LawAddressField);
                dbObject.FactAddress = eIISObject.GetFieldStringValue(AddressField);

                dbObject.TownName = eIISObject.GetFieldStringValue(TownNameField);
                dbObject.Phone = eIISObject.GetFieldStringValue(PhoneField);
                dbObject.Fax = eIISObject.GetFieldStringValue(FaxField);
                dbObject.EMail = eIISObject.GetFieldStringValue(EMailField);
                dbObject.Site = eIISObject.GetFieldStringValue(WebPageField);

                dbObject.ShortName = eIISObject.GetFieldStringValue(ShortNameField);

                string kindEIISId = eIISObject.GetFieldStringValue(KindIdField);
                if ((!String.IsNullOrEmpty(kindEIISId)) && (kindsMapCache_.ContainsKey(kindEIISId)))
                {
                    dbObject.KindId = kindsMapCache_[kindEIISId];
                }
                else if (isNew)
                {
                    dbObject.KindId = OtherOrganizationKindId;
                }

                dbObject.KPP = eIISObject.GetFieldStringValue(KppField);
            }

            dbObject.FullName = eIISObject.GetFieldStringValue(FullNameField);

            dbObject.INN = eIISObject.GetFieldStringValue(InnField);
            dbObject.OGRN = eIISObject.GetFieldStringValue(OgrnField);

            dbObject.IsFilial = eIISObject.GetFieldBooleanValue(IsBranchField).GetValueOrDefault();

            dbObject.IsLawEnforcmentOrganization = eIISObject.GetFieldBooleanValue(IsLawEnforcmentOrganizationField).GetValueOrDefault();
            dbObject.IsReligious = eIISObject.GetFieldBooleanValue(IsReligiousOrganizationField).GetValueOrDefault();

            string regionEIISId = eIISObject.GetFieldStringValue(RegionIdField);
            dbObject.RegionId = regionsCache_[regionEIISId];

            string statusEIISId = eIISObject.GetFieldStringValue(StatusIdField);
            int newStatus = statusesMapCache_[statusEIISId];
            if (dbObject.StatusId != newStatus)
            {
                dbObject.StatusId = newStatus;
            }

            string mainOrgEIISId = eIISObject.GetFieldStringValue(MainIdField);
            if ((!String.IsNullOrEmpty(mainOrgEIISId)) && (mainOrganizationsCache_.ContainsKey(mainOrgEIISId)))
            {
                dbObject.MainId = mainOrganizationsCache_[mainOrgEIISId];
            }

            string eiisTypeName;
            dbObject.TypeId = GetOrganizationType(dbObject.Eiis_Id, out eiisTypeName);
            dbObject.IsPrivate = GetOrganizationIsPrivate(dbObject.Eiis_Id);

            if (dbObject.HasChanges)
            {
                dbObject.UpdateDate = DateTime.Now;
                dbObject.Version++;
            }
        }


        private Dictionary<string, int> regionsCache_;
        private Dictionary<string, int> kindsMapCache_;
        private Dictionary<string, int> statusesMapCache_;
        private Dictionary<string, int> cacheByISLODId_;
        private Dictionary<string, int> mainOrganizationsCache_;
        private IEnumerable<ExtendedEIISObject> additionalData_;
        protected override void BeforeImportInternal()
        {
            regionsCache_ = repository_.GetWithNotEmptyEiisId<Region>()
                .ToDictionary((x) => x.Eiis_Id, (x) => x.Id, StringComparer.OrdinalIgnoreCase);
            kindsMapCache_ = repository_.GetWithNotEmptyEiisId<OrganizationKindEIISMap>()
                .Where(x => x.OrganizationKindId.HasValue)
                .ToDictionary((x) => x.Eiis_Id, (x) => x.OrganizationKindId.Value, StringComparer.OrdinalIgnoreCase);
            statusesMapCache_ = repository_.GetWithNotEmptyEiisId<OrganizationStatusEIISMap>()
                .Where(x => x.OrganizationOperatingStatusId.HasValue)
                .ToDictionary((x) => x.Eiis_Id, (x) => x.OrganizationOperatingStatusId.Value, StringComparer.OrdinalIgnoreCase);

            cacheByISLODId_ = new Dictionary<string, int>();
            foreach (Organization2010 organization in repository_.GetOrganizationWithNotEmptyIslodId())
            {
                if (!cacheByISLODId_.ContainsKey(organization.ISLOD_GUID))
                {
                    cacheByISLODId_.Add(organization.ISLOD_GUID, organization.Id);
                }
            }

            MemoryImporter additionalDataImporter = new MemoryImporter(EIISObjectCodes.OrganizationsAdditionalData);
            additionalDataImporter.Init(this.sessionId_, this.client_, this.connectionString_);
            additionalDataImporter.OnMessage += new EventHandler<MessageEventArgs>(MemoryImporter_OnMessage);            
            additionalDataImporter.ImportData();
            additionalData_ = additionalDataImporter.Objects;
        }

        protected void MemoryImporter_OnMessage(object sender, MessageEventArgs e)
        {
            RaiseMessage(String.Format("(вспомогательный объект) {0}", e.Message));
        }

        protected override void BeforeImportStepInternal()
        {
            mainOrganizationsCache_ = repository_.GetWithNotEmptyEiisId<Organization2010>()
                .ToDictionary((x) => x.Eiis_Id, (x) => x.Id, StringComparer.OrdinalIgnoreCase);
        }

        private const int HigherEducationalOrganizationType = 1;
        private const int MediumEducationalOrganizationType = 2;
        private const int OtherEducationalOrganizationType = 5;
        private const int NotImportingOrganizationType = -1;
        private int GetOrganizationType(string organizationEiisId, out string eiisTypeName)
        {
            ExtendedEIISObject additionalData = additionalData_.FirstOrDefault(x => x.GetFieldStringValue(AdditionalDataOrganizationIdField) == organizationEiisId);
            if (additionalData == null)
            {
                eiisTypeName = null;
                return OtherEducationalOrganizationType;
            }

            eiisTypeName = additionalData.GetFieldStringValue(AdditionalDataTypeNameField);
            if (String.IsNullOrEmpty(eiisTypeName))
                return OtherEducationalOrganizationType;

            if (String.Equals(eiisTypeName, "Образовательная организация высшего образования", StringComparison.OrdinalIgnoreCase))
                return HigherEducationalOrganizationType;
            else if (String.Equals(eiisTypeName, "Организация среднего профессионального образования", StringComparison.OrdinalIgnoreCase))
                return MediumEducationalOrganizationType;
            else if (String.Equals(eiisTypeName, "Научная организация", StringComparison.OrdinalIgnoreCase))
                return OtherEducationalOrganizationType;
            else if (String.Equals(eiisTypeName, "Образовательная организация дополнительного профессионального образования", StringComparison.OrdinalIgnoreCase))
                return OtherEducationalOrganizationType;
            else if (String.Equals(eiisTypeName, "Общеобразовательная организация", StringComparison.OrdinalIgnoreCase))
                return NotImportingOrganizationType;
            else if (String.Equals(eiisTypeName, "Другие", StringComparison.OrdinalIgnoreCase))
                return OtherEducationalOrganizationType;
            else
                return OtherEducationalOrganizationType;
        }

        private bool GetOrganizationIsPrivate(string organizationEiisId)
        {
            ExtendedEIISObject additionalData = additionalData_.FirstOrDefault(x => x.GetFieldStringValue(AdditionalDataOrganizationIdField) == organizationEiisId);
            if (additionalData == null)
                return false;

            bool isState = additionalData.GetFieldBooleanValue(AdditionalDataIsStateField).GetValueOrDefault(true);
            return !isState;
        }

        protected override bool ValidateObjectFieldValues(EIISObject eIISObject, out ErrorMessage message)
        {
            string regionEIISId = eIISObject.GetFieldStringValue(RegionIdField);
            if ((String.IsNullOrEmpty(regionEIISId)) || (!regionsCache_.ContainsKey(regionEIISId)))
            {
                message = new ErrorMessage(ErrorMessage.RelatedCatalogNotFoundMessage, String.Format("связанный объект (субъект РФ) с идентификатором {0} не найден", regionEIISId));
                return false;
            }

            string statusEIISId = eIISObject.GetFieldStringValue(StatusIdField);
            if ((String.IsNullOrEmpty(statusEIISId)) || (!statusesMapCache_.ContainsKey(statusEIISId)))
            {
                message = new ErrorMessage(ErrorMessage.RelatedCatalogNotFoundMessage, String.Format("связанный объект (статус организации) с идентификатором {0} не найден", statusEIISId));
                return false;
            }

            return base.ValidateObjectFieldValues(eIISObject, out message);
        }

        public string[] higherOrganizationTypes = 
        { 
            "45FE134B2CCD1699C83C3DE6AABE0F85",
            "4e27741606344006ac8a67c91a87d320",
            "702D32E926DD4CC5A9836A64AFD36453", 
            "7449D471271A141D0E061EEB463D7624"
        };

        public string[] mediumOrganizationTypes = 
        { 
            "1F637C9D1AB1B4F791EDF33AF1EE0413",
            "3CA3AAD447FA48408F323270921D3E15",
            "4FABD21CFE54438F9D7786AF0535B5D7",
            "76f9e36d13084c1182539a5370694bdd"
        };

        public string[] educationalOrganizationTypesForImport = 
        { 
            "45FE134B2CCD1699C83C3DE6AABE0F85",
            "4e27741606344006ac8a67c91a87d320", 
            "702D32E926DD4CC5A9836A64AFD36453",
            "7449D471271A141D0E061EEB463D7624"
        };

        protected override bool SkipObject(EIISObject eIISObject, out bool retry, out ErrorMessage message)
        {
            string mainOrgEIISId = eIISObject.GetFieldStringValue(MainIdField);

            string eiisTypeName;
            int typeId = GetOrganizationType(eIISObject.Eiis_Id, out eiisTypeName);
            if (typeId == NotImportingOrganizationType)
            {
                message = new ErrorMessage(ErrorMessage.ObjectSkippedMessage, String.Format("организация с типом {0} не подлежит импорту", eiisTypeName ?? "(не указан)"));
                retry = false;
                return true;
            }

            if ((!String.IsNullOrEmpty(mainOrgEIISId)) && (!mainOrganizationsCache_.ContainsKey(mainOrgEIISId)))
            {
                message = new ErrorMessage(ErrorMessage.RelatedHeadOrganizationNotFoundMessage, String.Format("связанный объект (головная организация) с идентификатором {0} не найден", mainOrgEIISId));
                retry = true;
                return true;
            }

            return base.SkipObject(eIISObject, out retry, out   message);
        }

        protected override Organization2010 GetExistingObject(EIISObject eIISObject)
        {
            return null;
        }

        protected override IEnumerable<string> RequiredFields
        {
            get { return new List<string>() { FullNameField, RegionIdField, StatusIdField }; }
        }

        private const string RegionIdField = "REGION_FK";
        private const string KindIdField = "SCHOOL_KIND_FK";
        private const string StatusIdField = "SCHOOL_STATUS_FK";
        private const string MainIdField = "PARENT_SCHOOL_FK";

        private const string FullNameField = "SCHOOLNAME";
        private const string ShortNameField = "SHORTNAME";
        private const string InnField = "INN";
        private const string KppField = "KPP";
        private const string OgrnField = "GOSREGNUM";
        private const string IsBranchField = "BRANCH";
        private const string DirectorPositionField = "CHARGEPOSITION";
        private const string DirectorFullNameField = "CHARGEFIO";
        private const string LawAddressField = "LAWADDRESS";
        private const string AddressField = "ADDRESS";
        private const string TownNameField = "CITY";
        private const string PhoneField = "PHONES";
        private const string FaxField = "FAXS";
        private const string EMailField = "MAILS";
        private const string WebPageField = "WWW";
        private const string IsLawEnforcmentOrganizationField = "ISSTRONG";
        private const string IsReligiousOrganizationField = "ISRELIGION";

        private const string AdditionalDataOrganizationIdField = "ID";
        private const string AdditionalDataIsStateField = "IS_GOS";
        private const string AdditionalDataTypeNameField = "SCHOOL_AGR_TYPE";
        private const string EducationalFormOrganizationIdField = "SCHOOL_FK";
        private const string EducationalFormFromIdField = "EDUFORM_FK";
        private const string FounderLinkOrganizationIdField = "SCHOOL_FK";
        private const string FounderLinkFounderIdField = "FONDER_FK";
    }
}
