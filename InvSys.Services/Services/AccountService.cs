using InvSys.Infrastructure;
using InvSys.Services.DTOs;
using Microsoft.EntityFrameworkCore;

namespace InvSys.Services.Services
{
    public class AccountService : IDisposable
    {
        private readonly AccountsDbContext _context;

        public AccountService()
        {
            _context = new AccountsDbContext();
        }

        public async Task<List<AccountDisplayDTO>> GetAllAccountsAsync()
        {
            return await _context.UserAccounts
                .OrderBy(u => u.Id)
                .Select(u => new AccountDisplayDTO
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt
                })
                .ToListAsync();
        }

        public void Dispose() => _context?.Dispose();
    }
}