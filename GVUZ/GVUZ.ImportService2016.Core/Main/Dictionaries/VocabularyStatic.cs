using GVUZ.ImportService2016.Core.Main.Dictionaries.Campaign;
using GVUZ.ImportService2016.Core.Main.Dictionaries.Direction;
using GVUZ.ImportService2016.Core.Main.Dictionaries.EntranceTest;
using GVUZ.ImportService2016.Core.Main.Dictionaries.Olympic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries
{
    public static class VocabularyStatic
    {
        public static BenefitVoc BenefitVoc { get; set; }
        public static DisabilityTypeVoc DisabilityTypeVoc { get; set; }
        public static SubjectVoc SubjectVoc { get; set; }
        public static InstitutionDocumentTypeVoc InstitutionDocumentTypeVoc { get; set; }
        public static EntranceTestTypeVoc EntranceTestTypeVoc { get; set; }
        public static EntranceTestResultSourceVoc EntranceTestResultSourceVoc { get; set; }
        public static CountryTypeVoc CountryTypeVoc { get; set; }
        public static IdentityDocumentTypeVoc IdentityDocumentTypeVoc { get; set; }
        public static ApplicationStatusTypeVoc ApplicationStatusTypeVoc { get; set; }
        public static AdmissionItemTypeVoc AdmissionItemTypeVoc { get; set; }
        public static DocumentTypeVoc DocumentTypeVoc { get; set; }
        public static GlobalMinEgeVoc GlobalMinEgeVoc { get; set; }
        public static LevelBudgetVoc LevelBudgetVoc { get; set; }
        public static SubjectEgeMinValueVoc SubjectEgeMinValueVoc { get; set; }
        public static CampaignStatusVoc CampaignStatusVoc { get; set; }
        public static CampaignTypeVoc CampaignTypeVoc { get; set; }
        public static EduLevelsToCampaignTypesVoc EduLevelsToCampaignTypesVoc { get; set; }
        public static EduLevelDocumentTypeVoc EduLevelDocumentTypeVoc { get; set; }
        public static IndividualAchievementsCategoryVoc IndividualAchievementsCategoryVoc { get; set; }
        public static OlympicProfileVoc OlympicProfileVoc { get; set; }

        public static RegionTypeVoc RegionTypeVoc { get; set; }
        public static TownTypeVoc TownTypeVoc { get; set; }

        public static OrphanCategoryVoc OrphanCategory { get; set; }
        public static CompatriotCategoryVoc CompatriotCategory { get; set; }

        //public static OlympicLevelVoc OlympicLevelVoc { get; set; }

        public static DirectionVoc DirectionVoc { get; set; }
        public static OlympicTypeVoc OlympicTypeVoc { get; set; }
        public static OlympicDiplomTypeVoc OlympicDiplomTypeVoc { get; set; }

        public static EntranceTestCreativeDirectionVoc EntranceTestCreativeDirectionVoc { get; set; }
        public static EntranceTestProfileDirectionVoc EntranceTestProfileDirectionVoc { get; set; }
        public static OlympicTypeProfileVoc OlympicTypeProfileVoc { get; set; }

        public static VeteranCategoryVoc VeteranCategory { get; set; }

        public static OlympicSubjectVoc OlympicSubjectVoc { get; set; }

        public static ParentsLostCategoryVoc ParentsLostCategory { get; set; }
        public static StateEmployeeCategoryVoc StateEmployeeCategory { get; set; }
        public static RadiationWorkCategoryVoc RadiationWorkCategory { get; set; }
        public static ReturnDocumentsTypeVoc ReturnDocumentsType { get; set; }

        static VocabularyStatic() { }

        static DataSet _dataSet = null;
        public static void Init(DataSet dataSet)
        {
            _dataSet = dataSet;

            BenefitVoc = new BenefitVoc(getTable(0));
            DisabilityTypeVoc = new DisabilityTypeVoc(getTable(1));
            SubjectVoc = new SubjectVoc(getTable(2));
            InstitutionDocumentTypeVoc = new InstitutionDocumentTypeVoc(getTable(3));
            EntranceTestTypeVoc = new EntranceTestTypeVoc(getTable(4));
            EntranceTestResultSourceVoc = new EntranceTestResultSourceVoc(getTable(5));
            CountryTypeVoc = new CountryTypeVoc(getTable(6));
            IdentityDocumentTypeVoc = new IdentityDocumentTypeVoc(getTable(7));
            ApplicationStatusTypeVoc = new ApplicationStatusTypeVoc(getTable(8));
            AdmissionItemTypeVoc = new AdmissionItemTypeVoc(getTable(9));
            DocumentTypeVoc = new DocumentTypeVoc(getTable(10));
            GlobalMinEgeVoc = new GlobalMinEgeVoc(getTable(11));
            LevelBudgetVoc = new LevelBudgetVoc(getTable(12));
            SubjectEgeMinValueVoc = new SubjectEgeMinValueVoc(getTable(13));
            CampaignStatusVoc = new CampaignStatusVoc(getTable(14));
            CampaignTypeVoc = new CampaignTypeVoc(getTable(15));
            EduLevelsToCampaignTypesVoc = new EduLevelsToCampaignTypesVoc(getTable(16));
            EduLevelDocumentTypeVoc = new EduLevelDocumentTypeVoc(getTable(17));
            IndividualAchievementsCategoryVoc = new IndividualAchievementsCategoryVoc(getTable(18));
            OlympicProfileVoc = new OlympicProfileVoc(getTable(19));

            RegionTypeVoc = new RegionTypeVoc(getTable(20));
            TownTypeVoc = new TownTypeVoc(getTable(21));
            OrphanCategory= new OrphanCategoryVoc(getTable(22));
            CompatriotCategory = new CompatriotCategoryVoc(getTable(23));

            //OlympicLevelVoc = new OlympicLevelVoc(getTable(24));

            // 25 - Специальности и Квалификации
            DirectionVoc = new DirectionVoc(getTable(25));

            // 26 - Олимпиады
            OlympicTypeVoc = new OlympicTypeVoc(getTable(26));
            // 27 - Тип диплома олимпиады
            OlympicDiplomTypeVoc = new OlympicDiplomTypeVoc(getTable(27));

            // 28 - Творческая направленность
            EntranceTestCreativeDirectionVoc = new EntranceTestCreativeDirectionVoc(getTable(28));
            // 29 - Профильные направления
            EntranceTestProfileDirectionVoc = new EntranceTestProfileDirectionVoc(getTable(29));

            // 30 - Профили олимпиад
            OlympicTypeProfileVoc = new OlympicTypeProfileVoc(getTable(30));

            VeteranCategory = new VeteranCategoryVoc(getTable(31));

            OlympicSubjectVoc = new OlympicSubjectVoc(getTable(32));

            ParentsLostCategory = new ParentsLostCategoryVoc(getTable(33));
            StateEmployeeCategory = new StateEmployeeCategoryVoc(getTable(34));
            RadiationWorkCategory = new RadiationWorkCategoryVoc(getTable(35));

            //36 - Способ возврата документов абитуриенту
            ReturnDocumentsType = new ReturnDocumentsTypeVoc(getTable(36));
        }

        private static DataTable getTable(int index)
        {
            return _dataSet != null && _dataSet.Tables.Count >= index + 1 ? _dataSet.Tables[index] : null;
        }
    }
}
