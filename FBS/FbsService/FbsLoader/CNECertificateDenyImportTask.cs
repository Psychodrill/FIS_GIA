using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Configuration;

namespace FbsService.FbsLoader
{
    public class CNECertificateDenyImportTask : Task
    {
        class Search : TaskStatus
        {
            public const string StatusCode = "search";

            static string LoadingDirectory = ConfigurationManager.AppSettings["CertificateDenyLoadingDirectory"];
            static bool IsAutoLoading = Boolean.Parse(
                    ConfigurationManager.AppSettings["AutoCertificateDenyLoading"]);
            static long LoaderAccountId = long.Parse(ConfigurationManager.AppSettings["LoaderAccountId"]);
            static string LoaderIp = ConfigurationManager.AppSettings["LoaderIp"];

            private CNECertificateDenyImportTask CurrentTask
            {
                get
                {
                    return (CNECertificateDenyImportTask)Task;
                }
            }

            protected internal override string GetStatusCode()
            {
                return StatusCode;
            }

            protected internal override void Execute()
            {
                LogInfo("###Search - Execute###");
                CNEDenyLoadingTask.RefreshTasks(Directory.GetFiles(LoadingDirectory, "*.csv").ToArray());
                if (IsAutoLoading)
                    CNEDenyLoadingTask.ActivateLoadingTaskAuto(LoaderAccountId, LoaderIp);

                CNEDenyLoadingTask loadingTask = CNEDenyLoadingTask.GetProcessTask();
                if (loadingTask == null)
                {
                    LogInfo("###Search - Execute. return###");
                    return;
                }

                CurrentTask.LoadingTaskId = loadingTask.Id;
                CurrentTask.LoadingBatchUrl = loadingTask.SourceBatchUrl;
                CurrentTask.EditorAccountId = loadingTask.EditorAccountId;
                CurrentTask.EditorIp = loadingTask.EditorIp;
                CurrentTask.UpdateId = loadingTask.UpdateId;
                CurrentTask.InitFileNames();
                SetStatus(ParseData.StatusCode);
                LogInfo("###Search - Execute. SetStatus###");
            }
        }

        class ParseData : TaskStatus
        {
            public const string StatusCode = "parse";

            static string LogFileName = ConfigurationManager.AppSettings["CertificateDenyLoadingLog"];
            static int MaxErrorCount = Int32.Parse(ConfigurationManager.AppSettings["CertificateDenyLoadingMaxErrorCount"]);
            
            private CNECertificateDenyImportTask CurrentTask
            {
                get
                {
                    return (CNECertificateDenyImportTask)Task;
                }
            }

            protected internal override string GetStatusCode()
            {
                return StatusCode;
            }

            private static void WriteLog(string errorMessage, int? lineIndex)
            {
                using (FileStream stream = File.Open(LogFileName, FileMode.Append, FileAccess.Write,
                        FileShare.ReadWrite))
                using (StreamWriter writer = new StreamWriter(stream, Encoding.GetEncoding(1251)))
                    if (lineIndex != null)
                        writer.WriteLine(string.Format("{0:u}. Cтрока {2}: {1}", DateTime.Now, errorMessage, lineIndex));
                    else
                        writer.WriteLine(string.Format("{0:u}. {1}", DateTime.Now, errorMessage));
            }

            private void AddError(string errorMessage, int lineIndex)
            {
                CNEDenyLoadingTask.AddError((long)CurrentTask.LoadingTaskId, lineIndex, errorMessage);
                WriteLog(errorMessage, lineIndex);
            }

            private static string ParseNewNumber(string comment)
            {
                string result = Regex.Replace(comment, 
                    @"Аннулировано в связи с внесением изменений. Актуальное свидетельство: (?<region>\d{2})-(?<number>\d{9})-(?<year>\d{2})",
                    "${region}-${number}-${year}");
                if (result == comment)
                    return null;
                return result;
            }

            /// <summary>
            /// Извлечение года из номера сертификата.
            /// Например: 59-000285186-08 -> 2008
            /// </summary>
            /// <param name="number"></param>
            /// <returns></returns>
            private static string ExtractYearFromNumber(string number)
            {
                try
                {
                    return (int.Parse(number.Split(new[] { '-' }, 3)[2]) + 2000).ToString();
                }
                catch (Exception)
                {
                    return DateTime.Now.Year.ToString();
                }
            }

            public void AddCertificateDeny(string certificateNumber, string comment, string newCertificateNumber)
            {
                if (IsBulkLoading)
                {
                    long id = long.Parse(Regex.Replace(certificateNumber, 
                            @"(?<region>\d{2})-(?<number>\d{9})-(?<year>\d{2})",
                            @"${year}${region}${number}"));
                    DenyBulkStreamWriter.WriteLine(
                            "{0}\t{1:u}\t{1:u}\t{2}\t{3}\t{4}\t{5}\t{6}",
                                id, DateTime.Now, CurrentTask.UpdateId, ExtractYearFromNumber(certificateNumber), certificateNumber,
                                comment.Replace('\t', ' '), newCertificateNumber);
                }
                else
                    CNEDenyLoadingTask.AddCertificateDeny(certificateNumber, comment.Replace('\t', ' '), 
                            newCertificateNumber, CurrentTask.UpdateId, CurrentTask.EditorAccountId, 
                            CurrentTask.EditorIp);
                long certificateId = GetCertificateId(certificateNumber);
                mCertificateIds.Add(certificateId, certificateId);
            }

            private FileStream SourceFileStream = null;

            private FileStream DenyBulkFileStream = null;
            private StreamWriter DenyBulkStreamWriter = null;

            private Hashtable mCertificateIds = new Hashtable();

            private string archiveFileDirectory;
            private string ArchiveFolder
            {
                get
                {
                    if (string.IsNullOrEmpty(archiveFileDirectory))
                        archiveFileDirectory = ConfigurationManager.AppSettings["ArchiveDirectory"];
                    return archiveFileDirectory;
                }
            }

            private string GetArchiveFileName(string fileName)
            {

                return Path.Combine(ArchiveFolder, Path.GetFileName(fileName));
            }            

            private bool BeginParse()
            {
                try
                {
                    string archiveFilename = GetArchiveFileName(CurrentTask.LoadingBatchUrl);
                    WriteLog(string.Format("Копируем файл из {0} к себе в {1}", CurrentTask.LoadingBatchUrl, archiveFilename), null);
                    File.Copy(CurrentTask.LoadingBatchUrl, archiveFilename, true);
                    WriteLog(string.Format("Удаляем файл {0}", CurrentTask.LoadingBatchUrl), null);
                    File.Delete(CurrentTask.LoadingBatchUrl);
                    SourceFileStream = File.Open(archiveFilename, FileMode.Open, FileAccess.Read, FileShare.None);

                    if (IsBulkLoading)
                    {
                        DenyBulkFileStream = File.Open(CurrentTask.DenyBulkFileName, FileMode.Create,
                                FileAccess.Write, FileShare.ReadWrite);
                        DenyBulkStreamWriter = new StreamWriter(DenyBulkFileStream, Encoding.GetEncoding(1251));
                    }
                }
                catch
                {
                    WriteLog(string.Format("Не удалось начать обработку файла: {0}", CurrentTask.LoadingBatchUrl), null);
                    return false;
                }
                return true;
            }

            private void EndParse()
            {
                try
                {
                    if (IsBulkLoading)
                    {
                        if (DenyBulkStreamWriter != null)
                        {
                            DenyBulkStreamWriter.Close();
                            DenyBulkStreamWriter.Dispose();
                            DenyBulkStreamWriter = null;
                        }
                        if (DenyBulkFileStream != null)
                        {
                            DenyBulkFileStream.Close();
                            DenyBulkFileStream.Dispose();
                            DenyBulkFileStream = null;
                        }
                        if (File.Exists(CurrentTask.DenyBulkFileName))
                            try
                            {
                                CNEDenyLoadingTask.ImportCertificateDenyes(CurrentTask.DenyBulkFileName);
                            }
                            catch (Exception ex)
                            {
                                this.LogException(ex);
                            }
                    }
                    mCertificateIds.Clear();
                    mCertificateIds = null;
                }
                finally
                {
                    if (SourceFileStream != null)
                    {
                        SourceFileStream.Dispose();
                        SourceFileStream = null;
                    }
                    GC.Collect();
                }
            }

            private static long GetCertificateId(string number)
            {
                return long.Parse(Regex.Replace(number, @"(?<region>\d{2})-(?<number>\d{9})-(?<year>\d{2})",
                        @"${year}${region}${number}"));
            }

            private bool HasCertificateNumber(string number)
            {
                long id = GetCertificateId(number);
                return mCertificateIds.ContainsKey(id);
            }

            private bool Parse(out bool isCorrectBatch)
            {
                isCorrectBatch = true;
                CNEDenyLoadingTask.PrepareLoading();

                //if (File.Exists(LogFileName))
                //    File.Delete(LogFileName);

                try
                {
                    if (!BeginParse())
                        return false;
                    WriteLog(string.Format("Начата обработка {0}", CurrentTask.LoadingBatchUrl), null);
                    using (StreamReader reader = new StreamReader(SourceFileStream, Encoding.GetEncoding(1251)))
                    {
                        string line;
                        int lineIndex = 0;
                        int errorCount = 0;

                        while ((line = reader.ReadLine()) != null)
                        {
                            string certificateNumber = null;
                            string comment = null;
                            string newCertificateNumber = null;

                            line = line.Trim();
                            lineIndex++;

                            if (string.IsNullOrEmpty(line))
                                continue;
                            string[] parts = line.Split('#');
                            string errorMessage = null;
                            
                            if (parts.Length != 3)
                                errorMessage = "(00) Некорректное количество параметров";

                            if (errorMessage == null)
                            {
                                certificateNumber = parts[0].Trim();
                                if (!Regex.IsMatch(certificateNumber, @"\d{2}-\d{9}-\d{2}"))
                                    errorMessage = "(01) Некорректный формат номера свидетельства";
                                else if (HasCertificateNumber(certificateNumber))
                                    errorMessage = string.Format("(02) Повтор номера свидетельства '{0}'", parts[0]);
                            }
                            if (errorMessage == null)
                            {
                                if (!string.IsNullOrEmpty(parts[1].Trim()))
                                    comment = parts[1].Trim();
                                else
                                    errorMessage = "(03) Не указана причина аннулирования";
                            }
                            if (errorMessage == null)
                            {
                                newCertificateNumber = parts[2].Trim();
                                if (!string.IsNullOrEmpty(newCertificateNumber) && !Regex.IsMatch(newCertificateNumber, @"\d{2}-\d{9}-\d{2}"))
                                    errorMessage = "(04) Некорректный формат нового номера свидетельства";
                            }

                            if (errorMessage != null)
                            {
                                AddError(errorMessage, lineIndex);
                                errorCount++;
                                if (errorCount < MaxErrorCount)
                                    continue;

                                WriteLog(string.Format(
                                        "(99) Достигнут порог максимального количества ошибок ({0}). Обработка прервана.",
                                        MaxErrorCount), null);
                                isCorrectBatch = false;
                                return false;
                            }

                            AddCertificateDeny(certificateNumber, comment, newCertificateNumber);
                        }
                    }
                    WriteLog("Данные обработаны успешно.", null);
                }
                finally
                {
                    EndParse();
                }
                return true;
            }

            protected internal override void Execute()
            {
                bool isCorrectBatch;
                if (!Parse(out isCorrectBatch))
                {
                    if (!isCorrectBatch)
                    {
                        CurrentTask.StoreResults(false);
                        CNEDenyLoadingTask.DeactivateTask((long)CurrentTask.LoadingTaskId);
                        SetStatus(Search.StatusCode);
                        CurrentTask.Clear();
                    }
                    return;
                }
                SetStatus(ExecuteLoad.StatusCode);
            }
        }

        class ExecuteLoad : TaskStatus
        {
            public const string StatusCode = "execute";

            private CNECertificateDenyImportTask CurrentTask
            {
                get
                {
                    return (CNECertificateDenyImportTask)Task;
                }
            }

            protected internal override string GetStatusCode()
            {
                return StatusCode;
            }

            protected internal override void Execute()
            {
                LogInfo("###Execute - Execute###");
                CNEDenyLoadingTask.ExecuteTask((long)CurrentTask.LoadingTaskId);
                CurrentTask.StoreResults(true);

                SetStatus(Search.StatusCode);
                CurrentTask.Clear();
            }
        }

        public const string CertificateDenyBulkFileCode = "CertificateDeny";
        
        static bool IsBulkLoading = Boolean.Parse(ConfigurationManager.AppSettings["BulkLoading"]);

        private string DenyBulkFileNameFormat = "CertificateDeny_{0:yyMMdd_HHmm}.dat";
        
        public string DenyBulkFileName = null;
        
        private void InitFileNames()
        {
            DenyBulkFileName = Path.Combine(BulkFile.Directory,
                    string.Format(DenyBulkFileNameFormat, DateTime.Now));
        }

        private void Clear()
        {
            DenyBulkFileName = null;
            LoadingTaskId = null;
            LoadingBatchUrl = null;
            EditorAccountId = null;
            EditorIp = null;
            UpdateId = null;
        }

        protected override string GetTaskCode()
        {
            return "CommonNationalExamCertificateDenyImport";
        }

        protected internal override TaskStatus GetStatus(string code)
        {
            if (LoadingTaskId != null)
                if (code == ParseData.StatusCode)
                    return new ParseData();
                else if (code == ExecuteLoad.StatusCode)
                    return new ExecuteLoad();
            return new Search();
        }

        private void StoreResults(bool success)
        {
            // TODO
            if (IsBulkLoading)
                if (success)
                    BulkFile.AppendBulkFile(CertificateDenyBulkFileCode, DenyBulkFileName);
                else
                    File.Delete(DenyBulkFileName);
        }

        public long? LoadingTaskId = null;
        public string LoadingBatchUrl = null;
        public long? EditorAccountId = null;
        public string EditorIp = null;
        public Guid? UpdateId = null;
    }
}
