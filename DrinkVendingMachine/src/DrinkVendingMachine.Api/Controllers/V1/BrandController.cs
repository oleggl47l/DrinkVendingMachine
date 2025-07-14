using DrinkVendingMachine.Application.DTOs.Brand;
using DrinkVendingMachine.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DrinkVendingMachine.Api.Controllers.V1;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class BrandController(IBrandService brandService) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(OperationId = "GetAllBrands")]
    public async Task<ActionResult<List<BrandModel>>> GetAll(CancellationToken cancellationToken)
    {
        var brands = await brandService.GetAllAsync(cancellationToken);
        return Ok(brands);
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation(OperationId = "GetBrandById")]
    public async Task<ActionResult<BrandModel>> GetById(int id, CancellationToken cancellationToken)
    {
        var brand = await brandService.GetByIdAsync(id, cancellationToken);
        return brand is null ? NotFound() : Ok(brand);
    }

    [HttpPost]
    [SwaggerOperation(OperationId = "CreateBrand")]
    public async Task<ActionResult<BrandModel>> Create([FromBody] BrandCreateModel model,
        CancellationToken cancellationToken)
    {
        var result = await brandService.AddAsync(model, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    [SwaggerOperation(OperationId = "UpdateBrand")]
    public async Task<ActionResult<BrandModel>> Update(int id, [FromBody] BrandUpdateModel model,
        CancellationToken cancellationToken)
    {
        model = model with { Id = id };
        var result = await brandService.UpdateAsync(model, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation(OperationId = "DeleteBrand")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await brandService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpGet("is-name-unique")]
    [SwaggerOperation(OperationId = "CheckBrandNameUnique")]
    public async Task<ActionResult<bool>> IsNameUnique([FromQuery] string name, CancellationToken cancellationToken)
    {
        var isUnique = await brandService.IsNameUniqueAsync(name, cancellationToken);
        return Ok(isUnique);
    }
}