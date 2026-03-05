using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvSys.Domain.Models.Enums
{
    public enum TransactionType
    {
        Receieved,    // + Items in
        Sold,        // - Items out  
        Adjustment   // Manual fix
    }
}
