using DrinkVendingMachine.Application.DTOs.Drink;
using DrinkVendingMachine.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DrinkVendingMachine.Api.Controllers.V1;

[ApiController]
[Route("api/[controller]")]
public class DrinkController(IDrinkService drinkService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<DrinkModel>>> GetAll(CancellationToken cancellationToken)
    {
        var drinks = await drinkService.GetAllAsync(cancellationToken);
        return Ok(drinks);
    }

    [HttpGet("filter")]
    public async Task<ActionResult<List<DrinkModel>>> GetFiltered(
        [FromQuery] DrinkFilterModel filter,
        CancellationToken cancellationToken)
    {
        var drinks = await drinkService.GetFilteredAsync(filter, cancellationToken);
        return Ok(drinks);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<DrinkModel>> GetById(int id, CancellationToken cancellationToken)
    {
        var drink = await drinkService.GetByIdAsync(id, cancellationToken);
        return drink is null ? NotFound() : Ok(drink);
    }

    [HttpGet("by-brand/{brandId:int}")]
    public async Task<ActionResult<List<DrinkModel>>> GetByBrand(int brandId, CancellationToken cancellationToken)
    {
        var drinks = await drinkService.GetByBrandAsync(brandId, cancellationToken);
        return Ok(drinks);
    }

    [HttpPut("{id:int}/quantity")]
    public async Task<IActionResult> UpdateQuantity(int id, [FromQuery] int quantity,
        CancellationToken cancellationToken)
    {
        await drinkService.UpdateQuantityAsync(id, quantity, cancellationToken);
        return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] DrinkCreateModel model, CancellationToken cancellationToken)
    {
        await drinkService.AddAsync(model, cancellationToken);
        return NoContent();
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] DrinkUpdateModel model,
        CancellationToken cancellationToken)
    {
        model = model with { Id = id };
        await drinkService.UpdateAsync(model, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await drinkService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}