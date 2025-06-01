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
        public PurchaseRepository(DataContext dataContext) {
            _dataContext = dataContext;
        }
        public Task<PurchaseModel> AddToCartAsync(int ListingId, int UserId)
        {
            throw new NotImplementedException();
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

        public Task<PurchaseModel> RemoveFromCartAsync(int ListingId)
        {
            throw new NotImplementedException();
        }

        public Task<PurchaseModel> RemoveFromCartAsync(int ListingId, int UserId)
        {
            throw new NotImplementedException();
        }

        public Task<PurchaseModel> UpdateCart(int ListingId)
        {
            throw new NotImplementedException();
        }
    }
}
