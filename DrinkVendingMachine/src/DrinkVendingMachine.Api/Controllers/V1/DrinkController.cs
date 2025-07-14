using DrinkVendingMachine.Application.DTOs.Drink;
using DrinkVendingMachine.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DrinkVendingMachine.Api.Controllers.V1;

[ApiController]
[Route("api/[controller]")]
public class DrinkController(IDrinkService drinkService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<DrinkModel>>> GetAll([FromQuery] DrinkFilterModel filter,
        CancellationToken cancellationToken)
    {
        var drinks = await drinkService.GetAllAsync(filter, cancellationToken);
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
        var result = await drinkService.AddAsync(model, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] DrinkUpdateModel model,
        CancellationToken cancellationToken)
    {
        model = model with { Id = id };
        var result = await drinkService.UpdateAsync(model, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await drinkService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpPost("import")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> ImportFromExcel(IFormFile file, CancellationToken cancellationToken)
    {
        await using var stream = file.OpenReadStream();
        await drinkService.ImportFromExcelAsync(stream, cancellationToken);

        return Ok();
    }
}