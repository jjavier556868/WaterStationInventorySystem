namespace InvSys.Services.DTOs
{
    public class SalesLineItemDto
    {
        public int SaleId { get; set; }
        public int PurchaseId { get; set; }
        public DateTime PurchasedOn { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }
        public decimal PurchaseTotal { get; set; }
        public string PaymentMethod { get; set; }
    }
}