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

        public Task<PurchaseModel> CheckOutCartAsync(int ListingId)
        {
            throw new NotImplementedException();
        }

        public Task<PurchaseModel> CheckOutCartAsync(int ListingId, int UserId)
        {
            throw new NotImplementedException();
        }

        public Task<PurchaseModel> CreatePurchaseAsync(PurchaseModel purchase)
        {
            throw new NotImplementedException();
        }

        public Task<PurchaseModel> CreatePurchaseAsync(PurchaseModel purchase, int UserId, int ListingId)
        {
            throw new NotImplementedException();
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

        public Task<PurchaseModel> UpdateCart(int ListingId)
        {
            throw new NotImplementedException();
        }
    }

}
