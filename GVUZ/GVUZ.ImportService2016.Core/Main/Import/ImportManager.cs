using GVUZ.ImportService2016.Core.Main.Repositories;
using GVUZ.ImportService2016.Core.Main.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GVUZ.ImportService2016.Core.Dto.Import;
using GVUZ.ServiceModel.Import.Core.Operations.Conflicts;
using GVUZ.ServiceModel.Import.Core.Packages;
using GVUZ.ImportService2016.Core.Dto.Partial;
using GVUZ.ImportService2016.Core.Main.Dictionaries;
using GVUZ.Model.Entrants.Documents;

namespace GVUZ.ImportService2016.Core.Main.Import
{
    public class ImportManager
    {
        private PackageData packageData;
        public PackageData PackageData { get { return packageData; } }

        private VocabularyStorage vocabularyStorage;
        private ImportConflictStorage conflictStorage;

        public static Dictionary<int, int> DeadlockErrors = new Dictionary<int, int>();
        public static readonly int MaxDeadlockErrors = 10;
        private static readonly string deadlockWork = "deadlock";

        static ImportManager()
        {
            var configDeadlockRestart = System.Configuration.ConfigurationManager.AppSettings["DeadlockRestart"];
            int maxErrors = 0;
            if (int.TryParse(configDeadlockRestart, out maxErrors))
                MaxDeadlockErrors = maxErrors;

            deadlockWork = System.Configuration.ConfigurationManager.AppSettings["DeadlockWord"];
            if (string.IsNullOrWhiteSpace(deadlockWork))
                deadlockWork = "deadlock";
        }


        public ImportManager(ImportPackage package)
        {
            // десереализовать xml в наш "дто"
            packageData = package.PackageData.ParseXML<GVUZ.ImportService2016.Core.Dto.Import.PackageData>();
            packageData.InstitutionId = package.InstitutionID;
            packageData.ImportPackageId = package.PackageID;
            packageData.PackageType = package.TypeID;
            vocabularyStorage = package.VocabularyStorage;
            
            conflictStorage = new ImportConflictStorage(vocabularyStorage);
        }

        /// <summary>
        /// Импортирует данные из текущего пакета
        /// </summary>
        /// <returns>true, если были успешно импортированы заявления (и по ним дальше будет проверка)</returns>
        public List<int> DoWork(bool deleteBulk)
        {
            while (true)
            {

                string comment = string.Empty;
                try
                {
                    if (packageData.CampaignInfo != null)
                        new CampaignInfoImporter(packageData, vocabularyStorage, conflictStorage, deleteBulk).DoImport();
                    if (packageData.TargetOrganizations != null)
                        new TargetOrganizationImporter(packageData, vocabularyStorage, conflictStorage, deleteBulk).DoImport();
                    if (packageData.InstitutionPrograms != null)
                        new InstitutionProgramsImporter(packageData, vocabularyStorage, conflictStorage, deleteBulk).DoImport();
                    if (packageData.AdmissionInfo != null)
                        new AdmissionInfoImporter(packageData, vocabularyStorage, conflictStorage, deleteBulk).DoImport();
                    if (packageData.InstitutionAchievements != null)
                        new InstitutionAchievementsImporter(packageData, vocabularyStorage, conflictStorage, deleteBulk).DoImport();
                    if (packageData.GetApplications != null)
                        new ApplicationImporter(packageData, vocabularyStorage, conflictStorage, deleteBulk).DoImport();
                    if (packageData.Orders != null)
                        new OrderOfAdmissionImporter(packageData, vocabularyStorage, conflictStorage, deleteBulk).DoImport();
                }
                catch (Exception ex)
                {
                    comment = ex.Message + "  " + ex.StackTrace;
                }

                var isDeadlock = comment.ToLower().Contains(deadlockWork.ToLower());
                if (isDeadlock)
                {
                    if (!DeadlockErrors.ContainsKey(packageData.ImportPackageId))
                        DeadlockErrors.Add(packageData.ImportPackageId, 1);
                    else
                        DeadlockErrors[packageData.ImportPackageId]++;

                    if (DeadlockErrors[packageData.ImportPackageId] <= MaxDeadlockErrors)
                    {
                        GVUZ.ImportService2016.Core.Main.Log.LogHelper.Log.ErrorFormat("№ {0} упал с DEADLOCK (IMPORT) в {1} раз и ушел на повторный импорт", packageData.ImportPackageId, DeadlockErrors[packageData.ImportPackageId]);
                        System.Threading.Thread.Sleep(30000); // Делаем паузу, чтобы дедлок разсосался за это время
                        ADOPackageRepository.ResetDeadlockImportPackage(packageData.ImportPackageId);
                    }
                    else
                    {
                        string applicationIds = conflictStorage.ImportedApplicationIDsString;
                        var resultPackage = conflictStorage.GetResultPackage(packageData.ImportPackageId);
                        ADOPackageRepository.UpdateImportPackage(resultPackage, applicationIds, packageData.ImportPackageId, comment);
                        GVUZ.ImportService2016.Core.Main.Log.LogHelper.Log.DebugFormat("№ {0} Импорт превысил максимальное число DEADLOCK-ов", packageData.ImportPackageId);
                        return conflictStorage.ImportedApplicationIDs;
                    }
                }
                else
                {

                    string applicationIds = conflictStorage.ImportedApplicationIDsString;
                    var resultPackage = conflictStorage.GetResultPackage(packageData.ImportPackageId);
                    ADOPackageRepository.UpdateImportPackage(resultPackage, applicationIds, packageData.ImportPackageId, comment);
                    GVUZ.ImportService2016.Core.Main.Log.LogHelper.Log.DebugFormat("№ {0} Импорт завершен", packageData.ImportPackageId);
                    return conflictStorage.ImportedApplicationIDs;
                }
            }
        }
    }
}
