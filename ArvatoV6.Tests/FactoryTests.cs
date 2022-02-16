using ArvatoV6.Library.Factories.Abstract;
using ArvatoV6.Models.Concrete;
using Xunit;

namespace ArvatoV6.Tests
{
    public class FactoryTests
    {
        [Fact]
        public void ProvideValidCardType_Should_Return_CardObject()
        {
            // Arrange
            var cardType = CardType.Visa;

            // Act
            var card = CardFactory.CreateCard(cardType);
            var createdCardType = card.GetCardType();

            // Assert
            Assert.Equal(cardType, createdCardType);
        }

        [Fact]
        public void ProvideInvalidCardType_Should_Return_Null()
        {
            // Arrange
            var cardType = CardType.Invalid;

            // Act
            var card = CardFactory.CreateCard(cardType);

            // Assert
            Assert.Null(card);
        }
    }
}