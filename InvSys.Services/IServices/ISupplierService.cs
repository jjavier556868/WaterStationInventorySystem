using InvSys.Services.DTOs;

public interface ISupplierService : IDisposable
{
    Task AddSupplierAsync(string name, string email, string location, string contact, bool isActive = true);
    Task<List<SupplierDTO>> GetAllSuppliersAsync();
    Task<SupplierDTO?> GetSupplierByIdAsync(int id);
    Task UpdateSupplierAsync(int id, string name, string email, string location, string contact, bool isActive = true);
    Task DeleteSupplierAsync(int id);
}