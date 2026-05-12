using InvSys.Domain.Models.InventoryItems;
using InvSys.Infrastructure;
using InvSys.Services.DTOs;
using InvSys.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InvSys.Services.Services
{
    public class ProductService : IProductService
    {
        private readonly InventoryDbContext _context;

        public ProductService()
        {
            _context = new InventoryDbContext();
        }

        public async Task AddProductAsync(string name, string description, decimal price, int supplierId)
        {
            var product = new Product
            {
                Name = name,
                Description = description,
                Price = price,
                SupplierId = supplierId,
                CreatedDate = DateTime.Now
            };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ProductDTO>> GetAllProductsAsync()
        {
            return await _context.Products
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    SupplierId = p.SupplierId,
                    SupplierName = p.Supplier.Name
                })
                .ToListAsync();
        }

        public async Task<ProductDTO?> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .Where(p => p.Id == id)
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    SupplierId = p.SupplierId,
                    SupplierName = p.Supplier.Name
                })
                .FirstOrDefaultAsync();
        }

        public async Task UpdateProductAsync(int id, string name, string description, decimal price, int supplierId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product != null)
            {
                product.Name = name;
                product.Description = description;
                product.Price = price;
                product.SupplierId = supplierId;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return;

            var now = DateTime.Now;

            var stockEntries = await _context.Stocks.Where(s => s.ProductId == id).ToListAsync();
            foreach (var stock in stockEntries)
                stock.DeletedDate = now;

            var salesEntries = await _context.Sales.Where(s => s.ProductId == id).ToListAsync();
            foreach (var sale in salesEntries)
                sale.DeletedDate = now;

            product.DeletedDate = now;
            await _context.SaveChangesAsync();
        }

        public void Dispose() => _context?.Dispose();
    }
}