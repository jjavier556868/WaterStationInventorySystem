using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvSys.Domain.Models.InventoryItems
{
    public class Sales : BaseEntity
    {
        // SaleDate → use BaseEntity.CreatedDate
        public int PurchaseId { get; set; }

        [ForeignKey(nameof(PurchaseId))]
        public Purchase Purchase { get; set; } = null!;

        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Column(TypeName = "DECIMAL(18,2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "DECIMAL(18,2)")]
        public decimal Subtotal { get; set; }
    }
}