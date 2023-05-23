using Microsoft.EntityFrameworkCore;
using API.Entities;
namespace API.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        //arsyja pse se kem kriju nje DbSet<BasketItem> eshte sepse neve sko me na u dasht kurr me e mar nje item individual direkt.
        
    }
}