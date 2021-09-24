using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class Users
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public Guid? RoleId { get; set; }
        public DateTime? InsertDate { get; set; }
        public int? IsActive { get; set; }
    }
}
