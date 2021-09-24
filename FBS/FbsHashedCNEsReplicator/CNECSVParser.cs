using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using Fbs.Core.Loggers;
using Fbs.Utility;
using FbsHashedCNEsReplicator.Properties;

namespace FbsHashedCNEsReplicator
{
    public static class CNECSVParser
    {
        const int MaxErrorCount = 50;
        const string ArchiveFolderName = "Archive";

       static  string  GetArchiveFolderPath( )
        {
            return Settings.Default.LoadFolder + "\\" + ArchiveFolderName;
        }

        public static void ParseFile(string fileName, string connectionString, ILogger logger)
        {
            string InternalFileName=GetArchiveFolderPath() + "\\" + Path.GetFileName(fileName);
            if (File.Exists(InternalFileName))
            {
                logger.WriteMessage("File '" + fileName + "' already processed");
                return;
            }

            File.Copy(fileName, InternalFileName);//Скопировать в нашу папку (отсюда будет идти обработка)
            logger.WriteMessage("File '" + fileName + "' copied to archive folder");
            //File.Delete(fileName);
            //logger.WriteMessage("Input file deleted");

            FileStream SourceFileStream = File.Open(InternalFileName, FileMode.Open, FileAccess.Read, FileShare.None);
            
            using (StreamReader FileReader = new StreamReader(SourceFileStream, Encoding.GetEncoding(1251)))
            {
                DateTime OperationStart = DateTime.Now;
                string CSVLine;
                int LineIndex = 0;
                int ErrorCount = 0;

                CNEDataAccessor DA = new CNEDataAccessor(connectionString);
                DA.OpenConnection();
                DA.CreateTempTables();
                logger.WriteMessage("DB Connection Opened " + DateTime.Now.ToLongTimeString());

                double  DBSpan = 0;
                double ParseSpan = 0;
                bool CriticalErrors = false;
                while ((CSVLine = FileReader.ReadLine()) != null)
                {
                    LineIndex++;
                    if (string.IsNullOrEmpty(CSVLine))
                        continue;

                    DateTime ParseStart = DateTime.Now;
                    CNE CNEFromLine = ParseLine(CSVLine);
                    ParseSpan += DateTime.Now.Subtract(ParseStart).Milliseconds;
                    if (CNEFromLine.ValidationSummary.ResultType == ValidationResultType.WrongFormat)
                    {
                        logger.WriteMessage("File format is incorrect, processing stopped");
                        CriticalErrors = true;
                        break;
                    }
                    else 
                    if (CNEFromLine.ValidationSummary.ResultType == ValidationResultType.Valid)
                    {
                        DateTime DBStart = DateTime.Now;
                        DA.AddToDB(CNEFromLine); //Add to DB
                        DBSpan += DateTime.Now.Subtract(DBStart).Milliseconds;
                    }
                    else if (CNEFromLine.ValidationSummary.ResultType == ValidationResultType.Incomplete)
                    {
                        DateTime DBStart = DateTime.Now;
                        DA.AddToDB(CNEFromLine); //Add to DB, write warning
                        DBSpan += DateTime.Now.Subtract(DBStart).Milliseconds;
                    }
                    else
                    {
                        ErrorCount++;
                        logger.WriteMessage("Error found as line " + LineIndex + CNEFromLine.ValidationSummary.Message);
                    }
                    if (LineIndex % 10000 == 0)
                    {
                        logger.WriteMessage(LineIndex.ToString() + " Lines processed " + DateTime.Now.ToLongTimeString());
                        logger.WriteMessage(" Average insert time is " + (DBSpan / 10000).ToString("0.##") + " ms");
                        logger.WriteMessage(" Average parse time is " + (ParseSpan / 10000).ToString("0.##") + " ms");

                        DBSpan = 0;
                        ParseSpan = 0;
                    }
                    if (ErrorCount >= MaxErrorCount)
                    {
                        logger.WriteMessage("Maximum number of errors reached");
                        CriticalErrors = true;
                        break;
                    }
                }
                if (!CriticalErrors)
                {
                    logger.WriteMessage(LineIndex.ToString() + " Lines processed " + DateTime.Now.ToLongTimeString());

                    logger.WriteMessage("Temporary tables replication started " + DateTime.Now.ToLongTimeString());

                    DA.ReplicateTables(logger);

                    logger.WriteMessage("Temporary tables replication succeed " + DateTime.Now.ToLongTimeString());
                }
                DA.DropTempTables();
                DA.CloseConnection();
                logger.WriteMessage("DB Connection Closed " + DateTime.Now.ToLongTimeString());
                logger.WriteMessage("Completed in (m:s) " + DateTime.Now.Subtract(OperationStart).Minutes.ToString() + ":" + DateTime.Now.Subtract(OperationStart).Seconds.ToString());
            }

            SourceFileStream.Close();
        }

        public static Dictionary<long, long> Ids = new Dictionary<long, long>(1100000);
        public static CNE ParseLine(string CSVLine)
        {
            CNE Result = new CNE();

            string[] LineParts = CSVLine.Split('#');
            if (LineParts.Length != Consts.CNEsCSVFieldsCount)
                return new CNE("(00) Некорректное количество параметров (" + LineParts.Length.ToString() + ")", ValidationResultType.WrongFormat);

            Result.Number = LineParts[(int)Consts.CNECSVStructure.CertificateNumber].Trim();
            if (string.IsNullOrEmpty(Result.Number))
                return new CNE("(01) Не указан номер свидетельства", ValidationResultType.NotValid);
            if (!Regex.IsMatch(Result.Number, @"\d{2}-\d{9}-\d{2}"))
                return new CNE(string.Format(
                                        "(02) Значение '{0}' в поле номер свидетельства не соответствует формату.",
                                        LineParts[(int)Consts.CNECSVStructure.CertificateNumber]), ValidationResultType.NotValid);

         
            Result.EducationInstitutionCode = LineParts[(int)Consts.CNECSVStructure.EducationInstitutionCode];


            if (string.Compare(LineParts[(int)Consts.CNECSVStructure.Sex].Trim(), "М", true) == 0)
                Result.Sex = true;
            else if (string.Compare(LineParts[(int)Consts.CNECSVStructure.Sex].Trim(), "Ж", true) == 0)
                Result.Sex = false;
            else
                return new CNE(string.Format("(04) Некорректное значение '{0}' в поле пол", LineParts[(int)Consts.CNECSVStructure.Sex]), ValidationResultType.NotValid);

            Result.LastName = LineParts[(int)Consts.CNECSVStructure.LastName].Trim();
            if (string.IsNullOrEmpty(Result.LastName))
                return new CNE("(10) Не указана фамилия", ValidationResultType.NotValid);

            Result.LastName = Hasher.Hash(Result.LastName);
            Result.FirstName = Hasher.Hash(LineParts[(int)Consts.CNECSVStructure.FirstName].Trim());

            Result.PatronymicName = Hasher.Hash(LineParts[(int)Consts.CNECSVStructure.PatronymicName].Trim());

            Result.PassportSeria = Hasher.Hash(LineParts[(int)Consts.CNECSVStructure.PassportSeria].Trim());

            Result.PassportNumber = LineParts[(int)Consts.CNECSVStructure.PassportNumber].Trim();
            if (string.IsNullOrEmpty(Result.PassportNumber))
                return new CNE("(05) Не указан номер паспорта", ValidationResultType.NotValid);
            Result.PassportNumber = Hasher.Hash(Result.PassportNumber);

            Result.Class = LineParts[(int)Consts.CNECSVStructure.Class].Trim();
            bool HasMarks = false;
            float Mark;
            for (int i = (int)Consts.CNECSVStructure.BeginMarks; i <= (int)Consts.CNECSVStructure.EndMarks; i += 2)
                if (!String.IsNullOrEmpty(LineParts[i].Trim()))
                {
                    if (!float.TryParse(LineParts[i].Trim(), NumberStyles.Number, CultureInfo.InvariantCulture.NumberFormat, out Mark)
                            || Mark < 0 || Mark > 100)
                        return new CNE(string.Format(
                            "(06) Некорректное значение '{1}' в поле баллов для предмета '{0}'",
                            Consts.SubjectNames[(i - 10) / 2], LineParts[i]), ValidationResultType.NotValid);
                    if (LineParts[i + 1].Trim() != "0" && LineParts[i + 1].Trim() != "1")
                        return new CNE(string.Format(
                                                "(07) Некорректное значение '{1}' в поле апелляция для предмета '{0}'",
                                                Consts.SubjectNames[(i - 10) / 2], LineParts[i + 1]), ValidationResultType.NotValid);
                    HasMarks = HasMarks || Mark >= 0;
                }

            if (!HasMarks)
            {
                Result.ValidationSummary = new ValidationResult("(08) Предупреждение: не указаны баллы ни по одному предмету", ValidationResultType.Incomplete);
            }   
            Result.EntrantNumber = LineParts[(int)Consts.CNECSVStructure.EntrantNumber];

            int CNERegionId;
            if (!int.TryParse(LineParts[(int)Consts.CNECSVStructure.RegionId].Trim(), out  CNERegionId))
                return new CNE(string.Format("(09) Некорректное значение '{0}' в поле кода региона", LineParts[(int)Consts.CNECSVStructure.RegionId]), ValidationResultType.NotValid);

            Result.RegionId = CNERegionId;
            Result.RegionId = ExtraxtNumberRegionId(Result.Number);

            Result.TypographicNumber = LineParts[(int)Consts.CNECSVStructure.TypographicNumber].Trim();

            Result.Year = ExtractYearFromNumber(Result.Number);

            Result.Id = GetCertificateId(Result.Number);
            if (Ids.ContainsKey(Result.Id))
                return new CNE(
                    string.Format("(03) Повтор номера свидетельства '{0}'", Result.Number), ValidationResultType.NotValid);


            for (int i = (int)Consts.CNECSVStructure.BeginMarks; i <= (int)Consts.CNECSVStructure.EndMarks; i += 2)
            {
                if (!string.IsNullOrEmpty(LineParts[i].Trim()) &&
                           float.TryParse(LineParts[i].Trim(), NumberStyles.Number, CultureInfo.InvariantCulture.NumberFormat, out Mark)
                           && Mark >= 0 && Mark <= 100)
                {
                    CNESubjectMark SubjectMark = new CNESubjectMark();
                    SubjectMark.RegionId = Result.RegionId;
                    SubjectMark.Mark = Mark;
                    SubjectMark.HasAppeal = (LineParts[i + 1].Trim() == "1");
                    SubjectMark.Year = Result.Year;
                    SubjectMark.SubjectId = Consts.SubjectIds[(i - 10) / 2];
                    Result.SubjectMarks.Add(SubjectMark);
                }
            }
            Ids.Add(Result.Id, Result.Id);
            return Result;
        }

        private static int ExtraxtNumberRegionId(string number)
        {
            return Convert.ToInt32(Regex.Replace(number,
                @"(?<region>\d{2})-(?<number>\d{9})-(?<year>\d{2})", "${region}"));
        }

        private static int ExtractYearFromNumber(string number)
        {
            try
            {
                return int.Parse(number.Split('-')[2]) + 2000;
            }
            catch (Exception)
            {
                return DateTime.Now.Year;
            }
        }

        private static long GetCertificateId(string number)
        {
            return long.Parse(Regex.Replace(number, @"(?<region>\d{2})-(?<number>\d{9})-(?<year>\d{2})",
                    @"${year}${region}${number}"));
        }
    }
}
