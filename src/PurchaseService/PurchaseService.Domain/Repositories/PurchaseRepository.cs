using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseService.Domain.Repositories
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly DataContext _dataContext;
        public PurchaseRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<PurchaseModel> AddToCartAsync(PurchaseModel purchase)
        {
            _dataContext.Add(purchase);
            await _dataContext.SaveChangesAsync();
            return purchase;

        }

        public async Task<PurchaseModel> UpdatePurchaseAsync(PurchaseModel purchase)
        {
            var existingPurchase = await _dataContext.Purchases.FindAsync(purchase.PurchaseId);
            if (existingPurchase == null)
            {
                throw new Exception("Purchase not found");
            }

            existingPurchase.status = purchase.status;
            existingPurchase.UpdatedAt = DateTime.UtcNow;

           

            _dataContext.Purchases.Update(existingPurchase);
            await _dataContext.SaveChangesAsync();

            return existingPurchase;
        }




        public async Task<PurchaseModel> CreatePurchaseAsync(PurchaseModel purchase)
        {
            _dataContext.Add(purchase);
            await _dataContext.SaveChangesAsync();
            return purchase;
        }

       
        public async Task<List<PurchaseModel>> GetAllAsync()
        {
            return await _dataContext.Purchases.ToListAsync();
        }

        public async Task<PurchaseModel> GetByIdAsync(int id)
        {
            return await _dataContext.Purchases.FindAsync(id);
        }

        public  async Task<PurchaseModel> GetCartItemAsync(int userId, int listingId)
        {
            return await _dataContext.Purchases.FirstOrDefaultAsync(p => p.BuyerId == userId && p.ListingId == listingId && p.status == Enums.Status.IN_CART);
        }
        public async Task<List<PurchaseModel>> GetCartItemsByUserId(int userId)
        {
            return await _dataContext.Purchases
                .Where(p => p.BuyerId == userId && p.status == Enums.Status.IN_CART)
                .ToListAsync(); 
        }

        public async Task<bool> RemoveFromCartAsync(int ListingId, int UserId)
        {
            var item = await GetCartItemAsync(UserId, ListingId);
            if(item==null)
            {
                return false;
            }
            _dataContext.Purchases.Remove(item);
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelPurchase(int purchaseId)
        {
            var purchase = await _dataContext.Purchases.FindAsync(purchaseId);
            if (purchase == null)
            {
                return false; 
            }

            _dataContext.Purchases.Remove(purchase);
            await _dataContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> IsItemInCartAsync(int listingId, int userId)
        {
            return await _dataContext.Purchases
                .AnyAsync(ci => ci.ListingId == listingId && ci.BuyerId == userId);
        }
        public async Task RemoveListingFromAllCartsAsync(int listingId)
        {
            var items = _dataContext.Purchases.Where(ci => ci.ListingId == listingId);
            _dataContext.Purchases.RemoveRange(items);
            await _dataContext.SaveChangesAsync();
        }

    }

}
