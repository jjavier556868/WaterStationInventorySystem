using InvSys.Domain.Models.InventoryItems;
using InvSys.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

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
            return _context.Suppliers.ToList();
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
                var productCount = _context.Products.Count(p => p.SupplierId == id);
                if (productCount > 0)
                {
                    throw new InvalidOperationException($"Cannot delete supplier with {productCount} product(s). Reassign or delete products first.");
                }

                _context.Suppliers.Remove(supplier);
                _context.SaveChanges();
            }
        }

        public List<object> GetAllProducts()
        {
            return _context.Products
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Price,
                    p.QuantityInStock,
                    SupplierName = _context.Suppliers
                        .Where(s => s.Id == p.SupplierId)
                        .Select(s => s.Name)
                        .FirstOrDefault() ?? "No Supplier",
                    p.CreatedDate
                })
                .OrderBy(p => p.Id)
                .ToList<object>();
        }

        

        public void UpdateProduct(int id, string name, decimal price, int quantity, int supplierId)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                product.Name = name;
                product.Price = price;
                product.QuantityInStock = quantity;
                product.SupplierId = supplierId;
                _context.SaveChanges();
            }
        }

        public void DeleteProduct(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }

        public void AddProduct(string name, decimal price, int quantity, int supplierId)
        {
            var product = new Product
            {
                Name = name,
                Price = price,
                QuantityInStock = quantity,
                SupplierId = supplierId,
                CreatedDate = DateTime.Now
            };
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
