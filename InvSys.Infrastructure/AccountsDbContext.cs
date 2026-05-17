using InvSys.Domain.Models.Account;
using Microsoft.EntityFrameworkCore;

namespace InvSys.Infrastructure
{
    public class AccountsDbContext : DbContext
    {
        public DbSet<UserAccount> UserAccounts { get; set; }

        public string DbPath { get; }

        public AccountsDbContext()
        {
            DbPath = "account.db";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ── Global soft-delete query filter ──────────────────────────
            modelBuilder.Entity<UserAccount>().HasQueryFilter(u => u.DeletedAt == null);

            // ── Unique constraints ───────────────────────────────────────
            modelBuilder.Entity<UserAccount>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<UserAccount>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}