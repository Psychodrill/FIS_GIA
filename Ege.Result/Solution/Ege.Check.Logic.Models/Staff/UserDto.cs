namespace Ege.Check.Logic.Models.Staff
{
    public class UserDto
    {
        public int Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public int? RegionId { get; set; }

        public string RegionName { get; set; }

        public Role Role { get; set; }

        public string RoleName { get; set; }

        public bool IsEnabled { get; set; }
    }
}