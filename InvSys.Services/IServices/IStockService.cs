using InvSys.Services.DTOs;

public interface IStockService : IDisposable
{
    Task RestockAsync(int productId, int quantity);
    Task<int> GetAvailableStockAsync(int productId);
    Task<List<StockDTO>> GetAllStockAsync();
    Task UpdateStockAsync(int stockId, int quantity);
    Task DeleteStockAsync(int id);
}