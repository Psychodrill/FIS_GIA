namespace FbsUnitTestServices
{
    using Fbs.Core.Organizations;

    using NUnit.Framework;

    /// <summary>
    /// тесты OrganizationDataAccessor
    /// </summary>
    [TestFixture]
    public class OrganizationDataAccessorTests
    {
        /// <summary>
        /// метод гет должен проходить без ошибок
        /// </summary>
        [Test]
        public void GetShouldRun()
        {
            OrganizationDataAccessor.Get(19925);
        }

        /// <summary>
        /// не должно быть эксепшинов (указанные id не важны)
        /// </summary>
        [Test]
        public void SelectOrgUpdateHistoryShouldRun()
        {
            OrganizationDataAccessor accessor = new OrganizationDataAccessor();
            accessor.SelectOrgUpdateHistory(7245, 0, 6);
        }

        /// <summary>
        /// не должно быть эксепшинов (указанные id не важны)
        /// </summary>
        [Test]
        public void GetByVersionShouldRun()
        {
            OrganizationDataAccessor.Get(7245, 2);
        }

        /// <summary>
        /// не должно быть эксепшинов (указанные id не важны)
        /// </summary>
        [Test]
        public void SelectOrgUpdateHistoryCountShouldRun()
        {
            OrganizationDataAccessor accessor = new OrganizationDataAccessor();
            accessor.SelectOrgUpdateHistoryCount(7245);
        }
    }
}