namespace Ege.Check.Dal.Cache.StaffUsers
{
    using Ege.Check.Dal.Cache.Interfaces;
    using Ege.Check.Logic.Models.Staff;

    public interface IStaffUserCache : ICache<int, UserModel>
    {
    }
}