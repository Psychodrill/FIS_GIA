using System;
using System.Collections.Generic;
using FBS.Replicator.DB;
using FBS.Replicator.DB.GVUZ;
using FBS.Replicator.DB.FBS;
using FBS.Replicator.Entities;
using FBS.Replicator.Entities.FBS;
using FBS.Replicator.Entities.GVUZ;
using FBS.Common;

namespace FBS.Replicator.Replication.FBSToGVUZ
{
    public class FBSToGVUZReplicator
    {
        private Tables _tables;
        public FBSToGVUZReplicator(Tables tables)
        {
            _tables = tables;
        }

        public bool HasCriticalErrors { get; private set; }
        public bool HasErrors { get; private set; }

        public bool Replicate()
        {
            Logger.WriteLine("Синхронизация данных");          
            Logger.WriteLine("Получение данных ФБС"); 
            Dictionary<int, IEnumerable<FBSPerson>> fbsPersons;
            bool fbsReadSuccess = new FBSReadDB(_tables).GetAllPersonsWithDocuments(out fbsPersons);
            GC.Collect();
            Logger.WriteLine("Получение данных ФБС завершено");
            if (!fbsReadSuccess)
            {
                Logger.WriteLine("При получении данных ФБС произошла ошибка");
                ProcessError("Синхронизация невозможна", true);
                return false;
            }

            Logger.WriteLine("Получение данных РВИ");
            Dictionary<int, IEnumerable<GVUZPerson>> gvuzPersons;
            bool gvuzReadSuccess = new GVUZReadDB().GetAllPersonsWithDocuments(out gvuzPersons);
            GC.Collect();
            Logger.WriteLine("Получение данных РВИ завершено");
            if (!gvuzReadSuccess)
            {
                Logger.WriteLine("При получении данных РВИ произошла ошибка");
                ProcessError("Синхронизация невозможна", true);
                return false;
            }

            Logger.WriteLine("Сравнение данных");
            bool mergeSuccess = FBSToGVUZMerger.Merge(fbsPersons, gvuzPersons);
            Logger.WriteLine("Сравнение данных завершено");
            if (!mergeSuccess)
            {
                Logger.WriteLine("При сравнении данных произошла ошибка");
                ProcessError("Синхронизация невозможна", true);
                return false;
            }

            ShowStats(fbsPersons);

            Logger.WriteLine("Пакетная запись данных");
            bool gvuzWriteSuccess = new GVUZWriteDB_Bulk().WriteChanges(fbsPersons);
            Logger.WriteLine("Пакетная запись данных завершена");

            if (!gvuzWriteSuccess)
            {
                Logger.WriteLine("При пакетной записи данных возникли ошибки, будет произведена пошаговая обработка");
                Logger.WriteLine("Пошаговая запись данных");
                new GVUZWriteDB().WriteChanges(fbsPersons);
                Logger.WriteLine("Пошаговая запись данных завершена");
            }

            ShowStats(fbsPersons);

            Logger.WriteLine("Пакетная запись данных о связях в ФБС");
            bool fbsWriteSuccess = new FBSWriteDB_Bulk(_tables).WriteChanges(fbsPersons);
            Logger.WriteLine("Пакетная запись данных о связях в ФБС завершена");
            if (!fbsWriteSuccess)
            {
                Logger.WriteLine("При пакетной записи данных возникли ошибки, будет произведена пошаговая обработка");
                Logger.WriteLine("Пошаговая запись данных о связях в ФБС");
                new FBSWriteDB(_tables).WriteChanges(fbsPersons);
                Logger.WriteLine("Пошаговая запись данных о связях в ФБС завершена");
            }

            ShowStats(fbsPersons);
            Logger.WriteLine("Синхронизация данных завершена");
            return true;
        }

        private static void ShowStats(Dictionary<int, IEnumerable<FBSPerson>> fbsPersons)
        {
            int personsDoneCount = 0;
            int personsForProcessCount = 0;
            int personsUndefinedCount = 0;


            foreach (IEnumerable<FBSPerson> fbsPersonsByName in fbsPersons.Values)
            {
                foreach (FBSPerson fbsPerson in fbsPersonsByName)
                {
                    if (fbsPerson.Action == FBSToGVUZActions.Done)
                    {
                        personsDoneCount++;
                    }
                    else if ((fbsPerson.Action != FBSToGVUZActions.None) && (fbsPerson.Action != FBSToGVUZActions.Undefined))
                    {
                        personsForProcessCount++;
                    }
                    else if (fbsPerson.Action == FBSToGVUZActions.Undefined)
                    {
                        personsUndefinedCount++;
                    }
                }
            }

            Logger.WriteLine("Физических лиц для обработки: " + personsForProcessCount.ToString());
            Logger.WriteLine("Физических лиц неопределенных: " + personsUndefinedCount.ToString());
            Logger.WriteLine("Физических лиц обработано: " + personsDoneCount.ToString());
        }

        private void ProcessError(string message, bool isCritical)
        {
            HasErrors = true;
            if (isCritical)
            {
                HasCriticalErrors = true;
            }
            Logger.WriteLine(message.ToUpper());
        }
    }
}
