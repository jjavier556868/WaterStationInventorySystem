using InvSys.Domain.Models.InventoryItems;

namespace InvSys.Services.Interfaces
{
    public interface IStockService : IDisposable
    {
        void Restock(int productId, int quantity);
        int GetAvailableStock(int productId);
        List<Stock> GetAllStock();
        void DeleteStock(int id);
    }
}