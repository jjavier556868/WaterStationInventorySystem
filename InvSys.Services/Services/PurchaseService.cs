using InvSys.Domain.Models.Enums;
using InvSys.Domain.Models.InventoryItems;
using InvSys.Infrastructure;
using InvSys.Services.DTOs;
using InvSys.Services.Interfaces;

namespace InvSys.Services.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly InventoryDbContext _context;

        public PurchaseService()
        {
            _context = new InventoryDbContext();
        }

        public Purchase ProcessPurchase(List<SaleItemRequest> items, PaymentMethod paymentMethod)
        {
            // Validate stock availability for all items first
            foreach (var item in items)
            {
                int stocked = _context.Stocks
                    .Where(s => s.ProductId == item.ProductId)
                    .Sum(s => (int?)s.Quantity) ?? 0;

                int sold = _context.Sales
                    .Where(s => s.ProductId == item.ProductId)
                    .Sum(s => (int?)s.Quantity) ?? 0;

                int available = stocked - sold;

                if (item.Quantity > available)
                {
                    var product = _context.Products.FirstOrDefault(p => p.Id == item.ProductId);
                    throw new InvalidOperationException(
                        $"Insufficient stock for '{product?.Name ?? "Unknown"}'. Available: {available}, Requested: {item.Quantity}");
                }
            }

            // Create purchase header
            var purchase = new Purchase
            {
                PaymentMethod = paymentMethod,
                TotalAmount = 0,
                CreatedDate = DateTime.Now
            };
            _context.Purchases.Add(purchase);
            _context.SaveChanges();

            // Create sales line items
            decimal total = 0;
            foreach (var item in items)
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == item.ProductId);
                decimal unitPrice = product?.Price ?? 0;
                decimal subtotal = unitPrice * item.Quantity;
                total += subtotal;

                _context.Sales.Add(new Sales
                {
                    PurchaseId = purchase.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = unitPrice,
                    Subtotal = subtotal,
                    CreatedDate = DateTime.Now
                });
            }

            // Update total on purchase
            purchase.TotalAmount = total;
            _context.SaveChanges();

            return purchase;
        }

        public List<SalesLineItemDto> GetAllSales()
        {
            return _context.Sales.ToList()
                .Select(s => new SalesLineItemDto
                {
                    SaleId = s.Id,
                    PurchaseId = s.PurchaseId,
                    PurchasedOn = s.CreatedDate,
                    ProductName = _context.Products
                                        .Where(p => p.Id == s.ProductId)
                                        .Select(p => p.Name)
                                        .FirstOrDefault() ?? "Unknown",
                    Quantity = s.Quantity,
                    UnitPrice = s.UnitPrice,
                    Subtotal = s.Subtotal,
                    PurchaseTotal = _context.Purchases
                                        .Where(p => p.Id == s.PurchaseId)
                                        .Select(p => p.TotalAmount)
                                        .FirstOrDefault(),
                    PaymentMethod = _context.Purchases
                                        .Where(p => p.Id == s.PurchaseId)
                                        .Select(p => p.PaymentMethod.ToString())
                                        .FirstOrDefault() ?? ""
                })
                .OrderByDescending(s => s.PurchasedOn)
                .ToList();
        }

        public void Dispose() => _context?.Dispose();
    }
}