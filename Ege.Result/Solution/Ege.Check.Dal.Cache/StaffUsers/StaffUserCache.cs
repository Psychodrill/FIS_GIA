namespace Ege.Check.Dal.Cache.StaffUsers
{
    using Ege.Check.Dal.Cache.Interfaces;
    using Ege.Check.Dal.Cache.Interfaces.CacheFactory;
    using Ege.Check.Logic.Models.Staff;
    using JetBrains.Annotations;

    internal class StaffUserCache : BaseLockableCache<UserModel, int>, IStaffUserCache
    {
        public StaffUserCache(
            [NotNull] ICacheFailureHelper failureHelper, 
            [NotNull] ICacheSettingsProvider settings,
            [NotNull] ICacheLockAcquirer acquirer)
            : base(failureHelper, settings, acquirer)
        {
        }

        protected override string GetKeyString(int key)
        {
            return string.Format("StaffUser{0}", key);
        }
    }
}