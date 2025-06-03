using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseService.Domain.Repositories
{
    public interface IPurchaseRepository
    {
        Task<List<PurchaseModel>> GetAllAsync();
        Task<PurchaseModel> GetByIdAsync(int id);
        Task<PurchaseModel> CreatePurchaseAsync(PurchaseModel purchase, int UserId, int ListingId);
        Task<PurchaseModel> AddToCartAsync(PurchaseModel purchase);
        Task<bool> RemoveFromCartAsync(int ListingId, int UserId);
        Task<PurchaseModel> CheckOutCartAsync(int ListingId, int UserId);
        Task<PurchaseModel> UpdateCart(int ListingId); //poczytac jak to zrobic
        Task<PurchaseModel> GetCartItemAsync(int userId, int listingId);
      

    }

}
