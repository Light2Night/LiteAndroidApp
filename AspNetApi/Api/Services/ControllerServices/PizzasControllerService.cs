using Api.Controllers;
using Api.DataTransferObjects;
using Api.Services.ControllerServices.Interfaces;
using Api.Services.Interfaces;
using Api.ViewModels.Pagination;
using Api.ViewModels.Pizza;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Model.Context;
using Model.Entities;

namespace Api.Services.ControllerServices;

public class PizzasControllerService(
	DataContext context,
	IMapper mapper,
	IImageService imageService,
	ICacheService cacheService,
	IPaginationService<PizzaVm, PizzaFilterVm> pagination
) : IPizzasControllerService {

	public async Task<IEnumerable<PizzaVm>> GetAllAsync() {
		var action = new ActionDto(
			ControllerName,
			nameof(PizzasController.GetAll)
		);

		if (await cacheService.IsContainsCacheAsync(action))
			return await cacheService.GetCacheAsync<IEnumerable<PizzaVm>>(action);

		var entities = await context.Pizzas
			.ProjectTo<PizzaVm>(mapper.ConfigurationProvider)
			.ToArrayAsync();

		await cacheService.SetCacheAsync(action, entities, TimeSpan.FromHours(1));

		return entities;
	}

	public async Task<PageVm<PizzaVm>> GetPageAsync(PizzaFilterVm vm) {
		var action = new ActionDto(
			ControllerName,
			nameof(PizzasController.GetPage)
		);

		if (await cacheService.IsContainsCacheAsync(action, vm))
			return await cacheService.GetCacheAsync<PageVm<PizzaVm>>(action, vm);

		var page = await pagination.GetPageAsync(vm);

		await cacheService.SetCacheAsync(action, vm, page, TimeSpan.FromHours(1));

		return page;
	}

	public async Task<PizzaVm?> TryGetByIdAsync(long id) {
		var action = new ActionDto(
			ControllerName,
			nameof(PizzasController.GetById)
		);

		if (await cacheService.IsContainsCacheAsync(action, id))
			return await cacheService.GetCacheAsync<PizzaVm?>(action, id);

		var entity = await context.Pizzas
			.ProjectTo<PizzaVm>(mapper.ConfigurationProvider)
			.FirstOrDefaultAsync(x => x.Id == id);

		await cacheService.SetCacheAsync(action, id, entity, TimeSpan.FromHours(1));

		return entity;
	}

	public async Task CreateAsync(CreatePizzaVm vm) {
		var entity = mapper.Map<Pizza>(vm);
		entity.Images = await SaveAndPrioritizePhotosAsync(vm.Images, entity);

		await context.Pizzas.AddAsync(entity);

		try {
			await context.SaveChangesAsync();
			await cacheService.DeleteCacheByControllerAsync(ControllerName);
		}
		catch (Exception) {
			imageService.DeleteImagesIfExists(entity.Images.Select(i => i.Name));
			throw;
		}
	}

	public async Task UpdateAsync(UpdatePizzaVm vm) {
		var entity = await context.Pizzas
			.Include(p => p.Images)
			.Include(p => p.Ingredients)
			.FirstAsync(x => x.Id == vm.Id);

		var oldImages = entity.Images
			.Select(pi => pi.Name)
			.ToArray();

		entity.Name = vm.Name;
		entity.Description = vm.Description;
		entity.Rating = vm.Rating;
		entity.IsAvailable = vm.IsAvailable;
		entity.CategoryId = vm.CategoryId;

		entity.Images.Clear();
		foreach (var image in await SaveAndPrioritizePhotosAsync(vm.Images, entity))
			entity.Images.Add(image);

		entity.Ingredients.Clear();
		if (vm.IngredientIds is not null)
			foreach (var ingredientId in vm.IngredientIds) {
				entity.Ingredients.Add(new PizzaIngredient {
					PizzaId = entity.Id,
					IngredientId = ingredientId
				});
			}

		try {
			await context.SaveChangesAsync();
			await cacheService.DeleteCacheByControllerAsync(ControllerName);

			imageService.DeleteImagesIfExists(oldImages);
		}
		catch (Exception) {
			imageService.DeleteImagesIfExists(entity.Images.Select(i => i.Name));
			throw;
		}
	}

	public async Task DeleteIfExistsAsync(long id) {
		var entity = await context.Pizzas
			.Include(p => p.Images)
			.FirstOrDefaultAsync(x => x.Id == id);

		if (entity is null)
			return;

		context.Pizzas.Remove(entity);
		await context.SaveChangesAsync();
		await cacheService.DeleteCacheByControllerAsync(ControllerName);

		imageService.DeleteImagesIfExists(entity.Images.Select(pi => pi.Name).ToArray());
	}

	private static string ControllerName => nameof(PizzasController);

	private async Task<PizzaImage[]> SaveAndPrioritizePhotosAsync(IEnumerable<IFormFile> imageFiles, Pizza pizza) {
		var images = await imageService.SaveImagesAsync(imageFiles);

		int index = 0;
		return images
			.Select(i => new PizzaImage {
				Pizza = pizza,
				Name = i,
				Priority = index++
			})
			.ToArray();
	}
}
