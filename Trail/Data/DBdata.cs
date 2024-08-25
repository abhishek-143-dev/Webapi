using Microsoft.EntityFrameworkCore;
using Trail.Model;

namespace Trail.Data
{
    public class DBdata : DbContext
    {
        public DBdata(DbContextOptions<DBdata> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Supplier { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
