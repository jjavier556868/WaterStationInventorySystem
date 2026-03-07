namespace InvSys.Services.DTOs
{
    public record ProductDto(
        int Id,
        string Name,
        decimal Price,
        int SupplierId,
        string SupplierName,
        int AvailableStock,
        DateTime CreatedDate
    );
}