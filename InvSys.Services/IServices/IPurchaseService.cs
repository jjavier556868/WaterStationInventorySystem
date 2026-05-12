using InvSys.Domain.Models.Enums;
using InvSys.Domain.Models.InventoryItems;
using InvSys.Services.DTOs;

namespace InvSys.Services.Interfaces
{
    public interface IPurchaseService : IDisposable
    {
        Task<Purchase> ProcessPurchaseAsync(List<SaleItemRequest> items, PaymentMethod paymentMethod);
        Task<List<SalesLineItemDto>> GetAllSalesAsync();
    }
}