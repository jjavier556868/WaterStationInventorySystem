using InvSys.Domain.Models.Enums;
using InvSys.Domain.Models.InventoryItems;
using InvSys.Infrastructure;
using InvSys.Services.DTOs;
using InvSys.Services.Interfaces;
using InvSys.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace InvSys.Services.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly InventoryDbContext _context;
        private readonly IStockService _stockService;

        public PurchaseService(InventoryDbContext context, IStockService stockService)
        {
            _context = context;
            _stockService = stockService;
        }

        public async Task<PurchaseDto> ProcessPurchaseAsync(List<SaleItemRequest> items, PaymentMethod paymentMethod)
        {
            // 1. Validate stock availability for all items first
            foreach (var item in items)
            {
                var available = await _stockService.GetAvailableStockAsync(item.ProductId);
                if (available < item.Quantity)
                {
                    var product = await _context.Products.FindAsync(item.ProductId);
                    throw new InvalidOperationException(
                        $"Insufficient stock for '{product?.Name ?? $"Product {item.ProductId}"}'. " +
                        $"Requested: {item.Quantity}, Available: {available}.");
                }
            }

            // 2. Create the Purchase header
            var purchase = new Purchase
            {
                PaymentMethod = paymentMethod,
                TotalAmount = 0,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = "system"
            };

            await _context.Purchases.AddAsync(purchase);
            await _context.SaveChangesAsync();

            // 3. Create Sales line items
            var salesItems = new List<Sales>();
            decimal total = 0;

            foreach (var item in items)
            {
                var product = await _context.Products.FindAsync(item.ProductId)
                    ?? throw new InvalidOperationException($"Product with ID {item.ProductId} not found.");

                var subtotal = product.Price * item.Quantity;
                total += subtotal;

                var sale = new Sales
                {
                    PurchaseId = purchase.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price,
                    Subtotal = subtotal,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "system"
                };

                salesItems.Add(sale);
                await _context.Sales.AddAsync(sale);
            }

            // 4. Update Purchase total and save
            purchase.TotalAmount = total;
            await _context.SaveChangesAsync();

            // 5. Return full PurchaseDto with line items
            return new PurchaseDto(
                purchase.Id,
                purchase.CreatedDate,
                purchase.TotalAmount,
                purchase.PaymentMethod,
                salesItems.Select(s => new SalesDto(
                    s.Id,
                    s.PurchaseId,
                    s.ProductId,
                    _context.Products.Find(s.ProductId)?.Name ?? string.Empty,
                    s.Quantity,
                    s.UnitPrice,
                    s.Subtotal
                )).ToList()
            );
        }

        public async Task<List<PurchaseDto>> GetAllPurchasesAsync()
        {
            return await _context.Purchases
                .Where(p => p.DeletedDate == null)
                .Include(p => p.SalesItems)
                    .ThenInclude(s => s.Product)
                .OrderByDescending(p => p.CreatedDate)
                .Select(p => new PurchaseDto(
                    p.Id,
                    p.CreatedDate,
                    p.TotalAmount,
                    p.PaymentMethod,
                    p.SalesItems.Select(s => new SalesDto(
                        s.Id,
                        s.PurchaseId,
                        s.ProductId,
                        s.Product.Name,
                        s.Quantity,
                        s.UnitPrice,
                        s.Subtotal
                    )).ToList()
                ))
                .ToListAsync();
        }

        public async Task<PurchaseDto?> GetPurchaseByIdAsync(int purchaseId)
        {
            var purchase = await _context.Purchases
                .Where(p => p.DeletedDate == null)
                .Include(p => p.SalesItems)
                    .ThenInclude(s => s.Product)
                .FirstOrDefaultAsync(p => p.Id == purchaseId);

            if (purchase is null) return null;

            return new PurchaseDto(
                purchase.Id,
                purchase.CreatedDate,
                purchase.TotalAmount,
                purchase.PaymentMethod,
                purchase.SalesItems.Select(s => new SalesDto(
                    s.Id,
                    s.PurchaseId,
                    s.ProductId,
                    s.Product.Name,
                    s.Quantity,
                    s.UnitPrice,
                    s.Subtotal
                )).ToList()
            );
        }

        public async Task<List<SalesLineItemDto>> GetAllSalesAsync()
        {
            return await _context.Sales
                .Include(s => s.Product)
                .Include(s => s.Purchase)
                .OrderByDescending(s => s.Purchase.CreatedDate)
                .Select(s => new SalesLineItemDto(
                    s.Id,
                    s.PurchaseId,
                    s.Purchase.CreatedDate,
                    s.Product.Name,
                    s.Quantity,
                    s.UnitPrice,
                    s.Subtotal,
                    s.Purchase.TotalAmount,
                    s.Purchase.PaymentMethod
                ))
                .ToListAsync();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}