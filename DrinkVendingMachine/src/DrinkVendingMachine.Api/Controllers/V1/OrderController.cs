using DrinkVendingMachine.Application.DTOs.Order;
using DrinkVendingMachine.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DrinkVendingMachine.Api.Controllers.V1;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class OrderController(IOrderService orderService) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(OperationId = "GetAllOrders")]
    public async Task<ActionResult<List<OrderModel>>> GetAll(CancellationToken cancellationToken)
    {
        var orders = await orderService.GetAllOrdersAsync(cancellationToken);
        return Ok(orders);
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation(OperationId = "GetOrderById")]
    public async Task<ActionResult<OrderModel>> GetById(int id, CancellationToken cancellationToken)
    {
        var order = await orderService.GetOrderByIdAsync(id, cancellationToken);
        return Ok(order);
    }

    [HttpPost("orders")]
    [SwaggerOperation(OperationId = "CreateOrder")]
    public async Task<ActionResult<OrderResultDto>> Create([FromBody] OrderCreateModel model,
        CancellationToken cancellationToken)
    {
        var result = await orderService.CreateOrderAsync(model, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation(OperationId = "DeleteOrder")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await orderService.DeleteOrderAsync(id, cancellationToken);
        return NoContent();
    }
}