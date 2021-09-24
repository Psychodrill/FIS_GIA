namespace Ege.Check.Dal.Cache.Interfaces
{
    using System;

    public interface ICacheFailureHelper
    {
        bool IsCacheFailed();

        bool IsGetProhibited();

        void Failed();
        
        void ProhibitGet();

        void Down();

        void Up();

        DateTime? GetLastFailureTime();
    }
}
