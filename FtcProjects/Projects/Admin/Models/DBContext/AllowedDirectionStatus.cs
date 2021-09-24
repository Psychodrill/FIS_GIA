using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class AllowedDirectionStatus
    {
        public AllowedDirectionStatus()
        {
            AllowedDirections = new HashSet<AllowedDirections>();
        }

        public int AllowedDirectionStatusId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<AllowedDirections> AllowedDirections { get; set; }
    }
}
