namespace Ege.Dal.Common.Tests.Bulk
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Globalization;
    using System.Threading.Tasks;
    using Ege.Check.Dal.Store.Bulk.Load;
    using Ege.Check.Dal.Store.Repositories;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BulkLoaderTests : BasePersistentTest
    {
        [TestMethod]
        public async Task TestLoadDataAsync()
        {
            var bl = new BulkLoader();
            var dataTable = new DataTable("LocalTempTable");
            dataTable.Columns.Add("Id", typeof (int));
            dataTable.Columns.Add("Name", typeof (string));

            const int bulkBatchSize = 10000;
            for (var i = 0; i < bulkBatchSize; i++)
            {
                dataTable.Rows.Add(i, i.ToString(CultureInfo.InvariantCulture));
            }
            using (var connection = await CreateConnectionAsync())
            {
                var cmd = connection.CreateCommand();
                cmd.CommandText = string.Format("Create table [{0}] (Id int, Name varchar(9))",
                                                dataTable.TableName);
                await cmd.ExecuteNonQueryAsync();

                var start = DateTime.Now;
                await bl.LoadDataAsync(dataTable, dataTable.TableName, connection);
                Debug.WriteLine(DateTime.Now - start);
                cmd.CommandText = string.Format("select * from {0}", dataTable.TableName);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    var inserted = new Dictionary<int, string>();
                    while (await reader.ReadAsync())
                    {
                        inserted.Add((int) reader["Id"], (string) reader["Name"]);
                    }
                    Assert.AreEqual(bulkBatchSize, inserted.Count);
                    for (var i = 0; i < bulkBatchSize; i++)
                    {
                        Assert.AreEqual(inserted[i], i.ToString(CultureInfo.InvariantCulture));
                    }
                }


                cmd.CommandText = string.Format("Drop table [{0}]", dataTable.TableName);
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}