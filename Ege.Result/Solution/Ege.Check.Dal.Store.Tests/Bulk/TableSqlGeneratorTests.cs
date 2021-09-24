namespace Ege.Check.Dal.Store.Bulk
{
    using System;
    using System.Data;
    using Ege.Dal.Common.Helpers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class TableSqlGeneratorTests
    {
        private TableSqlGenerator _tableSqlGenerator;
        private Mock<IDbTypeFactory> _typeFactory;

        [TestInitialize]
        public void TableSqlGeneratorTest()
        {
            _typeFactory = new Mock<IDbTypeFactory>(MockBehavior.Strict);
            _tableSqlGenerator = new TableSqlGenerator(_typeFactory.Object);
        }

        [TestMethod]
        public void CreateSqlTest()
        {
            const string expected = @"Create table [TableName] (
Id uniqueidentifier ,
RefId int ,
Name nvarchar(max) ,
CreateDate datetime 
)";
            using (var dt = new DataTable("TableName"))
            {
                dt.Columns.Add("Id", typeof (Guid));
                dt.Columns.Add("RefId", typeof (int));
                dt.Columns.Add("Name", typeof (string));
                dt.Columns.Add("CreateDate", typeof (DateTime));

                _typeFactory.Setup(x => x.Create(typeof (Guid))).Returns(DbType.Guid).Verifiable();
                _typeFactory.Setup(x => x.Create(typeof (int))).Returns(DbType.Int32).Verifiable();
                _typeFactory.Setup(x => x.Create(typeof (string))).Returns(DbType.String).Verifiable();
                _typeFactory.Setup(x => x.Create(typeof (DateTime))).Returns(DbType.DateTime).Verifiable();

                var actual = _tableSqlGenerator.CreateSql(dt, dt.TableName);

                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void DropSqlTest()
        {
            var sql = _tableSqlGenerator.DropSql("TableName");
            Assert.AreEqual("Drop table [TableName]", sql);
        }
    }
}