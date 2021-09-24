using GVUZ.ImportService2016.Core.Main.Dictionaries;
using GVUZ.ImportService2016.Core.Dto.Import;
using GVUZ.ServiceModel.Import.Core.Operations.Conflicts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using GVUZ.ImportService2016.Core.Main.Log;
using GVUZ.ImportService2016.Core.Main.Extensions;
using GVUZ.ImportService2016.Core.Main.Repositories;

namespace GVUZ.ImportService2016.Core.Main.Import
{
    public abstract class BaseImporter
    {
        protected GVUZ.ImportService2016.Core.Dto.Import.PackageData packageData;
        protected VocabularyStorage vocabularyStorage;
        protected ImportConflictStorage conflictStorage;
        protected bool deleteBulk;

        public BaseImporter(GVUZ.ImportService2016.Core.Dto.Import.PackageData packageData, VocabularyStorage vocabularyStorage, ImportConflictStorage importConflictStorage, bool deleteBulk)
        {
            this.packageData = packageData;
            this.vocabularyStorage = vocabularyStorage;
            this.conflictStorage = importConflictStorage;
            this.deleteBulk = deleteBulk;
        }

        protected abstract void Validate();

        protected abstract List<string> ImportDb();

        public virtual void DoImport()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Validate();
            sw.Stop();
            var message = "№ {1} " + this.GetType().Name + ".Validate(): {0} сек";
            Console.WriteLine(message, sw.Elapsed.TotalSeconds, packageData.ImportPackageId);
            LogHelper.Log.InfoFormat(message, sw.Elapsed.TotalSeconds, packageData.ImportPackageId);
            sw.Restart();
            var errors = ImportDb();
            sw.Stop();
            message = "№ {1} " + this.GetType().Name + ".ImportDB(): {0} сек";
            Console.WriteLine(message, sw.Elapsed.TotalSeconds, packageData.ImportPackageId);
            LogHelper.Log.InfoFormat(message, sw.Elapsed.TotalSeconds, packageData.ImportPackageId);

            if (errors.Any())
            {
                var totalError = string.Join(";\n ", errors);
                throw new Exception(totalError);
            }
        }
        

        #region Общие функции
        private Dictionary<Type, HashSet<string>> objectsUIDByType = new Dictionary<Type, HashSet<string>>();
        protected void CheckUniqueUID(IUid[] items, IBroken brokenObject)
        {
            if (items == null) return; 

            List<string> uniqueIDs = new List<string>();
            foreach (var item in items)
            {
                ICollection<string> uidCollection;
                Type type = item.GetType();

                if (!objectsUIDByType.ContainsKey(type))
                    uidCollection = objectsUIDByType[type] = new HashSet<string>();
                else
                    uidCollection = objectsUIDByType[type];

                //если уж пропустили такой UID, то его отсутствие валидно по схеме, и нечего проверять
                if (item.UID == null) return;

                if (uidCollection.Where(x => x == item.UID).Any())
                {
                    if (brokenObject != null) // Если мы проверяем дочерние у объекта импорта
                        conflictStorage.SetObjectIsBroken(brokenObject, ConflictMessages.UIDMustBeUniqueForAllObjectInstancesOfType, item.GetDescription(), item.UID);
                    else if (item is IBroken) // Если это сами объекты импорта "верхнего уровня"
                        conflictStorage.SetObjectIsBroken((IBroken)item, ConflictMessages.UIDMustBeUniqueForAllObjectInstancesOfType, item.GetDescription(), item.UID);
                    else // Такого не должно быть, это ошибка в коде проверки!
                        throw new Exception("Неверно указан объект с ошибкой валидации!");
                }
                else
                    uidCollection.Add(item.UID);
            }
        }
        #endregion
    }
}
