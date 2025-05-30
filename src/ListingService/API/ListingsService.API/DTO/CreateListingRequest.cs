using ListingsService.Domain.Enums;

namespace ListingsService.API.DTO
{
    public class CreateListingRequest
    {
       
            public string Name { get; set; }
            public string Description { get; set; }
            public string CategoryName { get; set; }  
            public decimal ProductPrice { get; set; }
            public Condition Condition { get; set; }
        
    }
}
