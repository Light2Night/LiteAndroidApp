﻿using Api.Services.ControllerServices.Interfaces;
using Api.ViewModels.SpecificationName;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class SpecificationNamesController(
	IValidator<CreateSpecificationNameVm> createValidator,
	IValidator<UpdateSpecificationNameVm> updateValidator,
	ISpecificationNamesControllerService service
) : ControllerBase {

	[HttpGet]
	public async Task<IActionResult> GetAll() {
		return Ok(await service.GetAllAsync());
	}

	[HttpGet]
	public async Task<IActionResult> GetPage([FromQuery] SpecificationNameFilterVm vm) {
		try {
			return Ok(await service.GetPageAsync(vm));
		}
		catch (Exception ex) {
			return BadRequest(ex.Message);
		}
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById(long id) {
		var entity = await service.TryGetByIdAsync(id);

		if (entity is null)
			return NotFound();

		return Ok(entity);
	}

	[HttpPost]
	public async Task<IActionResult> Create([FromForm] CreateSpecificationNameVm vm) {
		var validationResult = await createValidator.ValidateAsync(vm);

		if (!validationResult.IsValid)
			return BadRequest(validationResult.Errors);

		await service.CreateAsync(vm);

		return Ok();
	}

	[HttpPut]
	public async Task<IActionResult> Update([FromForm] UpdateSpecificationNameVm vm) {
		var validationResult = await updateValidator.ValidateAsync(vm);

		if (!validationResult.IsValid)
			return BadRequest(validationResult.Errors);

		await service.UpdateAsync(vm);

		return Ok();
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(long id) {
		await service.DeleteIfExistsAsync(id);

		return Ok();
	}
}
