using Microsoft.AspNetCore.Mvc;

namespace DiscountService.Controllers;

[ApiController]
[Route("api/discounts")]
public class DiscountController : ControllerBase
{
    [HttpGet("{productId:int}")]
    public IActionResult GetDiscount(int productId)
    {
        return Ok(new { percentage = 10 });
    }
}