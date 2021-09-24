using GVUZ.DAL.Dapper.Model.TargetOrganization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GVUZ.DAL.Dapper.Repository.Interfaces.TargetOrganization
{
    public interface ICompetitiveGroupTargetRepository
    {
        IEnumerable<CompetitiveGroupTarget> GetCompetitiveGroupTarget(int institutionId, string sortKey = null, bool sortDesc = false);
        Task<IEnumerable<CompetitiveGroupTarget>> GetCompetitiveGroupTargetAsync(int institutionId, string sortKey = null, bool sortDesc = false);
        bool DeleteCompetitiveGroupTarget(int institutionId, int competitiveGroupTargetId);
        Task<bool> DeleteCompetitiveGroupTargetAsync(int institutionId, int competitiveGroupTargetId);
        CompetitiveGroupTarget UpdateCompetitiveGroupTarget(CompetitiveGroupTarget model);
        Task<CompetitiveGroupTarget> UpdateCompetitiveGroupTargetAsync(CompetitiveGroupTarget model);
        //bool isFindCompetitiveGroupTarget(int institutionId, int competitiveGroupTargetId);
        //Task<bool> isFindCompetitiveGroupTargetAsync(int institutionId, int competitiveGroupTargetId);
        bool ValidateUpdateCompetitiveGroupTarget(CompetitiveGroupTarget model, ModelStateDictionary errors);
        Task<bool> ValidateUpdateCompetitiveGroupTargetAsync(CompetitiveGroupTarget model, ModelStateDictionary errors);
        List<string> CanDeleteCompetitiveGroupTarget(int institutionID, int competitiveGroupTargetId);
    }
}
