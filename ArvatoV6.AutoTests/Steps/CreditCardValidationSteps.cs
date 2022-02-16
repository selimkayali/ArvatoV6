using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArvatoV6.Library.Services.Abstract;
using ArvatoV6.Library.Services.Concrete;
using ArvatoV6.Models.Concrete;
using ArvatoV6.Models.Dto;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using TechTalk.SpecFlow;

namespace ArvatoV6.AutoTests.Steps;

[Binding]
public class CreditCardValidationSteps
{
    private Mock<ILogger<CardValidationService>> _logger;
    private Mock<IValidator<CreditCardInputDto>> _validator;
    private void CreateSUT()
    {
        _logger = new Mock<ILogger<CardValidationService>>();
        _validator = new Mock<IValidator<CreditCardInputDto>>();
    }
    
    private ApiResult _apiResult = new ApiResult();
    private CreditCardInputDto _creditCardInputDto = new CreditCardInputDto();
    private readonly ICardValidationService _cardValidationService;

    public CreditCardValidationSteps()
    {
        CreateSUT();
        _cardValidationService = new CardValidationService(_logger.Object, _validator.Object);
    }
    
    [Given(@"I have valid information \{cardHolder:""(.*)"", cardNumber: ""(.*)"", expiryDate:""(.*)"", cvv:""(.*)""}")]
    public void GivenIHaveValidInformationCardHolderCardNumberExpiryDateCvv(string p0, string p1, string p2, string p3)
    {
        _creditCardInputDto = new CreditCardInputDto
        {
            CardHolder = p0,
            CardNumber = p1,
            ExpiryDate = p2,
            CVV = p3
        };
    }

    [When(@"I validate the card")]
    public void WhenIValidateTheCard()
    {
        _validator.Setup(x => x.Validate(_creditCardInputDto)).Returns(new ValidationResult { });

       _apiResult = _cardValidationService.Validate(_creditCardInputDto);
     }

    [Then(@"I should get card type as Visa")]
    public void ThenIShouldGetCardTypeAsVisa()
    {
        _apiResult.Data.Should().Be( CardType.Visa);
    }
}