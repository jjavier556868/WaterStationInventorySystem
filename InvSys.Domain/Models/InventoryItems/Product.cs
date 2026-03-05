using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace InvSys.Domain.Models.InventoryItems
{
    public class Product : BaseEntity
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "DECIMAL(18,2)")]
        public decimal Price { get; set; }

        public int SupplierId { get; set; }
        [ForeignKey(nameof(SupplierId))]
        public Supplier? Supplier { get; set; }

        [Column(TypeName = "INTEGER")]
        public int QuantityInStock { get; set; } = 0;

        public virtual ICollection<Sales> AllSales { get; set; } = new List<Sales>();
        public virtual ICollection<Stock> StockTransactions { get; set; } = new List<Stock>();
    }
}
