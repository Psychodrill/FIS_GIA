using GVUZ.DAL.Dapper.Repository.Interfaces.Structure;
using GVUZ.DAL.Dto;
using Dapper;
using System.Linq;
using System;
using System.Collections.Generic;

namespace GVUZ.DAL.Dapper.Repository.Model.Structure
{
    public class StructuresRepository : GvuzRepository, IStructureRepository
    {
        public List<StructureInfoDto> GetStructure(int institutionId, string userName)
        {
            var sql = SQLQuery.GetInstitutionStructureDto;

            return DbConnection(db =>
            {
                return db.Query<StructureInfoDto>(sql: sql, param: new { institutionId = institutionId, userName = userName }).ToList();
            });
        }
    }
}
