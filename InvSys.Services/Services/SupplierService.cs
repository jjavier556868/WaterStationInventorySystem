using InvSys.Domain.Models.InventoryItems;
using InvSys.Infrastructure;
using InvSys.Services.DTOs;
using InvSys.Services.Interfaces;

namespace InvSys.Services.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly InventoryDbContext _context;

        public SupplierService()
        {
            _context = new InventoryDbContext();
        }

        public void AddSupplier(string name, string email, string location, string contact, bool isActive = true)
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
            _context.Suppliers.Add(supplier);
            _context.SaveChanges();
        }

        public List<SupplierDTO> GetAllSuppliers()
        {
            return _context.Suppliers
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
                .ToList();
        }

        public void UpdateSupplier(int id, string name, string email, string location, string contact, bool isActive)
        {
            var supplier = _context.Suppliers.FirstOrDefault(s => s.Id == id);
            if (supplier != null)
            {
                supplier.Name = name;
                supplier.Email = email;
                supplier.Location = location;
                supplier.ContactNo = contact;
                supplier.IsActive = isActive;
                _context.SaveChanges();
            }
        }

        public void DeleteSupplier(int id)
        {
            var supplier = _context.Suppliers.FirstOrDefault(s => s.Id == id);
            if (supplier != null)
            {
                var productCount = _context.Products.Count(p => p.SupplierId == id);
                if (productCount > 0)
                    throw new InvalidOperationException(
                        $"Cannot delete supplier with {productCount} product(s). Reassign or delete products first.");

                _context.Suppliers.Remove(supplier);
                _context.SaveChanges();
            }
        }

        public void Dispose() => _context?.Dispose();
    }
}