using InvSys.Domain.Models.Enums;
using InvSys.Infrastructure;
using InvSys.Services.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;

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
                    Role = u.Role == UserRole.Admin ? "Admin" : "User",
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<AccountDetailDTO> GetAccountByIdAsync(int id)
        {
            var u = await _context.UserAccounts.FirstOrDefaultAsync(x => x.Id == id);
            if (u == null) return null;
            return new AccountDetailDTO
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                PasswordHash = u.PasswordHash,
                Role = u.Role == UserRole.Admin ? "Admin" : "User",
                IsActive = u.IsActive
            };
        }

        public async Task<AccountDetailDTO> GetAccountByUsernameAsync(string username)
        {
            var u = await _context.UserAccounts
                .FirstOrDefaultAsync(x => x.Username == username);
            if (u == null) return null;
            return new AccountDetailDTO
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                PasswordHash = u.PasswordHash,
                Role = u.Role == UserRole.Admin ? "Admin" : "User",
                IsActive = u.IsActive
            };
        }

        public async Task UpdateAccountAsync(int id, string username, string email, string newPlaintextPassword = null, string role = null, bool? isActive = null)
        {
            var u = await _context.UserAccounts.FirstOrDefaultAsync(x => x.Id == id);
            if (u == null) throw new Exception("Account not found.");

            bool usernameTaken = await _context.UserAccounts
                .AnyAsync(x => x.Username == username && x.Id != id);
            if (usernameTaken)
                throw new Exception($"Username '{username}' is already taken.");

            bool emailTaken = await _context.UserAccounts
                .AnyAsync(x => x.Email == email && x.Id != id);
            if (emailTaken)
                throw new Exception($"Email '{email}' is already in use.");

            u.Username = username;
            u.Email = email;

            if (!string.IsNullOrWhiteSpace(newPlaintextPassword))
                u.PasswordHash = HashPassword(newPlaintextPassword);

            if (role != null)
                u.Role = role == "Admin" ? UserRole.Admin : UserRole.User;

            if (isActive.HasValue)
                u.IsActive = isActive.Value;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAccountAsync(int id)
        {
            var u = await _context.UserAccounts.FirstOrDefaultAsync(x => x.Id == id);
            if (u == null) throw new Exception("Account not found.");
            _context.UserAccounts.Remove(u);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsLastAdminAsync(int excludeId)
        {
            int adminCount = await _context.UserAccounts
                .CountAsync(x => x.Role == UserRole.Admin && x.Id != excludeId);
            return adminCount == 0;
        }

        private static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public void Dispose() => _context?.Dispose();
    }
}