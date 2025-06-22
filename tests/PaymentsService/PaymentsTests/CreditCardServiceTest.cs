using PaymentService.Application.Services;
using PaymentService.Domain.Enums;
using PaymentService.Domain.Exceptions;
using PaymentService.Domain.Exeptions;

namespace PaymentService.Tests
{
    public class CreditCardServiceTest
    {
        [Theory]
        [InlineData("4539 1488 0343 6467")]  // Visa
        [InlineData("4024-0071-6540-1778")] // Visa
        [InlineData("345-470-784-783-010")] // American Express (format fixed by method)
        [InlineData("4532 2080 2150 4434")] // Visa
        [InlineData("5551561443896215")]    // MasterCard
        public void ValidateCard_CardNumber_ShouldReturnTrue(string cardNum)
        {
            // Arrange
            var card = new CreditCardService();

            // Act & Assert
            var result = card.ValidateCard(cardNum);
            Assert.True(result);
        }

        [Theory]
        [InlineData("123433333333333333333333333")]
        [InlineData("111111111111111111111111111111111")]
        [InlineData("1444455666666666666666666666")]
        [InlineData("566&438283%%83u4u49m6666666666")]
        public void ValidateCard_CardNumber_tooLong(string cardNumber)
        {
            // Arrange
            var card = new CreditCardService();

            // Act & Assert
            var exception = Assert.Throws<CreditCardNumberTooLongException>(() => card.ValidateCard(cardNumber));
            Assert.Equal("Card number is too long", exception.Message);
        }

        [Theory]
        [InlineData("5")]
        [InlineData("6666")]
        [InlineData("5555555")]
        public void ValidateCard_CardNumber_tooShort(string cardNumber)
        {
            // Arrange
            var card = new CreditCardService();

            // Act & Assert
            var exception = Assert.Throws<CreditCardNumberTooShortException>(() => card.ValidateCard(cardNumber));
            Assert.Equal("Card number is too short", exception.Message);
        }

        [Theory]
        [InlineData("3497 7965 8312 797", CreditCardProvider.AmericanExpress)]  // AMEX 15 digits
        [InlineData("378523393817437", CreditCardProvider.AmericanExpress)]     // AMEX 15 digits
        [InlineData("4024-0071-6540-1778", CreditCardProvider.Visa)]            // Visa 16 digits
        [InlineData("4532 2080 2150 4434", CreditCardProvider.Visa)]            // Visa 16 digits
        [InlineData("5530016454538418", CreditCardProvider.Mastercard)]         // MasterCard 16 digits
        [InlineData("5131208517986691", CreditCardProvider.Mastercard)]         // MasterCard 16 digits
        public void GetCardType_should_return_correct(string cardNumber, CreditCardProvider expectedCardType)
        {
            // Arrange
            var card = new CreditCardService();

            // Act
            var result = card.GetCardType(cardNumber);

            // Assert
            Assert.Equal(expectedCardType, result);
        }

        [Theory]
        [InlineData("1234567812345678")]  // Invalid number
        [InlineData("6011123456789012")]  // Discover (not supported)
        [InlineData("3530111333300000")]  // JCB (not supported)
        public void GetCardType_InvalidCardNumber_ThrowsException(string cardNumber)
        {
            // Arrange
            var card = new CreditCardService();

            // Act & Assert
            Assert.Throws<CreditCardNumberInvalidException>(() => card.GetCardType(cardNumber));
        }

        [Theory]
        [InlineData("3497 7965 8312 797")]        // AMEX with spaces
        [InlineData("3785-2339-3817-437")]        // AMEX with dashes
        [InlineData("4024 0071 6540 1778")]       // Visa with spaces
        [InlineData("4532-2080-2150-4434")]       // Visa with dashes
        public void GetCardType_ShouldHandleFormattedNumbers(string formattedCardNumber)
        {
            // Arrange
            var card = new CreditCardService();

            // Act
            var result = card.GetCardType(formattedCardNumber);

            // Assert
            Assert.Contains(result, new[] {
                CreditCardProvider.Visa,
                CreditCardProvider.Mastercard,
                CreditCardProvider.AmericanExpress
            });
        }
    }
}