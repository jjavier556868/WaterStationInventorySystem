using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvSys.Domain.Models.InventoryItems
{
    public class Sales : BaseEntity
    {
        public DateTime SaleDate { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "REAL")]
        public decimal QuantitySold { get; set; }

        [Column(TypeName = "REAL")]
        public decimal TotalAmount { get; set; }

        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;
    }
}
