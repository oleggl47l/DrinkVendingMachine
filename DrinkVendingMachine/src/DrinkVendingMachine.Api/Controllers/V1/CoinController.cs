using DrinkVendingMachine.Application.DTOs.Coin;
using DrinkVendingMachine.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DrinkVendingMachine.Api.Controllers.V1;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class CoinController(ICoinService coinService) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(OperationId = "GetAllCoins")]
    public async Task<ActionResult<List<CoinModel>>> GetAll(CancellationToken cancellationToken)
    {
        var coins = await coinService.GetAllAsync(cancellationToken);
        return Ok(coins);
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation(OperationId = "GetCoinById")]
    public async Task<ActionResult<CoinModel>> GetById(int id, CancellationToken cancellationToken)
    {
        var coin = await coinService.GetByIdAsync(id, cancellationToken);
        return coin == null ? NotFound() : Ok(coin);
    }

    [HttpPost]
    [SwaggerOperation(OperationId = "CreateCoin")]
    public async Task<ActionResult<CoinModel>> Create([FromBody] CoinCreateModel model,
        CancellationToken cancellationToken)
    {
        var created = await coinService.AddAsync(model, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    [SwaggerOperation(OperationId = "UpdateCoin")]
    public async Task<ActionResult<CoinModel>> Update(int id, [FromBody] CoinUpdateModel model,
        CancellationToken cancellationToken)
    {
        model = model with { Id = id };
        var updated = await coinService.UpdateAsync(model, cancellationToken);
        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation(OperationId = "DeleteCoin")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await coinService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}