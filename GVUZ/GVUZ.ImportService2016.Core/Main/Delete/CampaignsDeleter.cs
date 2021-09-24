using GVUZ.ImportService2016.Core.Main.Conflicts;
using GVUZ.ImportService2016.Core.Main.Dictionaries;
using GVUZ.ImportService2016.Core.Dto.DataReaders;
using GVUZ.ServiceModel.Import.Core.Operations.Conflicts;
using GVUZ.ServiceModel.Import.WebService.Dto;
using GVUZ.ServiceModel.Import.WebService.Dto.Result.Import;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using GVUZ.ImportService2016.Core.Main.Extensions;

namespace GVUZ.ImportService2016.Core.Main.Delete
{
    public class CampaignsDeleter : BaseDeleter
    {
        public CampaignsDeleter(GVUZ.ImportService2016.Core.Dto.Delete.DataForDelete dataForDelete, VocabularyStorage vocabularyStorage, DeleteConflictStorage importConflictStorage, bool deleteBulk) : base(dataForDelete, vocabularyStorage, importConflictStorage, deleteBulk) { }

        List<VocabularyBaseDto> deleteItems = new List<VocabularyBaseDto>();

        protected override void Validate()
        {
            foreach (var campaign in dataForDelete.Campaigns.Items)
            {
                CampaignVocDto dbCampaign = null;
                IEnumerable<CampaignVocDto> campaigns = null;
                if (campaign is uint)
                {
                    int id = Convert.ToInt32(campaign);
                    campaigns = vocabularyStorage.CampaignVoc.Items.Where(x => x.CampaignID == id);
                }
                else
                {
                    // uid
                    campaigns = vocabularyStorage.CampaignVoc.Items.Where(t => t.UID == campaign.ToString());
                }

                if (campaigns.Count() > 1)
                {
                    SetBroken(ConflictMessages.CampaignMoreThenOne, string.Empty,
                       new ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto
                       {
                           Campaigns = new[] { campaign.ToString() },
                       });
                    continue;
                }
                dbCampaign = campaigns.FirstOrDefault();

                if (dbCampaign == null)
                {
                    SetBroken(ConflictMessages.CampaignIsNotFound, string.Empty,
                            new ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto
                            {
                                Campaigns = new[] { campaign.ToString() },
                            });
                    continue;
                }


                var cgs = vocabularyStorage.CompetitiveGroupVoc.Items.Where(t => t.CampaignID == dbCampaign.CampaignID);
                if (cgs.Any())
                {
                    SetBroken(ConflictMessages.CampaignHasDependentCompetitiveGroups, dbCampaign.Name,  new ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto
                            {
                                CompetitiveGroups = cgs.Select(t=> t.CompetitiveGroupID.ToString()).ToArray(),
                            });
                    continue;
                }

                var ias = vocabularyStorage.InstitutionAchievementsVoc.Items.Where(t => t.CampaignID == dbCampaign.CampaignID);
                if (ias.Any())
                {
                    SetBroken(ConflictMessages.CampaignHasDependentInstitutionArchievements, dbCampaign.Name, new ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto
                            {
                                InstitutionAchievements = ias.Select(t => t.UID).ToArray(),
                            });
                    continue;
                }

                // Если есть RecomendedLists (при условии RecomendedListsHistory.DateDelete = NULL) с данным CampaignID - ошибка 4029 (новая, внести в список ошибок!)
                var rl = vocabularyStorage.RecomendedListsVoc.Items.Where(t => t.CampaignID == dbCampaign.CampaignID && t.DateDelete == null);
                if (rl.Any())
                {
                    SetBroken(ConflictMessages.CampaignHasDependentRecomendedLists, dbCampaign.Name, new ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto
                            {
                                RecommendedLists = rl.Select(t => new RecommendedListShort { Stage = t.Stage }).ToArray(),
                            });
                    continue;
                }

                // Если есть OrdersOfAdmission с данным CampaignID - ошибка 4031 (новая, внести в список ошибок!)
                var orders = vocabularyStorage.OrderOfAdmissionVoc.Items.Where(t => t.CampaignID == dbCampaign.CampaignID);
                if (orders.Any())
                {
                    SetBroken(ConflictMessages.CampaignHasDependentOrderOfAdmissions, dbCampaign.Name, new ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto
                            {
                                OrdersOfAdmission = orders.Select(t => new ApplicationShortRef { ApplicationNumber = t.ApplicationNumber, RegistrationDateDate = t.ApplicationRegistrationDate }).ToArray()
                            });
                    continue;
                }

                deleteItems.Add(dbCampaign);
            }
        }

        private void SetBroken(int errorCode, string name, ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto conflictsResultDto)
        {
            var failDto = new CampaignDetailsFailDto();
            if (!string.IsNullOrEmpty(name))
                failDto.Name = name;

            failDto.ErrorInfo = new ErrorInfoImportDto(errorCode, conflictsResultDto);

            conflictStorage.notRemovedCampaigns.Add(failDto);
        }

        protected override void PrepareDataForDelete()
        {
            var deleteReader = new BulkDeleteReader(dataForDelete.ImportPackageId);
            deleteReader.AddRange(deleteItems);

            bulks.Add(new Tuple<string, IDataReader>("bulk_Delete", deleteReader));
        }

    }
}
