using ListingsService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListingsService.Application
{
    public interface IAddListingService
    {
         Task<Listing> AddListingAsync(Listing listing);
        Task<bool> DeleteListingAsync(int listingId, int userId, string role);
    }
}
