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

    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public IActionResult Post(CreditCardInputDto creditCardInput)
    {
        _logger.LogInformation($"Post action called - {nameof(Post)}");

        var result = _cardValidationService.Validate(creditCardInput);
        if (result.IsSuccess is false)
        {
            _logger.LogError(String.Join(", ", result.Message));
            return BadRequest(result.Message);
        }

        return Ok(result.Data);
    }
}

