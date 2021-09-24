namespace FbsUnitTestServices
{
    using System;

    using FbsServices;

    using NUnit.Framework;

    /// <summary>
    /// Тесты сервиса CNECService
    /// </summary>
    [TestFixture]
    public class CNECServiceTest
    {
        private readonly CNECService cnecService = new CNECService();

        /// <summary>
        /// SelectCNECCheckHystory должден запускаться (корректная процедура в бд должна существовать)
        /// </summary>
        [Test]
        public void SelectCNECCheckHystoryShouldRun()
        {
           this.cnecService.SelectCNECCheckHystory(new Guid("1"), 0, 10);
        }

        /// <summary>
        /// SelectCNECCheckHystoryCount должден запускаться (корректная процедура в бд должна существовать)
        /// </summary>
        [Test]
        public void SelectCNECCheckHystoryCountShouldRun()
        {
            this.cnecService.SelectCNECCheckHystoryCount(new Guid("1"));
        }
    }
}
