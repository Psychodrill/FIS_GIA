using GVUZ.ImportService2016.Core.Main.Conflicts;
using GVUZ.ImportService2016.Core.Main.Dictionaries;
using GVUZ.ImportService2016.Core.Dto.DataReaders;
using GVUZ.ServiceModel.Import.Core.Operations.Conflicts;
using GVUZ.ServiceModel.Import.WebService.Dto.Result.Import;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using GVUZ.ServiceModel.Import;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ImportService2016.Core.Main.Delete
{
    public class ApplicationsDeleter : BaseDeleter
    {
        public ApplicationsDeleter(GVUZ.ImportService2016.Core.Dto.Delete.DataForDelete dataForDelete, VocabularyStorage vocabularyStorage, DeleteConflictStorage importConflictStorage, bool deleteBulk) : base(dataForDelete, vocabularyStorage, importConflictStorage, deleteBulk) { }

        List<VocabularyBaseDto> deleteItems = new List<VocabularyBaseDto>();

        protected override void Validate()
        {
            foreach (var application in dataForDelete.Applications)
            {
                var dbApplications = vocabularyStorage.ApplicationVoc.Items.Where(x => x.ApplicationNumber == application.ApplicationNumber && x.RegistrationDate.Date == application.RegistrationDate.Date); 
                if (dbApplications.Count() != 1)
                {
                    SetBroken(ConflictMessages.ApplicationIsNotFound,  application.ApplicationNumber, application.RegistrationDate.GetDateTimeAsString() // .ToShortDateString()
                            , new ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto
                            {
                            });
                    continue;
                }
                var dbApplication = dbApplications.First();
                if (dbApplication.StatusID == GVUZ.ServiceModel.Import.ApplicationStatusType.InOrder || dbApplication.OrderOfAdmissionID != 0
                    || vocabularyStorage.ApplicationCompetitiveGroupItemVoc.Items.Any(t => t.ApplicationId == dbApplication.ApplicationID && (t.OrderOfAdmissionID != 0 || t.OrderOfExceptionID != 0 ) )
                    )
                {
                    SetBroken(ConflictMessages.ApplicationIsInOrder, application.ApplicationNumber, application.RegistrationDate.GetDateTimeAsString()
                            , new ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto
                            {
                            });
                    continue;
                }
                var recList = vocabularyStorage.RecomendedListsVoc.Items.Where(t => t.ApplicationID == dbApplication.ApplicationID && t.DateDelete == null);
                if (recList.Any())
                {
                    SetBroken(ConflictMessages.ApplicationIsInRecList, application.ApplicationNumber, application.RegistrationDate.GetDateTimeAsString()
                            , new ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto
                            {
                                RecommendedLists = recList.Select(t => new RecommendedListShort { Stage = t.Stage }).ToArray()
                            });
                    continue;
                }

                deleteItems.Add(dbApplication);
            }
        }

        private void SetBroken(int errorCode, string number, string regDate, ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto conflictsResultDto)
        {
            var failDto = new ApplicationFailDetailsDto();
            if (!string.IsNullOrEmpty(number))
                failDto.ApplicationNumber = number;
            if (!string.IsNullOrEmpty(number))
                failDto.RegistrationDate = regDate;

            failDto.ErrorInfo = new ErrorInfoImportDto(errorCode, conflictsResultDto);

            conflictStorage.notRemovedApps.Add(failDto);
        }

        protected override void PrepareDataForDelete()
        {
            var deleteReader = new BulkDeleteReader(dataForDelete.ImportPackageId);
            deleteReader.AddRange(deleteItems);

            bulks.Add(new Tuple<string, IDataReader>("bulk_Delete", deleteReader));
        }
    }
}
