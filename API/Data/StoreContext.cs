using Microsoft.EntityFrameworkCore;
using API.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace API.Data
{
    public class StoreContext : IdentityDbContext<User>
    {
        public StoreContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        //arsyja pse se kem kriju nje DbSet<BasketItem> eshte sepse neve sko me na u dasht kurr me e mar nje item individual direkt.

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>()
                    .HasData(
                        new IdentityRole{Name = "Member", NormalizedName = "MEMBER"},
                        new IdentityRole{Name = "Admin", NormalizedName = "ADMIN"}
                    );

        }

    }
}