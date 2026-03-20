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
        public void Restock(int productId, int quantity)
        {
            var stock = _context.Stocks.FirstOrDefault(s => s.ProductId == productId);
            if (stock != null)
            {
                stock.Quantity += quantity;
                stock.UpdatedDate = DateTime.Now;
            }
            else
            {
                _context.Stocks.Add(new Stock
                {
                    ProductId = productId,
                    Quantity = quantity,
                    CreatedDate = DateTime.Now
                });
            }
            _context.SaveChanges();
        }
        public int GetAvailableStock(int productId)
        {
            int stocked = _context.Stocks
                .Where(s => s.ProductId == productId)
                .Sum(s => (int?)s.Quantity) ?? 0;
            int sold = _context.Sales
                .Where(s => s.ProductId == productId)
                .Sum(s => (int?)s.Quantity) ?? 0;
            return stocked - sold;
        }
        public void DeleteStock(int id)
        {
            var stock = _context.Stocks.FirstOrDefault(s => s.Id == id);
            if (stock != null)
            {
                _context.Stocks.Remove(stock);
                _context.SaveChanges();
            }
        }
        public List<StockDTO> GetAllStock()
        {
            // Pre-load all sales quantities grouped by product so we subtract
            // sold units from each stock entry, giving true available quantity.
            var soldByProduct = _context.Sales
                .GroupBy(s => s.ProductId)
                .Select(g => new { ProductId = g.Key, TotalSold = g.Sum(s => s.Quantity) })
                .ToDictionary(x => x.ProductId, x => x.TotalSold);

            return _context.Stocks
                .Include(s => s.Product)
                .OrderByDescending(s => s.CreatedDate)
                .ToList()
                .Select(s =>
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
                })
                .ToList();
        }
        public void UpdateStock(int stockId, int quantity)
        {
            var stock = _context.Stocks.FirstOrDefault(s => s.Id == stockId);
            if (stock == null)
                throw new Exception("Stock entry not found.");
            stock.Quantity = quantity;
            stock.UpdatedDate = DateTime.Now;
            _context.SaveChanges();
        }
        public void Dispose() => _context?.Dispose();
    }
}