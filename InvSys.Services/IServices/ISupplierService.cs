using InvSys.Services.DTOs;

namespace InvSys.Services.IServices
{
    public interface ISupplierService : IDisposable
    {
        Task AddSupplierAsync(string name, string email, string location, string contact);
        Task<List<SupplierDto>> GetAllSuppliersAsync();
        Task UpdateSupplierAsync(int id, string name, string email, string location, string contact, bool isActive);
        Task DeleteSupplierAsync(int id);
    }
}