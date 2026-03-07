using InvSys.Domain.Models.Enums;

namespace InvSys.Services.DTOs
{
    public record SalesLineItemDto(
        int SaleId,
        int PurchaseId,
        DateTime PurchasedOn,
        string ProductName,
        int Quantity,
        decimal UnitPrice,
        decimal Subtotal,
        decimal PurchaseTotal,
        PaymentMethod PaymentMethod
    );
}