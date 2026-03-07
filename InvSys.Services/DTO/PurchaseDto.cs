using InvSys.Domain.Models.Enums;
using InvSys.Services.DTO;

namespace InvSys.Services.DTOs
{
    public record PurchaseDto(
        int Id,
        DateTime PurchasedOn,  // mapped from BaseEntity.CreatedDate
        decimal TotalAmount,
        PaymentMethod PaymentMethod,
        List<SalesDto> Items
    );
}