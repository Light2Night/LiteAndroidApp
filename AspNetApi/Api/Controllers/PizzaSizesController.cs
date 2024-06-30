using Api.Services.ControllerServices.Interfaces;
using Api.ViewModels.PizzaSize;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class PizzaSizesController(
	IValidator<CreatePizzaSizeVm> createValidator,
	IValidator<UpdatePizzaSizeVm> updateValidator,
	IPizzaSizesControllerService service
) : ControllerBase {

	[HttpPost]
	public async Task<IActionResult> Create([FromForm] CreatePizzaSizeVm vm) {
		var validationResult = await createValidator.ValidateAsync(vm);

		if (!validationResult.IsValid)
			return BadRequest(validationResult.Errors);

		await service.CreateAsync(vm);

		return Ok();
	}

	[HttpPut]
	public async Task<IActionResult> Update([FromForm] UpdatePizzaSizeVm vm) {
		var validationResult = await updateValidator.ValidateAsync(vm);

		if (!validationResult.IsValid)
			return BadRequest(validationResult.Errors);

		await service.UpdateAsync(vm);

		return Ok();
	}

	[HttpDelete("{pizzaId}/{sizeId}")]
	public async Task<IActionResult> Delete(long pizzaId, long sizeId) {
		await service.DeleteIfExistsAsync(pizzaId, sizeId);

		return Ok();
	}
}
