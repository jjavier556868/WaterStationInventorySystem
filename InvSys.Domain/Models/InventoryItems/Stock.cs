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
        public int Quantity { get; set; }
        // RestockDate → use BaseEntity.CreatedDate
    }
}