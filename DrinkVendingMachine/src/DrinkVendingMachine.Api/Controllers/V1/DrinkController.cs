using DrinkVendingMachine.Application.DTOs.Drink;
using DrinkVendingMachine.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DrinkVendingMachine.Api.Controllers.V1;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class DrinkController(IDrinkService drinkService) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(OperationId = "GetAllDrinks")]
    public async Task<ActionResult<List<DrinkModel>>> GetAll([FromQuery] DrinkFilterModel filter,
        CancellationToken cancellationToken)
    {
        var drinks = await drinkService.GetAllAsync(filter, cancellationToken);
        return Ok(drinks);
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation(OperationId = "GetDrinkById")]
    public async Task<ActionResult<DrinkModel>> GetById(int id, CancellationToken cancellationToken)
    {
        var drink = await drinkService.GetByIdAsync(id, cancellationToken);
        return drink is null ? NotFound() : Ok(drink);
    }

    [HttpGet("by-brand/{brandId:int}")]
    [SwaggerOperation(OperationId = "GetDrinksByBrand")]
    public async Task<ActionResult<List<DrinkModel>>> GetByBrand(int brandId, CancellationToken cancellationToken)
    {
        var drinks = await drinkService.GetByBrandAsync(brandId, cancellationToken);
        return Ok(drinks);
    }

    [HttpGet("price-range")]
    [SwaggerOperation(OperationId = "GetDrinksPriceRange")]
    public async Task<ActionResult<PriceRangeModel>> GetPriceRange([FromQuery] int? brandId,
        CancellationToken cancellationToken) =>
        Ok(await drinkService.GetPriceRangeAsync(brandId, cancellationToken));

    [HttpPut("{id:int}/quantity")]
    [SwaggerOperation(OperationId = "UpdateDrinkQuantity")]
    public async Task<IActionResult> UpdateQuantity(int id, [FromQuery] int quantity,
        CancellationToken cancellationToken)
    {
        await drinkService.UpdateQuantityAsync(id, quantity, cancellationToken);
        return NoContent();
    }

    [HttpPost]
    [SwaggerOperation(OperationId = "CreateDrink")]
    public async Task<IActionResult> Create([FromBody] DrinkCreateModel model, CancellationToken cancellationToken)
    {
        var result = await drinkService.AddAsync(model, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPatch("{id:int}")]
    [SwaggerOperation(OperationId = "UpdateDrink")]
    public async Task<IActionResult> Update(int id, [FromBody] DrinkUpdateModel model,
        CancellationToken cancellationToken)
    {
        model = model with { Id = id };
        var result = await drinkService.UpdateAsync(model, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation(OperationId = "DeleteDrink")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await drinkService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpPost("import")]
    [Consumes("multipart/form-data")]
    [SwaggerOperation(OperationId = "ImportDrinksFromExcel")]
    public async Task<IActionResult> ImportFromExcel(IFormFile file, CancellationToken cancellationToken)
    {
        await using var stream = file.OpenReadStream();
        await drinkService.ImportFromExcelAsync(stream, cancellationToken);

        return Ok();
    }
}