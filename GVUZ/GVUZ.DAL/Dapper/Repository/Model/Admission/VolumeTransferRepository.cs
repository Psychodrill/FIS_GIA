using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using GVUZ.DAL.Dapper.Model.AdmissionVolumes;
using GVUZ.DAL.Dapper.Model.Directions;
using GVUZ.DAL.Dapper.Model.DistributedAdmissionVolumes;
using GVUZ.DAL.Dapper.Model.ParentDirections;
using GVUZ.DAL.Dapper.Repository.Interfaces.Admission;
using GVUZ.DAL.Dapper.ViewModel.Dictionary;
using GDDMC = GVUZ.DAL.Dapper.Model.Campaigns;
using GVUZ.DAL.Dto;
using System.Data.SqlClient;


namespace GVUZ.DAL.Dapper.Repository.Model.Admission
{
    //Переброс мест
    public class VolumeTransferRepository : GvuzRepository, IVolumeTransferRepository
    {
        public bool CheckIfTransferAllowed(int campaignId, int institutionId)
        {
            bool result;
            return DbConnection(db =>
            {
                result = db.Query<bool>(sql: SQLQuery.PlanAdmissionVolumeByCampaign, param:
                   new { campaignId = campaignId }).Any();

                return result;
            });
        }

        public bool CheckIfTransferHasBenefit(int campaignId)
        {
            bool result;
            return DbConnection(db =>
            {
                result = db.Query<bool>(sql: SQLQuery.CheckIfTransferHasBenefit, param:
                   new { campaignId = campaignId }).Any();

                return result;
            });
        }

        public IEnumerable<TransferCheckDto> CheckTransferVolume(int campaignId)
        {
            IEnumerable<TransferCheckDto> result;
            return DbConnection(db =>
            {
                result = db.Query<TransferCheckDto>(sql: SQLQuery.CheckTransferVolume, param: new { campaignId = campaignId });
                return result;
            });
        }

        public dynamic VolumeTransferByCampaign(int campaignId, int institutionId)
        {
            dynamic res = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                //res = db.Query<dynamic>(sql: SQLQuery.VolumeTransferByCampaign, param:
                //    new { campaignId = campaignId });

                var q = SQLQuery.VolumeTransferByCampaign;
                using (var multi = connection.QueryMultiple(q, new { campaignId = campaignId, institutionId = institutionId }))
                {
                    res = multi.Read<dynamic>().ToList();
                    var groups = multi.Read<dynamic>().ToList();
                    var programs = multi.Read<dynamic>().ToList();

                    //для ссылок на конкурсы внутри оп
                    foreach (var r in res)
                    {
                        r.CompetitiveGroups = new List<dynamic>();
                        foreach (var g in groups.Where(t => t.DirectionID == r.DirectionID && t.EducationFormId == r.EducationFormId))
                        {
                            r.CompetitiveGroups.Add(g);

                            //добавляем программы
                            g.Programs = new List<dynamic>();
                            foreach (var p in programs.Where(t => t.CompetitiveGroupID == g.CompetitiveGroupID))
                            {
                                g.Programs.Add(p);
                            }
                        }
                    }
                    return res;
                }

            }
        }

        public dynamic BeginVolumeTransfer(int[] competitiveGroupIDs, int campaignId, int InstitutionID)
        {
            var competitiveGroupsTable = new System.Data.DataTable();
            competitiveGroupsTable.Columns.Add("id", typeof(int));

            foreach (var id in competitiveGroupIDs)
            {
                competitiveGroupsTable.Rows.Add(id);
            }
            var competitiveGroups = competitiveGroupsTable.AsTableValuedParameter("[Identifiers]");

            dynamic result = null;
            return DbConnection(db =>
            {
                result = db.Query<dynamic>(sql: SQLQuery.BeginVolumeTransfer, param: new { competitiveGroups = competitiveGroups, campaignId = campaignId, institutionID = InstitutionID });

                return result;
            });
        }

        public IEnumerable<int> GetCampaignEducationForms(int campaignId)
        {
            return DbConnection(db =>
            {
                int flags = db.Query<int>(sql: SQLQuery.GetCampaignFormFlags, param: new { campaignId = campaignId }).First();

                List<int> result = new List<int>();

                if ((flags & 1) > 0)
                {
                    result.Add(EDFormsConst.O);
                }
                if ((flags & 2) > 0)
                {
                    result.Add(EDFormsConst.OZ);
                }
                if ((flags & 4) > 0)
                {
                    result.Add(EDFormsConst.Z);
                }

                return result;
            });
        }

        public GDDMC.Campaign GetCampaign(int InstitutionId, int campaignId)
        {
            GDDMC.Campaign result;
            return DbConnection(db =>
            {
                result = db.Query<GDDMC.Campaign>(sql: SQLQuery.GetCampaign, param: new { InstitutionID = InstitutionId, CampaignID = campaignId }).First();

                return result;
            });
        }


    }
}
