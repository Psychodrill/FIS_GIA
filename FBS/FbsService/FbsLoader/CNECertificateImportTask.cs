using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using System.Text.RegularExpressions;

namespace FbsService.FbsLoader
{
    public class CNECertificateImportTask : Task
    {
        class Search : TaskStatus
        {
            public const string StatusCode = "search";

            static string LoadingDirectory = ConfigurationManager.AppSettings["CertificateLoadingDirectory"];
            static bool IsAutoLoading = Boolean.Parse(
                    ConfigurationManager.AppSettings["AutoCertificateLoading"]);
            static long LoaderAccountId = long.Parse(ConfigurationManager.AppSettings["LoaderAccountId"]);
            static string LoaderIp = ConfigurationManager.AppSettings["LoaderIp"];
            
            private CNECertificateImportTask CurrentTask
            {
                get
                {
                    return (CNECertificateImportTask)Task;
                }
            }

            protected internal override string GetStatusCode()
            {
                return StatusCode;
            }

            protected internal override void Execute()
            {
                CNELoadingTask.RefreshTasks(Directory.GetFiles(LoadingDirectory, "*.csv").ToArray());
                if (IsAutoLoading)
                    CNELoadingTask.ActivateLoadingTaskAuto(LoaderAccountId, LoaderIp);

                CNELoadingTask loadingTask = CNELoadingTask.GetProcessTask();
                if (loadingTask == null)
                    return;

                CurrentTask.LoadingTaskId = loadingTask.Id;
                CurrentTask.LoadingBatchUrl = loadingTask.SourceBatchUrl;
                CurrentTask.EditorAccountId = loadingTask.EditorAccountId;
                CurrentTask.EditorIp = loadingTask.EditorIp;
                CurrentTask.UpdateId = loadingTask.UpdateId;
                CurrentTask.InitFileNames();
                SetStatus(ParseData.StatusCode);
            }
        }

        class ParseData : TaskStatus
        {
            public const string StatusCode = "parse";

            static string LogFileName = ConfigurationManager.AppSettings["CertificateLoadingLog"];
            static int MaxErrorCount = Int32.Parse(ConfigurationManager.AppSettings["CertificateLoadingMaxErrorCount"]);
            
            private static int[] SubjectIds = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 
                    12, 13, 14 };

            private static string[] SubjectNames = new string[] { "русский язык", "математика", "физика", "химия",
                    "биология", "история России", "география", "английский язык", "немецкий язык", "французский язык", 
                    "обществознание", "литература", "испанский язык", "информатика" };

            private CNECertificateImportTask CurrentTask
            {
                get
                {
                    return (CNECertificateImportTask)Task;
                }
            }

            protected internal override string GetStatusCode()
            {
                return StatusCode;
            }

            private void WriteLog(string errorMessage, int? lineIndex)
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
                CNELoadingTask.AddError((long)CurrentTask.LoadingTaskId, lineIndex, errorMessage);
                WriteLog(errorMessage, lineIndex);
            }

            private int ExtraxtNumberRegionId(string number)
            {
                return Convert.ToInt32(Regex.Replace(number,
                    @"(?<region>\d{2})-(?<number>\d{9})-(?<year>\d{2})", "${region}"));
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

            private void AddCertificate(out long? id, string number, string educationInstitutionCode, string lastName,
                string firstName, string patronymicName, bool sex, string @class, string passportSeria,
                string passportNumber, string entrantNumber, int regionId, string typographicNumber, string year)
            {
                if (IsBulkLoading)
                {
                    id = GetCertificateId(number);
                    CertificateBulkStreamWriter.WriteLine(
                        "{0}\t{1:u}\t{1:u}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\t{13}\t{14}\t{15}\t{16}\t{17}\t{18}",
                            id, DateTime.Now, CurrentTask.UpdateId, CurrentTask.EditorAccountId, CurrentTask.EditorIp,
                            number, educationInstitutionCode, year, lastName, firstName, patronymicName,
                            sex ? 1 : 0, @class, passportSeria.Replace(" ", ""), passportSeria,
                            passportNumber, entrantNumber, regionId, typographicNumber);
                    mCertificateIds.Add(id, id);
                }
                else
                {
                    CNELoadingTask.AddCertificate(out id, number, educationInstitutionCode, lastName, firstName, patronymicName,
                            sex, @class, passportSeria, passportNumber, entrantNumber, regionId, CurrentTask.UpdateId,
                            CurrentTask.EditorAccountId, CurrentTask.EditorIp, typographicNumber);
                    long internalId = GetCertificateId(number);
                    mCertificateIds.Add(internalId, internalId);
                }
            }

            private void AddCertificateSubject(long certificateId, int regionId, int subjectId, float mark, bool hasAppeal, string year)
            {
                if (IsBulkLoading)
                {
                    long id = certificateId * 100 + subjectId;
                    CertificateSubjectBulkStreamWriter.WriteLine(
                            "{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}",
                            id, certificateId, subjectId, mark.ToString(CultureInfo.InvariantCulture.NumberFormat), hasAppeal ? 1 : 0, year, regionId);
                }
                else
                    CNELoadingTask.AddCertificateSubject(certificateId, regionId, subjectId, mark, hasAppeal);
            }

            private FileStream SourceFileStream = null;
            
            private FileStream CertificateBulkFileStream = null;
            private StreamWriter CertificateBulkStreamWriter = null;

            private FileStream CertificateSubjectBulkFileStream = null;
            private StreamWriter CertificateSubjectBulkStreamWriter = null;

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
                        CertificateBulkFileStream = File.Open(CurrentTask.CertificateBulkFileName, FileMode.Create, 
                                FileAccess.Write, FileShare.ReadWrite);
                        CertificateBulkStreamWriter = new StreamWriter(CertificateBulkFileStream, Encoding.GetEncoding(1251));
                        CertificateSubjectBulkFileStream = File.Open(CurrentTask.CertificateSubjectBulkFileName, 
                                FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                        CertificateSubjectBulkStreamWriter = new StreamWriter(CertificateSubjectBulkFileStream,
                                Encoding.GetEncoding(1251));
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
                        if (CertificateSubjectBulkStreamWriter != null)
                        {
                            CertificateSubjectBulkStreamWriter.Close();
                            CertificateSubjectBulkStreamWriter.Dispose();
                            CertificateSubjectBulkStreamWriter = null;
                        }
                        if (CertificateSubjectBulkFileStream != null)
                        {
                            CertificateSubjectBulkFileStream.Close();
                            CertificateSubjectBulkFileStream.Dispose();
                            CertificateSubjectBulkFileStream = null;
                        }
                        if (CertificateBulkStreamWriter != null)
                        {
                            CertificateBulkStreamWriter.Close();
                            CertificateBulkStreamWriter.Dispose();
                            CertificateBulkStreamWriter = null;
                        }
                        if (CertificateBulkFileStream != null)
                        {
                            CertificateBulkFileStream.Close();
                            CertificateBulkFileStream.Dispose();
                            CertificateBulkFileStream = null;
                        }
                        if (File.Exists(CurrentTask.CertificateBulkFileName))
                            try
                            {
                                CNELoadingTask.ImportCertificates(CurrentTask.CertificateBulkFileName);
                            }
                            catch (Exception ex)
                            {
                                this.LogException(ex);
                            }
                        if (File.Exists(CurrentTask.CertificateSubjectBulkFileName))
                            try
                            {
                                CNELoadingTask.ImportCertificateSubjects(CurrentTask.CertificateSubjectBulkFileName);
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

            private long GetCertificateId(string number)
            {
                return long.Parse(Regex.Replace(number, @"(?<region>\d{2})-(?<number>\d{9})-(?<year>\d{2})",
                        @"${year}${region}${number}"));
            }

            private bool HasCertificateNumber(string number)
            {
                long id = GetCertificateId(number);
                return mCertificateIds.ContainsKey(id);
            }

            /// <summary>
            /// Спецификация CSV файла
            /// </summary>
            private enum CSV
            {
                /// <summary>
                /// 0
                /// </summary>
                CertificateNumber,
                /// <summary>
                /// 1
                /// </summary>
                EducationInstitutionCode,
                /// <summary>
                /// 2
                /// </summary>
                Sex,
                /// <summary>
                /// 3
                /// </summary>
                LastName,
                /// <summary>
                /// 4
                /// </summary>
                FirstName,
                /// <summary>
                /// 5
                /// </summary>
                PatronymicName,
                /// <summary>
                /// 6
                /// </summary>
                Skip,
                /// <summary>
                /// 7
                /// </summary>
                PassportSeria,
                /// <summary>
                /// 8
                /// </summary>
                PassportNumber,
                /// <summary>
                /// 9
                /// </summary>
                Class,
                /// <summary>
                /// 10
                /// </summary>
                BeginMarks,
                /// <summary>
                /// 37
                /// </summary>
                EndMarks = 37, //зарезервированно под оценки 10-37,
                /// <summary>
                /// 38
                /// </summary>
                EntrantNumber,
                /// <summary>
                /// 39
                /// </summary>
                RegionId,
                /// <summary>
                /// 40
                /// </summary>
                TypographicNumber
            } ;

            private bool Parse(out bool isCorrectBatch)
            {
                isCorrectBatch = true;
                CNELoadingTask.PrepareLoading();
                
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

                        long? id;

                        while ((line = reader.ReadLine()) != null)
                        {
                            line = line.Trim();
                            lineIndex++;
                            
                            if (string.IsNullOrEmpty(line))
                                continue;

                            try
                            {
                                string[] parts = line.Split('#');

                                if (parts.Length != 41)
                                    throw new Exception("(00) Некорректное количество параметров (" + parts.Length.ToString() + ")");


                                string number = parts[(int)CSV.CertificateNumber].Trim();
                                if (string.IsNullOrEmpty(number))
                                    throw new Exception("(01) Не указан номер свидетельства");
                                if (!Regex.IsMatch(number, @"\d{2}-\d{9}-\d{2}"))
                                    throw new Exception(string.Format(
                                                            "(02) Значение '{0}' в поле номер свидетельства не соответствует формату.",
                                                            parts[(int) CSV.CertificateNumber]));
                                if (HasCertificateNumber(number))
                                    throw new Exception(
                                        string.Format("(03) Повтор номера свидетельства '{0}'", number));

                                string educationInstitutionCode = parts[(int)CSV.EducationInstitutionCode];

                                bool sex;
                                if (string.Compare(parts[(int)CSV.Sex].Trim(), "М", true) == 0)
                                    sex = true;
                                else if (string.Compare(parts[(int)CSV.Sex].Trim(), "Ж", true) == 0)
                                    sex = false;
                                else
                                    throw new Exception(string.Format("(04) Некорректное значение '{0}' в поле пол", parts[(int)CSV.Sex]));

                                string lastName = parts[(int)CSV.LastName].Trim();
                                if (string.IsNullOrEmpty(lastName))
                                    throw new Exception("(10) Не указана фамилия");

                                string firstName = parts[(int)CSV.FirstName].Trim();
                                //if (string.IsNullOrEmpty(firstName))
                                //    throw new Exception("(11) Не указано имя");

                                string patronymicName = parts[(int)CSV.PatronymicName].Trim();
                                //if (string.IsNullOrEmpty(patronymicName))
                                //    throw new Exception("(12) Не указано отчество");

                                string passportSeria = parts[(int)CSV.PassportSeria].Trim();
                                //if (string.IsNullOrEmpty(passportSeria))
                                //    throw new Exception("(13) Не указана серия паспорта");

                                string passportNumber = parts[(int)CSV.PassportNumber].Trim();
                                if (string.IsNullOrEmpty(passportNumber))
                                    throw new Exception("(05) Не указан номер паспорта");

                                string @class = parts[(int)CSV.Class].Trim();
                                bool hasMarks = false;
                                float mark;
                                for (int i = (int)CSV.BeginMarks; i <= (int)CSV.EndMarks; i += 2)
                                    if (!string.IsNullOrEmpty(parts[i].Trim()))
                                    {
                                        if (!float.TryParse(parts[i].Trim(), NumberStyles.Number, CultureInfo.InvariantCulture.NumberFormat, out mark) 
                                                || mark < 0 || mark > 100)
                                            throw new Exception(string.Format(
                                                "(06) Некорректное значение '{1}' в поле баллов для предмета '{0}'",
                                                SubjectNames[(i - 10) / 2], parts[i]));
                                        if (parts[i + 1].Trim() != "0" && parts[i + 1].Trim() != "1")
                                            throw new Exception(string.Format(
                                                                    "(07) Некорректное значение '{1}' в поле апелляция для предмета '{0}'",
                                                                    SubjectNames[(i - 10) / 2], parts[i + 1]));
                                        hasMarks = hasMarks || mark >= 0;
                                    }

                                if (!hasMarks)
                                    AddError("(08) Предупреждение: не указаны баллы ни по одному предмету", lineIndex);
                                string entrantNumber = parts[(int)CSV.EntrantNumber];
                                int regionId;
                                if (!int.TryParse(parts[(int)CSV.RegionId].Trim(), out regionId))
                                    throw new Exception(string.Format("(09) Некорректное значение '{0}' в поле кода региона", parts[(int)CSV.RegionId]));
                                // TODO: Когда будут присылаться корректные номера регионов, сделать преобразование 
                                // пришедшего региона и проверку с регионом, записанным в номере.
                                regionId = ExtraxtNumberRegionId(number);

                                string typographicNumber = parts[(int) CSV.TypographicNumber].Trim();

                                string year = ExtractYearFromNumber(number);
                                AddCertificate(out id, number, educationInstitutionCode, lastName, firstName,
                                    patronymicName, sex, @class, passportSeria, passportNumber, entrantNumber, regionId, typographicNumber, year);

                                for (int i = (int)CSV.BeginMarks; i <= (int)CSV.EndMarks; i += 2)
                                    if (!string.IsNullOrEmpty(parts[i].Trim()) &&
                                            float.TryParse(parts[i].Trim(), NumberStyles.Number, CultureInfo.InvariantCulture.NumberFormat, out mark)
                                            && mark >= 0 && mark <= 100)
                                    {
                                        bool hasAppeal = parts[i + 1].Trim() == "1";
                                        AddCertificateSubject((long)id, regionId, SubjectIds[(i - 10) / 2], mark, hasAppeal, year);
                                    }
                            }
                            catch (Exception ex)
                            {
                                AddError(ex.Message, lineIndex);
                                errorCount++;
                                if (errorCount < MaxErrorCount)
                                    continue;
                                WriteLog(string.Format(
                                        "(99) Достигнут порог максимального количества ошибок ({0}). Обработка прервана.",
                                        MaxErrorCount), null);
                                isCorrectBatch = false;
                                return false;
                            }

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
                        CNELoadingTask.DeactivateTask((long)CurrentTask.LoadingTaskId);
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

            private CNECertificateImportTask CurrentTask
            {
                get
                {
                    return (CNECertificateImportTask)Task;
                }
            }

            protected internal override string GetStatusCode()
            {
                return StatusCode;
            }

            protected internal override void Execute()
            {
                CNELoadingTask.ExecuteTask((long)CurrentTask.LoadingTaskId);
                CurrentTask.StoreResults(true);

                SetStatus(Search.StatusCode);
                CurrentTask.Clear();
            }
        }

        public const string CertificateBulkFileCode = "Certificate";
        public const string CertificateSubjectBulkFileCode = "CertificateSubject";

        static bool IsBulkLoading = Boolean.Parse(ConfigurationManager.AppSettings["BulkLoading"]);

        private string CertificateBulkFileNameFormat = "Certificates_{0:yyMMdd_HHmm}.dat";
        private string CertificateSubjectBulkFileNameFormat = "CertificateSubjects_{0:yyMMdd_HHmm}.dat";

        public string CertificateBulkFileName = null;

        public string CertificateSubjectBulkFileName = null;

        private void InitFileNames()
        {
            CertificateBulkFileName = Path.Combine(BulkFile.Directory,
                    string.Format(CertificateBulkFileNameFormat, DateTime.Now));
            CertificateSubjectBulkFileName = Path.Combine(BulkFile.Directory,
                    string.Format(CertificateSubjectBulkFileNameFormat, DateTime.Now));
        }

        private void Clear()
        {
            CertificateBulkFileName = null;
            CertificateSubjectBulkFileName = null;
            LoadingTaskId = null;
            LoadingBatchUrl = null;
            EditorAccountId = null;
            EditorIp = null;
            UpdateId = null;
        }

        protected override string GetTaskCode()
        {
            return "CommonNationalExamCertificateImport";
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
                {
                    BulkFile.AppendBulkFile(CertificateBulkFileCode, CertificateBulkFileName);
                    BulkFile.AppendBulkFile(CertificateSubjectBulkFileCode, 
                            CertificateSubjectBulkFileName);
                }
                else
                {
                    if (File.Exists(CertificateBulkFileName))
                        File.Delete(CertificateBulkFileName);
                    if (File.Exists(CertificateSubjectBulkFileName))
                        File.Delete(CertificateSubjectBulkFileName);
                }
        }

        public long? LoadingTaskId = null;
        public string LoadingBatchUrl = null;
        public long? EditorAccountId = null;
        public string EditorIp = null;
        public Guid? UpdateId = null;
    }
}
