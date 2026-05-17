using InvSys.Domain.Models.InventoryItems;
using InvSys.Infrastructure;
using InvSys.Services.DTOs;
using InvSys.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InvSys.Services.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly InventoryDbContext _context;

        public SupplierService()
        {
            _context = new InventoryDbContext();
        }

        public async Task AddSupplierAsync(string name, string email, string location, string contact, bool isActive = true)
        {
            var supplier = new Supplier
            {
                Name = name,
                Email = email,
                Location = location,
                ContactNo = contact,
                IsActive = isActive,
                CreatedDate = DateTime.Now
            };
            await _context.Suppliers.AddAsync(supplier);
            await _context.SaveChangesAsync();
        }

        public async Task<List<SupplierDTO>> GetAllSuppliersAsync()
        {
            return await _context.Suppliers
                .Select(s => new SupplierDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                    Email = s.Email,
                    Location = s.Location,
                    ContactNo = s.ContactNo,
                    IsActive = s.IsActive,
                    CreatedDate = s.CreatedDate
                })
                .ToListAsync();
        }

        public async Task<SupplierDTO?> GetSupplierByIdAsync(int id)
        {
            return await _context.Suppliers
                .Where(s => s.Id == id)
                .Select(s => new SupplierDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                    Email = s.Email,
                    Location = s.Location,
                    ContactNo = s.ContactNo,
                    IsActive = s.IsActive,
                    CreatedDate = s.CreatedDate
                })
                .FirstOrDefaultAsync();
        }

        public async Task UpdateSupplierAsync(int id, string name, string email, string location, string contact, bool isActive = true)
        {
            var supplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.Id == id);
            if (supplier != null)
            {
                supplier.Name = name;
                supplier.Email = email;
                supplier.Location = location;
                supplier.ContactNo = contact;
                supplier.IsActive = isActive;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteSupplierAsync(int id)
        {
            var supplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.Id == id);
            if (supplier != null)
            {
                var productCount = await _context.Products.CountAsync(p => p.SupplierId == id);
                if (productCount > 0)
                    throw new InvalidOperationException(
                        $"Cannot delete supplier with {productCount} product(s). Reassign or delete products first.");

                _context.Suppliers.Remove(supplier);
                await _context.SaveChangesAsync();
            }
        }

        public void Dispose() => _context?.Dispose();
    }
}