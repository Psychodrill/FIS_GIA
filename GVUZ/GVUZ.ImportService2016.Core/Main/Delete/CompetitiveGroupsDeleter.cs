using GVUZ.ImportService2016.Core.Main.Conflicts;
using GVUZ.ImportService2016.Core.Main.Dictionaries;
using GVUZ.ImportService2016.Core.Dto.DataReaders;
using GVUZ.ImportService2016.Core.Dto.DeleteManager;
using GVUZ.ImportService2016.Core.Dto.Import;
using GVUZ.ServiceModel.Import.Core.Operations.Conflicts;
using GVUZ.ServiceModel.Import.WebService.Dto.Result.Import;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GVUZ.ImportService2016.Core.Main.Extensions;

namespace GVUZ.ImportService2016.Core.Main.Delete
{
    public class CompetitiveGroupsDeleter : BaseDeleter
    {
        public CompetitiveGroupsDeleter(GVUZ.ImportService2016.Core.Dto.Delete.DataForDelete dataForDelete, VocabularyStorage vocabularyStorage, DeleteConflictStorage importConflictStorage, bool deleteBulk)
            : base(dataForDelete, vocabularyStorage, importConflictStorage, deleteBulk) 
        {
            competitiveGroupIDs = dataForDelete.CompetitiveGroups.Items.Select(t=> t.ToString()).ToList();
        }

        private List<string> competitiveGroupIDs;
        List<CompetitiveGroupDeleteManager> cgDMs = new List<CompetitiveGroupDeleteManager>();

        protected override void Validate()
        {
            foreach (var competitiveGroup in dataForDelete.CompetitiveGroups.Items)
            {
                CompetitiveGroupVocDto dbCompetitiveGroup = null;
                IEnumerable<CompetitiveGroupVocDto> competitiveGroups = null;
                if (competitiveGroup is uint)
                {
                    int id = Convert.ToInt32(competitiveGroup);
                    competitiveGroups = vocabularyStorage.CompetitiveGroupVoc.Items.Where(t => t.CompetitiveGroupID == id);
                }
                else
                {
                    // uid
                    competitiveGroups = vocabularyStorage.CompetitiveGroupVoc.Items.Where(t => t.UID == competitiveGroup.ToString());
                }

                if (competitiveGroups.Count() > 1)
                {
                    SetBroken(competitiveGroup, ConflictMessages.CompetitiveGroupMoreThenOne, string.Empty, new GVUZ.ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto { CompetitiveGroups = new[] { competitiveGroup.ToString() } });
                    continue;
                }
                dbCompetitiveGroup = competitiveGroups.FirstOrDefault();
                if (dbCompetitiveGroup == null){
                    SetBroken(competitiveGroup, ConflictMessages.CompetitiveGroupIsNotFound, string.Empty, new GVUZ.ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto { CompetitiveGroups = new[] { competitiveGroup.ToString() } });
                    continue;
                }

                //var dbCGI = vocabularyStorage.CompetitiveGroupItemVoc.Items.Where(t => t.CompetitiveGroupID == dbCompetitiveGroup.CompetitiveGroupID);
                //if (dbCGI.Any())
                //{
                //    SetBroken(competitiveGroup, ConflictMessages.CantDeleteCompetitiveGroupWithGroupItems, dbCompetitiveGroup.Name,
                //                    new ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto { CompetitiveGroupItems = dbCGI.Select(t => t.CompetitiveGroupItemID.ToString()).ToArray() });
                //    continue;
                //}

                CompetitiveGroupDeleteManager dm = new CompetitiveGroupDeleteManager(dbCompetitiveGroup, vocabularyStorage);
                if (dm.GetDependencies())
                {
                    var applicationsVocDto = dm.applications;
                    GVUZ.ServiceModel.Import.WebService.Dto.ApplicationShortRef[] appShortRefs = applicationsVocDto
                        .Select(t => new GVUZ.ServiceModel.Import.WebService.Dto.ApplicationShortRef() { ApplicationNumber = t.ApplicationNumber, RegistrationDateDate = t.RegistrationDate, UID = t.UID }).ToArray();

                    SetBroken(competitiveGroup, ConflictMessages.CantDeleteCompetitiveGroupWithApplications, dbCompetitiveGroup.Name,
                        new ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto { Applications = appShortRefs});
                    continue;
                }

                cgDMs.Add(dm);
            }
        }

        private void SetBroken(object competitiveGroupObj, int errorCode, string cgName, ServiceModel.Import.WebService.Dto.Result.ConflictsResultDto conflictsResultDto)
        {
            string competitiveGroup = competitiveGroupObj.ToString();
            if (competitiveGroupIDs.Contains(competitiveGroup))
                competitiveGroupIDs.Remove(competitiveGroup);

            var failDto = new CompetitiveGroupFailDetailsDto();
            if (!string.IsNullOrEmpty(cgName))
                failDto.CompetitiveGroupName = cgName;

            if (competitiveGroupObj is uint)
                failDto.ID = competitiveGroup;
            else
                failDto.UID = competitiveGroup;

            failDto.ErrorInfo = new ErrorInfoImportDto(errorCode, conflictsResultDto);
            
            conflictStorage.notRemovedCompetitiveGroups.Add(failDto); 
        }

        protected override void PrepareDataForDelete()
        {
            // Delete items
            var deleteReader = new BulkDeleteReader(dataForDelete.ImportPackageId);
            foreach (var dm in cgDMs)
                deleteReader.AddRange(dm.GetDeleteObjects());
            

            bulks.Add(new Tuple<string, IDataReader>("bulk_Delete", deleteReader));
        }
    }
}
