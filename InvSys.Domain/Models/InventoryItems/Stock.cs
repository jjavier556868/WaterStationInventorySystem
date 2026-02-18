using InvSys.Domain.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvSys.Domain.Models.InventoryItems
{
    public class Stock : BaseEntity
    {
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }  // Positive for IN, negative for OUT

        [Required, MaxLength(50)]
        public TransactionType Transaction { get; set; } = 0;  // "RECEIVED", "SOLD", "ADJUSTMENT"

        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
    }
}
