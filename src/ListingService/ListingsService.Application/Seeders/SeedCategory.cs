using ListingsService.Domain.Models;
using ListingsService.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListingsService.Application.Seeders
{
    public class SeedCategory
    {
        private readonly IListingRepository _repository;
        public SeedCategory(IListingRepository repository)
        {
            _repository = repository;
        }
        public async Task SeedCategoryAsync()
        {
            await SeedCategoriesIfNotExist("Matematyka");
            await SeedCategoriesIfNotExist("Programowanie Obiektowe");
        }
        private async Task SeedCategoriesIfNotExist(string name)
        {
            var categoryExists = await _repository.GetByNameAsync(name);
            if (categoryExists == null)
            {
                var category = new Category
                {
                    CategoryName = name
                };
                await _repository.AddCategoryAsync(category);
            }
        }
    }
}
