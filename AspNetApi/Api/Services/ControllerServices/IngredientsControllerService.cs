using Api.Configurations;
using Api.Controllers;
using Api.DataTransferObjects;
using Api.Services.ControllerServices.Interfaces;
using Api.Services.Interfaces;
using Api.ViewModels.Ingredient;
using Api.ViewModels.Pagination;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Model.Context;
using Model.Entities;

namespace Api.Services.ControllerServices;

public class IngredientsControllerService(
	DataContext context,
	IMapper mapper,
	IImageService imageService,
	ICacheService cacheService,
	IPaginationService<IngredientVm, IngredientFilterVm> pagination,
	IOptions<CacheExpirySeconds> options
) : IIngredientsControllerService {
	private readonly int _cacheExpirySeconds = options.Value.IngredientsController;

	public async Task<IEnumerable<IngredientVm>> GetAllAsync() {
		var action = new ActionDto(
			ControllerName,
			nameof(IngredientsController.GetAll)
		);

		if (await cacheService.IsContainsCacheAsync(action))
			return await cacheService.GetCacheAsync<IEnumerable<IngredientVm>>(action);

		var entities = await context.Ingredients
			.ProjectTo<IngredientVm>(mapper.ConfigurationProvider)
			.ToArrayAsync();

		await cacheService.SetCacheAsync(action, entities, TimeSpan.FromSeconds(_cacheExpirySeconds));

		return entities;
	}

	public async Task<PageVm<IngredientVm>> GetPageAsync(IngredientFilterVm vm) {
		var action = new ActionDto(
			ControllerName,
			nameof(IngredientsController.GetPage)
		);

		if (await cacheService.IsContainsCacheAsync(action, vm))
			return await cacheService.GetCacheAsync<PageVm<IngredientVm>>(action, vm);

		var page = await pagination.GetPageAsync(vm);

		await cacheService.SetCacheAsync(action, vm, page, TimeSpan.FromSeconds(_cacheExpirySeconds));

		return page;
	}

	public async Task<IngredientVm?> TryGetByIdAsync(long id) {
		var action = new ActionDto(
			ControllerName,
			nameof(IngredientsController.GetById)
		);

		if (await cacheService.IsContainsCacheAsync(action, id))
			return await cacheService.GetCacheAsync<IngredientVm?>(action, id);

		var entity = await context.Ingredients
			.ProjectTo<IngredientVm>(mapper.ConfigurationProvider)
			.FirstOrDefaultAsync(x => x.Id == id);

		await cacheService.SetCacheAsync(action, id, entity, TimeSpan.FromSeconds(_cacheExpirySeconds));

		return entity;
	}

	public async Task CreateAsync(CreateIngredientVm vm) {
		var entity = mapper.Map<Ingredient>(vm);
		entity.Image = await imageService.SaveImageAsync(vm.Image);

		await context.Ingredients.AddAsync(entity);

		try {
			await context.SaveChangesAsync();
			await cacheService.DeleteCacheByControllerAsync(ControllerName);
		}
		catch (Exception) {
			imageService.DeleteImageIfExists(entity.Image);
			throw;
		}
	}

	public async Task UpdateAsync(UpdateIngredientVm vm) {
		var entity = await context.Ingredients.FirstAsync(x => x.Id == vm.Id);

		string oldImage = entity.Image;

		entity.Name = vm.Name;
		entity.Image = await imageService.SaveImageAsync(vm.Image);

		try {
			await context.SaveChangesAsync();
			await cacheService.DeleteCacheByControllerAsync(ControllerName);

			imageService.DeleteImageIfExists(oldImage);
		}
		catch (Exception) {
			imageService.DeleteImageIfExists(entity.Image);
			throw;
		}
	}

	public async Task DeleteIfExistsAsync(long id) {
		var entity = await context.Ingredients.FirstOrDefaultAsync(x => x.Id == id);

		if (entity is null)
			return;

		context.Ingredients.Remove(entity);
		await context.SaveChangesAsync();
		await cacheService.DeleteCacheByControllerAsync(ControllerName);

		imageService.DeleteImageIfExists(entity.Image);
	}

	private static string ControllerName => nameof(IngredientsController);
}
