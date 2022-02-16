using System.Collections.Generic;
using ArvatoV6.Models.Dto;
using ArvatoV6.Validations;
using FluentValidation;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using Moq;
using Xunit;

namespace ArvatoV6.Tests;

public class ValidationTest
{
    private const string yearLimit = "YearLimit";
    private const int limitYear = 10;
    private IValidator<CreditCardInputDto> _validator;
    private Mock<IConfiguration> _configuration;

    private void CreateSUT()
    {
        _configuration = new Mock<IConfiguration>();
        _configuration.SetupGet(x => x[It.IsAny<string>()]).Returns("10");
        _validator = new CreditCardValidator(_configuration.Object);
    }


    [Theory]
    [MemberData(nameof(GetCardHolderTestData))]
    public void ProvideInvalidCardHolderInput_Should_ReturnException(CreditCardInputDto cardHolder)
    {
        // Arrange
        CreateSUT();

        // Act
        var result = _validator.TestValidate(cardHolder);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CardHolder);
    }
    
    [Theory]
    [MemberData(nameof(GetCVVTestData))]
    public void ProvideInvalidCardCVVInput_Should_ReturnException(CreditCardInputDto cvv)
    {
        // Arrange
        CreateSUT();

        // Act
        var result = _validator.TestValidate(cvv);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CVV);
    }
    
    [Theory]
    [MemberData(nameof(GetCardNumberData))]
    public void ProvideInvalidCardNumberInput_Should_ReturnException(CreditCardInputDto cardNumber)
    {
        // Arrange
        CreateSUT();

        // Act
        var result = _validator.TestValidate(cardNumber);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CardNumber);
    }
    
    [Theory]
    [MemberData(nameof(GetExpiryDateData))]
    public void ProvideInvalidExpiryDateInput_Should_ReturnException(CreditCardInputDto expiryDate)
    {
        // Arrange
        CreateSUT();

        // Act
        var result = _validator.TestValidate(expiryDate);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ExpiryDate);
    }

    public static IEnumerable<object[]> GetCardHolderTestData()
    {
        yield return new object[]
        {
            new CreditCardInputDto { CardHolder = "", CardNumber = "1234567890123456", ExpiryDate = "122024", CVV = "123" }
        };

        yield return new object[]
        {
            new CreditCardInputDto { CardHolder = "Selim 1 Kayali", CardNumber = "4111111111111111", ExpiryDate = "122024", CVV = "123" }
        };

        yield return new object[]
        {
            new CreditCardInputDto { CardHolder = "Selim 2 Kayali", CardNumber = "4111111111111111", ExpiryDate = "122024", CVV = "123" }
        };
    }
    
    public static IEnumerable<object[]> GetCVVTestData()
    {
        yield return new object[]
        {
            new CreditCardInputDto { CardHolder = "test", CardNumber = "1234567890123456", ExpiryDate = "122024", CVV = "" }
        };

        yield return new object[]
        {
            new CreditCardInputDto { CardHolder = "Selim Kayali", CardNumber = "4111111111111111", ExpiryDate = "122024", CVV = "45154" }
        };

        yield return new object[]
        {
            new CreditCardInputDto { CardHolder = "Selim Kayali", CardNumber = "4111111111111111", ExpiryDate = "122024", CVV = "dfg" }
        };
    }
    
    public static IEnumerable<object[]> GetCardNumberData()
    {
        yield return new object[]
        {
            new CreditCardInputDto { CardHolder = "test", CardNumber = "0000000000000000", ExpiryDate = "122024", CVV = "123" }
        };

        yield return new object[]
        {
            new CreditCardInputDto { CardHolder = "Selim Kayali", CardNumber = "asdf", ExpiryDate = "122024", CVV = "1234" }
        };

        yield return new object[]
        {
            new CreditCardInputDto { CardHolder = "Selim Kayali", CardNumber = "", ExpiryDate = "122024", CVV = "123" }
        };
    }
    
    public static IEnumerable<object[]> GetExpiryDateData()
    {
        yield return new object[]
        {
            new CreditCardInputDto { CardHolder = "test", CardNumber = "4111111111111111", ExpiryDate = "", CVV = "123" }
        };

        yield return new object[]
        {
            new CreditCardInputDto { CardHolder = "Selim Kayali", CardNumber = "4111111111111111", ExpiryDate = "122050", CVV = "1234" }
        };
        
        yield return new object[]
        {
            new CreditCardInputDto { CardHolder = "Selim Kayali", CardNumber = "4111111111111111", ExpiryDate = "12122024", CVV = "123" }
        };
    }
}