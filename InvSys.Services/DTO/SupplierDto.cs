namespace InvSys.Services.DTOs
{
    public record SupplierDto(
        int Id,
        string Name,
        string Email,
        string Location,
        string ContactNo,
        bool IsActive,
        DateTime CreatedDate
    );
}