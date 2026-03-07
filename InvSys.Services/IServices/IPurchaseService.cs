using InvSys.Domain.Models.Enums;
using InvSys.Domain.Models.InventoryItems;
using InvSys.Services.DTOs;

namespace InvSys.Services.Interfaces
{
    public record SaleItemRequest(int ProductId, int Quantity);

    public interface IPurchaseService : IDisposable
    {
        Purchase ProcessPurchase(List<SaleItemRequest> items, PaymentMethod paymentMethod);
        List<SalesLineItemDto> GetAllSales();
    }
}