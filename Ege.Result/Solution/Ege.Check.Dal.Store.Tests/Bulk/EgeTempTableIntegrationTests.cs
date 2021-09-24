namespace Ege.Check.Dal.Store.Bulk
{
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using Ege.Check.Common.Config;
    using Ege.Check.Dal.Store.Repositories;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class EgeTempTableIntegrationTests : BasePersistentTest
    {
        [TestMethod]
        public async Task IntegrationTest()
        {
            var tableSqlGenerator = new Mock<ITableSqlGenerator>(MockBehavior.Strict);
            var configReader = new Mock<IConfigReaderHelper>(MockBehavior.Strict);
            configReader.Setup(x => x.GetInt(EgeTempTableOperator.SuppressTemporaryTableDeletionSetting, It.IsAny<string>(), 0)).Returns(0).Verifiable();
            var tempTableOperator = new EgeTempTableOperator(tableSqlGenerator.Object, configReader.Object);

            var dataTable = new DataTable("MySuperTable");
            tableSqlGenerator.Setup(x => x.CreateSql(dataTable, It.IsAny<string>()))
                             .Returns("Create table #LocalTempTable (Id int)").Verifiable();

            using (var connection = await ConnectionFactory.CreateAsync())
            {
                var cmd = connection.CreateCommand();
                cmd.CommandText = "select OBJECT_ID('tempdb..#LocalTempTable')";
                await AssertCommandResult(cmd, true);
                using (var descriptor = await tempTableOperator.CreateAsync(Guid.NewGuid(), dataTable, connection))
                {
                    tableSqlGenerator.Setup(x => x.DropSql(descriptor.FullTableName))
                                     .Returns("Drop table #LocalTempTable").Verifiable();
                    await AssertCommandResult(cmd, false);
                }
                await AssertCommandResult(cmd, true);
            }
            tableSqlGenerator.VerifyAll();
        }

        [TestMethod]
        public async Task IntegrationTestWithoutDeletion()
        {
            var tableSqlGenerator = new Mock<ITableSqlGenerator>(MockBehavior.Strict);
            var configReader = new Mock<IConfigReaderHelper>(MockBehavior.Strict);
            configReader.Setup(x => x.GetInt(EgeTempTableOperator.SuppressTemporaryTableDeletionSetting, "", 0)).Returns(1).Verifiable();
            var tempTableOperator = new EgeTempTableOperator(tableSqlGenerator.Object, configReader.Object);

            var dataTable = new DataTable("MySuperTable");
            tableSqlGenerator.Setup(x => x.CreateSql(dataTable, It.IsAny<string>()))
                             .Returns("Create table #LocalTempTable (Id int)").Verifiable();

            using (var connection = await ConnectionFactory.CreateAsync())
            {
                var cmd = connection.CreateCommand();
                cmd.CommandText = "select OBJECT_ID('tempdb..#LocalTempTable')";
                await AssertCommandResult(cmd, true);
                using (await tempTableOperator.CreateAsync(Guid.NewGuid(), dataTable, connection))
                {
                    await AssertCommandResult(cmd, false);
                }
                await AssertCommandResult(cmd, false);
            }
            tableSqlGenerator.VerifyAll();
        }

        private static async Task AssertCommandResult(DbCommand cmd, bool isNull, [CallerLineNumber]int line = 0)
        {
            using (var dbReader = await cmd.ExecuteReaderAsync())
            {
                Assert.IsTrue(await dbReader.ReadAsync(), "Called from line {0}", line);
                Assert.AreEqual(isNull, await dbReader.IsDBNullAsync(0), "Called from line {0}", line);
            }
        }
    }
}
