using GVUZ.ImportService2016.Core.Main.Repositories;
using GVUZ.ServiceModel.Import.Core.Packages.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using GVUZ.ImportService2016.Core.Main.Extensions;
using GVUZ.ImportService2016.Core.Dto;
using System.Data.SqlClient;
using System.Configuration;
using GVUZ.ImportService2016.Core.Main.Import;
using GVUZ.ImportService2016.Core.Dto.Partial;
using GVUZ.ImportService2016.Core.Main.Check;
using GVUZ.ImportService2016.Core.Main.Delete;
using GVUZ.ImportService2016.Core.Main.Log;

namespace GVUZ.ImportService2016.Core.Main
{
    public class PackageManager
    {
        public static readonly HashSet<int> _processingInstitutions = new HashSet<int>();
        //private static readonly HashSet<int> _checkingInstitutions = new HashSet<int>();

        public static readonly Object lockObject = new Object();
        public static readonly Object lockObjectCheck = new Object();

        /// <summary>
        /// Обработка пакетов
        /// </summary>
        /// <returns>true - если нашёлся пакет</returns>
        public static bool TryProcessNextPackage(ref int institutionID, int packageID, bool deleteBulk)
        {
            // План действий
            // 1. Получить очередной пакет для обработки (импорта)
            // 2. Проверки его корректности
            // 3. Запись в базу (результаты импорта и результаты в ImportPackage)
            
            // потом будем стараться выбирать следующий пакет по данному институту, а если нет, значит берем другой институт
            ImportPackage package;

            // надо точно быть уверенным, другие потоки не возьмут данных по тому же институту
            //lock (_processingInstitutions)
            lock (lockObject)
            {
                do
                {
                    // берём пакет исключая институты, которые уже в процессе - теперь это внутри хранимки
                    package = ADOPackageRepository.GetUnprocessedPackage(packageID);
                    if (package == null)
                        return false;
                    // удаляем пакет из bulk_ таблиц
                    ADOPackageRepository.ResetImportPackages(false, false, true, package.PackageID);

                    if (_processingInstitutions.Contains(package.InstitutionID))
                        ADOPackageRepository.ReleasePackage(packageID);
                }
                while (_processingInstitutions.Contains(package.InstitutionID));

                // записываем институты, которые в обработке - просто для логов
                _processingInstitutions.Add(package.InstitutionID);
                LogHelper.Log.InfoFormat(string.Join(",", _processingInstitutions.ToArray()));
            }

            institutionID = package.InstitutionID;
            try
            {
                ProcessPackage(package, deleteBulk, packageID != 0);
            }
            catch (Exception ex)
            {
                ADOPackageRepository.UpdateImportPackage("", "", package.PackageID, ex.Message);
            }
            finally
            {
                lock (_processingInstitutions)
                {
                    _processingInstitutions.Remove(institutionID);
                }
            }

            return true;
        }

        public static List<ImportPackage> packagesToCheck = new List<ImportPackage>();
        /// <summary>
        /// Проверка заявлений из пакетов в ФБС
        /// </summary>
        /// <returns></returns>
        public static bool TryCheckNextPackage(ref int institutionID, int packageID)
        {
            // План действий
            // 1. Получить очередной пакет для обработки (проверки)
            // 2. Проверки его корректности
            // 3. Запись в базу (результаты импорта и результаты в ImportPackage)

            // потом будем стараться выбирать следующий пакет по данному институту, а если нет, значит берем другой институт
            ImportPackage package = null;
            if (packageID != 0)
            {
                package = ADOPackageRepository.GetUncheckedPackage(packageID);
            }
            else
            {
                // надо точно быть уверенным, другие потоки не возьмут данных по тому же институту
                lock (lockObjectCheck)
                {
                    if (packagesToCheck == null || !packagesToCheck.Any())
                    {
                        // берём пакет исключая институты, которые уже в процессе
                        packagesToCheck = ADOPackageRepository.GetUncheckedPackages();
                    }
                    if (packagesToCheck != null && packagesToCheck.Any())
                    {
                        package = packagesToCheck.FirstOrDefault();
                        packagesToCheck.Remove(package);
                    }
                }
            }
            if (package == null) return false;

            institutionID = package.InstitutionID;
            try
            {
                CheckPackage(package);
            }
            catch (Exception ex)
            {
                //ADOPackageRepository.UpdateImportPackage("", "", package.PackageID, ex.Message);
                ADOPackageRepository.UpdateImportPackageCheckResult(package.PackageID, null, ex.Message);
            }
            finally
            {
                //lock (_checkingInstitutions)
                //{
                //    _checkingInstitutions.Remove(institutionID);
                //}
            }
            return true;
        }

        public static int CheckPackage(ImportPackage package)
        {
            var sw = new Stopwatch();
            sw.Start();

            var importManager = new ImportManager(package);
            var applicationIDs = package.ImportedAppIDs.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).Select(t => int.Parse(t)).ToList();
            LogHelper.Log.DebugFormat("№ {0} - всего заявлений для проверки: {1}", package.PackageID, applicationIDs.Count);

            new CheckManager(importManager.PackageData, applicationIDs, package.UserLogin).DoCheck();

            sw.Stop();
            LogHelper.Log.InfoFormat("№ {0} проверен. Время: {1} сек.", package.PackageID, sw.Elapsed.TotalSeconds);

            return package.PackageID;
        }

        public static int ProcessPackage(ImportPackage package, bool deleteBulk, bool testMode)
        {
            var sw = new Stopwatch();
            sw.Start();

            switch (package.TypeID) 
            {
                case (int)GVUZ.ServiceModel.Import.Core.Packages.PackageType.Import: // 1: // импорт
                case (int)GVUZ.ServiceModel.Import.Core.Packages.PackageType.ImportApplicationSingle: //11: // импорт 1 заявления
                case 101: // Временно для отладки сервиса параллельно с работой старого 
                    var importManager = new ImportManager(package);
                    var applicationIDs = new List<int>();
                    if (package.StatusID == 1 || package.StatusID >= 5 || testMode)
                    {
                        applicationIDs = importManager.DoWork(deleteBulk);
                    }
                    else if (package.StatusID == 3 && package.CheckStatusID == 1)
                    {
                        applicationIDs = package.ImportedAppIDs.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).Select(t => int.Parse(t)).ToList();
                    }
                    else
                    {
                        if (!testMode)
                            throw new Exception("Неправильный статус пакета: " + package.PackageID); 
                    }

                    // 
                    // new CheckManager(importManager.PackageData, applicationIDs, package.VocabularyStorage.CampaignVoc, package.UserLogin).DoWork();

                    break;
                
                case (int)GVUZ.ServiceModel.Import.Core.Packages.PackageType.Delete: //2: // удаление
                    var deleteManager = new DeleteManager(package);
                    deleteManager.DoWork(deleteBulk);
                    break;

                default:
                    LogHelper.Log.ErrorFormat("Неправильный тип пакета: {0}, PackageID = {1} , StatusID = {2}, CheckStatusID = {3}", package.TypeID, package.PackageID, package.StatusID, package.CheckStatusID);
                    break;
            }

            sw.Stop();
            LogHelper.Log.InfoFormat("№ {0} загружен. Время: {1} сек.", package.PackageID, sw.Elapsed.TotalSeconds);

            return package.PackageID;
        }
    }
}
