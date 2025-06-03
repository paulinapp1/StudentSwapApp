using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseService.Application.DTO
{
    public class ListingRequest
    {
        public int UserId { get; set; }

        [JsonProperty("productPrice")]
        public decimal Price { get; set; }

    }

}
