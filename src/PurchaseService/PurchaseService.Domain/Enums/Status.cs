using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseService.Domain.Enums
{
    public enum Status
    {
        CREATED=2,
        IN_CART = 1,
        COMPLETED=3,
        PAYMENT_PENDING=4,
    }
}
