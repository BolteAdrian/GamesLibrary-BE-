using GamesLibrary.Repository.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GamesLibrary.Repository.Data
{
    public class GamesLibraryDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Review> Reviews { get; set; }

        public GamesLibraryDbContext(DbContextOptions<GamesLibraryDbContext> options)
            : base(options)
        {
        } 
    }
}
