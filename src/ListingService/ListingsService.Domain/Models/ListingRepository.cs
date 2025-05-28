using Microsoft.EntityFrameworkCore;
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
            
            _dataContext.Listings.Add(listing);
            await _dataContext.SaveChangesAsync();
            return listing;
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();

        }

        public async Task<Listing> GetByIdAsync(int id)
        {
            return await _dataContext.Listings.FindAsync(id);
        }

        public async Task<Listing> UpdateAsync(Listing listing)
        {
            _dataContext.Listings.Update(listing);
            await _dataContext.SaveChangesAsync();
            return listing;
        }
        public async Task<List<Listing>> GetAllAsync()
        {
            return await _dataContext.Listings.ToListAsync();
        }

      
        }
    }

