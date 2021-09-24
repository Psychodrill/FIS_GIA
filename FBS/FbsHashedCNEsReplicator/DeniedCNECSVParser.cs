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
    public static class DeniedCNECSVParser
    {
        const int MaxErrorCount = 50;
        const string ArchiveFolderName = "DenyArchive";

        static string GetArchiveFolderPath()
        {
            return Settings.Default.LoadFolder + "\\" + ArchiveFolderName;
        }

        public static void ParseFile(string fileName, string connectionString, ILogger logger)
        {
            string InternalFileName = GetArchiveFolderPath() + "\\" + Path.GetFileName(fileName);
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

                DeniedCNEDataAccessor DA = new DeniedCNEDataAccessor(connectionString);
                DA.OpenConnection();
                DA.CreateTempTables();
                logger.WriteMessage("DB Connection Opened " + DateTime.Now.ToLongTimeString());

                double DBSpan = 0;
                double ParseSpan = 0;
                bool CriticalErrors = false ;
                while ((CSVLine = FileReader.ReadLine()) != null)
                {
                    LineIndex++;
                    if (string.IsNullOrEmpty(CSVLine))
                        continue;

                    DateTime ParseStart = DateTime.Now;
                    DeniedCNE DeniedCNEFromLine = ParseLine(CSVLine);
                    ParseSpan += DateTime.Now.Subtract(ParseStart).Milliseconds;
                    if (DeniedCNEFromLine.ValidationSummary.ResultType == ValidationResultType.WrongFormat)
                    {
                        logger.WriteMessage("File format is incorrect, processing stopped");
                        CriticalErrors = true;
                        break;
                    }
                    else if (DeniedCNEFromLine.ValidationSummary.ResultType == ValidationResultType.Valid)
                    {
                        DateTime DBStart = DateTime.Now;
                        DA.AddToDB(DeniedCNEFromLine); //Add to DB
                        DBSpan += DateTime.Now.Subtract(DBStart).Milliseconds;
                    }
                    else
                    {
                        ErrorCount++;
                        logger.WriteMessage("Error found as line " + LineIndex + DeniedCNEFromLine.ValidationSummary.Message);
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

        public static Dictionary<long, long> Ids = new Dictionary<long, long>(50000);
        public static DeniedCNE ParseLine(string CSVLine)
        {
            DeniedCNE Result = new DeniedCNE();

            string[] LineParts = CSVLine.Split('#');
            if (LineParts.Length != Consts.DeniedCNEsCSVFieldsCount )
                return new DeniedCNE("(00) Некорректное количество параметров (" + LineParts.Length.ToString() + ")", ValidationResultType.WrongFormat);

            Result.CertificateNumber = LineParts[(int)Consts.DeniedCNECSVStructure.CertificateNumber].Trim();
            if (string.IsNullOrEmpty(Result.CertificateNumber))
                return new DeniedCNE("(01) Не указан номер свидетельства", ValidationResultType.NotValid);
            if (!Regex.IsMatch(Result.CertificateNumber, @"\d{2}-\d{9}-\d{2}"))
                return new DeniedCNE(string.Format(
                                        "(01) Значение '{0}' в поле номер свидетельства не соответствует формату.",
                                        LineParts[(int)Consts.DeniedCNECSVStructure.CertificateNumber]), ValidationResultType.NotValid);

            Result.Comment = LineParts[(int)Consts.DeniedCNECSVStructure.Comment].Trim();
            if (string.IsNullOrEmpty(Result.Comment))
                return new DeniedCNE("(03) Не указана причина аннулирования", ValidationResultType.NotValid);

            Result.NewCNENumber  = LineParts[(int)Consts.DeniedCNECSVStructure.NewCertificateNumber].Trim();
          
            if (!string.IsNullOrEmpty(Result.NewCNENumber)&&!Regex.IsMatch(Result.NewCNENumber, @"\d{2}-\d{9}-\d{2}"))
                return new DeniedCNE(string.Format(
                                        "(01) Значение '{0}' в поле новый номер свидетельства не соответствует формату.",
                                        LineParts[(int)Consts.DeniedCNECSVStructure.NewCertificateNumber]), ValidationResultType.NotValid);


            Result.Year = ExtractYearFromNumber(Result.CertificateNumber);

            Result.Id = GetCertificateId(Result.CertificateNumber);
            if (Ids.ContainsKey(Result.Id))
                return new DeniedCNE(
                    string.Format("(02) Повтор номера свидетельства '{0}'", Result.CertificateNumber), ValidationResultType.NotValid);
           
            Ids.Add(Result.Id, Result.Id);
            return Result;
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
