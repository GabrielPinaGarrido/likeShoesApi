using likeshoesapi.Models;
using Microsoft.EntityFrameworkCore;

namespace likeshoesapi
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
