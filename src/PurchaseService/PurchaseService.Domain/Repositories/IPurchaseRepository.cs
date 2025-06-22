using PurchaseService.Domain.Enums;
using PurchaseService.Domain.Models;
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
        Task<PurchaseModel> CreatePurchaseAsync(PurchaseModel purchase);
        Task<bool> CancelPurchase(int purchaseId);
        Task<PurchaseModel> AddToCartAsync(PurchaseModel purchase);
        Task<bool> RemoveFromCartAsync(int ListingId, int UserId);
        Task<PurchaseModel> GetCartItemAsync(int userId, int listingId);
        Task<List<PurchaseModel>> GetCartItemsByUserId(int userId);
        Task<bool> IsItemInCartAsync(int listingId, int userId);
        Task RemoveListingFromAllCartsAsync(int listingId);
        Task<PurchaseModel> UpdatePurchaseAsync(PurchaseModel purchase);
   





    }

}
