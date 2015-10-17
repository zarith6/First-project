using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace SampleDBandAuth
{
    class DB { }

    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string HashedPwd { get; set; }
        public string Salt { get; set; }


    }

    public class ConsoleDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
    }
}
