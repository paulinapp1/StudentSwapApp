using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListingsService.Domain.Models
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

        Task<List<Category>> GetAllCategoriesAsync();


    }
}
