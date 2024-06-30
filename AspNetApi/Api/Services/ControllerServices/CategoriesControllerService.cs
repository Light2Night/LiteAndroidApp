using Api.Configurations;
using Api.Controllers;
using Api.DataTransferObjects;
using Api.Services.ControllerServices.Interfaces;
using Api.Services.Interfaces;
using Api.ViewModels.Category;
using Api.ViewModels.Pagination;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Model.Context;
using Model.Entities;

namespace Api.Services.ControllerServices;

public class CategoriesControllerService(
	DataContext context,
	IMapper mapper,
	IImageService imageService,
	ICacheService cacheService,
	IPaginationService<CategoryVm, CategoryFilterVm> pagination,
	IOptions<CacheExpirySeconds> options
) : ICategoriesControllerService {
	private readonly int _cacheExpirySeconds = options.Value.CategoriesController;

	public async Task<IEnumerable<CategoryVm>> GetAllAsync() {
		var action = new ActionDto(
			ControllerName,
			nameof(CategoriesController.GetAll)
		);

		if (await cacheService.IsContainsCacheAsync(action))
			return await cacheService.GetCacheAsync<IEnumerable<CategoryVm>>(action);

		var entities = await context.Categories
			.ProjectTo<CategoryVm>(mapper.ConfigurationProvider)
			.ToArrayAsync();

		await cacheService.SetCacheAsync(action, entities, TimeSpan.FromSeconds(_cacheExpirySeconds));

		return entities;
	}

	public async Task<PageVm<CategoryVm>> GetPageAsync(CategoryFilterVm vm) {
		var action = new ActionDto(
			ControllerName,
			nameof(CategoriesController.GetPage)
		);

		if (await cacheService.IsContainsCacheAsync(action, vm))
			return await cacheService.GetCacheAsync<PageVm<CategoryVm>>(action, vm);

		var page = await pagination.GetPageAsync(vm);

		await cacheService.SetCacheAsync(action, vm, page, TimeSpan.FromSeconds(_cacheExpirySeconds));

		return page;
	}

	public async Task<CategoryVm?> TryGetByIdAsync(long id) {
		var action = new ActionDto(
			ControllerName,
			nameof(CategoriesController.GetById)
		);

		if (await cacheService.IsContainsCacheAsync(action, id))
			return await cacheService.GetCacheAsync<CategoryVm?>(action, id);

		var entity = await context.Categories
			.ProjectTo<CategoryVm>(mapper.ConfigurationProvider)
			.FirstOrDefaultAsync(c => c.Id == id);

		await cacheService.SetCacheAsync(action, id, entity, TimeSpan.FromSeconds(_cacheExpirySeconds));

		return entity;
	}

	public async Task CreateAsync(CreateCategoryVm vm) {
		var entity = mapper.Map<Category>(vm);
		entity.Image = await imageService.SaveImageAsync(vm.Image);

		await context.Categories.AddAsync(entity);

		try {
			await context.SaveChangesAsync();
			await cacheService.DeleteCacheByControllerAsync(ControllerName);
		}
		catch (Exception) {
			imageService.DeleteImageIfExists(entity.Image);
			throw;
		}
	}

	public async Task UpdateAsync(UpdateCategoryVm vm) {
		var entity = await context.Categories.FirstAsync(c => c.Id == vm.Id);

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
		var entity = await context.Categories
			.FirstOrDefaultAsync(c => c.Id == id);

		if (entity is null)
			return;

		var imagesForDelete = await context.Pizzas
			.Include(p => p.Images)
			.Where(p => p.CategoryId == id)
			.SelectMany(p => p.Images)
			.Select(pi => pi.Name)
			.ToArrayAsync();

		context.Categories.Remove(entity);
		await context.SaveChangesAsync();

		await cacheService.DeleteCacheByControllerAsync(ControllerName);
		await cacheService.DeleteCacheByControllerAsync(nameof(PizzasController));

		imageService.DeleteImagesIfExists(imagesForDelete.Append(entity.Image));
	}

	private static string ControllerName => nameof(CategoriesController);
}
