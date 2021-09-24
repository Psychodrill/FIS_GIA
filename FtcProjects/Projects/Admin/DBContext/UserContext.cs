using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Admin.Models;
using Admin.Models.DBContext;

namespace Admin.DBContext
{
    public class UserContext : DbContext
    {
        public DbSet<Users> Users { get; set; }
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }

    }
}
