using DrinkVendingMachine.Application.DTOs.Brand;
using DrinkVendingMachine.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DrinkVendingMachine.Api.Controllers.V1;

[ApiController]
[Route("api/[controller]")]
public class BrandController(IBrandService brandService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<BrandModel>>> GetAll(CancellationToken cancellationToken)
    {
        var brands = await brandService.GetAllAsync(cancellationToken);
        return Ok(brands);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BrandModel>> GetById(int id, CancellationToken cancellationToken)
    {
        var brand = await brandService.GetByIdAsync(id, cancellationToken);
        return brand is null ? NotFound() : Ok(brand);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] BrandCreateModel model, CancellationToken cancellationToken)
    {
        await brandService.AddAsync(model, cancellationToken);
        return NoContent();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] BrandUpdateModel model,
        CancellationToken cancellationToken)
    {
        model = model with { Id = id };
        await brandService.UpdateAsync(model, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await brandService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpGet("is-name-unique")]
    public async Task<ActionResult<bool>> IsNameUnique([FromQuery] string name, CancellationToken cancellationToken)
    {
        var isUnique = await brandService.IsNameUniqueAsync(name, cancellationToken);
        return Ok(isUnique);
    }
}