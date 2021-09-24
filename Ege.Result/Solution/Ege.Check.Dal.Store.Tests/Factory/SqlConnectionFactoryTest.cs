namespace Ege.Check.Dal.Store.Tests.Factory
{
    using System.Data;
    using System.Threading.Tasks;
    using Ege.Check.Dal.Store.Factory;
    using Ege.Dal.Common.Factory;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class SqlConnectionFactoryTest
    {
        [TestMethod]
        public async Task TestCreateAsync()
        {
            var connectionStringProvider = new Mock<IConnectionStringProvider>(MockBehavior.Strict);
            var factory = new SqlConnectionFactory(connectionStringProvider.Object);
            const string connectionString =
                "Data Source=LocalServer;Initial Catalog=CheckEgeUnitTest;Integrated Security=True;";
            connectionStringProvider.Setup(x => x.CheckEge())
                                    .Returns(
                                        connectionString)
                                    .Verifiable();

            using (var connection = await factory.CreateAsync())
            {
                Assert.AreEqual("CheckEgeUnitTest", connection.Database);
                Assert.AreEqual("LocalServer", connection.DataSource);
                Assert.AreEqual(connectionString, connection.ConnectionString);
                Assert.AreEqual(ConnectionState.Open, connection.State);
            }
        }
    }
}