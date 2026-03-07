using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvSys.Services.DTOs
{
    public record SalesDto(
        int Id,
        int PurchaseId,
        int ProductId,
        string ProductName,
        int Quantity,
        decimal UnitPrice,
        decimal Subtotal
    );
}
