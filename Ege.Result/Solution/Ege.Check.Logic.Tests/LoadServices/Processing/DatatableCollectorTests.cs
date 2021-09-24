namespace Ege.Check.Logic.LoadServices.Processing
{
    using System.Data;
    using System.Reflection;
    using Ege.Check.Logic.Helpers;
    using JetBrains.Annotations;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class DatatableCollectorTests
    {
        [NotNull] private DatatableCollector<Dto> _datatableCollector;
        [NotNull] private Mock<IExpressionHelper> _expressionHelperMock;

        [TestInitialize]
        public void DatatableCollectorTest()
        {
            _expressionHelperMock = new Mock<IExpressionHelper>(MockBehavior.Strict);
            _expressionHelperMock.Setup(x => x.Getter<Dto>(typeof (Dto).GetProperty("A")))
                                 .Returns((PropertyInfo pi) => dto => dto.A).Verifiable();

            _expressionHelperMock.Setup(x => x.Getter<Dto>(typeof (Dto).GetProperty("B")))
                                 .Returns((PropertyInfo pi) => dto => dto.B).Verifiable();
            _datatableCollector = new DatatableCollector<Dto>(_expressionHelperMock.Object);
        }

        [TestMethod]
        public void CraeteTest()
        {
            var dt = _datatableCollector.Create();

            Assert.AreEqual(2, dt.Columns.Count);

            Assert.AreEqual("A", dt.Columns[0].ColumnName);
            Assert.AreEqual(typeof (string), dt.Columns[0].DataType);

            Assert.AreEqual("B", dt.Columns[1].ColumnName);
            Assert.AreEqual(typeof (int), dt.Columns[1].DataType);

            _expressionHelperMock.VerifyAll();
        }

        [TestMethod]
        public void AddRowTest()
        {
            var dt = new DataTable("test");
            dt.Columns.Add("A", typeof (string));
            dt.Columns.Add("B", typeof (int));

            _datatableCollector.AddRow(new Dto("1", 1), dt);
            _datatableCollector.AddRow(new Dto("2", 2), dt);

            Assert.AreEqual(2, dt.Rows.Count);

            Assert.AreEqual("1", dt.Rows[0][0]);
            Assert.AreEqual(1, dt.Rows[0][1]);

            Assert.AreEqual("2", dt.Rows[1][0]);
            Assert.AreEqual(2, dt.Rows[1][1]);

            _expressionHelperMock.VerifyAll();
        }

        public class Dto
        {
            public Dto(string a, int b)
            {
                A = a;
                B = b;
            }

            public string A { get; set; }

            public int B { get; set; }
        }
    }
}