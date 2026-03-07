using InvSys.Services.DTOs;

namespace InvSys.Services.Interfaces
{
    public interface IProductService : IDisposable
    {
        Task AddProductAsync(string name, decimal price, int supplierId);
        Task<List<ProductDto>> GetAllProductsAsync();
        Task UpdateProductAsync(int id, string name, decimal price, int supplierId);
        Task DeleteProductAsync(int id);
    }
}