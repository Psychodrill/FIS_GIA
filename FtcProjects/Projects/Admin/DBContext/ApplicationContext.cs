using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Admin.Models.DBContext;
using System.Linq;

namespace Admin.DBContext
{
    public partial class ApplicationContext : DbContext
    {
        public ApplicationContext()
        {
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        public virtual DbSet<A> A { get; set; }
        public virtual DbSet<Aaa> Aaa { get; set; }
        public virtual DbSet<AbitInstitutionGeneral> AbitInstitutionGeneral { get; set; }
        public virtual DbSet<AbitInstitutionIndAch> AbitInstitutionIndAch { get; set; }
        public virtual DbSet<AbitInstitutionTests> AbitInstitutionTests { get; set; }
        public virtual DbSet<AbitInstitutionVolAndStruct> AbitInstitutionVolAndStruct { get; set; }
        public virtual DbSet<ActualId> ActualId { get; set; }
        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<AdmissionData> AdmissionData { get; set; }
        public virtual DbSet<AdmissionItemType> AdmissionItemType { get; set; }
        public virtual DbSet<AdmissionRules> AdmissionRules { get; set; }
        public virtual DbSet<AdmissionVolume> AdmissionVolume { get; set; }
        public virtual DbSet<AdmissionVolumeKcp> AdmissionVolumeKcp { get; set; }
        public virtual DbSet<AetdTmp> AetdTmp { get; set; }
        public virtual DbSet<AllowedDirectionStatus> AllowedDirectionStatus { get; set; }
        public virtual DbSet<AllowedDirections> AllowedDirections { get; set; }
        public virtual DbSet<Allsubjects> Allsubjects { get; set; }
        public virtual DbSet<AppealStatus> AppealStatus { get; set; }
        public virtual DbSet<Application> Application { get; set; }
        public virtual DbSet<ApplicationCheckStatus> ApplicationCheckStatus { get; set; }
        public virtual DbSet<ApplicationCompetitiveGroupItem> ApplicationCompetitiveGroupItem { get; set; }
        public virtual DbSet<ApplicationCompositionResults> ApplicationCompositionResults { get; set; }
        public virtual DbSet<ApplicationCompositionResultsApprob> ApplicationCompositionResultsApprob { get; set; }
        public virtual DbSet<ApplicationCompositionResultsTmp> ApplicationCompositionResultsTmp { get; set; }
        public virtual DbSet<ApplicationConsidered> ApplicationConsidered { get; set; }
        public virtual DbSet<ApplicationEntranceTestDocument> ApplicationEntranceTestDocument { get; set; }
        public virtual DbSet<ApplicationEntranceTestDocumentTmpR> ApplicationEntranceTestDocumentTmpR { get; set; }
        public virtual DbSet<ApplicationEntrantDocument> ApplicationEntrantDocument { get; set; }
        public virtual DbSet<ApplicationExportRequest> ApplicationExportRequest { get; set; }
        public virtual DbSet<ApplicationExtra> ApplicationExtra { get; set; }
        public virtual DbSet<ApplicationExtraDefinition> ApplicationExtraDefinition { get; set; }
        public virtual DbSet<ApplicationForcedAdmissionDocument> ApplicationForcedAdmissionDocument { get; set; }
        public virtual DbSet<ApplicationForcedAdmissionReason> ApplicationForcedAdmissionReason { get; set; }
        public virtual DbSet<ApplicationReturnDocumentsType> ApplicationReturnDocumentsType { get; set; }
        public virtual DbSet<ApplicationSelectedCompetitiveGroup> ApplicationSelectedCompetitiveGroup { get; set; }
        public virtual DbSet<ApplicationSelectedCompetitiveGroupItem> ApplicationSelectedCompetitiveGroupItem { get; set; }
        public virtual DbSet<ApplicationSelectedCompetitiveGroupTarget> ApplicationSelectedCompetitiveGroupTarget { get; set; }
        public virtual DbSet<ApplicationStatusType> ApplicationStatusType { get; set; }
        public virtual DbSet<AspnetApplications> AspnetApplications { get; set; }
        public virtual DbSet<AspnetMembership> AspnetMembership { get; set; }
        public virtual DbSet<AspnetPaths> AspnetPaths { get; set; }
        public virtual DbSet<AspnetPersonalizationAllUsers> AspnetPersonalizationAllUsers { get; set; }
        public virtual DbSet<AspnetPersonalizationPerUser> AspnetPersonalizationPerUser { get; set; }
        public virtual DbSet<AspnetProfile> AspnetProfile { get; set; }
        public virtual DbSet<AspnetRoles> AspnetRoles { get; set; }
        public virtual DbSet<AspnetSchemaVersions> AspnetSchemaVersions { get; set; }
        public virtual DbSet<AspnetUsers> AspnetUsers { get; set; }
        public virtual DbSet<AspnetUsersInRoles> AspnetUsersInRoles { get; set; }
        public virtual DbSet<AspnetWebEventEvents> AspnetWebEventEvents { get; set; }
        public virtual DbSet<Attachment> Attachment { get; set; }
        public virtual DbSet<InstitutionAttachment> InstitutionAttachment { get; set; }
        public virtual DbSet<AutoApplicationStatusType> AutoApplicationStatusType { get; set; }
        public virtual DbSet<AutoEntranceTestList> AutoEntranceTestList { get; set; }
        public virtual DbSet<AutoOrderAgreement> AutoOrderAgreement { get; set; }
        public virtual DbSet<AutoOrderStage> AutoOrderStage { get; set; }
        public virtual DbSet<BansFromRon> BansFromRon { get; set; }
        public virtual DbSet<Bb> Bb { get; set; }
        public virtual DbSet<Benefit> Benefit { get; set; }
        public virtual DbSet<BenefitItemC> BenefitItemC { get; set; }
        public virtual DbSet<BenefitItemColympicType> BenefitItemColympicType { get; set; }
        public virtual DbSet<BenefitItemColympicTypeProfile> BenefitItemColympicTypeProfile { get; set; }
        public virtual DbSet<BenefitItemCprofile> BenefitItemCprofile { get; set; }
        public virtual DbSet<BenefitItemSubject> BenefitItemSubject { get; set; }
        public virtual DbSet<Benefitbudg> Benefitbudg { get; set; }
        public virtual DbSet<Benefitplat> Benefitplat { get; set; }
        public virtual DbSet<BulkAdmissionVolume> BulkAdmissionVolume { get; set; }
        public virtual DbSet<BulkApplication> BulkApplication { get; set; }
        public virtual DbSet<BulkApplicationCompetitiveGroupItem> BulkApplicationCompetitiveGroupItem { get; set; }
        public virtual DbSet<BulkApplicationEntranceTestDocument> BulkApplicationEntranceTestDocument { get; set; }
        public virtual DbSet<BulkApplicationIndividualAchievements> BulkApplicationIndividualAchievements { get; set; }
        public virtual DbSet<BulkApplicationSelectedCompetitiveGroup> BulkApplicationSelectedCompetitiveGroup { get; set; }
        public virtual DbSet<BulkApplicationSelectedCompetitiveGroupItem> BulkApplicationSelectedCompetitiveGroupItem { get; set; }
        public virtual DbSet<BulkApplicationSelectedCompetitiveGroupTarget> BulkApplicationSelectedCompetitiveGroupTarget { get; set; }
        public virtual DbSet<BulkApplicationShortUpdate> BulkApplicationShortUpdate { get; set; }
        public virtual DbSet<BulkBenefitItemC> BulkBenefitItemC { get; set; }
        public virtual DbSet<BulkBenefitItemData> BulkBenefitItemData { get; set; }
        public virtual DbSet<BulkCampaign> BulkCampaign { get; set; }
        public virtual DbSet<BulkCampaignDate> BulkCampaignDate { get; set; }
        public virtual DbSet<BulkCompetitiveGroup> BulkCompetitiveGroup { get; set; }
        public virtual DbSet<BulkCompetitiveGroupItem> BulkCompetitiveGroupItem { get; set; }
        public virtual DbSet<BulkCompetitiveGroupProgram> BulkCompetitiveGroupProgram { get; set; }
        public virtual DbSet<BulkCompetitiveGroupTarget> BulkCompetitiveGroupTarget { get; set; }
        public virtual DbSet<BulkCompetitiveGroupTargetItem> BulkCompetitiveGroupTargetItem { get; set; }
        public virtual DbSet<BulkDelete> BulkDelete { get; set; }
        public virtual DbSet<BulkDistributedAdmissionVolume> BulkDistributedAdmissionVolume { get; set; }
        public virtual DbSet<BulkEntranceTestItemC> BulkEntranceTestItemC { get; set; }
        public virtual DbSet<BulkEntrant> BulkEntrant { get; set; }
        public virtual DbSet<BulkEntrantDocument> BulkEntrantDocument { get; set; }
        public virtual DbSet<BulkEntrantDocumentSubject> BulkEntrantDocumentSubject { get; set; }
        public virtual DbSet<BulkInstitutionAchievements> BulkInstitutionAchievements { get; set; }
        public virtual DbSet<BulkInstitutionProgram> BulkInstitutionProgram { get; set; }
        public virtual DbSet<BulkOrderOfAdmission> BulkOrderOfAdmission { get; set; }
        public virtual DbSet<C> C { get; set; }
        public virtual DbSet<CTmp2016Xls> CTmp2016Xls { get; set; }
        public virtual DbSet<Campaign> Campaign { get; set; }
        public virtual DbSet<CampaignAdmissionStatus> CampaignAdmissionStatus { get; set; }
        public virtual DbSet<CampaignEducationLevel> CampaignEducationLevel { get; set; }
        public virtual DbSet<CampaignOrderDateCatalog> CampaignOrderDateCatalog { get; set; }
        public virtual DbSet<CampaignStatus> CampaignStatus { get; set; }
        public virtual DbSet<CampaignTypes> CampaignTypes { get; set; }
        public virtual DbSet<CgiToGzgu> CgiToGzgu { get; set; }
        public virtual DbSet<CityType> CityType { get; set; }
        public virtual DbSet<CompatriotCategory> CompatriotCategory { get; set; }
        public virtual DbSet<CompetitiveGroup> CompetitiveGroup { get; set; }
        public virtual DbSet<CompetitiveGroupItem> CompetitiveGroupItem { get; set; }
        public virtual DbSet<CompetitiveGroupProgram> CompetitiveGroupProgram { get; set; }
        public virtual DbSet<CompetitiveGroupTarget> CompetitiveGroupTarget { get; set; }
        public virtual DbSet<CompetitiveGroupTargetItem> CompetitiveGroupTargetItem { get; set; }
        public virtual DbSet<CompetitiveGroupToProgram> CompetitiveGroupToProgram { get; set; }
        public virtual DbSet<CompositionThemes> CompositionThemes { get; set; }
        public virtual DbSet<CompositionThemesApprob> CompositionThemesApprob { get; set; }
        public virtual DbSet<CompositionThemesOld> CompositionThemesOld { get; set; }
        public virtual DbSet<CountryDocuments> CountryDocuments { get; set; }
        public virtual DbSet<CountryType> CountryType { get; set; }
        public virtual DbSet<CourseSubject> CourseSubject { get; set; }
        public virtual DbSet<CourseType> CourseType { get; set; }
        public virtual DbSet<DbaIndexDefragLog> DbaIndexDefragLog { get; set; }
        public virtual DbSet<DeletedFromEge> DeletedFromEge { get; set; }
        public virtual DbSet<Direction> Direction { get; set; }
        public virtual DbSet<DirectionOld> DirectionOld { get; set; }
        public virtual DbSet<DirectionPlanKcp> DirectionPlanKcp { get; set; }
        public virtual DbSet<DirectionSubjectLink> DirectionSubjectLink { get; set; }
        public virtual DbSet<DirectionSubjectLinkDirection> DirectionSubjectLinkDirection { get; set; }
        public virtual DbSet<DirectionSubjectLinkSubject> DirectionSubjectLinkSubject { get; set; }
        public virtual DbSet<DirectionTemp> DirectionTemp { get; set; }
        public virtual DbSet<DirectionTmp> DirectionTmp { get; set; }
        public virtual DbSet<DirectionToDirection> DirectionToDirection { get; set; }
        public virtual DbSet<DirectionsFromEiis> DirectionsFromEiis { get; set; }
        public virtual DbSet<DisabilityType> DisabilityType { get; set; }
        public virtual DbSet<DistributedAdmissionVolume> DistributedAdmissionVolume { get; set; }
        public virtual DbSet<DistributedPlanAdmissionVolume> DistributedPlanAdmissionVolume { get; set; }
        public virtual DbSet<DocumentCategory> DocumentCategory { get; set; }
        public virtual DbSet<DocumentCheckStatus> DocumentCheckStatus { get; set; }
        public virtual DbSet<DocumentType> DocumentType { get; set; }
        public virtual DbSet<EduLevelDocumentType> EduLevelDocumentType { get; set; }
        public virtual DbSet<EduLevels> EduLevels { get; set; }
        public virtual DbSet<EduLevelsToCampaignTypes> EduLevelsToCampaignTypes { get; set; }
        public virtual DbSet<EduLevelsToUgsCode> EduLevelsToUgsCode { get; set; }
        public virtual DbSet<EduProgramTypes> EduProgramTypes { get; set; }
        public virtual DbSet<EducationForms> EducationForms { get; set; }
        public virtual DbSet<Egorova270Oh2016> Egorova270Oh2016 { get; set; }
        public virtual DbSet<Egorova270Oh2017> Egorova270Oh2017 { get; set; }
        public virtual DbSet<EgorovaInostran> EgorovaInostran { get; set; }
        public virtual DbSet<EgorovaOlimp> EgorovaOlimp { get; set; }
        public virtual DbSet<EgorovaOlimp1> EgorovaOlimp1 { get; set; }
        public virtual DbSet<EgorovaOlimp2016> EgorovaOlimp2016 { get; set; }
        public virtual DbSet<EgorovaOlimp2017> EgorovaOlimp2017 { get; set; }
        public virtual DbSet<EgorovaOlimpi2016oh> EgorovaOlimpi2016oh { get; set; }
        public virtual DbSet<EgorovaOlimpi2017oh> EgorovaOlimpi2017oh { get; set; }
        public virtual DbSet<EgorovaOlimpic2016> EgorovaOlimpic2016 { get; set; }
        public virtual DbSet<EgorovaOlimpic2016oh> EgorovaOlimpic2016oh { get; set; }
        public virtual DbSet<EgorovaUgs2016> EgorovaUgs2016 { get; set; }
        public virtual DbSet<EgorovaUgs2016270> EgorovaUgs2016270 { get; set; }
        public virtual DbSet<EgorovaUgs2016B1> EgorovaUgs2016B1 { get; set; }
        public virtual DbSet<EgorovaUgs2016B3> EgorovaUgs2016B3 { get; set; }
        public virtual DbSet<EgorovaZachOlimp> EgorovaZachOlimp { get; set; }
        public virtual DbSet<EgorovaZachOlimp270Osh2016> EgorovaZachOlimp270Osh2016 { get; set; }
        public virtual DbSet<EgorovaZachOlimp270Osh2017> EgorovaZachOlimp270Osh2017 { get; set; }
        public virtual DbSet<EgorovaZachOlimp270Vsosh2016> EgorovaZachOlimp270Vsosh2016 { get; set; }
        public virtual DbSet<EgorovaZachOlimp270Vsosh2017> EgorovaZachOlimp270Vsosh2017 { get; set; }
        public virtual DbSet<EgorovaZachOlimpB1> EgorovaZachOlimpB1 { get; set; }
        public virtual DbSet<EgorovaZachOlimpB2> EgorovaZachOlimpB2 { get; set; }
        public virtual DbSet<EgorovaZachOlimpB3> EgorovaZachOlimpB3 { get; set; }
        public virtual DbSet<El> El { get; set; }
        public virtual DbSet<EntHistAft> EntHistAft { get; set; }
        public virtual DbSet<EntHistBef> EntHistBef { get; set; }
        public virtual DbSet<EntUnlinked> EntUnlinked { get; set; }
        public virtual DbSet<EntranceTestCreativeDirection> EntranceTestCreativeDirection { get; set; }
        public virtual DbSet<EntranceTestItemC> EntranceTestItemC { get; set; }
        public virtual DbSet<EntranceTestProfileDirection> EntranceTestProfileDirection { get; set; }
        public virtual DbSet<EntranceTestResultSource> EntranceTestResultSource { get; set; }
        public virtual DbSet<EntranceTestType> EntranceTestType { get; set; }
        public virtual DbSet<Entrant> Entrant { get; set; }
        public virtual DbSet<Entrant1> Entrant1 { get; set; }
        public virtual DbSet<EntrantDocument> EntrantDocument { get; set; }
        public virtual DbSet<EntrantDocumentCompatriot> EntrantDocumentCompatriot { get; set; }
        public virtual DbSet<EntrantDocumentCustom> EntrantDocumentCustom { get; set; }
        public virtual DbSet<EntrantDocumentDisability> EntrantDocumentDisability { get; set; }
        public virtual DbSet<EntrantDocumentEdu> EntrantDocumentEdu { get; set; }
        public virtual DbSet<EntrantDocumentEge> EntrantDocumentEge { get; set; }
        public virtual DbSet<EntrantDocumentEgeAndOlympicSubject> EntrantDocumentEgeAndOlympicSubject { get; set; }
        public virtual DbSet<EntrantDocumentIdentity> EntrantDocumentIdentity { get; set; }
        public virtual DbSet<EntrantDocumentIdentityTmp> EntrantDocumentIdentityTmp { get; set; }
        public virtual DbSet<EntrantDocumentInternationalOlympic> EntrantDocumentInternationalOlympic { get; set; }
        public virtual DbSet<EntrantDocumentMarks> EntrantDocumentMarks { get; set; }
        public virtual DbSet<EntrantDocumentOlympic> EntrantDocumentOlympic { get; set; }
        public virtual DbSet<EntrantDocumentOrphan> EntrantDocumentOrphan { get; set; }
        public virtual DbSet<EntrantDocumentParentsLost> EntrantDocumentParentsLost { get; set; }
        public virtual DbSet<EntrantDocumentRadiationWork> EntrantDocumentRadiationWork { get; set; }
        public virtual DbSet<EntrantDocumentStateEmployee> EntrantDocumentStateEmployee { get; set; }
        public virtual DbSet<EntrantDocumentUkraineOlympic> EntrantDocumentUkraineOlympic { get; set; }
        public virtual DbSet<EntrantDocumentVeteran> EntrantDocumentVeteran { get; set; }
        public virtual DbSet<EntrantLanguage> EntrantLanguage { get; set; }
        public virtual DbSet<FbsToFisMap> FbsToFisMap { get; set; }
        public virtual DbSet<FinalStudent> FinalStudent { get; set; }
        public virtual DbSet<FinalStudentDuble> FinalStudentDuble { get; set; }
        public virtual DbSet<FinalTable> FinalTable { get; set; }
        public virtual DbSet<FinalTable2911> FinalTable2911 { get; set; }
        public virtual DbSet<FindPathTable> FindPathTable { get; set; }
        public virtual DbSet<FindPathTableEge> FindPathTableEge { get; set; }
        public virtual DbSet<FindPathTableShort> FindPathTableShort { get; set; }
        public virtual DbSet<ForeignInstitutions> ForeignInstitutions { get; set; }
        public virtual DbSet<ForeignLanguageType> ForeignLanguageType { get; set; }
        public virtual DbSet<FormOfLaw> FormOfLaw { get; set; }
        public virtual DbSet<FtcRandDouble> FtcRandDouble { get; set; }
        public virtual DbSet<GenderType> GenderType { get; set; }
        public virtual DbSet<GetBenefitComplectiveGroup> GetBenefitComplectiveGroup { get; set; }
        public virtual DbSet<GetCompetitiveGroup> GetCompetitiveGroup { get; set; }
        public virtual DbSet<GetCompetitiveGroupProgram> GetCompetitiveGroupProgram { get; set; }
        public virtual DbSet<GetEntranceTestItemC> GetEntranceTestItemC { get; set; }
        public virtual DbSet<GetInstitutionAchievements> GetInstitutionAchievements { get; set; }
        public virtual DbSet<GetInstitutionCampaign> GetInstitutionCampaign { get; set; }
        public virtual DbSet<GetOlympics> GetOlympics { get; set; }
        public virtual DbSet<GetOlympicsDiplomants> GetOlympicsDiplomants { get; set; }
        public virtual DbSet<GetOlympicsSubjects> GetOlympicsSubjects { get; set; }
        public virtual DbSet<GlobalMinEge> GlobalMinEge { get; set; }
        public virtual DbSet<Grands> Grands { get; set; }
        public virtual DbSet<Grants> Grants { get; set; }
        public virtual DbSet<GrantsNew> GrantsNew { get; set; }
        public virtual DbSet<Gzgu115> Gzgu115 { get; set; }
        public virtual DbSet<Gzgu1153> Gzgu1153 { get; set; }
        public virtual DbSet<Gzgu18> Gzgu18 { get; set; }
        public virtual DbSet<Gzgu36> Gzgu36 { get; set; }
        public virtual DbSet<Gzgu361> Gzgu361 { get; set; }
        public virtual DbSet<Gzgu362> Gzgu362 { get; set; }
        public virtual DbSet<Gzgu363> Gzgu363 { get; set; }
        public virtual DbSet<Gzgu364> Gzgu364 { get; set; }
        public virtual DbSet<GzguAetd> GzguAetd { get; set; }
        public virtual DbSet<GzguApp24> GzguApp24 { get; set; }
        public virtual DbSet<GzguApp8> GzguApp8 { get; set; }
        public virtual DbSet<GzguAppSources> GzguAppSources { get; set; }
        public virtual DbSet<GzguTempResult> GzguTempResult { get; set; }
        public virtual DbSet<GzguTempResult1> GzguTempResult1 { get; set; }
        public virtual DbSet<GzgubackFull2017> GzgubackFull2017 { get; set; }
        public virtual DbSet<GzgubackFull20172016> GzgubackFull20172016 { get; set; }
        public virtual DbSet<GzgubakFull> GzgubakFull { get; set; }
        public virtual DbSet<HandwrittingRegionRus> HandwrittingRegionRus { get; set; }
        public virtual DbSet<History> History { get; set; }
        public virtual DbSet<HseEge2017> HseEge2017 { get; set; }
        public virtual DbSet<HseEge2018> HseEge2018 { get; set; }
        public virtual DbSet<HseEge2018P> HseEge2018P { get; set; }
        public virtual DbSet<IdentityDocumentType> IdentityDocumentType { get; set; }
        public virtual DbSet<ImportPackage> ImportPackage { get; set; }
        public virtual DbSet<ImportPackage2017> ImportPackage2017 { get; set; }
        public virtual DbSet<ImportPackageForBrokenApps> ImportPackageForBrokenApps { get; set; }
        public virtual DbSet<ImportPackageParsed> ImportPackageParsed { get; set; }
        public virtual DbSet<ImportPackageParsedBack> ImportPackageParsedBack { get; set; }
        public virtual DbSet<ImportPackageStatus> ImportPackageStatus { get; set; }
        public virtual DbSet<ImportPackageType> ImportPackageType { get; set; }
        public virtual DbSet<IndividualAchievementsCategory> IndividualAchievementsCategory { get; set; }
        public virtual DbSet<IndividualAchivement> IndividualAchivement { get; set; }
        public virtual DbSet<Institution> Institution { get; set; }
        public virtual DbSet<InstitutionAccreditation> InstitutionAccreditation { get; set; }
        public virtual DbSet<InstitutionAchievements> InstitutionAchievements { get; set; }
        public virtual DbSet<InstitutionDirectionRequest> InstitutionDirectionRequest { get; set; }
        public virtual DbSet<InstitutionDocumentType> InstitutionDocumentType { get; set; }
        public virtual DbSet<InstitutionDocuments> InstitutionDocuments { get; set; }
        public virtual DbSet<InstitutionFounder> InstitutionFounder { get; set; }
        public virtual DbSet<InstitutionFounderToInstitutions> InstitutionFounderToInstitutions { get; set; }
        public virtual DbSet<InstitutionFounderType> InstitutionFounderType { get; set; }
        public virtual DbSet<InstitutionHistory> InstitutionHistory { get; set; }
        public virtual DbSet<InstitutionItem> InstitutionItem { get; set; }
        public virtual DbSet<InstitutionItemType> InstitutionItemType { get; set; }
        public virtual DbSet<InstitutionLicense> InstitutionLicense { get; set; }
        public virtual DbSet<InstitutionLicenseStatus> InstitutionLicenseStatus { get; set; }
        public virtual DbSet<InstitutionLicenseSupplement> InstitutionLicenseSupplement { get; set; }
        public virtual DbSet<InstitutionProgram> InstitutionProgram { get; set; }
        public virtual DbSet<InstitutionStructure> InstitutionStructure { get; set; }
        public virtual DbSet<InstitutionType> InstitutionType { get; set; }
        public virtual DbSet<LevelBudget> LevelBudget { get; set; }
        public virtual DbSet<LicensedDirection> LicensedDirection { get; set; }
        public virtual DbSet<LicensedDirectionStatus> LicensedDirectionStatus { get; set; }
        public virtual DbSet<Log> Log { get; set; }
        public virtual DbSet<MapAdmissionData> MapAdmissionData { get; set; }
        public virtual DbSet<MapAdmissionVolume> MapAdmissionVolume { get; set; }
        public virtual DbSet<MapApplication> MapApplication { get; set; }
        public virtual DbSet<MapApplicationSelectedCompetitiveGroupItem> MapApplicationSelectedCompetitiveGroupItem { get; set; }
        public virtual DbSet<MapCompetitiveGroupItem> MapCompetitiveGroupItem { get; set; }
        public virtual DbSet<MapCompetitiveGroupTargetItem> MapCompetitiveGroupTargetItem { get; set; }
        public virtual DbSet<MapDirections> MapDirections { get; set; }
        public virtual DbSet<MapEntranceTestProfileDirection> MapEntranceTestProfileDirection { get; set; }
        public virtual DbSet<MapInstitutionItem> MapInstitutionItem { get; set; }
        public virtual DbSet<MapInstitutionStructure> MapInstitutionStructure { get; set; }
        public virtual DbSet<MigrationHistory> MigrationHistory { get; set; }
        public virtual DbSet<Migrations> Migrations { get; set; }
        public virtual DbSet<MinScoreByRon> MinScoreByRon { get; set; }
        public virtual DbSet<MonitoringBy1199Order> MonitoringBy1199Order { get; set; }
        public virtual DbSet<MonitoringBy1199OrderKvk> MonitoringBy1199OrderKvk { get; set; }
        public virtual DbSet<MonitoringBy1199OrderList3> MonitoringBy1199OrderList3 { get; set; }
        public virtual DbSet<MonitoringBy1199OrderM> MonitoringBy1199OrderM { get; set; }
        public virtual DbSet<MonitoringPriem> MonitoringPriem { get; set; }
        public virtual DbSet<MonitoringPriemV2> MonitoringPriemV2 { get; set; }
        public virtual DbSet<MonitoringPriemV3> MonitoringPriemV3 { get; set; }
        public virtual DbSet<MonitoringPriemV31> MonitoringPriemV31 { get; set; }
        public virtual DbSet<MyStudents> MyStudents { get; set; }
        public virtual DbSet<MyStudents2911> MyStudents2911 { get; set; }
        public virtual DbSet<MyStudents29112> MyStudents29112 { get; set; }
        public virtual DbSet<MyStudents2911Back> MyStudents2911Back { get; set; }
        public virtual DbSet<MyStudentsBack> MyStudentsBack { get; set; }
        public virtual DbSet<NationalityType> NationalityType { get; set; }
        public virtual DbSet<NormativeDictionary> NormativeDictionary { get; set; }
        public virtual DbSet<NormativeVersionState> NormativeVersionState { get; set; }
        public virtual DbSet<NostrificationTypes> NostrificationTypes { get; set; }
        public virtual DbSet<Npsvuzbudg> Npsvuzbudg { get; set; }
        public virtual DbSet<Npsvuzplat> Npsvuzplat { get; set; }
        public virtual DbSet<Olympic1Ege> Olympic1Ege { get; set; }
        public virtual DbSet<OlympicClasses> OlympicClasses { get; set; }
        public virtual DbSet<OlympicDiplomType> OlympicDiplomType { get; set; }
        public virtual DbSet<OlympicDiplomant> OlympicDiplomant { get; set; }
        public virtual DbSet<OlympicDiplomantDocument> OlympicDiplomantDocument { get; set; }
        public virtual DbSet<OlympicDiplomantStatus> OlympicDiplomantStatus { get; set; }
        public virtual DbSet<OlympicLevel> OlympicLevel { get; set; }
        public virtual DbSet<OlympicProfile> OlympicProfile { get; set; }
        public virtual DbSet<OlympicSubject> OlympicSubject { get; set; }
        public virtual DbSet<OlympicType> OlympicType { get; set; }
        public virtual DbSet<OlympicTypeCopy> OlympicTypeCopy { get; set; }
        public virtual DbSet<OlympicTypeProfile> OlympicTypeProfile { get; set; }
        public virtual DbSet<OlympicsSubjects> OlympicsSubjects { get; set; }
        public virtual DbSet<OrdA> OrdA { get; set; }
        public virtual DbSet<OrdC> OrdC { get; set; }
        public virtual DbSet<OrderOfAdmission> OrderOfAdmission { get; set; }
        public virtual DbSet<OrderOfAdmissionHistory> OrderOfAdmissionHistory { get; set; }
        public virtual DbSet<OrderOfAdmissionStatus> OrderOfAdmissionStatus { get; set; }
        public virtual DbSet<OrderOfAdmissionType> OrderOfAdmissionType { get; set; }
        public virtual DbSet<OrphanCategory> OrphanCategory { get; set; }
        public virtual DbSet<OvzVtgFrom2016To201718062018> OvzVtgFrom2016To201718062018 { get; set; }
        public virtual DbSet<ParentDirection> ParentDirection { get; set; }
        public virtual DbSet<ParentDirectionOld> ParentDirectionOld { get; set; }
        public virtual DbSet<ParentDirectionPlanKcp> ParentDirectionPlanKcp { get; set; }
        public virtual DbSet<ParentDirectionTmp> ParentDirectionTmp { get; set; }
        public virtual DbSet<ParentsLostCategory> ParentsLostCategory { get; set; }
        public virtual DbSet<PartInv> PartInv { get; set; }
        public virtual DbSet<PartInv112019> PartInv112019 { get; set; }
        public virtual DbSet<PartInvGia9> PartInvGia9 { get; set; }
        public virtual DbSet<PartInvGia92019> PartInvGia92019 { get; set; }
        public virtual DbSet<PartInvGia9M> PartInvGia9M { get; set; }
        public virtual DbSet<PartInvGia9M2019> PartInvGia9M2019 { get; set; }
        public virtual DbSet<PartOlymp> PartOlymp { get; set; }
        public virtual DbSet<PartSkfo> PartSkfo { get; set; }
        public virtual DbSet<ParticipantsWithCompositionsHse2017> ParticipantsWithCompositionsHse2017 { get; set; }
        public virtual DbSet<ParticipantsWithCompositionsHse2017Spb978> ParticipantsWithCompositionsHse2017Spb978 { get; set; }
        public virtual DbSet<PersonalDataAccessLog> PersonalDataAccessLog { get; set; }
        public virtual DbSet<PlanAdmissionVolume> PlanAdmissionVolume { get; set; }
        public virtual DbSet<PreparatoryCourse> PreparatoryCourse { get; set; }
        public virtual DbSet<PrivateOrg> PrivateOrg { get; set; }
        public virtual DbSet<ProhibitionType> ProhibitionType { get; set; }
        public virtual DbSet<Query> Query { get; set; }
        public virtual DbSet<RadiationWorkCategory> RadiationWorkCategory { get; set; }
        public virtual DbSet<Rating> Rating { get; set; }
        public virtual DbSet<RatingList> RatingList { get; set; }
        public virtual DbSet<RecomendedLists> RecomendedLists { get; set; }
        public virtual DbSet<RecomendedListsHistory> RecomendedListsHistory { get; set; }
        public virtual DbSet<RegionType> RegionType { get; set; }
        public virtual DbSet<ReligiousUniver> ReligiousUniver { get; set; }
        public virtual DbSet<RequestComments> RequestComments { get; set; }
        public virtual DbSet<RequestDirection> RequestDirection { get; set; }
        public virtual DbSet<Russian> Russian { get; set; }
        public virtual DbSet<Russian10> Russian10 { get; set; }
        public virtual DbSet<Russian11> Russian11 { get; set; }
        public virtual DbSet<Russian12> Russian12 { get; set; }
        public virtual DbSet<Russian2> Russian2 { get; set; }
        public virtual DbSet<Russian3> Russian3 { get; set; }
        public virtual DbSet<Russian4> Russian4 { get; set; }
        public virtual DbSet<Russian5> Russian5 { get; set; }
        public virtual DbSet<Russian6> Russian6 { get; set; }
        public virtual DbSet<Russian7> Russian7 { get; set; }
        public virtual DbSet<Russian8> Russian8 { get; set; }
        public virtual DbSet<Russian9> Russian9 { get; set; }
        public virtual DbSet<RussianDop> RussianDop { get; set; }
        public virtual DbSet<RviPersonIdNew> RviPersonIdNew { get; set; }
        public virtual DbSet<RviPersonIdOld> RviPersonIdOld { get; set; }
        public virtual DbSet<RvidocumentTypes> RvidocumentTypes { get; set; }
        public virtual DbSet<RvipersonIdentDocs> RvipersonIdentDocs { get; set; }
        public virtual DbSet<RvipersonIdentDocsTmp> RvipersonIdentDocsTmp { get; set; }
        public virtual DbSet<Rvipersons> Rvipersons { get; set; }
        public virtual DbSet<RvipersonsTmp> RvipersonsTmp { get; set; }
        public virtual DbSet<SashaZ> SashaZ { get; set; }
        public virtual DbSet<SearchTmp> SearchTmp { get; set; }
        public virtual DbSet<Selo2017> Selo2017 { get; set; }
        public virtual DbSet<Selo2018> Selo2018 { get; set; }
        public virtual DbSet<Sochinen> Sochinen { get; set; }
        public virtual DbSet<Social1> Social1 { get; set; }
        public virtual DbSet<Social2> Social2 { get; set; }
        public virtual DbSet<StateEmployeeCategory> StateEmployeeCategory { get; set; }
        public virtual DbSet<Student1> Student1 { get; set; }
        public virtual DbSet<StudentFirst> StudentFirst { get; set; }
        public virtual DbSet<Subject> Subject { get; set; }
        public virtual DbSet<SubjectEgeMinValue> SubjectEgeMinValue { get; set; }
        public virtual DbSet<SubjectEgeMinValueCopy> SubjectEgeMinValueCopy { get; set; }
        public virtual DbSet<TemRodion> TemRodion { get; set; }
        public virtual DbSet<TempAndrei2018> TempAndrei2018 { get; set; }
        public virtual DbSet<TempApps> TempApps { get; set; }
        public virtual DbSet<TempEgedata> TempEgedata { get; set; }
        public virtual DbSet<TempGgv> TempGgv { get; set; }
        public virtual DbSet<TempIslodEsrpProd> TempIslodEsrpProd { get; set; }
        public virtual DbSet<TempOgedata> TempOgedata { get; set; }
        public virtual DbSet<TempPersonIdAll> TempPersonIdAll { get; set; }
        public virtual DbSet<TempPersonIdVioltionId1> TempPersonIdVioltionId1 { get; set; }
        public virtual DbSet<TempPopko> TempPopko { get; set; }
        public virtual DbSet<Tmp1> Tmp1 { get; set; }
        public virtual DbSet<TmpAnn> TmpAnn { get; set; }
        public virtual DbSet<TmpAnya2017> TmpAnya2017 { get; set; }
        public virtual DbSet<TmpCompositionsForViporganizations> TmpCompositionsForViporganizations { get; set; }
        public virtual DbSet<TmpEge> TmpEge { get; set; }
        public virtual DbSet<TmpEge1> TmpEge1 { get; set; }
        public virtual DbSet<TmpEge2> TmpEge2 { get; set; }
        public virtual DbSet<TmpEge3> TmpEge3 { get; set; }
        public virtual DbSet<TmpEiisFis> TmpEiisFis { get; set; }
        public virtual DbSet<TmpEntrantsGak> TmpEntrantsGak { get; set; }
        public virtual DbSet<TmpFisFbsRegion> TmpFisFbsRegion { get; set; }
        public virtual DbSet<TmpForRon2016> TmpForRon2016 { get; set; }
        public virtual DbSet<TmpFrdo2016> TmpFrdo2016 { get; set; }
        public virtual DbSet<TmpOlympicDiplomant> TmpOlympicDiplomant { get; set; }
        public virtual DbSet<TmpOlympicDiplomantDocument> TmpOlympicDiplomantDocument { get; set; }
        public virtual DbSet<TmpOlympicTypeN> TmpOlympicTypeN { get; set; }
        public virtual DbSet<TmpOlympicTypeProfile> TmpOlympicTypeProfile { get; set; }
        public virtual DbSet<TmpOvzEgorova> TmpOvzEgorova { get; set; }
        public virtual DbSet<TmpPartId> TmpPartId { get; set; }
        public virtual DbSet<TmpSirius16052018> TmpSirius16052018 { get; set; }
        public virtual DbSet<TmpSledstvieVedetNikita> TmpSledstvieVedetNikita { get; set; }
        public virtual DbSet<TmpUserPolicy> TmpUserPolicy { get; set; }
        public virtual DbSet<TmpYmPaPa> TmpYmPaPa { get; set; }
        public virtual DbSet<Top20> Top20 { get; set; }
        public virtual DbSet<TownType> TownType { get; set; }
        public virtual DbSet<TranslationRvidtIdentityDt> TranslationRvidtIdentityDt { get; set; }
        public virtual DbSet<UgsFromEiis> UgsFromEiis { get; set; }
        public virtual DbSet<UsedId> UsedId { get; set; }
        public virtual DbSet<UserPolicy> UserPolicy { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<VCompetitiveGroup> VCompetitiveGroup { get; set; }
        public virtual DbSet<VEntrantDocuments> VEntrantDocuments { get; set; }
        public virtual DbSet<VeteranCategory> VeteranCategory { get; set; }
        public virtual DbSet<Village> Village { get; set; }
        public virtual DbSet<Violation> Violation { get; set; }
        public virtual DbSet<ViolationApplicationReception> ViolationApplicationReception { get; set; }
        public virtual DbSet<ViolationType> ViolationType { get; set; }
        public virtual DbSet<VtgGve> VtgGve { get; set; }
        public virtual DbSet<VtgOvz> VtgOvz { get; set; }
        public virtual DbSet<Vuz21> Vuz21 { get; set; }
        public virtual DbSet<VwAspnetApplications> VwAspnetApplications { get; set; }
        public virtual DbSet<VwAspnetMembershipUsers> VwAspnetMembershipUsers { get; set; }
        public virtual DbSet<VwAspnetProfiles> VwAspnetProfiles { get; set; }
        public virtual DbSet<VwAspnetRoles> VwAspnetRoles { get; set; }
        public virtual DbSet<VwAspnetUsers> VwAspnetUsers { get; set; }
        public virtual DbSet<VwAspnetUsersInRoles> VwAspnetUsersInRoles { get; set; }
        public virtual DbSet<VwAspnetWebPartStatePaths> VwAspnetWebPartStatePaths { get; set; }
        public virtual DbSet<VwAspnetWebPartStateShared> VwAspnetWebPartStateShared { get; set; }
        public virtual DbSet<VwAspnetWebPartStateUser> VwAspnetWebPartStateUser { get; set; }
        public virtual DbSet<VwCampaign2015> VwCampaign2015 { get; set; }
        public virtual DbSet<VwCampaign2015Vo> VwCampaign2015Vo { get; set; }
        public virtual DbSet<VwCampaign2015withApps> VwCampaign2015withApps { get; set; }
        public virtual DbSet<VwCampaign2015withAppsVo> VwCampaign2015withAppsVo { get; set; }
        public virtual DbSet<VwCampaign2016> VwCampaign2016 { get; set; }
        public virtual DbSet<VwCampaign2016withApps> VwCampaign2016withApps { get; set; }
        public virtual DbSet<VwCampaign2017withApps> VwCampaign2017withApps { get; set; }
        public virtual DbSet<VwCampaign2018withApps> VwCampaign2018withApps { get; set; }
        public virtual DbSet<VwCampaign2019withApps> VwCampaign2019withApps { get; set; }
        public virtual DbSet<VwCompain2014> VwCompain2014 { get; set; }
        public virtual DbSet<VwCompain2014Vo> VwCompain2014Vo { get; set; }
        public virtual DbSet<VwCompain2014withApps> VwCompain2014withApps { get; set; }
        public virtual DbSet<VwCompain2014withAppsVo> VwCompain2014withAppsVo { get; set; }
        public virtual DbSet<WithoutBdate> WithoutBdate { get; set; }
        public virtual DbSet<WithoutMiddle> WithoutMiddle { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=10.0.18.3;Database=gvuz_start_2016_jun20;User ID=nshkonda;Password=QWEqwe123;Persist Security Info=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<A>(entity =>
            {
                entity.HasKey(e => e.AcgiId);

                entity.ToTable("_A");

                entity.Property(e => e.AcgiId)
                    .HasColumnName("acgiID")
                    .ValueGeneratedNever();

                entity.Property(e => e.ARank).HasColumnName("aRank");

                entity.Property(e => e.AcgiPriority).HasColumnName("ACGI_Priority");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.BezVi).HasColumnName("bezVI");

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");

                entity.Property(e => e.CampaignName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.CompetitiveGroupItemId).HasColumnName("CompetitiveGroupItemID");

                entity.Property(e => e.CompetitiveGroupName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CompetitiveGroupTargetId).HasColumnName("CompetitiveGroupTargetID");

                entity.Property(e => e.CompetitiveGroupTargetName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ConditionCode)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.DirectionCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DirectionId).HasColumnName("DirectionID");

                entity.Property(e => e.DirectionName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.EducationFormId).HasColumnName("EducationFormID");

                entity.Property(e => e.EducationFormName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.EducationLevelId).HasColumnName("EducationLevelID");

                entity.Property(e => e.EducationLevelName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.EducationSourceId).HasColumnName("EducationSourceID");

                entity.Property(e => e.EducationSourceName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.EntrantId).HasColumnName("EntrantID");

                entity.Property(e => e.Excluded).HasDefaultValueSql("((0))");

                entity.Property(e => e.Fio)
                    .HasColumnName("FIO")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.IncludeTo)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.MarkIa).HasColumnName("MarkIA");

                entity.Property(e => e.SRank)
                    .HasColumnName("sRank")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Aaa>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("AAA");

                entity.Property(e => e.Apps).HasColumnName("apps");

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.EgeBall).HasColumnName("egeBall");

                entity.Property(e => e.Konkurs).HasColumnName("konkurs");

                entity.Property(e => e.ParticipantId).HasColumnName("ParticipantID");

                entity.Property(e => e.Places).HasColumnName("places");

                entity.Property(e => e.ProhodnoyBall)
                    .HasColumnName("prohodnoyBall")
                    .HasColumnType("decimal(10, 4)");

                entity.Property(e => e.ИсточникФинансирования)
                    .IsRequired()
                    .HasColumnName("Источник финансирования")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.НаименованиеОо)
                    .HasColumnName("Наименование ОО")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.НаправлениеПодготовки)
                    .HasColumnName("Направление подготовки")
                    .HasMaxLength(551)
                    .IsUnicode(false);

                entity.Property(e => e.УровеньОбразования)
                    .IsRequired()
                    .HasColumnName("Уровень образования")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ФормаОбучения)
                    .IsRequired()
                    .HasColumnName("Форма обучения")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AbitInstitutionGeneral>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("abit_InstitutionGeneral");

                entity.Property(e => e.Email).HasMaxLength(2000);

                entity.Property(e => e.Fax).HasMaxLength(2000);

                entity.Property(e => e.FullName).IsRequired();

                entity.Property(e => e.Inn)
                    .HasColumnName("INN")
                    .HasMaxLength(50);

                entity.Property(e => e.InstType)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.IslodGuid)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Phone).HasMaxLength(2000);

                entity.Property(e => e.RegionCode)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.RegionName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Site).HasMaxLength(2000);
            });

            modelBuilder.Entity<AbitInstitutionIndAch>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("abit_InstitutionIndAch");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.Level)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MaxValue).HasColumnType("decimal(7, 4)");

                entity.Property(e => e.Name)
                    .HasMaxLength(4000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AbitInstitutionTests>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("abit_InstitutionTests");

                entity.Property(e => e.BudgetOz).HasColumnName("BudgetOZ");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.Level)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MaxScore).HasColumnType("decimal(7, 4)");

                entity.Property(e => e.MinScoreRon).HasColumnName("MinScoreRON");

                entity.Property(e => e.PaidOz).HasColumnName("PaidOZ");

                entity.Property(e => e.QuotaOz).HasColumnName("QuotaOZ");

                entity.Property(e => e.SpecCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SpecName)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Subject)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.TargetOz).HasColumnName("TargetOZ");

                entity.Property(e => e.TestType)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AbitInstitutionVolAndStruct>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("abit_InstitutionVolAndStruct");

                entity.Property(e => e.AppBudgetO).HasColumnName("App_Budget_O");

                entity.Property(e => e.AppBudgetOz).HasColumnName("App_Budget_OZ");

                entity.Property(e => e.AppBudgetZ).HasColumnName("App_Budget_Z");

                entity.Property(e => e.AppPaidO).HasColumnName("App_Paid_O");

                entity.Property(e => e.AppPaidOz).HasColumnName("App_Paid_OZ");

                entity.Property(e => e.AppPaidZ).HasColumnName("App_Paid_Z");

                entity.Property(e => e.AppQuotaO).HasColumnName("App_Quota_O");

                entity.Property(e => e.AppQuotaOz).HasColumnName("App_Quota_OZ");

                entity.Property(e => e.AppQuotaZ).HasColumnName("App_Quota_Z");

                entity.Property(e => e.AppTargetO).HasColumnName("App_Target_O");

                entity.Property(e => e.AppTargetOz).HasColumnName("App_Target_OZ");

                entity.Property(e => e.AppTargetZ).HasColumnName("App_Target_Z");

                entity.Property(e => e.BudgetO).HasColumnName("Budget_O");

                entity.Property(e => e.BudgetOz).HasColumnName("Budget_OZ");

                entity.Property(e => e.BudgetZ).HasColumnName("Budget_Z");

                entity.Property(e => e.ContestBudgetO).HasColumnName("Contest_Budget_O");

                entity.Property(e => e.ContestBudgetOz).HasColumnName("Contest_Budget_OZ");

                entity.Property(e => e.ContestBudgetZ).HasColumnName("Contest_Budget_Z");

                entity.Property(e => e.ContestPaidO).HasColumnName("Contest_Paid_O");

                entity.Property(e => e.ContestPaidOz).HasColumnName("Contest_Paid_OZ");

                entity.Property(e => e.ContestPaidZ).HasColumnName("Contest_Paid_Z");

                entity.Property(e => e.ContestQuotaO).HasColumnName("Contest_Quota_O");

                entity.Property(e => e.ContestQuotaOz).HasColumnName("Contest_Quota_OZ");

                entity.Property(e => e.ContestQuotaZ).HasColumnName("Contest_Quota_Z");

                entity.Property(e => e.ContestTargetO).HasColumnName("Contest_Target_O");

                entity.Property(e => e.ContestTargetOz).HasColumnName("Contest_Target_OZ");

                entity.Property(e => e.ContestTargetZ).HasColumnName("Contest_Target_Z");

                entity.Property(e => e.CurrentScoreBudgetO).HasColumnName("CurrentScore_Budget_O");

                entity.Property(e => e.CurrentScoreBudgetOz).HasColumnName("CurrentScore_Budget_OZ");

                entity.Property(e => e.CurrentScoreBudgetZ).HasColumnName("CurrentScore_Budget_Z");

                entity.Property(e => e.CurrentScorePaidO).HasColumnName("CurrentScore_Paid_O");

                entity.Property(e => e.CurrentScorePaidOz).HasColumnName("CurrentScore_Paid_OZ");

                entity.Property(e => e.CurrentScorePaidZ).HasColumnName("CurrentScore_Paid_Z");

                entity.Property(e => e.CurrentScoreQuotaO).HasColumnName("CurrentScore_Quota_O");

                entity.Property(e => e.CurrentScoreQuotaOz).HasColumnName("CurrentScore_Quota_OZ");

                entity.Property(e => e.CurrentScoreQuotaZ).HasColumnName("CurrentScore_Quota_Z");

                entity.Property(e => e.CurrentScoreTargetO).HasColumnName("CurrentScore_Target_O");

                entity.Property(e => e.CurrentScoreTargetOz).HasColumnName("CurrentScore_Target_OZ");

                entity.Property(e => e.CurrentScoreTargetZ).HasColumnName("CurrentScore_Target_Z");

                entity.Property(e => e.EnrolledBudgetO).HasColumnName("Enrolled_Budget_O");

                entity.Property(e => e.EnrolledBudgetOz).HasColumnName("Enrolled_Budget_OZ");

                entity.Property(e => e.EnrolledBudgetZ).HasColumnName("Enrolled_Budget_Z");

                entity.Property(e => e.EnrolledPaidO).HasColumnName("Enrolled_Paid_O");

                entity.Property(e => e.EnrolledPaidOz).HasColumnName("Enrolled_Paid_OZ");

                entity.Property(e => e.EnrolledPaidZ).HasColumnName("Enrolled_Paid_Z");

                entity.Property(e => e.EnrolledQuotaO).HasColumnName("Enrolled_Quota_O");

                entity.Property(e => e.EnrolledQuotaOz).HasColumnName("Enrolled_Quota_OZ");

                entity.Property(e => e.EnrolledQuotaZ).HasColumnName("Enrolled_Quota_Z");

                entity.Property(e => e.EnrolledTargetO).HasColumnName("Enrolled_Target_O");

                entity.Property(e => e.EnrolledTargetOz).HasColumnName("Enrolled_Target_OZ");

                entity.Property(e => e.EnrolledTargetZ).HasColumnName("Enrolled_Target_Z");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.Level)
                    .IsRequired()
                    .HasColumnName("level")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LevelId).HasColumnName("levelID");

                entity.Property(e => e.NewSpecCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PaidO).HasColumnName("Paid_O");

                entity.Property(e => e.PaidOz).HasColumnName("Paid_OZ");

                entity.Property(e => e.PaidZ).HasColumnName("Paid_Z");

                entity.Property(e => e.ParentDirCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ParentDirId).HasColumnName("ParentDirID");

                entity.Property(e => e.ParentDirName)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.QuotaO).HasColumnName("Quota_O");

                entity.Property(e => e.QuotaOz).HasColumnName("Quota_OZ");

                entity.Property(e => e.QuotaZ).HasColumnName("Quota_Z");

                entity.Property(e => e.SpecCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SpecName)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.TargetO).HasColumnName("Target_O");

                entity.Property(e => e.TargetOz).HasColumnName("Target_OZ");

                entity.Property(e => e.TargetZ).HasColumnName("Target_Z");
            });

            modelBuilder.Entity<ActualId>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("actualId");

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.Property(e => e.AddressId).HasColumnName("AddressID");

                entity.Property(e => e.Building)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.BuildingPart)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CityName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.CountryName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.RegionId).HasColumnName("RegionID");

                entity.Property(e => e.RegionName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Room)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Street)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Address)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Address_CountryType");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.Address)
                    .HasForeignKey(d => d.RegionId)
                    .HasConstraintName("FK_Address_RegionType");
            });

            modelBuilder.Entity<AdmissionData>(entity =>
            {
                entity.HasKey(e => e.AdmissionStructureId)
                    .IsClustered(false);

                entity.HasIndex(e => e.AdmissionItemId)
                    .HasName("UK_AdmissionData_Item")
                    .IsUnique();

                entity.HasIndex(e => e.Lineage)
                    .HasName("UK_AdmissionData_Lineage")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.AdmissionStructureId)
                    .HasColumnName("AdmissionStructureID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AdmissionItemId).HasColumnName("AdmissionItemID");

                entity.Property(e => e.AdmissionTypeId).HasColumnName("AdmissionTypeID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DirectionId).HasColumnName("DirectionID");

                entity.Property(e => e.EducationLevelId).HasColumnName("EducationLevelID");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.ItemTypeId).HasColumnName("ItemTypeID");

                entity.Property(e => e.Lineage)
                    .IsRequired()
                    .HasMaxLength(146)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ParentId).HasColumnName("ParentID");

                entity.Property(e => e.StudyId).HasColumnName("StudyID");
            });

            modelBuilder.Entity<AdmissionItemType>(entity =>
            {
                entity.HasKey(e => e.ItemTypeId);

                entity.HasIndex(e => e.Name)
                    .HasName("UK_AdmissionItemType_Name")
                    .IsUnique();

                entity.Property(e => e.ItemTypeId)
                    .HasColumnName("ItemTypeID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Alias).HasMaxLength(20);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AdmissionRules>(entity =>
            {
                entity.HasKey(e => e.RecordId)
                    .HasName("PK_AdmissionRules_1");

                entity.Property(e => e.RecordId).HasColumnName("RecordID");

                entity.Property(e => e.ChangeDate).HasColumnType("datetime");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.File).IsRequired();

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.MimeType)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AdmissionVolume>(entity =>
            {
                entity.HasIndex(e => e.CampaignId)
                    .HasName("I_AdmissionVolume_CampaignID");

                entity.HasIndex(e => e.InstitutionId)
                    .HasName("I_AdmissionVolume_InstitutionID");

                entity.HasIndex(e => new { e.NumberTargetO, e.NumberTargetOz, e.NumberTargetZ, e.NumberQuotaO, e.NumberQuotaOz, e.NumberQuotaZ, e.NumberBudgetO, e.NumberBudgetOz, e.NumberBudgetZ, e.NumberPaidO, e.NumberPaidOz, e.NumberPaidZ, e.CreatedDate })
                    .HasName("I_AdmissionVolume_CreatedDate");

                entity.Property(e => e.AdmissionVolumeId).HasColumnName("AdmissionVolumeID");

                entity.Property(e => e.AdmissionItemTypeId).HasColumnName("AdmissionItemTypeID");

                entity.Property(e => e.AdmissionVolumeGuid).HasColumnName("AdmissionVolumeGUID");

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");

                entity.Property(e => e.Course).HasDefaultValueSql("((1))");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DirectionId).HasColumnName("DirectionID");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NumberBudgetOz).HasColumnName("NumberBudgetOZ");

                entity.Property(e => e.NumberPaidOz).HasColumnName("NumberPaidOZ");

                entity.Property(e => e.NumberQuotaOz).HasColumnName("NumberQuotaOZ");

                entity.Property(e => e.NumberTargetOz).HasColumnName("NumberTargetOZ");

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.AdmissionItemType)
                    .WithMany(p => p.AdmissionVolume)
                    .HasForeignKey(d => d.AdmissionItemTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AdmissionVolume_AdmissionItemType");

                entity.HasOne(d => d.Campaign)
                    .WithMany(p => p.AdmissionVolume)
                    .HasForeignKey(d => d.CampaignId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_AdmissionVolume_Campaign");

                entity.HasOne(d => d.Direction)
                    .WithMany(p => p.AdmissionVolume)
                    .HasForeignKey(d => d.DirectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AdmissionVolume_Direction");

                entity.HasOne(d => d.Institution)
                    .WithMany(p => p.AdmissionVolume)
                    .HasForeignKey(d => d.InstitutionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AdmissionVolume_Institution");
            });

            modelBuilder.Entity<AdmissionVolumeKcp>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("AdmissionVolumeKCP");

                entity.Property(e => e.AdmissionItemTypeId).HasColumnName("AdmissionItemTypeID");

                entity.Property(e => e.AdmissionVolumeId).HasColumnName("AdmissionVolumeID");

                entity.Property(e => e.DirectionId)
                    .HasColumnName("DirectionID")
                    .HasMaxLength(255);

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.NumberBudgetOz).HasColumnName("NumberBudgetOZ");
            });

            modelBuilder.Entity<AetdTmp>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("aetd_tmp");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e._1).HasColumnName("1");

                entity.Property(e => e._2).HasColumnName("2");

                entity.Property(e => e._3).HasColumnName("3");

                entity.Property(e => e._4).HasColumnName("4");
            });

            modelBuilder.Entity<AllowedDirectionStatus>(entity =>
            {
                entity.Property(e => e.AllowedDirectionStatusId)
                    .HasColumnName("AllowedDirectionStatusID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AllowedDirections>(entity =>
            {
                entity.HasIndex(e => e.InstitutionId)
                    .HasName("I_AllowedDirections_InstitutionID");

                entity.Property(e => e.AdmissionItemTypeId).HasColumnName("AdmissionItemTypeID");

                entity.Property(e => e.AllowedDirectionStatusId).HasColumnName("AllowedDirectionStatusID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DirectionId).HasColumnName("DirectionID");

                entity.Property(e => e.EiisId)
                    .HasColumnName("EIIS_ID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.EsrpId).HasColumnName("Esrp_ID");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.AdmissionItemType)
                    .WithMany(p => p.AllowedDirections)
                    .HasForeignKey(d => d.AdmissionItemTypeId)
                    .HasConstraintName("FK_AllowedDirections_AdmissionItemType");

                entity.HasOne(d => d.AllowedDirectionStatus)
                    .WithMany(p => p.AllowedDirections)
                    .HasForeignKey(d => d.AllowedDirectionStatusId)
                    .HasConstraintName("FK_AllowedDirections_StatusAllowedDirectionsEsrp");

                entity.HasOne(d => d.Direction)
                    .WithMany(p => p.AllowedDirections)
                    .HasForeignKey(d => d.DirectionId)
                    .HasConstraintName("FK_AllowedDirections_Direction");

                entity.HasOne(d => d.Institution)
                    .WithMany(p => p.AllowedDirections)
                    .HasForeignKey(d => d.InstitutionId)
                    .HasConstraintName("FK_AllowedDirections_Institution");
            });

            modelBuilder.Entity<Allsubjects>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("allsubjects");

                entity.Property(e => e.Hash).HasMaxLength(1000);

                entity.Property(e => e.Participant).HasMaxLength(1000);
            });

            modelBuilder.Entity<AppealStatus>(entity =>
            {
                entity.Property(e => e.AppealStatusId)
                    .HasColumnName("AppealStatusID")
                    .ValueGeneratedNever();

                entity.Property(e => e.StatusName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Application>(entity =>
            {
                entity.HasIndex(e => e.ApplicationNumber)
                    .HasName("I_Application_ApplicationNumber");

                entity.HasIndex(e => e.CreatedDate)
                    .HasName("I_Application_CreatedDate");

                entity.HasIndex(e => e.EntrantId)
                    .HasName("I_Application_EntrantID");

                entity.HasIndex(e => e.InstitutionId)
                    .HasName("I_Application_InstitutionID");

                entity.HasIndex(e => e.RegistrationDate)
                    .HasName("I_Application_RegistrationDate");

                entity.HasIndex(e => new { e.ApplicationId, e.OrderOfAdmissionId })
                    .HasName("idx_Application_OrderOfAdmissionID");

                entity.HasIndex(e => new { e.ApplicationId, e.RegistrationDate, e.StatusId })
                    .HasName("<Name of Missing Index, sysname,>");

                entity.HasIndex(e => new { e.EntrantId, e.ApplicationId, e.StatusId })
                    .HasName("IDX_APP_Query_Perfomance_N1849553");

                entity.HasIndex(e => new { e.RegistrationDate, e.ApplicationId, e.StatusId })
                    .HasName("application_ind2");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.ApplicationForcedAdmissionReasonsId).HasColumnName("ApplicationForcedAdmissionReasonsID");

                entity.Property(e => e.ApplicationGuid).HasColumnName("ApplicationGUID");

                entity.Property(e => e.ApplicationNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DistantPlace)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.EntrantId).HasColumnName("EntrantID");

                entity.Property(e => e.IndividualAchivementsMark).HasColumnType("decimal(10, 4)");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.IsDisabledDocumentId).HasColumnName("IsDisabledDocumentID");

                entity.Property(e => e.IsRequiresBudgetOz).HasColumnName("IsRequiresBudgetOZ");

                entity.Property(e => e.IsRequiresPaidOz).HasColumnName("IsRequiresPaidOZ");

                entity.Property(e => e.IsRequiresTargetOz).HasColumnName("IsRequiresTargetOZ");

                entity.Property(e => e.LastCheckDate).HasColumnType("datetime");

                entity.Property(e => e.LastDenyDate).HasColumnType("datetime");

                entity.Property(e => e.LastEgeDocumentsCheckDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.OrderCalculatedBenefitId).HasColumnName("OrderCalculatedBenefitID");

                entity.Property(e => e.OrderCalculatedRating)
                    .HasColumnType("decimal(10, 4)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.OrderCompetitiveGroupId).HasColumnName("OrderCompetitiveGroupID");

                entity.Property(e => e.OrderCompetitiveGroupItemId).HasColumnName("OrderCompetitiveGroupItemID");

                entity.Property(e => e.OrderCompetitiveGroupTargetId).HasColumnName("OrderCompetitiveGroupTargetID");

                entity.Property(e => e.OrderEducationFormId).HasColumnName("OrderEducationFormID");

                entity.Property(e => e.OrderEducationSourceId).HasColumnName("OrderEducationSourceID");

                entity.Property(e => e.OrderOfAdmissionId).HasColumnName("OrderOfAdmissionID");

                entity.Property(e => e.OriginalDocumentsReceivedDate).HasColumnType("datetime");

                entity.Property(e => e.PublishDate).HasColumnType("datetime");

                entity.Property(e => e.RegistrationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ReturnDocumentsDate).HasColumnType("datetime");

                entity.Property(e => e.SourceId)
                    .HasColumnName("SourceID")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.StatusDecision).IsUnicode(false);

                entity.Property(e => e.StatusId)
                    .HasColumnName("StatusID")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ViolationErrors).IsUnicode(false);

                entity.Property(e => e.ViolationId).HasColumnName("ViolationID");

                entity.Property(e => e.WizardStepId).HasColumnName("WizardStepID");

                entity.HasOne(d => d.ApplicationForcedAdmissionReasons)
                    .WithMany(p => p.Application)
                    .HasForeignKey(d => d.ApplicationForcedAdmissionReasonsId)
                    .HasConstraintName("FK__Applicati__Appli__75586032");

                entity.HasOne(d => d.Entrant)
                    .WithMany(p => p.Application)
                    .HasForeignKey(d => d.EntrantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Application_Entrant");

                entity.HasOne(d => d.Institution)
                    .WithMany(p => p.Application)
                    .HasForeignKey(d => d.InstitutionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Application_Institution");

                entity.HasOne(d => d.IsDisabledDocument)
                    .WithMany(p => p.Application)
                    .HasForeignKey(d => d.IsDisabledDocumentId)
                    .HasConstraintName("FK_Application_IsDisabledEntrantDocument");

                entity.HasOne(d => d.OrderCalculatedBenefit)
                    .WithMany(p => p.Application)
                    .HasForeignKey(d => d.OrderCalculatedBenefitId)
                    .HasConstraintName("FK_Application_Benefit");

                entity.HasOne(d => d.OrderCompetitiveGroup)
                    .WithMany(p => p.Application)
                    .HasForeignKey(d => d.OrderCompetitiveGroupId)
                    .HasConstraintName("FK_Application_CompetitiveGroup");

                entity.HasOne(d => d.OrderCompetitiveGroupTarget)
                    .WithMany(p => p.Application)
                    .HasForeignKey(d => d.OrderCompetitiveGroupTargetId)
                    .HasConstraintName("FK_Application_CompetitiveGroupTarget");

                entity.HasOne(d => d.OrderEducationForm)
                    .WithMany(p => p.ApplicationOrderEducationForm)
                    .HasForeignKey(d => d.OrderEducationFormId)
                    .HasConstraintName("FK_Application_AdmissionItemTypeForm");

                entity.HasOne(d => d.OrderEducationSource)
                    .WithMany(p => p.ApplicationOrderEducationSource)
                    .HasForeignKey(d => d.OrderEducationSourceId)
                    .HasConstraintName("FK_Application_AdmissionItemTypeSource");

                entity.HasOne(d => d.OrderIdLevelBudgetNavigation)
                    .WithMany(p => p.Application)
                    .HasForeignKey(d => d.OrderIdLevelBudget)
                    .HasConstraintName("FK_Application_LevelBudge");

                entity.HasOne(d => d.OrderOfAdmission)
                    .WithMany(p => p.Application)
                    .HasForeignKey(d => d.OrderOfAdmissionId)
                    .HasConstraintName("FK_Application_OrderOfAdmission");

                entity.HasOne(d => d.ReturnDocumentsType)
                    .WithMany(p => p.Application)
                    .HasForeignKey(d => d.ReturnDocumentsTypeId)
                    .HasConstraintName("FK_Application_ApplicationReturnDocumentsType");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Application)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Application_ApplicationStatusType");

                entity.HasOne(d => d.Violation)
                    .WithMany(p => p.Application)
                    .HasForeignKey(d => d.ViolationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Application_ViolationType");
            });

            modelBuilder.Entity<ApplicationCheckStatus>(entity =>
            {
                entity.HasKey(e => e.StatusId)
                    .HasName("PK__Applicat__C8EE20434AB81AF0");

                entity.Property(e => e.StatusId)
                    .HasColumnName("StatusID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ApplicationCompetitiveGroupItem>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .IsClustered(false);

                entity.HasIndex(e => e.ApplicationId)
                    .IsClustered();

                entity.HasIndex(e => e.CompetitiveGroupId)
                    .HasName("I_ApplicationCompetitiveGroupItem_CompetitiveGroupId");

                entity.HasIndex(e => e.CompetitiveGroupItemId)
                    .HasName("I_ApplicationCompetitiveGroupItem_CompetitiveGroupItemId");

                entity.HasIndex(e => e.OrderOfAdmissionId)
                    .HasName("I_ApplicationCompetitiveGroupItem_OrderOfAdmissionID");

                entity.HasIndex(e => e.OrderOfExceptionId)
                    .HasName("I_ApplicationCompetitiveGroupItem_OrderOfExceptionID");

                entity.HasIndex(e => new { e.ApplicationId, e.CompetitiveGroupId, e.OrderOfExceptionId, e.OrderOfAdmissionId })
                    .HasName("ApplicationCompetitiveGroupItem_Orders");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AdmissionDate).HasColumnType("datetime");

                entity.Property(e => e.CalculatedRating).HasColumnType("decimal(10, 4)");

                entity.Property(e => e.ExceptionDate).HasColumnType("datetime");

                entity.Property(e => e.IsAgreedDate).HasColumnType("datetime");

                entity.Property(e => e.IsDisagreedDate).HasColumnType("datetime");

                entity.Property(e => e.IsForSpoandVo).HasColumnName("IsForSPOandVO");

                entity.Property(e => e.OrderBenefitId).HasColumnName("OrderBenefitID");

                entity.Property(e => e.OrderOfAdmissionId).HasColumnName("OrderOfAdmissionID");

                entity.Property(e => e.OrderOfExceptionId).HasColumnName("OrderOfExceptionID");

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.ApplicationCompetitiveGroupItem)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApplicationCompetitiveGroupItem_Application");

                entity.HasOne(d => d.CompetitiveGroup)
                    .WithMany(p => p.ApplicationCompetitiveGroupItem)
                    .HasForeignKey(d => d.CompetitiveGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApplicationCompetitiveGroupItem_CompetitiveGroup");

                entity.HasOne(d => d.CompetitiveGroupItem)
                    .WithMany(p => p.ApplicationCompetitiveGroupItem)
                    .HasForeignKey(d => d.CompetitiveGroupItemId)
                    .HasConstraintName("FK_ApplicationCompetitiveGroupItem_CompetitiveGroupItem");

                entity.HasOne(d => d.OrderBenefit)
                    .WithMany(p => p.ApplicationCompetitiveGroupItem)
                    .HasForeignKey(d => d.OrderBenefitId)
                    .HasConstraintName("FK_ApplicationCompetitiveGroupItem_OrderBenefitID");

                entity.HasOne(d => d.OrderIdLevelBudgetNavigation)
                    .WithMany(p => p.ApplicationCompetitiveGroupItem)
                    .HasForeignKey(d => d.OrderIdLevelBudget)
                    .HasConstraintName("FK_ApplicationCompetitiveGroupItem_OrderIdLevelBudget");

                entity.HasOne(d => d.OrderOfAdmission)
                    .WithMany(p => p.ApplicationCompetitiveGroupItemOrderOfAdmission)
                    .HasForeignKey(d => d.OrderOfAdmissionId)
                    .HasConstraintName("FK_ApplicationCompetitiveGroupItem_OrderOfAdmission");

                entity.HasOne(d => d.OrderOfException)
                    .WithMany(p => p.ApplicationCompetitiveGroupItemOrderOfException)
                    .HasForeignKey(d => d.OrderOfExceptionId)
                    .HasConstraintName("FK_ApplicationCompetitiveGroupItem_OrderOfException");
            });

            modelBuilder.Entity<ApplicationCompositionResults>(entity =>
            {
                entity.HasKey(e => e.CompositionId)
                    .HasName("PK__Applicat__B8E2333F2ED0D4B0");

                entity.HasIndex(e => e.ApplicationId)
                    .HasName("I_ApplicationCompositionResults_ApplicationID");

                entity.HasIndex(e => new { e.ApplicationId, e.DownloadDate })
                    .HasName("I_ApplicationCompositionResults_DownloadDate");

                entity.Property(e => e.CompositionId)
                    .HasColumnName("CompositionID")
                    .HasComment("ИД записи");

                entity.Property(e => e.AppealStatus)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ApplicationId)
                    .HasColumnName("ApplicationID")
                    .HasComment("ИД заявления FK (Application - Application-ID)");

                entity.Property(e => e.CompositionPaths).IsUnicode(false);

                entity.Property(e => e.DownloadDate).HasColumnType("datetime");

                entity.Property(e => e.ExamDate).HasColumnType("datetime");

                entity.Property(e => e.SourceId)
                    .IsRequired()
                    .HasColumnName("SourceID")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Ссылка на запись в Системе- источнике результатов сочинений");

                entity.Property(e => e.ThemeId)
                    .HasColumnName("ThemeID")
                    .HasComment("Тема сочинения.  FK (CompositionThemes – ThemeID)");

                entity.Property(e => e.Year)
                    .HasColumnType("datetime")
                    .HasComment("Год результата сочинения");

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.ApplicationCompositionResults)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApplicationCompositionResults_Application");

                entity.HasOne(d => d.Theme)
                    .WithMany(p => p.ApplicationCompositionResults)
                    .HasForeignKey(d => d.ThemeId)
                    .HasConstraintName("FK_ApplicationCompositionResults_ThemeID");
            });

            modelBuilder.Entity<ApplicationCompositionResultsApprob>(entity =>
            {
                entity.HasKey(e => e.CompositionId);

                entity.ToTable("ApplicationCompositionResults_Approb");

                entity.Property(e => e.CompositionId).HasColumnName("CompositionID");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.FileName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ThemeId).HasColumnName("ThemeID");

                entity.Property(e => e.Year).HasColumnType("datetime");

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.ApplicationCompositionResultsApprob)
                    .HasForeignKey(d => d.ApplicationId)
                    .HasConstraintName("FK_ApplicationCompositionResults_Approb_Application");

                entity.HasOne(d => d.Theme)
                    .WithMany(p => p.ApplicationCompositionResultsApprob)
                    .HasForeignKey(d => d.ThemeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApplicationCompositionResults_Approb_CompositionThemes_Approb");
            });

            modelBuilder.Entity<ApplicationCompositionResultsTmp>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ApplicationCompositionResults_TMP");

                entity.Property(e => e.AppealStatus)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.CompositionId).HasColumnName("CompositionID");

                entity.Property(e => e.CompositionPaths).IsUnicode(false);

                entity.Property(e => e.DownloadDate).HasColumnType("datetime");

                entity.Property(e => e.ExamDate).HasColumnType("datetime");

                entity.Property(e => e.SourceId)
                    .IsRequired()
                    .HasColumnName("SourceID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ThemeId).HasColumnName("ThemeID");

                entity.Property(e => e.Year).HasColumnType("datetime");
            });

            modelBuilder.Entity<ApplicationConsidered>(entity =>
            {
                entity.Property(e => e.ApplicationConsideredId).HasColumnName("ApplicationConsideredID");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DirectionId).HasColumnName("DirectionID");

                entity.Property(e => e.EducationFormId).HasColumnName("EducationFormID");

                entity.Property(e => e.EducationLevelId).HasColumnName("EducationLevelID");

                entity.Property(e => e.FinanceSourceId).HasColumnName("FinanceSourceID");

                entity.Property(e => e.IsRequiresBudgetOz).HasColumnName("IsRequiresBudgetOZ");

                entity.Property(e => e.IsRequiresPaidOz).HasColumnName("IsRequiresPaidOZ");

                entity.Property(e => e.IsRequiresTargetOz).HasColumnName("IsRequiresTargetOZ");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.ApplicationConsidered)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApplicationConsidered_Application");

                entity.HasOne(d => d.Direction)
                    .WithMany(p => p.ApplicationConsidered)
                    .HasForeignKey(d => d.DirectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApplicationConsidered_Direction");

                entity.HasOne(d => d.EducationLevel)
                    .WithMany(p => p.ApplicationConsidered)
                    .HasForeignKey(d => d.EducationLevelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApplicationConsidered_AdmissionItemType");
            });

            modelBuilder.Entity<ApplicationEntranceTestDocument>(entity =>
            {
                entity.HasIndex(e => e.ApplicationId)
                    .HasName("I_ApplicationEntranceTestDocument_ApplicationID");

                entity.HasIndex(e => e.CompetitiveGroupId)
                    .HasName("I_ApplicationEntranceTestDocument_CompetitiveGroupID");

                entity.HasIndex(e => e.EntranceTestItemId)
                    .HasName("I_ApplicationEntranceTestDocument_EntranceTestItemID");

                entity.HasIndex(e => e.EntrantDocumentId)
                    .HasName("I_ApplicationEntranceTestDocument_EntrantDocumentID");

                entity.HasIndex(e => new { e.ApplicationId, e.BenefitId })
                    .HasName("ApplicationEntranceTestDocument_BenefitID");

                entity.HasIndex(e => new { e.Id, e.SourceId })
                    .HasName("ApplicationEntranceTestDocument_SourceID");

                entity.HasIndex(e => new { e.ApplicationId, e.BenefitId, e.ResultValue, e.SourceId })
                    .HasName("IDX_NPS_VUZ_perf");

                entity.HasIndex(e => new { e.EntranceTestItemId, e.ApplicationId, e.SourceId, e.SubjectId, e.BenefitId })
                    .HasName("I_ApplicationEntranceTestDocument_ForEge");

                entity.HasIndex(e => new { e.ApplicationId, e.SubjectId, e.SourceId, e.ResultValue, e.BenefitId, e.UsedInOrder })
                    .HasName("<Name of Missing Index, sysname,>");

                entity.HasIndex(e => new { e.EntranceTestItemId, e.ResultValue, e.Id, e.ApplicationId, e.SourceId, e.BenefitId })
                    .HasName("IDX_GZGU_AETD_2");

                entity.HasIndex(e => new { e.SourceId, e.ApplicationId, e.Id, e.ResultValue, e.EntranceTestItemId, e.BenefitId })
                    .HasName("IDX_GZGU_AETD_1");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AppealStatusId).HasColumnName("AppealStatusID");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.BenefitId).HasColumnName("BenefitID");

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EgeResultValue).HasColumnType("decimal(7, 4)");

                entity.Property(e => e.EntranceTestItemId).HasColumnName("EntranceTestItemID");

                entity.Property(e => e.EntranceTestTypeId).HasColumnName("EntranceTestTypeID");

                entity.Property(e => e.EntrantDocumentId).HasColumnName("EntrantDocumentID");

                entity.Property(e => e.InstitutionDocumentDate).HasColumnType("datetime");

                entity.Property(e => e.InstitutionDocumentNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.InstitutionDocumentTypeId).HasColumnName("InstitutionDocumentTypeID");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ResultValue).HasColumnType("decimal(7, 4)");

                entity.Property(e => e.SourceId).HasColumnName("SourceID");

                entity.Property(e => e.SubjectId).HasColumnName("SubjectID");

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.AppealStatus)
                    .WithMany(p => p.ApplicationEntranceTestDocument)
                    .HasForeignKey(d => d.AppealStatusId)
                    .HasConstraintName("FK_ApplicationEntranceTestDocument_AppealStatus");

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.ApplicationEntranceTestDocument)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApplicationEntranceTestDocument_Application");

                entity.HasOne(d => d.Benefit)
                    .WithMany(p => p.ApplicationEntranceTestDocument)
                    .HasForeignKey(d => d.BenefitId)
                    .HasConstraintName("FK_ApplicationEntranceTestDocument_Benefit");

                entity.HasOne(d => d.CompetitiveGroup)
                    .WithMany(p => p.ApplicationEntranceTestDocument)
                    .HasForeignKey(d => d.CompetitiveGroupId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ApplicationEntranceTestDocument_CompetitiveGroup");

                entity.HasOne(d => d.EntranceTestItem)
                    .WithMany(p => p.ApplicationEntranceTestDocument)
                    .HasForeignKey(d => d.EntranceTestItemId)
                    .HasConstraintName("FK_ApplicationEntranceTestDocument_EntranceTestItemC");

                entity.HasOne(d => d.EntranceTestType)
                    .WithMany(p => p.ApplicationEntranceTestDocument)
                    .HasForeignKey(d => d.EntranceTestTypeId)
                    .HasConstraintName("FK_ApplicationEntranceTestDocument_EntranceTestType");

                entity.HasOne(d => d.EntrantDocument)
                    .WithMany(p => p.ApplicationEntranceTestDocument)
                    .HasForeignKey(d => d.EntrantDocumentId)
                    .HasConstraintName("FK_ApplicationEntranceTestDocument_EntrantDocument");

                entity.HasOne(d => d.InstitutionDocumentType)
                    .WithMany(p => p.ApplicationEntranceTestDocument)
                    .HasForeignKey(d => d.InstitutionDocumentTypeId)
                    .HasConstraintName("FK_ApplicationEntranceTestDocument_InstitutionDocumentType");

                entity.HasOne(d => d.Source)
                    .WithMany(p => p.ApplicationEntranceTestDocument)
                    .HasForeignKey(d => d.SourceId)
                    .HasConstraintName("FK_ApplicationEntranceTestDocument_EntranceTestResultSource");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.ApplicationEntranceTestDocument)
                    .HasForeignKey(d => d.SubjectId)
                    .HasConstraintName("FK_ApplicationEntranceTestDocument_Subject");
            });

            modelBuilder.Entity<ApplicationEntranceTestDocumentTmpR>(entity =>
            {
                entity.ToTable("ApplicationEntranceTestDocument_tmpR");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AppealStatusId).HasColumnName("AppealStatusID");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.BenefitId).HasColumnName("BenefitID");

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EgeResultValue).HasColumnType("decimal(7, 4)");

                entity.Property(e => e.EntranceTestItemId).HasColumnName("EntranceTestItemID");

                entity.Property(e => e.EntranceTestTypeId).HasColumnName("EntranceTestTypeID");

                entity.Property(e => e.EntrantDocumentId).HasColumnName("EntrantDocumentID");

                entity.Property(e => e.InstitutionDocumentDate).HasColumnType("datetime");

                entity.Property(e => e.InstitutionDocumentNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.InstitutionDocumentTypeId).HasColumnName("InstitutionDocumentTypeID");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ResultValue).HasColumnType("decimal(7, 4)");

                entity.Property(e => e.SourceId).HasColumnName("SourceID");

                entity.Property(e => e.SubjectId).HasColumnName("SubjectID");

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ApplicationEntrantDocument>(entity =>
            {
                entity.HasIndex(e => e.ApplicationId)
                    .HasName("I_ApplicationEntrantDocument_ApplicationID");

                entity.HasIndex(e => e.EntrantDocumentId)
                    .HasName("I_ApplicationEntrantDocument_EntrantDocumentID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EntrantDocumentId).HasColumnName("EntrantDocumentID");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.OriginalReceivedDate).HasColumnType("datetime");

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.ApplicationEntrantDocument)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApplicationEntrantDocument_Application");

                entity.HasOne(d => d.EntrantDocument)
                    .WithMany(p => p.ApplicationEntrantDocument)
                    .HasForeignKey(d => d.EntrantDocumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApplicationEntrantDocument_EntrantDocument");
            });

            modelBuilder.Entity<ApplicationExportRequest>(entity =>
            {
                entity.HasKey(e => e.RequestId);

                entity.Property(e => e.RequestId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.RequestDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<ApplicationExtra>(entity =>
            {
                entity.Property(e => e.ApplicationExtraId)
                    .HasColumnName("ApplicationExtraID")
                    .ValueGeneratedNever();

                entity.Property(e => e.ApplicationExtraDefinitionId).HasColumnName("ApplicationExtraDefinitionID");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.ApplicationExtraDefinition)
                    .WithMany(p => p.ApplicationExtra)
                    .HasForeignKey(d => d.ApplicationExtraDefinitionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApplicationExtra_ApplicationExtraDefinition");

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.ApplicationExtra)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApplicationExtra_Application");
            });

            modelBuilder.Entity<ApplicationExtraDefinition>(entity =>
            {
                entity.Property(e => e.ApplicationExtraDefinitionId).HasColumnName("ApplicationExtraDefinitionID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ApplicationForcedAdmissionDocument>(entity =>
            {
                entity.Property(e => e.ApplicationForcedAdmissionDocumentId).HasColumnName("ApplicationForcedAdmissionDocumentID");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.AttachmentId).HasColumnName("AttachmentID");

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.ApplicationForcedAdmissionDocument)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Applicati__Appli__1D66518C");

                entity.HasOne(d => d.Attachment)
                    .WithMany(p => p.ApplicationForcedAdmissionDocument)
                    .HasForeignKey(d => d.AttachmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Applicati__Attac__1E5A75C5");
            });

            modelBuilder.Entity<ApplicationForcedAdmissionReason>(entity =>
            {
                entity.HasKey(e => e.ApplicationForcedAdmissionReasonsId)
                    .HasName("PK__Applicat__D29924CD66603565");

                entity.Property(e => e.ApplicationForcedAdmissionReasonsId)
                    .HasColumnName("ApplicationForcedAdmissionReasonsID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ApplicationReturnDocumentsType>(entity =>
            {
                entity.Property(e => e.ApplicationReturnDocumentsTypeId).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ApplicationSelectedCompetitiveGroup>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.CalculatedBenefitId).HasColumnName("CalculatedBenefitID");

                entity.Property(e => e.CalculatedRating).HasColumnType("decimal(10, 4)");

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.ItemId)
                    .HasColumnName("ItemID")
                    .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<ApplicationSelectedCompetitiveGroupItem>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.CompetitiveGroupItemId).HasColumnName("CompetitiveGroupItemID");

                entity.Property(e => e.ItemId)
                    .HasColumnName("ItemID")
                    .ValueGeneratedOnAdd();

                entity.HasOne(d => d.Application)
                    .WithMany()
                    .HasForeignKey(d => d.ApplicationId)
                    .HasConstraintName("FK_ApplicationSelectedCompetitiveGroupItem_Application");

                entity.HasOne(d => d.CompetitiveGroupItem)
                    .WithMany()
                    .HasForeignKey(d => d.CompetitiveGroupItemId)
                    .HasConstraintName("FK_ApplicationSelectedCompetitiveGroupItem_CompetitiveGroupItem");
            });

            modelBuilder.Entity<ApplicationSelectedCompetitiveGroupTarget>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.CompetitiveGroupTargetId).HasColumnName("CompetitiveGroupTargetID");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsForOz).HasColumnName("IsForOZ");

                entity.HasOne(d => d.Application)
                    .WithMany()
                    .HasForeignKey(d => d.ApplicationId)
                    .HasConstraintName("FK_ApplicationSelectedCompetitiveGroupTarget_Application");

                entity.HasOne(d => d.CompetitiveGroupTarget)
                    .WithMany()
                    .HasForeignKey(d => d.CompetitiveGroupTargetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApplicationSelectedCompetitiveGroupTarget_CompetitiveGroupTarget");
            });

            modelBuilder.Entity<ApplicationStatusType>(entity =>
            {
                entity.HasKey(e => e.StatusId);

                entity.Property(e => e.StatusId)
                    .HasColumnName("StatusID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AspnetApplications>(entity =>
            {
                entity.HasKey(e => e.ApplicationId)
                    .HasName("PK__aspnet_A__C93A4C98787EE5A0")
                    .IsClustered(false);

                entity.ToTable("aspnet_Applications");

                entity.HasIndex(e => e.ApplicationName)
                    .HasName("UQ__aspnet_A__3091033172C60C4A")
                    .IsUnique();

                entity.HasIndex(e => e.LoweredApplicationName)
                    .HasName("UQ__aspnet_A__17477DE475A278F5")
                    .IsUnique();

                entity.Property(e => e.ApplicationId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.ApplicationName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Description).HasMaxLength(256);

                entity.Property(e => e.LoweredApplicationName)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<AspnetMembership>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__aspnet_M__1788CC4D7C4F7684")
                    .IsClustered(false);

                entity.ToTable("aspnet_Membership");

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.Comment).HasColumnType("ntext");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.FailedPasswordAnswerAttemptWindowStart).HasColumnType("datetime");

                entity.Property(e => e.FailedPasswordAttemptWindowStart).HasColumnType("datetime");

                entity.Property(e => e.LastLockoutDate).HasColumnType("datetime");

                entity.Property(e => e.LastLoginDate).HasColumnType("datetime");

                entity.Property(e => e.LastPasswordChangedDate).HasColumnType("datetime");

                entity.Property(e => e.LoweredEmail).HasMaxLength(256);

                entity.Property(e => e.MobilePin)
                    .HasColumnName("MobilePIN")
                    .HasMaxLength(16);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.PasswordAnswer).HasMaxLength(128);

                entity.Property(e => e.PasswordQuestion).HasMaxLength(256);

                entity.Property(e => e.PasswordSalt)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.AspnetMembership)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__aspnet_Me__Appli__231F2AE2");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.AspnetMembership)
                    .HasForeignKey<AspnetMembership>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__aspnet_Me__UserI__24134F1B");
            });

            modelBuilder.Entity<AspnetPaths>(entity =>
            {
                entity.HasKey(e => e.PathId)
                    .HasName("PK__aspnet_P__CD67DC5800200768")
                    .IsClustered(false);

                entity.ToTable("aspnet_Paths");

                entity.Property(e => e.PathId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.LoweredPath)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.AspnetPaths)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__aspnet_Pa__Appli__25077354");
            });

            modelBuilder.Entity<AspnetPersonalizationAllUsers>(entity =>
            {
                entity.HasKey(e => e.PathId)
                    .HasName("PK__aspnet_P__CD67DC5903F0984C");

                entity.ToTable("aspnet_PersonalizationAllUsers");

                entity.Property(e => e.PathId).ValueGeneratedNever();

                entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.PageSettings)
                    .IsRequired()
                    .HasColumnType("image");

                entity.HasOne(d => d.Path)
                    .WithOne(p => p.AspnetPersonalizationAllUsers)
                    .HasForeignKey<AspnetPersonalizationAllUsers>(d => d.PathId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__aspnet_Pe__PathI__25FB978D");
            });

            modelBuilder.Entity<AspnetPersonalizationPerUser>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__aspnet_P__3214EC0607C12930")
                    .IsClustered(false);

                entity.ToTable("aspnet_PersonalizationPerUser");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.PageSettings)
                    .IsRequired()
                    .HasColumnType("image");

                entity.HasOne(d => d.Path)
                    .WithMany(p => p.AspnetPersonalizationPerUser)
                    .HasForeignKey(d => d.PathId)
                    .HasConstraintName("FK__aspnet_Pe__PathI__26EFBBC6");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspnetPersonalizationPerUser)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__aspnet_Pe__UserI__27E3DFFF");
            });

            modelBuilder.Entity<AspnetProfile>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__aspnet_P__1788CC4C0B91BA14");

                entity.ToTable("aspnet_Profile");

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.PropertyNames)
                    .IsRequired()
                    .HasColumnType("ntext");

                entity.Property(e => e.PropertyValuesBinary)
                    .IsRequired()
                    .HasColumnType("image");

                entity.Property(e => e.PropertyValuesString)
                    .IsRequired()
                    .HasColumnType("ntext");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.AspnetProfile)
                    .HasForeignKey<AspnetProfile>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__aspnet_Pr__UserI__28D80438");
            });

            modelBuilder.Entity<AspnetRoles>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("PK__aspnet_R__8AFACE1B0F624AF8")
                    .IsClustered(false);

                entity.ToTable("aspnet_Roles");

                entity.Property(e => e.RoleId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Description).HasMaxLength(256);

                entity.Property(e => e.LoweredRoleName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.AspnetRoles)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__aspnet_Ro__Appli__29CC2871");
            });

            modelBuilder.Entity<AspnetSchemaVersions>(entity =>
            {
                entity.HasKey(e => new { e.Feature, e.CompatibleSchemaVersion })
                    .HasName("PK__aspnet_S__5A1E6BC11332DBDC");

                entity.ToTable("aspnet_SchemaVersions");

                entity.Property(e => e.Feature).HasMaxLength(128);

                entity.Property(e => e.CompatibleSchemaVersion).HasMaxLength(128);
            });

            modelBuilder.Entity<AspnetUsers>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__aspnet_U__1788CC4D17036CC0")
                    .IsClustered(false);

                entity.ToTable("aspnet_Users");

                entity.HasIndex(e => e.LoweredUserName)
                    .HasName("I_aspnet_Users_LoweredUserName");

                entity.Property(e => e.UserId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.LastActivityDate).HasColumnType("datetime");

                entity.Property(e => e.LoweredUserName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.MobileAlias).HasMaxLength(16);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.AspnetUsers)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__aspnet_Us__Appli__2AC04CAA");
            });

            modelBuilder.Entity<AspnetUsersInRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId })
                    .HasName("PK__aspnet_U__AF2760AD1AD3FDA4");

                entity.ToTable("aspnet_UsersInRoles");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspnetUsersInRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__aspnet_Us__RoleI__2BB470E3");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspnetUsersInRoles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__aspnet_Us__UserI__2CA8951C");
            });

            modelBuilder.Entity<AspnetWebEventEvents>(entity =>
            {
                entity.HasKey(e => e.EventId)
                    .HasName("PK__aspnet_W__7944C8101EA48E88");

                entity.ToTable("aspnet_WebEvent_Events");

                entity.Property(e => e.EventId)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ApplicationPath).HasMaxLength(256);

                entity.Property(e => e.ApplicationVirtualPath).HasMaxLength(256);

                entity.Property(e => e.Details).HasColumnType("ntext");

                entity.Property(e => e.EventOccurrence).HasColumnType("decimal(19, 0)");

                entity.Property(e => e.EventSequence).HasColumnType("decimal(19, 0)");

                entity.Property(e => e.EventTime).HasColumnType("datetime");

                entity.Property(e => e.EventTimeUtc).HasColumnType("datetime");

                entity.Property(e => e.EventType)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.ExceptionType).HasMaxLength(256);

                entity.Property(e => e.MachineName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Message).HasMaxLength(1024);

                entity.Property(e => e.RequestUrl).HasMaxLength(1024);
            });

            modelBuilder.Entity<Attachment>(entity =>
            {
                entity.Property(e => e.AttachmentId).HasColumnName("AttachmentID");

                entity.Property(e => e.Body).IsRequired();

                entity.Property(e => e.ContentLength).HasDefaultValueSql("((0))");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DisplayName)
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.FileId)
                    .HasColumnName("FileID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.MimeType)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<InstitutionAttachment>(entity =>
            {
                entity.HasKey(e => e.AttachmentId)
                    .HasName("PK_InstitutionAttachment");

                entity.Property(e => e.AttachmentId).HasColumnName("AttachmentID");

                entity.Property(e => e.Body).IsRequired();

                entity.Property(e => e.ContentLength).HasDefaultValueSql("((0))");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DisplayName)
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.FileId)
                    .HasColumnName("FileID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.MimeType)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false);

            });

            modelBuilder.Entity<AutoApplicationStatusType>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AutoEntranceTestList>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.Pr1).HasColumnName("pr1");

                entity.Property(e => e.Pr10).HasColumnName("pr10");

                entity.Property(e => e.Pr2).HasColumnName("pr2");

                entity.Property(e => e.Pr3).HasColumnName("pr3");

                entity.Property(e => e.Pr4).HasColumnName("pr4");

                entity.Property(e => e.Pr5).HasColumnName("pr5");

                entity.Property(e => e.Pr6).HasColumnName("pr6");

                entity.Property(e => e.Pr7).HasColumnName("pr7");

                entity.Property(e => e.Pr8).HasColumnName("pr8");

                entity.Property(e => e.Pr9).HasColumnName("pr9");

                entity.Property(e => e.Rank).HasColumnName("rank");
            });

            modelBuilder.Entity<AutoOrderAgreement>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.InstitutionId });
            });

            modelBuilder.Entity<AutoOrderStage>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.StageName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BansFromRon>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("bans_from_ron");

                entity.Property(e => e.Comment)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.DateBegine).HasColumnType("datetime");

                entity.Property(e => e.DateEnd).HasColumnType("datetime");

                entity.Property(e => e.EsrpId).HasColumnName("esrpID");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Number)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Bb>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("bb");

                entity.Property(e => e.ParticipantId).HasColumnName("ParticipantID");

                entity.Property(e => e.ИсточникФинансирования)
                    .IsRequired()
                    .HasColumnName("Источник финансирования")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.НаименованиеОо)
                    .HasColumnName("Наименование ОО")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.НаличиеИндивидуальныхДостижений)
                    .IsRequired()
                    .HasColumnName("Наличие индивидуальных достижений")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.НаправлениеПодготовки)
                    .HasColumnName("Направление подготовки")
                    .HasMaxLength(551)
                    .IsUnicode(false);

                entity.Property(e => e.РегионОо).HasColumnName("Регион ОО");

                entity.Property(e => e.СведенияОбОлимпиадеАх).HasColumnName("Сведения об олимпиаде(ах)");

                entity.Property(e => e.Статус)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ТипМедали)
                    .HasColumnName("Тип медали")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ТипПоступающего)
                    .HasColumnName("Тип поступающего")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.УровеньОбразования)
                    .IsRequired()
                    .HasColumnName("Уровень образования")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ФормаОбучения)
                    .IsRequired()
                    .HasColumnName("Форма обучения")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Benefit>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UK_Benefit_Name")
                    .IsUnique();

                entity.Property(e => e.BenefitId).HasColumnName("BenefitID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ShortName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BenefitItemC>(entity =>
            {
                entity.HasKey(e => e.BenefitItemId);

                entity.HasIndex(e => e.BenefitId)
                    .HasName("I_BenefitItemC_BenefitID");

                entity.HasIndex(e => e.CompetitiveGroupId)
                    .HasName("I_BenefitItemC_CompetitiveGroupID");

                entity.HasIndex(e => e.EntranceTestItemId)
                    .HasName("I_BenefitItemC_EntranceTestItemID");

                entity.Property(e => e.BenefitItemId).HasColumnName("BenefitItemID");

                entity.Property(e => e.BenefitId).HasColumnName("BenefitID");

                entity.Property(e => e.BenefitItemGuid).HasColumnName("BenefitItemGUID");

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EntranceTestItemId).HasColumnName("EntranceTestItemID");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.OlympicDiplomTypeId).HasColumnName("OlympicDiplomTypeID");

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.Benefit)
                    .WithMany(p => p.BenefitItemC)
                    .HasForeignKey(d => d.BenefitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BenefitItemC_Benefit");

                entity.HasOne(d => d.CompetitiveGroup)
                    .WithMany(p => p.BenefitItemC)
                    .HasForeignKey(d => d.CompetitiveGroupId)
                    .HasConstraintName("FK_BenefitItemC_CompetitiveGroup");

                entity.HasOne(d => d.EntranceTestItem)
                    .WithMany(p => p.BenefitItemC)
                    .HasForeignKey(d => d.EntranceTestItemId)
                    .HasConstraintName("FK_BenefitItem_EntranceTestItemC");

                entity.HasOne(d => d.OlympicDiplomType)
                    .WithMany(p => p.BenefitItemC)
                    .HasForeignKey(d => d.OlympicDiplomTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BenefitItemC_OlympicDiplomType");
            });

            modelBuilder.Entity<BenefitItemColympicType>(entity =>
            {
                entity.ToTable("BenefitItemCOlympicType");

                entity.HasIndex(e => e.OlympicTypeId)
                    .HasName("I_BenefitItemCOlympicType_OlympicTypeID");

                entity.HasIndex(e => new { e.BenefitItemId, e.OlympicTypeId })
                    .HasName("UK_BenefitItemCOlympicType")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BenefitItemId).HasColumnName("BenefitItemID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Guid).HasColumnName("GUID");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.OlympicTypeId).HasColumnName("OlympicTypeID");

                entity.HasOne(d => d.BenefitItem)
                    .WithMany(p => p.BenefitItemColympicType)
                    .HasForeignKey(d => d.BenefitItemId)
                    .HasConstraintName("FK_BenefitItemCOlympicType_BenefitItemC");

                entity.HasOne(d => d.OlympicType)
                    .WithMany(p => p.BenefitItemColympicType)
                    .HasForeignKey(d => d.OlympicTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BenefitItemCOlympicType_OlympicType");
            });

            modelBuilder.Entity<BenefitItemColympicTypeProfile>(entity =>
            {
                entity.ToTable("BenefitItemCOlympicTypeProfile");

                entity.HasIndex(e => e.BenefitItemColympicTypeId)
                    .HasName("I_BenefitItemCOlympicTypeProfile_BenefitItemCOlympicTypeID");

                entity.HasIndex(e => e.OlympicProfileId)
                    .HasName("I_BenefitItemCOlympicTypeProfile_OlympicProfileID");

                entity.Property(e => e.BenefitItemColympicTypeProfileId).HasColumnName("BenefitItemCOlympicTypeProfileID");

                entity.Property(e => e.BenefitItemColympicTypeId).HasColumnName("BenefitItemCOlympicTypeID");

                entity.Property(e => e.OlympicProfileId).HasColumnName("OlympicProfileID");

                entity.HasOne(d => d.BenefitItemColympicType)
                    .WithMany(p => p.BenefitItemColympicTypeProfile)
                    .HasForeignKey(d => d.BenefitItemColympicTypeId)
                    .HasConstraintName("FK_BenefitItemCOlympicTypeProfile_BenefitItemCOlympicType");

                entity.HasOne(d => d.OlympicProfile)
                    .WithMany(p => p.BenefitItemColympicTypeProfile)
                    .HasForeignKey(d => d.OlympicProfileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BenefitItemCOlympicTypeProfile_OlympicProfile");
            });

            modelBuilder.Entity<BenefitItemCprofile>(entity =>
            {
                entity.ToTable("BenefitItemCProfile");

                entity.HasIndex(e => e.BenefitItemId)
                    .HasName("I_BenefitItemCProfile_BenefitItemID");

                entity.HasIndex(e => e.OlympicProfileId)
                    .HasName("I_BenefitItemCProfile_OlympicProfileID");

                entity.Property(e => e.BenefitItemCprofileId).HasColumnName("BenefitItemCProfileID");

                entity.Property(e => e.BenefitItemId).HasColumnName("BenefitItemID");

                entity.Property(e => e.OlympicProfileId).HasColumnName("OlympicProfileID");

                entity.HasOne(d => d.BenefitItem)
                    .WithMany(p => p.BenefitItemCprofile)
                    .HasForeignKey(d => d.BenefitItemId)
                    .HasConstraintName("FK_BenefitItemCProfile_BenefitItemC");

                entity.HasOne(d => d.OlympicProfile)
                    .WithMany(p => p.BenefitItemCprofile)
                    .HasForeignKey(d => d.OlympicProfileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BenefitItemCProfile_OlympicProfile");
            });

            modelBuilder.Entity<BenefitItemSubject>(entity =>
            {
                entity.HasIndex(e => e.BenefitItemId)
                    .HasName("I_BenefitItemSubject_BenefitItemId");

                entity.HasOne(d => d.BenefitItem)
                    .WithMany(p => p.BenefitItemSubject)
                    .HasForeignKey(d => d.BenefitItemId)
                    .HasConstraintName("FK_BenefitItemSubject_BenefitItemC1");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.BenefitItemSubject)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BenefitItemSubject_Subject1");
            });

            modelBuilder.Entity<Benefitbudg>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("BENEFITBUDG");

                entity.Property(e => e.AvgEgeBvi).HasColumnName("avgEgeBvi");

                entity.Property(e => e.AvgEgeClear).HasColumnName("avgEgeClear");

                entity.Property(e => e.EntrantCount).HasColumnName("entrantCount");

                entity.Property(e => e.InOrderBviOsh).HasColumnName("InOrderBviOSH");

                entity.Property(e => e.InOrderBviVsosh).HasColumnName("InOrderBviVSOSH");

                entity.Property(e => e.InOrderMaxegeosh).HasColumnName("InOrderMAXEGEOSH");

                entity.Property(e => e.InOrderMaxegevsosh).HasColumnName("InOrderMAXEGEVSOSH");

                entity.Property(e => e.InstitutionId).HasColumnName("institutionId");

                entity.Property(e => e.MinEntranceValue).HasColumnName("minEntranceValue");

                entity.Property(e => e.Nps)
                    .HasColumnName("nps")
                    .HasMaxLength(500);

                entity.Property(e => e.PlaceCount).HasColumnName("placeCount");

                entity.Property(e => e.Vuz)
                    .HasColumnName("vuz")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<Benefitplat>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("BENEFITPLAT");

                entity.Property(e => e.AvgEgeBvi).HasColumnName("avgEgeBvi");

                entity.Property(e => e.AvgEgeClear).HasColumnName("avgEgeClear");

                entity.Property(e => e.EntrantCount).HasColumnName("entrantCount");

                entity.Property(e => e.InOrderBviOsh).HasColumnName("InOrderBviOSH");

                entity.Property(e => e.InOrderBviVsosh).HasColumnName("InOrderBviVSOSH");

                entity.Property(e => e.InOrderMaxegeosh).HasColumnName("InOrderMAXEGEOSH");

                entity.Property(e => e.InOrderMaxegevsosh).HasColumnName("InOrderMAXEGEVSOSH");

                entity.Property(e => e.InstitutionId).HasColumnName("institutionId");

                entity.Property(e => e.MinEntranceValue).HasColumnName("minEntranceValue");

                entity.Property(e => e.Nps)
                    .HasColumnName("nps")
                    .HasMaxLength(500);

                entity.Property(e => e.PlaceCount).HasColumnName("placeCount");

                entity.Property(e => e.Vuz)
                    .HasColumnName("vuz")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<BulkAdmissionVolume>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("bulk_AdmissionVolume");

                entity.Property(e => e.AdmissionItemTypeId).HasColumnName("AdmissionItemTypeID");

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");

                entity.Property(e => e.DirectionId).HasColumnName("DirectionID");

                entity.Property(e => e.Guid).HasColumnName("GUID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ImportPackageId).HasColumnName("ImportPackageID");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.NumberBudgetOz).HasColumnName("NumberBudgetOZ");

                entity.Property(e => e.NumberPaidOz).HasColumnName("NumberPaidOZ");

                entity.Property(e => e.NumberQuotaOz).HasColumnName("NumberQuotaOZ");

                entity.Property(e => e.NumberTargetOz).HasColumnName("NumberTargetOZ");

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BulkApplication>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("bulk_Application");

                entity.Property(e => e.ApplicationNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EntrantUid)
                    .IsRequired()
                    .HasColumnName("EntrantUID")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Guid).HasColumnName("GUID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ImportPackageId).HasColumnName("ImportPackageID");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.IsRequiresBudgetOz).HasColumnName("IsRequiresBudgetOZ");

                entity.Property(e => e.IsRequiresPaidOz).HasColumnName("IsRequiresPaidOZ");

                entity.Property(e => e.IsRequiresTargetOz).HasColumnName("IsRequiresTargetOZ");

                entity.Property(e => e.RegistrationDate).HasColumnType("datetime");

                entity.Property(e => e.ReturnDocumentsDate).HasColumnType("datetime");

                entity.Property(e => e.StatusComment).IsUnicode(false);

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BulkApplicationCompetitiveGroupItem>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("bulk_ApplicationCompetitiveGroupItem");

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.CompetitiveGroupItemId).HasColumnName("CompetitiveGroupItemID");

                entity.Property(e => e.CompetitiveGroupTargetId).HasColumnName("CompetitiveGroupTargetID");

                entity.Property(e => e.IsAgreedDate).HasColumnType("datetime");

                entity.Property(e => e.IsDisagreedDate).HasColumnType("datetime");

                entity.Property(e => e.IsForSpoandVo).HasColumnName("IsForSPOandVO");
            });

            modelBuilder.Entity<BulkApplicationEntranceTestDocument>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("bulk_ApplicationEntranceTestDocument");

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.DisabledDocumentUid)
                    .HasColumnName("DisabledDocumentUID")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.DistantPlace)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.EgeDocumentId)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.EntranceTestItemId).HasColumnName("EntranceTestItemID");

                entity.Property(e => e.EtentrantDocumentId).HasColumnName("ETEntrantDocumentId");

                entity.Property(e => e.InstitutionDocumentDate).HasColumnType("datetime");

                entity.Property(e => e.InstitutionDocumentNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ResultValue).HasColumnType("decimal(7, 4)");

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BulkApplicationIndividualAchievements>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("bulk_ApplicationIndividualAchievements");

                entity.Property(e => e.EntrantDocumentId).HasColumnName("EntrantDocumentID");

                entity.Property(e => e.EntrantDocumentUid)
                    .HasColumnName("EntrantDocumentUID")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Iamark)
                    .HasColumnName("IAMark")
                    .HasColumnType("decimal(7, 4)");

                entity.Property(e => e.Ianame)
                    .HasColumnName("IAName")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Iauid)
                    .HasColumnName("IAUID")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.InstitutionArchievementId).HasColumnName("InstitutionArchievementID");

                entity.Property(e => e.IsAdvantageRight).HasColumnName("isAdvantageRight");
            });

            modelBuilder.Entity<BulkApplicationSelectedCompetitiveGroup>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("bulk_ApplicationSelectedCompetitiveGroup");

                entity.Property(e => e.CalculatedRating).HasColumnType("decimal(10, 4)");

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BulkApplicationSelectedCompetitiveGroupItem>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("bulk_ApplicationSelectedCompetitiveGroupItem");

                entity.Property(e => e.CompetitiveGroupItemId).HasColumnName("CompetitiveGroupItemID");
            });

            modelBuilder.Entity<BulkApplicationSelectedCompetitiveGroupTarget>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("bulk_ApplicationSelectedCompetitiveGroupTarget");

                entity.Property(e => e.CompetitiveGroupTargetId).HasColumnName("CompetitiveGroupTargetID");

                entity.Property(e => e.IsForOz).HasColumnName("IsForOZ");
            });

            modelBuilder.Entity<BulkApplicationShortUpdate>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("bulk_ApplicationShortUpdate");

                entity.Property(e => e.ApplicationNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.CustomInformation).IsUnicode(false);

                entity.Property(e => e.EntrantDocumentId).HasColumnName("EntrantDocumentID");

                entity.Property(e => e.EntrantDocumentUid)
                    .HasColumnName("EntrantDocumentUID")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ImportPackageId).HasColumnName("ImportPackageID");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.IsAgreedDate).HasColumnType("datetime");

                entity.Property(e => e.IsDisagreedDate).HasColumnType("datetime");

                entity.Property(e => e.OriginalReceivedDate).HasColumnType("datetime");

                entity.Property(e => e.RegistrationDate).HasColumnType("datetime");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");
            });

            modelBuilder.Entity<BulkBenefitItemC>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("bulk_BenefitItemC");

                entity.Property(e => e.BenefitId).HasColumnName("BenefitID");

                entity.Property(e => e.Guid).HasColumnName("GUID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ImportPackageId).HasColumnName("ImportPackageID");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.OlympicDiplomTypeId).HasColumnName("OlympicDiplomTypeID");

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BulkBenefitItemData>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("bulk_BenefitItemData");

                entity.Property(e => e.Guid).HasColumnName("GUID");

                entity.Property(e => e.ImportPackageId).HasColumnName("ImportPackageID");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.OlympicProfileId).HasColumnName("OlympicProfileID");

                entity.Property(e => e.OlympicTypeId).HasColumnName("OlympicTypeID");

                entity.Property(e => e.ParentId).HasColumnName("ParentID");
            });

            modelBuilder.Entity<BulkCampaign>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("bulk_Campaign");

                entity.Property(e => e.CampaignTypeId).HasColumnName("CampaignTypeID");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BulkCampaignDate>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("bulk_CampaignDate");

                entity.Property(e => e.DateEnd).HasColumnType("datetime");

                entity.Property(e => e.DateOrder).HasColumnType("datetime");

                entity.Property(e => e.DateStart).HasColumnType("datetime");

                entity.Property(e => e.EducationFormId).HasColumnName("EducationFormID");

                entity.Property(e => e.EducationLevelId).HasColumnName("EducationLevelID");

                entity.Property(e => e.EducationSourceId).HasColumnName("EducationSourceID");

                entity.Property(e => e.ImportPackageId).HasColumnName("ImportPackageID");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BulkCompetitiveGroup>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("bulk_CompetitiveGroup");

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");

                entity.Property(e => e.DirectionId).HasColumnName("DirectionID");

                entity.Property(e => e.EducationFormId).HasColumnName("EducationFormID");

                entity.Property(e => e.EducationLevelId).HasColumnName("EducationLevelID");

                entity.Property(e => e.EducationSourceId).HasColumnName("EducationSourceID");

                entity.Property(e => e.Guid).HasColumnName("GUID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ImportPackageId).HasColumnName("ImportPackageID");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BulkCompetitiveGroupItem>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("bulk_CompetitiveGroupItem");

                entity.Property(e => e.Guid).HasColumnName("GUID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ImportPackageId).HasColumnName("ImportPackageID");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.NumberBudgetOz).HasColumnName("NumberBudgetOZ");

                entity.Property(e => e.NumberPaidOz).HasColumnName("NumberPaidOZ");

                entity.Property(e => e.NumberQuotaOz).HasColumnName("NumberQuotaOZ");

                entity.Property(e => e.NumberTargetOz).HasColumnName("NumberTargetOZ");

                entity.Property(e => e.ParentId).HasColumnName("ParentID");
            });

            modelBuilder.Entity<BulkCompetitiveGroupProgram>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("bulk_CompetitiveGroupProgram");

                entity.Property(e => e.Guid).HasColumnName("GUID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ImportPackageId).HasColumnName("ImportPackageID");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.InstitutionProgramId).HasColumnName("InstitutionProgramID");

                entity.Property(e => e.ParentId).HasColumnName("ParentID");

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BulkCompetitiveGroupTarget>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("bulk_CompetitiveGroupTarget");

                entity.Property(e => e.Guid).HasColumnName("GUID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ImportPackageId).HasColumnName("ImportPackageID");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ParentId).HasColumnName("ParentID");

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BulkCompetitiveGroupTargetItem>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("bulk_CompetitiveGroupTargetItem");

                entity.Property(e => e.CompetitiveGroupGuid).HasColumnName("CompetitiveGroupGUID");

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.Guid).HasColumnName("GUID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ImportPackageId).HasColumnName("ImportPackageID");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.NumberTargetOz).HasColumnName("NumberTargetOZ");

                entity.Property(e => e.TargetId).HasColumnName("TargetID");
            });

            modelBuilder.Entity<BulkDelete>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("bulk_Delete");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ImportPackageId).HasColumnName("ImportPackageID");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<BulkDistributedAdmissionVolume>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("bulk_DistributedAdmissionVolume");

                entity.Property(e => e.AdmissionVolumeGuid).HasColumnName("AdmissionVolumeGUID");

                entity.Property(e => e.AdmissionVolumeId).HasColumnName("AdmissionVolumeID");

                entity.Property(e => e.Guid).HasColumnName("GUID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ImportPackageId).HasColumnName("ImportPackageID");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.NumberBudgetOz).HasColumnName("NumberBudgetOZ");

                entity.Property(e => e.NumberQuotaOz).HasColumnName("NumberQuotaOZ");

                entity.Property(e => e.NumberTargetOz).HasColumnName("NumberTargetOZ");
            });

            modelBuilder.Entity<BulkEntranceTestItemC>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("bulk_EntranceTestItemC");

                entity.Property(e => e.EntranceTestTypeId).HasColumnName("EntranceTestTypeID");

                entity.Property(e => e.Form)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.Guid).HasColumnName("GUID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ImportPackageId).HasColumnName("ImportPackageID");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.MinScore).HasColumnType("decimal(7, 4)");

                entity.Property(e => e.ParentId).HasColumnName("ParentID");

                entity.Property(e => e.ReplacedEntranceTestItemUid)
                    .HasColumnName("ReplacedEntranceTestItemUID")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.SubjectId).HasColumnName("SubjectID");

                entity.Property(e => e.SubjectName)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BulkEntrant>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("bulk_Entrant");

                entity.Property(e => e.Address)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CustomInformation).IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.GenderId).HasColumnName("GenderID");

                entity.Property(e => e.Guid).HasColumnName("GUID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ImportPackageId).HasColumnName("ImportPackageID");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.IsFromKrymEntrantDocumentUid)
                    .HasColumnName("IsFromKrymEntrantDocumentUID")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RegionId).HasColumnName("RegionID");

                entity.Property(e => e.TownTypeId).HasColumnName("TownTypeID");

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BulkEntrantDocument>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("bulk_EntrantDocument");

                entity.Property(e => e.AdditionalInfo).IsUnicode(false);

                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.BirthPlace)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.DocumentDate).HasColumnType("datetime");

                entity.Property(e => e.DocumentNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentOrganization)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentSeries)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentSpecificData)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.DocumentTypeNameText)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.EgeSubjectId).HasColumnName("EgeSubjectID");

                entity.Property(e => e.EntrantDocumentId).HasColumnName("EntrantDocumentID");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Gpa).HasColumnName("GPA");

                entity.Property(e => e.InstitutionName)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.OlympicDate).HasColumnType("datetime");

                entity.Property(e => e.OlympicName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.OlympicPlace)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.OlympicProfile)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.OriginalReceivedDate).HasColumnType("datetime");

                entity.Property(e => e.ParentsLostCategoryId).HasColumnName("ParentsLostCategoryID");

                entity.Property(e => e.ProfileSubjectId).HasColumnName("ProfileSubjectID");

                entity.Property(e => e.QualificationName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RadiationWorkCategoryId).HasColumnName("RadiationWorkCategoryID");

                entity.Property(e => e.RegistrationNumber)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.SpecialityName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.StateEmployeeCategoryId).HasColumnName("StateEmployeeCategoryID");

                entity.Property(e => e.SubdivisionCode)
                    .HasMaxLength(70)
                    .IsUnicode(false);

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.VeteranCategoryId).HasColumnName("VeteranCategoryID");
            });

            modelBuilder.Entity<BulkEntrantDocumentSubject>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("bulk_EntrantDocumentSubject");

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BulkInstitutionAchievements>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("bulk_InstitutionAchievements");

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");

                entity.Property(e => e.MaxValue).HasColumnType("decimal(7, 4)");

                entity.Property(e => e.Name)
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.Uid)
                    .IsRequired()
                    .HasColumnName("UID")
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BulkInstitutionProgram>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("bulk_InstitutionProgram");

                entity.Property(e => e.Code)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Guid).HasColumnName("GUID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ImportPackageId).HasColumnName("ImportPackageID");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ParentId).HasColumnName("ParentID");

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BulkOrderOfAdmission>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("bulk_OrderOfAdmission");

                entity.Property(e => e.ApplicationCgitemId).HasColumnName("ApplicationCGItemID");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.ApplicationLevelBudgetId).HasColumnName("ApplicationLevelBudgetID");

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");

                entity.Property(e => e.DirectionId).HasColumnName("DirectionID");

                entity.Property(e => e.EducationFormId).HasColumnName("EducationFormID");

                entity.Property(e => e.EducationLevelId).HasColumnName("EducationLevelID");

                entity.Property(e => e.FinanceSourceId).HasColumnName("FinanceSourceID");

                entity.Property(e => e.Guid).HasColumnName("GUID");

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.OrderDatePublished).HasColumnType("datetime");

                entity.Property(e => e.OrderName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.OrderNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Uid)
                    .IsRequired()
                    .HasColumnName("UID")
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<C>(entity =>
            {
                entity.HasKey(e => e.ConditionCode);

                entity.ToTable("_C");

                entity.Property(e => e.ConditionCode)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");

                entity.Property(e => e.CampaignName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.CompetitiveGroupItemId).HasColumnName("CompetitiveGroupItemID");

                entity.Property(e => e.CompetitiveGroupName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CompetitiveGroupTargetId).HasColumnName("CompetitiveGroupTargetID");

                entity.Property(e => e.CompetitiveGroupTargetName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.DirectionCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DirectionId).HasColumnName("DirectionID");

                entity.Property(e => e.DirectionName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.EducationFormId).HasColumnName("EducationFormID");

                entity.Property(e => e.EducationFormName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.EducationLevelId).HasColumnName("EducationLevelID");

                entity.Property(e => e.EducationLevelName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.EducationSourceId).HasColumnName("EducationSourceID");

                entity.Property(e => e.EducationSourceName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");
            });

            modelBuilder.Entity<CTmp2016Xls>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("C:\\tmp\\2016.xls");

                entity.Property(e => e.ИдУчастникаЕгэ).HasColumnName("ИД участника ЕГЭ");

                entity.Property(e => e.Имя)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.ИсточникФинансирования)
                    .HasColumnName("Источник Финансирования")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.КодСпециальности)
                    .HasColumnName("Код специальности")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.НаименованиеОо)
                    .HasColumnName("Наименование ОО")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.НаименованиеПриказа)
                    .HasColumnName("Наименование приказа")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.НаименованиеСпециальности)
                    .HasColumnName("Наименование специальности")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.НомерПриказа)
                    .HasColumnName("Номер приказа")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Отчество)
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.СтатусЗаявления)
                    .HasColumnName("Статус заявления")
                    .HasMaxLength(33)
                    .IsUnicode(false);

                entity.Property(e => e.УровеньОбразования)
                    .HasColumnName("Уровень образования")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Фамилия)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.ФормаОбучения)
                    .HasColumnName("Форма обучения")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Campaign>(entity =>
            {
                entity.HasIndex(e => e.CreatedDate)
                    .HasName("I_Campaign_CreatedDate");

                entity.HasIndex(e => e.InstitutionId)
                    .HasName("I_Campaign_InstitutionID");

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");

                entity.Property(e => e.CampaignAdmissionStatusId).HasDefaultValueSql("((0))");

                entity.Property(e => e.CampaignGuid).HasColumnName("CampaignGUID");

                entity.Property(e => e.CampaignTypeId).HasColumnName("CampaignTypeID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.CampaignAdmissionStatus)
                    .WithMany(p => p.Campaign)
                    .HasForeignKey(d => d.CampaignAdmissionStatusId)
                    .HasConstraintName("FK_CampaignAdmissionStatus");

                entity.HasOne(d => d.Institution)
                    .WithMany(p => p.Campaign)
                    .HasForeignKey(d => d.InstitutionId)
                    .HasConstraintName("FK_Campaign_Institution");
            });

            modelBuilder.Entity<CampaignAdmissionStatus>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CampaignEducationLevel>(entity =>
            {
                entity.Property(e => e.CampaignEducationLevelId).HasColumnName("CampaignEducationLevelID");

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");

                entity.Property(e => e.EducationLevelId).HasColumnName("EducationLevelID");

                entity.HasOne(d => d.Campaign)
                    .WithMany(p => p.CampaignEducationLevel)
                    .HasForeignKey(d => d.CampaignId)
                    .HasConstraintName("FK_CampaignEducationLevel_Campaign");

                entity.HasOne(d => d.EducationLevel)
                    .WithMany(p => p.CampaignEducationLevel)
                    .HasForeignKey(d => d.EducationLevelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CampaignEducationLevel_AdmissionItemType");
            });

            modelBuilder.Entity<CampaignOrderDateCatalog>(entity =>
            {
                entity.HasKey(e => e.YearStart);

                entity.Property(e => e.YearStart).ValueGeneratedNever();

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.PaidOrderDate).HasColumnType("date");

                entity.Property(e => e.Stage1OrderDate).HasColumnType("date");

                entity.Property(e => e.Stage2OrderDate).HasColumnType("date");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.TargetOrderDate).HasColumnType("date");
            });

            modelBuilder.Entity<CampaignStatus>(entity =>
            {
                entity.HasKey(e => e.StatusId)
                    .HasName("PK__Campaign__C8EE20435CA1C101");

                entity.Property(e => e.StatusId)
                    .HasColumnName("StatusID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CampaignTypes>(entity =>
            {
                entity.HasKey(e => e.CampaignTypeId);

                entity.Property(e => e.CampaignTypeId)
                    .HasColumnName("CampaignTypeID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CgiToGzgu>(entity =>
            {
                entity.HasKey(e => e.CompetitiveGroupItemId)
                    .HasName("CGI_TO_GZGU1");

                entity.ToTable("CGI_TO_GZGU");

                entity.Property(e => e.CompetitiveGroupItemId)
                    .HasColumnName("CompetitiveGroupItemID")
                    .ValueGeneratedNever();

                entity.Property(e => e.NumberPaidOz).HasColumnName("NumberPaidOZ");
            });

            modelBuilder.Entity<CityType>(entity =>
            {
                entity.HasKey(e => e.CityId);

                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.OkatoCode)
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.OkatoModified).HasColumnType("datetime");

                entity.Property(e => e.RegionId).HasColumnName("RegionID");
            });

            modelBuilder.Entity<CompatriotCategory>(entity =>
            {
                entity.Property(e => e.CompatriotCategoryId)
                    .HasColumnName("CompatriotCategoryID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CompetitiveGroup>(entity =>
            {
                entity.HasIndex(e => e.CampaignId)
                    .HasName("I_CompetitiveGroup_CampaignID");

                entity.HasIndex(e => e.CreatedDate)
                    .HasName("I_CompetitiveGroup_CreatedDate");

                entity.HasIndex(e => e.InstitutionId)
                    .HasName("I_CompetitiveGroup_InstitutionID");

                entity.HasIndex(e => new { e.InstitutionId, e.Name, e.CampaignId })
                    .HasName("UK_CompetitiveGroup_UniqueInstitutionName")
                    .IsUnique();

                entity.HasIndex(e => new { e.CampaignId, e.CompetitiveGroupId, e.EducationSourceId, e.EducationLevelId })
                    .HasName("<Name of Missing Index, sysname,>");

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");

                entity.Property(e => e.CompetitiveGroupGuid).HasColumnName("CompetitiveGroupGUID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DirectionId).HasColumnName("DirectionID");

                entity.Property(e => e.EducationLevelId).HasColumnName("EducationLevelID");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.Campaign)
                    .WithMany(p => p.CompetitiveGroup)
                    .HasForeignKey(d => d.CampaignId)
                    .HasConstraintName("FK_CompetitiveGroup_Campaign");

                entity.HasOne(d => d.Direction)
                    .WithMany(p => p.CompetitiveGroup)
                    .HasForeignKey(d => d.DirectionId)
                    .HasConstraintName("FK_CompetitiveGroup_Direction");

                entity.HasOne(d => d.EducationForm)
                    .WithMany(p => p.CompetitiveGroupEducationForm)
                    .HasForeignKey(d => d.EducationFormId)
                    .HasConstraintName("FK_CompetitiveGroup_EducationForm");

                entity.HasOne(d => d.EducationLevel)
                    .WithMany(p => p.CompetitiveGroupEducationLevel)
                    .HasForeignKey(d => d.EducationLevelId)
                    .HasConstraintName("FK_CompetitiveGroup_EducationLevel");

                entity.HasOne(d => d.EducationSource)
                    .WithMany(p => p.CompetitiveGroupEducationSource)
                    .HasForeignKey(d => d.EducationSourceId)
                    .HasConstraintName("FK_CompetitiveGroup_EducationSource");

                entity.HasOne(d => d.IdLevelBudgetNavigation)
                    .WithMany(p => p.CompetitiveGroup)
                    .HasForeignKey(d => d.IdLevelBudget)
                    .HasConstraintName("FK_CompetitiveGroup_LevelBudget");

                entity.HasOne(d => d.Institution)
                    .WithMany(p => p.CompetitiveGroup)
                    .HasForeignKey(d => d.InstitutionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompetitiveGroup_Institution");
            });

            modelBuilder.Entity<CompetitiveGroupItem>(entity =>
            {
                entity.HasKey(e => e.CompetitiveGroupItemId)
                    .IsClustered(false);

                entity.HasIndex(e => e.CompetitiveGroupId)
                    .HasName("I_CompetitiveGroupItem_CompetitiveGroupID");

                entity.Property(e => e.CompetitiveGroupItemId).HasColumnName("CompetitiveGroupItemID");

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NumberBudgetOz).HasColumnName("NumberBudgetOZ");

                entity.Property(e => e.NumberPaidOz).HasColumnName("NumberPaidOZ");

                entity.Property(e => e.NumberQuotaOz).HasColumnName("NumberQuotaOZ");

                entity.Property(e => e.NumberTargetOz).HasColumnName("NumberTargetOZ");

                entity.HasOne(d => d.CompetitiveGroup)
                    .WithMany(p => p.CompetitiveGroupItem)
                    .HasForeignKey(d => d.CompetitiveGroupId)
                    .HasConstraintName("FK_CompetitiveGroupItem_CompetitiveGroup");
            });

            modelBuilder.Entity<CompetitiveGroupProgram>(entity =>
            {
                entity.HasKey(e => e.ProgramId);

                entity.HasIndex(e => e.CompetitiveGroupId)
                    .HasName("I_CompetitiveGroupProgram_CompetitiveGroupID");

                entity.Property(e => e.ProgramId).HasColumnName("ProgramID");

                entity.Property(e => e.Code)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.CompetitiveGroup)
                    .WithMany(p => p.CompetitiveGroupProgram)
                    .HasForeignKey(d => d.CompetitiveGroupId)
                    .HasConstraintName("FK_CompetitiveGroupProgram_CompetitiveGroup");
            });

            modelBuilder.Entity<CompetitiveGroupTarget>(entity =>
            {
                entity.HasKey(e => e.CompetitiveGroupTargetId)
                    .IsClustered(false);

                entity.HasIndex(e => e.CreatedDate)
                    .HasName("I_CompetitiveGroupTarget_CreatedDate");

                entity.HasIndex(e => e.InstitutionId)
                    .HasName("I_CompetitiveGroupTarget_InstitutionID");

                entity.Property(e => e.CompetitiveGroupTargetId).HasColumnName("CompetitiveGroupTargetID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.Institution)
                    .WithMany(p => p.CompetitiveGroupTarget)
                    .HasForeignKey(d => d.InstitutionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompetitiveGroupTarget_Institution");
            });

            modelBuilder.Entity<CompetitiveGroupTargetItem>(entity =>
            {
                entity.HasKey(e => e.CompetitiveGroupTargetItemId)
                    .IsClustered(false);

                entity.HasIndex(e => e.CompetitiveGroupId)
                    .HasName("I_CompetitiveGroupTargetItem_CompetitiveGroupID");

                entity.HasIndex(e => e.CompetitiveGroupTargetId)
                    .HasName("I_CompetitiveGroupTargetItem_CompetitiveGroupTargetID");

                entity.Property(e => e.CompetitiveGroupTargetItemId).HasColumnName("CompetitiveGroupTargetItemID");

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.CompetitiveGroupTargetId).HasColumnName("CompetitiveGroupTargetID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NumberTargetOz).HasColumnName("NumberTargetOZ");

                entity.HasOne(d => d.CompetitiveGroup)
                    .WithMany(p => p.CompetitiveGroupTargetItem)
                    .HasForeignKey(d => d.CompetitiveGroupId)
                    .HasConstraintName("FK_CompetitiveGroupTargetItem_CompetitiveGroup");

                entity.HasOne(d => d.CompetitiveGroupTarget)
                    .WithMany(p => p.CompetitiveGroupTargetItem)
                    .HasForeignKey(d => d.CompetitiveGroupTargetId)
                    .HasConstraintName("FK_CompetitiveGroupTargetItem_CompetitiveGroupTarget");
            });

            modelBuilder.Entity<CompetitiveGroupToProgram>(entity =>
            {
                entity.HasKey(e => e.CompetitiveGroupProgramId)
                    .HasName("PK__Competit__4FC3917172910220");

                entity.HasIndex(e => e.CompetitiveGroupProgramId)
                    .HasName("I_InstitutionProgram_CompetitiveGroupID");

                entity.HasIndex(e => e.InstitutionProgramId)
                    .HasName("I_InstitutionProgram_InstitutionProgramID");

                entity.Property(e => e.CompetitiveGroupProgramId).HasColumnName("CompetitiveGroupProgramID");

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.InstitutionProgramId).HasColumnName("InstitutionProgramID");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.CompetitiveGroup)
                    .WithMany(p => p.CompetitiveGroupToProgram)
                    .HasForeignKey(d => d.CompetitiveGroupId)
                    .HasConstraintName("FK_CompetitiveGroupToProgram_CompetitiveGroup");

                entity.HasOne(d => d.InstitutionProgram)
                    .WithMany(p => p.CompetitiveGroupToProgram)
                    .HasForeignKey(d => d.InstitutionProgramId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CompetitiveGroupToProgram_InstitutionProgram");
            });

            modelBuilder.Entity<CompositionThemes>(entity =>
            {
                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.ThemeID).HasColumnName("ThemeID");
            });

            modelBuilder.Entity<CompositionThemesApprob>(entity =>
            {
                entity.HasKey(e => e.ThemeId);

                entity.ToTable("CompositionThemes_Approb");

                entity.Property(e => e.ThemeId)
                    .HasColumnName("ThemeID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CompositionThemesOld>(entity =>
            {
                entity.HasKey(e => e.ThemeId)
                    .HasName("PK__Composit__FBB3E4B932A16594");

                entity.ToTable("CompositionThemes_Old");

                entity.HasComment("Темы сочинений");

                entity.Property(e => e.ThemeId)
                    .HasColumnName("ThemeID")
                    .HasComment("ИД записи");

                entity.Property(e => e.Name)
                    .IsUnicode(false)
                    .HasComment("Название темы сочинения");
            });

            modelBuilder.Entity<CountryDocuments>(entity =>
            {
                entity.HasKey(e => new { e.CountryId, e.DocumentId });

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.CountryDocuments)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK_CountryDocuments_Country");

                entity.HasOne(d => d.Document)
                    .WithMany(p => p.CountryDocuments)
                    .HasForeignKey(d => d.DocumentId)
                    .HasConstraintName("FK_CountryDocuments_Document");
            });

            modelBuilder.Entity<CountryType>(entity =>
            {
                entity.HasKey(e => e.CountryId);

                entity.HasIndex(e => e.Alfa2Code)
                    .HasName("UX_CountryType_Alfa2Code")
                    .IsUnique();

                entity.HasIndex(e => e.Alfa3Code)
                    .HasName("UX_CountryType_Alfa3Code")
                    .IsUnique();

                entity.HasIndex(e => e.DigitCode)
                    .HasName("UX_CountryType_DigitCode")
                    .IsUnique();

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.Alfa2Code)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Alfa3Code)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DigitCode)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.DisplayOrder).HasDefaultValueSql("((1))");

                entity.Property(e => e.Expire).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CourseSubject>(entity =>
            {
                entity.HasIndex(e => new { e.PreparatoryCourseId, e.SubjectName })
                    .HasName("UK_CourseSubject")
                    .IsUnique();

                entity.Property(e => e.CourseSubjectId).HasColumnName("CourseSubjectID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.PreparatoryCourseId).HasColumnName("PreparatoryCourseID");

                entity.Property(e => e.SubjectId).HasColumnName("SubjectID");

                entity.Property(e => e.SubjectName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.PreparatoryCourse)
                    .WithMany(p => p.CourseSubject)
                    .HasForeignKey(d => d.PreparatoryCourseId)
                    .HasConstraintName("FK_CourseSubject_PreparatoryCourse");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.CourseSubject)
                    .HasForeignKey(d => d.SubjectId)
                    .HasConstraintName("FK_CourseSubject_Subject");
            });

            modelBuilder.Entity<CourseType>(entity =>
            {
                entity.HasKey(e => e.CourseId);

                entity.Property(e => e.CourseId)
                    .HasColumnName("CourseID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DbaIndexDefragLog>(entity =>
            {
                entity.HasKey(e => e.IndexDefragId)
                    .HasName("PK_indexDefragLog");

                entity.ToTable("dba_indexDefragLog");

                entity.Property(e => e.IndexDefragId).HasColumnName("indexDefrag_id");

                entity.Property(e => e.DateTimeStart)
                    .HasColumnName("dateTimeStart")
                    .HasColumnType("datetime");

                entity.Property(e => e.DurationSeconds).HasColumnName("durationSeconds");

                entity.Property(e => e.Fragmentation).HasColumnName("fragmentation");

                entity.Property(e => e.IndexId).HasColumnName("indexID");

                entity.Property(e => e.IndexName)
                    .IsRequired()
                    .HasColumnName("indexName")
                    .HasMaxLength(130);

                entity.Property(e => e.ObjectId).HasColumnName("objectID");

                entity.Property(e => e.ObjectName)
                    .IsRequired()
                    .HasColumnName("objectName")
                    .HasMaxLength(130);

                entity.Property(e => e.PageCount).HasColumnName("page_count");
            });

            modelBuilder.Entity<DeletedFromEge>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("deletedFromEge");

                entity.Property(e => e.Lastname)
                    .HasColumnName("lastname")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Midname)
                    .HasColumnName("midname")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Region)
                    .HasColumnName("region")
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Direction>(entity =>
            {
                entity.HasKey(e => e.DirectionId)
                    .IsClustered(false);

                entity.HasIndex(e => e.Qualificationcode)
                    .HasName("IX_Direction_QualificationCode")
                    .IsClustered();

                entity.Property(e => e.DirectionId).HasColumnName("DirectionID");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EduDirectory)
                    .HasColumnName("EDU_DIRECTORY")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Edulevel)
                    .HasColumnName("EDULEVEL")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.EduprAdditional)
                    .HasColumnName("EDUPR_ADDITIONAL")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Eduprogramtype)
                    .HasColumnName("EDUPROGRAMTYPE")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.EiisId)
                    .HasColumnName("EIIS_ID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EiisIdNew)
                    .HasColumnName("EIIS_ID_NEW")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EsrpId).HasColumnName("Esrp_ID");

                entity.Property(e => e.IsActual)
                    .HasColumnName("IS_ACTUAL")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.NewCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ParentId).HasColumnName("ParentID");

                entity.Property(e => e.Period)
                    .HasColumnName("PERIOD")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Qualificationcode)
                    .HasColumnName("QUALIFICATIONCODE")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Qualificationname)
                    .HasColumnName("QUALIFICATIONNAME")
                    .HasMaxLength(1500)
                    .IsUnicode(false);

                entity.Property(e => e.SysGuid)
                    .HasColumnName("SYS_GUID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Ugscode)
                    .HasColumnName("UGSCODE")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Ugsname)
                    .HasColumnName("UGSNAME")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.EducationProgramType)
                    .WithMany(p => p.Direction)
                    .HasForeignKey(d => d.EducationProgramTypeId)
                    .HasConstraintName("FK_Direction_EDU_PROGRAM_TYPES");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.Direction)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_Direction_ParentDirection");
            });

            modelBuilder.Entity<DirectionOld>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Direction_old");

                entity.Property(e => e.Code)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DirectionId).HasColumnName("DirectionID");

                entity.Property(e => e.EduDirectory)
                    .HasColumnName("EDU_DIRECTORY")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Edulevel)
                    .HasColumnName("EDULEVEL")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.EduprAdditional)
                    .HasColumnName("EDUPR_ADDITIONAL")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Eduprogramtype)
                    .HasColumnName("EDUPROGRAMTYPE")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.NewCode)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ParentId).HasColumnName("ParentID");

                entity.Property(e => e.Period)
                    .HasColumnName("PERIOD")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Qualificationcode)
                    .HasColumnName("QUALIFICATIONCODE")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Qualificationname)
                    .HasColumnName("QUALIFICATIONNAME")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.SysGuid)
                    .HasColumnName("SYS_GUID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Ugscode)
                    .HasColumnName("UGSCODE")
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Ugsname)
                    .HasColumnName("UGSNAME")
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DirectionPlanKcp>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("DirectionPlanKCP");

                entity.Property(e => e.EducationLevelId).HasColumnName("EducationLevelID");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.NewCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NumberBudgetOz).HasColumnName("NumberBudgetOZ");
            });

            modelBuilder.Entity<DirectionSubjectLink>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ProfileSubjectId).HasColumnName("ProfileSubjectID");

                entity.HasOne(d => d.ProfileSubject)
                    .WithMany(p => p.DirectionSubjectLink)
                    .HasForeignKey(d => d.ProfileSubjectId)
                    .HasConstraintName("FK_DirectionSubjectLink_Subject");
            });

            modelBuilder.Entity<DirectionSubjectLinkDirection>(entity =>
            {
                entity.HasIndex(e => e.DirectionId)
                    .HasName("UK_DirectionSubjectLinkDirectionDirectionID")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DirectionId).HasColumnName("DirectionID");

                entity.Property(e => e.LinkId).HasColumnName("LinkID");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Direction)
                    .WithOne(p => p.DirectionSubjectLinkDirection)
                    .HasForeignKey<DirectionSubjectLinkDirection>(d => d.DirectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DirectionSubjectLinkDirection_Direction");

                entity.HasOne(d => d.Link)
                    .WithMany(p => p.DirectionSubjectLinkDirection)
                    .HasForeignKey(d => d.LinkId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DirectionSubjectLinkDirection_DirectionSubjectLink");
            });

            modelBuilder.Entity<DirectionSubjectLinkSubject>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.LinkId).HasColumnName("LinkID");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.SubjectId).HasColumnName("SubjectID");

                entity.HasOne(d => d.Link)
                    .WithMany(p => p.DirectionSubjectLinkSubject)
                    .HasForeignKey(d => d.LinkId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DirectionSubjectLinkSubject_DirectionSubjectLink");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.DirectionSubjectLinkSubject)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DirectionSubjectLinkSubject_Subject");
            });

            modelBuilder.Entity<DirectionTemp>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("directionTEMP");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DirectionTmp>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Direction_tmp");

                entity.Property(e => e.Code)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DirectionId).HasColumnName("DirectionID");

                entity.Property(e => e.EduDirectory)
                    .HasColumnName("EDU_DIRECTORY")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Edulevel)
                    .HasColumnName("EDULEVEL")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.EduprAdditional)
                    .HasColumnName("EDUPR_ADDITIONAL")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Eduprogramtype)
                    .HasColumnName("EDUPROGRAMTYPE")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.NewCode)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ParentId).HasColumnName("ParentID");

                entity.Property(e => e.Period)
                    .HasColumnName("PERIOD")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Qualificationcode)
                    .HasColumnName("QUALIFICATIONCODE")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Qualificationname)
                    .HasColumnName("QUALIFICATIONNAME")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.SysGuid)
                    .HasColumnName("SYS_GUID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Ugscode)
                    .HasColumnName("UGSCODE")
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Ugsname)
                    .HasColumnName("UGSNAME")
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DirectionToDirection>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Guid1)
                    .HasColumnName("guid1")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Guid2)
                    .HasColumnName("guid2")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Id).HasColumnName("id");
            });

            modelBuilder.Entity<DirectionsFromEiis>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("DIRECTIONS_FROM_EIIS");

                entity.Property(e => e.Code)
                    .HasColumnName("CODE")
                    .IsUnicode(false);

                entity.Property(e => e.Edulevelfk)
                    .HasColumnName("EDULEVELFK")
                    .IsUnicode(false);

                entity.Property(e => e.Eduprogrammfk)
                    .HasColumnName("EDUPROGRAMMFK")
                    .IsUnicode(false);

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasColumnName("ID")
                    .IsUnicode(false);

                entity.Property(e => e.IsActual)
                    .HasColumnName("IS_ACTUAL")
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .IsUnicode(false);

                entity.Property(e => e.NotTrue)
                    .HasColumnName("NOT_TRUE")
                    .IsUnicode(false);

                entity.Property(e => e.Reiod)
                    .HasColumnName("REIOD")
                    .IsUnicode(false);

                entity.Property(e => e.Standart)
                    .HasColumnName("STANDART")
                    .IsUnicode(false);

                entity.Property(e => e.UgsFk)
                    .HasColumnName("UGS_FK")
                    .IsUnicode(false);

                entity.Property(e => e.Ugscode)
                    .HasColumnName("UGSCODE")
                    .IsUnicode(false);

                entity.Property(e => e.Ugsname)
                    .HasColumnName("UGSNAME")
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DisabilityType>(entity =>
            {
                entity.HasKey(e => e.DisabilityId);

                entity.Property(e => e.DisabilityId)
                    .HasColumnName("DisabilityID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DistributedAdmissionVolume>(entity =>
            {
                entity.HasIndex(e => e.AdmissionVolumeId)
                    .HasName("I_DistributedAdmissionVolume_AdmissionVolumeID");

                entity.Property(e => e.DistributedAdmissionVolumeId).HasColumnName("DistributedAdmissionVolumeID");

                entity.Property(e => e.AdmissionVolumeId).HasColumnName("AdmissionVolumeID");

                entity.Property(e => e.NumberBudgetOz).HasColumnName("NumberBudgetOZ");

                entity.Property(e => e.NumberQuotaOz).HasColumnName("NumberQuotaOZ");

                entity.Property(e => e.NumberTargetOz).HasColumnName("NumberTargetOZ");

                entity.HasOne(d => d.AdmissionVolume)
                    .WithMany(p => p.DistributedAdmissionVolume)
                    .HasForeignKey(d => d.AdmissionVolumeId)
                    .HasConstraintName("FK_DistributedAdmissionVolume_AdmissionVolume");

                entity.HasOne(d => d.IdLevelBudgetNavigation)
                    .WithMany(p => p.DistributedAdmissionVolume)
                    .HasForeignKey(d => d.IdLevelBudget)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DistributedAdmissionVolume_LevelBudget");
            });

            modelBuilder.Entity<DistributedPlanAdmissionVolume>(entity =>
            {
                entity.Property(e => e.DistributedPlanAdmissionVolumeId).HasColumnName("DistributedPlanAdmissionVolumeID");

                entity.Property(e => e.PlanAdmissionVolumeId).HasColumnName("PlanAdmissionVolumeID");

                entity.HasOne(d => d.IdLevelBudgetNavigation)
                    .WithMany(p => p.DistributedPlanAdmissionVolume)
                    .HasForeignKey(d => d.IdLevelBudget)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DistributedPlanAdmissionVolume_IdLevelBudget");

                entity.HasOne(d => d.PlanAdmissionVolume)
                    .WithMany(p => p.DistributedPlanAdmissionVolume)
                    .HasForeignKey(d => d.PlanAdmissionVolumeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DistributedPlanAdmissionVolume_PlanAdmissionVolumeID");
            });

            modelBuilder.Entity<DocumentCategory>(entity =>
            {
                entity.HasKey(e => e.CategoryId)
                    .HasName("PK__Document__19093A2B235F2204");

                entity.HasComment("Справочник категорий документов");

                entity.Property(e => e.CategoryId)
                    .HasColumnName("CategoryID")
                    .HasComment("ИД категории документов");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Наименование категории документов");
            });

            modelBuilder.Entity<DocumentCheckStatus>(entity =>
            {
                entity.HasKey(e => e.StatusId)
                    .HasName("PK__Document__C8EE20432057CCD0");

                entity.Property(e => e.StatusId)
                    .HasColumnName("StatusID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DocumentType>(entity =>
            {
                entity.HasKey(e => e.DocumentId);

                entity.Property(e => e.DocumentId)
                    .HasColumnName("DocumentID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CategoryId)
                    .HasColumnName("CategoryID")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(120)
                    .IsUnicode(false);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.DocumentType)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DocumentType_DocumentCategory");
            });

            modelBuilder.Entity<EduLevelDocumentType>(entity =>
            {
                entity.HasComment("Типы документов, необходимые для поступления на уровень образования");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("ИД записи");

                entity.Property(e => e.DocumentTypeId).HasComment("Тип документа FK (Document¬Type – Document¬ID)");

                entity.Property(e => e.LevelId)
                    .HasColumnName("LevelID")
                    .HasComment("Уровень образования FK (EduLevels – LevelID)");

                entity.HasOne(d => d.DocumentType)
                    .WithMany(p => p.EduLevelDocumentType)
                    .HasForeignKey(d => d.DocumentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EduLevelDocumentType_DocumentType");

                entity.HasOne(d => d.Level)
                    .WithMany(p => p.EduLevelDocumentType)
                    .HasForeignKey(d => d.LevelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EduLevelDocumentType_EduLevels");
            });

            modelBuilder.Entity<EduLevels>(entity =>
            {
                entity.HasKey(e => e.LevelId)
                    .HasName("PK__EduLevel__09F03C06272FB2E8");

                entity.HasComment("Справочник уровней образования");

                entity.Property(e => e.LevelId).HasColumnName("LevelID");

                entity.Property(e => e.AdmissionItemTypeId)
                    .HasColumnName("AdmissionItemTypeID")
                    .HasComment("Ссылка на уровень образования из AdmissionItemType FK (AdmissionItemType – ItemTypeID)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("Наименование уровня образования");

                entity.HasOne(d => d.AdmissionItemType)
                    .WithMany(p => p.EduLevels)
                    .HasForeignKey(d => d.AdmissionItemTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EduLevels_AdmissionItemType");
            });

            modelBuilder.Entity<EduLevelsToCampaignTypes>(entity =>
            {
                entity.HasKey(e => new { e.CampaignTypeId, e.AdmissionItemTypeId })
                    .HasName("PK_EduLevelEduLevelsToCampaignTypes");

                entity.HasComment("Соответствие уровней образования типам приемных кампаний");

                entity.Property(e => e.CampaignTypeId)
                    .HasColumnName("CampaignTypeID")
                    .HasComment("ИД типа ПК FK (CampaignTypes – CampaignTypeID)");

                entity.Property(e => e.AdmissionItemTypeId)
                    .HasColumnName("AdmissionItemTypeID")
                    .HasComment("ИД уровня образования FK (AdmissionItemType – ItemTypeID)");

                entity.HasOne(d => d.AdmissionItemType)
                    .WithMany(p => p.EduLevelsToCampaignTypes)
                    .HasForeignKey(d => d.AdmissionItemTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EduLevelsToCampaignType_AdmissionItemType");

                entity.HasOne(d => d.CampaignType)
                    .WithMany(p => p.EduLevelsToCampaignTypes)
                    .HasForeignKey(d => d.CampaignTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EduLevelsToCampaignType_CampaignType");
            });

            modelBuilder.Entity<EduLevelsToUgsCode>(entity =>
            {
                entity.HasKey(e => new { e.EducationLevelId, e.QualificationCode });

                entity.Property(e => e.QualificationCode)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.EducationLevel)
                    .WithMany(p => p.EduLevelsToUgsCode)
                    .HasForeignKey(d => d.EducationLevelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EduLevelsToUgsCode_AdmissionItemType");
            });

            modelBuilder.Entity<EduProgramTypes>(entity =>
            {
                entity.ToTable("EDU_PROGRAM_TYPES");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Code)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.EiisId)
                    .HasColumnName("EIIS_ID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EsrpId).HasColumnName("Esrp_Id");

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Shortname)
                    .HasColumnName("SHORTNAME")
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EducationForms>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Egorova270Oh2016>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EGOROVA_270_OH_2016");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.DocumentTypeId).HasColumnName("DocumentTypeID");
            });

            modelBuilder.Entity<Egorova270Oh2017>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EGOROVA_270_OH_2017");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.DocumentTypeId).HasColumnName("DocumentTypeID");
            });

            modelBuilder.Entity<EgorovaInostran>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("egorova_inostran");

                entity.Property(e => e._1)
                    .HasColumnName("1")
                    .IsUnicode(false);

                entity.Property(e => e._10)
                    .HasColumnName("10")
                    .IsUnicode(false);

                entity.Property(e => e._11)
                    .HasColumnName("11")
                    .IsUnicode(false);

                entity.Property(e => e._12)
                    .HasColumnName("12")
                    .IsUnicode(false);

                entity.Property(e => e._13)
                    .HasColumnName("13")
                    .IsUnicode(false);

                entity.Property(e => e._14)
                    .HasColumnName("14")
                    .IsUnicode(false);

                entity.Property(e => e._15)
                    .HasColumnName("15")
                    .IsUnicode(false);

                entity.Property(e => e._16)
                    .HasColumnName("16")
                    .IsUnicode(false);

                entity.Property(e => e._17)
                    .HasColumnName("17")
                    .IsUnicode(false);

                entity.Property(e => e._18)
                    .HasColumnName("18")
                    .IsUnicode(false);

                entity.Property(e => e._19)
                    .HasColumnName("19")
                    .IsUnicode(false);

                entity.Property(e => e._2)
                    .HasColumnName("2")
                    .IsUnicode(false);

                entity.Property(e => e._20)
                    .HasColumnName("20")
                    .IsUnicode(false);

                entity.Property(e => e._21)
                    .HasColumnName("21")
                    .IsUnicode(false);

                entity.Property(e => e._22)
                    .HasColumnName("22")
                    .IsUnicode(false);

                entity.Property(e => e._23)
                    .HasColumnName("23")
                    .IsUnicode(false);

                entity.Property(e => e._24)
                    .HasColumnName("24")
                    .IsUnicode(false);

                entity.Property(e => e._25)
                    .HasColumnName("25")
                    .IsUnicode(false);

                entity.Property(e => e._26)
                    .HasColumnName("26")
                    .IsUnicode(false);

                entity.Property(e => e._3)
                    .HasColumnName("3")
                    .IsUnicode(false);

                entity.Property(e => e._4)
                    .HasColumnName("4")
                    .IsUnicode(false);

                entity.Property(e => e._5)
                    .HasColumnName("5")
                    .IsUnicode(false);

                entity.Property(e => e._6)
                    .HasColumnName("6")
                    .IsUnicode(false);

                entity.Property(e => e._7)
                    .HasColumnName("7")
                    .IsUnicode(false);

                entity.Property(e => e._8)
                    .HasColumnName("8")
                    .IsUnicode(false);

                entity.Property(e => e._9)
                    .HasColumnName("9")
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EgorovaOlimp>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EGOROVA_OLIMP");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.Value).HasColumnName("VALUE");
            });

            modelBuilder.Entity<EgorovaOlimp1>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EGOROVA_OLIMP1");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.Value).HasColumnName("VALUE");
            });

            modelBuilder.Entity<EgorovaOlimp2016>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EGOROVA_OLIMP2016");

                entity.Property(e => e.CntVl).HasColumnName("CNT_VL");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.Value).HasColumnName("VALUE");
            });

            modelBuilder.Entity<EgorovaOlimp2017>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EGOROVA_OLIMP2017");

                entity.Property(e => e.CntVl).HasColumnName("CNT_VL");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.Value).HasColumnName("VALUE");
            });

            modelBuilder.Entity<EgorovaOlimpi2016oh>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EGOROVA_OLIMPI2016OH");

                entity.Property(e => e.Avgmark).HasColumnName("AVGMARK");

                entity.Property(e => e.Cnt)
                    .IsRequired()
                    .HasColumnName("CNT")
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.PersonId).HasColumnName("PersonID");

                entity.Property(e => e.Ugscode)
                    .IsRequired()
                    .HasColumnName("UGSCODE")
                    .HasMaxLength(4000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EgorovaOlimpi2017oh>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EGOROVA_OLIMPI2017OH");

                entity.Property(e => e.Avgmark).HasColumnName("AVGMARK");

                entity.Property(e => e.Cnt)
                    .IsRequired()
                    .HasColumnName("CNT")
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.PersonId).HasColumnName("PersonID");

                entity.Property(e => e.Ugscode)
                    .IsRequired()
                    .HasColumnName("UGSCODE")
                    .HasMaxLength(4000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EgorovaOlimpic2016>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EGOROVA_OLIMPIC2016");

                entity.Property(e => e.Avgmark).HasColumnName("AVGMARK");

                entity.Property(e => e.Cnt).HasColumnName("CNT");

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.Ugscode)
                    .HasColumnName("UGSCODE")
                    .HasMaxLength(1)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EgorovaOlimpic2016oh>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EGOROVA_OLIMPIC2016OH");

                entity.Property(e => e.Avgmark).HasColumnName("AVGMARK");

                entity.Property(e => e.Cnt)
                    .IsRequired()
                    .HasColumnName("CNT")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.Ugscode)
                    .HasColumnName("UGSCODE")
                    .HasMaxLength(1)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EgorovaUgs2016>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EGOROVA_UGS_2016");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.Ugscode)
                    .IsRequired()
                    .HasColumnName("UGSCODE")
                    .HasMaxLength(4000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EgorovaUgs2016270>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EGOROVA_UGS_2016_270");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.BenefitId).HasColumnName("BenefitID");

                entity.Property(e => e.DocumentTypeId).HasColumnName("DocumentTypeID");

                entity.Property(e => e.Ugscode)
                    .IsRequired()
                    .HasColumnName("UGSCODE")
                    .HasMaxLength(4000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EgorovaUgs2016B1>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EGOROVA_UGS_2016_B1");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.DocumentTypeId).HasColumnName("DocumentTypeID");

                entity.Property(e => e.Ugscode)
                    .IsRequired()
                    .HasColumnName("UGSCODE")
                    .HasMaxLength(4000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EgorovaUgs2016B3>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EGOROVA_UGS_2016_B3");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.DocumentTypeId).HasColumnName("DocumentTypeID");

                entity.Property(e => e.Ugscode)
                    .IsRequired()
                    .HasColumnName("UGSCODE")
                    .HasMaxLength(4000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EgorovaZachOlimp>(entity =>
            {
                entity.HasKey(e => new { e.ApplicationId, e.CompetitiveGroupId, e.EducationSourceId })
                    .HasName("EGOROVA_ZACH_OLIMP1");

                entity.ToTable("EGOROVA_ZACH_OLIMP");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.Year).HasColumnName("YEAR");
            });

            modelBuilder.Entity<EgorovaZachOlimp270Osh2016>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EGOROVA_ZACH_OLIMP_270_OSH_2016");

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.Value).HasColumnName("VALUE");
            });

            modelBuilder.Entity<EgorovaZachOlimp270Osh2017>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EGOROVA_ZACH_OLIMP_270_OSH_2017");

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.Value).HasColumnName("VALUE");
            });

            modelBuilder.Entity<EgorovaZachOlimp270Vsosh2016>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EGOROVA_ZACH_OLIMP_270_VSOSH_2016");

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.Value).HasColumnName("VALUE");
            });

            modelBuilder.Entity<EgorovaZachOlimp270Vsosh2017>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EGOROVA_ZACH_OLIMP_270_VSOSH_2017");

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.Value).HasColumnName("VALUE");
            });

            modelBuilder.Entity<EgorovaZachOlimpB1>(entity =>
            {
                entity.HasKey(e => new { e.ApplicationId, e.CompetitiveGroupId, e.EducationSourceId })
                    .HasName("EGOROVA_ZACH_OLIMP2");

                entity.ToTable("EGOROVA_ZACH_OLIMP_B1");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.Documenttypeid).HasColumnName("DOCUMENTTYPEID");

                entity.Property(e => e.Year).HasColumnName("YEAR");
            });

            modelBuilder.Entity<EgorovaZachOlimpB2>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EGOROVA_ZACH_OLIMP_B2");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.Documenttypeid).HasColumnName("DOCUMENTTYPEID");

                entity.Property(e => e.Year).HasColumnName("YEAR");
            });

            modelBuilder.Entity<EgorovaZachOlimpB3>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EGOROVA_ZACH_OLIMP_B3");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.Documenttypeid).HasColumnName("DOCUMENTTYPEID");

                entity.Property(e => e.Year).HasColumnName("YEAR");
            });

            modelBuilder.Entity<El>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EL");

                entity.Property(e => e.Id1).HasColumnName("id1");

                entity.Property(e => e.Id2).HasColumnName("id2");
            });

            modelBuilder.Entity<EntHistAft>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("__ENT_HIST_AFT");

                entity.Property(e => e.DocumentNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.EntrantId).HasColumnName("EntrantID");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.ParticipantId).HasColumnName("ParticipantID");

                entity.Property(e => e.PersonId).HasColumnName("PersonID");

                entity.Property(e => e.SecondName)
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.Surname)
                    .HasMaxLength(80)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EntHistBef>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("__ENT_HIST_BEF");

                entity.Property(e => e.DocumentNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.EntrantId).HasColumnName("EntrantID");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.ParticipantId).HasColumnName("ParticipantID");

                entity.Property(e => e.PersonId).HasColumnName("PersonID");

                entity.Property(e => e.SecondName)
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.Surname)
                    .HasMaxLength(80)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EntUnlinked>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("__ENT_UNLINKED");

                entity.Property(e => e.EntrantId).HasColumnName("EntrantID");
            });

            modelBuilder.Entity<EntranceTestCreativeDirection>(entity =>
            {
                entity.HasKey(e => e.DirectionId)
                    .HasName("PK_EntranceTestCreativeDirections");

                entity.Property(e => e.DirectionId)
                    .HasColumnName("DirectionID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Direction)
                    .WithOne(p => p.EntranceTestCreativeDirection)
                    .HasForeignKey<EntranceTestCreativeDirection>(d => d.DirectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EntranceTestCreativeDirections_Direction");
            });

            modelBuilder.Entity<EntranceTestItemC>(entity =>
            {
                entity.HasKey(e => e.EntranceTestItemId);

                entity.HasIndex(e => e.ReplacedEntranceTestItemId)
                    .HasName("I_EntranceTestItemC_ReplacedEntranceTestItemID");

                entity.HasIndex(e => e.SubjectId)
                    .HasName("I_EntranceTestItemC_SubjectID");

                entity.HasIndex(e => new { e.CompetitiveGroupId, e.SubjectId, e.EntranceTestTypeId, e.SubjectName })
                    .HasName("UK_EntranceTestItemC")
                    .IsUnique();

                entity.HasIndex(e => new { e.EntranceTestTypeId, e.IsForSpoandVo, e.ReplacedEntranceTestItemId, e.CompetitiveGroupId, e.SubjectId })
                    .HasName("I_EntranceTestItemC_ForEge");

                entity.Property(e => e.EntranceTestItemId).HasColumnName("EntranceTestItemID");

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EntranceTestItemGuid).HasColumnName("EntranceTestItemGUID");

                entity.Property(e => e.EntranceTestTypeId).HasColumnName("EntranceTestTypeID");

                entity.Property(e => e.IsForSpoandVo).HasColumnName("IsForSPOandVO");

                entity.Property(e => e.MinScore).HasColumnType("decimal(7, 4)");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ReplacedEntranceTestItemId).HasColumnName("ReplacedEntranceTestItemID");

                entity.Property(e => e.SubjectId).HasColumnName("SubjectID");

                entity.Property(e => e.SubjectName)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.CompetitiveGroup)
                    .WithMany(p => p.EntranceTestItemC)
                    .HasForeignKey(d => d.CompetitiveGroupId)
                    .HasConstraintName("FK_EntranceTestItemC_CompetitiveGroupID");

                entity.HasOne(d => d.EntranceTestType)
                    .WithMany(p => p.EntranceTestItemC)
                    .HasForeignKey(d => d.EntranceTestTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EntranceTestItemC_EntranceTestType");

                entity.HasOne(d => d.ReplacedEntranceTestItem)
                    .WithMany(p => p.InverseReplacedEntranceTestItem)
                    .HasForeignKey(d => d.ReplacedEntranceTestItemId)
                    .HasConstraintName("FK_EntranceTestItemC_EntranceTestItemC");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.EntranceTestItemC)
                    .HasForeignKey(d => d.SubjectId)
                    .HasConstraintName("FK_EntranceTestItemC_Subject");
            });

            modelBuilder.Entity<EntranceTestProfileDirection>(entity =>
            {
                entity.HasKey(e => e.DirectionId);

                entity.Property(e => e.DirectionId)
                    .HasColumnName("DirectionID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Direction)
                    .WithOne(p => p.EntranceTestProfileDirection)
                    .HasForeignKey<EntranceTestProfileDirection>(d => d.DirectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EntranceTestProfileDirection_DirectionID");
            });

            modelBuilder.Entity<EntranceTestResultSource>(entity =>
            {
                entity.HasKey(e => e.SourceId);

                entity.Property(e => e.SourceId)
                    .HasColumnName("SourceID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<EntranceTestType>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UK_EntranceTestType_Name")
                    .IsUnique();

                entity.Property(e => e.EntranceTestTypeId).HasColumnName("EntranceTestTypeID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Entrant>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("_Entrant");

                entity.Property(e => e.EntrantId).HasColumnName("EntrantID");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");
            });

            modelBuilder.Entity<Entrant1>(entity =>
            {
                entity.HasKey(e => e.EntrantId);

                entity.ToTable("Entrant");

                entity.HasIndex(e => e.CreatedDate)
                    .HasName("I_Entrant_CreatedDate");

                entity.HasIndex(e => e.IdentityDocumentId)
                    .HasName("I_Entrant_IdentityDocumentID");

                entity.HasIndex(e => e.InstitutionId)
                    .HasName("I_Entrant_InstitutionID");

                entity.HasIndex(e => e.PersonId)
                    .HasName("I_Entrant_PersonId");

                entity.Property(e => e.EntrantId).HasColumnName("EntrantID");

                entity.Property(e => e.Address)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CustomInformation).IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.EntrantGuid).HasColumnName("EntrantGUID");

                entity.Property(e => e.FactAddressId).HasColumnName("FactAddressID");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.GenderId).HasColumnName("GenderID");

                entity.Property(e => e.IdentityDocumentId).HasColumnName("IdentityDocumentID");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.IsFromKrymEntrantDocumentId).HasColumnName("IsFromKrymEntrantDocumentID");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MobilePhone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PersonLinkDate).HasColumnType("datetime");

                entity.Property(e => e.RegionId)
                    .HasColumnName("RegionID")
                    .HasDefaultValueSql("((1000))");

                entity.Property(e => e.RegistrationAddressId).HasColumnName("RegistrationAddressID");

                entity.Property(e => e.Snils)
                    .HasColumnName("SNILS")
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.TownTypeId).HasColumnName("TownTypeID");

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.FactAddress)
                    .WithMany(p => p.Entrant1FactAddress)
                    .HasForeignKey(d => d.FactAddressId)
                    .HasConstraintName("FK_Entrant_Address_Fact");

                entity.HasOne(d => d.Gender)
                    .WithMany(p => p.Entrant1)
                    .HasForeignKey(d => d.GenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Entrant_GenderType");

                entity.HasOne(d => d.IdentityDocument)
                    .WithMany(p => p.Entrant1IdentityDocument)
                    .HasForeignKey(d => d.IdentityDocumentId)
                    .HasConstraintName("FK_Entrant_EntrantDocument");

                entity.HasOne(d => d.Institution)
                    .WithMany(p => p.Entrant1)
                    .HasForeignKey(d => d.InstitutionId)
                    .HasConstraintName("FK_Entrant_Institution");

                entity.HasOne(d => d.IsFromKrymEntrantDocument)
                    .WithMany(p => p.Entrant1IsFromKrymEntrantDocument)
                    .HasForeignKey(d => d.IsFromKrymEntrantDocumentId)
                    .HasConstraintName("FK_Entrant_KrymEntrantDocument");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Entrant1)
                    .HasForeignKey(d => d.PersonId)
                    .HasConstraintName("FK_Entrant_RVIPersons");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.Entrant1)
                    .HasForeignKey(d => d.RegionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Entrant_RegionType");

                entity.HasOne(d => d.RegistrationAddress)
                    .WithMany(p => p.Entrant1RegistrationAddress)
                    .HasForeignKey(d => d.RegistrationAddressId)
                    .HasConstraintName("FK_Entrant_Address_Registration");

                entity.HasOne(d => d.TownType)
                    .WithMany(p => p.Entrant1)
                    .HasForeignKey(d => d.TownTypeId)
                    .HasConstraintName("FK_Entrant_TownType");
            });

            modelBuilder.Entity<EntrantDocument>(entity =>
            {
                entity.HasIndex(e => e.AttachmentId)
                    .HasName("I_EntrantDocument_AttachmentID");

                entity.HasIndex(e => new { e.EntrantId, e.DocumentTypeId })
                    .HasName("<Name of Missing Index, sysname,>");

                entity.HasIndex(e => new { e.DocumentNumber, e.DocumentSeries, e.DocumentTypeId })
                    .HasName("I_EntrantDocument_NumberSeriesType");

                entity.HasIndex(e => new { e.EntrantId, e.DocumentNumber, e.DocumentTypeId, e.DocumentDate })
                    .HasName("I_EntrantDocument_EntrantIDNumber");

                entity.Property(e => e.EntrantDocumentId).HasColumnName("EntrantDocumentID");

                entity.Property(e => e.AttachmentId).HasColumnName("AttachmentID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DocumentDate).HasColumnType("datetime");

                entity.Property(e => e.DocumentName)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentOrganization)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentSeries)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentSpecificData)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.DocumentTypeId).HasColumnName("DocumentTypeID");

                entity.Property(e => e.EntrantDocumentGuid).HasColumnName("EntrantDocumentGUID");

                entity.Property(e => e.EntrantId).HasColumnName("EntrantID");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.Attachment)
                    .WithMany(p => p.EntrantDocument)
                    .HasForeignKey(d => d.AttachmentId)
                    .HasConstraintName("FK_EntrantDocument_Attachment");

                entity.HasOne(d => d.DocumentType)
                    .WithMany(p => p.EntrantDocument)
                    .HasForeignKey(d => d.DocumentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EntrantDocument_DocumentType");

                entity.HasOne(d => d.Entrant)
                    .WithMany(p => p.EntrantDocument)
                    .HasForeignKey(d => d.EntrantId)
                    .HasConstraintName("FK_EntrantDocument_Entrant");
            });

            modelBuilder.Entity<EntrantDocumentCompatriot>(entity =>
            {
                entity.HasNoKey();

                entity.HasIndex(e => e.EntrantDocumentId)
                    .HasName("I_EntrantDocumentCompatriot_EntrantDocumentID");

                entity.Property(e => e.CompatriotCategoryId).HasColumnName("CompatriotCategoryID");

                entity.Property(e => e.EntrantDocumentId).HasColumnName("EntrantDocumentID");

                entity.HasOne(d => d.CompatriotCategory)
                    .WithMany()
                    .HasForeignKey(d => d.CompatriotCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EntrantDocumentCompatriot_CompatriotCategory");

                entity.HasOne(d => d.EntrantDocument)
                    .WithMany()
                    .HasForeignKey(d => d.EntrantDocumentId)
                    .HasConstraintName("FK_EntrantDocumentCompatriot_EntrantDocument");
            });

            modelBuilder.Entity<EntrantDocumentCustom>(entity =>
            {
                entity.HasNoKey();

                entity.HasIndex(e => e.EntrantDocumentId)
                    .HasName("I_EntrantDocumentCustom_EntrantDocumentID");

                entity.Property(e => e.AdditionalInfo).IsUnicode(false);

                entity.Property(e => e.EntrantDocumentId).HasColumnName("EntrantDocumentID");

                entity.HasOne(d => d.EntrantDocument)
                    .WithMany()
                    .HasForeignKey(d => d.EntrantDocumentId)
                    .HasConstraintName("FK_EntrantDocumentCustom_EntrantDocument");
            });

            modelBuilder.Entity<EntrantDocumentDisability>(entity =>
            {
                entity.HasNoKey();

                entity.HasIndex(e => e.EntrantDocumentId)
                    .HasName("I_EntrantDocumentDisability_EntrantDocumentID");

                entity.Property(e => e.DisabilityTypeId).HasColumnName("DisabilityTypeID");

                entity.Property(e => e.EntrantDocumentId).HasColumnName("EntrantDocumentID");

                entity.HasOne(d => d.EntrantDocument)
                    .WithMany()
                    .HasForeignKey(d => d.EntrantDocumentId)
                    .HasConstraintName("FK_EntrantDocumentDisability_EntrantDocument");
            });

            modelBuilder.Entity<EntrantDocumentEdu>(entity =>
            {
                entity.HasNoKey();

                entity.HasIndex(e => e.EntrantDocumentId)
                    .HasName("I_EntrantDocumentEdu_EntrantDocumentID");

                entity.Property(e => e.ActComment)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ActDate).HasColumnType("datetime");

                entity.Property(e => e.ActNumber)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.AdditionalFirstName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.AdditionalLastName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.AdditionalMiddleName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.BlankAttachmentId).HasColumnName("BlankAttachmentID");

                entity.Property(e => e.BlankDate).HasColumnType("datetime");

                entity.Property(e => e.BlankRegNumber)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.EntrantDocumentId).HasColumnName("EntrantDocumentID");

                entity.Property(e => e.FrdoDocName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.FrdoOrganization)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.FrdoQualification)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.FrdoSpeciality)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Gpa).HasColumnName("GPA");

                entity.Property(e => e.InstitutionName)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.NostrificatedComment)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.QualificationName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RegistrationNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SpecialityName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.BlankAttachment)
                    .WithMany()
                    .HasForeignKey(d => d.BlankAttachmentId)
                    .HasConstraintName("FK_EntrantDocumentEdu_to_BlankAttachment");

                entity.HasOne(d => d.Country)
                    .WithMany()
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK_EntrantDocumentEdu_to_CountryType");

                entity.HasOne(d => d.EntrantDocument)
                    .WithMany()
                    .HasForeignKey(d => d.EntrantDocumentId)
                    .HasConstraintName("FK_EntrantDocumentEdu_EntrantDocument");

                entity.HasOne(d => d.NostrificationType)
                    .WithMany()
                    .HasForeignKey(d => d.NostrificationTypeId)
                    .HasConstraintName("FK_EntrantDocumentEdu_to_NostrificationTypes");
            });

            modelBuilder.Entity<EntrantDocumentEge>(entity =>
            {
                entity.HasNoKey();

                entity.HasIndex(e => e.EntrantDocumentId)
                    .HasName("I_EntrantDocumentEge_EntrantDocumentID");

                entity.Property(e => e.Decision)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.DecisionDate).HasColumnType("datetime");

                entity.Property(e => e.DecisionNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.EntrantDocumentId).HasColumnName("EntrantDocumentID");

                entity.Property(e => e.TypographicNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.EntrantDocument)
                    .WithMany()
                    .HasForeignKey(d => d.EntrantDocumentId)
                    .HasConstraintName("FK_EntrantDocumentEge_EntrantDocument");
            });

            modelBuilder.Entity<EntrantDocumentEgeAndOlympicSubject>(entity =>
            {
                entity.HasNoKey();

                entity.HasIndex(e => e.EntrantDocumentId)
                    .HasName("I_EntrantDocumentEgeAndOlympicSubject_EntrantDocumentID");

                entity.Property(e => e.AppealStatusId).HasColumnName("AppealStatusID");

                entity.Property(e => e.EntrantDocumentId).HasColumnName("EntrantDocumentID");

                entity.Property(e => e.SubjectId).HasColumnName("SubjectID");

                entity.Property(e => e.Value).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.AppealStatus)
                    .WithMany()
                    .HasForeignKey(d => d.AppealStatusId)
                    .HasConstraintName("FK_EntrantDocumentEgeAndOlympicSubject_AppealStatus");

                entity.HasOne(d => d.EntrantDocument)
                    .WithMany()
                    .HasForeignKey(d => d.EntrantDocumentId)
                    .HasConstraintName("FK_EntrantDocumentEgeAndOlympicSubject_EntrantDocument");
            });

            modelBuilder.Entity<EntrantDocumentIdentity>(entity =>
            {
                entity.HasNoKey();

                entity.HasIndex(e => e.EntrantDocumentId)
                    .HasName("I_EntrantDocumentIdentity_EntrantDocumentID");

                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.BirthPlace)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.EntrantDocumentId).HasColumnName("EntrantDocumentID");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.GenderTypeId).HasColumnName("GenderTypeID");

                entity.Property(e => e.IdentityDocumentTypeId).HasColumnName("IdentityDocumentTypeID");

                entity.Property(e => e.LastName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NationalityTypeId).HasColumnName("NationalityTypeID");

                entity.Property(e => e.SubdivisionCode)
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.HasOne(d => d.EntrantDocument)
                    .WithMany()
                    .HasForeignKey(d => d.EntrantDocumentId)
                    .HasConstraintName("FK_EntrantDocumentIdentity_EntrantDocument");

                entity.HasOne(d => d.IdentityDocumentType)
                    .WithMany()
                    .HasForeignKey(d => d.IdentityDocumentTypeId)
                    .HasConstraintName("FK_EntrantDocumentIdentity_IdentityDocumentType");

                entity.HasOne(d => d.NationalityType)
                    .WithMany()
                    .HasForeignKey(d => d.NationalityTypeId)
                    .HasConstraintName("FK_EntrantDocumentIdentity_CountryType");
            });

            modelBuilder.Entity<EntrantDocumentIdentityTmp>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EntrantDocumentIdentity_tmp");

                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.BirthPlace)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.EntrantDocumentId).HasColumnName("EntrantDocumentID");

                entity.Property(e => e.GenderTypeId).HasColumnName("GenderTypeID");

                entity.Property(e => e.IdentityDocumentTypeId).HasColumnName("IdentityDocumentTypeID");

                entity.Property(e => e.NationalityTypeId).HasColumnName("NationalityTypeID");

                entity.Property(e => e.SubdivisionCode)
                    .HasMaxLength(7)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EntrantDocumentInternationalOlympic>(entity =>
            {
                entity.HasNoKey();

                entity.HasIndex(e => e.EntrantDocumentId)
                    .HasName("I_EntrantDocumentInternationalOlympic_EntrantDocumentID");

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.EntrantDocumentId).HasColumnName("EntrantDocumentID");

                entity.Property(e => e.OlympicDate).HasColumnType("datetime");

                entity.Property(e => e.OlympicPlace)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Profile)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Country)
                    .WithMany()
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EntrantDocumentInternationalOlympic_CountryType");

                entity.HasOne(d => d.EntrantDocument)
                    .WithMany()
                    .HasForeignKey(d => d.EntrantDocumentId)
                    .HasConstraintName("FK_EntrantDocumentInternationalOlympic_EntrantDocument");
            });

            modelBuilder.Entity<EntrantDocumentMarks>(entity =>
            {
                entity.HasKey(e => e.EntrantDocumentId)
                    .HasName("PK__EntrantD__C54A675C61316BF4");

                entity.HasComment("Баллы по предметам, указанные в документах абитуриента ");

                entity.Property(e => e.EntrantDocumentId)
                    .HasColumnName("EntrantDocumentID")
                    .HasComment("ИД документа абитуриента FK (EntrantDocument - EntrantDocumentID");

                entity.Property(e => e.SubjectId)
                    .HasColumnName("SubjectID")
                    .HasComment("ИД предмета FK (Subject¬ - Subject¬ID) ");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.EntrantDocumentMarks)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EntrantDocumentMarks_Subject");
            });

            modelBuilder.Entity<EntrantDocumentOlympic>(entity =>
            {
                entity.HasNoKey();

                entity.HasIndex(e => e.EntrantDocumentId)
                    .HasName("PK_EntrantOlympicDocument")
                    .IsClustered();

                entity.Property(e => e.DiplomaTypeId).HasColumnName("DiplomaTypeID");

                entity.Property(e => e.EgeSubjectId).HasColumnName("EgeSubjectID");

                entity.Property(e => e.EntrantDocumentId).HasColumnName("EntrantDocumentID");

                entity.Property(e => e.OlympicId).HasColumnName("OlympicID");

                entity.Property(e => e.OlympicTypeProfileId).HasColumnName("OlympicTypeProfileID");

                entity.Property(e => e.ProfileSubjectId).HasColumnName("ProfileSubjectID");

                entity.HasOne(d => d.EgeSubject)
                    .WithMany()
                    .HasForeignKey(d => d.EgeSubjectId)
                    .HasConstraintName("FK_EntrantDocumentOlympic_EgeSubject");

                entity.HasOne(d => d.EntrantDocument)
                    .WithMany()
                    .HasForeignKey(d => d.EntrantDocumentId)
                    .HasConstraintName("FK_EntrantDocumentOlympic_EntrantDocument");

                entity.HasOne(d => d.OlympicTypeProfile)
                    .WithMany()
                    .HasForeignKey(d => d.OlympicTypeProfileId)
                    .HasConstraintName("FK_EntrantDocumentOlympic_OlympicTypeProfile");

                entity.HasOne(d => d.ProfileSubject)
                    .WithMany()
                    .HasForeignKey(d => d.ProfileSubjectId)
                    .HasConstraintName("FK_EntrantDocumentOlympic_ProfileSubject");
            });

            modelBuilder.Entity<EntrantDocumentOrphan>(entity =>
            {
                entity.HasNoKey();

                entity.HasIndex(e => e.EntrantDocumentId)
                    .HasName("I_EntrantDocumentOrphan_EntrantDocumentID");

                entity.Property(e => e.EntrantDocumentId).HasColumnName("EntrantDocumentID");

                entity.Property(e => e.OrphanCategoryId).HasColumnName("OrphanCategoryID");

                entity.HasOne(d => d.EntrantDocument)
                    .WithMany()
                    .HasForeignKey(d => d.EntrantDocumentId)
                    .HasConstraintName("FK_EntrantDocumentOrphan_EntrantDocument");

                entity.HasOne(d => d.OrphanCategory)
                    .WithMany()
                    .HasForeignKey(d => d.OrphanCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EntrantDocumentOrphan_OrphanCategory");
            });

            modelBuilder.Entity<EntrantDocumentParentsLost>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.EntrantDocumentId).HasColumnName("EntrantDocumentID");

                entity.Property(e => e.ParentsLostCategoryId).HasColumnName("ParentsLostCategoryID");

                entity.HasOne(d => d.EntrantDocument)
                    .WithMany()
                    .HasForeignKey(d => d.EntrantDocumentId)
                    .HasConstraintName("FK_EntrantDocumentParentsLost_EntrantDocument");

                entity.HasOne(d => d.ParentsLostCategory)
                    .WithMany()
                    .HasForeignKey(d => d.ParentsLostCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EntrantDocumentParentsLost_ParentsLostCategory");
            });

            modelBuilder.Entity<EntrantDocumentRadiationWork>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.EntrantDocumentId).HasColumnName("EntrantDocumentID");

                entity.Property(e => e.RadiationWorkCategoryId).HasColumnName("RadiationWorkCategoryID");

                entity.HasOne(d => d.EntrantDocument)
                    .WithMany()
                    .HasForeignKey(d => d.EntrantDocumentId)
                    .HasConstraintName("FK_EntrantDocumentRadiationWork_EntrantDocument");

                entity.HasOne(d => d.RadiationWorkCategory)
                    .WithMany()
                    .HasForeignKey(d => d.RadiationWorkCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EntrantDocumentRadiationWork_RadiationWorkCategory");
            });

            modelBuilder.Entity<EntrantDocumentStateEmployee>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.EntrantDocumentId).HasColumnName("EntrantDocumentID");

                entity.Property(e => e.StateEmployeeCategoryId).HasColumnName("StateEmployeeCategoryID");

                entity.HasOne(d => d.EntrantDocument)
                    .WithMany()
                    .HasForeignKey(d => d.EntrantDocumentId)
                    .HasConstraintName("FK_EntrantDocumentStateEmployee_EntrantDocument");

                entity.HasOne(d => d.StateEmployeeCategory)
                    .WithMany()
                    .HasForeignKey(d => d.StateEmployeeCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EntrantDocumentStateEmployee_StateEmployeeCategory");
            });

            modelBuilder.Entity<EntrantDocumentUkraineOlympic>(entity =>
            {
                entity.HasNoKey();

                entity.HasIndex(e => e.EntrantDocumentId)
                    .HasName("I_EntrantDocumentUkraineOlympic_EntrantDocumentID");

                entity.Property(e => e.DiplomaTypeId).HasColumnName("DiplomaTypeID");

                entity.Property(e => e.EntrantDocumentId).HasColumnName("EntrantDocumentID");

                entity.Property(e => e.OlympicDate).HasColumnType("datetime");

                entity.Property(e => e.OlympicPlace)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Profile)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.DiplomaType)
                    .WithMany()
                    .HasForeignKey(d => d.DiplomaTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EntrantDocumentUkraineOlympic_OlympicDiplomType");

                entity.HasOne(d => d.EntrantDocument)
                    .WithMany()
                    .HasForeignKey(d => d.EntrantDocumentId)
                    .HasConstraintName("FK_EntrantDocumentUkraineOlympic_EntrantDocument");
            });

            modelBuilder.Entity<EntrantDocumentVeteran>(entity =>
            {
                entity.HasNoKey();

                entity.HasIndex(e => e.EntrantDocumentId)
                    .HasName("I_EntrantDocumentVeteran_EntrantDocumentID");

                entity.Property(e => e.EntrantDocumentId).HasColumnName("EntrantDocumentID");

                entity.Property(e => e.VeteranCategoryId).HasColumnName("VeteranCategoryID");

                entity.HasOne(d => d.EntrantDocument)
                    .WithMany()
                    .HasForeignKey(d => d.EntrantDocumentId)
                    .HasConstraintName("FK_EntrantDocumentVeteran_EntrantDocument");

                entity.HasOne(d => d.VeteranCategory)
                    .WithMany()
                    .HasForeignKey(d => d.VeteranCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EntrantDocumentVeteran_VeteranCategory");
            });

            modelBuilder.Entity<EntrantLanguage>(entity =>
            {
                entity.HasIndex(e => new { e.EntrantId, e.LanguageId })
                    .HasName("UK_EntrantLanguage")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EntrantId).HasColumnName("EntrantID");

                entity.Property(e => e.LanguageId).HasColumnName("LanguageID");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Entrant)
                    .WithMany(p => p.EntrantLanguage)
                    .HasForeignKey(d => d.EntrantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EntrantLanguage_Entrant");

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.EntrantLanguage)
                    .HasForeignKey(d => d.LanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EntrantLanguage_ForeignLanguageType");
            });

            modelBuilder.Entity<FbsToFisMap>(entity =>
            {
                entity.HasOne(d => d.IdSubjectFisNavigation)
                    .WithMany(p => p.FbsToFisMap)
                    .HasForeignKey(d => d.IdSubjectFis)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FbsToFisMap_Subject");
            });

            modelBuilder.Entity<FinalStudent>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("final_student");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.Bday)
                    .HasColumnName("bday")
                    .HasColumnType("datetime");

                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.DirectionName)
                    .IsRequired()
                    .HasColumnName("Direction_name")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentSeries)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.EducationLevelId).HasColumnName("EducationLevelID");

                entity.Property(e => e.EntrantId).HasColumnName("EntrantID");

                entity.Property(e => e.Fio)
                    .IsRequired()
                    .HasColumnName("FIO")
                    .HasMaxLength(48)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MyId).HasColumnName("my_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NewCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Post)
                    .IsRequired()
                    .HasColumnName("post")
                    .HasMaxLength(9)
                    .IsUnicode(false);

                entity.Property(e => e.ИсточникФинансирования)
                    .HasColumnName("Источник финансирования")
                    .HasMaxLength(38)
                    .IsUnicode(false);

                entity.Property(e => e.СтатусЗаявления)
                    .IsRequired()
                    .HasColumnName("Статус заявления")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.УровеньОбразования)
                    .HasColumnName("Уровень образования")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.ФормаОбучения)
                    .HasColumnName("Форма обучения")
                    .HasMaxLength(23)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FinalStudentDuble>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("final_student_duble");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.Bday)
                    .HasColumnName("bday")
                    .HasColumnType("datetime");

                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.DirectionName)
                    .IsRequired()
                    .HasColumnName("Direction_name")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentSeries)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.EducationLevelId).HasColumnName("EducationLevelID");

                entity.Property(e => e.EntrantId).HasColumnName("EntrantID");

                entity.Property(e => e.Fio)
                    .IsRequired()
                    .HasColumnName("FIO")
                    .HasMaxLength(48)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MyId).HasColumnName("my_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NewCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Post)
                    .IsRequired()
                    .HasColumnName("post")
                    .HasMaxLength(9)
                    .IsUnicode(false);

                entity.Property(e => e.ИсточникФинансирования)
                    .HasColumnName("Источник финансирования")
                    .HasMaxLength(38)
                    .IsUnicode(false);

                entity.Property(e => e.СтатусЗаявления)
                    .IsRequired()
                    .HasColumnName("Статус заявления")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.УровеньОбразования)
                    .HasColumnName("Уровень образования")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.ФормаОбучения)
                    .HasColumnName("Форма обучения")
                    .HasMaxLength(23)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FinalTable>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("final_table");

                entity.Property(e => e.BDate)
                    .IsRequired()
                    .HasColumnName("b_date")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.BudgContr)
                    .HasColumnName("budg_contr")
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentSeries)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Fio)
                    .IsRequired()
                    .HasColumnName("FIO")
                    .HasMaxLength(41)
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.MyStudentsFk).HasColumnName("my_students_fk");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(120)
                    .IsUnicode(false);

                entity.Property(e => e.OchnayaBakSpec)
                    .IsRequired()
                    .HasColumnName("ochnaya_bak_spec")
                    .HasMaxLength(3)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FinalTable2911>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("final_table_2911");

                entity.Property(e => e.BakSpecOchnaya)
                    .IsRequired()
                    .HasColumnName("bak_spec_ochnaya")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.DirectionCode).HasMaxLength(100);

                entity.Property(e => e.DirectionName).HasMaxLength(500);

                entity.Property(e => e.EduForm).HasMaxLength(100);

                entity.Property(e => e.EduLevel).HasMaxLength(100);

                entity.Property(e => e.EduSource4).HasMaxLength(100);

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IstFin)
                    .IsRequired()
                    .HasColumnName("ist_fin")
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Ssuz)
                    .HasColumnName("ssuz")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Vuz)
                    .HasColumnName("vuz")
                    .HasMaxLength(1000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FindPathTable>(entity =>
            {
                entity.HasKey(e => e.Nomer)
                    .HasName("PK_findPathTable_1");

                entity.ToTable("findPathTable");

                entity.Property(e => e.Nomer).ValueGeneratedNever();

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Naf)
                    .IsRequired()
                    .HasColumnName("NAF")
                    .HasMaxLength(50);

                entity.Property(e => e.Number)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Series)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<FindPathTableEge>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("findPathTableEGE");

                entity.Property(e => e.DocumentNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentSeries)
                    .HasMaxLength(9)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.ParticipantId).HasColumnName("ParticipantID");

                entity.Property(e => e.SecondName)
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.Surname)
                    .HasMaxLength(80)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FindPathTableShort>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("findPathTable_short");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Naf)
                    .IsRequired()
                    .HasColumnName("NAF")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ForeignInstitutions>(entity =>
            {
                entity.Property(e => e.Name).IsUnicode(false);

                entity.HasOne(d => d.CountryType)
                    .WithMany(p => p.ForeignInstitutions)
                    .HasForeignKey(d => d.CountryTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CountryTypeConstraint");

                entity.HasOne(d => d.EduLevel)
                    .WithMany(p => p.ForeignInstitutions)
                    .HasForeignKey(d => d.EduLevelId)
                    .HasConstraintName("EduLevelConstraint");
            });

            modelBuilder.Entity<ForeignLanguageType>(entity =>
            {
                entity.HasKey(e => e.LanguageId);

                entity.HasIndex(e => e.Name)
                    .HasName("UK_ForeignLanguageType_Name")
                    .IsUnique();

                entity.Property(e => e.LanguageId).HasColumnName("LanguageID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FormOfLaw>(entity =>
            {
                entity.HasIndex(e => e.FormOfLawId)
                    .HasName("UK_FormOfLaw_Name")
                    .IsUnique();

                entity.Property(e => e.FormOfLawId).HasColumnName("FormOfLawID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DisplayOrder).HasDefaultValueSql("((1))");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FtcRandDouble>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ftc_RandDouble");
            });

            modelBuilder.Entity<GenderType>(entity =>
            {
                entity.HasKey(e => e.GenderId);

                entity.Property(e => e.GenderId)
                    .HasColumnName("GenderID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GetBenefitComplectiveGroup>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("GetBenefitComplectiveGroup");

                entity.Property(e => e.BenefitId).HasColumnName("BenefitID");

                entity.Property(e => e.BenefitItemGuid).HasColumnName("BenefitItemGUID");

                entity.Property(e => e.BenefitItemId).HasColumnName("BenefitItemID");

                entity.Property(e => e.BenefitName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.BenefitShortName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.EntranceTestItemId).HasColumnName("EntranceTestItemID");

                entity.Property(e => e.OlympicDiplomTypeId).HasColumnName("OlympicDiplomTypeID");

                entity.Property(e => e.OlympicDiplomTypeName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GetCompetitiveGroup>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("GetCompetitiveGroup");

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.DirectName)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.DirectionId).HasColumnName("DirectionID");

                entity.Property(e => e.EducationFormName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EducationLevelId).HasColumnName("EducationLevelID");

                entity.Property(e => e.EducationLevelName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EducationSourceName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.NewCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NumberBudgetOz).HasColumnName("NumberBudgetOZ");

                entity.Property(e => e.NumberPaidOz).HasColumnName("NumberPaidOZ");

                entity.Property(e => e.NumberQuotaOz).HasColumnName("NumberQuotaOZ");

                entity.Property(e => e.NumberTargetOz).HasColumnName("NumberTargetOZ");

                entity.Property(e => e.ParentName)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GetCompetitiveGroupProgram>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("GetCompetitiveGroupProgram");

                entity.Property(e => e.Code)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.CompetitiveGroupProgramId).HasColumnName("CompetitiveGroupProgramID");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.InstitutionProgramId).HasColumnName("InstitutionProgramID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GetEntranceTestItemC>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("GetEntranceTestItemC");

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.EntranceTestItemId).HasColumnName("EntranceTestItemID");

                entity.Property(e => e.EntranceTestTypeId).HasColumnName("EntranceTestTypeID");

                entity.Property(e => e.EntranceTestTypeName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Guid).HasColumnName("GUID");

                entity.Property(e => e.MinScore).HasColumnType("decimal(7, 4)");

                entity.Property(e => e.SubjectId).HasColumnName("SubjectID");

                entity.Property(e => e.SubjectName)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.SubjectOlympName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GetInstitutionAchievements>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("GetInstitutionAchievements");

                entity.Property(e => e.AchivementName)
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.MaxValue).HasColumnType("decimal(7, 4)");

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GetInstitutionCampaign>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("GetInstitutionCampaign");

                entity.Property(e => e.BriefName)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.CampageTypeName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");

                entity.Property(e => e.CampaignTypeId).HasColumnName("CampaignTypeID");

                entity.Property(e => e.DirectName)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.DirectionId).HasColumnName("DirectionID");

                entity.Property(e => e.EduLevelName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EduLevelTypeId).HasColumnName("EduLevelTypeID");

                entity.Property(e => e.EiisId)
                    .HasColumnName("EIIS_ID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Inn)
                    .HasColumnName("INN")
                    .HasMaxLength(14)
                    .IsUnicode(false);

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.Kpp)
                    .HasColumnName("KPP")
                    .HasMaxLength(13)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NewCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NumberBudgetOz).HasColumnName("NumberBudgetOZ");

                entity.Property(e => e.NumberPaidOz).HasColumnName("NumberPaidOZ");

                entity.Property(e => e.NumberQuotaOz).HasColumnName("NumberQuotaOZ");

                entity.Property(e => e.NumberTargetOz).HasColumnName("NumberTargetOZ");

                entity.Property(e => e.Ogrn)
                    .HasColumnName("OGRN")
                    .HasMaxLength(18)
                    .IsUnicode(false);

                entity.Property(e => e.ParentName)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GetOlympics>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("GetOlympics");

                entity.Property(e => e.CoOrganizerId).HasColumnName("CoOrganizerID");

                entity.Property(e => e.Email)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.LastName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.OlympicLevelId).HasColumnName("OlympicLevelID");

                entity.Property(e => e.OlympicLevelName)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.OlympicProfileId).HasColumnName("OlympicProfileID");

                entity.Property(e => e.OlympicTypeId).HasColumnName("OlympicTypeID");

                entity.Property(e => e.OlympicTypeName)
                    .IsRequired()
                    .HasMaxLength(1023)
                    .IsUnicode(false);

                entity.Property(e => e.OlympicTypeProfileId).HasColumnName("OlympicTypeProfileID");

                entity.Property(e => e.OrgOlympicEnterId).HasColumnName("OrgOlympicEnterID");

                entity.Property(e => e.OrganizerAddress)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.OrganizerName)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Position)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ProfileName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GetOlympicsDiplomants>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("GetOlympicsDiplomants");

                entity.Property(e => e.AdoptionUnfoundedComment).IsUnicode(false);

                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.Comment).IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.DateIssue).HasColumnType("datetime");

                entity.Property(e => e.DeleteDate).HasColumnType("datetime");

                entity.Property(e => e.DiplomaDateIssue).HasColumnType("datetime");

                entity.Property(e => e.DiplomaNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DiplomaSeries)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentSeries)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.IdentityDocumentTypeId).HasColumnName("IdentityDocumentTypeID");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.OlympicDiplomantDocumentId).HasColumnName("OlympicDiplomantDocumentID");

                entity.Property(e => e.OlympicDiplomantId).HasColumnName("OlympicDiplomantID");

                entity.Property(e => e.OlympicDiplomantIdentityDocumentId).HasColumnName("OlympicDiplomantIdentityDocumentID");

                entity.Property(e => e.OlympicTypeProfileId).HasColumnName("OlympicTypeProfileID");

                entity.Property(e => e.OrganizationIssue)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.PersonLinkDate).HasColumnType("datetime");

                entity.Property(e => e.ResultLevelId).HasColumnName("ResultLevelID");

                entity.Property(e => e.SchoolEgeName)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.SchoolRegionId).HasColumnName("SchoolRegionID");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");
            });

            modelBuilder.Entity<GetOlympicsSubjects>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("GetOlympicsSubjects");

                entity.Property(e => e.OlympicSubjectId).HasColumnName("OlympicSubjectID");

                entity.Property(e => e.OlympicTypeProfileId).HasColumnName("OlympicTypeProfileID");

                entity.Property(e => e.SubjectId).HasColumnName("SubjectID");

                entity.Property(e => e.SubjectName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GlobalMinEge>(entity =>
            {
                entity.HasKey(e => e.EgeYear);

                entity.Property(e => e.EgeYear).ValueGeneratedNever();
            });

            modelBuilder.Entity<Grands>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("GRANDS");

                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Grants>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("grants");

                entity.Property(e => e.Date)
                    .HasColumnName("DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Fio)
                    .HasColumnName("FIO")
                    .HasMaxLength(1000);
            });

            modelBuilder.Entity<GrantsNew>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("grants_new");

                entity.Property(e => e.Date)
                    .HasColumnName("DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.FirstName).HasMaxLength(1000);

                entity.Property(e => e.LastName).HasMaxLength(1000);

                entity.Property(e => e.MiddleName).HasMaxLength(1000);
            });

            modelBuilder.Entity<Gzgu115>(entity =>
            {
                entity.HasKey(e => new { e.ApplicationId, e.DocumentTypeId, e.CompetitiveGroupId });

                entity.ToTable("gzgu_1_15");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.DocumentTypeId).HasColumnName("DocumentTypeID");

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");
            });

            modelBuilder.Entity<Gzgu1153>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("gzgu_1_15_3");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.DocumentTypeId).HasColumnName("DocumentTypeID");
            });

            modelBuilder.Entity<Gzgu18>(entity =>
            {
                entity.HasKey(e => new { e.ApplicationId, e.CompetitiveGroupId, e.EducationSourceId });

                entity.ToTable("gzgu_1_8");

                entity.HasIndex(e => new { e.ApplicationId, e.CompetitiveGroupId, e.MoreRegDate })
                    .HasName("IDX_GZGU_1_8_2");

                entity.HasIndex(e => new { e.CompetitiveGroupId, e.ApplicationId, e.EducationSourceId, e.MoreRegDate })
                    .HasName("IDX_GZGU_1_8_1");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.MoreRegDate).HasColumnName("moreRegDate");
            });

            modelBuilder.Entity<Gzgu36>(entity =>
            {
                entity.HasKey(e => new { e.DocumentTypeId, e.CompetitiveGroupId });

                entity.ToTable("gzgu_3_6");

                entity.Property(e => e.DocumentTypeId).HasColumnName("DocumentTypeID");

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.Value).HasColumnName("value");
            });

            modelBuilder.Entity<Gzgu361>(entity =>
            {
                entity.HasKey(e => new { e.DocumentTypeId, e.CompetitiveGroupId });

                entity.ToTable("gzgu_3_61");

                entity.Property(e => e.DocumentTypeId).HasColumnName("DocumentTypeID");

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");
            });

            modelBuilder.Entity<Gzgu362>(entity =>
            {
                entity.HasKey(e => e.ApplicationId);

                entity.ToTable("gzgu_3_62");

                entity.Property(e => e.ApplicationId)
                    .HasColumnName("ApplicationID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.DocumentTypeId).HasColumnName("DocumentTypeID");
            });

            modelBuilder.Entity<Gzgu363>(entity =>
            {
                entity.HasKey(e => e.CompetitiveGroupId);

                entity.ToTable("gzgu_3_63");

                entity.Property(e => e.CompetitiveGroupId)
                    .HasColumnName("CompetitiveGroupID")
                    .ValueGeneratedNever();

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.DocumentTypeId).HasColumnName("DocumentTypeID");
            });

            modelBuilder.Entity<Gzgu364>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("gzgu_3_64");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.DocumentTypeId).HasColumnName("DocumentTypeID");
            });

            modelBuilder.Entity<GzguAetd>(entity =>
            {
                entity.HasKey(e => new { e.Idt, e.ApplicationId });

                entity.ToTable("gzgu_aetd");

                entity.HasIndex(e => new { e.ResultValue, e.ApplicationId })
                    .HasName("<Name of Missing Index, sysname,>");

                entity.Property(e => e.Idt)
                    .HasColumnName("IDT")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");
            });

            modelBuilder.Entity<GzguApp24>(entity =>
            {
                entity.HasKey(e => new { e.ApplicationId, e.CompetitiveGroupId });

                entity.ToTable("gzgu_App_24");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.CampaignTypeId).HasColumnName("CampaignTypeID");
            });

            modelBuilder.Entity<GzguApp8>(entity =>
            {
                entity.HasKey(e => new { e.ApplicationId, e.CompetitiveGroupId });

                entity.ToTable("gzgu_App_8");

                entity.HasIndex(e => new { e.CompetitiveGroupId, e.CampaignTypeId, e.EducationSourceId })
                    .HasName("<Name of Missing Index, sysname,>");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.CampaignTypeId).HasColumnName("CampaignTypeID");
            });

            modelBuilder.Entity<GzguAppSources>(entity =>
            {
                entity.HasKey(e => e.ApplicationId);

                entity.ToTable("gzgu_App_Sources");

                entity.Property(e => e.ApplicationId)
                    .HasColumnName("ApplicationID")
                    .ValueGeneratedNever();

                entity.Property(e => e.S1).HasColumnName("s1");

                entity.Property(e => e.S2).HasColumnName("s2");

                entity.Property(e => e.S3).HasColumnName("s3");

                entity.Property(e => e.S4).HasColumnName("s4");

                entity.Property(e => e.S5).HasColumnName("s5");

                entity.Property(e => e.S6).HasColumnName("s6");
            });

            modelBuilder.Entity<GzguTempResult>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("gzgu_temp_result");

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.DirectionId).HasColumnName("DirectionID");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e._110).HasColumnName("1_10");

                entity.Property(e => e._111).HasColumnName("1_11");

                entity.Property(e => e._112).HasColumnName("1_12");

                entity.Property(e => e._113).HasColumnName("1_13");

                entity.Property(e => e._114).HasColumnName("1_14");

                entity.Property(e => e._115).HasColumnName("1_15");

                entity.Property(e => e._116).HasColumnName("1_16");

                entity.Property(e => e._117).HasColumnName("1_17");

                entity.Property(e => e._118).HasColumnName("1_18");

                entity.Property(e => e._119).HasColumnName("1_19");

                entity.Property(e => e._120).HasColumnName("1_20");

                entity.Property(e => e._121).HasColumnName("1_21");

                entity.Property(e => e._122).HasColumnName("1_22");

                entity.Property(e => e._123).HasColumnName("1_23");

                entity.Property(e => e._124).HasColumnName("1_24");

                entity.Property(e => e._125).HasColumnName("1_25");

                entity.Property(e => e._126).HasColumnName("1_26");

                entity.Property(e => e._127).HasColumnName("1_27");

                entity.Property(e => e._128).HasColumnName("1_28");

                entity.Property(e => e._129).HasColumnName("1_29");

                entity.Property(e => e._130).HasColumnName("1_30");

                entity.Property(e => e._131).HasColumnName("1_31");

                entity.Property(e => e._132).HasColumnName("1_32");

                entity.Property(e => e._133).HasColumnName("1_33");

                entity.Property(e => e._134).HasColumnName("1_34");

                entity.Property(e => e._135).HasColumnName("1_35");

                entity.Property(e => e._136).HasColumnName("1_36");

                entity.Property(e => e._137).HasColumnName("1_37");

                entity.Property(e => e._138).HasColumnName("1_38");

                entity.Property(e => e._139).HasColumnName("1_39");

                entity.Property(e => e._14).HasColumnName("1_4");

                entity.Property(e => e._140).HasColumnName("1_40");

                entity.Property(e => e._141).HasColumnName("1_41");

                entity.Property(e => e._142).HasColumnName("1_42");

                entity.Property(e => e._143).HasColumnName("1_43");

                entity.Property(e => e._144).HasColumnName("1_44");

                entity.Property(e => e._145).HasColumnName("1_45");

                entity.Property(e => e._146).HasColumnName("1_46");

                entity.Property(e => e._147).HasColumnName("1_47");

                entity.Property(e => e._148).HasColumnName("1_48");

                entity.Property(e => e._14Cnt).HasColumnName("1_4_cnt");

                entity.Property(e => e._15).HasColumnName("1_5");

                entity.Property(e => e._16).HasColumnName("1_6");

                entity.Property(e => e._17).HasColumnName("1_7");

                entity.Property(e => e._18).HasColumnName("1_8");

                entity.Property(e => e._19).HasColumnName("1_9");

                entity.Property(e => e._21)
                    .HasColumnName("2_1")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._210)
                    .HasColumnName("2_10")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._211)
                    .HasColumnName("2_11")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._212)
                    .HasColumnName("2_12")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._213)
                    .HasColumnName("2_13")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._214)
                    .HasColumnName("2_14")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._215).HasColumnName("2_15");

                entity.Property(e => e._216).HasColumnName("2_16");

                entity.Property(e => e._217).HasColumnName("2_17");

                entity.Property(e => e._218).HasColumnName("2_18");

                entity.Property(e => e._219).HasColumnName("2_19");

                entity.Property(e => e._21Cnt).HasColumnName("2_1_cnt");

                entity.Property(e => e._22)
                    .HasColumnName("2_2")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._220).HasColumnName("2_20");

                entity.Property(e => e._221)
                    .HasColumnName("2_21")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._221Cnt).HasColumnName("2_21_cnt");

                entity.Property(e => e._222)
                    .HasColumnName("2_22")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._222Cnt).HasColumnName("2_22_cnt");

                entity.Property(e => e._223)
                    .HasColumnName("2_23")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._223Cnt).HasColumnName("2_23_cnt");

                entity.Property(e => e._224)
                    .HasColumnName("2_24")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._224Cnt).HasColumnName("2_24_cnt");

                entity.Property(e => e._225)
                    .HasColumnName("2_25")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._225Cnt).HasColumnName("2_25_cnt");

                entity.Property(e => e._226)
                    .HasColumnName("2_26")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._226Cnt).HasColumnName("2_26_cnt");

                entity.Property(e => e._22Cnt).HasColumnName("2_2_cnt");

                entity.Property(e => e._23)
                    .HasColumnName("2_3")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._23Cnt).HasColumnName("2_3_cnt");

                entity.Property(e => e._24)
                    .HasColumnName("2_4")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._24Cnt).HasColumnName("2_4_cnt");

                entity.Property(e => e._25)
                    .HasColumnName("2_5")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._25Cnt).HasColumnName("2_5_cnt");

                entity.Property(e => e._26)
                    .HasColumnName("2_6")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._26Cnt).HasColumnName("2_6_cnt");

                entity.Property(e => e._27)
                    .HasColumnName("2_7")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._27Cnt).HasColumnName("2_7_cnt");

                entity.Property(e => e._28)
                    .HasColumnName("2_8")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._28Cnt).HasColumnName("2_8_cnt");

                entity.Property(e => e._29)
                    .HasColumnName("2_9")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._31).HasColumnName("3_1");

                entity.Property(e => e._312).HasColumnName("3_12");

                entity.Property(e => e._313).HasColumnName("3_13");

                entity.Property(e => e._314).HasColumnName("3_14");

                entity.Property(e => e._315).HasColumnName("3_15");

                entity.Property(e => e._316).HasColumnName("3_16");

                entity.Property(e => e._317).HasColumnName("3_17");

                entity.Property(e => e._318).HasColumnName("3_18");

                entity.Property(e => e._319).HasColumnName("3_19");

                entity.Property(e => e._32).HasColumnName("3_2");

                entity.Property(e => e._320).HasColumnName("3_20");

                entity.Property(e => e._326).HasColumnName("3_26");

                entity.Property(e => e._327).HasColumnName("3_27");

                entity.Property(e => e._328).HasColumnName("3_28");

                entity.Property(e => e._329)
                    .HasColumnName("3_29")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._329Cnt).HasColumnName("3_29_cnt");

                entity.Property(e => e._33).HasColumnName("3_3");

                entity.Property(e => e._330)
                    .HasColumnName("3_30")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._330Cnt).HasColumnName("3_30_cnt");

                entity.Property(e => e._331)
                    .HasColumnName("3_31")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._331Cnt).HasColumnName("3_31_cnt");

                entity.Property(e => e._332)
                    .HasColumnName("3_32")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._332Cnt).HasColumnName("3_32_cnt");

                entity.Property(e => e._333)
                    .HasColumnName("3_33")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._333Cnt).HasColumnName("3_33_cnt");

                entity.Property(e => e._334)
                    .HasColumnName("3_34")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._334Cnt).HasColumnName("3_34_cnt");

                entity.Property(e => e._335)
                    .HasColumnName("3_35")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._335Cnt).HasColumnName("3_35_cnt");

                entity.Property(e => e._336)
                    .HasColumnName("3_36")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._336Cnt).HasColumnName("3_36_cnt");

                entity.Property(e => e._337)
                    .HasColumnName("3_37")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._337Cnt).HasColumnName("3_37_cnt");

                entity.Property(e => e._338)
                    .HasColumnName("3_38")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._338Cnt).HasColumnName("3_38_cnt");

                entity.Property(e => e._339)
                    .HasColumnName("3_39")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._339Cnt).HasColumnName("3_39_cnt");

                entity.Property(e => e._34).HasColumnName("3_4");

                entity.Property(e => e._340)
                    .HasColumnName("3_40")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._340Cnt).HasColumnName("3_40_cnt");

                entity.Property(e => e._341)
                    .HasColumnName("3_41")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._341Cnt).HasColumnName("3_41_cnt");

                entity.Property(e => e._342)
                    .HasColumnName("3_42")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._342Cnt).HasColumnName("3_42_cnt");

                entity.Property(e => e._35).HasColumnName("3_5");

                entity.Property(e => e._36).HasColumnName("3_6");
            });

            modelBuilder.Entity<GzguTempResult1>(entity =>
            {
                entity.HasKey(e => new { e.InstitutionId, e.CompetitiveGroupId, e.DirectionId, e.EducationFormId, e.EducationSourceId, e.IdLevelBudget })
                    .HasName("PK_gzgu_temp_result");

                entity.ToTable("gzgu_temp_result1");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.DirectionId).HasColumnName("DirectionID");

                entity.Property(e => e._110).HasColumnName("1_10");

                entity.Property(e => e._111).HasColumnName("1_11");

                entity.Property(e => e._112).HasColumnName("1_12");

                entity.Property(e => e._113).HasColumnName("1_13");

                entity.Property(e => e._114).HasColumnName("1_14");

                entity.Property(e => e._115).HasColumnName("1_15");

                entity.Property(e => e._116).HasColumnName("1_16");

                entity.Property(e => e._117).HasColumnName("1_17");

                entity.Property(e => e._118).HasColumnName("1_18");

                entity.Property(e => e._119).HasColumnName("1_19");

                entity.Property(e => e._120).HasColumnName("1_20");

                entity.Property(e => e._121).HasColumnName("1_21");

                entity.Property(e => e._122).HasColumnName("1_22");

                entity.Property(e => e._123).HasColumnName("1_23");

                entity.Property(e => e._124).HasColumnName("1_24");

                entity.Property(e => e._125).HasColumnName("1_25");

                entity.Property(e => e._126).HasColumnName("1_26");

                entity.Property(e => e._127).HasColumnName("1_27");

                entity.Property(e => e._128).HasColumnName("1_28");

                entity.Property(e => e._129).HasColumnName("1_29");

                entity.Property(e => e._130).HasColumnName("1_30");

                entity.Property(e => e._131).HasColumnName("1_31");

                entity.Property(e => e._132).HasColumnName("1_32");

                entity.Property(e => e._133).HasColumnName("1_33");

                entity.Property(e => e._134).HasColumnName("1_34");

                entity.Property(e => e._135).HasColumnName("1_35");

                entity.Property(e => e._136).HasColumnName("1_36");

                entity.Property(e => e._137).HasColumnName("1_37");

                entity.Property(e => e._138).HasColumnName("1_38");

                entity.Property(e => e._139).HasColumnName("1_39");

                entity.Property(e => e._14).HasColumnName("1_4");

                entity.Property(e => e._140).HasColumnName("1_40");

                entity.Property(e => e._141).HasColumnName("1_41");

                entity.Property(e => e._142).HasColumnName("1_42");

                entity.Property(e => e._143).HasColumnName("1_43");

                entity.Property(e => e._144).HasColumnName("1_44");

                entity.Property(e => e._145).HasColumnName("1_45");

                entity.Property(e => e._146).HasColumnName("1_46");

                entity.Property(e => e._147).HasColumnName("1_47");

                entity.Property(e => e._148).HasColumnName("1_48");

                entity.Property(e => e._14Cnt).HasColumnName("1_4_cnt");

                entity.Property(e => e._15).HasColumnName("1_5");

                entity.Property(e => e._16).HasColumnName("1_6");

                entity.Property(e => e._17).HasColumnName("1_7");

                entity.Property(e => e._18).HasColumnName("1_8");

                entity.Property(e => e._19).HasColumnName("1_9");

                entity.Property(e => e._21)
                    .HasColumnName("2_1")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._210)
                    .HasColumnName("2_10")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._211)
                    .HasColumnName("2_11")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._212)
                    .HasColumnName("2_12")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._213)
                    .HasColumnName("2_13")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._214)
                    .HasColumnName("2_14")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._215).HasColumnName("2_15");

                entity.Property(e => e._216).HasColumnName("2_16");

                entity.Property(e => e._217).HasColumnName("2_17");

                entity.Property(e => e._218).HasColumnName("2_18");

                entity.Property(e => e._219).HasColumnName("2_19");

                entity.Property(e => e._21Cnt).HasColumnName("2_1_cnt");

                entity.Property(e => e._22)
                    .HasColumnName("2_2")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._220).HasColumnName("2_20");

                entity.Property(e => e._221)
                    .HasColumnName("2_21")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._221Cnt).HasColumnName("2_21_cnt");

                entity.Property(e => e._222)
                    .HasColumnName("2_22")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._222Cnt).HasColumnName("2_22_cnt");

                entity.Property(e => e._223)
                    .HasColumnName("2_23")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._223Cnt).HasColumnName("2_23_cnt");

                entity.Property(e => e._224)
                    .HasColumnName("2_24")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._224Cnt).HasColumnName("2_24_cnt");

                entity.Property(e => e._225)
                    .HasColumnName("2_25")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._225Cnt).HasColumnName("2_25_cnt");

                entity.Property(e => e._226)
                    .HasColumnName("2_26")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._226Cnt).HasColumnName("2_26_cnt");

                entity.Property(e => e._22Cnt).HasColumnName("2_2_cnt");

                entity.Property(e => e._23)
                    .HasColumnName("2_3")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._23Cnt).HasColumnName("2_3_cnt");

                entity.Property(e => e._24)
                    .HasColumnName("2_4")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._24Cnt).HasColumnName("2_4_cnt");

                entity.Property(e => e._25)
                    .HasColumnName("2_5")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._25Cnt).HasColumnName("2_5_cnt");

                entity.Property(e => e._26)
                    .HasColumnName("2_6")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._26Cnt).HasColumnName("2_6_cnt");

                entity.Property(e => e._27)
                    .HasColumnName("2_7")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._27Cnt).HasColumnName("2_7_cnt");

                entity.Property(e => e._28)
                    .HasColumnName("2_8")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._28Cnt).HasColumnName("2_8_cnt");

                entity.Property(e => e._29)
                    .HasColumnName("2_9")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._31).HasColumnName("3_1");

                entity.Property(e => e._312).HasColumnName("3_12");

                entity.Property(e => e._313).HasColumnName("3_13");

                entity.Property(e => e._314).HasColumnName("3_14");

                entity.Property(e => e._315).HasColumnName("3_15");

                entity.Property(e => e._316).HasColumnName("3_16");

                entity.Property(e => e._317).HasColumnName("3_17");

                entity.Property(e => e._318).HasColumnName("3_18");

                entity.Property(e => e._319).HasColumnName("3_19");

                entity.Property(e => e._32).HasColumnName("3_2");

                entity.Property(e => e._320).HasColumnName("3_20");

                entity.Property(e => e._326).HasColumnName("3_26");

                entity.Property(e => e._327).HasColumnName("3_27");

                entity.Property(e => e._328).HasColumnName("3_28");

                entity.Property(e => e._329)
                    .HasColumnName("3_29")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._329Cnt).HasColumnName("3_29_cnt");

                entity.Property(e => e._33).HasColumnName("3_3");

                entity.Property(e => e._330)
                    .HasColumnName("3_30")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._330Cnt).HasColumnName("3_30_cnt");

                entity.Property(e => e._331)
                    .HasColumnName("3_31")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._331Cnt).HasColumnName("3_31_cnt");

                entity.Property(e => e._332)
                    .HasColumnName("3_32")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._332Cnt).HasColumnName("3_32_cnt");

                entity.Property(e => e._333)
                    .HasColumnName("3_33")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._333Cnt).HasColumnName("3_33_cnt");

                entity.Property(e => e._334)
                    .HasColumnName("3_34")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._334Cnt).HasColumnName("3_34_cnt");

                entity.Property(e => e._335)
                    .HasColumnName("3_35")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._335Cnt).HasColumnName("3_35_cnt");

                entity.Property(e => e._336)
                    .HasColumnName("3_36")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._336Cnt).HasColumnName("3_36_cnt");

                entity.Property(e => e._337)
                    .HasColumnName("3_37")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._337Cnt).HasColumnName("3_37_cnt");

                entity.Property(e => e._338)
                    .HasColumnName("3_38")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._338Cnt).HasColumnName("3_38_cnt");

                entity.Property(e => e._339)
                    .HasColumnName("3_39")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._339Cnt).HasColumnName("3_39_cnt");

                entity.Property(e => e._34).HasColumnName("3_4");

                entity.Property(e => e._340)
                    .HasColumnName("3_40")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._340Cnt).HasColumnName("3_40_cnt");

                entity.Property(e => e._341)
                    .HasColumnName("3_41")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._341Cnt).HasColumnName("3_41_cnt");

                entity.Property(e => e._342)
                    .HasColumnName("3_42")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e._342Cnt).HasColumnName("3_42_cnt");

                entity.Property(e => e._35).HasColumnName("3_5");

                entity.Property(e => e._36).HasColumnName("3_6");
            });

            modelBuilder.Entity<GzgubackFull2017>(entity =>
            {
                entity.ToTable("GZGUBackFull_2017");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DirCode)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Inn)
                    .HasColumnName("INN")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Kpp)
                    .HasColumnName("KPP")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._11)
                    .HasColumnName("1_1")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._110)
                    .HasColumnName("1_10")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._111)
                    .HasColumnName("1_11")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._112)
                    .HasColumnName("1_12")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._113)
                    .HasColumnName("1_13")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._114)
                    .HasColumnName("1_14")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._115)
                    .HasColumnName("1_15")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._116)
                    .HasColumnName("1_16")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._117)
                    .HasColumnName("1_17")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._118)
                    .HasColumnName("1_18")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._119)
                    .HasColumnName("1_19")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._12)
                    .HasColumnName("1_2")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._120)
                    .HasColumnName("1_20")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._124)
                    .HasColumnName("1_24")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._125)
                    .HasColumnName("1_25")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._126)
                    .HasColumnName("1_26")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._127)
                    .HasColumnName("1_27")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._128)
                    .HasColumnName("1_28")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._129)
                    .HasColumnName("1_29")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._13)
                    .HasColumnName("1_3")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._130)
                    .HasColumnName("1_30")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._131)
                    .HasColumnName("1_31")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._132)
                    .HasColumnName("1_32")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._133)
                    .HasColumnName("1_33")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._134)
                    .HasColumnName("1_34")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._135)
                    .HasColumnName("1_35")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._136)
                    .HasColumnName("1_36")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._137)
                    .HasColumnName("1_37")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._138)
                    .HasColumnName("1_38")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._139)
                    .HasColumnName("1_39")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._14)
                    .HasColumnName("1_4")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._140)
                    .HasColumnName("1_40")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._141)
                    .HasColumnName("1_41")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._142)
                    .HasColumnName("1_42")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._143)
                    .HasColumnName("1_43")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._144)
                    .HasColumnName("1_44")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._145)
                    .HasColumnName("1_45")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._146)
                    .HasColumnName("1_46")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._147)
                    .HasColumnName("1_47")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._148)
                    .HasColumnName("1_48")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._15)
                    .HasColumnName("1_5")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._16)
                    .HasColumnName("1_6")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._17)
                    .HasColumnName("1_7")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._18)
                    .HasColumnName("1_8")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._19)
                    .HasColumnName("1_9")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._21)
                    .HasColumnName("2_1")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._210)
                    .HasColumnName("2_10")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._211)
                    .HasColumnName("2_11")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._212)
                    .HasColumnName("2_12")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._213)
                    .HasColumnName("2_13")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._214)
                    .HasColumnName("2_14")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._215)
                    .HasColumnName("2_15")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._216)
                    .HasColumnName("2_16")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._217)
                    .HasColumnName("2_17")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._218)
                    .HasColumnName("2_18")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._219)
                    .HasColumnName("2_19")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._22)
                    .HasColumnName("2_2")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._220)
                    .HasColumnName("2_20")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._221)
                    .HasColumnName("2_21")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._222)
                    .HasColumnName("2_22")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._223)
                    .HasColumnName("2_23")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._224)
                    .HasColumnName("2_24")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._225)
                    .HasColumnName("2_25")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._226)
                    .HasColumnName("2_26")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._23)
                    .HasColumnName("2_3")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._24)
                    .HasColumnName("2_4")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._25)
                    .HasColumnName("2_5")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._26)
                    .HasColumnName("2_6")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._27)
                    .HasColumnName("2_7")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._28)
                    .HasColumnName("2_8")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._29)
                    .HasColumnName("2_9")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._31)
                    .HasColumnName("3_1")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._312)
                    .HasColumnName("3_12")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._313)
                    .HasColumnName("3_13")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._314)
                    .HasColumnName("3_14")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._315)
                    .HasColumnName("3_15")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._316)
                    .HasColumnName("3_16")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._317)
                    .HasColumnName("3_17")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._318)
                    .HasColumnName("3_18")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._319)
                    .HasColumnName("3_19")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._32)
                    .HasColumnName("3_2")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._320)
                    .HasColumnName("3_20")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._326)
                    .HasColumnName("3_26")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._327)
                    .HasColumnName("3_27")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._328)
                    .HasColumnName("3_28")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._329)
                    .HasColumnName("3_29")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._33)
                    .HasColumnName("3_3")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._330)
                    .HasColumnName("3_30")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._331)
                    .HasColumnName("3_31")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._332)
                    .HasColumnName("3_32")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._333)
                    .HasColumnName("3_33")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._334)
                    .HasColumnName("3_34")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._335)
                    .HasColumnName("3_35")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._336)
                    .HasColumnName("3_36")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._337)
                    .HasColumnName("3_37")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._338)
                    .HasColumnName("3_38")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._339)
                    .HasColumnName("3_39")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._34)
                    .HasColumnName("3_4")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._340)
                    .HasColumnName("3_40")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._341)
                    .HasColumnName("3_41")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._342)
                    .HasColumnName("3_42")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._35)
                    .HasColumnName("3_5")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._36)
                    .HasColumnName("3_6")
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GzgubackFull20172016>(entity =>
            {
                entity.ToTable("GZGUBackFull_2017_2016");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DirCode)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Inn)
                    .HasColumnName("INN")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Kpp)
                    .HasColumnName("KPP")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._11)
                    .HasColumnName("1_1")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._110)
                    .HasColumnName("1_10")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._111)
                    .HasColumnName("1_11")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._112)
                    .HasColumnName("1_12")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._113)
                    .HasColumnName("1_13")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._114)
                    .HasColumnName("1_14")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._115)
                    .HasColumnName("1_15")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._116)
                    .HasColumnName("1_16")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._117)
                    .HasColumnName("1_17")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._118)
                    .HasColumnName("1_18")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._119)
                    .HasColumnName("1_19")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._12)
                    .HasColumnName("1_2")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._120)
                    .HasColumnName("1_20")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._124)
                    .HasColumnName("1_24")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._125)
                    .HasColumnName("1_25")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._126)
                    .HasColumnName("1_26")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._127)
                    .HasColumnName("1_27")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._128)
                    .HasColumnName("1_28")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._129)
                    .HasColumnName("1_29")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._13)
                    .HasColumnName("1_3")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._130)
                    .HasColumnName("1_30")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._131)
                    .HasColumnName("1_31")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._132)
                    .HasColumnName("1_32")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._133)
                    .HasColumnName("1_33")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._134)
                    .HasColumnName("1_34")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._135)
                    .HasColumnName("1_35")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._136)
                    .HasColumnName("1_36")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._137)
                    .HasColumnName("1_37")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._138)
                    .HasColumnName("1_38")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._139)
                    .HasColumnName("1_39")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._14)
                    .HasColumnName("1_4")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._140)
                    .HasColumnName("1_40")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._141)
                    .HasColumnName("1_41")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._142)
                    .HasColumnName("1_42")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._143)
                    .HasColumnName("1_43")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._144)
                    .HasColumnName("1_44")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._145)
                    .HasColumnName("1_45")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._146)
                    .HasColumnName("1_46")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._147)
                    .HasColumnName("1_47")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._148)
                    .HasColumnName("1_48")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._15)
                    .HasColumnName("1_5")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._16)
                    .HasColumnName("1_6")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._17)
                    .HasColumnName("1_7")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._18)
                    .HasColumnName("1_8")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._19)
                    .HasColumnName("1_9")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._21)
                    .HasColumnName("2_1")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._210)
                    .HasColumnName("2_10")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._211)
                    .HasColumnName("2_11")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._212)
                    .HasColumnName("2_12")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._213)
                    .HasColumnName("2_13")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._214)
                    .HasColumnName("2_14")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._215)
                    .HasColumnName("2_15")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._216)
                    .HasColumnName("2_16")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._217)
                    .HasColumnName("2_17")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._218)
                    .HasColumnName("2_18")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._219)
                    .HasColumnName("2_19")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._22)
                    .HasColumnName("2_2")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._220)
                    .HasColumnName("2_20")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._221)
                    .HasColumnName("2_21")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._222)
                    .HasColumnName("2_22")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._223)
                    .HasColumnName("2_23")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._224)
                    .HasColumnName("2_24")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._225)
                    .HasColumnName("2_25")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._226)
                    .HasColumnName("2_26")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._23)
                    .HasColumnName("2_3")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._24)
                    .HasColumnName("2_4")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._25)
                    .HasColumnName("2_5")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._26)
                    .HasColumnName("2_6")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._27)
                    .HasColumnName("2_7")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._28)
                    .HasColumnName("2_8")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._29)
                    .HasColumnName("2_9")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._31)
                    .HasColumnName("3_1")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._312)
                    .HasColumnName("3_12")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._313)
                    .HasColumnName("3_13")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._314)
                    .HasColumnName("3_14")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._315)
                    .HasColumnName("3_15")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._316)
                    .HasColumnName("3_16")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._317)
                    .HasColumnName("3_17")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._318)
                    .HasColumnName("3_18")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._319)
                    .HasColumnName("3_19")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._32)
                    .HasColumnName("3_2")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._320)
                    .HasColumnName("3_20")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._326)
                    .HasColumnName("3_26")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._327)
                    .HasColumnName("3_27")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._329)
                    .HasColumnName("3_29")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._33)
                    .HasColumnName("3_3")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._330)
                    .HasColumnName("3_30")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._331)
                    .HasColumnName("3_31")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._332)
                    .HasColumnName("3_32")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._333)
                    .HasColumnName("3_33")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._334)
                    .HasColumnName("3_34")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._336)
                    .HasColumnName("3_36")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._337)
                    .HasColumnName("3_37")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._338)
                    .HasColumnName("3_38")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._339)
                    .HasColumnName("3_39")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._34)
                    .HasColumnName("3_4")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._340)
                    .HasColumnName("3_40")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._341)
                    .HasColumnName("3_41")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._35)
                    .HasColumnName("3_5")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._36)
                    .HasColumnName("3_6")
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GzgubakFull>(entity =>
            {
                entity.ToTable("GZGUBakFull");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DirCode)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Inn)
                    .HasColumnName("INN")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.InsName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Kpp)
                    .HasColumnName("KPP")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._11)
                    .HasColumnName("1_1")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._110)
                    .HasColumnName("1_10")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._111)
                    .HasColumnName("1_11")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._112)
                    .HasColumnName("1_12")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._113)
                    .HasColumnName("1_13")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._114)
                    .HasColumnName("1_14")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._115)
                    .HasColumnName("1_15")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._116)
                    .HasColumnName("1_16")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._117)
                    .HasColumnName("1_17")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._118)
                    .HasColumnName("1_18")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._119)
                    .HasColumnName("1_19")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._12)
                    .HasColumnName("1_2")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._120)
                    .HasColumnName("1_20")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._121)
                    .HasColumnName("1_21")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._122)
                    .HasColumnName("1_22")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._123)
                    .HasColumnName("1_23")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._124)
                    .HasColumnName("1_24")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._125)
                    .HasColumnName("1_25")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._126)
                    .HasColumnName("1_26")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._127)
                    .HasColumnName("1_27")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._128)
                    .HasColumnName("1_28")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._129)
                    .HasColumnName("1_29")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._12Id)
                    .HasColumnName("1_2_ID")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._13)
                    .HasColumnName("1_3")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._130)
                    .HasColumnName("1_30")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._131)
                    .HasColumnName("1_31")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._132)
                    .HasColumnName("1_32")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._133)
                    .HasColumnName("1_33")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._134)
                    .HasColumnName("1_34")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._135)
                    .HasColumnName("1_35")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._136)
                    .HasColumnName("1_36")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._137)
                    .HasColumnName("1_37")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._138)
                    .HasColumnName("1_38")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._139)
                    .HasColumnName("1_39")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._13Id)
                    .HasColumnName("1_3_ID")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._140)
                    .HasColumnName("1_40")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._141)
                    .HasColumnName("1_41")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._142)
                    .HasColumnName("1_42")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._143)
                    .HasColumnName("1_43")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._144)
                    .HasColumnName("1_44")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._145)
                    .HasColumnName("1_45")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._146)
                    .HasColumnName("1_46")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._147)
                    .HasColumnName("1_47")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._15)
                    .HasColumnName("1_5")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._16)
                    .HasColumnName("1_6")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._17)
                    .HasColumnName("1_7")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._18)
                    .HasColumnName("1_8")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._19)
                    .HasColumnName("1_9")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._248)
                    .HasColumnName("2_48")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._249)
                    .HasColumnName("2_49")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._250)
                    .HasColumnName("2_50")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._251)
                    .HasColumnName("2_51")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._252)
                    .HasColumnName("2_52")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._253)
                    .HasColumnName("2_53")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e._254)
                    .HasColumnName("2_54")
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<HandwrittingRegionRus>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("handwritting_region_rus");

                entity.Property(e => e.PHash)
                    .HasColumnName("p_hash")
                    .HasMaxLength(50);

                entity.Property(e => e.PName)
                    .HasColumnName("p_name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<History>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Hash).HasMaxLength(1000);

                entity.Property(e => e.Participant).HasMaxLength(1000);
            });

            modelBuilder.Entity<HseEge2017>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("HSE_EGE_2017");

                entity.Property(e => e.FisName).HasColumnName("FIS_Name");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.InstId).HasColumnName("instId");

                entity.Property(e => e.Region).HasMaxLength(255);
            });

            modelBuilder.Entity<HseEge2018>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("HSE_EGE_2018");

                entity.Property(e => e.FisName).HasColumnName("FIS_Name");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.InstId).HasColumnName("instId");

                entity.Property(e => e.Region).HasMaxLength(255);
            });

            modelBuilder.Entity<HseEge2018P>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("HSE_EGE_2018_P");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Instid).HasColumnName("instid");

                entity.Property(e => e.Name).HasColumnName("name");
            });

            modelBuilder.Entity<IdentityDocumentType>(entity =>
            {
                entity.Property(e => e.IdentityDocumentTypeId)
                    .HasColumnName("IdentityDocumentTypeID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ImportPackage>(entity =>
            {
                entity.HasKey(e => e.PackageId)
                    .IsClustered(false);

                entity.HasIndex(e => e.CreateDate);

                entity.HasIndex(e => new { e.CreateDate, e.InstitutionId, e.CheckStatusId })
                    .HasName("IDX_CheckPerf");

                entity.HasIndex(e => new { e.InstitutionId, e.CreateDate, e.StatusId })
                    .HasName("I_ImportPackage_StatusID");

                entity.HasIndex(e => new { e.CreateDate, e.StatusId, e.TypeId, e.CheckStatusId })
                    .HasName("IX_ImportPackage_StatusID_TypeID_CheckStatusID");

                entity.HasIndex(e => new { e.PackageId, e.Comment, e.InstitutionId, e.CompleteDate })
                    .HasName("IX_Reset_ImportPackage");

                entity.Property(e => e.PackageId).HasColumnName("PackageID");

                entity.Property(e => e.CheckCompleteDate).HasColumnType("datetime");

                entity.Property(e => e.CheckInProgressDate).HasColumnType("datetime");

                entity.Property(e => e.CheckResultInfo).HasColumnType("xml");

                entity.Property(e => e.CheckStatusId).HasColumnName("CheckStatusID");

                entity.Property(e => e.Comment).HasColumnType("varchar(max)");

                entity.Property(e => e.CompleteDate).HasColumnType("datetime");

                entity.Property(e => e.Content)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ImportedAppIds)
                    .HasColumnName("ImportedAppIDs")
                    .HasColumnType("xml");

                entity.Property(e => e.InProgressDate).HasColumnType("datetime");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.LastDateChanged)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PackageData)
                    .IsRequired()
                    .HasColumnType("xml");

                entity.Property(e => e.ProcessResultInfo).HasColumnType("xml");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.TypeId).HasColumnName("TypeID");

                entity.Property(e => e.UserLogin)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.Institution)
                    .WithMany(p => p.ImportPackage)
                    .HasForeignKey(d => d.InstitutionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ImportPackage_Institution");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.ImportPackage)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ImportPackage_ImportPackageStatus");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.ImportPackage)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ImportPackage_ImportPackageType");
            });

            modelBuilder.Entity<ImportPackage2017>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ImportPackage_2017");

                entity.Property(e => e.CheckCompleteDate).HasColumnType("datetime");

                entity.Property(e => e.CheckInProgressDate).HasColumnType("datetime");

                entity.Property(e => e.CheckResultInfo).HasColumnType("xml");

                entity.Property(e => e.CheckStatusId).HasColumnName("CheckStatusID");

                entity.Property(e => e.Comment).IsUnicode(false);

                entity.Property(e => e.CompleteDate).HasColumnType("datetime");

                entity.Property(e => e.Content)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.ImportedAppIds)
                    .HasColumnName("ImportedAppIDs")
                    .HasColumnType("xml");

                entity.Property(e => e.InProgressDate).HasColumnType("datetime");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.LastDateChanged).HasColumnType("datetime");

                entity.Property(e => e.PackageData)
                    .IsRequired()
                    .HasColumnType("xml");

                entity.Property(e => e.PackageId).HasColumnName("PackageID");

                entity.Property(e => e.ProcessResultInfo).HasColumnType("xml");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.TypeId).HasColumnName("TypeID");

                entity.Property(e => e.UserLogin)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ImportPackageForBrokenApps>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("importPackage_ForBrokenApps");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Institutionid).HasColumnName("institutionid");

                entity.Property(e => e.Packageid).HasColumnName("packageid");
            });

            modelBuilder.Entity<ImportPackageParsed>(entity =>
            {
                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.ApplicationNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Comment).IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DirectionId).HasColumnName("DirectionID");

                entity.Property(e => e.EducationFormId).HasColumnName("EducationFormID");

                entity.Property(e => e.EducationLevelId).HasColumnName("EducationLevelID");

                entity.Property(e => e.FinanceSourceId).HasColumnName("FinanceSourceID");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.PackageCreatedDate).HasColumnType("datetime");

                entity.Property(e => e.PackageId).HasColumnName("PackageID");

                entity.Property(e => e.PackageModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RegistrationDate).HasColumnType("datetime");

                entity.Property(e => e.TrashTypeId).HasColumnName("TrashTypeID");
            });

            modelBuilder.Entity<ImportPackageParsedBack>(entity =>
            {
                entity.ToTable("ImportPackageParsed_back");

                entity.Property(e => e.ApplicationNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Comment).IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DirectionId).HasColumnName("DirectionID");

                entity.Property(e => e.EducationFormId).HasColumnName("EducationFormID");

                entity.Property(e => e.EducationLevelId).HasColumnName("EducationLevelID");

                entity.Property(e => e.FinanceSourceId).HasColumnName("FinanceSourceID");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.PackageCreatedDate).HasColumnType("datetime");

                entity.Property(e => e.PackageId).HasColumnName("PackageID");

                entity.Property(e => e.PackageModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.RegistrationDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<ImportPackageStatus>(entity =>
            {
                entity.HasKey(e => e.StatusId);

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ImportPackageType>(entity =>
            {
                entity.HasKey(e => e.TypeId);

                entity.Property(e => e.TypeId).HasColumnName("TypeID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(512)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<IndividualAchievementsCategory>(entity =>
            {
                entity.HasKey(e => e.IdCategory)
                    .HasName("PK__Individu__CBD747064984CAEC");

                entity.HasComment("Индивидуальные достижения, учитываемые образовательной организацией");

                entity.Property(e => e.IdCategory).HasComment("ИД категории инд. достижения");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasComment("Наименование категории инд. достижения");
            });

            modelBuilder.Entity<IndividualAchivement>(entity =>
            {
                entity.HasKey(e => e.Iaid);

                entity.HasIndex(e => e.ApplicationId)
                    .HasName("I_IndividualAchivement_ApplicationID");

                entity.HasIndex(e => e.EntrantDocumentId)
                    .HasName("I_IndividualAchivement_EntrantDocumentID");

                entity.Property(e => e.Iaid).HasColumnName("IAID");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EntrantDocumentId).HasColumnName("EntrantDocumentID");

                entity.Property(e => e.Iamark)
                    .HasColumnName("IAMark")
                    .HasColumnType("decimal(7, 4)");

                entity.Property(e => e.Ianame)
                    .HasColumnName("IAName")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Iauid)
                    .HasColumnName("IAUID")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IsAdvantageRight).HasColumnName("isAdvantageRight");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.IndividualAchivement)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IndividualAchivement_Application1");

                entity.HasOne(d => d.EntrantDocument)
                    .WithMany(p => p.IndividualAchivement)
                    .HasForeignKey(d => d.EntrantDocumentId)
                    .HasConstraintName("FK_IndividualAchivement_EntrantDocument1");

                entity.HasOne(d => d.IdAchievementNavigation)
                    .WithMany(p => p.IndividualAchivement)
                    .HasForeignKey(d => d.IdAchievement)
                    .HasConstraintName("FK_IndividualAchivement_InstitutionAchievements");
            });

            modelBuilder.Entity<Institution>(entity =>
            {
                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.Address)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.AdmissionStructurePublishDate).HasColumnType("datetime");

                entity.Property(e => e.BriefName)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.EiisId)
                    .HasColumnName("EIIS_ID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EsrpOrgId).HasColumnName("EsrpOrgID");

                entity.Property(e => e.Fax)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.FormOfLawId).HasColumnName("FormOfLawID");

                entity.Property(e => e.FullName)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.HostelAttachmentId).HasColumnName("HostelAttachmentID");

                entity.Property(e => e.Inn)
                    .HasColumnName("INN")
                    .HasMaxLength(14)
                    .IsUnicode(false);

                entity.Property(e => e.InstitutionTypeId)
                    .HasColumnName("InstitutionTypeID")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Kpp)
                    .HasColumnName("KPP")
                    .HasMaxLength(13)
                    .IsUnicode(false);

                entity.Property(e => e.LawAddress)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Ogrn)
                    .HasColumnName("OGRN")
                    .HasMaxLength(18)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.OwnerDepartment)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.ReceivingApplicationDate).HasColumnType("datetime");

                entity.Property(e => e.RegionId).HasColumnName("RegionID");

                entity.Property(e => e.Site)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.FormOfLaw)
                    .WithMany(p => p.Institution)
                    .HasForeignKey(d => d.FormOfLawId)
                    .HasConstraintName("FK_Institution_FormOfLaw");

                entity.HasOne(d => d.HostelAttachment)
                    .WithMany(p => p.Institution)
                    .HasForeignKey(d => d.HostelAttachmentId)
                    .HasConstraintName("FK_Institution_Attachment");

                entity.HasOne(d => d.InstitutionType)
                    .WithMany(p => p.Institution)
                    .HasForeignKey(d => d.InstitutionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Institution_InstitutionType");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.Institution)
                    .HasForeignKey(d => d.RegionId)
                    .HasConstraintName("FK_Institution_RegionType");
            });

            modelBuilder.Entity<InstitutionAccreditation>(entity =>
            {
                entity.HasKey(e => e.AccreditationId);

                entity.Property(e => e.AccreditationId).HasColumnName("AccreditationID");

                entity.Property(e => e.Accreditation)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.AttachmentId).HasColumnName("AttachmentID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                // entity.HasOne(d => d.InstitutionAttachment)
                //     .WithOne(p => p.InstitutionLicense)
                //     .HasForeignKey<InstitutionAttachment>(e => e.AttachmentId)
                //     .HasConstraintName("FK_InstitutionLicense_InstitutionAttachment");

                entity.HasOne(d => d.InstitutionAttachment)
                    .WithOne(p => p.InstitutionAccreditation)
                    .HasForeignKey<InstitutionAttachment>(d => d.AttachmentId)
                    .HasConstraintName("FK_InstitutionAccreditation_InstitutionAttachment");

                entity.HasOne(d => d.Institution)
                    .WithMany(p => p.InstitutionAccreditation)
                    .HasForeignKey(d => d.InstitutionId)
                    .HasConstraintName("FK_InstitutionAccreditation_Institution");
            });

            modelBuilder.Entity<InstitutionAchievements>(entity =>
            {
                entity.HasKey(e => e.IdAchievement)
                    .HasName("PK__Institut__B09E6C033A42875C");

                entity.HasComment("Таблица “Индивидуальные достижения, учитываемые образовательной организацией");

                entity.HasIndex(e => e.CampaignId)
                    .HasName("I_InstitutionAchievements_CampaignID");

                entity.HasIndex(e => e.CreatedDate)
                    .HasName("I_InstitutionAchievements_CreatedDate");

                entity.HasIndex(e => new { e.IdAchievement, e.IdCategory })
                    .HasName("<Name of Missing Index, sysname,>");

                entity.Property(e => e.IdAchievement).HasComment("ИД достижения, учитываемого ОО");

                entity.Property(e => e.CampaignId)
                    .HasColumnName("CampaignID")
                    .HasComment("Ссылка на ПК ОО FK (Campaign – Campaign¬ID) ");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IdCategory).HasComment("Ссылка на IndividualAchievementsCategory FK (IndividualAchievementsCategory – IdCategory) ");

                entity.Property(e => e.MaxValue)
                    .HasColumnType("decimal(7, 4)")
                    .HasComment("Макс балл за достижение");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(4000)
                    .IsUnicode(false)
                    .HasComment("Наименование инд. достижения");

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasComment("Идентификатор достижения, указанный ОО");

                entity.HasOne(d => d.Campaign)
                    .WithMany(p => p.InstitutionAchievements)
                    .HasForeignKey(d => d.CampaignId)
                    .HasConstraintName("FK_InstitutionAchievements_Campaign");

                entity.HasOne(d => d.IdCategoryNavigation)
                    .WithMany(p => p.InstitutionAchievements)
                    .HasForeignKey(d => d.IdCategory)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InstitutionAchievements_IndividualAchievementsCategory");
            });

            modelBuilder.Entity<InstitutionDirectionRequest>(entity =>
            {
                entity.HasKey(e => e.RequestId);

                entity.Property(e => e.DenialComment)
                    .HasMaxLength(2048)
                    .IsUnicode(false);

                entity.Property(e => e.DenialDate).HasColumnType("datetime");

                entity.Property(e => e.RequestComment)
                    .HasMaxLength(2048)
                    .IsUnicode(false);

                entity.Property(e => e.RequestDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Direction)
                    .WithMany(p => p.InstitutionDirectionRequest)
                    .HasForeignKey(d => d.DirectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InstitutionDirectionRequest_Direction");

                entity.HasOne(d => d.Institution)
                    .WithMany(p => p.InstitutionDirectionRequest)
                    .HasForeignKey(d => d.InstitutionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InstitutionDirectionRequest_Institution");
            });

            modelBuilder.Entity<InstitutionDocumentType>(entity =>
            {
                entity.Property(e => e.InstitutionDocumentTypeId).HasColumnName("InstitutionDocumentTypeID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<InstitutionDocuments>(entity =>
            {
                entity.HasKey(e => new { e.InstitutionId, e.AttachmentId });

                entity.HasOne(d => d.Attachment)
                    .WithMany(p => p.InstitutionDocuments)
                    .HasForeignKey(d => d.AttachmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InstitutionDocuments_Attachment");

                entity.HasOne(d => d.Institution)
                    .WithMany(p => p.InstitutionDocuments)
                    .HasForeignKey(d => d.InstitutionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InstitutionDocuments_Institution");
            });

            modelBuilder.Entity<InstitutionFounder>(entity =>
            {
                entity.Property(e => e.EiisId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Emails)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Faxes)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.InitPassword)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Inn)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Kpp)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LawAddress)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.Ogrn)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OrganizationFullName)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.OrganizationShortName)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.Patronymic)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Phones)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PhysicalAddress)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.RegistrationDate).HasColumnType("datetime");

                entity.Property(e => e.UserName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.InstitutionFounderType)
                    .WithMany(p => p.InstitutionFounder)
                    .HasForeignKey(d => d.InstitutionFounderTypeId)
                    .HasConstraintName("FK_InstitutionFounder_InstitutionFounderType");
            });

            modelBuilder.Entity<InstitutionFounderToInstitutions>(entity =>
            {
                entity.Property(e => e.EiisId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.InstitutionFounder)
                    .WithMany(p => p.InstitutionFounderToInstitutions)
                    .HasForeignKey(d => d.InstitutionFounderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InstitutionFounderToInstitutions_InstitutionFounder");

                entity.HasOne(d => d.Institution)
                    .WithMany(p => p.InstitutionFounderToInstitutions)
                    .HasForeignKey(d => d.InstitutionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InstitutionFounderToInstitutions_Institution");
            });

            modelBuilder.Entity<InstitutionFounderType>(entity =>
            {
                entity.Property(e => e.EiisId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<InstitutionHistory>(entity =>
            {
                entity.Property(e => e.InstitutionHistoryId).HasColumnName("InstitutionHistoryID");

                entity.Property(e => e.Accreditation)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.AccreditationAttachmentId).HasColumnName("AccreditationAttachmentID");

                entity.Property(e => e.Address)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.AdmissionStructurePublishDate).HasColumnType("datetime");

                entity.Property(e => e.BriefName)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EsrpOrgId).HasColumnName("EsrpOrgID");

                entity.Property(e => e.Fax)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.FormOfLawId).HasColumnName("FormOfLawID");

                entity.Property(e => e.FullName)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.HostelAttachmentId).HasColumnName("HostelAttachmentID");

                entity.Property(e => e.Inn)
                    .HasColumnName("INN")
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.InstitutionTypeId).HasColumnName("InstitutionTypeID");

                entity.Property(e => e.LicenseAttachmentId).HasColumnName("LicenseAttachmentID");

                entity.Property(e => e.LicenseDate).HasColumnType("datetime");

                entity.Property(e => e.LicenseNumber)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Ogrn)
                    .HasColumnName("OGRN")
                    .HasMaxLength(18)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.OwnerDepartment)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.ReceivingApplicationDate).HasColumnType("datetime");

                entity.Property(e => e.RegionId).HasColumnName("RegionID");

                entity.Property(e => e.Site)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.AccreditationAttachment)
                    .WithMany(p => p.InstitutionHistoryAccreditationAttachment)
                    .HasForeignKey(d => d.AccreditationAttachmentId)
                    .HasConstraintName("FK_InstitutionHistory_AccreditationAttachment");

                entity.HasOne(d => d.FormOfLaw)
                    .WithMany(p => p.InstitutionHistory)
                    .HasForeignKey(d => d.FormOfLawId)
                    .HasConstraintName("FK_InstitutionHistory_FormOfLaw");

                entity.HasOne(d => d.HostelAttachment)
                    .WithMany(p => p.InstitutionHistoryHostelAttachment)
                    .HasForeignKey(d => d.HostelAttachmentId)
                    .HasConstraintName("FK_InstitutionHistory_HostelAttachment");

                entity.HasOne(d => d.InstitutionType)
                    .WithMany(p => p.InstitutionHistory)
                    .HasForeignKey(d => d.InstitutionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InstitutionHistory_InstitutionType");

                entity.HasOne(d => d.LicenseAttachment)
                    .WithMany(p => p.InstitutionHistoryLicenseAttachment)
                    .HasForeignKey(d => d.LicenseAttachmentId)
                    .HasConstraintName("FK_InstitutionHistory_LicenseAttachment");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.InstitutionHistory)
                    .HasForeignKey(d => d.RegionId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_InstitutionHistory_RegionType");
            });

            modelBuilder.Entity<InstitutionItem>(entity =>
            {
                entity.HasIndex(e => e.InstitutionId)
                    .HasName("I_InstitutionItem_InstitutionID");

                entity.HasIndex(e => new { e.Name, e.ParentId, e.InstitutionId })
                    .HasName("UK_InstitutionItem_Name")
                    .IsUnique();

                entity.Property(e => e.InstitutionItemId).HasColumnName("InstitutionItemID");

                entity.Property(e => e.BriefName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DirectionId).HasColumnName("DirectionID");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.ItemTypeId).HasColumnName("ItemTypeID");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ParentId).HasColumnName("ParentID");

                entity.Property(e => e.Site)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.Direction)
                    .WithMany(p => p.InstitutionItem)
                    .HasForeignKey(d => d.DirectionId)
                    .HasConstraintName("FK_InstitutionItem_Direction");

                entity.HasOne(d => d.Institution)
                    .WithMany(p => p.InstitutionItem)
                    .HasForeignKey(d => d.InstitutionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InstitutionItem_Institution");

                entity.HasOne(d => d.ItemType)
                    .WithMany(p => p.InstitutionItem)
                    .HasForeignKey(d => d.ItemTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InstitutionItem_InstitutionItemType");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_InstitutionItem_ParentInstitutionItem");
            });

            modelBuilder.Entity<InstitutionItemType>(entity =>
            {
                entity.HasKey(e => e.ItemTypeId);

                entity.HasIndex(e => e.Name)
                    .HasName("UK_InstitutionItemType_Name")
                    .IsUnique();

                entity.Property(e => e.ItemTypeId)
                    .HasColumnName("ItemTypeID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<InstitutionLicense>(entity =>
            {
                entity.HasKey(e => e.LicenseId);

                entity.Property(e => e.LicenseId).HasColumnName("LicenseID");

                entity.Property(e => e.AttachmentId).HasColumnName("AttachmentID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EiisId)
                    .HasColumnName("EIIS_ID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EsrpId).HasColumnName("Esrp_Id");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.LicenseDate).HasColumnType("datetime");

                entity.Property(e => e.LicenseNumber)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.InstitutionAttachment)
                    .WithOne(p => p.InstitutionLicense)
                    .HasForeignKey<InstitutionAttachment>(e => e.AttachmentId)
                    .HasConstraintName("FK_InstitutionLicense_InstitutionAttachment");

                entity.HasOne(d => d.Institution)
                    .WithMany(p => p.InstitutionLicense)
                    .HasForeignKey(d => d.InstitutionId)
                    .HasConstraintName("FK_InstitutionLicense_Institution");
            });

            modelBuilder.Entity<InstitutionLicenseStatus>(entity =>
            {
                entity.HasKey(e => e.LicenseStatusId);

                entity.Property(e => e.LicenseStatusId)
                    .HasColumnName("LicenseStatusID")
                    .ValueGeneratedNever();

                entity.Property(e => e.EiisId)
                    .HasColumnName("EIIS_ID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<InstitutionLicenseSupplement>(entity =>
            {
                entity.HasKey(e => e.LicSupplementId);

                entity.Property(e => e.LicSupplementId).HasColumnName("LicSupplementID");

                entity.Property(e => e.EiisId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ExpireDate).HasColumnType("datetime");

                entity.Property(e => e.RegNumber)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StatusId).HasDefaultValueSql("((6))");

                entity.HasOne(d => d.License)
                    .WithMany(p => p.InstitutionLicenseSupplement)
                    .HasForeignKey(d => d.LicenseId)
                    .HasConstraintName("FK_InstitutionLicenseSupplement_License");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.InstitutionLicenseSupplement)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InstitutionLicenseSupplement_InstitutionLicenseStatus");
            });

            modelBuilder.Entity<InstitutionProgram>(entity =>
            {
                entity.HasIndex(e => e.InstitutionId)
                    .HasName("I_InstitutionProgram_InstitutionID");

                entity.Property(e => e.InstitutionProgramId).HasColumnName("InstitutionProgramID");

                entity.Property(e => e.Code)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.Institution)
                    .WithMany(p => p.InstitutionProgram)
                    .HasForeignKey(d => d.InstitutionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InstitutionProgram_Institution");
            });

            modelBuilder.Entity<InstitutionStructure>(entity =>
            {
                entity.HasKey(e => e.InstitutionStructureId)
                    .IsClustered(false);

                entity.HasIndex(e => e.InstitutionItemId)
                    .HasName("I_InstitutionStructure_InstitutionItemID");

                entity.HasIndex(e => e.Lineage)
                    .HasName("UK_InstitutionStructure_Lineage")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.InstitutionStructureId).HasColumnName("InstitutionStructureID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Depth).HasDefaultValueSql("((1))");

                entity.Property(e => e.InstitutionItemId).HasColumnName("InstitutionItemID");

                entity.Property(e => e.Lineage)
                    .IsRequired()
                    .HasMaxLength(76)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(('/'+CONVERT([varchar](36),newid(),(0)))+'/')");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ParentId).HasColumnName("ParentID");

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.InstitutionItem)
                    .WithMany(p => p.InstitutionStructure)
                    .HasForeignKey(d => d.InstitutionItemId)
                    .HasConstraintName("FK_InstitutionStructure_InstitutionItem");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_InstitutionStructure_InstitutionStructure");
            });

            modelBuilder.Entity<InstitutionType>(entity =>
            {
                entity.HasIndex(e => e.BriefName)
                    .HasName("UK_InstitutionType_BriefName")
                    .IsUnique();

                entity.HasIndex(e => e.Name)
                    .HasName("UK_InstitutionType_Name")
                    .IsUnique();

                entity.Property(e => e.InstitutionTypeId)
                    .HasColumnName("InstitutionTypeID")
                    .ValueGeneratedNever();

                entity.Property(e => e.BriefName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LevelBudget>(entity =>
            {
                entity.HasKey(e => e.IdLevelBudget);

                entity.Property(e => e.IdLevelBudget).ValueGeneratedNever();

                entity.Property(e => e.BudgetName)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LicensedDirection>(entity =>
            {
                entity.HasKey(e => e.Ldid)
                    .HasName("PK_LicenseDirection");

                entity.Property(e => e.Ldid).HasColumnName("LDID");

                entity.Property(e => e.StatusId).HasDefaultValueSql("((6))");

                entity.HasOne(d => d.Direction)
                    .WithMany(p => p.LicensedDirection)
                    .HasForeignKey(d => d.DirectionId)
                    .HasConstraintName("FK_LicenseDirection_Direction");

                entity.HasOne(d => d.EduLevel)
                    .WithMany(p => p.LicensedDirection)
                    .HasForeignKey(d => d.EduLevelId)
                    .HasConstraintName("FK_LicensedDirection_AdmissionItemType");

                entity.HasOne(d => d.LicSupplement)
                    .WithMany(p => p.LicensedDirection)
                    .HasForeignKey(d => d.LicSupplementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LicensedDirection_InstitutionLicenseSupplement");

                entity.HasOne(d => d.License)
                    .WithMany(p => p.LicensedDirection)
                    .HasForeignKey(d => d.LicenseId)
                    .HasConstraintName("FK_LicenseDirection_License");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.LicensedDirection)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LicenseDirection_Status");
            });

            modelBuilder.Entity<LicensedDirectionStatus>(entity =>
            {
                entity.HasKey(e => e.StatusId);

                entity.Property(e => e.StatusId)
                    .HasColumnName("StatusID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Action).IsUnicode(false);

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<MapAdmissionData>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("map_AdmissionData");
            });

            modelBuilder.Entity<MapAdmissionVolume>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("map_AdmissionVolume");

                entity.Property(e => e.NewUid)
                    .HasColumnName("NewUID")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.OldUid)
                    .HasColumnName("OldUID")
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MapApplication>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("map_Application");
            });

            modelBuilder.Entity<MapApplicationSelectedCompetitiveGroupItem>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("map_ApplicationSelectedCompetitiveGroupItem");
            });

            modelBuilder.Entity<MapCompetitiveGroupItem>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("map_CompetitiveGroupItem");

                entity.Property(e => e.NewUid)
                    .HasColumnName("NewUID")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.OldUid)
                    .HasColumnName("OldUID")
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MapCompetitiveGroupTargetItem>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("map_CompetitiveGroupTargetItem");

                entity.Property(e => e.NewUid)
                    .HasColumnName("NewUID")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.OldUid)
                    .HasColumnName("OldUID")
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MapDirections>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("map_Directions");

                entity.Property(e => e.NewUid)
                    .HasColumnName("NewUID")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.OldUid)
                    .HasColumnName("OldUID")
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MapEntranceTestProfileDirection>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("map_EntranceTestProfileDirection");
            });

            modelBuilder.Entity<MapInstitutionItem>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("map_InstitutionItem");

                entity.Property(e => e.NewUid)
                    .HasColumnName("NewUID")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.OldUid)
                    .HasColumnName("OldUID")
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MapInstitutionStructure>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("map_InstitutionStructure");

                entity.Property(e => e.NewUid)
                    .HasColumnName("NewUID")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.OldUid)
                    .HasColumnName("OldUID")
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MigrationHistory>(entity =>
            {
                entity.HasKey(e => new { e.MigrationId, e.ContextKey })
                    .HasName("PK_dbo.__MigrationHistory");

                entity.ToTable("__MigrationHistory");

                entity.Property(e => e.MigrationId).HasMaxLength(150);

                entity.Property(e => e.ContextKey).HasMaxLength(300);

                entity.Property(e => e.Model).IsRequired();

                entity.Property(e => e.ProductVersion)
                    .IsRequired()
                    .HasMaxLength(32);
            });

            modelBuilder.Entity<Migrations>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.ScriptDate).HasColumnType("datetime");

                entity.Property(e => e.ScriptName)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MinScoreByRon>(entity =>
            {
                entity.ToTable("MinScoreByRON");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.MinScoreByRon)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MinScoreByRON_MinScoreByRON");
            });

            modelBuilder.Entity<MonitoringBy1199Order>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Monitoring_by_1199_order");

                entity.Property(e => e.FederalName)
                    .HasColumnName("federalName")
                    .IsUnicode(false);

                entity.Property(e => e.Founders)
                    .HasColumnName("founders")
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .HasColumnName("fullName")
                    .IsUnicode(false);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Inn)
                    .HasColumnName("INN")
                    .IsUnicode(false);

                entity.Property(e => e.IsFilial)
                    .HasColumnName("isFilial")
                    .IsUnicode(false);

                entity.Property(e => e.IsPrivate).IsUnicode(false);

                entity.Property(e => e.MainId).HasColumnName("mainId");

                entity.Property(e => e.RegionName)
                    .HasColumnName("regionName")
                    .IsUnicode(false);

                entity.Property(e => e._11)
                    .HasColumnName("1.1")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._110)
                    .HasColumnName("1.1.0")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._111)
                    .HasColumnName("1.1.1")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._12)
                    .HasColumnName("1.2")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._13)
                    .HasColumnName("1.3")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._14)
                    .HasColumnName("1.4")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._15)
                    .HasColumnName("1.5")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._16)
                    .HasColumnName("1.6")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._17)
                    .HasColumnName("1.7")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._18)
                    .HasColumnName("1.8")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._21)
                    .HasColumnName("2.1")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._22)
                    .HasColumnName("2.2")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._23)
                    .HasColumnName("2.3")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._24)
                    .HasColumnName("2.4")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._25)
                    .HasColumnName("2.5")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._26)
                    .HasColumnName("2.6")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._31)
                    .HasColumnName("3.1")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._32)
                    .HasColumnName("3.2")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._41)
                    .HasColumnName("4.1")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._42)
                    .HasColumnName("4.2")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._43)
                    .HasColumnName("4.3")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._51)
                    .HasColumnName("5.1")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._61)
                    .HasColumnName("6.1")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._62)
                    .HasColumnName("6.2")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._63)
                    .HasColumnName("6.3")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._64)
                    .HasColumnName("6.4")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._65)
                    .HasColumnName("6.5")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._66)
                    .HasColumnName("6.6")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MonitoringBy1199OrderKvk>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Monitoring_by_1199_order_kvk");

                entity.Property(e => e.FederalName)
                    .HasColumnName("federalName")
                    .IsUnicode(false);

                entity.Property(e => e.Founders)
                    .HasColumnName("founders")
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .HasColumnName("fullName")
                    .IsUnicode(false);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Inn)
                    .HasColumnName("INN")
                    .IsUnicode(false);

                entity.Property(e => e.IsFilial)
                    .HasColumnName("isFilial")
                    .IsUnicode(false);

                entity.Property(e => e.IsPrivate).IsUnicode(false);

                entity.Property(e => e.MainId).HasColumnName("mainId");

                entity.Property(e => e.RegionName)
                    .HasColumnName("regionName")
                    .IsUnicode(false);

                entity.Property(e => e._11)
                    .HasColumnName("1.1")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._110)
                    .HasColumnName("1.1.0")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._111)
                    .HasColumnName("1.1.1")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._12)
                    .HasColumnName("1.2")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._13)
                    .HasColumnName("1.3")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._14)
                    .HasColumnName("1.4")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._15)
                    .HasColumnName("1.5")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._16)
                    .HasColumnName("1.6")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._17)
                    .HasColumnName("1.7")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._18)
                    .HasColumnName("1.8")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._21)
                    .HasColumnName("2.1")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._22)
                    .HasColumnName("2.2")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._23)
                    .HasColumnName("2.3")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._24)
                    .HasColumnName("2.4")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._25)
                    .HasColumnName("2.5")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._26)
                    .HasColumnName("2.6")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._31)
                    .HasColumnName("3.1")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._32)
                    .HasColumnName("3.2")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._41)
                    .HasColumnName("4.1")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._42)
                    .HasColumnName("4.2")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._43)
                    .HasColumnName("4.3")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._51)
                    .HasColumnName("5.1")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._61)
                    .HasColumnName("6.1")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._62)
                    .HasColumnName("6.2")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._63)
                    .HasColumnName("6.3")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._64)
                    .HasColumnName("6.4")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._65)
                    .HasColumnName("6.5")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._66)
                    .HasColumnName("6.6")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MonitoringBy1199OrderList3>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Monitoring_by_1199_order_list_3");

                entity.Property(e => e.D1)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.D2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.D3)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.D4)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.D5)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.D6)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.D8).HasColumnType("decimal(7, 0)");

                entity.Property(e => e.FederalName)
                    .HasColumnName("federalName")
                    .HasMaxLength(255);

                entity.Property(e => e.Founders)
                    .IsRequired()
                    .HasColumnName("founders");

                entity.Property(e => e.FullName)
                    .HasColumnName("fullName")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Inn)
                    .HasColumnName("INN")
                    .HasMaxLength(50);

                entity.Property(e => e.IsFilial)
                    .IsRequired()
                    .HasColumnName("isFilial")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.IsPrivate)
                    .IsRequired()
                    .HasMaxLength(17)
                    .IsUnicode(false);

                entity.Property(e => e.MainId).HasColumnName("mainId");

                entity.Property(e => e.Num).HasColumnName("num");

                entity.Property(e => e.RegionName)
                    .HasColumnName("regionName")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<MonitoringBy1199OrderM>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Monitoring_by_1199_order_m");

                entity.Property(e => e.FederalName)
                    .HasColumnName("federalName")
                    .IsUnicode(false);

                entity.Property(e => e.Founders)
                    .HasColumnName("founders")
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .HasColumnName("fullName")
                    .IsUnicode(false);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Inn)
                    .HasColumnName("INN")
                    .IsUnicode(false);

                entity.Property(e => e.IsFilial)
                    .HasColumnName("isFilial")
                    .IsUnicode(false);

                entity.Property(e => e.IsPrivate).IsUnicode(false);

                entity.Property(e => e.MainId).HasColumnName("mainId");

                entity.Property(e => e.RegionName)
                    .HasColumnName("regionName")
                    .IsUnicode(false);

                entity.Property(e => e._11)
                    .HasColumnName("1.1")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._110)
                    .HasColumnName("1.1.0")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._111)
                    .HasColumnName("1.1.1")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._12)
                    .HasColumnName("1.2")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._13)
                    .HasColumnName("1.3")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._14)
                    .HasColumnName("1.4")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._15)
                    .HasColumnName("1.5")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._16)
                    .HasColumnName("1.6")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._17)
                    .HasColumnName("1.7")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._18)
                    .HasColumnName("1.8")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._21)
                    .HasColumnName("2.1")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._22)
                    .HasColumnName("2.2")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._23)
                    .HasColumnName("2.3")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._24)
                    .HasColumnName("2.4")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._25)
                    .HasColumnName("2.5")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._26)
                    .HasColumnName("2.6")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._31)
                    .HasColumnName("3.1")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._32)
                    .HasColumnName("3.2")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._41)
                    .HasColumnName("4.1")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._42)
                    .HasColumnName("4.2")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._43)
                    .HasColumnName("4.3")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._51)
                    .HasColumnName("5.1")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._61)
                    .HasColumnName("6.1")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._62)
                    .HasColumnName("6.2")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._63)
                    .HasColumnName("6.3")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._64)
                    .HasColumnName("6.4")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._65)
                    .HasColumnName("6.5")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e._66)
                    .HasColumnName("6.6")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MonitoringPriem>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Action)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.DateBegin).HasColumnType("datetime");

                entity.Property(e => e.DateEnd).HasColumnType("datetime");

                entity.Property(e => e.EsrpProdId).HasColumnName("esrp_prodId");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MonitoringPriemV2>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.DateBegin).HasColumnType("datetime");

                entity.Property(e => e.DateEnd).HasColumnType("datetime");

                entity.Property(e => e.EsrpProdid).HasColumnName("esrp_prodid");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Number)
                    .HasColumnName("number")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MonitoringPriemV3>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.DateBegin).HasColumnType("datetime");

                entity.Property(e => e.DateEnd).HasColumnType("datetime");

                entity.Property(e => e.EsrpProdid).HasColumnName("esrp_prodid");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Number)
                    .HasColumnName("number")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MonitoringPriemV31>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("MonitoringPriemV3_1");

                entity.Property(e => e.DateBegin).HasColumnType("datetime");

                entity.Property(e => e.DateEnd).HasColumnType("datetime");

                entity.Property(e => e.EsrpProdid).HasColumnName("esrp_prodid");

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Number)
                    .HasColumnName("number")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MyStudents>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("my_students");

                entity.Property(e => e.BDate)
                    .IsRequired()
                    .HasColumnName("b_date")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.BDateFis)
                    .HasColumnName("b_date_fis")
                    .HasColumnType("date");

                entity.Property(e => e.Fio)
                    .IsRequired()
                    .HasColumnName("FIO")
                    .HasMaxLength(41)
                    .IsUnicode(false);

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<MyStudents2911>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("my_students_2911");

                entity.Property(e => e.BDate)
                    .HasColumnName("b_date")
                    .HasColumnType("date");

                entity.Property(e => e.Fio)
                    .IsRequired()
                    .HasColumnName("FIO")
                    .HasMaxLength(41)
                    .IsUnicode(false);

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<MyStudents29112>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("my_students_2911_2");

                entity.Property(e => e.BDate)
                    .HasColumnName("b_date")
                    .HasColumnType("date");

                entity.Property(e => e.Fio)
                    .IsRequired()
                    .HasColumnName("FIO")
                    .HasMaxLength(41)
                    .IsUnicode(false);

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<MyStudents2911Back>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("my_students_2911_back");

                entity.Property(e => e.BDate)
                    .IsRequired()
                    .HasColumnName("b_date")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Fio)
                    .IsRequired()
                    .HasColumnName("FIO")
                    .HasMaxLength(41)
                    .IsUnicode(false);

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<MyStudentsBack>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("my_students_back");

                entity.Property(e => e.BDate)
                    .IsRequired()
                    .HasColumnName("b_date")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Fio)
                    .IsRequired()
                    .HasColumnName("FIO")
                    .HasMaxLength(41)
                    .IsUnicode(false);

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<NationalityType>(entity =>
            {
                entity.HasKey(e => e.NationalityId);

                entity.Property(e => e.NationalityId)
                    .HasColumnName("NationalityID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(120)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<NormativeDictionary>(entity =>
            {
                entity.HasKey(e => e.DictionaryId);

                entity.HasIndex(e => e.Name)
                    .HasName("UK_NormativeDictionary_Name")
                    .IsUnique();

                entity.Property(e => e.DictionaryId)
                    .HasColumnName("DictionaryID")
                    .ValueGeneratedNever();

                entity.Property(e => e.ActivationDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.VersionId).HasColumnName("VersionID");

                entity.Property(e => e.VersionStateId).HasColumnName("VersionStateID");

                entity.HasOne(d => d.VersionState)
                    .WithMany(p => p.NormativeDictionary)
                    .HasForeignKey(d => d.VersionStateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NormativeDictionary_NormativeVersionState");
            });

            modelBuilder.Entity<NormativeVersionState>(entity =>
            {
                entity.HasKey(e => e.VersionStateId);

                entity.Property(e => e.VersionStateId).HasColumnName("VersionStateID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<NostrificationTypes>(entity =>
            {
                entity.HasKey(e => e.NostrificationTypeId)
                    .HasName("PK__Nostrifi__554BB8370EC32C7A");

                entity.Property(e => e.NostrificationTypeId).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Npsvuzbudg>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("NPSVUZBUDG");

                entity.Property(e => e.AvgEgeBvi).HasColumnName("avgEgeBvi");

                entity.Property(e => e.AvgEgeClear).HasColumnName("avgEgeClear");

                entity.Property(e => e.EntrantCount).HasColumnName("entrantCount");

                entity.Property(e => e.InstitutionId).HasColumnName("institutionId");

                entity.Property(e => e.MinEntranceValue).HasColumnName("minEntranceValue");

                entity.Property(e => e.Nps)
                    .HasColumnName("nps")
                    .HasMaxLength(500);

                entity.Property(e => e.PlaceCount).HasColumnName("placeCount");

                entity.Property(e => e.Vuz)
                    .HasColumnName("vuz")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<Npsvuzplat>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("NPSVUZPLAT");

                entity.Property(e => e.AvgEgeBvi).HasColumnName("avgEgeBvi");

                entity.Property(e => e.AvgEgeClear).HasColumnName("avgEgeClear");

                entity.Property(e => e.EntrantCount).HasColumnName("entrantCount");

                entity.Property(e => e.InstitutionId).HasColumnName("institutionId");

                entity.Property(e => e.MinEntranceValue).HasColumnName("minEntranceValue");

                entity.Property(e => e.Nps)
                    .HasColumnName("nps")
                    .HasMaxLength(500);

                entity.Property(e => e.PlaceCount).HasColumnName("placeCount");

                entity.Property(e => e.Vuz)
                    .HasColumnName("vuz")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<Olympic1Ege>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("olympic1_ege");

                entity.Property(e => e.BirthDate)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CreateDate)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CreatedDate)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.DocumentNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentSeries)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.OtName)
                    .IsRequired()
                    .HasColumnName("otName")
                    .HasMaxLength(1023)
                    .IsUnicode(false);

                entity.Property(e => e.Place)
                    .IsRequired()
                    .HasColumnName("place")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ProfileName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.SName)
                    .HasColumnName("s_name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<OlympicClasses>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Class)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<OlympicDiplomType>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UK_OlympicDiplomType_Name")
                    .IsUnique();

                entity.Property(e => e.OlympicDiplomTypeId).HasColumnName("OlympicDiplomTypeID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<OlympicDiplomant>(entity =>
            {
                entity.HasIndex(e => e.OlympicDiplomantIdentityDocumentId)
                    .HasName("I_OlympicDiplomant_OlympicDiplomantIdentityDocumentID");

                entity.HasIndex(e => e.OlympicTypeProfileId)
                    .HasName("I_OlympicDiplomant_OlympicTypeProfileID");

                entity.HasIndex(e => e.PersonId)
                    .HasName("I_OlympicDiplomant_PersonId");

                entity.Property(e => e.OlympicDiplomantId).HasColumnName("OlympicDiplomantID");

                entity.Property(e => e.AdoptionUnfoundedComment).IsUnicode(false);

                entity.Property(e => e.Comment).IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.DeleteDate).HasColumnType("datetime");

                entity.Property(e => e.DiplomaDateIssue).HasColumnType("datetime");

                entity.Property(e => e.DiplomaNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DiplomaSeries)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.OlympicDiplomantIdentityDocumentId).HasColumnName("OlympicDiplomantIdentityDocumentID");

                entity.Property(e => e.OlympicTypeProfileId).HasColumnName("OlympicTypeProfileID");

                entity.Property(e => e.PersonLinkDate).HasColumnType("datetime");

                entity.Property(e => e.ResultLevelId).HasColumnName("ResultLevelID");

                entity.Property(e => e.SchoolEgeName)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.SchoolRegionId).HasColumnName("SchoolRegionID");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.HasOne(d => d.OlympicDiplomantIdentityDocument)
                    .WithMany(p => p.OlympicDiplomant)
                    .HasForeignKey(d => d.OlympicDiplomantIdentityDocumentId)
                    .HasConstraintName("FK_OlympicDiplomant_OlympicDiplomantDocument");

                entity.HasOne(d => d.OlympicTypeProfile)
                    .WithMany(p => p.OlympicDiplomant)
                    .HasForeignKey(d => d.OlympicTypeProfileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OlympicDiplomant_OlympicTypeProfile");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.OlympicDiplomant)
                    .HasForeignKey(d => d.PersonId)
                    .HasConstraintName("FK_OlympicDiplomant_RVIPersons");

                entity.HasOne(d => d.ResultLevel)
                    .WithMany(p => p.OlympicDiplomant)
                    .HasForeignKey(d => d.ResultLevelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OlympicDiplomant_OlympicDiplomType");

                entity.HasOne(d => d.SchoolRegion)
                    .WithMany(p => p.OlympicDiplomant)
                    .HasForeignKey(d => d.SchoolRegionId)
                    .HasConstraintName("FK_OlympicDiplomant_RegionType");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.OlympicDiplomant)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_OlympicDiplomant_OlympicDiplomantStatus");
            });

            modelBuilder.Entity<OlympicDiplomantDocument>(entity =>
            {
                entity.HasIndex(e => e.OlympicDiplomantId)
                    .HasName("I_OlympicDiplomantDocument_OlympicDiplomantID");

                entity.HasIndex(e => new { e.LastName, e.FirstName, e.DocumentNumber })
                    .HasName("I_OlympicDiplomantDocument_Name_DocumentNumber");

                entity.Property(e => e.OlympicDiplomantDocumentId).HasColumnName("OlympicDiplomantDocumentID");

                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.DateIssue).HasColumnType("datetime");

                entity.Property(e => e.DocumentNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentSeries)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.IdentityDocumentTypeId).HasColumnName("IdentityDocumentTypeID");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.OlympicDiplomantId).HasColumnName("OlympicDiplomantID");

                entity.Property(e => e.OrganizationIssue)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdentityDocumentType)
                    .WithMany(p => p.OlympicDiplomantDocument)
                    .HasForeignKey(d => d.IdentityDocumentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OlympicDiplomantDocument_IdentityDocumentType");

                entity.HasOne(d => d.OlympicDiplomantNavigation)
                    .WithMany(p => p.OlympicDiplomantDocument)
                    .HasForeignKey(d => d.OlympicDiplomantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OlympicDiplomantDocument_OlympicDiplomant");
            });

            modelBuilder.Entity<OlympicDiplomantStatus>(entity =>
            {
                entity.Property(e => e.OlympicDiplomantStatusId).HasColumnName("OlympicDiplomantStatusID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<OlympicLevel>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UK_OlympicLevel_Name")
                    .IsUnique();

                entity.Property(e => e.OlympicLevelId).HasColumnName("OlympicLevelID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<OlympicProfile>(entity =>
            {
                entity.HasIndex(e => e.ProfileName)
                    .HasName("I_OlympicProfile_ProfileName");

                entity.Property(e => e.OlympicProfileId).HasColumnName("OlympicProfileID");

                entity.Property(e => e.ProfileName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<OlympicSubject>(entity =>
            {
                entity.Property(e => e.OlympicSubjectId).HasColumnName("OlympicSubjectID");

                entity.Property(e => e.OlympicTypeProfileId).HasColumnName("OlympicTypeProfileID");

                entity.Property(e => e.SubjectId).HasColumnName("SubjectID");

                entity.HasOne(d => d.OlympicTypeProfile)
                    .WithMany(p => p.OlympicSubject)
                    .HasForeignKey(d => d.OlympicTypeProfileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OlympicSubject_OlympicTypeProfile");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.OlympicSubject)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OlympicSubject_Subject");
            });

            modelBuilder.Entity<OlympicType>(entity =>
            {
                entity.HasKey(e => e.OlympicId);

                entity.HasIndex(e => e.Name)
                    .HasName("I_OlympicType_Name");

                entity.Property(e => e.OlympicId).HasColumnName("OlympicID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(1023)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<OlympicTypeCopy>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("OlympicType_copy");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(1023)
                    .IsUnicode(false);

                entity.Property(e => e.OlympicId).HasColumnName("OlympicID");

                entity.Property(e => e.OlympicLevelId).HasColumnName("OlympicLevelID");

                entity.Property(e => e.OrganizerName)
                    .HasMaxLength(4000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<OlympicTypeProfile>(entity =>
            {
                entity.HasIndex(e => new { e.OrgOlympicEnterId, e.OlympicProfileId, e.OlympicTypeId })
                    .HasName("I_OlympicTypeProfile_OrgOlympicEnterID");

                entity.Property(e => e.OlympicTypeProfileId).HasColumnName("OlympicTypeProfileID");

                entity.Property(e => e.CoOrganizerId).HasColumnName("CoOrganizerID");

                entity.Property(e => e.Email)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.OlympicLevelId).HasColumnName("OlympicLevelID");

                entity.Property(e => e.OlympicProfileId).HasColumnName("OlympicProfileID");

                entity.Property(e => e.OlympicTypeId).HasColumnName("OlympicTypeID");

                entity.Property(e => e.OrgOlympicEnterId).HasColumnName("OrgOlympicEnterID");

                entity.Property(e => e.OrganizerAddress)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.OrganizerId).HasColumnName("OrganizerID");

                entity.Property(e => e.OrganizerName)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Position)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.CoOrganizer)
                    .WithMany(p => p.OlympicTypeProfileCoOrganizer)
                    .HasForeignKey(d => d.CoOrganizerId)
                    .HasConstraintName("FK_OlympicTypeProfile_CoOrganizer");

                entity.HasOne(d => d.OlympicLevel)
                    .WithMany(p => p.OlympicTypeProfile)
                    .HasForeignKey(d => d.OlympicLevelId)
                    .HasConstraintName("FK_OlympicTypeProfile_OlympicLevel");

                entity.HasOne(d => d.OlympicProfile)
                    .WithMany(p => p.OlympicTypeProfile)
                    .HasForeignKey(d => d.OlympicProfileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OlympicTypeProfile_OlympicProfile");

                entity.HasOne(d => d.OlympicType)
                    .WithMany(p => p.OlympicTypeProfile)
                    .HasForeignKey(d => d.OlympicTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OlympicTypeProfile_OlympicType");

                entity.HasOne(d => d.OrgOlympicEnter)
                    .WithMany(p => p.OlympicTypeProfileOrgOlympicEnter)
                    .HasForeignKey(d => d.OrgOlympicEnterId)
                    .HasConstraintName("FK_OlympicTypeProfile_OlympicEnter");

                entity.HasOne(d => d.Organizer)
                    .WithMany(p => p.OlympicTypeProfileOrganizer)
                    .HasForeignKey(d => d.OrganizerId)
                    .HasConstraintName("FK_OlympicTypeProfile_Organizer");
            });

            modelBuilder.Entity<OlympicsSubjects>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("olympics_subjects");

                entity.Property(e => e.BirthDate)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CreatedDate)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.DocumentNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentSeries)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.OtName)
                    .IsRequired()
                    .HasColumnName("otName")
                    .HasMaxLength(1023)
                    .IsUnicode(false);

                entity.Property(e => e.Place)
                    .IsRequired()
                    .HasColumnName("place")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ProfileName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.SName)
                    .HasColumnName("s_name")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<OrdA>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("__ORD_A");

                entity.Property(e => e.ARank).HasColumnName("aRank");

                entity.Property(e => e.AcgiId).HasColumnName("acgiID");

                entity.Property(e => e.AcgiPriority).HasColumnName("ACGI_Priority");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.BezVi).HasColumnName("bezVI");

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");

                entity.Property(e => e.CampaignName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.CompetitiveGroupItemId).HasColumnName("CompetitiveGroupItemID");

                entity.Property(e => e.CompetitiveGroupName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CompetitiveGroupTargetId).HasColumnName("CompetitiveGroupTargetID");

                entity.Property(e => e.CompetitiveGroupTargetName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ConditionCode)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.DirectionCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DirectionId).HasColumnName("DirectionID");

                entity.Property(e => e.DirectionName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.EducationFormId).HasColumnName("EducationFormID");

                entity.Property(e => e.EducationFormName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.EducationLevelId).HasColumnName("EducationLevelID");

                entity.Property(e => e.EducationLevelName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.EducationSourceId).HasColumnName("EducationSourceID");

                entity.Property(e => e.EducationSourceName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.EntrantId).HasColumnName("EntrantID");

                entity.Property(e => e.Fio)
                    .HasColumnName("FIO")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.IncludeTo)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.MarkIa).HasColumnName("MarkIA");

                entity.Property(e => e.SRank)
                    .HasColumnName("sRank")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<OrdC>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("__ORD_C");

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");

                entity.Property(e => e.CampaignName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.CompetitiveGroupItemId).HasColumnName("CompetitiveGroupItemID");

                entity.Property(e => e.CompetitiveGroupName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CompetitiveGroupTargetId).HasColumnName("CompetitiveGroupTargetID");

                entity.Property(e => e.CompetitiveGroupTargetName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ConditionCode)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.DirectionCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DirectionId).HasColumnName("DirectionID");

                entity.Property(e => e.DirectionName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.EducationFormId).HasColumnName("EducationFormID");

                entity.Property(e => e.EducationFormName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.EducationLevelId).HasColumnName("EducationLevelID");

                entity.Property(e => e.EducationLevelName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.EducationSourceId).HasColumnName("EducationSourceID");

                entity.Property(e => e.EducationSourceName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");
            });

            modelBuilder.Entity<OrderOfAdmission>(entity =>
            {
                entity.HasKey(e => e.OrderId);

                entity.HasIndex(e => e.CampaignId)
                    .HasName("I_OrderOfAdmission_CampaignID");

                entity.HasIndex(e => e.DateCreated)
                    .HasName("I_OrderOfAdmission_CreateDate");

                entity.HasIndex(e => e.InstitutionId)
                    .HasName("I_OrderOfAdmission_InstitutionID");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DateEdited).HasColumnType("datetime");

                entity.Property(e => e.DatePublished).HasColumnType("datetime");

                entity.Property(e => e.EducationFormId).HasColumnName("EducationFormID");

                entity.Property(e => e.EducationLevelId).HasColumnName("EducationLevelID");

                entity.Property(e => e.EducationSourceId).HasColumnName("EducationSourceID");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.OrderName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.OrderNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OrderOfAdmissionStatusId).HasColumnName("OrderOfAdmissionStatusID");

                entity.Property(e => e.OrderOfAdmissionTypeId).HasColumnName("OrderOfAdmissionTypeID");

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.Campaign)
                    .WithMany(p => p.OrderOfAdmission)
                    .HasForeignKey(d => d.CampaignId)
                    .HasConstraintName("FK_OrderOfAdmission_Campaign");

                entity.HasOne(d => d.EducationForm)
                    .WithMany(p => p.OrderOfAdmissionEducationForm)
                    .HasForeignKey(d => d.EducationFormId)
                    .HasConstraintName("FK_OrderOfAdmission_AdmissionItemTypeForm");

                entity.HasOne(d => d.EducationLevel)
                    .WithMany(p => p.OrderOfAdmissionEducationLevel)
                    .HasForeignKey(d => d.EducationLevelId)
                    .HasConstraintName("FK_OrderOfAdmission_AdmissionItemTypeLevel");

                entity.HasOne(d => d.EducationSource)
                    .WithMany(p => p.OrderOfAdmissionEducationSource)
                    .HasForeignKey(d => d.EducationSourceId)
                    .HasConstraintName("FK_OrderOfAdmission_AdmissionItemTypeSource");

                entity.HasOne(d => d.Institution)
                    .WithMany(p => p.OrderOfAdmission)
                    .HasForeignKey(d => d.InstitutionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderOfAdmission_Institution");

                entity.HasOne(d => d.OrderOfAdmissionStatus)
                    .WithMany(p => p.OrderOfAdmission)
                    .HasForeignKey(d => d.OrderOfAdmissionStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderOfAdmission_OrderOfAdmissionStatus");

                entity.HasOne(d => d.OrderOfAdmissionType)
                    .WithMany(p => p.OrderOfAdmission)
                    .HasForeignKey(d => d.OrderOfAdmissionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderOfAdmission_OrderOfAdmissionType");
            });

            modelBuilder.Entity<OrderOfAdmissionHistory>(entity =>
            {
                entity.HasIndex(e => new { e.ApplicationId, e.CreatedDate })
                    .HasName("I_OrderOfAdmissionHistory_CreateDate");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DatePublished).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.OrderOfAdmissionHistory)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderOfAdmissionHistory_Application");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderOfAdmissionHistory)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderOfAdmissionHistory_OrderOfAdmission");
            });

            modelBuilder.Entity<OrderOfAdmissionStatus>(entity =>
            {
                entity.Property(e => e.OrderOfAdmissionStatusId).HasColumnName("OrderOfAdmissionStatusID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<OrderOfAdmissionType>(entity =>
            {
                entity.Property(e => e.OrderOfAdmissionTypeId).HasColumnName("OrderOfAdmissionTypeID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<OrphanCategory>(entity =>
            {
                entity.Property(e => e.OrphanCategoryId)
                    .HasColumnName("OrphanCategoryID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<OvzVtgFrom2016To201718062018>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("OVZ_VTG_from2016_to2017_18062018");

                entity.Property(e => e.ParticipantId)
                    .HasColumnName("ParticipantID")
                    .HasMaxLength(50);

                entity.Property(e => e.Vtg).HasColumnName("VTG");

                entity.Property(e => e.Year).HasColumnName("YEAR");
            });

            modelBuilder.Entity<ParentDirection>(entity =>
            {
                entity.HasKey(e => e.ParentDirectionId)
                    .IsClustered(false);

                entity.Property(e => e.ParentDirectionId).HasColumnName("ParentDirectionID");

                entity.Property(e => e.Code)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EiisId)
                    .HasColumnName("EIIS_ID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EsrpId).HasColumnName("Esrp_Id");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ParentDirectionOld>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ParentDirection_old");

                entity.Property(e => e.Code)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ParentDirectionId).HasColumnName("ParentDirectionID");
            });

            modelBuilder.Entity<ParentDirectionPlanKcp>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ParentDirectionPlanKCP");

                entity.Property(e => e.EducationLevelId).HasColumnName("EducationLevelID");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.NumberBudgetOz).HasColumnName("NumberBudgetOZ");

                entity.Property(e => e.ParentDirectionId).HasColumnName("ParentDirectionID");
            });

            modelBuilder.Entity<ParentDirectionTmp>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ParentDirection_TMP");

                entity.Property(e => e.Code)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EiisId)
                    .HasColumnName("EIIS_ID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EsrpId).HasColumnName("Esrp_Id");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ParentDirectionId)
                    .HasColumnName("ParentDirectionID")
                    .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<ParentsLostCategory>(entity =>
            {
                entity.Property(e => e.ParentsLostCategoryId)
                    .HasColumnName("ParentsLostCategoryID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PartInv>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("part_inv");

                entity.Property(e => e.Gia)
                    .HasColumnName("GIA")
                    .IsUnicode(false);

                entity.Property(e => e.Number).HasColumnName("number");

                entity.Property(e => e.Participantid).HasColumnName("participantid");
            });

            modelBuilder.Entity<PartInv112019>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("part_inv_11__2019");

                entity.Property(e => e.Participantid).HasColumnName("participantid");
            });

            modelBuilder.Entity<PartInvGia9>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("part_inv_gia9");

                entity.Property(e => e.BDay)
                    .HasColumnName("b_day")
                    .HasColumnType("datetime");

                entity.Property(e => e.DocumentNumber)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentSeries)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Lastname)
                    .HasColumnName("lastname")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Middlename)
                    .HasColumnName("middlename")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Numb).HasColumnName("numb");

                entity.Property(e => e.ParticipantId).HasColumnName("ParticipantID");
            });

            modelBuilder.Entity<PartInvGia92019>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("part_inv_gia9(2019)");

                entity.Property(e => e.ParticipantId).HasColumnName("ParticipantID");
            });

            modelBuilder.Entity<PartInvGia9M>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("part_inv_gia9_m");

                entity.Property(e => e.BDay)
                    .HasColumnName("b_day")
                    .HasColumnType("datetime");

                entity.Property(e => e.DocumentNumber)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentSeries)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Lastname)
                    .HasColumnName("lastname")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Middlename)
                    .HasColumnName("middlename")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Numb).HasColumnName("numb");

                entity.Property(e => e.ParticipantId).HasColumnName("ParticipantID");
            });

            modelBuilder.Entity<PartInvGia9M2019>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("part_inv_gia9_m_2019");

                entity.Property(e => e.BDay)
                    .HasColumnName("b_day")
                    .HasColumnType("datetime");

                entity.Property(e => e.DocumentNumber).HasMaxLength(15);

                entity.Property(e => e.DocumentSeries).HasMaxLength(10);

                entity.Property(e => e.Lastname)
                    .HasColumnName("lastname")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Middlename)
                    .HasColumnName("middlename")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Numb).HasColumnName("numb");

                entity.Property(e => e.ParticipantId).HasColumnName("ParticipantID");
            });

            modelBuilder.Entity<PartOlymp>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("part_olymp");

                entity.Property(e => e.Participantid).HasColumnName("participantid");
            });

            modelBuilder.Entity<PartSkfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("part_SKFO");

                entity.Property(e => e.Participantid).HasColumnName("participantid");
            });

            modelBuilder.Entity<ParticipantsWithCompositionsHse2017>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("participantsWithCompositionsHSE_2017");

                entity.Property(e => e.ParticipantId).HasColumnName("ParticipantID");
            });

            modelBuilder.Entity<ParticipantsWithCompositionsHse2017Spb978>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("participantsWithCompositionsHSE_2017_SPB_978");

                entity.Property(e => e.ParticipantId).HasColumnName("ParticipantID");
            });

            modelBuilder.Entity<PersonalDataAccessLog>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AccessDate).HasColumnType("datetime");

                entity.Property(e => e.AccessMethod)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.Method)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.NewData).HasColumnType("text");

                entity.Property(e => e.ObjectId).HasColumnName("ObjectID");

                entity.Property(e => e.ObjectType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OldData).HasColumnType("text");

                entity.Property(e => e.UserLogin)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PlanAdmissionVolume>(entity =>
            {
                entity.Property(e => e.PlanAdmissionVolumeId).HasColumnName("PlanAdmissionVolumeID");

                entity.Property(e => e.AdmissionItemTypeId).HasColumnName("AdmissionItemTypeID");

                entity.Property(e => e.AdmissionVolumeGuid).HasColumnName("AdmissionVolumeGUID");

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DirectionId).HasColumnName("DirectionID");

                entity.Property(e => e.EducationFormId).HasColumnName("EducationFormID");

                entity.Property(e => e.EducationSourceId).HasColumnName("EducationSourceID");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Uid)
                    .HasColumnName("UID")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.AdmissionItemType)
                    .WithMany(p => p.PlanAdmissionVolumeAdmissionItemType)
                    .HasForeignKey(d => d.AdmissionItemTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PlanAdmissionVolume_AdmissionItemTypeID");

                entity.HasOne(d => d.Campaign)
                    .WithMany(p => p.PlanAdmissionVolume)
                    .HasForeignKey(d => d.CampaignId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PlanAdmissionVolume_CampaignID");

                entity.HasOne(d => d.Direction)
                    .WithMany(p => p.PlanAdmissionVolume)
                    .HasForeignKey(d => d.DirectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PlanAdmissionVolume_DirectionID");

                entity.HasOne(d => d.EducationForm)
                    .WithMany(p => p.PlanAdmissionVolumeEducationForm)
                    .HasForeignKey(d => d.EducationFormId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PlanAdmissionVolume_EducationFormID");

                entity.HasOne(d => d.EducationSource)
                    .WithMany(p => p.PlanAdmissionVolumeEducationSource)
                    .HasForeignKey(d => d.EducationSourceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PlanAdmissionVolume_EducationSourceID");
            });

            modelBuilder.Entity<PreparatoryCourse>(entity =>
            {
                entity.HasIndex(e => new { e.InstitutionId, e.CourseName })
                    .HasName("UK_PreparatoryCourse")
                    .IsUnique();

                entity.Property(e => e.PreparatoryCourseId).HasColumnName("PreparatoryCourseID");

                entity.Property(e => e.CourseName)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CourseTypeId).HasColumnName("CourseTypeID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Information).IsUnicode(false);

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.CourseType)
                    .WithMany(p => p.PreparatoryCourse)
                    .HasForeignKey(d => d.CourseTypeId)
                    .HasConstraintName("FK_PreparatoryCourse_CourseType");

                entity.HasOne(d => d.Institution)
                    .WithMany(p => p.PreparatoryCourse)
                    .HasForeignKey(d => d.InstitutionId)
                    .HasConstraintName("FK_PreparatoryCourse_Institution");

                entity.HasOne(d => d.MoreInformationNavigation)
                    .WithMany(p => p.PreparatoryCourse)
                    .HasForeignKey(d => d.MoreInformation)
                    .HasConstraintName("FK_PreparatoryCourse_Attachment");
            });

            modelBuilder.Entity<PrivateOrg>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("private_org");

                entity.Property(e => e.FisName).HasColumnName("FIS_name");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Profile).HasColumnName("profile");

                entity.Property(e => e.Region).HasColumnName("region");
            });

            modelBuilder.Entity<ProhibitionType>(entity =>
            {
                entity.Property(e => e.ProhibitionTypeId)
                    .HasColumnName("ProhibitionTypeID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Query>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.ДатаРождения)
                    .HasColumnName("Дата рождения")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Имя)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.НомерДокумента)
                    .HasColumnName("Номер документа")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Отчество)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Пол)
                    .IsRequired()
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.СерияДокумента)
                    .HasColumnName("Серия документа")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Фамилия)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RadiationWorkCategory>(entity =>
            {
                entity.Property(e => e.RadiationWorkCategoryId)
                    .HasColumnName("RadiationWorkCategoryID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Rating>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.HasOne(d => d.CompetitiveGroup)
                    .WithMany(p => p.Rating)
                    .HasForeignKey(d => d.CompetitiveGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Rating_CompetitiveGroup");
            });

            modelBuilder.Entity<RatingList>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.DateOfEdit).HasColumnType("datetime");

                entity.Property(e => e.PointsEe).HasColumnName("PointsEE");

                entity.Property(e => e.RatingId).HasColumnName("RatingID");

                entity.Property(e => e.ReasonForExclusion)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.RatingList)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RatingList_Application");

                entity.HasOne(d => d.Rating)
                    .WithMany(p => p.RatingList)
                    .HasForeignKey(d => d.RatingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RatingList_Rating");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.RatingList)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_RatingList_UserPolicy");
            });

            modelBuilder.Entity<RecomendedLists>(entity =>
            {
                entity.HasKey(e => e.RecListId);

                entity.Property(e => e.RecListId).HasColumnName("RecListID");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.DirectionId).HasColumnName("DirectionID");

                entity.Property(e => e.EduFormId).HasColumnName("EduFormID");

                entity.Property(e => e.EduLevelId).HasColumnName("EduLevelID");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.Rating).HasColumnType("decimal(7, 4)");
            });

            modelBuilder.Entity<RecomendedListsHistory>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DateAdd).HasColumnType("datetime");

                entity.Property(e => e.DateDelete).HasColumnType("datetime");

                entity.Property(e => e.RecListId).HasColumnName("RecListID");

                entity.HasOne(d => d.RecList)
                    .WithMany(p => p.RecomendedListsHistory)
                    .HasForeignKey(d => d.RecListId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RecomendedListsHistory_RecomendedLists1");
            });

            modelBuilder.Entity<RegionType>(entity =>
            {
                entity.HasKey(e => e.RegionId);

                entity.Property(e => e.RegionId).ValueGeneratedNever();

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DisplayOrder).HasDefaultValueSql("((1))");

                entity.Property(e => e.EsrpCode)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.OkatoCode)
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.OkatoModified).HasColumnType("datetime");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.RegionType)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RegionType_CountryType");
            });

            modelBuilder.Entity<ReligiousUniver>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Sortorder).HasColumnName("sortorder");
            });

            modelBuilder.Entity<RequestComments>(entity =>
            {
                entity.HasKey(e => e.CommentId);

                entity.Property(e => e.CommentId)
                    .HasColumnName("Comment ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Comment).HasColumnType("text");

                entity.Property(e => e.Commentor)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.DirectionId).HasColumnName("DirectionID");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.HasOne(d => d.RequestDirection)
                    .WithMany(p => p.RequestComments)
                    .HasForeignKey(d => new { d.DirectionId, d.InstitutionId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RequestComments_RequestDirection");
            });

            modelBuilder.Entity<RequestDirection>(entity =>
            {
                entity.HasKey(e => new { e.DirectionId, e.RequestId });

                entity.Property(e => e.DirectionId).HasColumnName("Direction ID");

                entity.Property(e => e.RequestId).HasColumnName("Request ID");

                entity.Property(e => e.Action)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Activity)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ChangeDate).HasColumnType("datetime");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.HasOne(d => d.Request)
                    .WithMany(p => p.RequestDirection)
                    .HasForeignKey(d => d.RequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RequestDirection_Request");
            });

            modelBuilder.Entity<Russian>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Hash).HasMaxLength(1000);

                entity.Property(e => e.Participant).HasMaxLength(1000);
            });

            modelBuilder.Entity<Russian10>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Russian_10");

                entity.Property(e => e.Hash).HasMaxLength(1000);

                entity.Property(e => e.Participant).HasMaxLength(1000);
            });

            modelBuilder.Entity<Russian11>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Russian_11");

                entity.Property(e => e.Hash).HasMaxLength(1000);

                entity.Property(e => e.Participant).HasMaxLength(1000);
            });

            modelBuilder.Entity<Russian12>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Russian_12");

                entity.Property(e => e.Hash).HasMaxLength(1000);

                entity.Property(e => e.Participant).HasMaxLength(1000);
            });

            modelBuilder.Entity<Russian2>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Russian_2");

                entity.Property(e => e.Hash).HasMaxLength(1000);

                entity.Property(e => e.Participant).HasMaxLength(1000);
            });

            modelBuilder.Entity<Russian3>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Russian_3");

                entity.Property(e => e.Hash).HasMaxLength(1000);

                entity.Property(e => e.Participant).HasMaxLength(1000);
            });

            modelBuilder.Entity<Russian4>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Russian_4");

                entity.Property(e => e.Hash).HasMaxLength(1000);

                entity.Property(e => e.Participant).HasMaxLength(1000);
            });

            modelBuilder.Entity<Russian5>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Russian_5");

                entity.Property(e => e.Hash).HasMaxLength(1000);

                entity.Property(e => e.Participant).HasMaxLength(1000);
            });

            modelBuilder.Entity<Russian6>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Russian_6");

                entity.Property(e => e.Hash).HasMaxLength(1000);

                entity.Property(e => e.Participant).HasMaxLength(1000);
            });

            modelBuilder.Entity<Russian7>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Russian_7");

                entity.Property(e => e.Hash).HasMaxLength(1000);

                entity.Property(e => e.Participant).HasMaxLength(1000);
            });

            modelBuilder.Entity<Russian8>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Russian_8");

                entity.Property(e => e.Hash).HasMaxLength(1000);

                entity.Property(e => e.Participant).HasMaxLength(1000);
            });

            modelBuilder.Entity<Russian9>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Russian_9");

                entity.Property(e => e.Hash).HasMaxLength(1000);

                entity.Property(e => e.Participant).HasMaxLength(1000);
            });

            modelBuilder.Entity<RussianDop>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Russian_dop");

                entity.Property(e => e.Hash).HasMaxLength(1000);

                entity.Property(e => e.Participant).HasMaxLength(1000);
            });

            modelBuilder.Entity<RviPersonIdNew>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("RVI_PersonID_new");

                entity.Property(e => e.EddocumentNumber)
                    .HasColumnName("EDDocumentNumber")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EddocumentSeries)
                    .HasColumnName("EDDocumentSeries")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EdibirthDate)
                    .HasColumnName("EDIBirthDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.EdiidentityDocumentTypeId).HasColumnName("EDIIdentityDocumentTypeID");

                entity.Property(e => e.EentrantId).HasColumnName("EEntrantID");

                entity.Property(e => e.EfirstName)
                    .HasColumnName("EFirstName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EidentityDocumentId).HasColumnName("EIdentityDocumentID");

                entity.Property(e => e.ElastName)
                    .HasColumnName("ELastName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EmiddleName)
                    .HasColumnName("EMiddleName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PbirthDay)
                    .HasColumnName("PBirthDay")
                    .HasColumnType("datetime");

                entity.Property(e => e.PdocumentNumber)
                    .HasColumnName("PDocumentNumber")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PdocumentSeries)
                    .HasColumnName("PDocumentSeries")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PdocumentTypeCode).HasColumnName("PDocumentTypeCode");

                entity.Property(e => e.Pname)
                    .HasColumnName("PName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PparticipantId).HasColumnName("PParticipantID");

                entity.Property(e => e.PsecondName)
                    .HasColumnName("PSecondName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Psurname)
                    .HasColumnName("PSurname")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RvibirthDay)
                    .HasColumnName("RVIBirthDay")
                    .HasColumnType("datetime");

                entity.Property(e => e.RvinormName)
                    .HasColumnName("RVINormName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RvinormSecondName)
                    .HasColumnName("RVINormSecondName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RvinormSurname)
                    .HasColumnName("RVINormSurname")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RvipersonId).HasColumnName("RVIPersonId");
            });

            modelBuilder.Entity<RviPersonIdOld>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("RVI_PersonID_old");

                entity.Property(e => e.EddocumentNumber)
                    .HasColumnName("EDDocumentNumber")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EddocumentSeries)
                    .HasColumnName("EDDocumentSeries")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EdibirthDate)
                    .HasColumnName("EDIBirthDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.EdiidentityDocumentTypeId).HasColumnName("EDIIdentityDocumentTypeID");

                entity.Property(e => e.EentrantId).HasColumnName("EEntrantID");

                entity.Property(e => e.EfirstName)
                    .HasColumnName("EFirstName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EidentityDocumentId).HasColumnName("EIdentityDocumentID");

                entity.Property(e => e.ElastName)
                    .HasColumnName("ELastName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EmiddleName)
                    .HasColumnName("EMiddleName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PbirthDay)
                    .HasColumnName("PBirthDay")
                    .HasColumnType("datetime");

                entity.Property(e => e.PdocumentNumber)
                    .HasColumnName("PDocumentNumber")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PdocumentSeries)
                    .HasColumnName("PDocumentSeries")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PdocumentTypeCode).HasColumnName("PDocumentTypeCode");

                entity.Property(e => e.Pname)
                    .HasColumnName("PName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PparticipantId).HasColumnName("PParticipantID");

                entity.Property(e => e.PsecondName)
                    .HasColumnName("PSecondName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Psurname)
                    .HasColumnName("PSurname")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RvibirthDay)
                    .HasColumnName("RVIBirthDay")
                    .HasColumnType("datetime");

                entity.Property(e => e.RvinormName)
                    .HasColumnName("RVINormName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RvinormSecondName)
                    .HasColumnName("RVINormSecondName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RvinormSurname)
                    .HasColumnName("RVINormSurname")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RvipersonId).HasColumnName("RVIPersonId");
            });

            modelBuilder.Entity<RvidocumentTypes>(entity =>
            {
                entity.HasKey(e => e.DocumentTypeCode);

                entity.ToTable("RVIDocumentTypes");

                entity.Property(e => e.DocumentTypeCode).ValueGeneratedNever();

                entity.Property(e => e.DocumentTypeName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<RvipersonIdentDocs>(entity =>
            {
                entity.HasKey(e => e.PersonIdentDocId)
                    .IsClustered(false);

                entity.ToTable("RVIPersonIdentDocs");

                entity.HasIndex(e => new { e.DocumentTypeCode, e.DocumentSeries, e.DocumentNumber, e.CreateDate, e.PersonId })
                    .HasName("I_RVIPersonIdentDocs_PersonId");

                entity.Property(e => e.PersonIdentDocId)
                    .HasColumnName("PersonIdentDocID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.DocumentDate).HasColumnType("datetime");

                entity.Property(e => e.DocumentName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentNumber)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentOrganization)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentSeries)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.DocumentTypeCodeNavigation)
                    .WithMany(p => p.RvipersonIdentDocs)
                    .HasForeignKey(d => d.DocumentTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RVIPersonIdentDocs_RVIDocumentTypes");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.RvipersonIdentDocs)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RVIPersonIdentDocs_RVIPersons");
            });

            modelBuilder.Entity<RvipersonIdentDocsTmp>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("RVIPersonIdentDocs_TMP");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.DocumentDate).HasColumnType("datetime");

                entity.Property(e => e.DocumentName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentNumber)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentOrganization)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentSeries)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PersonIdentDocId).HasColumnName("PersonIdentDocID");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Rvipersons>(entity =>
            {
                entity.HasKey(e => e.PersonId);

                entity.ToTable("RVIPersons");

                entity.HasIndex(e => new { e.BirthDay, e.CreateDate, e.IsRecordDeleted, e.NormSurname, e.NormName, e.NormSecondName })
                    .HasName("I_RVIPersons_NormFullName");

                entity.Property(e => e.PersonId).ValueGeneratedNever();

                entity.Property(e => e.BirthDay).HasColumnType("datetime");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Inn)
                    .HasColumnName("INN")
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.IntegralUpdateDate).HasColumnType("datetime");

                entity.Property(e => e.MobilePhone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NormName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NormSecondName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NormSurname)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ParticipantId).HasColumnName("ParticipantID");

                entity.Property(e => e.Snils)
                    .HasColumnName("SNILS")
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<RvipersonsTmp>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("RVIPersons_TMP");

                entity.Property(e => e.BirthDay).HasColumnType("datetime");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Inn)
                    .HasColumnName("INN")
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.IntegralUpdateDate).HasColumnType("datetime");

                entity.Property(e => e.MobilePhone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NormName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NormSecondName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NormSurname)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ParticipantId).HasColumnName("ParticipantID");

                entity.Property(e => e.Snils)
                    .HasColumnName("SNILS")
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<SashaZ>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("sasha_z");

                entity.Property(e => e.EdulevelName)
                    .HasColumnName("EDULEVEL_NAME")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Inn)
                    .HasColumnName("INN")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Kpp)
                    .HasColumnName("KPP")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(1000);

                entity.Property(e => e.Ogrn)
                    .HasColumnName("OGRN")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SchoolFk)
                    .HasColumnName("school_fk")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<SearchTmp>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SearchTMP");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DocumentId).HasColumnName("DocumentID");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(120)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Selo2017>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("selo_2017");

                entity.Property(e => e.ParticipantId).HasColumnName("ParticipantID");
            });

            modelBuilder.Entity<Selo2018>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("selo_2018");

                entity.Property(e => e.ParticipantId).HasColumnName("ParticipantID");
            });

            modelBuilder.Entity<Sochinen>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Participant).HasMaxLength(1000);
            });

            modelBuilder.Entity<Social1>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Social_1");

                entity.Property(e => e.Hash).HasMaxLength(1000);

                entity.Property(e => e.Participant).HasMaxLength(1000);
            });

            modelBuilder.Entity<Social2>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Social_2");

                entity.Property(e => e.Hash).HasMaxLength(1000);

                entity.Property(e => e.Participant).HasMaxLength(1000);
            });

            modelBuilder.Entity<StateEmployeeCategory>(entity =>
            {
                entity.Property(e => e.StateEmployeeCategoryId)
                    .HasColumnName("StateEmployeeCategoryID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Student1>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("student_1");

                entity.Property(e => e.Bday)
                    .HasColumnName("bday")
                    .HasColumnType("datetime");

                entity.Property(e => e.BdayInt)
                    .IsRequired()
                    .HasColumnName("bday_int")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Fio)
                    .IsRequired()
                    .HasColumnName("fio")
                    .HasMaxLength(48)
                    .IsUnicode(false);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Post)
                    .IsRequired()
                    .HasColumnName("post")
                    .HasMaxLength(9)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<StudentFirst>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("student_first");

                entity.Property(e => e.Bday)
                    .HasColumnName("bday")
                    .HasColumnType("datetime");

                entity.Property(e => e.BdayInt)
                    .IsRequired()
                    .HasColumnName("bday_int")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Fio)
                    .IsRequired()
                    .HasColumnName("fio")
                    .HasMaxLength(48)
                    .IsUnicode(false);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Post)
                    .IsRequired()
                    .HasColumnName("post")
                    .HasMaxLength(9)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UK_Subject_Name")
                    .IsUnique();

                entity.Property(e => e.SubjectId).HasColumnName("SubjectID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SubjectEgeMinValue>(entity =>
            {
                entity.HasKey(e => e.ScoreId);

                entity.HasIndex(e => e.SubjectId)
                    .HasName("UK_SubjectEgeMinValue_SubjectID")
                    .IsUnique();

                entity.Property(e => e.ScoreId).HasColumnName("ScoreID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.SubjectId).HasColumnName("SubjectID");

                entity.HasOne(d => d.Subject)
                    .WithOne(p => p.SubjectEgeMinValue)
                    .HasForeignKey<SubjectEgeMinValue>(d => d.SubjectId)
                    .HasConstraintName("FK_SubjectEgeMinValue_Subject");
            });

            modelBuilder.Entity<SubjectEgeMinValueCopy>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SubjectEgeMinValue_copy");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ScoreId).HasColumnName("ScoreID");

                entity.Property(e => e.SubjectId).HasColumnName("SubjectID");
            });

            modelBuilder.Entity<TemRodion>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tem_Rodion");
            });

            modelBuilder.Entity<TempAndrei2018>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("temp_andrei_2018");

                entity.Property(e => e.InstitutionId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<TempApps>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("temp_apps");

                entity.Property(e => e.Apps).HasColumnName("apps");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.PackageId).HasColumnName("PackageID");
            });

            modelBuilder.Entity<TempEgedata>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TEMP_EGEdata");

                entity.Property(e => e.ДатаРождения).HasColumnType("date");

                entity.Property(e => e.Пол).HasMaxLength(10);
            });

            modelBuilder.Entity<TempGgv>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Temp_GGV");

                entity.Property(e => e.CurrentDate)
                    .HasColumnName("CURRENT_DATE")
                    .IsUnicode(false);

                entity.Property(e => e.ГоловнаяОрганизация)
                    .HasColumnName("Головная организация")
                    .IsUnicode(false);

                entity.Property(e => e.ГоловнойФилиал)
                    .HasColumnName("Головной/Филиал")
                    .IsUnicode(false);

                entity.Property(e => e.ЕсрпИд).HasColumnName("ЕСРП ИД");

                entity.Property(e => e.ЗакрылиПк2014)
                    .HasColumnName("Закрыли ПК 2014")
                    .IsUnicode(false);

                entity.Property(e => e.Зачисленные).IsUnicode(false);

                entity.Property(e => e.ЗачисленныеНаБюджет)
                    .HasColumnName("Зачисленные на бюджет")
                    .IsUnicode(false);

                entity.Property(e => e.ЗачисленныеНаПлатное)
                    .HasColumnName("Зачисленные на платное")
                    .IsUnicode(false);

                entity.Property(e => e.Инн)
                    .HasColumnName("ИНН")
                    .IsUnicode(false);

                entity.Property(e => e.КатегорияОо)
                    .HasColumnName("Категория ОО")
                    .IsUnicode(false);

                entity.Property(e => e.КатегорияОо2)
                    .HasColumnName("Категория ОО 2")
                    .IsUnicode(false);

                entity.Property(e => e.Кпп)
                    .HasColumnName("КПП")
                    .IsUnicode(false);

                entity.Property(e => e.КцпБюджет)
                    .HasColumnName("КЦП бюджет")
                    .IsUnicode(false);

                entity.Property(e => e.КцпПланНаПлатное)
                    .HasColumnName("КЦП + план на платное")
                    .IsUnicode(false);

                entity.Property(e => e.ОбязательностьПодключения)
                    .HasColumnName("Обязательность подключения")
                    .IsUnicode(false);

                entity.Property(e => e.Огрн)
                    .HasColumnName("ОГРН")
                    .IsUnicode(false);

                entity.Property(e => e.Опф)
                    .HasColumnName("ОПФ")
                    .IsUnicode(false);

                entity.Property(e => e.ПередаютДанныеЗаПк2015Года)
                    .HasColumnName("Передают данные за ПК 2015 года")
                    .IsUnicode(false);

                entity.Property(e => e.ПланНаПлатное)
                    .HasColumnName("План на платное")
                    .IsUnicode(false);

                entity.Property(e => e.ПоданныеЗаявления)
                    .HasColumnName("Поданные заявления")
                    .IsUnicode(false);

                entity.Property(e => e.ПоданныеНаБюджет)
                    .HasColumnName("Поданные на бюджет")
                    .IsUnicode(false);

                entity.Property(e => e.ПоданныеНаПлатное)
                    .HasColumnName("Поданные на платное")
                    .IsUnicode(false);

                entity.Property(e => e.ПолноеНаименование)
                    .HasColumnName("Полное наименование")
                    .IsUnicode(false);

                entity.Property(e => e.СтатусЛицензии)
                    .HasColumnName("Статус лицензии")
                    .IsUnicode(false);

                entity.Property(e => e.СтатусОо)
                    .HasColumnName("Статус ОО")
                    .IsUnicode(false);

                entity.Property(e => e.СтатусПодключения)
                    .HasColumnName("Статус подключения")
                    .IsUnicode(false);

                entity.Property(e => e.СубъектРф)
                    .HasColumnName("Субъект РФ")
                    .IsUnicode(false);

                entity.Property(e => e.СхемаПодключения)
                    .HasColumnName("Схема подключения")
                    .IsUnicode(false);

                entity.Property(e => e.Тип).IsUnicode(false);

                entity.Property(e => e.Учредитель).IsUnicode(false);

                entity.Property(e => e.Фо)
                    .HasColumnName("ФО")
                    .IsUnicode(false);

                entity.Property(e => e.ЭтапПодключения)
                    .HasColumnName("Этап подключения")
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TempIslodEsrpProd>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TEMP_ISLOD_esrp_prod");

                entity.Property(e => e.EsrpProdId)
                    .HasColumnName("esrp_prod_ID")
                    .HasMaxLength(50);

                entity.Property(e => e.IslodId)
                    .HasColumnName("ISLOD_ID")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TempOgedata>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TEMP_OGEdata");

                entity.Property(e => e.ДатаРождения).HasColumnType("date");

                entity.Property(e => e.Пол).HasMaxLength(10);
            });

            modelBuilder.Entity<TempPersonIdAll>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("temp_personId_All");
            });

            modelBuilder.Entity<TempPersonIdVioltionId1>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("temp_personId_violtionID1");
            });

            modelBuilder.Entity<TempPopko>(entity =>
            {
                entity.ToTable("temp_Popko");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("numeric(5, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RegionId)
                    .IsRequired()
                    .HasColumnName("regionID")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SecondName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SurName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Tmp1>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tmp_1");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");
            });

            modelBuilder.Entity<TmpAnn>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tmp_ann");

                entity.Property(e => e.Inn)
                    .IsRequired()
                    .HasColumnName("INN")
                    .HasMaxLength(14)
                    .IsUnicode(false);

                entity.Property(e => e.Kpp)
                    .IsRequired()
                    .HasColumnName("KPP")
                    .HasMaxLength(13)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TmpAnya2017>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tmp_anya2017");

                entity.Property(e => e.Value1).HasColumnName("value1");

                entity.Property(e => e.Value2).HasColumnName("value2");

                entity.Property(e => e.Value3).HasColumnName("value3");
            });

            modelBuilder.Entity<TmpCompositionsForViporganizations>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tmp_CompositionsForVIPOrganizations");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.ParticipantId).HasColumnName("ParticipantID");
            });

            modelBuilder.Entity<TmpEge>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tmp_ege");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.БаллЕгэ).HasColumnName("Балл ЕГЭ");

                entity.Property(e => e.БаллПриЗачислении)
                    .HasColumnName("Балл, при зачислении")
                    .HasColumnType("decimal(7, 4)");

                entity.Property(e => e.Предмет)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TmpEge1>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tmp_ege1");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.БаллЕгэ).HasColumnName("Балл ЕГЭ");

                entity.Property(e => e.БаллПриЗачислении)
                    .HasColumnName("Балл, при зачислении")
                    .HasColumnType("decimal(7, 4)");

                entity.Property(e => e.Предмет)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TmpEge2>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tmp_ege2");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.БаллЕгэ).HasColumnName("Балл ЕГЭ");

                entity.Property(e => e.БаллПриЗачислении)
                    .HasColumnName("Балл, при зачислении")
                    .HasColumnType("decimal(7, 4)");

                entity.Property(e => e.Предмет)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TmpEge3>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tmp_ege3");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.БаллПриЗачислении)
                    .HasColumnName("Балл, при зачислении")
                    .HasColumnType("decimal(7, 4)");

                entity.Property(e => e.ЕстьЕгэ).HasColumnName("Есть ЕГЭ");

                entity.Property(e => e.Предмет)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TmpEiisFis>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tmp_EIIS_FIS");

                entity.Property(e => e.FullName)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Guid)
                    .HasColumnName("GUID")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TmpEntrantsGak>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tmp_entrants_gak");

                entity.Property(e => e.Birthdate)
                    .HasColumnName("birthdate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Documentnumber)
                    .IsRequired()
                    .HasColumnName("documentnumber")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Documentseries)
                    .IsRequired()
                    .HasColumnName("documentseries")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Middlename)
                    .IsRequired()
                    .HasColumnName("middlename")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasColumnName("surname")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TmpFisFbsRegion>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tmp_fis_fbs_region");

                entity.Property(e => e.EntrantId).HasColumnName("EntrantID");

                entity.Property(e => e.Region).HasColumnName("REGION");
            });

            modelBuilder.Entity<TmpForRon2016>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tmp_ForRon_2016");

                entity.Property(e => e.BirthDate)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.EducationDocumentName)
                    .HasMaxLength(120)
                    .IsUnicode(false);

                entity.Property(e => e.EducationNumber)
                    .HasColumnName("Education_Number")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EducationSeries)
                    .HasColumnName("Education_Series")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.GodPostyplenia).HasColumnName("God_Postyplenia");

                entity.Property(e => e.GuidOo)
                    .HasColumnName("GUID_OO")
                    .HasMaxLength(500);

                entity.Property(e => e.IdentityNumber)
                    .HasColumnName("Identity_Number")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IdentitySeries)
                    .HasColumnName("Identity_Series")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Inn)
                    .HasColumnName("INN")
                    .HasMaxLength(14)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Level)
                    .HasColumnName("level")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ParticipantId)
                    .HasColumnName("ParticipantID")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<TmpFrdo2016>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TMP_FRDO_2016");

                entity.Property(e => e.ДатаРождения)
                    .HasColumnName("Дата рождения")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Имя)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Номер)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Отчество)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Пол)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Серия)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Фамилия)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TmpOlympicDiplomant>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TMP_OlympicDiplomant");

                entity.Property(e => e.AdoptionUnfoundedComment).IsUnicode(false);

                entity.Property(e => e.Comment).IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.DeleteDate).HasColumnType("datetime");

                entity.Property(e => e.DiplomaDateIssue).HasColumnType("datetime");

                entity.Property(e => e.DiplomaNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DiplomaSeries)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.OlympicDiplomantId)
                    .HasColumnName("OlympicDiplomantID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.OlympicDiplomantIdentityDocumentId).HasColumnName("OlympicDiplomantIdentityDocumentID");

                entity.Property(e => e.OlympicTypeProfileId).HasColumnName("OlympicTypeProfileID");

                entity.Property(e => e.PersonLinkDate).HasColumnType("datetime");

                entity.Property(e => e.ResultLevelId).HasColumnName("ResultLevelID");

                entity.Property(e => e.SchoolEgeName)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.SchoolRegionId).HasColumnName("SchoolRegionID");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");
            });

            modelBuilder.Entity<TmpOlympicDiplomantDocument>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TMP_OlympicDiplomantDocument");

                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.DateIssue).HasColumnType("datetime");

                entity.Property(e => e.DocumentNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentSeries)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.IdentityDocumentTypeId).HasColumnName("IdentityDocumentTypeID");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.OlympicDiplomantDocumentId)
                    .HasColumnName("OlympicDiplomantDocumentID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.OlympicDiplomantId).HasColumnName("OlympicDiplomantID");

                entity.Property(e => e.OrganizationIssue)
                    .HasMaxLength(1000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TmpOlympicTypeN>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tmp_OlympicTypeN");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(1023)
                    .IsUnicode(false);

                entity.Property(e => e.OlympicId)
                    .HasColumnName("OlympicID")
                    .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<TmpOlympicTypeProfile>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TMP_OlympicTypeProfile");

                entity.Property(e => e.CoOrganizerId).HasColumnName("CoOrganizerID");

                entity.Property(e => e.Email)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.OlympicLevelId).HasColumnName("OlympicLevelID");

                entity.Property(e => e.OlympicProfileId).HasColumnName("OlympicProfileID");

                entity.Property(e => e.OlympicTypeId).HasColumnName("OlympicTypeID");

                entity.Property(e => e.OlympicTypeProfileId)
                    .HasColumnName("OlympicTypeProfileID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.OrgOlympicEnterId).HasColumnName("OrgOlympicEnterID");

                entity.Property(e => e.OrganizerAddress)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.OrganizerId).HasColumnName("OrganizerID");

                entity.Property(e => e.OrganizerName)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Position)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TmpOvzEgorova>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TMP_OVZ_Egorova");

                entity.Property(e => e.ParticipantId)
                    .HasColumnName("ParticipantID")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Vtg).HasColumnName("VTG");

                entity.Property(e => e.Year).HasColumnName("YEAR");
            });

            modelBuilder.Entity<TmpPartId>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tmpPartID");

                entity.Property(e => e.PartId).HasColumnName("partID");

                entity.Property(e => e.Year).HasColumnName("year");
            });

            modelBuilder.Entity<TmpSirius16052018>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tmp_sirius_16052018");

                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.DCode)
                    .HasColumnName("d_code")
                    .HasMaxLength(50);

                entity.Property(e => e.DName)
                    .HasColumnName("d_name")
                    .HasMaxLength(255);

                entity.Property(e => e.EducationForm).HasMaxLength(50);

                entity.Property(e => e.EducationSource).HasMaxLength(50);

                entity.Property(e => e.Fio)
                    .HasColumnName("FIO")
                    .HasMaxLength(255);

                entity.Property(e => e.Number).HasColumnName("number");

                entity.Property(e => e.OFullname)
                    .HasColumnName("o_Fullname")
                    .HasMaxLength(255);

                entity.Property(e => e.RegionName).HasMaxLength(100);

                entity.Property(e => e.SYear).HasColumnName("s_year");
            });

            modelBuilder.Entity<TmpSledstvieVedetNikita>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tmp_sledstvie_vedet_Nikita");

                entity.Property(e => e.Birthdate)
                    .HasColumnName("birthdate")
                    .HasColumnType("datetime");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TmpUserPolicy>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tmp_UserPolicy");

                entity.Property(e => e.Comment)
                    .HasMaxLength(3000)
                    .IsUnicode(false);

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.FilialId).HasColumnName("FilialID");

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PinCode)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Position)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TmpYmPaPa>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tmp_ym_pa_pa");

                entity.Property(e => e.ВузВКоторомПродолжилОбучение)
                    .HasColumnName("Вуз, в котором продолжил обучение")
                    .HasMaxLength(100);

                entity.Property(e => e.ДатаРождения)
                    .HasColumnName("Дата рождения")
                    .HasMaxLength(10);

                entity.Property(e => e.Имя)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ИсточникФинансирования).HasColumnName("Источник финансирования");

                entity.Property(e => e.КодСпециальностиНаКоторойПродолжилОбучение)
                    .HasColumnName("Код специальности, на которой продолжил обучение")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.НомерДокумента)
                    .HasColumnName("Номер документа")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Отчество)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Пол)
                    .IsRequired()
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.СерияДокумента)
                    .HasColumnName("Серия документа")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.СпециальностьНаКоторойПродолжилОбучение)
                    .HasColumnName("Специальность, на которой продолжил обучение")
                    .HasMaxLength(3000)
                    .IsUnicode(false);

                entity.Property(e => e.УровеньОбразования).HasColumnName("Уровень образования");

                entity.Property(e => e.Фамилия)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ФормаОбучения).HasColumnName("Форма обучения");
            });

            modelBuilder.Entity<Top20>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("top20");

                entity.Property(e => e.Order).HasColumnName("order");
            });

            modelBuilder.Entity<TownType>(entity =>
            {
                entity.Property(e => e.TownTypeId).HasColumnName("TownTypeID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TranslationRvidtIdentityDt>(entity =>
            {
                entity.ToTable("Translation_RVIDT_IdentityDT");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.IdentityDocumentTypeId).HasColumnName("IdentityDocumentTypeID");

                entity.HasOne(d => d.DocumentTypeCodeNavigation)
                    .WithMany(p => p.TranslationRvidtIdentityDt)
                    .HasForeignKey(d => d.DocumentTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Translation_RVIDT_IdentityDT_RVIDocumentTypes");

                entity.HasOne(d => d.IdentityDocumentType)
                    .WithMany(p => p.TranslationRvidtIdentityDt)
                    .HasForeignKey(d => d.IdentityDocumentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Translation_RVIDT_IdentityDT_IdentityDocumentType");
            });

            modelBuilder.Entity<UgsFromEiis>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("UGS_FROM_EIIS");

                entity.Property(e => e.Code)
                    .HasColumnName("CODE")
                    .IsUnicode(false);

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasColumnName("ID")
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.IsActual).HasColumnName("IS_ACTUAL");

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .IsUnicode(false);

                entity.Property(e => e.NotTrue)
                    .HasColumnName("NOT_TRUE")
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Standart)
                    .HasColumnName("STANDART")
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UsedId>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("used_id");

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<UserPolicy>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.HasIndex(e => e.InstitutionId)
                    .HasName("I_UserPolicy_InstitutionID");

                entity.HasIndex(e => e.UserName)
                    .HasName("I_UserPolicy_UserName");

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Comment)
                    .HasMaxLength(6000)
                    .IsUnicode(false);

                entity.Property(e => e.DateUpdated).HasColumnType("datetime");

                entity.Property(e => e.FilialId).HasColumnName("FilialID");

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PinCode)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Position)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.Institution)
                    .WithMany(p => p.UserPolicy)
                    .HasForeignKey(d => d.InstitutionId)
                    .HasConstraintName("FK_UserPolicy_Institution");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.UserPolicy)
                    .HasForeignKey<UserPolicy>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserPolicy_aspnet_Users");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.InsertDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.Login).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(50);
            });

            modelBuilder.Entity<VCompetitiveGroup>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vCompetitiveGroup");

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");

                entity.Property(e => e.CompetitiveGroupId).HasColumnName("CompetitiveGroupID");

                entity.Property(e => e.DirectionId).HasColumnName("DirectionID");

                entity.Property(e => e.DirectionName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.EducationLevelId).HasColumnName("EducationLevelID");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.NumberBudgetOz).HasColumnName("NumberBudgetOZ");

                entity.Property(e => e.NumberPaidOz).HasColumnName("NumberPaidOZ");

                entity.Property(e => e.NumberTargetOz).HasColumnName("NumberTargetOZ");
            });

            modelBuilder.Entity<VEntrantDocuments>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vEntrantDocuments");

                entity.Property(e => e.EntrantDocumentId).HasColumnName("EntrantDocumentID");

                entity.Property(e => e.EntrantId).HasColumnName("EntrantID");
            });

            modelBuilder.Entity<VeteranCategory>(entity =>
            {
                entity.Property(e => e.VeteranCategoryId)
                    .HasColumnName("VeteranCategoryID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Village>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("village");

                entity.Property(e => e.Participant).HasMaxLength(1000);
            });

            modelBuilder.Entity<Violation>(entity =>
            {
                entity.HasIndex(e => e.EntrantDocumentId)
                    .HasName("UK_Violation_EntrantDocumentID")
                    .IsUnique();

                entity.Property(e => e.ViolationId).HasColumnName("ViolationID");

                entity.Property(e => e.BlankDate).HasColumnType("datetime");

                entity.Property(e => e.BlankRegNumber)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.DocumentDate).HasColumnType("datetime");

                entity.Property(e => e.DocumentNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentOrganization)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentSeries)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentTypeId).HasColumnName("DocumentTypeID");

                entity.Property(e => e.EducationLevelId).HasColumnName("EducationLevelID");

                entity.Property(e => e.EntrantDocumentId).HasColumnName("EntrantDocumentID");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.HasOne(d => d.Campaign)
                    .WithMany(p => p.Violation)
                    .HasForeignKey(d => d.CampaignId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Violation__Campa__6D4D2A16");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Violation)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Violation__Count__6E414E4F");

                entity.HasOne(d => d.DocumentType)
                    .WithMany(p => p.Violation)
                    .HasForeignKey(d => d.DocumentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Violation__Docum__6F357288");

                entity.HasOne(d => d.EducationForm)
                    .WithMany(p => p.ViolationEducationForm)
                    .HasForeignKey(d => d.EducationFormId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Violation__Educa__711DBAFA");

                entity.HasOne(d => d.EducationLevel)
                    .WithMany(p => p.ViolationEducationLevel)
                    .HasForeignKey(d => d.EducationLevelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Violation__Educa__702996C1");

                entity.HasOne(d => d.EntrantDocument)
                    .WithOne(p => p.Violation)
                    .HasForeignKey<Violation>(d => d.EntrantDocumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Violation__Entra__7211DF33");
            });

            modelBuilder.Entity<ViolationApplicationReception>(entity =>
            {
                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");

                entity.Property(e => e.DateAchievements).HasColumnType("datetime");

                entity.Property(e => e.DateApprovalEntranceTests).HasColumnType("datetime");

                entity.Property(e => e.DateEnrollmentOrder).HasColumnType("datetime");

                entity.Property(e => e.DateExclusionOrder).HasColumnType("datetime");

                entity.Property(e => e.DateReturnDocuments).HasColumnType("datetime");

                entity.Property(e => e.DateSubmissionInforamtionApplicants).HasColumnType("datetime");

                entity.Property(e => e.Has3DayViolationAchievements).HasDefaultValueSql("((0))");

                entity.Property(e => e.ViolationCheckDate).HasColumnType("datetime");

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.ViolationApplicationReception)
                    .HasForeignKey(d => d.ApplicationId)
                    .HasConstraintName("FK_ViolationApplicationReception_Application");

                entity.HasOne(d => d.Campaign)
                    .WithMany(p => p.ViolationApplicationReception)
                    .HasForeignKey(d => d.CampaignId)
                    .HasConstraintName("FK_ViolationApplicationReception_Campaign");
            });

            modelBuilder.Entity<ViolationType>(entity =>
            {
                entity.HasKey(e => e.ViolationId);

                entity.Property(e => e.ViolationId)
                    .HasColumnName("ViolationID")
                    .ValueGeneratedNever();

                entity.Property(e => e.BriefName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VtgGve>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("VTG_GVE");

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<VtgOvz>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("VTG_OVZ");

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<Vuz21>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("vuz21");

                entity.Property(e => e.Instid).HasColumnName("INSTID");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VwAspnetApplications>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_aspnet_Applications");

                entity.Property(e => e.ApplicationName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Description).HasMaxLength(256);

                entity.Property(e => e.LoweredApplicationName)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<VwAspnetMembershipUsers>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_aspnet_MembershipUsers");

                entity.Property(e => e.Comment).HasColumnType("ntext");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.FailedPasswordAnswerAttemptWindowStart).HasColumnType("datetime");

                entity.Property(e => e.FailedPasswordAttemptWindowStart).HasColumnType("datetime");

                entity.Property(e => e.LastActivityDate).HasColumnType("datetime");

                entity.Property(e => e.LastLockoutDate).HasColumnType("datetime");

                entity.Property(e => e.LastLoginDate).HasColumnType("datetime");

                entity.Property(e => e.LastPasswordChangedDate).HasColumnType("datetime");

                entity.Property(e => e.LoweredEmail).HasMaxLength(256);

                entity.Property(e => e.MobileAlias).HasMaxLength(16);

                entity.Property(e => e.MobilePin)
                    .HasColumnName("MobilePIN")
                    .HasMaxLength(16);

                entity.Property(e => e.PasswordAnswer).HasMaxLength(128);

                entity.Property(e => e.PasswordQuestion).HasMaxLength(256);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<VwAspnetProfiles>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_aspnet_Profiles");

                entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<VwAspnetRoles>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_aspnet_Roles");

                entity.Property(e => e.Description).HasMaxLength(256);

                entity.Property(e => e.LoweredRoleName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<VwAspnetUsers>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_aspnet_Users");

                entity.Property(e => e.LastActivityDate).HasColumnType("datetime");

                entity.Property(e => e.LoweredUserName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.MobileAlias).HasMaxLength(16);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<VwAspnetUsersInRoles>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_aspnet_UsersInRoles");
            });

            modelBuilder.Entity<VwAspnetWebPartStatePaths>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_aspnet_WebPartState_Paths");

                entity.Property(e => e.LoweredPath)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<VwAspnetWebPartStateShared>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_aspnet_WebPartState_Shared");

                entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<VwAspnetWebPartStateUser>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_aspnet_WebPartState_User");

                entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<VwCampaign2015>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_Campaign2015");

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");
            });

            modelBuilder.Entity<VwCampaign2015Vo>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_Campaign2015_VO");

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");
            });

            modelBuilder.Entity<VwCampaign2015withApps>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_Campaign2015withApps");

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");
            });

            modelBuilder.Entity<VwCampaign2015withAppsVo>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_Campaign2015withApps_VO");

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");
            });

            modelBuilder.Entity<VwCampaign2016>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_Campaign2016");

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");
            });

            modelBuilder.Entity<VwCampaign2016withApps>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_Campaign2016withApps");

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");
            });

            modelBuilder.Entity<VwCampaign2017withApps>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_Campaign2017withApps");

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");
            });

            modelBuilder.Entity<VwCampaign2018withApps>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_Campaign2018withApps");

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");
            });

            modelBuilder.Entity<VwCampaign2019withApps>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_Campaign2019withApps");

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");
            });

            modelBuilder.Entity<VwCompain2014>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_Compain2014");

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");
            });

            modelBuilder.Entity<VwCompain2014Vo>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_Compain2014_VO");

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");
            });

            modelBuilder.Entity<VwCompain2014withApps>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_Compain2014withApps");

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");
            });

            modelBuilder.Entity<VwCompain2014withAppsVo>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_Compain2014withApps_VO");

                entity.Property(e => e.CampaignId).HasColumnName("CampaignID");
            });

            modelBuilder.Entity<WithoutBdate>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Without_Bdate");

                entity.Property(e => e.BDate)
                    .HasColumnName("b_date")
                    .HasColumnType("date");

                entity.Property(e => e.Fio)
                    .IsRequired()
                    .HasColumnName("FIO")
                    .HasMaxLength(41)
                    .IsUnicode(false);

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<WithoutMiddle>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Without_Middle");

                entity.Property(e => e.BDate)
                    .HasColumnName("b_date")
                    .HasColumnType("date");

                entity.Property(e => e.Fio)
                    .IsRequired()
                    .HasColumnName("FIO")
                    .HasMaxLength(41)
                    .IsUnicode(false);

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
