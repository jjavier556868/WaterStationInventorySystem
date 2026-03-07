using System.ComponentModel.DataAnnotations.Schema;
using InvSys.Domain.Models.Enums;

namespace InvSys.Domain.Models.InventoryItems
{
    public class Purchase : BaseEntity
    {
        [Column(TypeName = "DECIMAL(18,2)")]
        public decimal TotalAmount { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public virtual ICollection<Sales> SalesItems { get; set; } = new List<Sales>();
    }
}