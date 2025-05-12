using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListingsService.Domain.Models
{
    public class ListingRepository : IListingRepository
    {
        private readonly DataContext _dataContext;

        public ListingRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Listing> AddAsync(Listing listing)
        {
            _dataContext.Add(listing);
            await _dataContext.SaveChangesAsync();
            return listing;
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Listing> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Listing> UpdateAsync(Listing listing)
        {
            throw new NotImplementedException();
        }
    }
}
