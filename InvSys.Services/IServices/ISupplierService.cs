using InvSys.Services.DTOs;

public interface ISupplierService : IDisposable
{
    void AddSupplier(string name, string email, string location, string contact, bool isActive = true);

    List<SupplierDTO> GetAllSuppliers();

    void UpdateSupplier(int id, string name, string email, string location, string contact, bool isActive = true);

    void DeleteSupplier(int id);
}