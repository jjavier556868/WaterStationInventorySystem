using InvSys.Domain.Models;
using InvSys.Domain.Models.InventoryItems;
using Microsoft.EntityFrameworkCore;

namespace InvSys.Infrastructure
{
    public class InventoryDbContext : DbContext
    {
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Sales> Sales { get; set; }

        public string DbPath { get; }

        public InventoryDbContext()
        {
            DbPath = "inventory.db";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ── Global soft-delete query filters ────────────────────────
            // These automatically exclude soft-deleted records from ALL
            // queries so you never need .Where(x => x.DeletedDate == null)
            // in any service method.
            modelBuilder.Entity<Supplier>().HasQueryFilter(s => s.DeletedDate == null);
            modelBuilder.Entity<Product>().HasQueryFilter(p => p.DeletedDate == null);
            modelBuilder.Entity<Stock>().HasQueryFilter(s => s.DeletedDate == null);
            modelBuilder.Entity<Purchase>().HasQueryFilter(p => p.DeletedDate == null);
            modelBuilder.Entity<Sales>().HasQueryFilter(s => s.DeletedDate == null);

            // ── Relationships ────────────────────────────────────────────

            // Supplier → Products (one-to-many)
            // Restrict delete: cannot remove supplier if they have products
            modelBuilder.Entity<Supplier>()
                .HasMany(s => s.Products)
                .WithOne(p => p.Supplier)
                .HasForeignKey(p => p.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            // Product → StockTransactions (one-to-many)
            modelBuilder.Entity<Product>()
                .HasMany(p => p.StockTransactions)
                .WithOne(s => s.Product)
                .HasForeignKey(s => s.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Product → Sales (one-to-many)
            modelBuilder.Entity<Product>()
                .HasMany(p => p.AllSales)
                .WithOne(s => s.Product)
                .HasForeignKey(s => s.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Purchase → Sales line items (one-to-many)
            // Cascade: deleting a purchase removes its line items
            modelBuilder.Entity<Purchase>()
                .HasMany(p => p.SalesItems)
                .WithOne(s => s.Purchase)
                .HasForeignKey(s => s.PurchaseId)
                .OnDelete(DeleteBehavior.Cascade);

            // ── Column precision ─────────────────────────────────────────
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("DECIMAL(18,2)");

            modelBuilder.Entity<Purchase>()
                .Property(p => p.TotalAmount)
                .HasColumnType("DECIMAL(18,2)");

            modelBuilder.Entity<Sales>()
                .Property(s => s.UnitPrice)
                .HasColumnType("DECIMAL(18,2)");

            modelBuilder.Entity<Sales>()
                .Property(s => s.Subtotal)
                .HasColumnType("DECIMAL(18,2)");
        }
    }
}