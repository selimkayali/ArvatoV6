using System.Collections.Generic;
using ArvatoV6.Library.Services.Abstract;
using ArvatoV6.Library.Services.Concrete;
using ArvatoV6.Models.Concrete;
using ArvatoV6.Models.Dto;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ArvatoV6.Tests;

public class ServiceTests
{
    private ICardValidationService _cardValidationService;
    private Mock<ILogger<CardValidationService>> _logger;
    private Mock<IValidator<CreditCardInputDto>> _validator;
    private void CreateSUT()
    {
        _logger = new Mock<ILogger<CardValidationService>>();
        _validator = new Mock<IValidator<CreditCardInputDto>>();
        _cardValidationService = new CardValidationService(_logger.Object, _validator.Object);
    }

    [Theory]
    [InlineData("4111111111111111", "123", CardType.Visa)]
    [InlineData("5399759303865595", "456", CardType.MasterCard)]
    [InlineData("371802759421027", "7890", CardType.AmericanExpress)]
    public void ProvideValidInputDto_Should_ReturnApiResultWithData(string cardNumber, string cvv, string cardType)
    {
        // Arrange
        CreateSUT();

        var cardInputDto = new CreditCardInputDto
        {
            CardHolder = "Selim Kayali",
            CardNumber = cardNumber,
            CVV = cvv,
            ExpiryDate = "12/2024"
        };

        _validator.Setup(x => x.Validate(cardInputDto)).Returns(new ValidationResult { });
        // Act
        var result = _cardValidationService.Validate(cardInputDto);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(cardType, result.Data);
    }

    [Fact]
    public void ProvideInvalidCardNumber_Should_ReturnApiResultWithError()
    {
        // Arrange
        CreateSUT();
        var cardInputDto = new CreditCardInputDto
        {
            CardHolder = "Selim Kayali",
            CardNumber = "1111111111111111",
            CVV = "1234",
            ExpiryDate = "12/2024"
        };

        _validator.Setup(x => x.Validate(cardInputDto)).Returns(new ValidationResult { });

        // Act
        var result = _cardValidationService.Validate(cardInputDto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Message);
    }

    [Fact]
    public void ProvideInvalidCardCvv_Should_ReturnApiResultWithError()
    {
        // Arrange
        CreateSUT();
        var cardInputDto = new CreditCardInputDto
        {
            CardHolder = "Selim Kayali",
            CardNumber = "4111111111111111",
            CVV = "1234",
            ExpiryDate = "122024"
        };

        _validator.Setup(x => x.Validate(cardInputDto)).Returns(new ValidationResult { });

        // Act
        var result = _cardValidationService.Validate(cardInputDto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Message);
    }
}
