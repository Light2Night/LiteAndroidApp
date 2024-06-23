using Api.Controllers;
using Api.DataTransferObjects;
using Api.Services.ControllerServices.Interfaces;
using Api.Services.Interfaces;
using Api.ViewModels.Category;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Model.Context;
using Model.Entities;

namespace Api.Services.ControllerServices;

public class CategoriesControllerService(
	DataContext context,
	IMapper mapper,
	IImageService imageService,
	ICacheService cacheService
) : ICategoriesControllerService {

	public async Task<IEnumerable<CategoryVm>> GetAllAsync() {
		var action = new ActionDto(
			nameof(CategoriesController),
			nameof(CategoriesController.GetAll)
		);

		if (await cacheService.IsContainsCacheAsync(action))
			return await cacheService.GetCacheAsync<IEnumerable<CategoryVm>>(action);

		var entities = await context.Categories
			.ProjectTo<CategoryVm>(mapper.ConfigurationProvider)
			.ToArrayAsync();

		await cacheService.SetCacheAsync(action, entities, TimeSpan.FromHours(1));

		return entities;
	}

	public async Task CreateAsync(CreateCategoryVm vm) {
		var entity = mapper.Map<Category>(vm);
		entity.Image = await imageService.SaveImageAsync(vm.Image);

		await context.Categories.AddAsync(entity);

		try {
			await context.SaveChangesAsync();
			await cacheService.DeleteCacheByControllerAsync(nameof(CategoriesController));
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
			await cacheService.DeleteCacheByControllerAsync(nameof(CategoriesController));

			imageService.DeleteImageIfExists(oldImage);
		}
		catch (Exception) {
			imageService.DeleteImageIfExists(entity.Image);
			throw;
		}
	}

	public async Task DeleteIfExistsAsync(long id) {
		var entity = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);

		if (entity is null)
			return;

		context.Categories.Remove(entity);
		await context.SaveChangesAsync();
		await cacheService.DeleteCacheByControllerAsync(nameof(CategoriesController));

		imageService.DeleteImageIfExists(entity.Image);
	}
}
