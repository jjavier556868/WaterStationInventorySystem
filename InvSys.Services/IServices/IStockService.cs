using InvSys.Services.DTOs;

public interface IStockService:IDisposable
{
    void Restock(int productId, int quantity);
    int GetAvailableStock(int productId);
    List<StockDTO> GetAllStock();
    void UpdateStock(int stockId, int quantity);
    void DeleteStock(int id);
    void Dispose();
}