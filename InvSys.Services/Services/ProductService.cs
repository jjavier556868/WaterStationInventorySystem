using InvSys.Domain.Models.InventoryItems;
using InvSys.Infrastructure;
using InvSys.Services.DTOs;
using InvSys.Services.Interfaces;

namespace InvSys.Services.Services
{
    public class ProductService : IProductService
    {
        private readonly InventoryDbContext _context;

        public ProductService()
        {
            _context = new InventoryDbContext();
        }

        public void AddProduct(string name, string description, decimal price, int supplierId)
        {
            var product = new Product
            {
                Name = name,
                Description = description,
                Price = price,
                SupplierId = supplierId,
                CreatedDate = DateTime.Now
            };
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public List<ProductDTO> GetAllProducts()
        {
            return _context.Products
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    SupplierId = p.SupplierId,
                    SupplierName = p.Supplier.Name
                })
                .ToList();
        }

        public void UpdateProduct(int id, string name, string description, decimal price, int supplierId)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                product.Name = name;
                product.Description = description;
                product.Price = price;
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

        public void Dispose() => _context?.Dispose();
    }
}