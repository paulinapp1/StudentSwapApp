using ListingsService.Domain.Enums;
using ListingsService.Domain.Models;
using ListingsService.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListingsService.Application.Seeders
{
    public class SeedListing
    {
        private readonly IListingRepository _repository;
        public SeedListing(IListingRepository repository)
        {
            _repository = repository;
        }
        public async Task SeedListingAsync()
        {
            await SeedListingIfNotExist(2, "Czysta Architektura", "Supi Książka", 2, 25, Condition.USED, Status.ACTIVE );
            await SeedListingIfNotExist(3, "Analiza i Algebra", "Płacz", 1, 13, Condition.DAMAGED,Status.ACTIVE);
        }
        private async Task SeedListingIfNotExist(int id, string name, string description, int categoryId, decimal productPrice, Condition condition, Status status )
        {
            var listingExists = await _repository.GetListingByNameAsync(name);
            if (listingExists == null)
            {
                var listing = new Listing
                {
                    UserId = id,
                    Name = name,
                    Description = description,
                    CategoryId = categoryId,
                    ProductPrice = productPrice,
                    Condition = condition,
                    status = status
                };
                await _repository.AddAsync(listing);
            }
        }
    }
}
