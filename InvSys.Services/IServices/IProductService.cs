using InvSys.Services.DTOs;

public interface IProductService : IDisposable
{
    Task AddProductAsync(string name, string description, decimal price, int supplierId);
    Task<List<ProductDTO>> GetAllProductsAsync();
    Task UpdateProductAsync(int id, string name, string description, decimal price, int supplierId);
    Task<ProductDTO?> GetProductByIdAsync(int id);
    Task DeleteProductAsync(int id);
}