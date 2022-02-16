using ArvatoV6.Library.Services.Abstract;
using ArvatoV6.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace ArvatoV6.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CardValidationController : ControllerBase
{
    private readonly ILogger<CardValidationController> _logger;
    private readonly ICardValidationService _cardValidationService;

    public CardValidationController(ILogger<CardValidationController> logger, ICardValidationService cardValidationService)
    {
        _logger = logger;
        _cardValidationService = cardValidationService;
    }

    [HttpPost()]
    public IActionResult Post(CreditCardInputDto creditCardInput)
    {
        var result = _cardValidationService.Validate(creditCardInput);
        if (result.IsSuccess is false)
        {
            return BadRequest(result.Message);
        }

        return Ok(result.Data);
    }
}

