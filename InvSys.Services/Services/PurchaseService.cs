using InvSys.Domain.Models.Enums;
using InvSys.Domain.Models.InventoryItems;
using InvSys.Infrastructure;
using InvSys.Services.DTOs;
using InvSys.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InvSys.Services.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly InventoryDbContext _context;

        public PurchaseService()
        {
            _context = new InventoryDbContext();
        }

        public async Task<Purchase> ProcessPurchaseAsync(List<SaleItemRequest> items, PaymentMethod paymentMethod)
        {
            foreach (var item in items)
            {
                int stocked = await _context.Stocks
                    .Where(s => s.ProductId == item.ProductId)
                    .SumAsync(s => (int?)s.Quantity) ?? 0;

                int sold = await _context.Sales
                    .Where(s => s.ProductId == item.ProductId)
                    .SumAsync(s => (int?)s.Quantity) ?? 0;

                int available = stocked - sold;

                if (item.Quantity > available)
                {
                    var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);
                    throw new InvalidOperationException(
                        $"Insufficient stock for '{product?.Name ?? "Unknown"}'. Available: {available}, Requested: {item.Quantity}");
                }
            }

            var purchase = new Purchase
            {
                PaymentMethod = paymentMethod,
                TotalAmount = 0,
                CreatedDate = DateTime.Now
            };
            await _context.Purchases.AddAsync(purchase);
            await _context.SaveChangesAsync();

            decimal total = 0;
            foreach (var item in items)
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);
                decimal unitPrice = product?.Price ?? 0;
                decimal subtotal = unitPrice * item.Quantity;
                total += subtotal;

                await _context.Sales.AddAsync(new Sales
                {
                    PurchaseId = purchase.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = unitPrice,
                    Subtotal = subtotal,
                    CreatedDate = DateTime.Now
                });
            }

            purchase.TotalAmount = total;
            await _context.SaveChangesAsync();

            return purchase;
        }

        public async Task<List<SalesLineItemDto>> GetAllSalesAsync()
        {
            return await _context.Sales
                .Join(_context.Products,
                    s => s.ProductId,
                    p => p.Id,
                    (s, p) => new { Sale = s, ProductName = p.Name })
                .Join(_context.Purchases,
                    sp => sp.Sale.PurchaseId,
                    pu => pu.Id,
                    (sp, pu) => new SalesLineItemDto
                    {
                        SaleId = sp.Sale.Id,
                        PurchaseId = sp.Sale.PurchaseId,
                        PurchasedOn = sp.Sale.CreatedDate,
                        ProductName = sp.ProductName,
                        Quantity = sp.Sale.Quantity,
                        UnitPrice = sp.Sale.UnitPrice,
                        Subtotal = sp.Sale.Subtotal,
                        PurchaseTotal = pu.TotalAmount,
                        PaymentMethod = pu.PaymentMethod.ToString()
                    })
                .OrderByDescending(s => s.PurchasedOn)
                .ToListAsync();
        }

        public void Dispose() => _context?.Dispose();
    }
}