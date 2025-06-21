using PurchaseService.Application.DTO;
using PurchaseService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseService.Application
{
    public interface ICartService
    {
        Task<PurchaseModel> AddToCartAsync(int listingId, int userId);
        Task<bool> RemoveFromCartAsync(int listingId, int userId);
        Task<List<CartDetailsRequest>> GetCartItems(int userId);


    }

}
