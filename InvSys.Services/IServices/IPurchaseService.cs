using InvSys.Domain.Models.Enums;
using InvSys.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvSys.Services.IServices
{
    public record SaleItemRequest(int ProductId, int Quantity);

    public interface IPurchaseService : IDisposable
    {
        Task<PurchaseDto> ProcessPurchaseAsync(List<SaleItemRequest> items, PaymentMethod paymentMethod);
        Task<List<PurchaseDto>> GetAllPurchasesAsync();
        Task<PurchaseDto?> GetPurchaseByIdAsync(int purchaseId);

        // Flat list — one row per sale item, for grid display
        Task<List<SalesLineItemDto>> GetAllSalesAsync();
    }
}
