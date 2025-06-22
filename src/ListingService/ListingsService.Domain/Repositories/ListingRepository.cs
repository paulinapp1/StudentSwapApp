using ListingsService.Domain.Enums;
using ListingsService.Domain.Models;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ListingsService.Domain.Repositories
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
        public async Task<Category> AddCategoryAsync(Category category)
        {

            _dataContext.Categories.Add(category);
            await _dataContext.SaveChangesAsync();
            return category;
        }
     
        public async Task<Category> GetByNameAsync(string categoryName)
        {
            return await _dataContext.Categories
                                     .FirstOrDefaultAsync(c => c.CategoryName == categoryName);
        }
        public async Task<Listing> GetListingByNameAsync(string name)
        {
            return await _dataContext.Listings.FirstOrDefaultAsync(l => l.Name == name);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var listing = await _dataContext.Listings.FindAsync(id);
            if (listing == null)
            {
                return false;
            }
            _dataContext.Listings.Remove(listing);
            await _dataContext.SaveChangesAsync();
            return true;

        }

        public async Task<Listing> GetByIdAsync(int id)
        {
            return await _dataContext.Listings.FindAsync(id);
        }
        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _dataContext.Categories.FindAsync(id);
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

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _dataContext.Categories.ToListAsync();

        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _dataContext.Categories.FindAsync(id);
            if (category == null)
            {
                return false;
            }
            _dataContext.Remove(category);
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            _dataContext.Categories.Update(category);
            await _dataContext.SaveChangesAsync();
            return category;
        }
        public async Task<bool> UpdateStatusAsync(int listingId, Status newStatus)
        {
            var listing = await _dataContext.Listings.FindAsync(listingId);
            if (listing == null)
                return false;

            listing.status = newStatus;
            _dataContext.Listings.Update(listing);
            await _dataContext.SaveChangesAsync();
            return true;
        }
        public async Task<Status> CheckListingStatus(int listingId)
        {
            var listing = await _dataContext.Listings.FindAsync(listingId);
            return listing.status;
        }
    }
}

