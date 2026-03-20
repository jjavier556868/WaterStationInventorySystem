using InvSys.Services.DTOs;

public interface IProductService : IDisposable
{
    void AddProduct(string name, string description, decimal price, int supplierId);

    List<ProductDTO> GetAllProducts();

    void UpdateProduct(int id, string name, string description, decimal price, int supplierId);

    ProductDTO? GetProductById(int id);

    void DeleteProduct(int id);
}