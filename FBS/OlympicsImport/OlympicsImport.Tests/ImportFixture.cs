using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using LumenWorks.Framework.IO.Csv;
using Moq;
using NUnit.Framework;
using OlympicImport.Services;

namespace OlympicsImport.Tests
{
    [TestFixture]
    public class ImportFixture
    {
        [Test]
        public void ConnectDb()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ImportTest"].ConnectionString;

            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();
            }
        }

        [Test]
        public void ValidateCsvCustom()
        {
            FileInfo fi = new FileInfo("sample_data.txt");
            Assert.IsTrue(fi.Exists);
            using (var txtReader = fi.OpenText())
            {
                using (var csv = new CsvReader(txtReader, true, '#', '"', '\\', '#', ValueTrimmingOptions.None))
                {
                    string[] headers = csv.GetFieldHeaders();
                    int line = 1;
                    while (!csv.EndOfStream)
                    try
                    {
                        csv.ReadNextRecord();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            string tmp = csv[headers[i]];
                        }
                        line++;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("###ERROR LINE {0}### {1}", line, e.Message);
                        Console.WriteLine();
                    }
                }
            }
        }

        [Test]
        public void FetchOlympics()
        {
            FileInfo fi = new FileInfo("sample_data.txt");
            Assert.IsTrue(fi.Exists);
            HashSet<int> processed = new HashSet<int>();
            HashSet<OlympicsCsvRecord> recs = new HashSet<OlympicsCsvRecord>();
            using (var txtReader = fi.OpenText())
            {
                using (var csv = new CsvReader(txtReader, true, '#', '"', '\\', '#', ValueTrimmingOptions.None))
                {
                    int line = 1;
                    while (!csv.EndOfStream)
                    try
                    {
                        csv.ReadNextRecord();
                        int num = Int32.Parse(csv[OlympicsImportSchema.OlympiadNumber]);

                        if (!processed.Contains(num))
                        {
                            OlympicsCsvRecord ol = OlympicsCsvRecord.ParseCsv(csv);
                            recs.Add(ol);
                            Console.WriteLine("code_name = {0}", ol.CodeName);
                            Console.WriteLine("olympiad_name = {0}", ol.OlympiadName);
                            Console.WriteLine("olympiad_number = {0}", ol.OlympiadNumber);
                            Console.WriteLine("olympiad_level = {0}", ol.OlympiadLevel);
                            Console.WriteLine("olympiad_subject_name = {0}", ol.OlympiadSubjectName);
                            Console.WriteLine("olympiad_subject_profile_name = {0}", ol.OlympiadSubjectProfileName);
                            Console.WriteLine("olympiad_year = {0}", ol.OlympiadYear);
                            Console.WriteLine("------------------------------------");
                            Console.WriteLine();
                            processed.Add(num);
                        }
                        line++;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("###ERROR LINE {0}### {1}", line, e.Message);
                        Console.WriteLine();
                    }
                }
            }
        }

        [Test]
        public void FetchDiplomants()
        {
            FileInfo fi = new FileInfo("sample_data.txt");
            Assert.IsTrue(fi.Exists);
            HashSet<int> processed = new HashSet<int>();
            using (var txtReader = fi.OpenText())
            {
                using (var csv = new CsvReader(txtReader, true, '#'))
                {
                    int line = 1;
                    while (!csv.EndOfStream)
                        try
                        {
                            csv.ReadNextRecord();

                            OlympicDiplomantCsvRecord dr = OlympicDiplomantCsvRecord.ParseCsv(csv);

                            Console.Write(dr.Id);
                            line++;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("###ERROR LINE {0}### {1}", line, e.Message);
                            Console.WriteLine();
                        }
                }
            }
        }

        [Test]
        public void CreateSubjectRecords()
        {
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();
            csb.DataSource = ".";
            csb.InitialCatalog = "fbs";
            csb.IntegratedSecurity = true;

            var provider = new Mock<IConnectionStringProvider>(MockBehavior.Loose);
            provider.Setup(x => x.ConnectionString).Returns(csb.ConnectionString);

            FileInfo fi = new FileInfo("sample_data.txt");
            Assert.IsTrue(fi.Exists);
            HashSet<int> processed = new HashSet<int>();

            var store = new OlympicSubjectRepository(provider.Object);

            using (var txtReader = fi.OpenText())
            {
                using (var csv = new CsvReader(txtReader, true, '#'))
                {
                    int line = 1;
                    while (!csv.EndOfStream)
                    {
                        try
                        {
                            csv.ReadNextRecord();
                            int num = Int32.Parse(csv[OlympicsImportSchema.OlympiadNumber]);

                            if (!processed.Contains(num))
                            {
                                OlympicsCsvRecord ol = OlympicsCsvRecord.ParseCsv(csv);

                                for (int i = 0; i < ol.OlympicThemeSubjects.Count; i++)
                                {
                                    ol.OlympicThemeSubjects[i] = store.GetOrCreate(ol.OlympicThemeSubjects[i].SubjectName);
                                }

                                for (int i = 0; i < ol.OlympicProfileSubjects.Count; i++)
                                {
                                    ol.OlympicProfileSubjects[i] = store.GetOrCreate(ol.OlympicProfileSubjects[i].SubjectName);
                                }

                                processed.Add(num);
                            }
                            line++;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("###ERROR LINE {0}### {1}", line, e.Message);
                            Console.WriteLine();
                        }
                    }
                }
            }
        }

        [Test]
        public void RunImportController()
        {
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();
            csb.DataSource = "10.32.200.164";
            csb.InitialCatalog = "fbs_2015_debug";
            csb.Pooling = true;
            csb.IntegratedSecurity = false;
            csb.UserID = "scholar";
            csb.Password = "scholar";

            var connectionProvider = new Mock<IConnectionStringProvider>(MockBehavior.Loose);
            connectionProvider.Setup(x => x.ConnectionString).Returns(csb.ConnectionString);

            FileInfo importFile = new FileInfo("sample_data_2015.txt");
            //Assert.IsTrue(importFile.Exists, "Import file missing");
            var controller = new OlympicImportController(connectionProvider.Object, importFile);
            controller.CleanTables(true, true, true);
            //controller.Run();
        }
    }
}