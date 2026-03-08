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
            return _context.Stocks
                .Include(s => s.Product)
                .OrderByDescending(s => s.CreatedDate)
                .Select(s => new StockDTO
                {
                    Id = s.Id,
                    ProductId = s.ProductId,
                    ProductName = s.Product.Name,
                    Quantity = s.Quantity,
                    CreatedDate = s.CreatedDate,
                    UpdatedDate = s.UpdatedDate
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