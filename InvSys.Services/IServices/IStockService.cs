using InvSys.Services.DTO;

namespace InvSys.Services.IServices
{
    public interface IStockService : IDisposable
    {
        Task RestockAsync(int productId, int quantity);
        Task<List<StockDto>> GetStockByProductAsync(int productId);
        Task<int> GetAvailableStockAsync(int productId);
        Task UpdateStockAsync(int id, int quantity);
        Task DeleteStockAsync(int id);
    }
}