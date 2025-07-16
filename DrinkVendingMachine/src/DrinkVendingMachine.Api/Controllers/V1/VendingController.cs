using DrinkVendingMachine.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DrinkVendingMachine.Api.Controllers.V1;

[ApiController]
[Route("api/v1/[controller]/lock")]
[Produces("application/json")]
public class VendingController(IVendingMachineLockService lockService) : ControllerBase
{
    [HttpGet("status")]
    public async Task<IActionResult> GetLockStatus()
    {
        var isLocked = await lockService.IsLockedAsync();
        return Ok(new { isLocked });
    }

    [HttpPost("acquire")]
    public async Task<IActionResult> AcquireLock([FromQuery] string clientId)
    {
        var acquired = await lockService.TryAcquireLockAsync(clientId);
        return Ok(new { acquired });
    }

    [HttpPost("heartbeat")]
    public async Task<IActionResult> Heartbeat([FromQuery] string clientId)
    {
        await lockService.RefreshLockAsync(clientId);
        return Ok();
    }

    [HttpPost("release")]
    public async Task<IActionResult> ReleaseLock([FromQuery] string clientId)
    {
        await lockService.ReleaseLockAsync(clientId);
        return Ok();
    }
}