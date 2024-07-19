using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class CachesController(
	ICacheService cache
) : ControllerBase {

	[HttpPost]
	public async Task<IActionResult> Clear() {
		await cache.ClearCacheAsync();
		return Ok();
	}
}
