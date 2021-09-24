namespace Ege.Check.Logic.LoadServices.Processing
{
    using System;
    using Ege.Check.Logic.Services.Dtos.Metadata;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AttributeProcedureNameGetterTests
    {
        private const string ProcedureName = "procedure";

        [TestMethod]
        public void GetNameTest()
        {
            var attributeProcedureNameGetter = new AttributeProcedureNameGetter();
            Assert.AreEqual(ProcedureName, attributeProcedureNameGetter.GetName<DtoWithAttr>());
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void GetNameExceptionTest()
        {
            var attributeProcedureNameGetter = new AttributeProcedureNameGetter();
            attributeProcedureNameGetter.GetName<DtoWithoutAttr>();
        }

        [BulkMergeProcedure(ProcedureName)]
        public class DtoWithAttr
        {
        }

        public class DtoWithoutAttr
        {
        }
    }
}