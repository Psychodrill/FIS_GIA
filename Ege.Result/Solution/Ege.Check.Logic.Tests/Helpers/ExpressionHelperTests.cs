namespace Ege.Check.Logic.Helpers
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ExpressionHelperTests
    {
        [TestMethod]
        public void GetterTest()
        {
            var eh = new ExpressionHelper();
            var getterA = eh.Getter<Dto>(typeof (Dto).GetProperty("A"));
            var getterB = eh.Getter<Dto>(typeof (Dto).GetProperty("B"));
            var getterC = eh.Getter<Dto>(typeof (Dto).GetProperty("C"));

            var newGuid = Guid.NewGuid();
            var dto = new Dto("1", 1, newGuid);

            Assert.AreEqual("1", getterA(dto));
            Assert.AreEqual(1, getterB(dto));
            Assert.AreEqual(newGuid, getterC(dto));
        }

        public class Dto
        {
            public Dto(string a, int b, Guid c)
            {
                A = a;
                B = b;
                C = c;
            }

            public Dto(string a, int b) : this(a, b, Guid.NewGuid())
            {
                A = a;
                B = b;
            }

            public string A { get; set; }

            public int B { get; set; }

            public Guid C { get; set; }
        }
    }
}