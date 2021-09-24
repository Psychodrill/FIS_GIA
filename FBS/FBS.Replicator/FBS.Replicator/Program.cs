using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FBS.Replicator.DB;
using FBS.Replicator.DB.FBS;
using FBS.Replicator.Replication.ERBDToFBS;
using FBS.Replicator.Replication.FBSToGVUZ;
using FBS.Common;

namespace FBS.Replicator
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
            var assembly_data = Helpers.AssemblyInfo.Get(asm);
            string platform = "x??";
            if (IntPtr.Size == 4)
                platform = "x86";
            else if (IntPtr.Size == 8)
                platform = "x64";

            Logger.WriteLine(string.Format("СИНХРОНИЗАЦИЯ ДАННЫХ v.{0} ({1})", assembly_data.AssemblyVersion, platform));

            bool help;
            bool repair;
            IEnumerable<int> years;
            bool ERBDToFBS;
            bool FBSToGVUZ;
            bool loadCompositions2015;
            bool loadCompositions2016Plus;
            bool enableDetailedLog;
            IEnumerable<Guid> debugIds;
            bool delete_indices = false;

            if (!ParseArgs(args, out help, out repair, out years, out ERBDToFBS, out FBSToGVUZ, out loadCompositions2015, out loadCompositions2016Plus, out enableDetailedLog, out delete_indices, out debugIds))
                return;

            if (help)
            { 
                Logger.SetPrefix("Help");
                Logger.WriteLine("Параметры: ", false);
                Logger.WriteLine(" - help - вызов данного информационного окна. Пример: FBS.Replicator.exe help", false);
                Logger.WriteLine(String.Format(" - year - указание годов обработки. Значения: четырехзначные числа от {0} до {1} года включительно, через запятую без пробелов. По умолчанию - все годы с {2} до текущего вкючительно. Для обработки текущго года можно указать \"year=now\" или \"year=current\". Пример: FBS.Replicator.exe year=2015,2016", ReplicationMinYear, ReplicationMaxYear, ReplicationDefaultMinYear), false);
                Logger.WriteLine(" - erbd - включение или отключение репликации ЕРБД-ФБС. Значения: 1, yes - включено; 0, no - отключено. По умолчанию - включено. Пример: FBS.Replicator.exe erbd=no (отключено), FBS.Replicator.exe erbd=1 (включено)", false);
                Logger.WriteLine(" - rvi - включение или отключение репликации ФБС-ФИС(RVI). Значения: 1, yes - включено; 0, no - отключено. По умолчанию - включено. Пример: FBS.Replicator.exe rvi=no (отключено), FBS.Replicator.exe rvi=1 (включено)", false);
                Logger.WriteLine(" - compositions - режим загрузки сочинений. Значения: 2015 - загрузка только 2015 года; 2016 - загрузка 2016 и последующих лет; 0, no - отключено. По умолчанию - за 2015 последующие годы. Пример: FBS.Replicator.exe compositions=2016", false);
                Logger.WriteLine(" - detailslog - режим детализированного журнала. Значения: 1, yes - включено; 0, no - отключено. По умолчанию - включено только если используется отладочный режим, в противном случае - отключено. Пример: FBS.Replicator.exe detailslog=1 (включено)", false);
                Logger.WriteLine(" - debugids - режим отладки. Значения: набор идентификаторов (GUID) через запятую без пробелов. По умолчанию - отключено. Пример: FBS.Replicator.exe debugids=1E4724F4-ABF4-4A6F-A14B-5324FB4034B9,3E4024BD-A7D9-45D5-A2C5-98E684173F81", false);
                Logger.SetPrefix(null);
                End("Режим помощи, синхронизация не выполняналась", true);
                return;
            }

            Logger.DetailedLogger.Enabled = enableDetailedLog;

            if (repair)
            {
                Logger.WriteLine("ПАРАМЕТРЫ: режим восстановления индексов");
            }
            else
            {
                string compositionsModeStr = "нет";
                if ((loadCompositions2015) && (loadCompositions2016Plus))
                {
                    compositionsModeStr = "2015 и 2016+";
                }
                else if (loadCompositions2015)
                {
                    compositionsModeStr = "2015";
                }
                else if (loadCompositions2016Plus)
                {
                    compositionsModeStr = "2016+";
                }

                string debugIdsStr = "нет";
                if (debugIds.Any())
                {
                    debugIdsStr = String.Join(", ", debugIds);
                }

                Logger.WriteLine(String.Format("ПАРАМЕТРЫ: год(ы) = {0}; синхронизация ЕРБД с ФБС = {1}; синхронизация ФБС с РВИ = {2}; загрузка бланков сочинений = {3}; детализированный журнал = {4}; отладка = {5}", String.Join(",", years.OrderBy(x => x)), ERBDToFBS ? "да" : "нет", FBSToGVUZ ? "да" : "нет", compositionsModeStr, enableDetailedLog ? "да" : "нет", debugIdsStr));
            }

            if (!Check(ERBDToFBS, FBSToGVUZ, loadCompositions2015, loadCompositions2016Plus))
                return;

            Tables syncronizationTables;
            bool getSyncronizationTablesSuccess = GetFBSSyncronizationTables(out syncronizationTables);
            if (!getSyncronizationTablesSuccess)
                return;

            //if (delete_indices)
            //{
            //    bool removeIndexesSuccess = RemoveIndexes(syncronizationTables);
            //    if (!removeIndexesSuccess)
            //        return;
            //}
            if (ERBDToFBS)
            {
                SyncERBDToFBS(syncronizationTables, years, loadCompositions2015, loadCompositions2016Plus, debugIds);
            } 

            //Пока отладочный режим только для ФБС
            if (debugIds.Any())
            {
                End("Режим отладки, синхронизация не выполняналась", true);
                return;
            }

            if (FBSToGVUZ)
            {
                SyncFBSToGVUZ(syncronizationTables);
            }
            bool setCurrentTablesSuccess = SetFBSCurrentTables(syncronizationTables);
            if (setCurrentTablesSuccess)
            {
                End("Синхронизация данных успешно завершена", true);
            }
            else
            {
                End("Синхронизация данных завершена с ошибками", true);
            }
            return;
        }

        private static bool ParseArgs(string[] args, out bool help, out bool repair, out IEnumerable<int> years, out bool ERBDToFBS, out bool FBSToGVUZ, out bool loadCompositions2015, out bool loadCompositions2016Plus, out bool enableDetailedLog, out bool delete_indices, out IEnumerable<Guid> debugIds)
        {
            help = false;
            repair = false;
            ERBDToFBS = true;
            FBSToGVUZ = true;
            loadCompositions2015 = true;
            loadCompositions2016Plus = true;
            enableDetailedLog = false;
            delete_indices = false;

            List<Guid> debugIdsList = new List<Guid>();
            debugIds = debugIdsList;

            List<int> yearsList = new List<int>();
            years = yearsList;

            string comp_now_s = string.Format("compositions={0}", DateTime.Now.Year);

            foreach (string arg in args)
            {
                if (arg.ToLower() == "help")
                {
                    help = true;
                    break;
                }
                else if ((arg.ToLower() == "year") || (arg.ToLower() == "year=now") || (arg.ToLower() == "year=current"))
                {
                    yearsList.Add(DateTime.Now.Year);
                }
                else if (arg.ToLower().StartsWith("year="))
                {
                    string yearsStr = arg.Substring("year=".Length);
                    foreach (string yearStr in yearsStr.Split(',', ';'))
                    {
                        int temp;
                        if (Int32.TryParse(yearStr, out temp))
                        {
                            if ((temp > ReplicationMaxYear) || (temp < ReplicationMinYear))
                            {
                                Logger.WriteLine("Год должен быть в диапазоне от " + ReplicationMinYear.ToString() + " до " + ReplicationMaxYear.ToString());
                                End("Синхронизация данных невозможна", true);

                                ERBDToFBS = false;
                                FBSToGVUZ = false;
                                return false;
                            }
                            else
                            {
                                yearsList.Add(temp);
                            }
                        }
                        else
                        {
                            Logger.WriteLine("Год задан неверно");
                            End("Синхронизация данных невозможна", true);
                            return false;
                        }
                    }
                }
                else if ((arg.ToLower() == "erbd") || (arg.ToLower() == "erbd=yes") || (arg.ToLower() == "erbd=1"))
                {
                    ERBDToFBS = true;
                }
                else if ((arg.ToLower() == "erbd=no") || (arg.ToLower() == "erbd=0"))
                {
                    ERBDToFBS = false;
                }
                else if ((arg.ToLower() == "rvi") || (arg.ToLower() == "rvi=yes") || (arg.ToLower() == "rvi=1"))
                {
                    FBSToGVUZ = true;
                }
                else if ((arg.ToLower() == "rvi=no") || (arg.ToLower() == "rvi=0"))
                {
                    FBSToGVUZ = false;
                }
                else if (arg.ToLower() == "repair")
                {
                    repair = true;
                }
                else if (arg.ToLower() == "compositions=2015")
                {
                    loadCompositions2015 = true;
                    loadCompositions2016Plus = false;
                }
                else if (arg.ToLower() == "compositions=2016" || arg.ToLower() == comp_now_s)
                {
                    loadCompositions2015 = false;
                    loadCompositions2016Plus = true;
                }
                else if ((arg.ToLower() == "compositions=no") || (arg.ToLower() == "compositions=0"))
                {
                    loadCompositions2015 = false;
                    loadCompositions2016Plus = false;
                }
                else if ((arg.ToLower() == "detailslog") || (arg.ToLower() == "detailslog=yes") || (arg.ToLower() == "detailslog=1"))
                {
                    enableDetailedLog = true;
                }
                else if ((arg.ToLower() == "detailslog=no") || (arg.ToLower() == "detailslog=0"))
                {
                    enableDetailedLog = false;
                }
                else if (arg.ToLower().StartsWith("debugids="))
                {
                    enableDetailedLog = true;
                    string idsStr = arg.Substring("debugids=".Length);
                    foreach (string idStr in idsStr.Split(',', ';'))
                    {
                        Guid temp;
                        if (Guid.TryParse(idStr, out temp))
                        {
                            debugIdsList.Add(temp);
                        }
                    }
                }
                else if (arg.ToLower().StartsWith("delete_indices=1") || arg.ToLower().StartsWith("delete_indices=yes"))
                {
                    delete_indices = true;
                }
            }

            if ((repair) || (help))
            {
                ERBDToFBS = false;
                FBSToGVUZ = false;
                loadCompositions2015 = false;
                loadCompositions2016Plus = false;
                debugIdsList.Clear();
            }

            if (!yearsList.Any())
            {
                for (int year = ReplicationDefaultMinYear; year <= DateTime.Now.Year; year++)
                {
                    yearsList.Add(year);
                }
            }

            if (!yearsList.Any(x => x == 2015))
            {
                loadCompositions2015 = false;
            }
            if (!yearsList.Any(x => x >= 2016))
            {
                loadCompositions2016Plus = false;
            }

            return true;
        }

        private static bool Check(bool needsERBD, bool needsGVUZ, bool loadCompositions2015, bool loadCompositions2016Plus)
        {
            if (AlreadyRunning())
            {
                Logger.WriteLine("Процесс переноса данных уже запущен");
                End("Синхронизация данных невозможна", true);
                return false;
            }

            Logger.WriteLine("Проверка доступности БД");
            string errorMessage;

            if (!Connections.TryConnectToFBS(out errorMessage))
            {
                Logger.WriteLine("Не удалось подключиться к БД ФБС: " + errorMessage);
                End("Синхронизация данных невозможна", true);
                return false;
            }
            else
            {
                Logger.WriteLine("Соединение с БД ФБС установлено");
            }

            if (needsERBD)
            {
                if (!Connections.TryConnectToERBD(out errorMessage))
                {
                    Logger.WriteLine("Не удалось подключиться к БД ЕРБД: " + errorMessage);
                    End("Синхронизация данных невозможна", true);
                    return false;
                }
                else
                {
                    Logger.WriteLine("Соединение с БД ЕРБД установлено");
                }

                if (loadCompositions2016Plus)
                {
                    if (!String.IsNullOrEmpty(Connections.CompositionsPagesCountPath2016Plus))
                    {
                        if (!Connections.CheckPagesCountExists(out errorMessage))
                        {
                            string message = string.Format("Каталог: {0}", Connections.CompositionsPagesCountPath2016Plus);
                            Logger.WriteLine(message);
                            Logger.WriteLine("Ошибка чтения каталога для определения числа бланков сочинений за 2016+ год (файлы pagesCount): " + errorMessage);
                            End("Синхронизация данных невозможна", true);
                            return false;
                        }
                    }
                    else if (!Connections.TryOpen2016PlusDirectory(out errorMessage))
                    {
                        Logger.WriteLine("Не удалось подключиться к каталогу сочинений за 2016+ год: " + errorMessage);
                        End("Синхронизация данных невозможна", true);
                        return false;
                    }
                    else
                    {
                        Logger.WriteLine("Доступ к каталогу сочинений за 2016+ год получен");
                    }
                }
                if (loadCompositions2015)
                {
                    if (!Connections.TryOpen2015Directory(out errorMessage))
                    {
                        Logger.WriteLine("Не удалось подключиться к каталогу сочинений за 2015 год: " + errorMessage);
                        End("Синхронизация данных невозможна", true);
                        return false;
                    }
                    else
                    {
                        Logger.WriteLine("Доступ к каталогу сочинений за 2015 год получен");
                    }
                }
            }

            if (needsGVUZ)
            {
                if (!Connections.TryConnectToGVUZ(out errorMessage))
                {
                    Logger.WriteLine("Не удалось подключиться к БД РВИ/ФИС: " + errorMessage);
                    End("Синхронизация данных невозможна", true);
                    return false;
                }
                else
                {
                    Logger.WriteLine("Соединение с БД РВИ/ФИС установлено");
                }
            }

            return true;
        }

        private static bool GetFBSSyncronizationTables(out Tables syncronizationTables)
        {
            bool getCurrentTablesSuccess;
            Tables usingTables = FBSAlterDB.GetCurrentTables(out getCurrentTablesSuccess);
            syncronizationTables = TablesHelper.Reverse(usingTables);

            if (!getCurrentTablesSuccess)
            {
                Logger.WriteLine("При получении информации об используемом комплекте таблиц ФБС произошла ошибка");
                End("Синхронизация данных невозможна", true);
            }

            Logger.WriteLine("Комплект таблиц: " + TablesHelper.ToString(syncronizationTables));
            return getCurrentTablesSuccess;
        }

        private static bool RemoveIndexes(Tables syncronizationTables)
        {
            Logger.WriteLine("Удаление индексов");
            bool indexesDisabled = FBSAlterDB.DisableIndexes(syncronizationTables);
            Logger.WriteLine("Удаление индексов завершено");
            if (!indexesDisabled)
            {
                Logger.WriteLine("При удалении индексов произошла ошибка");
                End("Синхронизация данных невозможна", true);
                return false;
            }
            return true;
        }

        private static bool CreateIndexes(Tables syncronizationTables)
        {
            Logger.WriteLine("Восстановление индексов");
            bool indexesEnabled = FBSAlterDB.EnableIndexes(syncronizationTables);
            Logger.WriteLine("Восстановление индексов завершено");

            if (!indexesEnabled)
            {
                Logger.WriteLine("При восстановлении индексов произошла ошибка, переключение комплекта используемых таблиц производиться не будет");
                End("Синхронизация данных выполнена только в части переноса данных", true);
                return false;
            }
            return true;
        }

        private static bool SetFBSCurrentTables(Tables syncronizationTables)
        {
            Logger.WriteLine("Перенастройка представлений");
            bool setUsingTablesSuccess = FBSAlterDB.SetUsingTables(syncronizationTables);
            Logger.WriteLine("Перенастройка представлений завершена");

            if (!setUsingTablesSuccess)
            {
                Logger.WriteLine("При перенастройке представлений произошла ошибка, переключение комплекта используемых таблиц выполнено некорректно");
                End("Синхронизация данных выполнена только в части переноса данных", true);
                return false;
            }

            FBSAlterDB.SaveCurrentTables(syncronizationTables);
            return true;
        }

        private static void SyncERBDToFBS(Tables syncronizationTables, IEnumerable<int> selectedYears, bool loadCompositions2015, bool loadCompositions2016Plus, IEnumerable<Guid> debugIds)
        {
            string syncName = debugIds.Any() ? "ОТЛАДКА СИНХРОНИЗАЦИИ ДАННЫХ ЕРБД - ФБС" : "СИНХРОНИЗАЦИЯ ДАННЫХ ЕРБД - ФБС";
            Logger.WriteLine(syncName);

            ERBDToFBSReplicator replicator = new ERBDToFBSReplicator(syncronizationTables, loadCompositions2015, loadCompositions2016Plus, debugIds);

            foreach (int year in selectedYears.OrderBy(x => x))
            {
                Logger.SetPrefix("ERBDToFBS_" + year.ToString());
                Logger.WriteLine(syncName);

                bool replicationSuccess = replicator.Replicate(year);
                GC.Collect();
                if ((!replicationSuccess) && (replicator.HasCriticalErrors))
                {
                    Logger.SetPrefix(null);
                    End(syncName + " невозможна", false);
                    return;
                }
            }

            Logger.SetPrefix(null);
            Logger.WriteLine(syncName + " успешно завершена");
        }

        private static void SyncFBSToGVUZ(Tables syncronizationTables)
        {
            string syncName = "СИНХРОНИЗАЦИЯ ДАННЫХ ФБС - РВИ";
            Logger.WriteLine(syncName);

            FBSToGVUZReplicator replicator = new FBSToGVUZReplicator(syncronizationTables);

            Logger.SetPrefix("FBSToGVUZ");
            Logger.WriteLine(syncName);

            bool replicationSuccess = replicator.Replicate();
            GC.Collect();
            if ((!replicationSuccess) && (replicator.HasCriticalErrors))
            {
                Logger.SetPrefix(null);
                End(syncName + " невозможна", false);
                return;
            }
             
            Logger.SetPrefix(null);
            Logger.WriteLine(syncName + " успешно завершена");
        }

        private static bool AlreadyRunning()
        {
            int count = 0;
            foreach (Process process in Process.GetProcesses())
            {
                if (process.ProcessName.Contains("FBS.Replicator"))
                {
                    count++;
                }
            }
            if (count >= 2)
                return true;
            return false;
        }

        private static void End(string message, bool close)
        {
            if (!String.IsNullOrEmpty(message))
            {
                Logger.WriteLine(message.ToUpper());
            }
            if (close)
            {
                Console.WriteLine("Приложение будет закрыто автоматически через 2 минуты");
                System.Threading.Thread.Sleep(2 * 60 * 1000);
            }
        }

        private const int ReplicationDefaultMinYear = 2011;
        private static int ReplicationMinYear { get { return ReplicationDefaultMinYear - 2; } }
        private static int ReplicationMaxYear { get { return DateTime.Now.Year + 2; } }
    }
}
