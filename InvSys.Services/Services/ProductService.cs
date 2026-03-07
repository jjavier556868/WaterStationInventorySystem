using InvSys.Domain.Models.InventoryItems;
using InvSys.Infrastructure;
using InvSys.Services.DTOs;
using InvSys.Services.Interfaces;
using InvSys.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace InvSys.Services.Services
{
    public class ProductService : IProductService
    {
        private readonly InventoryDbContext _context;

        public ProductService(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task AddProductAsync(string name, decimal price, int supplierId)
        {
            var product = new Product
            {
                Name = name,
                Price = price,
                SupplierId = supplierId,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = "system"
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            return await _context.Products

                .Include(p => p.Supplier)
                .OrderBy(p => p.Id)
                .Select(p => new ProductDto(
                    p.Id,
                    p.Name,
                    p.Price,
                    p.SupplierId,
                    p.Supplier != null ? p.Supplier.Name : "No Supplier",
                    p.StockTransactions.Sum(s => s.Quantity) - p.AllSales.Sum(s => s.Quantity),
                    p.CreatedDate
                ))
                .ToListAsync();
        }

        public async Task UpdateProductAsync(int id, string name, decimal price, int supplierId)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product != null)
            {
                product.Name = name;
                product.Price = price;
                product.SupplierId = supplierId;
                product.UpdatedDate = DateTime.UtcNow;
                product.UpdatedBy = "system";

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product != null)
            {
                // Soft delete
                product.IsActive = false;
                product.DeletedDate = DateTime.UtcNow;
                product.DeletedBy = "system";

                await _context.SaveChangesAsync();
            }
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}