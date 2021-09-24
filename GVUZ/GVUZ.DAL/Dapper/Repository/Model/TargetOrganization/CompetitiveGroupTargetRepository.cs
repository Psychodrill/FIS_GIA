using Dapper;
using GVUZ.DAL.Dapper.Model.TargetOrganization;
using GVUZ.DAL.Dapper.Repository.Interfaces.TargetOrganization;
using GVUZ.DAL.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace GVUZ.DAL.Dapper.Repository.Model.TargetOrganization
{
    public class CompetitiveGroupTargetRepository : GvuzRepository, ICompetitiveGroupTargetRepository
    {
        public IEnumerable<CompetitiveGroupTarget> GetCompetitiveGroupTarget(int institutionId, string sortKey = null, bool sortDesc = false)
        {
            return DbConnection(db =>
            {
                return db.Query<CompetitiveGroupTarget>(sql: SQLQuery.GetCompetitiveGroupTargets, commandType: System.Data.CommandType.Text, param: new { InstitutionID = institutionId } );
            });

        }
        public async Task<IEnumerable<CompetitiveGroupTarget>> GetCompetitiveGroupTargetAsync(int institutionId, string sortKey = null, bool sortDesc = false)
        {
            return await DbConnectionAsync(async db =>
            {
                return await db.QueryAsync<CompetitiveGroupTarget>(sql: SQLQuery.GetCompetitiveGroupTargets, commandType: System.Data.CommandType.Text, param: new { InstitutionID = institutionId });
            });
        }
        public bool DeleteCompetitiveGroupTarget(int institutionId, int competitiveGroupTargetId)
        {
            return DbConnection(db =>
            {


                var result = db.Execute(sql: SQLQuery.DeleteCompetitiveGroupTarget, commandType: System.Data.CommandType.Text, param: new { competitiveGroupTargetId, institutionId });
                return result > 0 ? true : false;
            });
        }
        public async Task<bool> DeleteCompetitiveGroupTargetAsync(int institutionId, int competitiveGroupTargetId)
        {
            return await DbConnectionAsync(async db =>
            {
                var result = await db.ExecuteAsync(sql: SQLQuery.DeleteCompetitiveGroupTarget, commandType: System.Data.CommandType.Text, param: new { competitiveGroupTargetId, institutionId });
                return result > 0 ? true : false;
            });
        }
        public CompetitiveGroupTarget UpdateCompetitiveGroupTarget(CompetitiveGroupTarget model)
        {
            return DbConnection(db =>
            {
                var result = db.Query<CompetitiveGroupTarget>(sql: SQLQuery.UpdateCompetitiveGroupTarget,
                    param: new
                    {
                        model.CompetitiveGroupTargetID,
                        model.CreatedDate,
                        model.ModifiedDate,
                        model.UID,
                        model.ContractOrganizationName,
                        model.InstitutionID,
                        model.HaveContract,
                        model.ContractNumber,
                        model.ContractDate,
                        model.ContractOrganizationOGRN,
                        model.ContractOrganizationKPP,
                        model.EmployerOrganizationName,
                        model.EmployerOrganizationOGRN,
                        model.EmployerOrganizationKPP,
                        model.LocationEmployerOrganizations
                    });
                return result.First();
            });
        }
        public async Task<CompetitiveGroupTarget> UpdateCompetitiveGroupTargetAsync(CompetitiveGroupTarget model)
        {
            return await DbConnectionAsync(async db =>
            {
                var result = await db.QueryAsync<CompetitiveGroupTarget>(sql: SQLQuery.UpdateCompetitiveGroupTarget,
                    param: new
                    {
                        model.CompetitiveGroupTargetID,
                        model.CreatedDate,
                        model.ModifiedDate,
                        model.UID,
                        model.ContractOrganizationName,
                        model.InstitutionID,
                        model.HaveContract,
                        model.ContractNumber,
                        model.ContractDate,
                        model.ContractOrganizationOGRN,
                        model.ContractOrganizationKPP,
                        model.EmployerOrganizationName,
                        model.EmployerOrganizationOGRN,
                        model.EmployerOrganizationKPP,
                        model.LocationEmployerOrganizations
                    });
                return result.First();
            });
        }
        public bool IsCompetitiveGroupTargetExists(int institutionId, int competitiveGroupTargetId)
        {
            return DbConnection(db =>
            {
                return db.Query<CompetitiveGroupTarget>(sql: SQLQuery.FindCompetitiveGroupTarget, commandType: System.Data.CommandType.Text, param: new { InstitutionID = institutionId, CompetitiveGroupTargetID = competitiveGroupTargetId }).Any();
            });
        }
        //public async Task<bool> isFindCompetitiveGroupTargetAsync(int institutionId, int competitiveGroupTargetId)
        //{
        //    return await DbConnectionAsync(async db =>
        //    {
        //        var result = await db.QueryAsync<CompetitiveGroupTarget>(sql: SQLQuery.FindCompetitiveGroupTarget, commandType: System.Data.CommandType.Text, param: new { InstitutionID = institutionId, CompetitiveGroupTargetID = competitiveGroupTargetId });
        //        return result.Any();
        //    });
        //}
        public bool ValidateUpdateCompetitiveGroupTarget(CompetitiveGroupTarget model, ModelStateDictionary errors)
        {
            //if (string.IsNullOrEmpty(model.UID))
            //{
            //    return true;
            //}
            var CompetitiveGroupTarget = DbConnection(db =>
            {
                return db.Query<CompetitiveGroupTarget>(sql: SQLQuery.GetCompetitiveGroupTargets, commandType: System.Data.CommandType.Text, param: new { InstitutionID = model.InstitutionID });
            });
            if (!string.IsNullOrEmpty(model.UID) &&
                CompetitiveGroupTarget.Any(x => x.UID != null && x.UID.Equals(model.UID, StringComparison.OrdinalIgnoreCase) && x.CompetitiveGroupTargetID != model.CompetitiveGroupTargetID && x.InstitutionID == model.InstitutionID))
            {
                errors.AddModelError("UID", "Значение в поле UID должно быть уникальным среди всех целевых организаций!");
            }
            //if (CompetitiveGroupTarget.Any(x => x.ContractOrganizationName != null && x.CompetitiveGroupTargetID != model.CompetitiveGroupTargetID && 
            //x.ContractOrganizationName.Equals(model.ContractOrganizationName, StringComparison.OrdinalIgnoreCase)))
            //{
            //    errors.AddModelError("ContractOrganizationName", "Организация с таким наименованием уже существует");
            //}


            return errors.IsValid;
        }
        public async Task<bool> ValidateUpdateCompetitiveGroupTargetAsync(CompetitiveGroupTarget model, ModelStateDictionary errors)
        {
            if (string.IsNullOrEmpty(model.UID))
            {
                return true;
            }
            var CompetitiveGroupTarget = await DbConnectionAsync(async db =>
            {
                return await db.QueryAsync<CompetitiveGroupTarget>(sql: SQLQuery.GetCompetitiveGroupTargets, commandType: System.Data.CommandType.Text, param: new { InstitutionID = model.InstitutionID });
            });
            if (CompetitiveGroupTarget.Any(x => x.UID.Equals(model.UID, StringComparison.OrdinalIgnoreCase) && x.CompetitiveGroupTargetID != model.CompetitiveGroupTargetID && x.InstitutionID == model.InstitutionID))
            {
                errors.AddModelError("UID", "Значение в поле UID должно быть уникальным среди всех целевых организаций!");
            }
            if (CompetitiveGroupTarget.Any(x => x.ContractOrganizationName.Equals(model.ContractOrganizationName, StringComparison.OrdinalIgnoreCase)))
            {
                errors.AddModelError("ContractOrganizationName", "Организация с таким именем уже существует");
            }
            return errors.IsValid;
        }


        public List<string> CanDeleteCompetitiveGroupTarget(int institutionID, int competitiveGroupTargetId)
        {
            return DbConnection(db =>
            {
                var errors = new List<string>();
                var exists = db.Query<CompetitiveGroupTarget>(sql: SQLQuery.FindCompetitiveGroupTarget, commandType: System.Data.CommandType.Text, param: new { InstitutionID = institutionID, CompetitiveGroupTargetID = competitiveGroupTargetId }).Any();
                if (!exists)
                    errors.Add("Объект не найден, возможно, он был удален другим пользователем.");

                var sql = @"
                    --declare @CompetitiveGroupTargetID int = 41;
                    select cg.Name
                    From CompetitiveGroup cg (NOLOCK)
                    inner join CompetitiveGroupTargetItem cgti (NOLOCK) on cgti.CompetitiveGroupID = cg.CompetitiveGroupID
                    Where cgti.CompetitiveGroupTargetID = @CompetitiveGroupTargetID;
                ";
                var cgNames = db.Query<string>(sql: sql, commandType: System.Data.CommandType.Text, param: new { CompetitiveGroupTargetID = competitiveGroupTargetId }).ToList();
                cgNames.ForEach(t => errors.Add(string.Format(@"Удаление невозможно: данная ЦО используется в конкурсе ""{0}"".", t)));

                return errors;
            });
        }
    }
}
