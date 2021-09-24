using GVUZ.ImportService2016.Core.Main.Dictionaries.Application;
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
    public class VocabularyStorage
    {
        public string InstitutionName {get; set;}
        public int FounderEsrpOrgId { get; set; }

        public ApplicationVoc ApplicationVoc { get; set; }
        public EntrantVoc EntrantVoc { get; set; }
        public CompetitiveGroupVoc CompetitiveGroupVoc { get; set; }
        public CompetitiveGroupItemVoc CompetitiveGroupItemVoc { get; set; }
        public CompetitiveGroupTargetVoc CompetitiveGroupTargetVoc { get; set; }
        public EntranceTestItemCVoc EntranceTestItemCVoc { get; set; }

        public EntrantDocumentVoc EntrantDocumentVoc { get; set; }

        public CompetitiveGroupTargetItemVoc CompetitiveGroupTargetItemVoc { get; set; }
        
        public AllowedDirectionsVoc AllowedDirectionsVoc { get; set; }
        public CampaignVoc CampaignVoc { get; set; }

        public CampaignEducationLevelVoc CampaignEducationLevelVoc { get; set; }

        

        public BenefitItemCVoc BenefitItemCVoc { get; set; }
        public RecomendedListsVoc RecomendedListsVoc { get; set; }

        public InstitutionAchievementsVoc InstitutionAchievementsVoc { get; set; }
        public AdmissionVolumeVoc AdmissionVolumeVoc { get; set; }
        public OrderOfAdmissionVoc OrderOfAdmissionVoc { get; set; }
        
        public ApplicationEntranceTestDocumentVoc ApplicationEntranceTestDocumentVoc { get; set; }
        public ApplicationEntrantDocumentVoc ApplicationEntrantDocumentVoc { get; set; }
        public IndividualAchivementVoc IndividualAchivementVoc { get; set; }

        public DistributedAdmissionVolumeVoc DistributedAdmissionVolumeVoc { get; set; }

        public InstitutionProgramVoc InstitutionProgramVoc { get; set; }
        
        public ApplicationCompetitiveGroupItemVoc ApplicationCompetitiveGroupItemVoc { get; set; }

        public CompetitiveGroupProgramVoc CompetitiveGroupProgramVoc { get; set; }

        public PlanAdmissionVolumeVoc PlanAdmissionVolumeVoc { get; set; }

        public DistributedPlanAdmissionVolumeVoc DistributedPlanAdmissionVolumeVoc { get; set; }

        public VocabularyStorage() { } // для корректной работы в рамках PackageData

        public VocabularyStorage(DataSet dataSet)
        {
            // 0 - был сам ImportPackage

            // 1 - Application
            ApplicationVoc = new ApplicationVoc(getTable(dataSet, 1));
            // 2 - Абитуриенты данного ОУ
            EntrantVoc = new EntrantVoc(getTable(dataSet, 2));

            // 3-5 - Конкурсы + Целевые организации
            CompetitiveGroupVoc = new CompetitiveGroupVoc(getTable(dataSet, 3));
            CompetitiveGroupItemVoc = new CompetitiveGroupItemVoc(getTable(dataSet, 4));
            CompetitiveGroupTargetVoc = new CompetitiveGroupTargetVoc(getTable(dataSet, 5));

            // 6 - Результаты вступительных испытаний по данному ОУ
            EntranceTestItemCVoc = new EntranceTestItemCVoc(getTable(dataSet, 6));

            // 7 - InstitutiionName - нужно название текущего ОУ для импорта
            var institutionData = getInstitutionData(getTable(dataSet, 7));
            InstitutionName = institutionData.Item1;
            FounderEsrpOrgId = institutionData.Item2;

            // 8 - Возможно, самый тормознутый кусок - список всех документов абитуриентов этого ОУ
            EntrantDocumentVoc = new EntrantDocumentVoc(getTable(dataSet, 8));

            // 9 - CompetitiveGroupTargetItem
            CompetitiveGroupTargetItemVoc = new CompetitiveGroupTargetItemVoc(getTable(dataSet, 9));

            // 10 - доступные направления подготовки 
            AllowedDirectionsVoc = new AllowedDirectionsVoc(getTable(dataSet, 10));
            
            // 11 - ПК
            CampaignVoc = new CampaignVoc(getTable(dataSet, 11));
            // 12 - ПК уровни образования
            CampaignEducationLevelVoc = new CampaignEducationLevelVoc(getTable(dataSet, 12));

            // 13 - Льготы
            BenefitItemCVoc = new BenefitItemCVoc(getTable(dataSet, 13));

            // 14 - RecomendedLists + History
            RecomendedListsVoc = new RecomendedListsVoc(getTable(dataSet, 14));
            
            // 15 InstitutionAchievements
            InstitutionAchievementsVoc = new InstitutionAchievementsVoc(getTable(dataSet, 15));

            // 16 AdmissionVolume
            AdmissionVolumeVoc = new AdmissionVolumeVoc(getTable(dataSet, 16));
            
            // 17 OrderOfAdmission
            OrderOfAdmissionVoc = new OrderOfAdmissionVoc(getTable(dataSet, 17));

            // 18 - ApplicationEntranceTestDocument
            ApplicationEntranceTestDocumentVoc = new ApplicationEntranceTestDocumentVoc(getTable(dataSet, 18));
            // 19 - ApplicationEntrantDocument
            ApplicationEntrantDocumentVoc = new ApplicationEntrantDocumentVoc(getTable(dataSet, 19));
            
            // 20 - Индивидуальные достижения
            IndividualAchivementVoc = new IndividualAchivementVoc(getTable(dataSet, 20));

            // 21 - Распределенный объем приема
            DistributedAdmissionVolumeVoc = new DistributedAdmissionVolumeVoc(getTable(dataSet, 21));

            // 22 - Программы обучения ОО
            InstitutionProgramVoc = new InstitutionProgramVoc(getTable(dataSet, 22));

            // 23 - ACGI для приказов
            ApplicationCompetitiveGroupItemVoc = new ApplicationCompetitiveGroupItemVoc(getTable(dataSet, 23));

            // 24 - Программы обучения конкурса
            CompetitiveGroupProgramVoc = new CompetitiveGroupProgramVoc(getTable(dataSet, 24));

            // 25 - План PlanAdmissionVolume, 26 - DistributedPlanAdmissionVolume
            PlanAdmissionVolumeVoc = new PlanAdmissionVolumeVoc(getTable(dataSet, 25));
            DistributedPlanAdmissionVolumeVoc = new DistributedPlanAdmissionVolumeVoc(getTable(dataSet, 26));
        }

        private DataTable getTable(DataSet dataSet, int index)
        {
            return dataSet.Tables.Count >= index + 1 ? dataSet.Tables[index] : null;
        }

        private Tuple<string, int> getInstitutionData(DataTable dt)
        {
            if (dt == null || dt.Rows.Count==0)
                return new Tuple<string, int>(string.Empty, 0);
            return new Tuple<string, int>(dt.Rows[0][0].ToString(), (int)dt.Rows[0][1]);
        }
    }
}
