using System.Data.Entity;

namespace IGamesData.StorageDB
{
    public class Database : DbContext
    {
        public Database()
            : base("name=DB")
        {
        }
        public virtual DbSet<StorageUserRegustration> StorageUsers { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}


