using GamesLibrary.DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GamesLibrary.DataAccessLayer.Data
{
    public class GamesLibraryDbContext : IdentityDbContext<IdentityUser>
    {
        public GamesLibraryDbContext(DbContextOptions<GamesLibraryDbContext> options)
            : base(options)
        {
        }
        // DbSet for your custom User model (ApplicationUser)
        public DbSet<IdentityUser> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Purchase> Purchases { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the table name for Users
            modelBuilder.Entity<IdentityUser>().ToTable("Users");
        }
    }
}
