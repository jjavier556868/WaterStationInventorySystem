using InvSys.Domain.Models.Account;
using InvSys.Domain.Models.InventoryItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace InvSys.Infrastructure
{
    public class InventoryDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Sales> Sale { get; set; }  

        public string DbPath { get; }

        public InventoryDbContext()
        {
            DbPath = "inventory.db";  
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite($"Data Source={DbPath}");
    }
}
