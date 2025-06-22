using ListingsService.Domain.Enums;
using ListingsService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListingsService.Domain.Repositories
{
    public interface IListingRepository
    {
        Task<Listing> AddAsync(Listing listing);
        Task<Listing> UpdateAsync(Listing listing);
        Task<bool> DeleteAsync(int id);
        Task<Listing> GetByIdAsync(int id);
        Task<Category> AddCategoryAsync(Category category);
        Task<List<Listing>> GetAllAsync();
        Task<Category> GetByNameAsync(string categoryName);
        Task<bool> DeleteCategoryAsync(int id);
        Task<Category> UpdateCategoryAsync(Category category);
        Task<Category> GetCategoryByIdAsync(int id);
        Task<List<Category>> GetAllCategoriesAsync();
        Task<bool> UpdateStatusAsync(int listingId, Status newStatus);
        Task<Status> CheckListingStatus(int listingId);
        Task<Listing> GetListingByNameAsync(string name); 



    }
}
