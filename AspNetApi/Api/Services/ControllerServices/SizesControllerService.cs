using Api.Configurations;
using Api.Controllers;
using Api.DataTransferObjects;
using Api.Services.ControllerServices.Interfaces;
using Api.Services.Interfaces;
using Api.ViewModels.Pagination;
using Api.ViewModels.Size;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Model.Context;
using Model.Entities;

namespace Api.Services.ControllerServices;

public class SizesControllerService(
	DataContext context,
	IMapper mapper,
	ICacheService cacheService,
	IPaginationService<SizeVm, SizeFilterVm> pagination,
	IOptions<CacheExpirySeconds> options
) : ISizesControllerService {
	private readonly int _cacheExpirySeconds = options.Value.SizesController;

	public async Task<IEnumerable<SizeVm>> GetAllAsync() {
		var action = new ActionDto(
			ControllerName,
			nameof(SizesController.GetAll)
		);

		if (await cacheService.IsContainsCacheAsync(action))
			return await cacheService.GetCacheAsync<IEnumerable<SizeVm>>(action);

		var entities = await context.Sizes
			.ProjectTo<SizeVm>(mapper.ConfigurationProvider)
			.ToArrayAsync();

		await cacheService.SetCacheAsync(action, entities, TimeSpan.FromSeconds(_cacheExpirySeconds));

		return entities;
	}

	public async Task<PageVm<SizeVm>> GetPageAsync(SizeFilterVm vm) {
		var action = new ActionDto(
			ControllerName,
			nameof(SizesController.GetPage)
		);

		if (await cacheService.IsContainsCacheAsync(action, vm))
			return await cacheService.GetCacheAsync<PageVm<SizeVm>>(action, vm);

		var page = await pagination.GetPageAsync(vm);

		await cacheService.SetCacheAsync(action, vm, page, TimeSpan.FromSeconds(_cacheExpirySeconds));

		return page;
	}

	public async Task<SizeVm?> TryGetByIdAsync(long id) {
		var action = new ActionDto(
			ControllerName,
			nameof(SizesController.GetById)
		);

		if (await cacheService.IsContainsCacheAsync(action, id))
			return await cacheService.GetCacheAsync<SizeVm?>(action, id);

		var entity = await context.Sizes
			.ProjectTo<SizeVm>(mapper.ConfigurationProvider)
			.FirstOrDefaultAsync(c => c.Id == id);

		await cacheService.SetCacheAsync(action, id, entity, TimeSpan.FromSeconds(_cacheExpirySeconds));

		return entity;
	}

	public async Task CreateAsync(CreateSizeVm vm) {
		var entity = mapper.Map<Size>(vm);

		await context.Sizes.AddAsync(entity);

		try {
			await context.SaveChangesAsync();
			await cacheService.DeleteCacheByControllerAsync(ControllerName);
		}
		catch (Exception) {
			throw;
		}
	}

	public async Task UpdateAsync(UpdateSizeVm vm) {
		var entity = await context.Sizes.FirstAsync(s => s.Id == vm.Id);

		entity.Name = vm.Name;

		await context.SaveChangesAsync();
		await cacheService.DeleteCacheByControllerAsync(ControllerName);
		await cacheService.DeleteCacheByControllerAsync(nameof(PizzasController));
	}

	public async Task DeleteIfExistsAsync(long id) {
		var entity = await context.Sizes
			.FirstOrDefaultAsync(s => s.Id == id);

		if (entity is null)
			return;

		context.Sizes.Remove(entity);
		await context.SaveChangesAsync();

		await cacheService.DeleteCacheByControllerAsync(ControllerName);
	}

	private static string ControllerName => nameof(SizesController);
}
