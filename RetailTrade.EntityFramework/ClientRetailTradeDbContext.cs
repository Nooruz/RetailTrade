using Microsoft.EntityFrameworkCore;
using RetailTrade.Domain.Models;

namespace RetailTrade.EntityFramework
{
    public class ClientRetailTradeDbContext : DbContext
    {
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Product> Products { get; set; }

        public ClientRetailTradeDbContext(DbContextOptions<ClientRetailTradeDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {



            base.OnModelCreating(modelBuilder);
        }
    }
}
