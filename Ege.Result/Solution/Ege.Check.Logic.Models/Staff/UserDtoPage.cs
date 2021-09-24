namespace Ege.Check.Logic.Models.Staff
{
    using System.Collections.Generic;

    public class UserDtoPage
    {
        public int Count { get; set; }

        public ICollection<UserDto> Users { get; set; }
    }
}