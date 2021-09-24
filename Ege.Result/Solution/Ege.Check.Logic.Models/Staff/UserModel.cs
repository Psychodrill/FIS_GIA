namespace Ege.Check.Logic.Models.Staff
{
    public class UserModel
    {
        public int Id { get; set; }

        public int? RegionId { get; set; }

        public string Login { get; set; }

        public string PasswordHash { get; set; }

        public bool IsEnabled { get; set; }

        public Role Role { get; set; }
    }
}