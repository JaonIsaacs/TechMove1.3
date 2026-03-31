using Microsoft.AspNetCore.Mvc;
using TechMove1._3.Domain.Interfaces;

[Route("api/[controller]")]
[ApiController]
public class CurrencyController : ControllerBase
{
    private readonly ICurrencyStrategy _currency;

    public CurrencyController(ICurrencyStrategy currency)
    {
        _currency = currency;
    }

    [HttpGet]
    public async Task<string> Get(decimal usd)
    {
        var zar = await _currency.ConvertToZAR(usd);
        return zar.ToString("0.00");
    }
}