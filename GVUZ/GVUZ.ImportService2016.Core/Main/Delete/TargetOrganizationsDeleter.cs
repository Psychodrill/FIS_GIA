using GVUZ.ImportService2016.Core.Dto.DataReaders;
using GVUZ.ImportService2016.Core.Main.Conflicts;
using GVUZ.ImportService2016.Core.Main.Dictionaries;
using GVUZ.ServiceModel.Import.Core.Operations.Conflicts;
using GVUZ.ServiceModel.Import.WebService.Dto;
using GVUZ.ServiceModel.Import.WebService.Dto.Result.Import;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace GVUZ.ImportService2016.Core.Main.Delete
{
    public class TargetOrganizationsDeleter : BaseDeleter
    {
        public TargetOrganizationsDeleter(GVUZ.ImportService2016.Core.Dto.Delete.DataForDelete dataForDelete, VocabularyStorage vocabularyStorage, DeleteConflictStorage importConflictStorage, bool deleteBulk) : base(dataForDelete, vocabularyStorage, importConflictStorage, deleteBulk) { }

        List<VocabularyBaseDto> deleteItems = new List<VocabularyBaseDto>();

        protected override void Validate()
        {
            foreach (var targetUID in dataForDelete.TargetOrganizations)
            {
                CompetitiveGroupTargetVocDto dbTarget = null;
                IEnumerable<CompetitiveGroupTargetVocDto> targets = null;

                targets = vocabularyStorage.CompetitiveGroupTargetVoc.Items.Where(t => t.UID == targetUID.ToString());

                if (targets.Count() > 1)
                {
                    SetBroken(ConflictMessages.TargetOrganizationMoreThenOne, targetUID, string.Empty,
                       new ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto
                       {
                           TargetOrganizations = new[] { targetUID.ToString() },
                       });
                    continue;
                }

                dbTarget = targets.FirstOrDefault();
                if (dbTarget == null)
                {
                    SetBroken(ConflictMessages.TargetOrganizationIsNotFound, targetUID, string.Empty,
                            new ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto
                            {
                                TargetOrganizations = new[] { targetUID.ToString() },
                            });
                    continue;
                }

                // Внешние зависимости:
                // CompetitiveGroupTargetItem.CompetitiveGroupTargetID
                var cgti = vocabularyStorage.CompetitiveGroupTargetItemVoc.Items.Where(t => t.CompetitiveGroupTargetID == dbTarget.ID);
                if (cgti.Any())
                {
                    SetBroken(ConflictMessages.CompetitiveGroupTargetHasDependentCompetitiveGroups, targetUID, dbTarget.Name, new ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto
                    {
                        CompetitiveGroups = cgti.Select(t => t.CompetitiveGroupID.ToString()).Distinct().ToArray()
                    });
                    continue;
                }

                // ApplicationCompetitiveGroupItem.CompetitiveGroupTargetId

                var acgi = vocabularyStorage.ApplicationCompetitiveGroupItemVoc.Items.Where(t => t.CompetitiveGroupTargetId == dbTarget.ID);
                if (acgi.Any())
                {
                    SetBroken(ConflictMessages.CompetitiveGroupTargetHasDependentApplications, targetUID, dbTarget.Name, new ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto
                    {
                        // Applications = acgi.Select(t => new ApplicationShortRef {   t.ApplicationId.ToString()).ToArray(),
                    });
                    continue;
                }

                deleteItems.Add(dbTarget);
            }
        }

        private void SetBroken(int errorCode, string uid, string name, ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto conflictsResultDto)
        {
            var failDto = new TargetOrganizationFailDetailsDto();
            failDto.TargetOrganizationName = name;
            failDto.UID = uid;

            failDto.ErrorInfo = new ErrorInfoImportDto(errorCode, conflictsResultDto);
            conflictStorage.notRemovedTargetOrganizations.Add(failDto);
        }

        protected override void PrepareDataForDelete()
        {
            var deleteReader = new BulkDeleteReader(dataForDelete.ImportPackageId);
            deleteReader.AddRange(deleteItems);

            bulks.Add(new Tuple<string, IDataReader>("bulk_Delete", deleteReader));
        }

    }
}
