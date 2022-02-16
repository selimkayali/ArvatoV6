using System.Text.RegularExpressions;
using ArvatoV6.Models;
using ArvatoV6.Services.Abstract;
using FluentValidation;

namespace ArvatoV6.Services.Concrete;

public class CardValidationService : ICardValidationService
{
    private readonly IValidator<CreditCardInputDto> _validator;
    public CardValidationService(IValidator<CreditCardInputDto> validator)
    {
        _validator = validator;
    }

    public ApiResult Validate(CreditCardInputDto creditCardInput)
    {
        ApiResult result = new() { IsSuccess = false };

        var res = _validator.Validate(creditCardInput);

        if (res.Errors.Any() is true)
        {
            result.Message = res.Errors.First().ErrorMessage;
            return result;
        }

        var cardType = GetCardType(creditCardInput.CardNumber);

        if (cardType is CardType.Invalid)
        {
            result.Message = "Invalid card type";
            return result;
        }
        var cvvCheck = CheckCvv(cardType, creditCardInput.CVV);
        if (cvvCheck is false)
        {
            result.Message = "Invalid CVV";
            return result;
        }

        return new ApiResult
        {
            IsSuccess = true,
            Data = creditCardInput
        };
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
