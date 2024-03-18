using likeshoesapi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace likeshoesapi
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<ShoeSectionShoeType>()
                .HasKey(ss => new { ss.ShoeSectionId, ss.ShoeTypeId });
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ShoeSection> ShoeSections { get; set; }
        public DbSet<ShoeType> ShoeTypes { get; set; }
        public DbSet<ShoeVariant> ShoeVariants { get; set; }
        public DbSet<ShoeSectionShoeType> ShoeSectionsShoeType { get; set; }
    }
}
