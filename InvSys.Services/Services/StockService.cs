using InvSys.Domain.Models.InventoryItems;
using InvSys.Infrastructure;
using InvSys.Services.DTO;
using InvSys.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace InvSys.Services.Services
{
    public class StockService : IStockService
    {
        private readonly InventoryDbContext _context;

        public StockService(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task RestockAsync(int productId, int quantity)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == productId && p.DeletedDate == null)
                ?? throw new InvalidOperationException($"Product with ID {productId} not found.");

            var stock = new Stock
            {
                ProductId = productId,
                Quantity = quantity,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = "system"
            };

            await _context.Stocks.AddAsync(stock);
            await _context.SaveChangesAsync();
        }

        public async Task<List<StockDto>> GetStockByProductAsync(int productId)
        {
            return await _context.Stocks
                .Include(s => s.Product)
                .Where(s => s.ProductId == productId)
                .OrderByDescending(s => s.CreatedDate)
                .Select(s => new StockDto(
                    s.Id,
                    s.ProductId,
                    s.Product.Name,
                    s.Quantity,
                    s.CreatedDate
                ))
                .ToListAsync();
        }

        public async Task<int> GetAvailableStockAsync(int productId)
        {
            var totalRestocked = await _context.Stocks
                .Where(s => s.ProductId == productId)
                .SumAsync(s => (int?)s.Quantity) ?? 0;

            var totalSold = await _context.Sales
                .Where(s => s.ProductId == productId)
                .SumAsync(s => (int?)s.Quantity) ?? 0;

            return totalRestocked - totalSold;
        }

        public async Task UpdateStockAsync(int id, int quantity)
        {
            var stock = await _context.Stocks
                .FirstOrDefaultAsync(s => s.Id == id)
                ?? throw new InvalidOperationException($"Stock entry with ID {id} not found.");

            stock.Quantity = quantity;
            stock.UpdatedDate = DateTime.UtcNow;
            stock.UpdatedBy = "system";

            await _context.SaveChangesAsync();
        }

        public async Task DeleteStockAsync(int id)
        {
            var stock = await _context.Stocks
                .FirstOrDefaultAsync(s => s.Id == id)
                ?? throw new InvalidOperationException($"Stock entry with ID {id} not found.");

            stock.IsActive = false;
            stock.DeletedDate = DateTime.UtcNow;
            stock.DeletedBy = "system";

            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}