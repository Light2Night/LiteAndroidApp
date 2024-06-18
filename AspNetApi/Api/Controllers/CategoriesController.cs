using Api.Services.ControllerServices.Interfaces;
using Api.Services.Interfaces;
using Api.ViewModels.Category;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.Context;

namespace Api.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class CategoriesController(
	DataContext context,
	IMapper mapper,
	IValidator<CreateCategoryVm> createValidator,
	IValidator<UpdateCategoryVm> updateValidator,
	ICategoriesControllerService service,
	IPaginationService<CategoryVm, CategoryFilterVm> pagination
) : ControllerBase {

	[HttpGet]
	public async Task<IActionResult> GetAll() {
		var entities = await context.Categories
			.ProjectTo<CategoryVm>(mapper.ConfigurationProvider)
			.ToArrayAsync();

		return Ok(entities);
	}

	[HttpGet]
	public async Task<IActionResult> GetPage([FromQuery] CategoryFilterVm vm) {
		try {
			return Ok(await pagination.GetPageAsync(vm));
		}
		catch (Exception ex) {
			return BadRequest(ex.Message);
		}
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById(long id) {
		var entities = await context.Categories
			.ProjectTo<CategoryVm>(mapper.ConfigurationProvider)
			.FirstOrDefaultAsync(c => c.Id == id);

		if (entities is null)
			return NotFound();

		return Ok(entities);
	}

	[HttpPost]
	public async Task<IActionResult> Create([FromForm] CreateCategoryVm vm) {
		var validationResult = await createValidator.ValidateAsync(vm);

		if (!validationResult.IsValid)
			return BadRequest(validationResult.Errors);

		await service.CreateAsync(vm);

		return Ok();
	}

	[HttpPut]
	public async Task<IActionResult> Update([FromForm] UpdateCategoryVm vm) {
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
