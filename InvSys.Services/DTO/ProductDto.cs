namespace InvSys.Services.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public int SupplierId { get; set; }

        public string SupplierName { get; set; }
    }
}