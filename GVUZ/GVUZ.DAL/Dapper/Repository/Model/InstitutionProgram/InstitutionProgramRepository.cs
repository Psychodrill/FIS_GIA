using Dapper;
using GVUZ.DAL.Dapper.ViewModel.InstitutionProgram;
using GVUZ.DAL.Dapper.Repository.Interfaces.InstitutionProgram;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

using System.Linq;


namespace GVUZ.DAL.Dapper.Repository.Model.InstitutionProgram
{
    public class InstitutionProgramRepository : GvuzRepository, IInstitutionProgramRepository
    {
        public dynamic GetInstitutionProgram(int institutionId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var q = SQLQuery.GetInstitutionProgram;
                using (var multi = connection.QueryMultiple(q, new {institutionId}))
                {
                    var programs = multi.Read<dynamic>().ToList();
                    var groups = multi.Read<dynamic>().ToList();

                    //для ссылок на конкурсы внутри оп
                    foreach (var p in programs)
                    {
                        p.CompetitiveGroups = new List<dynamic>();
                        foreach (var g in groups.Where(t => t.InstitutionProgramID == p.InstitutionProgramID))
                        { 
                            p.CompetitiveGroups.Add(g);
                        }
                    }
                    return programs;
                }
            }
            //return DbConnection(db =>
            //{
            //    return db.Query<dynamic>(sql: SQLQuery.GetInstitutionProgram, commandType: System.Data.CommandType.Text, param: new { InstitutionID = institutionId } );
            //});
        }

        public List<string> CanDeleteProgram(int institutionID, int? institutionProgramID)
        {
            return DbConnection(db =>
            {
                var errors = new List<string>();
                //как-то тупо это!
                var exists = db.Query<bool?>(sql: SQLQuery.FindInstitutionProgram, commandType: System.Data.CommandType.Text, param: new { institutionID = institutionID, institutionProgramID = institutionProgramID }).Any();
                if (!exists)
                    errors.Add("Объект не найден, возможно, он был удален другим пользователем.");

                var sql = @"                   
                    SELECT ip.name
	                    From CompetitiveGroupToProgram pr  (NOLOCK)
	                    JOIN InstitutionProgram ip (NOLOCK) ON pr.institutionProgramID = ip.InstitutionProgramID
                    WHERE ip.InstitutionID=@InstitutionID AND ip.institutionProgramID = @institutionProgramID;
                ";
                var cgNames = db.Query<string>(sql: sql, commandType: System.Data.CommandType.Text, param: new { institutionID = institutionID, institutionProgramID = institutionProgramID }).ToList();
                cgNames.ForEach(t => errors.Add(string.Format(@"Удаление невозможно: данная программа используется в конкурсе ""{0}"".", t)));

                return errors;
            });
        }

        public bool DeleteProgram(int institutionId, int? institutionProgramId)
        {
            return DbConnection(db =>
            {
                var result = db.Execute(sql: SQLQuery.DeleteInstitutionProgram, commandType: System.Data.CommandType.Text, param: new { institutionProgramId, institutionId });
                return result > 0 ? true : false;
            });
        }

        public dynamic UpdateProgram(int InstitutionID, InstitutionProgramModel data)
        {
            IEnumerable<dynamic> result = null;
            return DbConnection(db =>
            {
                result = db.Query<dynamic>(sql: SQLQuery.UpdateInstitutionProgram,
                param: new
                {
                    data.InstitutionProgramID,
                    data.UID,
                    data.Name,
                    data.Code,
                    InstitutionID
                });

                return result;
            });
        }
    }
}
