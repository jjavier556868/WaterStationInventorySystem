using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvSys.Domain.Models.InventoryItems
{
    public class Product: BaseEntity
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "REAL")]
        public decimal Price { get; set; }

        public int SupplierId { get; set; }
        [ForeignKey(nameof(SupplierId))]
        public Supplier Supplier { get; set; } = null!;

        [Column(TypeName = "REAL")]
        public decimal QuantityInStock { get; set; } = 0;
        public bool IsInStock => QuantityInStock > 0;
        public bool IsLowStock => QuantityInStock < 10;

        public virtual ICollection<Sales> Sale { get; set; } = new List<Sales>();
    }
}
