using GVUZ.DAL.Dapper.ViewModel.CompetitiveGroups;
using CGM = GVUZ.DAL.Dapper.Model.CompetitiveGroups;
using System.Collections.Generic;
using System.Web.Mvc;

namespace GVUZ.DAL.Dapper.Repository.Interfaces.CompetitiveGroups
{
    public interface ICompetitiveGroupRepository
    {
        IEnumerable<CGM.CompetitiveGroup> GetCompetitiveGroups(int institutionId);

        CompetitiveGroupViewModel GetCompetitiveGroupList(int institutionId);

        Model.CompetitiveGroups.CompetitiveGroupRepository.ValidateCompetitiveGroup DeleteCompetitiveGroup(int competitiveGroupID);

        CompetitiveGroupViewModel FillCompetitiveGroupEditModel(int? competitiveGroupId, int? institutionId, bool IsMultiProfiile = false);
        int UpdateCompetitiveGroup(CompetitiveGroupViewModel model);
        bool ValidateUpdateCompetitiveGroup(CompetitiveGroupViewModel model, ModelStateDictionary modelState);
        dynamic CompetitiveGroupCopy(int[] competitiveGroupIDs, int copy_year, int copy_сampaignType, int copy_levelBudget, int InstitutionID);
    }
}
