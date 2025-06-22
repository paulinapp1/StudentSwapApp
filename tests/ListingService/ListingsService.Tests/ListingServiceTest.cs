using ListingsService.API.Controllers;
using ListingsService.Domain.Models;
using ListingsService.Application;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using ListingsService.API.DTO;
using ListingsService.Domain.Enums;
using System.Reflection;

namespace ListingsService.Tests
{
    public class ListingControllerTests
    {
        private readonly Mock<IListingRepository> _mockRepo;
        private readonly Mock<IAddListingService> _mockService;
        private readonly ListingController _controller;

        public ListingControllerTests()
        {
            _mockRepo = new Mock<IListingRepository>();
            _mockService = new Mock<IAddListingService>();
            _controller = new ListingController(_mockRepo.Object, _mockService.Object);
        }

        [Fact]
        public async Task UpdateListingStatus_UpdatesStatusAndReturnsOk()
        {
            // Arrange
            var mockRepo = new Mock<IListingRepository>();
            var mockService = new Mock<IAddListingService>();

            int listingId = 1;
            Status oldStatus = (Status)2; // example old status
            Status newStatus = (Status)3;

            // Setup UpdateStatusAsync to return true (update succeeded)
            mockRepo.Setup(r => r.UpdateStatusAsync(listingId, newStatus)).ReturnsAsync(true);

            var controller = new ListingController(mockRepo.Object, mockService.Object);

            // Act
            var result = await controller.UpdateListingStatus(listingId, newStatus);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("Listing status updated successfully.", okResult.Value);
        }

        [Fact]
        public async Task GetAllListings_Returns200Ok()
        {
            // Arrange
            var mockRepo = new Mock<IListingRepository>();
            var mockService = new Mock<IAddListingService>();

            var listings = new List<Listing>
            {
                new Listing
                {
                    ListingId = 1,
                    Name = "Metody Numeryczne",
                    Description = "Podrêcznik. 2 rok, Informatyka Stosowana UEK",
                    ProductPrice = 99.99m,
                    Condition = Condition.LIKE_NEW,
                    UserId = 101,
                    CategoryId = 10,
                    status = Status.ACTIVE
                },
                new Listing
                {
                    ListingId = 2,
                    Name = "Zeszyt",
                    Description = "Ca³y zapisany. Nie wiem z czego te notatki :/",
                    ProductPrice = 5.50m,
                    Condition = Condition.USED,
                    UserId = 102,
                    CategoryId = 11,
                    status = Status.ACTIVE
                }
            };

            mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(listings);

            var controller = new ListingController(mockRepo.Object, mockService.Object);

            // Act
            var result = await controller.GetAllListings();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            // Optional: Assert the returned data is the expected list
            var returnedListings = Assert.IsAssignableFrom<IEnumerable<Listing>>(okResult.Value);
            Assert.Equal(2, returnedListings.Count());
        }

        [Fact]
        public async Task DeleteListing_AsAdmin_Returns200Ok()
        {
            // Arrange
            var mockRepo = new Mock<IListingRepository>();
            var mockService = new Mock<IAddListingService>();

            var controller = new ListingController(mockRepo.Object, mockService.Object);

            // Setup admin user context with correct claim types
            var adminClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "123"), // User ID as string
                new Claim(ClaimTypes.Role, "Administrator")   // Role claim
            };

            var identity = new ClaimsIdentity(adminClaims, "TestAuth");
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(identity)
                }
            };

            // Mock the service method
            int listingId = 1;
            int expectedUserId = 123;
            string expectedRole = "Administrator";

            mockService.Setup(s => s.DeleteListingAsync(listingId, expectedUserId, expectedRole))
                       .ReturnsAsync(true);

            // Act
            var result = await controller.DeleteListing(listingId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("Listing deleted and removed from carts.", okResult.Value);

            // Verify the service was called with correct parameters
            mockService.Verify(s => s.DeleteListingAsync(listingId, expectedUserId, expectedRole), Times.Once);
        }

        [Fact]
        public async Task DeleteListing_NoUserIdClaim_ReturnsUnauthorized()
        {
            // Arrange
            var mockRepo = new Mock<IListingRepository>();
            var mockService = new Mock<IAddListingService>();
            var controller = new ListingController(mockRepo.Object, mockService.Object);

            // Setup user context without NameIdentifier claim
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Administrator")
            };

            var identity = new ClaimsIdentity(claims, "TestAuth");
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(identity)
                }
            };

            // Act
            var result = await controller.DeleteListing(1);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("User not authenticated.", unauthorizedResult.Value);
        }

        [Fact]
        public async Task AddCategory_NoUserIdClaim_ReturnsUnauthorized()
        {
            // Arrange
            var mockRepo = new Mock<IListingRepository>();
            var mockService = new Mock<IAddListingService>();
            var controller = new ListingController(mockRepo.Object, mockService.Object);

            // Setup user context without NameIdentifier claim
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Administrator")
            };

            var identity = new ClaimsIdentity(claims, "TestAuth");
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(identity)
                }
            };

            // Act
            var result = await controller.DeleteListing(1);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("User not authenticated.", unauthorizedResult.Value);
        }

        [Fact]
        public async Task DeleteListing_ServiceReturnsFalse_ReturnsBadRequest()
        {
            // Arrange
            var mockRepo = new Mock<IListingRepository>();
            var mockService = new Mock<IAddListingService>();
            var controller = new ListingController(mockRepo.Object, mockService.Object);

            // Setup admin user context
            var adminClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "123"),
                new Claim(ClaimTypes.Role, "Administrator")
            };

            var identity = new ClaimsIdentity(adminClaims, "TestAuth");
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(identity)
                }
            };

            // Mock service to return false (deletion failed)
            mockService.Setup(s => s.DeleteListingAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                       .ReturnsAsync(false);

            // Act
            var result = await controller.DeleteListing(1);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Could not delete listing. Either it does not exist or you're not authorized.", badRequestResult.Value);
        }

        [Fact]
        public async Task GetAllCategories_ReturnsOkResult_WithCategories()
        {
            // Arrange
            var mockCategories = new List<Category>
            {
                new Category { CategoryId = 1, CategoryName = "Electronics" },
                new Category { CategoryId = 2, CategoryName = "Books" }
            };

            _mockRepo.Setup(r => r.GetAllCategoriesAsync()).ReturnsAsync(mockCategories);

            // Act
            var result = await _controller.GetAllCategories();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            var returnedCategories = Assert.IsAssignableFrom<IEnumerable<Category>>(okResult.Value);
            Assert.Equal(2, ((List<Category>)returnedCategories).Count);
        }

        [Fact]
        public async Task AddCategory_AsAdmin_Returns200Ok()
        {
            // Arrange
            var mockRepo = new Mock<IListingRepository>();
            var mockService = new Mock<IAddListingService>();
            var controller = new ListingController(mockRepo.Object, mockService.Object);

            // Setup admin user context
            var adminClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "123"),
                new Claim(ClaimTypes.Role, "Administrator")
            };

            var identity = new ClaimsIdentity(adminClaims, "TestAuth");
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(identity)
                }
            };

            // Setup test data
            var request = new AddCategoryRequest { CategoryName = "Electronics" };
            var expectedCategory = new Category { CategoryName = "Electronics" };

            mockRepo.Setup(r => r.AddCategoryAsync(It.IsAny<Category>()))
                    .ReturnsAsync(expectedCategory);

            // Act
            var result = await controller.AddCategory(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(expectedCategory, okResult.Value);

            // Verify the repository was called
            mockRepo.Verify(r => r.AddCategoryAsync(It.Is<Category>(c => c.CategoryName == "Electronics")), Times.Once);
        }

        [Fact]
        public async Task AddCategory_AsUser_Returns401Unauthorized()
        {
            // Arrange
            var mockRepo = new Mock<IListingRepository>();
            var mockService = new Mock<IAddListingService>();
            var controller = new ListingController(mockRepo.Object, mockService.Object);

            // Setup regular user context
            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "123"),
                new Claim(ClaimTypes.Role, "User")
            };

            var identity = new ClaimsIdentity(userClaims, "TestAuth");
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(identity)
                }
            };

            var request = new AddCategoryRequest { CategoryName = "Electronics" };

            // Act & Assert
            var method = typeof(ListingController).GetMethod("AddCategory");
            var authorizeAttr = method.GetCustomAttribute<AuthorizeAttribute>();

            Assert.NotNull(authorizeAttr);
            Assert.Equal("Administrator", authorizeAttr.Roles);
        }

        [Fact]
        public async Task DeleteCategory_AsAdmin_Returns200Ok()
        {
            // Arrange
            var mockRepo = new Mock<IListingRepository>();
            var mockService = new Mock<IAddListingService>();
            var controller = new ListingController(mockRepo.Object, mockService.Object);

            // Setup admin user context
            var adminClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "123"),
                new Claim(ClaimTypes.Role, "Administrator")
            };

            var identity = new ClaimsIdentity(adminClaims, "TestAuth");
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(identity)
                }
            };

            // Setup mock - category exists and can be deleted
            int categoryId = 1;
            mockRepo.Setup(r => r.DeleteCategoryAsync(categoryId))
                    .ReturnsAsync(true);

            // Act
            var result = await controller.DeleteCategory(categoryId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(true, okResult.Value);

            // Verify the repository was called
            mockRepo.Verify(r => r.DeleteCategoryAsync(categoryId), Times.Once);
        }

        [Fact]
        public async Task DeleteCategory_AsAdmin_CategoryNotFound_Returns404NotFound()
        {
            // Arrange
            var mockRepo = new Mock<IListingRepository>();
            var mockService = new Mock<IAddListingService>();
            var controller = new ListingController(mockRepo.Object, mockService.Object);

            // Setup admin user context
            var adminClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "123"),
                new Claim(ClaimTypes.Role, "Administrator")
            };

            var identity = new ClaimsIdentity(adminClaims, "TestAuth");
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(identity)
                }
            };

            // Setup mock - category doesn't exist
            int categoryId = 999;
            mockRepo.Setup(r => r.DeleteCategoryAsync(categoryId))
                    .ReturnsAsync(false);

            // Act
            var result = await controller.DeleteCategory(categoryId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Listing with this ID does not exist", notFoundResult.Value);
        }

        [Fact]
        public async Task DeleteCategory_AsUser_Returns401Unauthorized()
        {
            // Arrange
            var mockRepo = new Mock<IListingRepository>();
            var mockService = new Mock<IAddListingService>();
            var controller = new ListingController(mockRepo.Object, mockService.Object);

            // Setup regular user context
            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "123"),
                new Claim(ClaimTypes.Role, "User")
            };

            var identity = new ClaimsIdentity(userClaims, "TestAuth");
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(identity)
                }
            };

            // Act & Assert
            // Check that the method has the correct authorization attribute
            var method = typeof(ListingController).GetMethod("DeleteCategory");
            var authorizeAttr = method.GetCustomAttribute<AuthorizeAttribute>();

            Assert.NotNull(authorizeAttr);
            Assert.Equal("Administrator", authorizeAttr.Roles);
        }

        [Fact]
        public async Task UpdateCategory_AsAdmin_Returns200Ok()
        {
            // Arrange
            var mockRepo = new Mock<IListingRepository>();
            var mockService = new Mock<IAddListingService>();
            var controller = new ListingController(mockRepo.Object, mockService.Object);

            // Setup admin user context
            var adminClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "123"),
                new Claim(ClaimTypes.Role, "Administrator")
            };

            var identity = new ClaimsIdentity(adminClaims, "TestAuth");
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(identity)
                }
            };

            // Setup test data
            var categoryToUpdate = new Category
            {
                CategoryId = 1,
                CategoryName = "Updated Electronics"
            };

            var existingCategory = new Category
            {
                CategoryId = 1,
                CategoryName = "Electronics"
            };

            var updatedCategory = new Category
            {
                CategoryId = 1,
                CategoryName = "Updated Electronics"
            };

            // Setup mocks
            mockRepo.Setup(r => r.GetCategoryByIdAsync(1))
                    .ReturnsAsync(existingCategory);

            mockRepo.Setup(r => r.UpdateCategoryAsync(It.IsAny<Category>()))
                    .ReturnsAsync(updatedCategory);

            // Act
            var result = await controller.UpdateCategory(categoryToUpdate);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(updatedCategory, okResult.Value);

            // Verify the repository methods were called
            mockRepo.Verify(r => r.GetCategoryByIdAsync(1), Times.Once);
            mockRepo.Verify(r => r.UpdateCategoryAsync(It.IsAny<Category>()), Times.Once);
        }

        [Fact]
        public async Task UpdateCategory_AsAdmin_CategoryNotFound_Returns404NotFound()
        {
            // Arrange
            var mockRepo = new Mock<IListingRepository>();
            var mockService = new Mock<IAddListingService>();
            var controller = new ListingController(mockRepo.Object, mockService.Object);

            // Setup admin user context
            var adminClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "123"),
                new Claim(ClaimTypes.Role, "Administrator")
            };

            var identity = new ClaimsIdentity(adminClaims, "TestAuth");
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(identity)
                }
            };

            // Setup test data
            var categoryToUpdate = new Category
            {
                CategoryId = 999,
                CategoryName = "Non-existent Category"
            };

            // Setup mock - category doesn't exist
            mockRepo.Setup(r => r.GetCategoryByIdAsync(999))
                    .ReturnsAsync((Category)null);

            // Act
            var result = await controller.UpdateCategory(categoryToUpdate);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Category with ID 999 not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task UpdateCategory_AsAdmin_InvalidCategoryData_Returns400BadRequest()
        {
            // Arrange
            var mockRepo = new Mock<IListingRepository>();
            var mockService = new Mock<IAddListingService>();
            var controller = new ListingController(mockRepo.Object, mockService.Object);

            // Setup admin user context
            var adminClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "123"),
                new Claim(ClaimTypes.Role, "Administrator")
            };

            var identity = new ClaimsIdentity(adminClaims, "TestAuth");
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(identity)
                }
            };

            // Act - Test with null category
            var result1 = await controller.UpdateCategory(null);

            // Test with category ID = 0
            var result2 = await controller.UpdateCategory(new Category { CategoryId = 0, CategoryName = "Test" });

            // Assert
            var badRequestResult1 = Assert.IsType<BadRequestObjectResult>(result1);
            Assert.Equal("Invalid category data.", badRequestResult1.Value);

            var badRequestResult2 = Assert.IsType<BadRequestObjectResult>(result2);
            Assert.Equal("Invalid category data.", badRequestResult2.Value);
        }

        [Fact]
        public async Task UpdateCategory_AsUser_Returns401Unauthorized()
        {
            // Arrange
            var mockRepo = new Mock<IListingRepository>();
            var mockService = new Mock<IAddListingService>();
            var controller = new ListingController(mockRepo.Object, mockService.Object);

            // Setup regular user context
            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "123"),
                new Claim(ClaimTypes.Role, "User")
            };

            var identity = new ClaimsIdentity(userClaims, "TestAuth");
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(identity)
                }
            };

            // Act & Assert
            // Check that the method has the correct authorization attribute
            var method = typeof(ListingController).GetMethod("UpdateCategory");
            var authorizeAttr = method.GetCustomAttribute<AuthorizeAttribute>();

            Assert.NotNull(authorizeAttr);
            Assert.Equal("Administrator", authorizeAttr.Roles);
        }
    }
}