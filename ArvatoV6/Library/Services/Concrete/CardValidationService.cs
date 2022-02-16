using System.Text.RegularExpressions;
using ArvatoV6.Library.Factories.Abstract;
using ArvatoV6.Library.Services.Abstract;
using ArvatoV6.Models.Concrete;
using ArvatoV6.Models.Dto;
using FluentValidation;

namespace ArvatoV6.Library.Services.Concrete;

public class CardValidationService : ICardValidationService
{
    private readonly ILogger<CardValidationService> _logger;
    private readonly IValidator<CreditCardInputDto> _validator;
    public CardValidationService(ILogger<CardValidationService> logger, IValidator<CreditCardInputDto> validator)
    {
        _logger = logger;
        _validator = validator;
    }

    public ApiResult Validate(CreditCardInputDto creditCardInput)
    {
        ApiResult result = new() { IsSuccess = false, Message = new List<string>()};

        var validationResult = _validator.Validate(creditCardInput);

        if (validationResult.Errors.Any() is true)
        {
            _logger.LogError(String.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage)));
            result.Message = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
            return result;
        }

        var cardType = GetCardType(creditCardInput.CardNumber);

        if (cardType is CardType.Invalid)
        {
            _logger.LogError("Invalid card type");
            result.Message.Add("Invalid card type");
            return result;
        }
        var cvvCheck = CheckCvv(cardType, creditCardInput.CVV);
        if (cvvCheck is false)
        {
            _logger.LogError("Invalid CVV");
            result.Message.Add("Invalid CVV");
            return result;
        }

        var creditCard = CardFactory.CreateCard(cardType);

        result.IsSuccess = true;
        result.Data = creditCard.GetCardType();

        return result;
    }

    private bool CheckCvv(string cardType, string cvv)
    {
        // check cvv for card type
        switch (cardType)
        {
            case CardType.Visa:
            case CardType.MasterCard:
                return cvv.Length is 3;

            case CardType.AmericanExpress:
                return cvv.Length is 4;
            default:
                return false;
        }
    }

    private string GetCardType(string cardNumber)
    {
        // get card type from card number using regex
        switch (cardNumber)
        {
            case var _ when new Regex(@"^4[0-9]{12}(?:[0-9]{3})?$").IsMatch(cardNumber):
                return CardType.Visa;
            case var _ when new Regex(@"^(?:5[1-5][0-9]{2}|222[1-9]|22[3-9][0-9]|2[3-6][0-9]{2}|27[01][0-9]|2720)[0-9]{12}$").IsMatch(cardNumber):
                return CardType.MasterCard;
            case var _ when new Regex(@"^3[47][0-9]{13}$").IsMatch(cardNumber):
                return CardType.AmericanExpress;
            default:
                return CardType.Invalid;
        }
    }
}
