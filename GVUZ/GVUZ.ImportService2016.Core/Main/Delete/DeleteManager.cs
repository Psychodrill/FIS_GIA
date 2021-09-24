using GVUZ.ImportService2016.Core.Main.Dictionaries;
using GVUZ.ImportService2016.Core.Dto.Delete;
using GVUZ.ImportService2016.Core.Dto.Partial;
using GVUZ.ImportService2016.Core.Main.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GVUZ.ImportService2016.Core.Main.Repositories;
using GVUZ.ServiceModel.Import.Package;
using GVUZ.ServiceModel.Import.WebService.Dto.Result.Import;
using GVUZ.ImportService2016.Core.Main.Conflicts;

namespace GVUZ.ImportService2016.Core.Main.Delete
{
    public class DeleteManager
    {
        private GVUZ.ImportService2016.Core.Dto.Delete.DataForDelete dataForDelete;
        public GVUZ.ImportService2016.Core.Dto.Delete.DataForDelete DataForDelete { get { return dataForDelete; } }

        private VocabularyStorage vocabularyStorage;
        private DeleteConflictStorage conflictStorage;


        public DeleteManager(ImportPackage package)
        {
            // десереализовать xml в наш "дто"
            dataForDelete = package.PackageData.ParseXML<GVUZ.ImportService2016.Core.Dto.Delete.DataForDelete>();
            dataForDelete.InstitutionId = package.InstitutionID;
            dataForDelete.ImportPackageId = package.PackageID;
            vocabularyStorage = package.VocabularyStorage;

            conflictStorage = new DeleteConflictStorage(vocabularyStorage);
        }

        /// <summary>
        /// Удаляет данные в БД по информации из текущего пакета
        /// </summary>
        public void DoWork(bool deleteBulk)
        {
            string comment = string.Empty;
            var errors = new List<string>();
            try
            {
                if (dataForDelete.ApplicationCommonBenefits != null)
                    errors.AddRange(new ApplicationCommonBenefitsDeleter(dataForDelete, vocabularyStorage, conflictStorage, deleteBulk).DoDelete());
                
                if (dataForDelete.EntranceTestResults != null)
                    errors.AddRange(new EntranceTestResultsDeleter(dataForDelete, vocabularyStorage, conflictStorage, deleteBulk).DoDelete());

                if (dataForDelete.Orders != null || dataForDelete.ApplicationsInOrders != null)
                    errors.AddRange(new OrderOfAdmissionDeleter(dataForDelete, vocabularyStorage, conflictStorage, deleteBulk).DoDelete());
                
                if (dataForDelete.Applications != null)
                    errors.AddRange(new ApplicationsDeleter(dataForDelete, vocabularyStorage, conflictStorage, deleteBulk).DoDelete());

                if (dataForDelete.CompetitiveGroups != null)
                    errors.AddRange(new CompetitiveGroupsDeleter(dataForDelete, vocabularyStorage, conflictStorage, deleteBulk).DoDelete());

                if (dataForDelete.InstitutionAchievements != null)
                    errors.AddRange(new InstitutionAchievementsDeleter(dataForDelete, vocabularyStorage, conflictStorage, deleteBulk).DoDelete());

                if (dataForDelete.TargetOrganizations != null)
                    errors.AddRange(new TargetOrganizationsDeleter(dataForDelete, vocabularyStorage, conflictStorage, deleteBulk).DoDelete());

                if (dataForDelete.Campaigns != null)
                    errors.AddRange(new CampaignsDeleter(dataForDelete, vocabularyStorage, conflictStorage, deleteBulk).DoDelete());

                if (errors.Any())
                    comment = string.Join(";\n ", errors);
                
            }
            catch (DeleteManagerNotImplementedException dmEx)
            {
                var error = new ErrorInfoImportDto() { ErrorCode="", Message = dmEx.Message };
                var resultError = PackageHelper.GenerateXmlPackageIntoString(error, "Error");
                ADOPackageRepository.UpdateImportPackage(resultError, "", dataForDelete.ImportPackageId, "");
                return;
            }
            catch (Exception ex)
            {
                comment = ex.Message + "  " + ex.StackTrace;
            }
            
            // Пакету присвоить статус 3 (обработан)
            //string applicationIds = conflictStorage.ImportedApplicationIDsString;
            //var resultPackage = conflictStorage.GetResultPackage(dataForDelete.ImportPackageId);

            string resultPackage = PackageHelper.GenerateXmlPackageIntoString(conflictStorage.PrepareProcessResultObject());

            ADOPackageRepository.UpdateImportPackage(resultPackage, "", dataForDelete.ImportPackageId, comment);
        }


        
    }

    public class DeleteManagerNotImplementedException : Exception {
        public DeleteManagerNotImplementedException(string message) : base(message) { }
    }
}
