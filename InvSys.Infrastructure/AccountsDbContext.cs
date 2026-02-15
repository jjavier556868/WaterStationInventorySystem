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
            => optionsBuilder.UseSqlite($"Data Source={DbPath}");
    }
}
