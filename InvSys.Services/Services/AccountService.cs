using InvSys.Infrastructure;
using InvSys.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InvSys.Services.Services
{
    public class AccountService : IDisposable
    {
        private readonly AccountsDbContext _context;

        public AccountService()
        {
            _context = new AccountsDbContext();
        }

        public List<AccountDisplayDTO> GetAllAccounts()
        {
            return _context.UserAccounts
                .OrderBy(u => u.Id)
                .Select(u => new AccountDisplayDTO
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt
                })
                .ToList();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}