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
using System.Text;
using System.Threading.Tasks;
using GVUZ.ImportService2016.Core.Main.Extensions;
using GVUZ.ImportService2016.Core.Main.Dictionaries.Application;

namespace GVUZ.ImportService2016.Core.Main.Delete
{
    class EntranceTestResultsDeleter : BaseDeleter
    {
        public EntranceTestResultsDeleter(GVUZ.ImportService2016.Core.Dto.Delete.DataForDelete dataForDelete, VocabularyStorage vocabularyStorage, DeleteConflictStorage importConflictStorage, bool deleteBulk) : base(dataForDelete, vocabularyStorage, importConflictStorage, deleteBulk) { }

        List<VocabularyBaseDto> deleteItems = new List<VocabularyBaseDto>();

        protected override void Validate()
        {
            foreach (var applicationCommonBenefit in dataForDelete.EntranceTestResults.Items)
            {
                IEnumerable<ApplicationEntranceTestDocumentVocDto> etDocs = null;
                ApplicationEntranceTestDocumentVocDto etDoc = null;
                if (applicationCommonBenefit is uint)
                {
                    // uint => это id
                    int id = Convert.ToInt32(applicationCommonBenefit);
                    etDocs = vocabularyStorage.ApplicationEntranceTestDocumentVoc.Items
                                            .Where(x => x.ID == id && x.EntranceTestItemID != 0);
                }
                else
                {
                    // не uint => это строка uid 
                    etDocs = vocabularyStorage.ApplicationEntranceTestDocumentVoc.Items
                                           .Where(x => x.UID == applicationCommonBenefit.ToString() && x.EntranceTestItemID != 0);
                }

                if (etDocs.Count() > 1)
                {
                    SetBroken(ConflictMessages.AppEntranceTestMoreThenOne, string.Empty, string.Empty, string.Empty,
                         new ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto
                         {
                             EntranceTestResults = new[] { applicationCommonBenefit.ToString() },
                         });
                    continue;
                }

                etDoc = etDocs.FirstOrDefault();

               
                if (etDoc == null)
                {
                    SetBroken(ConflictMessages.AppEntranceTestIsNotFound, string.Empty, string.Empty, string.Empty,
                            new ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto
                            {
                                EntranceTestResults = new[] { applicationCommonBenefit.ToString() },
                            });
                    continue;
                }

                var cg = vocabularyStorage.CompetitiveGroupVoc.Items.Where(t => t.CompetitiveGroupID == etDoc.CompetitiveGroupID).FirstOrDefault();
                var cgName = cg != null ? cg.Name : string.Empty;

                var entranceTestTypeName = string.Empty;
                var subjectName = string.Empty;
                var entranceTestItemC = vocabularyStorage.EntranceTestItemCVoc.Items.Where(t => t.EntranceTestItemID == etDoc.EntranceTestItemID).FirstOrDefault();
                if (entranceTestItemC != null)
                {
                    var entranceTestType = VocabularyStatic.EntranceTestTypeVoc.Items.Where(t => t.EntranceTestTypeID == entranceTestItemC.EntranceTestTypeID).FirstOrDefault();
                    var subject = entranceTestItemC.SubjectID != 0 ? VocabularyStatic.SubjectVoc.Items.Where(t => t.SubjectID == entranceTestItemC.SubjectID).FirstOrDefault() : null;

                    entranceTestTypeName = entranceTestType != null ? entranceTestType.Name : string.Empty;
                    subjectName = subject==null ?  entranceTestItemC.SubjectName : subject.Name;
                }


                var application = vocabularyStorage.ApplicationVoc.Items.Where(t => t.ApplicationID == etDoc.ApplicationID).FirstOrDefault();
                if (application.StatusID == GVUZ.ServiceModel.Import.ApplicationStatusType.InOrder)
                {
                    SetBroken(ConflictMessages.CantDeleteResultLinkedWithAppInOrder, cgName, entranceTestTypeName, subjectName,
                         new ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto
                         {
                             EntranceTestResults = new[] { applicationCommonBenefit.ToString() },
                             Applications = new[] { new ApplicationShortRef { ApplicationNumber = application.ApplicationNumber, RegistrationDateDate = application.RegistrationDate } }
                         });

                    continue;
                }


                // Проверки пройдены, теперь отцепляем от заявления документ, если он больше нигде не используется
                var doc = vocabularyStorage.ApplicationEntrantDocumentVoc.Items.Where(x => x.EntrantDocumentID == etDoc.EntrantDocumentID).FirstOrDefault();
                if (doc != null)
                {
                    int docCnt = vocabularyStorage.ApplicationEntranceTestDocumentVoc.Items.Count(x => x.EntrantDocumentID == etDoc.EntrantDocumentID
                                                                                       && x.ID != etDoc.ID
                                                                                       && x.ApplicationID == etDoc.ApplicationID);
                    if (docCnt == 0)
                        deleteItems.Add(doc);
                }

                deleteItems.Add(etDoc);
            }
        }

        private void SetBroken(int errorCode, string cgName, string entranceTestTypeName, string subjectName, ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto conflictsResultDto)
        {
            var failDto = new EntranceTestItemFailDetailsDto();
            if (!string.IsNullOrEmpty(cgName))
                failDto.CompetitiveGroupName = cgName;
            if (!string.IsNullOrEmpty(entranceTestTypeName))
                failDto.EntranceTestType = entranceTestTypeName;
            if (!string.IsNullOrEmpty(subjectName))
                failDto.SubjectName = subjectName;

            failDto.ErrorInfo = new ErrorInfoImportDto(errorCode, conflictsResultDto);

            conflictStorage.notRemovedEntranceTests.Add(failDto);
        }

        protected override void PrepareDataForDelete()
        {
            var deleteReader = new BulkDeleteReader(dataForDelete.ImportPackageId);
            deleteReader.AddRange(deleteItems);

            bulks.Add(new Tuple<string, IDataReader>("bulk_Delete", deleteReader));
        }
    }
}
