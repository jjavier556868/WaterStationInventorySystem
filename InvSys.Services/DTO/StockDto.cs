using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvSys.Services.DTO
{
    public record StockDto(
            int Id,
            int ProductId,
            string ProductName,
            int Quantity,
            DateTime RestockedOn  
        );
}
