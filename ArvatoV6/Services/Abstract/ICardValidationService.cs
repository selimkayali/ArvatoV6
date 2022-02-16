using ArvatoV6.Models;

namespace ArvatoV6.Services.Abstract;

public interface ICardValidationService
{
    ApiResult Validate(CreditCardInputDto creditCardInput);
}