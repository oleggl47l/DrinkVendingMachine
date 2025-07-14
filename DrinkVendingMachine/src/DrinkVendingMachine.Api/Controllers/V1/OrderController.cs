using DrinkVendingMachine.Application.DTOs.Order;
using DrinkVendingMachine.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DrinkVendingMachine.Api.Controllers.V1;

[ApiController]
[Route("api/[controller]")]
public class OrderController(IOrderService orderService) : Controller
{
    [HttpGet]
    public async Task<ActionResult<List<OrderModel>>> GetAllOrders(CancellationToken cancellationToken)
    {
        var orders = await orderService.GetAllOrdersAsync(cancellationToken);
        return Ok(orders);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderModel>> GetOrderById(int id, CancellationToken cancellationToken)
    {
        var order = await orderService.GetOrderByIdAsync(id, cancellationToken);
        return Ok(order);
    }

    [HttpPost("orders")]
    public async Task<ActionResult<OrderResultDto>> CreateOrder([FromBody] OrderCreateModel model,
        CancellationToken cancellationToken)
    {
        var result = await orderService.CreateOrderAsync(model, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteOrder(int id, CancellationToken cancellationToken)
    {
        await orderService.DeleteOrderAsync(id, cancellationToken);
        return NoContent();
    }
}