namespace Ege.Check.Dal.Store.Repositories
// ReSharper restore CheckNamespace
{
    using System;
    using System.Data.SqlClient;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Ege.Check.Dal.Store.Factory;
    using Ege.Dal.Common.Factory;
    using JetBrains.Annotations;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NDbUnit.Core;
    using NDbUnit.Core.SqlClient;

    [DeploymentItem("../../TestDataSet.xsd")]
    public class BasePersistentTest
    {
        [NotNull] protected IDbConnectionFactory ConnectionFactory;
        [NotNull] protected IConnectionStringProvider ConnectionStringProvider;

        public BasePersistentTest()
        {
            ConnectionStringProvider = new ConfigConnectionStringProvider("CheckEgeUnitTest", "stub");
            ConnectionFactory = new SqlConnectionFactory(ConnectionStringProvider);
        }

        public async Task<SqlConnection> CreateConnectionAsync()
        {
            return (await ConnectionFactory.CreateAsync()) as SqlConnection;
        }

        public virtual void Init()
        {
            var sqlDatabase = new SqlDbUnitTest(ConnectionStringProvider.CheckEge());

            sqlDatabase.ReadXmlSchema(@"TestDataSet.xsd");

            var allDeploymentItem =
                GetType()
                    .GetCustomAttributes(typeof (DeploymentItemAttribute), true)
                    .Cast<DeploymentItemAttribute>();
            var isFirst = true;
            foreach (var di in allDeploymentItem.Where(x => x != null && x.Path.EndsWith("TestData.xml")))
            {
                var fileName = Path.GetFileName(di.Path);
                var path = Path.Combine(di.OutputDirectory, fileName);
                try
                {
                    sqlDatabase.ReadXml(path);
                    sqlDatabase.PerformDbOperation(!isFirst
                                                       ? DbOperationFlag.InsertIdentity
                                                       : DbOperationFlag.CleanInsertIdentity);
                }
                catch (Exception e)
                {
                    throw new Exception(string.Format("Error in file {0}", path), e);
                }

                isFirst = false;
            }
        }
    }
}