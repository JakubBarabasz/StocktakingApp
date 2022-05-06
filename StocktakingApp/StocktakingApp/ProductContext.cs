using Microsoft.EntityFrameworkCore;

namespace StocktakingApp.Classes
{
    public class ProductContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(
                "Data Source=test.db");
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}