using GVUZ.DAL.Dapper.Repository.Interfaces.AutoOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GVUZ.DAL.Dapper.ViewModel.AutoOrder;
using Dapper;

namespace GVUZ.DAL.Dapper.Repository.Model.AutoOrder
{
    public class AutoOrderRepository : GvuzRepository, IAutoOrderRepository
    {
        public List<AutoOrderCheckBoxModel> Load(int institutionId)
        {
            var sql = @"Select Id, IsAgreed From AutoOrderAgreement Where InstitutionID = @InstitutionID";

            return DbConnection(db =>
            {
                return db.Query<AutoOrderCheckBoxModel>(sql: sql, param: new { InstitutionID = institutionId }).ToList();
            });
        }

        public void Save(AutoOrderCheckBoxModel model, int institutionId)
        {
            var sql = @"
--Declare @Id int = 1;
--Declare @IsAgreed bit = 1;
--Declare @InstitutionId int = 186;

MERGE AutoOrderAgreement as Target
USING 
    (Select @Id as ID, @IsAgreed as IsAgreed, @InstitutionID as InstitutionID)
AS SOURCE ON (Target.Id = Source.Id AND Target.InstitutionID = Source.InstitutionID)
When Matched Then
Update SET
    Target.IsAgreed = Source.IsAgreed
WHEN NOT MATCHED BY TARGET THEN 
    INSERT(Id, IsAgreed, InstitutionID)
    Values (Source.Id, Source.IsAgreed, Source.InstitutionID);
";

            DbConnection(db =>
            {
                return db.Query<AutoOrderCheckBoxModel>(sql: sql, param: new {Id = model.Id, IsAgreed = model.IsAgreed, InstitutionID = institutionId }).ToList();
            });
        }
    }
}
