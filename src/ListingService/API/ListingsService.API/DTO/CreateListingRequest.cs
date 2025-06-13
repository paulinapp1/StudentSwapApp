using ListingsService.Domain.Enums;

namespace ListingsService.API.DTO
{
    public class CreateListingRequest
    {
       
            public required string Name { get; set; }
            public required string Description { get; set; }
            public required string CategoryName { get; set; }  
            public required decimal ProductPrice { get; set; }
            public required Condition Condition { get; set; }
        
    }
}
