using InvSys.Domain.Models.InventoryItems;
using InvSys.Infrastructure;
using InvSys.Services.Interfaces;

namespace InvSys.Services.Services
{
    public class StockService : IStockService
    {
        private readonly InventoryDbContext _context;

        public StockService()
        {
            _context = new InventoryDbContext();
        }

        // Updates existing stock quantity for a product, or creates one if none exists
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

        // Available = total stocked - total sold
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

        public List<Stock> GetAllStock()
        {
            return _context.Stocks
                .OrderByDescending(s => s.CreatedDate)
                .ToList();
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

        public void Dispose() => _context?.Dispose();
    }
}