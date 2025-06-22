using System;
using System.Threading.Tasks;
using Moq;
using PaymentService.Application;
using PaymentService.Application.DTO;
using PaymentService.Application.Interfaces;
using PaymentService.Application.Services;
using PaymentService.Domain.Enums;
using PaymentService.Domain.Interfaces;
using PaymentService.Domain.Models;

namespace PaymentService.Tests
{
    public class PaymentsServiceTests
    {
        private readonly Mock<ICreditCardService> _cardServiceMock;
        private readonly Mock<ITransactionRepository> _repositoryMock;

        public PaymentsServiceTests()
        {
            _cardServiceMock = new Mock<ICreditCardService>();
            _repositoryMock = new Mock<ITransactionRepository>();
        }

        [Fact]
        public async Task ProcessPayment_ShouldValidateCardAndAddTransaction()
        {
            // Arrange
            var random = new Random();
            var purchaseId = random.Next();
            var userId = random.Next();
            var listingId = random.Next();

            var request = new PaymentRequest
            {
                PurchaseId = purchaseId,
                UserId = userId,
                Amount = 50,
                CardNumber = "1111222233334444",
                ListingId = listingId
            };

            _cardServiceMock.Setup(c => c.ValidateCard(It.IsAny<string>()));
            _cardServiceMock.Setup(c => c.GetCardType(It.IsAny<string>())).Returns(CreditCardProvider.Mastercard);
            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<PaymentTransaction>()))
                .ReturnsAsync((PaymentTransaction t) => t);

            var service = new PaymentsService(_cardServiceMock.Object, _repositoryMock.Object);

            // Act — expect exception due to HttpClient call
            await Assert.ThrowsAnyAsync<Exception>(() => service.ProcessPayment(request));

            // Assert
            _cardServiceMock.Verify(c => c.ValidateCard(request.CardNumber), Times.Once);
            _cardServiceMock.Verify(c => c.GetCardType(request.CardNumber), Times.Once);
            _repositoryMock.Verify(r => r.AddAsync(It.Is<PaymentTransaction>(t =>
                t.PurchaseId == purchaseId &&
                t.UserId == userId &&
                t.Amount == request.Amount &&
                t.CardProvider == CreditCardProvider.Mastercard &&
                t.CardLast4 == "4444" &&
                t.Status == PaymentStatus.Pending
            )), Times.Once);
        }

        [Fact]
        public void ProcessPayment_ShouldThrow_WhenCardIsInvalid()
        {
            // Arrange
            var random = new Random();
            var invalidCardNumber = "invalidcard";

            _cardServiceMock.Setup(c => c.ValidateCard(invalidCardNumber))
                            .Throws(new ArgumentException("Invalid card number"));

            var request = new PaymentRequest
            {
                PurchaseId = random.Next(),
                UserId = random.Next(),
                Amount = 50,
                CardNumber = invalidCardNumber,
                ListingId = random.Next()
            };

            var service = new PaymentsService(_cardServiceMock.Object, _repositoryMock.Object);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(() => service.ProcessPayment(request));
            Assert.Equal("Invalid card number", ex.Result.Message);
        }

        [Fact]
        public async Task ProcessPayment_ShouldSetCorrectCardType()
        {
            // Arrange
            var random = new Random();
            var request = new PaymentRequest
            {
                PurchaseId = random.Next(),
                UserId = random.Next(),
                Amount = 75,
                CardNumber = "9876543210987654",
                ListingId = random.Next()
            };

            _cardServiceMock.Setup(c => c.ValidateCard(It.IsAny<string>()));
            _cardServiceMock.Setup(c => c.GetCardType(It.IsAny<string>())).Returns(CreditCardProvider.Visa);
            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<PaymentTransaction>()))
                .ReturnsAsync((PaymentTransaction t) => t);

            var service = new PaymentsService(_cardServiceMock.Object, _repositoryMock.Object);

            // Act — expect exception due to HttpClient call
            await Assert.ThrowsAnyAsync<Exception>(() => service.ProcessPayment(request));

            // Assert
            _repositoryMock.Verify(r => r.AddAsync(It.Is<PaymentTransaction>(t =>
                t.CardProvider == CreditCardProvider.Visa
            )), Times.Once);
        }
    }
}