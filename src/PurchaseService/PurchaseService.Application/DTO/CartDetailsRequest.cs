using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseService.Application.DTO
{
    public class CartDetailsRequest
    {
      
            public int ListingId { get; set; }
            public string Name { get; set; }
            public decimal ProductPrice { get; set; }
            public string Description { get; set; }
            public int SellerId { get; set; }
        }

    }

