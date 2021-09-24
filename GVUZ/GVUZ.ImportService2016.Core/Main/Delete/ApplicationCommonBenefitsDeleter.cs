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
    public class ApplicationCommonBenefitsDeleter : BaseDeleter
    {
        public ApplicationCommonBenefitsDeleter(Dto.Delete.DataForDelete dataForDelete, VocabularyStorage vocabularyStorage, DeleteConflictStorage importConflictStorage, bool deleteBulk) : base(dataForDelete, vocabularyStorage, importConflictStorage, deleteBulk) { }

        List<VocabularyBaseDto> deleteItems = new List<VocabularyBaseDto>();

        protected override void Validate()
        {
            foreach (var applicationCommonBenefit in dataForDelete.ApplicationCommonBenefits.Items)
            {
                IEnumerable<ApplicationEntranceTestDocumentVocDto> etDocs = null;
                ApplicationEntranceTestDocumentVocDto etDoc = null;
                if (applicationCommonBenefit is uint)
                {
                    // uint => это id
                    int id = Convert.ToInt32(applicationCommonBenefit);
                    etDocs = vocabularyStorage.ApplicationEntranceTestDocumentVoc.Items
                                            .Where(x => x.ID == id && x.EntranceTestItemID == 0);
                }
                else
                {
                    // не uint => это строка uid 
                    etDocs = vocabularyStorage.ApplicationEntranceTestDocumentVoc.Items
                                           .Where(x => x.UID == applicationCommonBenefit.ToString() && x.EntranceTestItemID == 0);
                }

                if (etDocs.Count() > 1)
                {
                    SetBroken(ConflictMessages.ApplicationBenefitIsMoreThenOne, string.Empty, string.Empty,
                         new ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto
                         {
                             ApplicationCommonBenefits = new[] { applicationCommonBenefit.ToString() },
                         });
                    continue;
                }

                etDoc = etDocs.FirstOrDefault();
                if (etDoc == null)
                {
                    SetBroken(ConflictMessages.ApplicationBenefitIsNotFound, string.Empty, string.Empty,
                            new ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto
                         {
                             ApplicationCommonBenefits = new[] { applicationCommonBenefit.ToString() },
                         });
                    continue;
                }

                var cg = vocabularyStorage.CompetitiveGroupVoc.Items.Where(t => t.CompetitiveGroupID == etDoc.CompetitiveGroupID).FirstOrDefault();
                var cgName = cg != null ? cg.Name : string.Empty;

                var b = VocabularyStatic.BenefitVoc.Items.Where(t => t.BenefitID == etDoc.BenefitID).FirstOrDefault();
                var benefitName = b != null ? b.Name : string.Empty;

                var application = vocabularyStorage.ApplicationVoc.Items.Where(t => t.ApplicationID == etDoc.ApplicationID).FirstOrDefault();

                if (application.StatusID == GVUZ.ServiceModel.Import.ApplicationStatusType.InOrder && etDoc.BenefitID != 0)
                {
                    SetBroken(ConflictMessages.CantDeleteCommonBenefitUsedInApp, cgName, benefitName,
                         new ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto
                         {
                             ApplicationCommonBenefits = new[] { applicationCommonBenefit.ToString() },
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

        private void SetBroken(int errorCode, string cgName, string benefitName, ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto conflictsResultDto)
        {
            var failDto = new CommonBenefitFailDetailsDto();
            if (!string.IsNullOrEmpty(cgName))
                failDto.CompetitiveGroupName = cgName;
            if (!string.IsNullOrEmpty(benefitName))
                failDto.BenefitKindName = benefitName;

            failDto.ErrorInfo = new ErrorInfoImportDto(errorCode, conflictsResultDto);

            conflictStorage.notRemovedCommonBenefits.Add(failDto);
        }

        protected override void PrepareDataForDelete()
        {
            var deleteReader = new BulkDeleteReader(dataForDelete.ImportPackageId);
            deleteReader.AddRange(deleteItems);

            bulks.Add(new Tuple<string, IDataReader>("bulk_Delete", deleteReader));
        }
    
    }
}
