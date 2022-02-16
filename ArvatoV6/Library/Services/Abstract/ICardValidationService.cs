using ArvatoV6.Models.Concrete;
using ArvatoV6.Models.Dto;

namespace ArvatoV6.Library.Services.Abstract;

public interface ICardValidationService
{
    ApiResult Validate(CreditCardInputDto creditCardInput);
}