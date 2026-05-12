using InvSys.Domain.Models.InventoryItems;
using InvSys.Infrastructure;
using InvSys.Services.DTOs;
using InvSys.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InvSys.Services.Services
{
    public class StockService : IStockService
    {
        private readonly InventoryDbContext _context;

        public StockService()
        {
            _context = new InventoryDbContext();
        }

        public async Task RestockAsync(int productId, int quantity)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.ProductId == productId);
            if (stock != null)
            {
                stock.Quantity += quantity;
                stock.UpdatedDate = DateTime.Now;
            }
            else
            {
                await _context.Stocks.AddAsync(new Stock
                {
                    ProductId = productId,
                    Quantity = quantity,
                    CreatedDate = DateTime.Now
                });
            }
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetAvailableStockAsync(int productId)
        {
            int stocked = await _context.Stocks
                .Where(s => s.ProductId == productId)
                .SumAsync(s => (int?)s.Quantity) ?? 0;

            int sold = await _context.Sales
                .Where(s => s.ProductId == productId)
                .SumAsync(s => (int?)s.Quantity) ?? 0;

            return stocked - sold;
        }

        public async Task<List<StockDTO>> GetAllStockAsync()
        {
            var soldByProduct = await _context.Sales
                .GroupBy(s => s.ProductId)
                .Select(g => new { ProductId = g.Key, TotalSold = g.Sum(s => s.Quantity) })
                .ToDictionaryAsync(x => x.ProductId, x => x.TotalSold);

            var stocks = await _context.Stocks
                .Include(s => s.Product)
                .OrderByDescending(s => s.CreatedDate)
                .ToListAsync();

            return stocks.Select(s =>
            {
                int sold = soldByProduct.TryGetValue(s.ProductId, out int q) ? q : 0;
                int available = Math.Max(0, s.Quantity - sold);
                return new StockDTO
                {
                    Id = s.Id,
                    ProductId = s.ProductId,
                    ProductName = s.Product.Name,
                    Quantity = available,
                    CreatedDate = s.CreatedDate,
                    UpdatedDate = s.UpdatedDate
                };
            }).ToList();
        }

        public async Task UpdateStockAsync(int stockId, int quantity)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == stockId);
            if (stock == null)
                throw new Exception("Stock entry not found.");

            stock.Quantity = quantity;
            stock.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteStockAsync(int id)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);
            if (stock != null)
            {
                _context.Stocks.Remove(stock);
                await _context.SaveChangesAsync();
            }
        }

        public void Dispose() => _context?.Dispose();
    }
}