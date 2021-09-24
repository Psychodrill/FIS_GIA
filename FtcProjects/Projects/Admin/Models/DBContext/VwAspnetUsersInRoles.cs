using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class VwAspnetUsersInRoles
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
    }
}
