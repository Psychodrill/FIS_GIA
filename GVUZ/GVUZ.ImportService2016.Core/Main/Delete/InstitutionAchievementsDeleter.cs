using GVUZ.ImportService2016.Core.Main.Conflicts;
using GVUZ.ImportService2016.Core.Main.Dictionaries;
using GVUZ.ImportService2016.Core.Dto.DataReaders;
using GVUZ.ServiceModel.Import.Core.Operations.Conflicts;
using GVUZ.ServiceModel.Import.WebService.Dto.Result.Import;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Delete
{
    public class InstitutionAchievementsDeleter : BaseDeleter
    {
        public InstitutionAchievementsDeleter(GVUZ.ImportService2016.Core.Dto.Delete.DataForDelete dataForDelete, VocabularyStorage vocabularyStorage, DeleteConflictStorage importConflictStorage, bool deleteBulk) : base(dataForDelete, vocabularyStorage, importConflictStorage, deleteBulk) { }

        List<VocabularyBaseDto> deleteItems = new List<VocabularyBaseDto>();

        protected override void Validate()
        {
            foreach (var uid in dataForDelete.InstitutionAchievements)
            {
                var ia = vocabularyStorage.InstitutionAchievementsVoc.Items.Where(x => x.UID == uid).FirstOrDefault();
                if (ia == null)
                {
                    SetBroken(ConflictMessages.InstitutionAchievementIsNotFound, uid, string.Empty,
                            new ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto
                            {
                                InstitutionAchievements = new[] { uid },
                            });
                    continue;
                }

                var indAch = vocabularyStorage.IndividualAchivementVoc.Items.Where(t => t.IdAchievement == ia.IdAchievement);
                if (indAch.Any())
                {
                    var apps = vocabularyStorage.ApplicationVoc.Items.Where(t=> indAch.Any(i => i.ApplicationID ==  t.ApplicationID));

                    SetBroken(ConflictMessages.InstitutionAchievementCannotBeRemovedWithApplications, uid, ia.Name,
                            new ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto
                            {
                                Applications = apps.Select(t => new ServiceModel.Import.WebService.Dto.ApplicationShortRef { ApplicationNumber = t.ApplicationNumber, RegistrationDateDate = t.RegistrationDate }).ToArray()
                            });
                    continue;
                }


                deleteItems.Add(ia);
            }
        }

        private void SetBroken(int errorCode, string uid, string name, ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto conflictsResultDto)
        {
            var failDto = new InstitutionAchievementFailDetailsDto();
            if (!string.IsNullOrEmpty(uid))
                failDto.IAUID = uid;
            if (!string.IsNullOrEmpty(name))
                failDto.Name = name;

            failDto.ErrorInfo = new ErrorInfoImportDto(errorCode, conflictsResultDto);

            conflictStorage.notRemovedInstitutionAchievements.Add(failDto);
        }

        protected override void PrepareDataForDelete()
        {
            var deleteReader = new BulkDeleteReader(dataForDelete.ImportPackageId);
            deleteReader.AddRange(deleteItems);

            bulks.Add(new Tuple<string, IDataReader>("bulk_Delete", deleteReader));
        }
    }
}