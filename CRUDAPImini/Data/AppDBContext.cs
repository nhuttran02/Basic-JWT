using CRUDAPImini.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CRUDAPImini.Data
{
    public class AppDBContext(DbContextOptions<AppDBContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
