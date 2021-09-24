using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using GVUZ.Util;
using GVUZ.Util.Services.Parser;
using NUnit.Framework;

namespace GVUZ.AppExport.Tests
{
    [TestFixture]
    public class ParsePackageFixture
    {
        [Test]
        [Ignore()]
        public void InsertWithBulkOnTheFly()
        {
            var csb = new SqlConnectionStringBuilder();
            csb.DataSource = "10.32.200.164";
            csb.InitialCatalog = "gvuz_tags";
            csb.MultipleActiveResultSets = true;
            csb.UserID = "scholar";
            csb.Password = "scholar";
            csb.ConnectTimeout = 300;

            using (var cn = new SqlConnection(csb.ConnectionString))
            {
                cn.Open();

                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "TRUNCATE TABLE [dbo].[ImportPackageParsed]";
                    cmd.ExecuteNonQuery();
                }
            }

            var filter = new ParseImportPackageFilter
            {
                MinDate = DateTime.Now.Date.AddYears(-2),
                InstitutionId = new[] { 587, 6361 }
            };

            
            using (var packageList = new ImportPackageList(csb.ConnectionString, filter, 1000))
            {
                packageList.ReportProgress = true;
                packageList.ProgressNotificationRate = 5;
                packageList.Progress += (sender, args) => Console.WriteLine("{0} of {1}, {2}%", args.ReadPackages, args.TotalPackages, args.Progress);
                using (var reader = new ApplicationOrderRecordListReader(new ApplicationOrderRecordList(packageList)))
                {
                    var bulk = new SqlBulkCopy(csb.ConnectionString);
                    bulk.DestinationTableName = "[dbo].[ImportPackageParsed]";
                    bulk.BatchSize = 500;
                    bulk.BulkCopyTimeout = 0;
                    reader.GetColumnMappings().ToList().ForEach(x => bulk.ColumnMappings.Add(x));
                    var timer = new Stopwatch();
                    timer.Start();
                    bulk.WriteToServer(reader);
                    Console.WriteLine(timer.Elapsed);
                    timer.Stop();
                }
            }
        }

        [Test]
        [Ignore()]
        public void EnumeratePackagesAsList()
        {
            var csb = new SqlConnectionStringBuilder();
            csb.DataSource = "10.32.200.164";
            csb.InitialCatalog = "gvuz_tags";
            csb.MultipleActiveResultSets = true;
            csb.UserID = "scholar";
            csb.Password = "scholar";
            csb.ConnectTimeout = 300;

            using (var cn = new SqlConnection(csb.ConnectionString))
            {
                cn.Open();

                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "TRUNCATE TABLE [dbo].[ImportPackageParsed]";
                    cmd.ExecuteNonQuery();
                }
            }

            var filter = new ParseImportPackageFilter
            {
                MinDate = DateTime.Now.Date.AddYears(-2),
                //InstitutionId = new[] { 587 }
            };

            var timer = new Stopwatch();

            using (var list = new ImportPackageList(csb.ConnectionString, filter, 1000))
            {
                timer.Start();
                int count = list.Count;
                
                foreach (var package in list)
                {
                    //Console.WriteLine(package.PackageId);
                }
                Console.WriteLine("Iterated {0} records in {1}", count, timer.Elapsed);
                timer.Stop();
            }
        }
    }
}