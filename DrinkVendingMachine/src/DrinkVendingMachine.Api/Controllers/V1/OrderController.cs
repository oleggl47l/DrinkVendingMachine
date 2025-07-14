using DrinkVendingMachine.Application.DTOs.Order;
using DrinkVendingMachine.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DrinkVendingMachine.Api.Controllers.V1;

[ApiController]
[Route("api/[controller]")]
public class OrderController(IOrderService orderService) : Controller
{
    [HttpPost("orders")]
    public async Task<ActionResult<OrderResultDto>> CreateOrder([FromBody] OrderCreateModel model,
        CancellationToken cancellationToken)
    {
        var result = await orderService.CreateOrderAsync(model, cancellationToken);
        return Ok(result);
    }
}