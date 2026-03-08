namespace InvSys.Services.DTOs
{
    
    public class StockViewDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }  
        public string Description { get; set; }
        public string SupplierName { get; set; }
    }
}
