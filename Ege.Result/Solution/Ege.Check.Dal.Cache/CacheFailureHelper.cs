namespace Ege.Check.Dal.Cache
{
    using System;
    using Ege.Check.Common;
    using Ege.Check.Dal.Cache.Interfaces;

    public class CacheFailureHelper : ICacheFailureHelper
    {
        private const int FailureDuration = 5;
        private static volatile AtomicNullable<DateTime> LastFailureTime;
        private static volatile bool IsGloballyDown;
        private static volatile bool IsGetGloballyProhibited;

        public bool IsCacheFailed()
        {
            var lastFailureTime = LastFailureTime;
            return IsGloballyDown ||
                   (lastFailureTime != null && (DateTime.Now - lastFailureTime.Value).Minutes < FailureDuration);
        }

        public void Failed()
        {
            LastFailureTime = DateTime.Now;
        }

        public void Up()
        {
            IsGloballyDown = false;
            IsGetGloballyProhibited = false;
            LastFailureTime = null;
        }

        public void Down()
        {
            IsGloballyDown = true;
        }

        public DateTime? GetLastFailureTime()
        {
            return LastFailureTime != null ? LastFailureTime.Value : (DateTime?) null;
        }

        public bool IsGetProhibited()
        {
            return IsGetGloballyProhibited;
        }

        public void ProhibitGet()
        {
            IsGetGloballyProhibited = true;
        }
    }
}