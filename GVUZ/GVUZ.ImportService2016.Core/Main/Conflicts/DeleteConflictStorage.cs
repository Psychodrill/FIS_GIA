using GVUZ.ImportService2016.Core.Main.Dictionaries;
using GVUZ.ServiceModel.Import.Core.Operations.Conflicts;
using GVUZ.ServiceModel.Import.Package;
using GVUZ.ServiceModel.Import.WebService.Dto.Result.Import;
//using GVUZ.ServiceModel.Import.WebService.Dto.Result.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Conflicts
{
    public class DeleteConflictStorage
    {
        private VocabularyStorage vocabularyStorage;
        public DeleteConflictStorage(VocabularyStorage _vocStorage)
        {
            vocabularyStorage = _vocStorage;
        }

        public int removedAppCount = 0;
        public readonly List<ApplicationFailDetailsDto> notRemovedApps = new List<ApplicationFailDetailsDto>();

        public int removedCompetitiveGroupsCount;
        public readonly List<CompetitiveGroupFailDetailsDto> notRemovedCompetitiveGroups = new List<CompetitiveGroupFailDetailsDto>();

        public int removedCompetitiveGroupItemsCount;
        public readonly List<CompetitiveGroupItemFailDetailsDto> notRemovedCompetitiveGroupItems = new List<CompetitiveGroupItemFailDetailsDto>();

        public int removedInstitutionAchievementsCount = 0;
        public readonly List<InstitutionAchievementFailDetailsDto> notRemovedInstitutionAchievements = new List<InstitutionAchievementFailDetailsDto>();

        public int removedOrderApplicationsCount = 0;
        public readonly List<OrderApplicationFailDetailsDto> notExcludedApps = new List<OrderApplicationFailDetailsDto>();
        public int removedOrdersCount = 0;
        public readonly List<OrderFailDetailsDto> notRemovedOrders = new List<OrderFailDetailsDto>();

        public int removedCampaignsCount = 0;
        public readonly List<CampaignDetailsFailDto> notRemovedCampaigns = new List<CampaignDetailsFailDto>();

        public readonly List<CommonBenefitFailDetailsDto> notRemovedCommonBenefits = new List<CommonBenefitFailDetailsDto>();

        public readonly List<EntranceTestItemFailDetailsDto> notRemovedEntranceTests = new List<EntranceTestItemFailDetailsDto>();

        public int removedTargetOrganizationsCount = 0;
        public readonly List<TargetOrganizationFailDetailsDto> notRemovedTargetOrganizations = new List<TargetOrganizationFailDetailsDto>();

        public int removedInstitutionProgramsCount = 0;
        public readonly List<InstitutionProgramFailDetailsDto> notRemovedInstitutionPrograms = new List<InstitutionProgramFailDetailsDto>();

        /// <summary>
        /// Готовим ответ клиенту
        /// </summary>
        public DeleteResultPackage PrepareProcessResultObject()
        {
            var x = new DeleteResultPackage
            {
                Conflicts = null,
                Log = new LogDto
                {
                    Successful = new SuccessfulImportStatisticsDto
                    {
                        Applications = removedAppCount.ToString(),
                        CompetitiveGroups = removedCompetitiveGroupsCount.ToString(),
                        CompetitiveGroupItems = removedCompetitiveGroupItemsCount.ToString(),
                        ordersImported = removedOrdersCount,
                        applicationsInOrdersImported = removedOrderApplicationsCount,
                        campaignsImported = removedCampaignsCount,
                        InstitutionAchievements = removedInstitutionAchievementsCount.ToString(),
                        TargetOrganizations = removedTargetOrganizationsCount.ToString(),
                        InstitutionPrograms = removedInstitutionProgramsCount.ToString(),
                    },
                    Failed = new FailedImportInfoDto
                    {
                        Applications = notRemovedApps.ToArray(),

                        OrdersOfAdmissions = notRemovedOrders.ToArray(),
                        ApplicationsInOrders = notExcludedApps.ToArray(),

                        CompetitiveGroups = notRemovedCompetitiveGroups.ToArray(),
                        CompetitiveGroupItems = notRemovedCompetitiveGroupItems.ToArray(),
                        CommonBenefit = notRemovedCommonBenefits.ToArray(),
                        EntranceTestItems = notRemovedEntranceTests.ToArray(),
                        Campaigns = notRemovedCampaigns.ToArray(),
                        InstitutionAchievements = notRemovedInstitutionAchievements.ToArray(),
                        TargetOrganizations = notRemovedTargetOrganizations.ToArray(),
                        InstitutionPrograms = notRemovedInstitutionPrograms.ToArray(),
                    }
                }
            };
            if (x.Log.Failed.Applications.Length == 0)
                x.Log.Failed.Applications = null;
            if (x.Log.Failed.OrdersOfAdmissions.Length == 0)
                x.Log.Failed.OrdersOfAdmissions = null;
            if (x.Log.Failed.ApplicationsInOrders.Length == 0)
                x.Log.Failed.ApplicationsInOrders = null;
            if (x.Log.Failed.CompetitiveGroups.Length == 0)
                x.Log.Failed.CompetitiveGroups = null;
            if (x.Log.Failed.CompetitiveGroupItems.Length == 0)
                x.Log.Failed.CompetitiveGroupItems = null;
            if (x.Log.Failed.CommonBenefit.Length == 0)
                x.Log.Failed.CommonBenefit = null;
            if (x.Log.Failed.EntranceTestItems.Length == 0)
                x.Log.Failed.EntranceTestItems = null;
            if (x.Log.Failed.Campaigns.Length == 0)
                x.Log.Failed.Campaigns = null;
            if (x.Log.Failed.InstitutionAchievements.Length == 0)
                x.Log.Failed.InstitutionAchievements = null;
            return x;
        }


        public void SetCompetitiveGroupError(int errorCode, int id)
        {
            notRemovedCompetitiveGroups.Add(new CompetitiveGroupFailDetailsDto
            {
                ErrorInfo = new ErrorInfoImportDto(errorCode,  
                    new GVUZ.ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto { CompetitiveGroups = new[] { id.ToString() } })
            });  
        }
    }
}
