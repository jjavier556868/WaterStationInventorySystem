using InvSys.Domain.Models;
using InvSys.Domain.Models.InventoryItems;
using InvSys.Infrastructure;
using InvSys.Services.DTOs;
using InvSys.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace InvSys.Services.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly InventoryDbContext _context;

        public SupplierService(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task AddSupplierAsync(string name, string email, string location, string contact)
        {
            var supplier = new Supplier
            {
                Name = name,
                Email = email,
                Location = location,
                ContactNo = contact,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = "system"
                // IsActive defaults to true via BaseEntity
            };

            await _context.Suppliers.AddAsync(supplier);
            await _context.SaveChangesAsync();
        }

        // Global query filter handles DeletedDate == null automatically
        public async Task<List<SupplierDto>> GetAllSuppliersAsync()
        {
            return await _context.Suppliers
                .Select(s => new SupplierDto(
                    s.Id,
                    s.Name,
                    s.Email,
                    s.Location,
                    s.ContactNo,
                    s.IsActive,
                    s.CreatedDate
                ))
                .ToListAsync();
        }

        public async Task UpdateSupplierAsync(int id, string name, string email, string location, string contact, bool isActive)
        {
            var supplier = await _context.Suppliers
                .FirstOrDefaultAsync(s => s.Id == id);

            if (supplier != null)
            {
                supplier.Name = name;
                supplier.Email = email;
                supplier.Location = location;
                supplier.ContactNo = contact;
                supplier.IsActive = isActive;
                supplier.UpdatedDate = DateTime.UtcNow;
                supplier.UpdatedBy = "system";

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteSupplierAsync(int id)
        {
            var supplier = await _context.Suppliers
                .FirstOrDefaultAsync(s => s.Id == id);

            if (supplier != null)
            {
                // Global filter already excludes deleted products,
                // so this count only includes active ones
                var productCount = await _context.Products
                    .CountAsync(p => p.SupplierId == id);

                if (productCount > 0)
                    throw new InvalidOperationException(
                        $"Cannot delete supplier with {productCount} active product(s). Reassign or delete products first.");

                supplier.IsActive = false;
                supplier.DeletedDate = DateTime.UtcNow;
                supplier.DeletedBy = "system";

                await _context.SaveChangesAsync();
            }
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}