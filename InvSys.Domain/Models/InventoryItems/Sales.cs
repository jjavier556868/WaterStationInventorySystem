using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvSys.Domain.Models.InventoryItems
{
    public class Sales : BaseEntity
    {
        public DateTime SaleDate { get; set; } = DateTime.UtcNow;

        [Range(1, int.MaxValue)]  
        public int QuantitySold { get; set; }  

        [Column(TypeName = "DECIMAL(18,2)")]
        [Range(0.01, double.MaxValue)]
        public decimal TotalAmount { get; set; }

        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;
    }
}
