using System;
using System.Collections.Generic;
using System.Linq;
using InvSys.Infrastructure;
using InvSys.Domain.Models.InventoryItems;

namespace InvSys.Infrastructure
{
    public class InventoryService : IDisposable
    {
        private readonly InventoryDbContext _context;

        public InventoryService()
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
                isActive = isActive,
                CreatedDate = DateTime.Now
            };
            _context.Suppliers.Add(supplier);
            _context.SaveChanges();
        }

        public List<Supplier> GetAllSuppliers()
        {
            return _context.Suppliers
                .Where(s => s.isActive)
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
                supplier.isActive = isActive;
               

                _context.SaveChanges();
            }
        }
        public void DeleteSupplier(int id)
        {
            var supplier = _context.Suppliers.FirstOrDefault(s => s.Id == id);
            if (supplier != null)
            {
                _context.Suppliers.Remove(supplier);
                _context.SaveChanges();
            }
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
